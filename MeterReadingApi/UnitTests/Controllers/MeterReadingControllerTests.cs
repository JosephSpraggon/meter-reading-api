using MeterReadingApi.Controllers;
using MeterReadingApi.Services;
using MeterReadingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace MeterReadingApi.Tests.Controllers
{
    public class MeterReadingControllerTests
    {
        private readonly Mock<IMeterReadingService> _mockMeterReadingService;
        private readonly MeterReadingController _controller;

        public MeterReadingControllerTests()
        {
            _mockMeterReadingService = new Mock<IMeterReadingService>();
            _controller = new MeterReadingController(_mockMeterReadingService.Object);
        }

        [Fact]
        public async Task UploadMeterReadings_ShouldReturnBadRequest_WhenServiceResultIsInvalid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var serviceResult = new ServiceResult();
            serviceResult.AddCriticalError("Critical error");

            _mockMeterReadingService.Setup(s => s.ProcessMeterReadingCsv(It.IsAny<IFormFile>())).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.UploadMeterReadings(mockFile.Object);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.ShouldNotBeNull();
            badRequestResult.Value.ShouldBeEquivalentTo(new
            {
                CriticalError = "Critical error"
            });
        }

        [Fact]
        public async Task UploadMeterReadings_ShouldReturnOk_WhenServiceResultIsValid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var serviceResult = new ServiceResult();
            serviceResult.AddSuccessfulResult();
            serviceResult.AddErrorMessage("Failed reading error 1");
            serviceResult.AddErrorMessage("Failed reading error 2");

            _mockMeterReadingService.Setup(s => s.ProcessMeterReadingCsv(It.IsAny<IFormFile>())).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.UploadMeterReadings(mockFile.Object);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();
            okResult.Value.ShouldBeEquivalentTo(new
            {
                Message = "Meter reading file was uploaded successfully with errors",
                SuccessfulReadings = 1,
                FailedReadings = 2,
                FailedReadingsErrors = new List<string>
                {
                    "Failed reading error 1",
                    "Failed reading error 2"
                }
            });
        }
    }
}
