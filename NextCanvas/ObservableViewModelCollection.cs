#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using NextCanvas.ViewModels;

#endregion

namespace NextCanvas
{
    public sealed class ObservableViewModelCollection<TViewModel, TModel> : ObservableCollection<TViewModel>
        where TViewModel : IViewModel<TModel> where TModel : class, new()
    {
        public ObservableViewModelCollection()
        {
            CollectionChanged += Sync_Collection;
        }

        public ObservableViewModelCollection(IList<TModel> modelCollection = null,
            Func<TModel, TViewModel> vmConstructor = null) : this()
        {
            ModelCollection = modelCollection;
            if (ModelCollection != null && vmConstructor != null)
                ExecuteWithSyncDisabled(() =>
                {
                    foreach (var item in ModelCollection)
                    {
                        var viewModel = vmConstructor(item);
                        Add(viewModel);
                    }
                });
        }

        public ObservableViewModelCollection(IList<TModel> modelCollection = null,
            Func<TModel, TViewModel> vmConstructor = null, Action<TViewModel> added = null,
            Action<TViewModel> removed = null) : this(modelCollection, vmConstructor)
        {
            ItemAdded = added;
            ItemRemoved = removed;
        }

        public bool DisableSync { get; set; }
        public Action<TViewModel> ItemAdded { get; set; }
        public Action<TViewModel> ItemRemoved { get; set; }
        private List<PropertyChangedAdder> addHandlers = new List<PropertyChangedAdder>();
        private List<PropertyChangedRemover> removeHandlers = new List<PropertyChangedRemover>();
        private IList<TModel> ModelCollection { get; }

        public void ExecuteWithSyncDisabled(Action action)
        {
            DisableSync = true;
            action();
            DisableSync = false;
        }

        public void SubscribeToPropertyChanged(PropertyChangedEventHandler handler)
        {
            foreach (var viewModel in this)
            {
                ((INotifyPropertyChanged) viewModel).PropertyChanged += handler;
            }

            var addHandler = new PropertyChangedAdder(handler);
            var removeHandler = new PropertyChangedRemover(handler);
            ItemAdded +=  addHandler;
            ItemRemoved += removeHandler;
            addHandlers.Add(addHandler);
            removeHandlers.Add(removeHandler);
        }
        public void UnSubscribeToPropertyChanged(PropertyChangedEventHandler handler)
        {
            foreach (var viewModel in this)
            {
                ((INotifyPropertyChanged) viewModel).PropertyChanged -= handler;
            }

            var adder = addHandlers.First(a => a.Handler == handler);
            var remover = removeHandlers.First(a => a.Handler == handler);
            ItemAdded -= adder;
            ItemRemoved -= remover;
            removeHandlers.Remove(remover);
            addHandlers.Remove(adder);
        }
        private void Sync_Collection(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DisableSync) return;

            if (e.NewItems != null)
                foreach (var item in e.NewItems.Cast<TViewModel>())
                {
                    ItemAdded?.Invoke(item);
                    ModelCollection?.Add(item.Model);
                }

            if (e.OldItems != null)
                foreach (var item in e.OldItems.Cast<TViewModel>())
                {
                    ItemRemoved?.Invoke(item);
                    ModelCollection?.Remove(item.Model);
                }
        }

        private abstract class PropertyChangedDealer
        {
            public abstract Action<TViewModel> Action { get; }
            public PropertyChangedEventHandler Handler { get; set; }

            public PropertyChangedDealer(PropertyChangedEventHandler handler)
            {
                Handler = handler;
            }

            public static implicit operator Action<TViewModel>(PropertyChangedDealer d) => d.Action;
            public static implicit operator PropertyChangedEventHandler(PropertyChangedDealer d) => d.Handler;
        }
        private class PropertyChangedAdder : PropertyChangedDealer
        {
            public override Action<TViewModel> Action => vm => ((INotifyPropertyChanged) vm).PropertyChanged += Handler;

            public PropertyChangedAdder(PropertyChangedEventHandler handler) : base(handler)
            {
            }
        }
        private class PropertyChangedRemover : PropertyChangedDealer
        {
            public override Action<TViewModel> Action => vm => ((INotifyPropertyChanged)vm).PropertyChanged -= Handler;

            public PropertyChangedRemover(PropertyChangedEventHandler handler) : base(handler)
            {
            }
        }
    }
}