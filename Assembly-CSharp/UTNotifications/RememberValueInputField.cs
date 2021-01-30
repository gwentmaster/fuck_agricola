using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x0200013A RID: 314
	[RequireComponent(typeof(InputField))]
	public class RememberValueInputField : MonoBehaviour
	{
		// Token: 0x06000BEB RID: 3051 RVA: 0x00053DB0 File Offset: 0x00051FB0
		private void Awake()
		{
			this.inputField = base.GetComponent<InputField>();
			this.uniqueName = SampleUtils.UniqueName(base.transform);
			this.inputField.text = PlayerPrefs.GetString(this.uniqueName, this.inputField.text);
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00053E17 File Offset: 0x00052017
		private void OnEndEdit(string value)
		{
			PlayerPrefs.SetString(this.uniqueName, value);
		}

		// Token: 0x04000CED RID: 3309
		private InputField inputField;

		// Token: 0x04000CEE RID: 3310
		private string uniqueName;
	}
}
