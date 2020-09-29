using System.IO;

namespace ShortVideo.API.Models
{
    public class UploadModel
    {
        public string FolderId { get; set; } = "197VtRLoVQn6_mwVfkyjHkqgBVBAqfaJb";
        public string FileName { get; set; }
        public byte[] ByteArray { get; set; }
        public string ContentType { get; set; }
    }
}
