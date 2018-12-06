#region

using NextCanvas.Utilities.Content;

#endregion

namespace NextCanvas.Content.ViewModels
{
    public abstract class ResourceElementViewModel : ContentElementViewModel
    {
        private readonly IResourceLocator _locator = new BridgeResourceLocator();
        private ResourceViewModel _resource;

        internal ResourceElementViewModel(ResourceElement model, IResourceLocator locator = null) : base(model)
        {
            if (locator == null) return;
            this._locator = locator;
        }

        public new ResourceElement Model => (ResourceElement) base.Model;

        public ResourceViewModel Resource
        {
            get
            {
                if (_resource == null) Resource = new ResourceViewModel(Model.Resource, _locator);
                return _resource;
            }
            set
            {
                if (value == _resource) return; // It's certainly given back on the second set.
                _resource?.Dispose();
                _resource = value;
                Model.Resource = _resource.Model;
                OnResourceChanged();
                OnPropertyChanged(nameof(Resource));
            }
        }

        protected virtual void OnResourceChanged()
        {
        }
    }
}