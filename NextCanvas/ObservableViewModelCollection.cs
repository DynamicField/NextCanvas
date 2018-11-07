#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
                    foreach (var item in ModelCollection) Add(vmConstructor(item));
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

        private IList<TModel> ModelCollection { get; }

        public void ExecuteWithSyncDisabled(Action action)
        {
            DisableSync = true;
            action();
            DisableSync = false;
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
    }
}