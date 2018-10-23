using System.Windows.Media.Imaging;
using NextCanvas.Models.Content;
using NextCanvas.Utilities.Content;

namespace NextCanvas.ViewModels.Content
{
    public class ImageElementViewModel : ResourceElementViewModel
    {
        private BitmapImage image;

        public new ImageElement Model => (ImageElement) base.Model;

        // TODO : Implement Image setter. 
        public BitmapImage Image
        {
            get
            {
                if (image != null)
                {
                    return image;
                }
                CreateBitmapImage();
                return image;
            }
        }

        public ImageElementViewModel() : base(new ImageElement())
        {
        }

        public ImageElementViewModel(ImageElement model) : base(model)
        {
        }

        internal ImageElementViewModel(ImageElement model, IResourceLocator resource) : base(model, resource)
        {
        }

        protected override ContentElement BuildDefaultModel()
        {
            return new ImageElement();
        }

        private void CreateBitmapImage()
        {
            if (Resource?.Data == null)
            {
                return;
            }
            Resource.Data.Position = 0;
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = Resource.Data;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();
        }

        protected override void OnResourceChanged()
        {
            base.OnResourceChanged();
            CreateBitmapImage();
        }
    }
}