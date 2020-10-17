using System.Collections.Generic;

namespace BaseLibrary.Constants
{
    public class MediaConstants
    {
        public const string ImageFormats = "ImageFormats";
        public const string VideoFormats = "VideoFormats";

        public static Dictionary<string, List<string>> SupportedFormats = new Dictionary<string, List<string>>()
        {
            {"ImageFormats", new List<string> { ".jpg", ".jpeg", ".png" } },
            {"VideoFormats", new List<string> { ".mp4", ".mpeg"} },
        };

    }
}
