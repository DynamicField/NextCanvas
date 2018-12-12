#region

using System;
using System.IO;
using NextCanvas.Utilities.Content;
using NextCanvas.ViewModels;

#endregion

namespace NextCanvas.Content.ViewModels
{
    public class ResourceViewModel : ViewModelBase<Resource>, IDisposable
    {
        public ResourceViewModel(Resource model = null) : base(model)
        {
        }

        internal ResourceViewModel(Resource model, IResourceLocator resourceLocator)
        {
            Model = resourceLocator.GetResourceDataFor(model);
        }

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ResourceType Type
        {
            get => Model.Type;
            set
            {
                Model.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public Stream Data
        {
            get => Model.Data;
            set
            {
                Model.Data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public string DataMD5Hash
        {
            get => Model.DataMD5Hash;
            set
            {
                Model.DataMD5Hash = value;
                OnPropertyChanged(nameof(DataMD5Hash));
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResourceViewModel o)) return false;
            return DataMD5Hash == o.DataMD5Hash || Model == o.Model;
        }

        public override int GetHashCode()
        {
            return new { DataMD5Hash }.GetHashCode();
        }

        public void Dispose()
        {
            Model.Dispose();
        }
    }
}