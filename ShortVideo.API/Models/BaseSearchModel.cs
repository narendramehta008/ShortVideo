namespace ShortVideo.API.Models
{
    public class BaseSearchModel
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        
        public string Deeplink_url { get; set; }
        public string Feed_type { get; set; }
    }

    public class PopularCreators : BaseSearchModel //POPULAR CREATORS
    {
        public bool Is_verified { get; set; }
        public bool Verified { get; set; }
    }

    public class Categories: BaseSearchModel //CATEGORIES
    {
        public int RecordId { get; set; }
        public string Description { get; set; }

    }
    public class Feed : BaseSearchModel //FEED
    {
        public int RecordId { get; set; }
        public string Description { get; set; }

    }
    public class PopularHashtag : Feed //POPULAR HASHTAG
    {
        public string Rich_deeplink { get; set; }

    }
}
