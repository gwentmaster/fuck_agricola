using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000642 RID: 1602
	public class ToggleButton : MonoBehaviour
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003B05 RID: 15109 RVA: 0x001256FB File Offset: 0x001238FB
		// (set) Token: 0x06003B06 RID: 15110 RVA: 0x00125703 File Offset: 0x00123903
		public bool IsOn
		{
			get
			{
				return this._isOn;
			}
			set
			{
				this._isOn = value;
				this._UpdateUI();
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00125712 File Offset: 0x00123912
		private void _UpdateUI()
		{
			GameObject onLayer = this._ui.onLayer;
			if (onLayer != null)
			{
				onLayer.SetActive(this._isOn);
			}
			GameObject offLayer = this._ui.offLayer;
			if (offLayer == null)
			{
				return;
			}
			offLayer.SetActive(!this._isOn);
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0012574E File Offset: 0x0012394E
		private void OnEnable()
		{
			this._UpdateUI();
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00125756 File Offset: 0x00123956
		public void OnButtonClicked()
		{
			this.IsOn = !this.IsOn;
		}

		// Token: 0x0400263A RID: 9786
		private const string _documentation = "<b>ToggleButton</b> requires a <b>Button</b> and displays <b>onLayer</b> or <b>offLayer</b> according to <b>IsOn</b> property";

		// Token: 0x0400263B RID: 9787
		[SerializeField]
		private ToggleButton.UI _ui;

		// Token: 0x0400263C RID: 9788
		private bool _isOn;

		// Token: 0x02000945 RID: 2373
		[Serializable]
		public class UI
		{
			// Token: 0x0400311B RID: 12571
			public GameObject onLayer;

			// Token: 0x0400311C RID: 12572
			public GameObject offLayer;
		}
	}
}
