using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200065A RID: 1626
	[Serializable]
	public class TabToggleTMP : Toggle
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x00129B9B File Offset: 0x00127D9B
		protected override void Start()
		{
			base.Start();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00129BCC File Offset: 0x00127DCC
		private void OnValueChanged(bool value)
		{
			this._isHighlighted = value;
			this.RefreshShadowLine();
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00129BDB File Offset: 0x00127DDB
		private void RefreshShadowLine()
		{
			if (this.Text != null)
			{
				this.Text.color = (this._isHighlighted ? this.ColorOn : this.ColorOff);
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00129C0C File Offset: 0x00127E0C
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this._isHighlighted = true;
			this.RefreshShadowLine();
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x00129C22 File Offset: 0x00127E22
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00129C3D File Offset: 0x00127E3D
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this._isHighlighted = true;
			this.RefreshShadowLine();
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00129C53 File Offset: 0x00127E53
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x040026CE RID: 9934
		public Color ColorOn;

		// Token: 0x040026CF RID: 9935
		public Color ColorOff;

		// Token: 0x040026D0 RID: 9936
		public TextMeshProUGUI Text;

		// Token: 0x040026D1 RID: 9937
		private bool _isHighlighted;
	}
}
