#region

using System.Windows.Media.Imaging;
using NextCanvas.Properties;
using NextCanvas.Utilities.Content;

#endregion

namespace NextCanvas.Content.ViewModels
{
    public class ImageElementViewModel : ResourceElementViewModel, INamedObject
    {
        private BitmapImage _image;

        internal ImageElementViewModel(ResourceElement model, IResourceLocator resource = null) : base(model, resource)
        {
        }

        public new ImageElement Model => (ImageElement) base.Model;

        // TODO : Implement Image setter. 
        public BitmapImage Image
        {
            get
            {
                if (_image != null) return _image;
                CreateBitmapImage();
                return _image;
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
            _image = new BitmapImage();
            _image.BeginInit();
            _image.StreamSource = Resource.Data;
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.EndInit();
            _image.Freeze();
            OnPropertyChanged(nameof(Image));
        }
        protected override void OnResourceChanged()
        {
            base.OnResourceChanged();
            CreateBitmapImage();
        }

        public string Name => DefaultObjectNamesResources.Image;
    }
}