#region

using NextCanvas.Content;

#endregion

namespace NextCanvas.Utilities.Content
{
    internal interface IResourceLocator
    {
        Resource GetResourceDataFor(Resource resource);
    }
}