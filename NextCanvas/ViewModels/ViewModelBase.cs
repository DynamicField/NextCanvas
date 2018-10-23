namespace NextCanvas.ViewModels
{
    public class ViewModelBase<T> : PropertyChangedObject, IViewModel<T> where T : new()
    {
        public T Model { get; protected set; }
        public ViewModelBase(T model)
        {
            Model = model;
        }
        public ViewModelBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            // I know this isn't the best :c
            // but it's not bad according that it's the effect we want
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
}
