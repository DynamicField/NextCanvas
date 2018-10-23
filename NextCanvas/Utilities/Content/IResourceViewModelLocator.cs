using NextCanvas.Models.Content;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.Utilities.Content
{
    internal interface IResourceViewModelLocator : IResourceLocator
    {
        ResourceViewModel GetResourceViewModelDataFor(ResourceViewModel resource);
        ResourceViewModel GetResourceViewModelDataFor(Resource resource);
    }
}