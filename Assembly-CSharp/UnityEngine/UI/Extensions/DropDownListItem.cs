using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000175 RID: 373
	[Serializable]
	public class DropDownListItem
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0005C51C File Offset: 0x0005A71C
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x0005C524 File Offset: 0x0005A724
		public string Caption
		{
			get
			{
				return this._caption;
			}
			set
			{
				this._caption = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0005C540 File Offset: 0x0005A740
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x0005C548 File Offset: 0x0005A748
		public Sprite Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0005C564 File Offset: 0x0005A764
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x0005C56C File Offset: 0x0005A76C
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				this._isDisabled = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0005C588 File Offset: 0x0005A788
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x0005C590 File Offset: 0x0005A790
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0005C599 File Offset: 0x0005A799
		public DropDownListItem(string caption = "", string inId = "", Sprite image = null, bool disabled = false, Action onSelect = null)
		{
			this._caption = caption;
			this._image = image;
			this._id = inId;
			this._isDisabled = disabled;
			this.OnSelect = onSelect;
		}

		// Token: 0x04000E19 RID: 3609
		[SerializeField]
		private string _caption;

		// Token: 0x04000E1A RID: 3610
		[SerializeField]
		private Sprite _image;

		// Token: 0x04000E1B RID: 3611
		[SerializeField]
		private bool _isDisabled;

		// Token: 0x04000E1C RID: 3612
		[SerializeField]
		private string _id;

		// Token: 0x04000E1D RID: 3613
		public Action OnSelect;

		// Token: 0x04000E1E RID: 3614
		internal Action OnUpdate;
	}
}
