using System.Collections.Generic;

namespace BaseLibrary.Helpers
{
    public class DataHelper
    {
        public static string GetBetween(string @this, string before, string after)
        {
            int beforeStartIndex = @this.IndexOf(before);
            int startIndex = beforeStartIndex + before.Length;
            int afterStartIndex = @this.IndexOf(after, startIndex);

            if (beforeStartIndex == -1 || afterStartIndex == -1)
                return "";

            return @this.Substring(startIndex, afterStartIndex - startIndex);
        }

        public static List<T> DeepClone<T>(List<T> list) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        public static T DeepClone<T>(T obj) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}