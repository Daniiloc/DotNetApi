namespace DotNetApi.Models.Helpers
{
    public static class AdvertiserNodeHelper
    {
        public static AdvertiserNode? GetNodeByValueFromList(this List<AdvertiserNode> nodes, string value, bool isNeedIdentical)
        {
            foreach (var node in nodes)
            {
                var foundNode = node.GetNodeByValue(value, isNeedIdentical);
                if (foundNode != null) return foundNode;
            }
            return null;
        }

        public static AdvertiserNode? GetNodeByValue(this AdvertiserNode treeNode, string value, bool isNeedIdentical)
        {
            if (treeNode.Value == value) return treeNode;
            if (!value.StartsWith(treeNode.Value)) return null;
            if (!isNeedIdentical && treeNode.Children.Count == 0) return treeNode;
            return treeNode.Children.GetNodeByValueFromList(value, isNeedIdentical) ?? (!isNeedIdentical && value.StartsWith(treeNode.Value) ? treeNode : null);
        }
    }
}
