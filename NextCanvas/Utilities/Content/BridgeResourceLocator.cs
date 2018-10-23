using NextCanvas.Models.Content;

namespace NextCanvas.Utilities.Content
{
    internal class BridgeResourceLocator : IResourceLocator
    {
        public Resource GetResourceDataFor(Resource resource) => resource;
    }
}
