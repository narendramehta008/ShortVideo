using BaseLibrary.Constants;

namespace BaseLibrary.Models.APIResponse
{
    public class StatusModel
    {
        public StatusModel(string errorMessage = null)
        {
            ErrorMessage = errorMessage;
            Status = string.IsNullOrWhiteSpace(errorMessage) ? GlobalConstants.Success : GlobalConstants.Failed;
        }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }

}
