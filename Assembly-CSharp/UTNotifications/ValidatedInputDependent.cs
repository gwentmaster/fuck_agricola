using System;
using UnityEngine;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x0200013E RID: 318
	[RequireComponent(typeof(Button))]
	public class ValidatedInputDependent : MonoBehaviour
	{
		// Token: 0x06000BF6 RID: 3062 RVA: 0x0005400C File Offset: 0x0005220C
		private void Start()
		{
			if (this.AllowWhenPushDisabled && !this.PushNotificationsEnabled())
			{
				base.enabled = false;
				return;
			}
			this.button = base.GetComponent<Button>();
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00054034 File Offset: 0x00052234
		private void Update()
		{
			bool interactable = true;
			ValidatedInputField[] validatedInputFields = this.ValidatedInputFields;
			for (int i = 0; i < validatedInputFields.Length; i++)
			{
				if (!validatedInputFields[i].IsValid())
				{
					interactable = false;
					break;
				}
			}
			this.button.interactable = interactable;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002A062 File Offset: 0x00028262
		private bool PushNotificationsEnabled()
		{
			return false;
		}

		// Token: 0x04000CF4 RID: 3316
		public bool AllowWhenPushDisabled;

		// Token: 0x04000CF5 RID: 3317
		public ValidatedInputField[] ValidatedInputFields;

		// Token: 0x04000CF6 RID: 3318
		private Button button;
	}
}
