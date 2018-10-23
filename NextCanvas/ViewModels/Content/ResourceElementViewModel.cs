using NextCanvas.Models.Content;
using NextCanvas.Utilities.Content;

namespace NextCanvas.ViewModels.Content
{
    public abstract class ResourceElementViewModel : ContentElementViewModel
    {
        public new ResourceElement Model => (ResourceElement)base.Model;

        internal ResourceElementViewModel(ResourceElement model) : base(model)
        {
            
        }

        internal ResourceElementViewModel(ResourceElement model, IResourceLocator locator): base(model)
        {
            this.locator = locator;
        }

        private readonly IResourceLocator locator = new BridgeResourceLocator();
        private ResourceViewModel resource;
        public ResourceViewModel Resource
        {
            get
            {
                if (resource == null)
                {
                    Resource = new ResourceViewModel(Model.Resource, locator);
                }
                return resource;
            }
            set
            {
                if (value == resource) return; // It's certainly given back on the second set.
                resource?.Dispose();
                resource = value;
                Model.Resource = resource.Model;
                OnResourceChanged();
                OnPropertyChanged(nameof(Resource));
            }
        }
        protected virtual void OnResourceChanged() { }
    }
}
