using System;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000625 RID: 1573
	public class ExpandCollapseButton : MonoBehaviour
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06003A05 RID: 14853 RVA: 0x00120CBC File Offset: 0x0011EEBC
		// (remove) Token: 0x06003A06 RID: 14854 RVA: 0x00120CF4 File Offset: 0x0011EEF4
		public event Action onExpandCollapseButtonClicked;

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x00120D29 File Offset: 0x0011EF29
		// (set) Token: 0x06003A08 RID: 14856 RVA: 0x00120D31 File Offset: 0x0011EF31
		public bool IsCollapsed
		{
			get
			{
				return this._collapsed;
			}
			set
			{
				if (this._collapsed != value)
				{
					this._collapsed = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00120D4A File Offset: 0x0011EF4A
		public void OnButtonClicked()
		{
			if (this.onExpandCollapseButtonClicked != null)
			{
				this.onExpandCollapseButtonClicked();
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00120D5F File Offset: 0x0011EF5F
		private void OnEnable()
		{
			this._needsUpdate = true;
			CoreApplication.Instance.Preferences.InterfaceOrientationDidChange += this._InterfaceOrientationDidChange;
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x00120D84 File Offset: 0x0011EF84
		private void LateUpdate()
		{
			if (this._needsUpdate)
			{
				this._needsUpdate = false;
				bool flag = CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal;
				this.expandHorizontal.gameObject.SetActive(this._collapsed && flag);
				this.collapseHorizontal.gameObject.SetActive(!this._collapsed && flag);
				this.expandVertical.gameObject.SetActive(this._collapsed && !flag);
				this.collpaseVertical.gameObject.SetActive(!this._collapsed && !flag);
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00120E27 File Offset: 0x0011F027
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.InterfaceOrientationDidChange -= this._InterfaceOrientationDidChange;
			}
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x00120E4B File Offset: 0x0011F04B
		private void _InterfaceOrientationDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x04002582 RID: 9602
		public Button button;

		// Token: 0x04002583 RID: 9603
		public RectTransform expandHorizontal;

		// Token: 0x04002584 RID: 9604
		public RectTransform collapseHorizontal;

		// Token: 0x04002585 RID: 9605
		public RectTransform expandVertical;

		// Token: 0x04002586 RID: 9606
		public RectTransform collpaseVertical;

		// Token: 0x04002588 RID: 9608
		private bool _collapsed;

		// Token: 0x04002589 RID: 9609
		private bool _needsUpdate;
	}
}
