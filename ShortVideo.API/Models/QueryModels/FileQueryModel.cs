using  BaseLibrary.Constants;

namespace ShortVideo.API.Models.QueryModels
{
    public class FileQueryModel
    {
        public string FolderName { get; set; }
        public FileType FileType { get; set; }
    }
}
