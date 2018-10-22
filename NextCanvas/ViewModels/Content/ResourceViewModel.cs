using NextCanvas.Models.Content;
using System;
using System.IO;

namespace NextCanvas.ViewModels.Content
{
    public class ResourceViewModel : ViewModelBase<Resource>
    {
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

        public virtual Stream Data
        {
            get => Model.Data;
            set
            {
                Model.Data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
    }
}

