using NextCanvas.Models.Content;

namespace NextCanvas.Utilities.Content
{
    internal interface IResourceLocator
    {
        Resource GetResourceDataFor(Resource resource);
    }
}
