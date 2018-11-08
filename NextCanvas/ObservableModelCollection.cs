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
    public sealed class ObservableModelCollection<TModel> : ObservableCollection<TModel> where TModel : class, new()
    {
        public ObservableModelCollection()
        {
            CollectionChanged += Sync_Collection;
        }

        public ObservableModelCollection(IList<TModel> modelCollection = null) : this()
        {
            ModelCollection = modelCollection;
            if (ModelCollection != null)
                ExecuteWithSyncDisabled(() =>
                {
                    foreach (var item in ModelCollection) Add(item);
                });
        }

        public ObservableModelCollection(IList<TModel> modelCollection = null, Action<TModel> added = null, Action<TModel> removed = null) : this(modelCollection)
        {
            ItemAdded = added;
            ItemRemoved = removed;
        }

        public bool DisableSync { get; set; }
        public Action<TModel> ItemAdded { get; set; }
        public Action<TModel> ItemRemoved { get; set; }

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
                foreach (var item in e.NewItems.Cast<TModel>())
                {
                    ItemAdded?.Invoke(item);
                    ModelCollection?.Add(item);
                }

            if (e.OldItems != null)
                foreach (var item in e.OldItems.Cast<TModel>())
                {
                    ItemRemoved?.Invoke(item);
                    ModelCollection?.Remove(item);
                }
        }
    }
}