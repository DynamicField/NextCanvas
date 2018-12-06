#region

using NextCanvas.Content;
using NextCanvas.Content.ViewModels;

#endregion

namespace NextCanvas.Utilities.Content
{
    internal interface IResourceViewModelLocator : IResourceLocator
    {
        ResourceViewModel GetResourceViewModelDataFor(ResourceViewModel resource);
        ResourceViewModel GetResourceViewModelDataFor(Resource resource);
    }
}