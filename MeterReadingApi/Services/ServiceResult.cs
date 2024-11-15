using System.ComponentModel.DataAnnotations;

namespace MeterReadingApi.Services
{
    public class ServiceResult
    {
       public string ErrorMessage { get; private set; }
       public bool IsValid 
       {
            get
            {
                return string.IsNullOrEmpty(ErrorMessage);
            }
       }
       public void AddErrorMessage(string errorMessage)
       {
            ErrorMessage = errorMessage;
       }
    }
}