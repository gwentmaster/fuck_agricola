using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x0200013B RID: 315
	[RequireComponent(typeof(Toggle))]
	public class RememberValueToggle : MonoBehaviour
	{
		// Token: 0x06000BEE RID: 3054 RVA: 0x00053E28 File Offset: 0x00052028
		private void Awake()
		{
			this.toggle = base.GetComponent<Toggle>();
			this.uniqueName = SampleUtils.UniqueName(base.transform);
			this.toggle.isOn = (PlayerPrefs.GetInt(this.uniqueName, this.toggle.isOn ? 1 : 0) != 0);
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00053E9B File Offset: 0x0005209B
		private void OnValueChanged(bool value)
		{
			PlayerPrefs.SetInt(this.uniqueName, value ? 1 : 0);
		}

		// Token: 0x04000CEF RID: 3311
		private Toggle toggle;

		// Token: 0x04000CF0 RID: 3312
		private string uniqueName;
	}
}
