using NextCanvas.Models.Content;

namespace NextCanvas.Utilities.Content
{
    /// <summary>
    ///     Dummy resource locator that just gives you back the resource.
    /// </summary>
    internal class BridgeResourceLocator : IResourceLocator
    {
        public Resource GetResourceDataFor(Resource resource)
        {
            return resource;
        }
    }
}