namespace DotNetApi.Models
{
    public class AdvertiserNode
    {
        public AdvertiserNode()
        {
            Advertisers = [];
            Value = "";
            Parent = null;
            Children = [];
        }
        public List<string> Advertisers { get; set; }
        public string Value { get; set; }
        public AdvertiserNode? Parent { get; set; }
        public List<AdvertiserNode> Children { get; set; }
    }
}
