using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class UI_Connecting : UI_NetworkBase
{
	// Token: 0x06000A6B RID: 2667 RVA: 0x00045354 File Offset: 0x00043554
	public void OnEnterMenu()
	{
		if (this.m_bHandlePopup)
		{
			this.m_bHandlePopup = false;
			base.StartCoroutine(this.HandleBackFromPopup());
			return;
		}
		this.m_serverError = EServerError.E_NO_ERROR;
		this.m_bGoBackToLogin = false;
		this.m_bDisplayingServerMessage = false;
		if (string.Compare(ScreenManager.s_onStartScreen, "Connecting") == 0)
		{
			ScreenManager.s_onStartScreen = "OnlineLobby";
			this.m_bGoBackToLogin = true;
		}
		this.m_bCreatingAccount = AgricolaLib.NetworkIsCreatingAccount();
		this.m_bServerConnectionWasLost = Network.m_Network.m_bServerConnectionLost;
		if (this.m_connectingMessageText != null)
		{
			this.m_connectingMessageText.KeyText = (this.m_bCreatingAccount ? "${Key_CreatingNewAccount}" : "${Key_Connect}");
		}
		if (this.m_bServerConnectionWasLost)
		{
			Network.RetryLogin();
		}
		this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDisplayAmount));
		this.m_connectCoroutine = base.StartCoroutine(this.ProcessConnection(this.m_connectionTimeout, this.m_connectionRetryTime));
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x00045440 File Offset: 0x00043640
	public void OnExitMenu(bool bUnderPopup)
	{
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00045444 File Offset: 0x00043644
	protected override void NetworkOnDestroy()
	{
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		if (this.m_connectCoroutine != null)
		{
			base.StopCoroutine(this.m_connectCoroutine);
			this.m_connectCoroutine = null;
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00003022 File Offset: 0x00001222
	public void OnSelect(bool bConfirm)
	{
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0004547C File Offset: 0x0004367C
	private IEnumerator HandleBackFromPopup()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (this.m_bGoBackToLogin)
		{
			ScreenManager.instance.GoToScene("OnlineLogin", true);
		}
		else
		{
			ScreenManager.instance.PopScene();
		}
		yield break;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0004548B File Offset: 0x0004368B
	private IEnumerator ProcessDelayTime(float totalDelayTime)
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		this.m_bMinDisplayAmountReached = false;
		this.m_bGoToOnlineLobby = false;
		while (accumulatedTime < totalDelayTime)
		{
			accumulatedTime += Time.time - previousTime;
			previousTime = Time.time;
			yield return new WaitForEndOfFrame();
		}
		this.m_bMinDisplayAmountReached = true;
		if (this.m_bGoToOnlineLobby)
		{
			if (this.m_bServerConnectionWasLost)
			{
				ScreenManager.instance.PopScene();
			}
			else
			{
				ScreenManager.instance.GoToScene("OnlineLobby", true);
			}
		}
		else if (this.m_serverError != EServerError.E_NO_ERROR)
		{
			this.DisplayServerMessages(this.m_serverError);
		}
		yield break;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x000454A1 File Offset: 0x000436A1
	private IEnumerator ProcessConnection(float connectTimeout, float connectRetryTime)
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		if (Network.IsCreateAccount())
		{
			Network.Connect();
		}
		else
		{
			yield return Network.RequestAsmodeeAccessToken();
			if (!Network.HasAsmodeeAccessToken())
			{
				this.DisplayServerMessages(this.m_bServerConnectionWasLost ? EServerError.E_NETWORK_LOST_CONNECTION_UNK : EServerError.E_NETWORK_INCORRECT_PASSWORD);
				yield return null;
			}
			yield return Network.RequestAsmodeeAccountInformation();
			Network.ConnectAsmodee();
		}
		while (accumulatedTime < connectTimeout)
		{
			float num = Time.time - previousTime;
			accumulatedTime += num;
			previousTime = Time.time;
			yield return new WaitForEndOfFrame();
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(new UI_ConfirmPopup.ClickCallback(this.OnSelect), "Key_FailConnect", UI_ConfirmPopup.ButtonFormat.OneButton);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
		this.m_bGoBackToLogin = true;
		yield return new WaitForEndOfFrame();
		yield break;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x000454B8 File Offset: 0x000436B8
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (eventType == NetworkEvent.EventType.Event_LoginError)
		{
			this.DisplayServerMessages((EServerError)eventData);
			return;
		}
		if (eventType == NetworkEvent.EventType.Event_LoginComplete)
		{
			PlayerPrefs.SetInt("AsmodeeAccountLink", 1);
			Network.m_Network.m_bLoggedInToServer = true;
			UI_NetworkBase.m_localUserID = (uint)eventData;
			base.RequestLocalPlayerProfile(null);
			if (this.m_connectCoroutine != null)
			{
				base.StopCoroutine(this.m_connectCoroutine);
				this.m_connectCoroutine = null;
			}
			if (!this.m_bMinDisplayAmountReached)
			{
				this.m_bGoToOnlineLobby = true;
				return;
			}
			if (this.m_bServerConnectionWasLost)
			{
				ScreenManager.instance.PopScene();
				return;
			}
			ScreenManager.instance.GoToScene("OnlineLobby", true);
			return;
		}
		else
		{
			if (eventType == NetworkEvent.EventType.Event_CreateAccountReply)
			{
				this.DisplayServerMessages((EServerError)eventData);
				return;
			}
			if (eventType == NetworkEvent.EventType.Event_UpdatedPlayerProfile)
			{
				base.HandleUpdatedPlayerProfileEvent(eventData);
			}
			return;
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x00045560 File Offset: 0x00043760
	private void DisplayServerMessages(EServerError serverError)
	{
		if (this.m_bDisplayingServerMessage)
		{
			return;
		}
		if (this.m_connectCoroutine != null)
		{
			base.StopCoroutine(this.m_connectCoroutine);
			this.m_connectCoroutine = null;
		}
		if (!this.m_bMinDisplayAmountReached)
		{
			this.m_serverError = serverError;
			return;
		}
		string messageKey = "";
		switch (serverError)
		{
		case EServerError.E_NETWORK_VERSION_MISMATCH:
			messageKey = "Key_Update";
			break;
		case EServerError.E_NETWORK_DUPLICATE_EMAIL:
			messageKey = "Key_EmailInUse";
			break;
		case EServerError.E_NETWORK_DUPLICATE_USERNAME:
			messageKey = "Key_InvalidUsername";
			break;
		case EServerError.E_NETWORK_ACCOUNT_CREATION_FAILED:
			messageKey = "Key_AccountCreateFailed";
			break;
		case EServerError.E_NETWORK_ACCOUNT_ERROR:
			messageKey = "Key_AccountCreateError";
			break;
		case EServerError.E_NETWORK_ACCOUNT_NOT_ACTIVATED:
			messageKey = "Key_AccountNotActivated";
			break;
		case EServerError.E_NETWORK_INCORRECT_PASSWORD:
			messageKey = "Key_InvalidPassword";
			this.m_bGoBackToLogin = true;
			break;
		case EServerError.E_NETWORK_ACCOUNT_CREATION_SUCCESS:
			messageKey = "Key_AccountCreateSuccess";
			this.m_bGoBackToLogin = true;
			break;
		case EServerError.E_NETWORK_ACCOUNT_CREATION_PENDING:
			messageKey = "Key_CheckYourEmail";
			this.m_bGoBackToLogin = true;
			break;
		case EServerError.E_NETWORK_LOST_CONNECTION_UNK:
			messageKey = "Key_LostConnectionUnk";
			this.m_bGoBackToLogin = true;
			break;
		default:
			Debug.LogWarning("ERROR: Unknown server error " + serverError);
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(new UI_ConfirmPopup.ClickCallback(this.OnSelect), messageKey, UI_ConfirmPopup.ButtonFormat.OneButton);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
		this.m_bDisplayingServerMessage = true;
	}

	// Token: 0x04000B08 RID: 2824
	public GameObject m_connectingMessageNode;

	// Token: 0x04000B09 RID: 2825
	public UILocalizedText m_connectingMessageText;

	// Token: 0x04000B0A RID: 2826
	public float m_connectionTimeout;

	// Token: 0x04000B0B RID: 2827
	public float m_connectionRetryTime;

	// Token: 0x04000B0C RID: 2828
	public float m_minDisplayAmount;

	// Token: 0x04000B0D RID: 2829
	private Coroutine m_delayCoroutine;

	// Token: 0x04000B0E RID: 2830
	private Coroutine m_connectCoroutine;

	// Token: 0x04000B0F RID: 2831
	private EServerError m_serverError;

	// Token: 0x04000B10 RID: 2832
	private bool m_bMinDisplayAmountReached;

	// Token: 0x04000B11 RID: 2833
	private bool m_bGoToOnlineLobby;

	// Token: 0x04000B12 RID: 2834
	private bool m_bCreatingAccount;

	// Token: 0x04000B13 RID: 2835
	private bool m_bServerConnectionWasLost;

	// Token: 0x04000B14 RID: 2836
	private bool m_bGoBackToLogin;

	// Token: 0x04000B15 RID: 2837
	private bool m_bDisplayingServerMessage;

	// Token: 0x04000B16 RID: 2838
	private bool m_bHandlePopup;
}
