using System;
using UnityEngine;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000135 RID: 309
	[RequireComponent(typeof(Button))]
	public class ButtonHelper : MonoBehaviour
	{
		// Token: 0x06000BD2 RID: 3026 RVA: 0x0005385B File Offset: 0x00051A5B
		private void Start()
		{
			this.button = base.GetComponent<Button>();
			this.text = base.transform.GetComponentInChildren<Text>();
			this.initialColor = this.text.color;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0005388C File Offset: 0x00051A8C
		private void Update()
		{
			if (this.lastInteractable != this.button.interactable)
			{
				this.lastInteractable = this.button.interactable;
				this.text.color = (this.lastInteractable ? this.initialColor : this.DisabledColor);
			}
		}

		// Token: 0x04000CD2 RID: 3282
		public Color DisabledColor = new Color(0.462f, 0.482f, 0.494f);

		// Token: 0x04000CD3 RID: 3283
		private Button button;

		// Token: 0x04000CD4 RID: 3284
		private Text text;

		// Token: 0x04000CD5 RID: 3285
		private bool lastInteractable = true;

		// Token: 0x04000CD6 RID: 3286
		private Color initialColor;
	}
}
