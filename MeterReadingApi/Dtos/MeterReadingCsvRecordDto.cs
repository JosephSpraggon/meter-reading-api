namespace MeterReadingApi.Dtos
{
    public class MeterReadingCsvRecordDto
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }
    }
}