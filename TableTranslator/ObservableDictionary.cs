using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace TableTranslator
{
    internal sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        internal ObservableDictionary()
        {
        }

        internal ObservableDictionary(int capacity)
            : base(capacity)
        {
        }

        internal ObservableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        internal ObservableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        internal ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        internal ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                TValue oldValue;
                var exist = base.TryGetValue(key, out oldValue);
                var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                base[key] = value;
                var newItem = new KeyValuePair<TKey, TValue>(key, value);
                if (exist)
                {
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace, newItem, oldItem, base.Keys.ToList().IndexOf(key)));
                }
                else
                {
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                        newItem, base.Keys.ToList().IndexOf(key)));
                }
            }
        }

        public new void Add(TKey key, TValue value)
        {
            var item = new KeyValuePair<TKey, TValue>(key, value);
            base.Add(key, value);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item,
                base.Keys.ToList().IndexOf(key)));
        }

        public new bool Remove(TKey key)
        {
            TValue value;
            if (base.TryGetValue(key, out value))
            {
                var item = new KeyValuePair<TKey, TValue>(key, base[key]);
                var index = base.Keys.ToList().IndexOf(key);
                var result = base.Remove(key);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item.Value, index));
                return result;
            }
            return false;
        }

        public new void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }
    }
}