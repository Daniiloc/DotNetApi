using DotNetApi.Models;
using DotNetApi.Models.Helpers;

namespace DotNetApi.Services
{
    public class AdvertiserNodeService : IAdvertiserNodeService
    {
        public void AddNode(List<AdvertiserNode> tree, string location, string advertiser)
        {
            AdvertiserNode? foundedNode = tree.GetNodeByValueFromList(location, false);
            if (foundedNode == null)
            {
                foundedNode = new AdvertiserNode() { Value = location, Advertisers = [advertiser] };
                tree.Add(foundedNode);
            }
            else
            {
                if (foundedNode.Value == location) foundedNode.Advertisers.Add(advertiser);
                while (foundedNode.Value != location)
                {
                    var child = new AdvertiserNode() { Value = $"{foundedNode.Value}/{location.Substring(foundedNode.Value.Length).Split("/")[1]}", Advertisers = [advertiser] };
                    foundedNode.Children.Add(child);
                    foundedNode = child;
                }
            }
        }

        public void FillAdvertisers(AdvertiserNode node)
        {
            foreach (var child in node.Children)
            {
                child.Advertisers.AddRange(node.Advertisers.Except(child.Advertisers));
                FillAdvertisers(child);
            }
        }
    }
}
