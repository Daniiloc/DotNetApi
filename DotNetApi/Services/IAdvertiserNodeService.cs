using DotNetApi.Models;

namespace DotNetApi.Services
{
    public interface IAdvertiserNodeService
    {
        void AddNode(List<AdvertiserNode> tree, string location, string advertiser);
        void FillAdvertisers(AdvertiserNode node);
    }
}
