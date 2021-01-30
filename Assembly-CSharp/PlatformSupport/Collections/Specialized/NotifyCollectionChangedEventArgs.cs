using System;
using System.Collections;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x0200055F RID: 1375
	public class NotifyCollectionChangedEventArgs : EventArgs
	{
		// Token: 0x060031AE RID: 12718 RVA: 0x000FE7B4 File Offset: 0x000FC9B4
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
		{
			if (action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000FE7E4 File Offset: 0x000FC9E4
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[]
				{
					changedItem
				}, -1);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000FE84C File Offset: 0x000FCA4C
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[]
				{
					changedItem
				}, index);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException("action");
			}
			if (index != -1)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000FE8C0 File Offset: 0x000FCAC0
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException("action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				this.InitializeAddOrRemove(action, changedItems, -1);
				return;
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000FE92C File Offset: 0x000FCB2C
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException("action");
				}
				if (startingIndex != -1)
				{
					throw new ArgumentException("action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				if (startingIndex < -1)
				{
					throw new ArgumentException("startingIndex");
				}
				this.InitializeAddOrRemove(action, changedItems, startingIndex);
				return;
			}
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000FE9B4 File Offset: 0x000FCBB4
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			this.InitializeMoveOrReplace(action, new object[]
			{
				newItem
			}, new object[]
			{
				oldItem
			}, -1, -1);
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000FEA04 File Offset: 0x000FCC04
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			this.InitializeMoveOrReplace(action, new object[]
			{
				newItem
			}, new object[]
			{
				oldItem
			}, index, index);
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000FEA54 File Offset: 0x000FCC54
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000FEAAC File Offset: 0x000FCCAC
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000FEB08 File Offset: 0x000FCD08
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException("action");
			}
			if (index < 0)
			{
				throw new ArgumentException("index");
			}
			object[] array = new object[]
			{
				changedItem
			};
			this.InitializeMoveOrReplace(action, array, array, index, oldIndex);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000FEB5E File Offset: 0x000FCD5E
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException("action");
			}
			if (index < 0)
			{
				throw new ArgumentException("index");
			}
			this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000FEBA0 File Offset: 0x000FCDA0
		internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : new ReadOnlyList(newItems));
			this._oldItems = ((oldItems == null) ? null : new ReadOnlyList(oldItems));
			this._newStartingIndex = newIndex;
			this._oldStartingIndex = oldIndex;
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000FEBFC File Offset: 0x000FCDFC
		private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action == NotifyCollectionChangedAction.Add)
			{
				this.InitializeAdd(action, changedItems, startingIndex);
				return;
			}
			if (action == NotifyCollectionChangedAction.Remove)
			{
				this.InitializeRemove(action, changedItems, startingIndex);
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000FEC18 File Offset: 0x000FCE18
		private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : new ReadOnlyList(newItems));
			this._newStartingIndex = newStartingIndex;
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000FEC3A File Offset: 0x000FCE3A
		private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
		{
			this._action = action;
			this._oldItems = ((oldItems == null) ? null : new ReadOnlyList(oldItems));
			this._oldStartingIndex = oldStartingIndex;
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000FEC5C File Offset: 0x000FCE5C
		private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
		{
			this.InitializeAdd(action, newItems, startingIndex);
			this.InitializeRemove(action, oldItems, oldStartingIndex);
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060031BE RID: 12734 RVA: 0x000FEC72 File Offset: 0x000FCE72
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x000FEC7A File Offset: 0x000FCE7A
		public IList NewItems
		{
			get
			{
				return this._newItems;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x000FEC82 File Offset: 0x000FCE82
		public IList OldItems
		{
			get
			{
				return this._oldItems;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x000FEC8A File Offset: 0x000FCE8A
		public int NewStartingIndex
		{
			get
			{
				return this._newStartingIndex;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000FEC92 File Offset: 0x000FCE92
		public int OldStartingIndex
		{
			get
			{
				return this._oldStartingIndex;
			}
		}

		// Token: 0x04002101 RID: 8449
		private NotifyCollectionChangedAction _action;

		// Token: 0x04002102 RID: 8450
		private IList _newItems;

		// Token: 0x04002103 RID: 8451
		private IList _oldItems;

		// Token: 0x04002104 RID: 8452
		private int _newStartingIndex = -1;

		// Token: 0x04002105 RID: 8453
		private int _oldStartingIndex = -1;
	}
}
