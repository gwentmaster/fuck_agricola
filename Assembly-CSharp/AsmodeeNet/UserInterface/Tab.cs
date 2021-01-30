using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000631 RID: 1585
	public class Tab : MonoBehaviour
	{
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06003A4D RID: 14925 RVA: 0x001219E0 File Offset: 0x0011FBE0
		// (remove) Token: 0x06003A4E RID: 14926 RVA: 0x00121A18 File Offset: 0x0011FC18
		public event Action<Tab> OnTabSelected;

		// Token: 0x06003A4F RID: 14927 RVA: 0x00121A50 File Offset: 0x0011FC50
		public void Setup(CommunityHubSkin.TabIcons icons, ToggleGroup toggleGroup, bool isHorizontal)
		{
			this._icons = icons;
			this._horizontal.root.gameObject.SetActive(isHorizontal);
			this._vertical.root.gameObject.SetActive(!isHorizontal);
			RectTransform rectTransform = isHorizontal ? this._horizontal.root : this._vertical.root;
			(base.transform as RectTransform).sizeDelta = rectTransform.sizeDelta;
			(isHorizontal ? this._horizontal.toggle : this._vertical.toggle).group = toggleGroup;
			this._Update();
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00121AEC File Offset: 0x0011FCEC
		public void OnToggleValueChanged(bool value)
		{
			this.IsOn = value;
			if (value && this.OnTabSelected != null)
			{
				this.OnTabSelected(this);
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00121B0C File Offset: 0x0011FD0C
		// (set) Token: 0x06003A52 RID: 14930 RVA: 0x00121B14 File Offset: 0x0011FD14
		public bool IsOn
		{
			get
			{
				return this._isOn;
			}
			set
			{
				this._isOn = value;
				this._Update();
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00121B24 File Offset: 0x0011FD24
		private void _Update()
		{
			bool activeSelf = this._horizontal.root.gameObject.activeSelf;
			(activeSelf ? this._horizontal.icon : this._vertical.icon).sprite = (this.IsOn ? this._icons.spriteOn : this._icons.spriteOff);
			(activeSelf ? this._horizontal.toggle : this._vertical.toggle).isOn = this.IsOn;
		}

		// Token: 0x040025AF RID: 9647
		[SerializeField]
		private Tab.TabOutlet _vertical;

		// Token: 0x040025B0 RID: 9648
		[SerializeField]
		private Tab.TabOutlet _horizontal;

		// Token: 0x040025B2 RID: 9650
		private CommunityHubSkin.TabIcons _icons;

		// Token: 0x040025B3 RID: 9651
		private bool _isOn;

		// Token: 0x02000924 RID: 2340
		[Serializable]
		private struct TabOutlet
		{
			// Token: 0x040030B5 RID: 12469
			public RectTransform root;

			// Token: 0x040030B6 RID: 12470
			public Image icon;

			// Token: 0x040030B7 RID: 12471
			public Toggle toggle;
		}
	}
}
