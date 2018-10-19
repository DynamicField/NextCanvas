using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextCanvas.ViewModels
{
    public class ViewModelBase<T> : PropertyChangedObject, IViewModel<T> where T : new()
    {
        public T Model { get; }
        public ViewModelBase(T model)
        {
            Model = model;
        }
        public ViewModelBase()
        {
            Model = BuildDefaultModel();
        }
        public override string ToString()
        {
            return Model.ToString();
        }
        protected virtual T BuildDefaultModel()
        {
            return new T();
        }
    }
    public interface IViewModel<out T>
    {
        T Model { get; }
    }
}
