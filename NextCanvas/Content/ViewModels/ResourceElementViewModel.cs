#region

using NextCanvas.Utilities.Content;

#endregion

namespace NextCanvas.Content.ViewModels
{
    public abstract class ResourceElementViewModel : ContentElementViewModel
    {
        private readonly IResourceLocator locator = new BridgeResourceLocator();
        private ResourceViewModel resource;

        internal ResourceElementViewModel(ResourceElement model, IResourceLocator locator = null) : base(model)
        {
            if (locator == null) return;
            this.locator = locator;
        }

        public new ResourceElement Model => (ResourceElement) base.Model;

        public ResourceViewModel Resource
        {
            get
            {
                if (resource == null) Resource = new ResourceViewModel(Model.Resource, locator);
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

        protected virtual void OnResourceChanged()
        {
        }
    }
}