using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000137 RID: 311
	[RequireComponent(typeof(Button))]
	public class MoreButton : MonoBehaviour
	{
		// Token: 0x06000BDA RID: 3034 RVA: 0x00053A30 File Offset: 0x00051C30
		public static MoreButton FindInstance()
		{
			return UnityEngine.Object.FindObjectOfType<MoreButton>();
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00053A37 File Offset: 0x00051C37
		private void Start()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00053A58 File Offset: 0x00051C58
		private void OnClick()
		{
			if (this.popup != null)
			{
				UnityEngine.Object.Destroy(this.popup);
				this.popup = null;
				return;
			}
			this.popup = UnityEngine.Object.Instantiate<GameObject>(this.PopupPrefab, base.GetComponentInParent<Canvas>().transform, false);
			Popup component = this.popup.GetComponent<Popup>();
			foreach (MoreButton.PopupMenuItem popupMenuItem in this.MenuItems)
			{
				component.AddItem(popupMenuItem.label, popupMenuItem.action);
			}
		}

		// Token: 0x04000CE0 RID: 3296
		public GameObject PopupPrefab;

		// Token: 0x04000CE1 RID: 3297
		public MoreButton.PopupMenuItem[] MenuItems;

		// Token: 0x04000CE2 RID: 3298
		private GameObject popup;

		// Token: 0x02000813 RID: 2067
		public struct PopupMenuItem
		{
			// Token: 0x0600441E RID: 17438 RVA: 0x00141885 File Offset: 0x0013FA85
			public PopupMenuItem(string label, UnityAction action)
			{
				this.label = label;
				this.action = action;
			}

			// Token: 0x04002E2A RID: 11818
			public readonly string label;

			// Token: 0x04002E2B RID: 11819
			public readonly UnityAction action;
		}
	}
}
