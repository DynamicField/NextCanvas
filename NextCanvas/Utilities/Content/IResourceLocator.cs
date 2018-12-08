#region

using NextCanvas.Content;

#endregion

namespace NextCanvas.Utilities.Content
{
    public interface IResourceLocator
    {
        Resource GetResourceDataFor(Resource resource);
    }
}