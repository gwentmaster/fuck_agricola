using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200061D RID: 1565
	public class AlertController : MonoBehaviour
	{
		// Token: 0x06003993 RID: 14739 RVA: 0x0011E5E4 File Offset: 0x0011C7E4
		public static AlertController InstantiateAlertController(string title, string message)
		{
			AlertController alertController = (AlertController)UnityEngine.Object.FindObjectOfType(typeof(AlertController));
			if (alertController == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.alertControllerPrefab);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				alertController = gameObject.GetComponent<AlertController>();
				alertController._Init(title, message);
			}
			else
			{
				AsmoLogger.Error("AlertController", "Try to InstantiateAlertController twice", null);
			}
			return alertController;
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x0011E64C File Offset: 0x0011C84C
		private void _Init(string title, string message)
		{
			if (!string.IsNullOrEmpty(title))
			{
				this._ui.titleLabel.text = title;
			}
			else
			{
				this._ui.header.gameObject.SetActive(false);
			}
			if (!string.IsNullOrEmpty(message))
			{
				this._ui.messageLabel.text = message;
			}
			else
			{
				this._ui.content.gameObject.SetActive(false);
			}
			Button[] buttons = this._ui.buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0011E6E3 File Offset: 0x0011C8E3
		private void Awake()
		{
			this._responsivePopUp = base.GetComponent<ResponsivePopUp>();
			if (this._responsivePopUp == null)
			{
				AsmoLogger.Error("AlertController", "Missing ResponsivePopUp behavior", null);
			}
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0011E710 File Offset: 0x0011C910
		public void AddAction(string title, AlertController.ButtonStyle style = AlertController.ButtonStyle.Default, Action action = null)
		{
			if (this._actions.Count >= 2)
			{
				throw new ArgumentOutOfRangeException();
			}
			AlertController.ButtonAction buttonAction = default(AlertController.ButtonAction);
			buttonAction.title = title;
			buttonAction.style = style;
			buttonAction.action = action;
			this._actions.Add(buttonAction);
			Button button = this._ui.buttons[this._actions.Count];
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<TextMeshProUGUI>().text = buttonAction.title;
			button.onClick.AddListener(delegate()
			{
				button.interactable = false;
				this.Dismiss();
				if (buttonAction.action != null)
				{
					buttonAction.action();
				}
			});
			AlertControllerButton component = button.GetComponent<AlertControllerButton>();
			if (component != null)
			{
				component.Style = buttonAction.style;
			}
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0011E80A File Offset: 0x0011CA0A
		public void Dismiss()
		{
			this._responsivePopUp.FadeOut(delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
			});
		}

		// Token: 0x0400253A RID: 9530
		private const string _kModuleName = "AlertController";

		// Token: 0x0400253B RID: 9531
		[SerializeField]
		private AlertController.UI _ui;

		// Token: 0x0400253C RID: 9532
		private ResponsivePopUp _responsivePopUp;

		// Token: 0x0400253D RID: 9533
		private List<AlertController.ButtonAction> _actions = new List<AlertController.ButtonAction>(2);

		// Token: 0x02000912 RID: 2322
		[Serializable]
		public class UI
		{
			// Token: 0x0400307A RID: 12410
			public RectTransform header;

			// Token: 0x0400307B RID: 12411
			public TextMeshProUGUI titleLabel;

			// Token: 0x0400307C RID: 12412
			public RectTransform content;

			// Token: 0x0400307D RID: 12413
			public TextMeshProUGUI messageLabel;

			// Token: 0x0400307E RID: 12414
			public Button[] buttons;
		}

		// Token: 0x02000913 RID: 2323
		public enum ButtonStyle
		{
			// Token: 0x04003080 RID: 12416
			Default,
			// Token: 0x04003081 RID: 12417
			Cancel,
			// Token: 0x04003082 RID: 12418
			Destructive
		}

		// Token: 0x02000914 RID: 2324
		private struct ButtonAction
		{
			// Token: 0x04003083 RID: 12419
			public string title;

			// Token: 0x04003084 RID: 12420
			public AlertController.ButtonStyle style;

			// Token: 0x04003085 RID: 12421
			public Action action;
		}
	}
}
