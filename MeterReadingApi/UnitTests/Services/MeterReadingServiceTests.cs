using MeterReadingApi.Data.Interfaces;
using MeterReadingApi.Services;
using Moq;
using Xunit;
using System.Text;
using Shouldly;
using MeterReadingApi.Models;

namespace MeterReadingApi.Tests
{
    public class MeterReadingServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMeterReadingsRepository> _mockMeterReadingsRepository;
        private readonly MeterReadingService _service;

        public MeterReadingServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMeterReadingsRepository = new Mock<IMeterReadingsRepository>();
            _service = new MeterReadingService(_mockAccountRepository.Object, _mockMeterReadingsRepository.Object);
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldReturnCriticalError_WhenFileIsNull()
        {
            // Act
            var result = await _service.ProcessMeterReadingCsv(null);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.CriticalErrorMessage.ShouldNotBeNull();
            result.CriticalErrorMessage.ShouldBe("No file uploaded or the file is empty");
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldReturnCriticalError_WhenFileIsNotCsv()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.txt");
            fileMock.Setup(f => f.Length).Returns(100);

            // Act
            var result = await _service.ProcessMeterReadingCsv(fileMock.Object);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.CriticalErrorMessage.ShouldNotBeNull();
            result.CriticalErrorMessage.ShouldBe("Only CSV files are allowed");
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldAddError_WhenAccountIsNotFound()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,16/11/2024 10:00,12345");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account)null);

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.ErrorMessages.Count().ShouldBe(1);
            result.ErrorMessages.First().ShouldBe("Account not found for AccountId 1");
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldAddError_WhenMeterReadingFormatIsInvalid()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,16/11/2024 10:00,-123");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Account());

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.ErrorMessages.Count().ShouldBe(1);
            result.ErrorMessages.First().ShouldBe("Invalid meter reading value '-00123' for AccountId 1 on 16/11/2024 10:00");
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldAddError_WhenMeterReadingIsDuplicate()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,16/11/2024 10:00,12345");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Account());
            _mockMeterReadingsRepository.Setup(x => x.DoesMeterReadingExistAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.ErrorMessages.Count().ShouldBe(1);
            result.ErrorMessages.First().ShouldBe("Duplicate meter reading detected for AccountId 1 on 16/11/2024 10:00");
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldAddError_WhenMeterReadingIsOlderThanExisting()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,15/11/2024 10:00,12345");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Account());
            _mockMeterReadingsRepository.Setup(x => x.GetLatestReadingDateAsync(1)).ReturnsAsync(new DateTime(2024, 11, 16));

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.ErrorMessages.Count().ShouldBe(1);
            result.ErrorMessages.First().ShouldBe("Meter reading for AccountId 1 is older than existing meter reading on 16/11/2024 00:00");
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldProcessWithNoErrors_ForValidRecords()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,16/11/2024 10:00,12345");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Account());
            _mockMeterReadingsRepository.Setup(x => x.DoesMeterReadingExistAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(false);
            _mockMeterReadingsRepository.Setup(x => x.GetLatestReadingDateAsync(It.IsAny<int>())).ReturnsAsync((DateTime?)null);

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.IsValid.ShouldBeTrue();
            result.ErrorMessages.Count().ShouldBe(0);
        }

        [Fact]
        public async Task ProcessMeterReadingCsv_ShouldReturnCriticalError_WhenExceptionIsThrown()
        {
            // Arrange
            var file = CreateMockCsvFile("AccountId,MeterReadingDateTime,MeterReadValue\n1,16/11/2024 10:00,123");
            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _service.ProcessMeterReadingCsv(file);

            // Assert
            result.ShouldBeOfType<ServiceResult>();
            result.CriticalErrorMessage.ShouldNotBeNull();
            result.CriticalErrorMessage.ShouldBe("Error processing CSV file: Test exception");
            result.IsValid.ShouldBeFalse();
        }



        private IFormFile CreateMockCsvFile(string content)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns("test.csv");
            fileMock.Setup(f => f.Length).Returns(stream.Length);

            return fileMock.Object;
        }
    }
}
