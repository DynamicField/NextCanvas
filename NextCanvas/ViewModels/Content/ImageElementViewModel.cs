using System.Windows.Media.Imaging;
using NextCanvas.Models.Content;

namespace NextCanvas.ViewModels.Content
{
    public class ImageElementViewModel : ContentElementViewModel
    {
        public new ImageElement Model => (ImageElement)base.Model;

        public ImageElementViewModel()
        {
                
        }
        public ImageElementViewModel(ImageElement model) : base(model)
        {
            
        }

        protected override ContentElement BuildDefaultModel()
        {
            return new ImageElement();
        }

        private BitmapImage image;

        // TODO : Implement Image setter. 
        public BitmapImage Image
        {
            get
            {
                if (image != null) return image;
                CreateBitmapImage();
                return image;
            }
        }

        private void CreateBitmapImage()
        {
            ImageResource.Data.Position = 0;
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ImageResource.Data;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();
        }

        private ResourceViewModel imageResource;
        public ResourceViewModel ImageResource
        {
            get
            {
                if (imageResource == null)
                {
                    ImageResource = new ResourceViewModel(Model.Resource);
                }
                return imageResource;
            }
            set
            {
                imageResource = value;
                Model.Resource = imageResource.Model;
                CreateBitmapImage();               
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(ImageResource));
            }
        }
    }
}
