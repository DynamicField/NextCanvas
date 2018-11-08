namespace NextCanvas.ViewModels
{
    public class ViewModelBase<T> : PropertyChangedObject, IViewModel<T> where T : class, new()
    {
        public ViewModelBase(T model = null)
        {
            Model = model ?? BuildDefaultModel();
        }
        public ViewModelBase() : this(null) { }
        public T Model { get; protected set; }        
        public override string ToString()
        {
            var modelToString = Model.ToString();
            return modelToString == Model.GetType().ToString() ? base.ToString() : modelToString;
        }

        protected virtual T BuildDefaultModel()
        {
            return new T();
        }
    }
}