using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ZalDomain.tools
{
    public class ObservableSortedSet<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        readonly SortedSet<T> InnerCollection;

        public ObservableSortedSet(IComparer<T> comparer) {
            InnerCollection = new SortedSet<T>(comparer);
        }

        public int Count => InnerCollection.Count;

        public bool IsReadOnly => (InnerCollection as ICollection<T>).IsReadOnly;

        public void Add(T item) {
            InnerCollection.Add(item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void AddAll(ICollection<T> items) {
            foreach(T item in items) {
                InnerCollection.Add(item);
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }

        public void Clear() {
            InnerCollection.Clear();
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Remove(T item) {
            bool isRemoved = InnerCollection.Remove(item);
            if (isRemoved) {
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
            return isRemoved;
        }

        public bool Contains(T item) {
            return InnerCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            InnerCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return InnerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) {
            if (CollectionChanged != null) {
                CollectionChanged.Invoke(this, args);
            }
        }
    }
}
