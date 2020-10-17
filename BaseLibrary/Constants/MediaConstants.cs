using BaseLibrary.Helpers;
using System.Collections.Generic;

namespace BaseLibrary.Constants
{
    public class MediaConstants
    {
        public static Dictionary<FileType, IEnumerable<string>> SupportedFormats = new Dictionary<FileType, IEnumerable<string>>()
        {
            {FileType.Image, new List<string> { ".jpg", ".jpeg", ".png" } },
            {FileType.Video, new List<string> { ".mp4", ".mpeg"} },
            {FileType.File, MimeTypeMap.GetAllExtension() },
        };
    }

    public enum FileType
    {
        Image,
        Video,
        File
    }
}