using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000141 RID: 321
	public class UTNotificationsSample : MonoBehaviour
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x000541AB File Offset: 0x000523AB
		public static UTNotificationsSample Instance
		{
			get
			{
				return UTNotificationsSample.instance;
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000541B4 File Offset: 0x000523B4
		public void Initialize()
		{
			if (Manager.Instance.Initialize(true, 0, false))
			{
				Debug.Log("UTNotifications: Initialized successfully");
				return;
			}
			Debug.LogWarning("UTNotifications: Initialization failed!");
			this.InitializeText.text = this.initializeTextOriginal + "\nInitialization Error! Please check the logs.";
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00054200 File Offset: 0x00052400
		public void NotifyAll()
		{
			this.CreateNotificationDialog.Show("Push Notify All Registered Devices", false, false, delegate(string title, string text, int id, string notificationProfile, int badge, bool hasImage, bool hasButtons)
			{
				base.StartCoroutine(this.NotifyAll(title, text, id, notificationProfile, badge));
			});
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00054220 File Offset: 0x00052420
		public IEnumerator NotifyAll(string title, string text, int id, string notificationProfile, int badgeNumber)
		{
			title = this.EscapeURL(title);
			text = this.EscapeURL(text);
			string text2 = "&_NO_CACHE=" + UnityEngine.Random.value;
			bool finished = false;
			base.StartCoroutine(UTNotificationsSample.HttpRequest(string.Format("{0}/notify?title={1}&text={2}&id={3}&badge={4}{5}{6}", new object[]
			{
				this.DemoServerURLInputField.text,
				title,
				text,
				id,
				badgeNumber,
				string.IsNullOrEmpty(notificationProfile) ? "" : ("&notification_profile=" + notificationProfile),
				text2
			}), null, delegate(string successText)
			{
				finished = true;
				this.NotifyAllText.text = this.notifyAllTextOriginal + "\n" + successText;
			}, delegate(string errorText, string error)
			{
				finished = true;
				this.NotifyAllText.text = string.Concat(new string[]
				{
					this.notifyAllTextOriginal,
					"\nError: ",
					error,
					" ",
					errorText
				});
				if (!UTNotificationsSample.CheckAndroidCleartextAllowed(new Uri(this.DemoServerURLInputField.text).Host))
				{
					Text notifyAllText = this.NotifyAllText;
					notifyAllText.text = notifyAllText.text + "\n" + UTNotificationsSample.CleartextHint;
				}
			}));
			int dots = 0;
			do
			{
				string text3 = this.notifyAllTextOriginal + "\nSending";
				dots = (dots + 1) % 4;
				for (int i = 0; i < dots; i++)
				{
					text3 += ".";
				}
				this.NotifyAllText.text = text3;
				yield return new WaitForSeconds(0.15f);
			}
			while (!finished);
			yield break;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00054254 File Offset: 0x00052454
		public void CreateLocalNotification()
		{
			this.CreateNotificationDialog.Show("Create Local Notification", true, true, delegate(string title, string text, int id, string notificationProfile, int badge, bool hasImage, bool hasButtons)
			{
				Manager.Instance.PostLocalNotification(title, text, id, this.UserData(hasImage), notificationProfile, badge, this.Buttons(hasButtons));
			});
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00054274 File Offset: 0x00052474
		public void ScheduleLocalNotification()
		{
			this.CreateNotificationDialog.Show("Schedule Local Notification", true, true, delegate(string title, string text, int id, string notificationProfile, int badge, bool hasImage, bool hasButtons)
			{
				Manager.Instance.ScheduleNotification(30, title, text, id, this.UserData(hasImage), notificationProfile, badge, this.Buttons(hasButtons));
			});
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00054294 File Offset: 0x00052494
		public void ScheduleRepeatingLocalNotification()
		{
			this.CreateNotificationDialog.Show("Schedule Local Notification", true, true, delegate(string title, string text, int id, string notificationProfile, int badge, bool hasImage, bool hasButtons)
			{
				Manager.Instance.ScheduleNotificationRepeating(DateTime.Now.AddSeconds(10.0), 25, title, text, id, this.UserData(hasImage), notificationProfile, badge, this.Buttons(hasButtons));
			});
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000542B4 File Offset: 0x000524B4
		public void Hide(int id)
		{
			Manager.Instance.HideNotification(id);
			this.NotificationDetailsDialog.Hide(id);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000542CD File Offset: 0x000524CD
		public void Cancel(int id)
		{
			Manager.Instance.CancelNotification(id);
			this.NotificationDetailsDialog.Hide(id);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000542E6 File Offset: 0x000524E6
		public void CancelAll()
		{
			Manager.Instance.CancelAllNotifications();
			Manager.Instance.SetBadge(0);
			this.NotificationDetailsDialog.CancelAll();
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00054308 File Offset: 0x00052508
		public void IncrementBadge()
		{
			Manager.Instance.SetBadge(Manager.Instance.GetBadge() + 1);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00054320 File Offset: 0x00052520
		public void OnNotificationsEnabledToggleValueChanged(bool value)
		{
			if (value != Manager.Instance.NotificationsEnabled())
			{
				Manager.Instance.SetNotificationsEnabled(value);
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0005433C File Offset: 0x0005253C
		protected Dictionary<string, string> UserData(bool hasImage)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("user", "data");
			if (hasImage)
			{
				dictionary.Add("image_url", "https://thecatapi.com/api/images/get?format=src&type=png&size=med");
			}
			return dictionary;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00054374 File Offset: 0x00052574
		protected List<Button> Buttons(bool hasButtons)
		{
			if (!hasButtons)
			{
				return null;
			}
			return new List<Button>
			{
				new Button("Open App", new Dictionary<string, string>
				{
					{
						"button",
						"first"
					}
				}),
				new Button("Open URL", new Dictionary<string, string>
				{
					{
						"open_url",
						"https://assetstore.unity.com/packages/tools/utnotifications-professional-local-push-notification-plugin-37767"
					},
					{
						"button",
						"second"
					}
				})
			};
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000543E5 File Offset: 0x000525E5
		protected void OnInitialized()
		{
			Manager.Instance.SetBadge(0);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x000543F4 File Offset: 0x000525F4
		protected void SendRegistrationId(string providerName, string registrationId)
		{
			Manager.Instance.SubscribeToTopic("all");
			string userId = SampleUtils.GenerateDeviceUniqueIdentifier();
			base.StartCoroutine(this.SendRegistrationId(userId, providerName, registrationId));
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00054426 File Offset: 0x00052626
		protected void OnPushRegistrationFailed(string error)
		{
			Debug.LogError(error);
			this.InitializeText.text = this.initializeTextOriginal + "\nPush Registration Failed: " + error;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0005444A File Offset: 0x0005264A
		protected IEnumerator SendRegistrationId(string userId, string providerName, string registrationId)
		{
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("uid", userId);
			wwwform.AddField("provider", providerName);
			wwwform.AddField("id", registrationId);
			bool finished = false;
			base.StartCoroutine(UTNotificationsSample.HttpRequest(this.DemoServerURLInputField.text + "/register", wwwform, delegate(string successText)
			{
				finished = true;
				this.InitializeText.text = this.initializeTextOriginal + "\n" + successText;
			}, delegate(string errorText, string error)
			{
				finished = true;
				this.InitializeText.text = string.Concat(new string[]
				{
					this.initializeTextOriginal,
					"\nError Sending Push Registration Id to DemoServer:\n",
					error,
					" ",
					errorText
				});
				if (!UTNotificationsSample.CheckAndroidCleartextAllowed(new Uri(this.DemoServerURLInputField.text).Host))
				{
					Text initializeText = this.InitializeText;
					initializeText.text = initializeText.text + "\n" + UTNotificationsSample.CleartextHint;
				}
			}));
			int dots = 0;
			do
			{
				string text = this.initializeTextOriginal + "\nSending registrationId";
				dots = (dots + 1) % 4;
				for (int i = 0; i < dots; i++)
				{
					text += ".";
				}
				this.InitializeText.text = text;
				yield return new WaitForSeconds(0.15f);
			}
			while (!finished);
			yield break;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0005446E File Offset: 0x0005266E
		private static IEnumerator HttpRequest(string uri, WWWForm wwwForm, UnityAction<string> onSuccess, UnityAction<string, string> onError)
		{
			using (UnityWebRequest webRequest = (wwwForm != null) ? UnityWebRequest.Post(uri, wwwForm) : UnityWebRequest.Get(uri))
			{
				yield return webRequest.SendWebRequest();
				if (webRequest.isNetworkError || webRequest.isHttpError)
				{
					onError(webRequest.downloadHandler.text, webRequest.error);
				}
				else
				{
					onSuccess(webRequest.downloadHandler.text);
				}
			}
			UnityWebRequest webRequest = null;
			yield break;
			yield break;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00054492 File Offset: 0x00052692
		private string EscapeURL(string stringToUrlEscape)
		{
			return UnityWebRequest.EscapeURL(stringToUrlEscape);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0000900B File Offset: 0x0000720B
		private static bool CheckAndroidCleartextAllowed(string hostname)
		{
			return true;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0005449A File Offset: 0x0005269A
		protected void OnNotificationClicked(ReceivedNotification notification)
		{
			this.NotificationDetailsDialog.OnClicked(notification);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000544A8 File Offset: 0x000526A8
		protected void OnNotificationsReceived(IList<ReceivedNotification> receivedNotifications)
		{
			foreach (ReceivedNotification received in receivedNotifications)
			{
				this.NotificationDetailsDialog.OnReceived(received);
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000544F8 File Offset: 0x000526F8
		private void Awake()
		{
			if (UTNotificationsSample.instance != null)
			{
				throw new UnityException("Creating the second instance of UTNotificationsSample...");
			}
			UTNotificationsSample.instance = this;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00054518 File Offset: 0x00052718
		private void Start()
		{
			MoreButton moreButton = MoreButton.FindInstance();
			if (moreButton != null)
			{
				MoreButton moreButton2 = moreButton;
				MoreButton.PopupMenuItem[] array = new MoreButton.PopupMenuItem[1];
				array[0] = new MoreButton.PopupMenuItem("EXIT", delegate()
				{
					Application.Quit();
				});
				moreButton2.MenuItems = array;
			}
			this.notifyAllTextOriginal = this.NotifyAllText.text;
			this.initializeTextOriginal = this.InitializeText.text;
			this.NotificationsEnabledToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnNotificationsEnabledToggleValueChanged));
			Manager manager = Manager.Instance;
			manager.OnInitialized += this.OnInitialized;
			manager.OnSendRegistrationId += this.SendRegistrationId;
			manager.OnPushRegistrationFailed += this.OnPushRegistrationFailed;
			manager.OnNotificationClicked += this.OnNotificationClicked;
			manager.OnNotificationsReceived += this.OnNotificationsReceived;
			if (this.DemoServerURLInputField.IsValid())
			{
				this.Initialize();
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0005461F File Offset: 0x0005281F
		private void Update()
		{
			if (Manager.Instance.Initialized)
			{
				this.NotificationsEnabledToggle.isOn = Manager.Instance.NotificationsEnabled();
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00054650 File Offset: 0x00052850
		private void OnDestroy()
		{
			Manager manager = Manager.Instance;
			if (manager != null)
			{
				manager.OnInitialized -= this.OnInitialized;
				manager.OnSendRegistrationId -= this.SendRegistrationId;
				manager.OnPushRegistrationFailed -= this.OnPushRegistrationFailed;
				manager.OnNotificationClicked -= this.OnNotificationClicked;
				manager.OnNotificationsReceived -= this.OnNotificationsReceived;
			}
			if (UTNotificationsSample.instance == this)
			{
				UTNotificationsSample.instance = null;
			}
		}

		// Token: 0x04000CFB RID: 3323
		public ValidatedInputField DemoServerURLInputField;

		// Token: 0x04000CFC RID: 3324
		public Text NotifyAllText;

		// Token: 0x04000CFD RID: 3325
		public Text InitializeText;

		// Token: 0x04000CFE RID: 3326
		public Toggle NotificationsEnabledToggle;

		// Token: 0x04000CFF RID: 3327
		public CreateNotificationDialog CreateNotificationDialog;

		// Token: 0x04000D00 RID: 3328
		public NotificationDetailsDialog NotificationDetailsDialog;

		// Token: 0x04000D01 RID: 3329
		private static UTNotificationsSample instance = null;

		// Token: 0x04000D02 RID: 3330
		private static readonly string CleartextHint = "Please enable cleartext HTTP traffic in Assets/Plugins/AndroidManifest.xml.";

		// Token: 0x04000D03 RID: 3331
		private string notifyAllTextOriginal;

		// Token: 0x04000D04 RID: 3332
		private string initializeTextOriginal;
	}
}
