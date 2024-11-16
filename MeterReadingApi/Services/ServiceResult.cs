namespace MeterReadingApi.Services
{
    public class ServiceResult
    {
        public List<string> ErrorMessages { get; private set; } = new List<string>();
        public string CriticalErrorMessage { get; private set; }
        public int SuccessfulResult { get; set; }
        public int FailedResult { get; set; } 

        public bool IsValid 
        {
            get
            {
                return string.IsNullOrEmpty(CriticalErrorMessage);
            }
        }

        public void AddErrorMessage(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
            FailedResult++;
        }

        public void AddCriticalError(string errorMessage)
        {
            CriticalErrorMessage = errorMessage;
        }

        public void AddSuccessfulResult(){
            SuccessfulResult++;
        }
    }
}
