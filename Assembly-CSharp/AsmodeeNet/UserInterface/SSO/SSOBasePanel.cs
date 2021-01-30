using System;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x02000648 RID: 1608
	public class SSOBasePanel : MonoBehaviour
	{
		// Token: 0x06003B45 RID: 15173 RVA: 0x00126A89 File Offset: 0x00124C89
		public virtual void Show()
		{
			this.SwitchWaitingPanelMode(false, -1);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0002A073 File Offset: 0x00028273
		public virtual void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x00126AA0 File Offset: 0x00124CA0
		public void SwitchWaitingPanelMode(bool isWaiting, int waitingButtonIndex = -1)
		{
			if (isWaiting && waitingButtonIndex == -1)
			{
				AsmoLogger.Warning("SSOBasePanel", "In " + base.gameObject.name + " you may want to specify a \"waitingButtonIndex\"", null);
			}
			for (int i = 0; i < this._buttons.Length; i++)
			{
				ActivityIndicatorButton component = this._buttons[i].GetComponent<ActivityIndicatorButton>();
				if (component != null)
				{
					if (!isWaiting)
					{
						component.Waiting = false;
						component.Interactable = true;
					}
					else if (i == waitingButtonIndex)
					{
						component.Waiting = true;
						component.Interactable = false;
					}
					else
					{
						component.Waiting = false;
						component.Interactable = false;
					}
				}
				else
				{
					this._buttons[i].interactable = !isWaiting;
				}
			}
		}

		// Token: 0x0400265A RID: 9818
		private const string _kModuleName = "SSOBasePanel";

		// Token: 0x0400265B RID: 9819
		[Tooltip("In this order: no, yes, others")]
		[SerializeField]
		protected Button[] _buttons;
	}
}
