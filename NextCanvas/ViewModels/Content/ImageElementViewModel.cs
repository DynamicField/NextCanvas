using System.Windows.Media.Imaging;
using NextCanvas.Models.Content;
using NextCanvas.Utilities.Content;

namespace NextCanvas.ViewModels.Content
{
    public class ImageElementViewModel : ResourceElementViewModel, INamedObject
    {
        private BitmapImage image;

        internal ImageElementViewModel(ResourceElement model, IResourceLocator resource = null) : base(model, resource)
        {
        }

        public new ImageElement Model => (ImageElement) base.Model;

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

        protected override ContentElement BuildDefaultModel()
        {
            return new ImageElement();
        }

        public override double Width
        {
            get => base.Width;
            set
            {            
                base.Width = FixWidth(value);
            }
        }

        public override double Height
        {
            get => base.Height;
            set { base.Height = FixHeight(value); }
        }

        private double FixWidth(double width)
        {
            if ((int)width == Image.PixelWidth) return width;
            var ratio = (double) Image.PixelWidth / Image.PixelHeight;
            base.Height = width / ratio;
            return width;
        }
        private double FixHeight(double height)
        {
            if ((int)height == Image.PixelHeight) return height;
            var ratio = (double)Image.PixelWidth / Image.PixelHeight;
            base.Width = height * ratio;
            return height;
        }
        private void CreateBitmapImage()
        {
            if (Resource?.Data == null) return;
            Resource.Data.Position = 0;
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = Resource.Data;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();
            OnPropertyChanged(nameof(Image));
        }
        protected override void OnResourceChanged()
        {
            base.OnResourceChanged();
            CreateBitmapImage();
        }

        public string Name => "Image";
    }
}