using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PlatformSupport.Collections.Specialized;

namespace PlatformSupport.Collections.ObjectModel
{
	// Token: 0x02000561 RID: 1377
	public class ObservableDictionary<TKey, TValue> : IDictionary<!0, !1>, ICollection<KeyValuePair<!0, !1>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000FED16 File Offset: 0x000FCF16
		protected IDictionary<TKey, TValue> Dictionary
		{
			get
			{
				return this._Dictionary;
			}
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000FED1E File Offset: 0x000FCF1E
		public ObservableDictionary()
		{
			this._Dictionary = new Dictionary<!0, !1>();
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000FED31 File Offset: 0x000FCF31
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this._Dictionary = new Dictionary<!0, !1>(dictionary);
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000FED45 File Offset: 0x000FCF45
		public ObservableDictionary(IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<!0, !1>(comparer);
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000FED59 File Offset: 0x000FCF59
		public ObservableDictionary(int capacity)
		{
			this._Dictionary = new Dictionary<!0, !1>(capacity);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000FED6D File Offset: 0x000FCF6D
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<!0, !1>(dictionary, comparer);
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x000FED82 File Offset: 0x000FCF82
		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<!0, !1>(capacity, comparer);
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000FED97 File Offset: 0x000FCF97
		public void Add(TKey key, TValue value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000FEDA2 File Offset: 0x000FCFA2
		public bool ContainsKey(TKey key)
		{
			return this.Dictionary.ContainsKey(key);
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x000FEDB0 File Offset: 0x000FCFB0
		public ICollection<TKey> Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000FEDC0 File Offset: 0x000FCFC0
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TValue tvalue;
			this.Dictionary.TryGetValue(key, out tvalue);
			bool flag = this.Dictionary.Remove(key);
			if (flag)
			{
				this.OnCollectionChanged();
			}
			return flag;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000FEE04 File Offset: 0x000FD004
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.Dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000FEE13 File Offset: 0x000FD013
		public ICollection<TValue> Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}

		// Token: 0x170005ED RID: 1517
		public TValue this[TKey key]
		{
			get
			{
				return this.Dictionary[key];
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000FEE39 File Offset: 0x000FD039
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Insert(item.Key, item.Value, true);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000FEE50 File Offset: 0x000FD050
		public void Clear()
		{
			if (this.Dictionary.Count > 0)
			{
				this.Dictionary.Clear();
				this.OnCollectionChanged();
			}
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000FEE71 File Offset: 0x000FD071
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.Dictionary.Contains(item);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000FEE7F File Offset: 0x000FD07F
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.Dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x000FEE8E File Offset: 0x000FD08E
		public int Count
		{
			get
			{
				return this.Dictionary.Count;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x000FEE9B File Offset: 0x000FD09B
		public bool IsReadOnly
		{
			get
			{
				return this.Dictionary.IsReadOnly;
			}
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000FEEA8 File Offset: 0x000FD0A8
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000FEEB7 File Offset: 0x000FD0B7
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000FEEC4 File Offset: 0x000FD0C4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060031EC RID: 12780 RVA: 0x000FEED4 File Offset: 0x000FD0D4
		// (remove) Token: 0x060031ED RID: 12781 RVA: 0x000FEF0C File Offset: 0x000FD10C
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060031EE RID: 12782 RVA: 0x000FEF44 File Offset: 0x000FD144
		// (remove) Token: 0x060031EF RID: 12783 RVA: 0x000FEF7C File Offset: 0x000FD17C
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060031F0 RID: 12784 RVA: 0x000FEFB4 File Offset: 0x000FD1B4
		public void AddRange(IDictionary<TKey, TValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Count > 0)
			{
				if (this.Dictionary.Count > 0)
				{
					if (items.Keys.Any((TKey k) => this.Dictionary.ContainsKey(k)))
					{
						throw new ArgumentException("An item with the same key has already been added.");
					}
					using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = items.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<TKey, TValue> item = enumerator.Current;
							this.Dictionary.Add(item);
						}
						goto IL_85;
					}
				}
				this._Dictionary = new Dictionary<!0, !1>(items);
				IL_85:
				this.OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray<KeyValuePair<TKey, TValue>>());
			}
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000FF064 File Offset: 0x000FD264
		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TValue tvalue;
			if (!this.Dictionary.TryGetValue(key, out tvalue))
			{
				this.Dictionary[key] = value;
				this.OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<!0, !1>(key, value));
				return;
			}
			if (add)
			{
				throw new ArgumentException("An item with the same key has already been added.");
			}
			if (object.Equals(tvalue, value))
			{
				return;
			}
			this.Dictionary[key] = value;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<!0, !1>(key, value), new KeyValuePair<!0, !1>(key, tvalue));
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000FF0F4 File Offset: 0x000FD2F4
		private void OnPropertyChanged()
		{
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnPropertyChanged("Keys");
			this.OnPropertyChanged("Values");
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000FF122 File Offset: 0x000FD322
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000FF13E File Offset: 0x000FD33E
		private void OnCollectionChanged()
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000FF160 File Offset: 0x000FD360
		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000FF188 File Offset: 0x000FD388
		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
			}
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000FF1B6 File Offset: 0x000FD3B6
		private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
			}
		}

		// Token: 0x04002107 RID: 8455
		private const string CountString = "Count";

		// Token: 0x04002108 RID: 8456
		private const string IndexerName = "Item[]";

		// Token: 0x04002109 RID: 8457
		private const string KeysName = "Keys";

		// Token: 0x0400210A RID: 8458
		private const string ValuesName = "Values";

		// Token: 0x0400210B RID: 8459
		private IDictionary<TKey, TValue> _Dictionary;
	}
}
