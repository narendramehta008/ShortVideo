using ShortVideo.API.Services;

namespace ShortVideo.API.Models
{
    public class UploadModel
    {
        public string ParentFolderId { get; set; } = GoogleDriveFolderId.DefaultFileFolder;
        public string FileName { get; set; }
        public byte[] ByteArray { get; set; }
        public string ContentType { get; set; }
    }

    public class FolderModel
    {
        // root folder id
        public string ParentFolderId { get; set; } = GoogleDriveFolderId.RootParentFolder;

        public string FolderName { get; set; }
        public string ContentType { get; set; } = "application/vnd.google-apps.folder";
    }
}