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
}