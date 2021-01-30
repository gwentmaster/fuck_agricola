using System;
using System.Runtime.InteropServices;
using AsmodeeNet.Analytics;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class InGameNetworkListener : MonoBehaviour
{
	// Token: 0x06000A2B RID: 2603 RVA: 0x000438F8 File Offset: 0x00041AF8
	public void HandleExitAfterNetworkDisconnect()
	{
		if (this.m_networkMessagePopup != null)
		{
			this.m_networkMessagePopup.SetActive(false);
		}
		ScreenManager.s_onStartScreen = "OnlineLobby";
		AgricolaLib.ExitCurrentGame();
		AgricolaGame.HandleLeaveGameAnalytics("network_error", -1);
		ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0004394C File Offset: 0x00041B4C
	public void HandleNextGameButtonPressed()
	{
		if (this.m_nextGameWaitingID != 0U)
		{
			AgricolaGame.HandleLeaveGameAnalytics("next_game", -1);
			AgricolaLib.NetworkResumeGame((int)this.m_nextGameWaitingID);
			GCHandle gchandle = GCHandle.Alloc(new byte[Marshal.SizeOf(typeof(ShortSaveStruct))], GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			AgricolaLib.NetworkGetActiveGameWithID(intPtr, 1, this.m_nextGameWaitingID);
			ShortSaveStruct shortSaveStruct = (ShortSaveStruct)Marshal.PtrToStructure(intPtr, typeof(ShortSaveStruct));
			string matchSessionId = "Agr_" + this.m_nextGameWaitingID.ToString();
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)shortSaveStruct.deckFlags);
			string launchMethod = "resume";
			if (!AgricolaLib.NetworkGameHasLocalPlayerMoves(this.m_nextGameWaitingID) && shortSaveStruct.roundNumber <= 1)
			{
				if (shortSaveStruct.rematchGameID != 0)
				{
					launchMethod = "rematch";
				}
				else if ((long)shortSaveStruct.soloGameCurrentScore == (long)((ulong)Network.NetworkGetLocalID()))
				{
					launchMethod = "create";
				}
				else
				{
					launchMethod = "join";
				}
			}
			AnalyticsEvents.LogMatchStartEvent(matchSessionId, string.Empty, "online", string.Empty, activatedDlc, (int)(shortSaveStruct.packedPlayerCount >> 16 & 65535U), 0, null, launchMethod, new int?((int)shortSaveStruct.updateTime), string.Empty, true, false, new bool?(true), null, null, null, null, null, null);
			gchandle.Free();
			ScreenManager.instance.LoadIntoGameScreen(1);
		}
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00043AB8 File Offset: 0x00041CB8
	private void Start()
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
		if (this.m_nextGameController != null)
		{
			this.m_nextGameController.SetNextGameButtonVisible(false);
		}
		if (this.m_gameWaitingIcon != null)
		{
			this.m_gameWaitingIcon.SetActive(false);
		}
		if (this.m_gameWaitingIconMobile != null)
		{
			this.m_gameWaitingIconMobile.SetActive(false);
		}
		if (this.m_networkMessagePopup != null)
		{
			this.m_networkMessagePopup.SetActive(false);
		}
		this.m_nextGameWaitingID = 0U;
		this.m_activeGameWaitingIcon = this.m_gameWaitingIcon;
		this.m_AvailableScenarioFlags = 0U;
		this.m_AvailableAdditionalCardFlags = 0U;
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00043B71 File Offset: 0x00041D71
	private void OnDestroy()
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00043B98 File Offset: 0x00041D98
	private void Update()
	{
		if (Network.m_Network == null)
		{
			if (this.m_activeGameWaitingIcon != null)
			{
				this.m_activeGameWaitingIcon.SetActive(false);
			}
			if (this.m_nextGameController != null)
			{
				this.m_nextGameController.SetNextGameButtonVisible(false);
				return;
			}
		}
		else if (Network.m_Network.m_bConnectedToServer)
		{
			this.m_nextGameWaitingID = AgricolaLib.NetworkGetNextActiveGameID(this.m_AvailableScenarioFlags, this.m_AvailableAdditionalCardFlags);
			if (this.m_nextGameController != null)
			{
				this.m_nextGameController.SetNextGameButtonVisible(this.m_nextGameWaitingID > 0U);
			}
			if (AgricolaLib.NetworkGetGameWaitingCount() > 1)
			{
				if (this.m_activeGameWaitingIcon != null)
				{
					this.m_activeGameWaitingIcon.SetActive(true);
					return;
				}
			}
			else if (this.m_activeGameWaitingIcon != null)
			{
				this.m_activeGameWaitingIcon.SetActive(this.m_nextGameWaitingID > 0U);
			}
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00043C74 File Offset: 0x00041E74
	private void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (eventType == NetworkEvent.EventType.Event_ConnectionLost && this.m_networkMessagePopup != null)
		{
			this.m_networkMessagePopup.SetActive(true);
		}
	}

	// Token: 0x04000AB8 RID: 2744
	public HudSideController_Right m_nextGameController;

	// Token: 0x04000AB9 RID: 2745
	public GameObject m_gameWaitingIcon;

	// Token: 0x04000ABA RID: 2746
	public GameObject m_gameWaitingIconMobile;

	// Token: 0x04000ABB RID: 2747
	public GameObject m_networkMessagePopup;

	// Token: 0x04000ABC RID: 2748
	public GameObject m_base;

	// Token: 0x04000ABD RID: 2749
	private GameObject m_activeGameWaitingIcon;

	// Token: 0x04000ABE RID: 2750
	private uint m_nextGameWaitingID;

	// Token: 0x04000ABF RID: 2751
	private uint m_AvailableScenarioFlags;

	// Token: 0x04000AC0 RID: 2752
	private uint m_AvailableAdditionalCardFlags;
}
