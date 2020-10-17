using BaseLibrary.Constants;
using System.Collections.Generic;
using System.IO;

namespace ShortVideo.API.Models
{
    public class UploadModel
    {
        public string ParentFolderId { get; set; } = "197VtRLoVQn6_mwVfkyjHkqgBVBAqfaJb";
        public string FileName { get; set; }
        public byte[] ByteArray { get; set; }
        public string ContentType { get; set; }
    }
    public class FolderModel
    {
        // root folder id
        public string ParentFolderId { get; set; } = "11PfREpvFuSUeMbclw4_pems2X4vngXVA";
        public string FolderName { get; set; }
        public string ContentType { get; set; } = "application/vnd.google-apps.folder";
    }

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
