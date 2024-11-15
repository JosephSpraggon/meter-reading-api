namespace MeterReadingApi.Models
{
    public class MeterReading
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public DateTime DateTime { get; set; }
        public int Value { get; set; }
    }
}
