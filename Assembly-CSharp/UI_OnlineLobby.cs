using System;
using TMPro;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class UI_OnlineLobby : UI_NetworkBase
{
	// Token: 0x06000B3C RID: 2876 RVA: 0x0004E158 File Offset: 0x0004C358
	public void OnEnterMenu()
	{
		if (this.m_backgroundAnimator != null && this.m_backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
		{
			this.m_backgroundAnimator.Play("Reset_OnlineLobby");
		}
		if (!Network.m_Network.m_bConnectedToServer)
		{
			base.TryNetworkReconnect();
			return;
		}
		if (ScreenManager.s_onStartScreen == "OnlineLobby")
		{
			base.RequestLocalPlayerProfile(null);
		}
		this.DisplayLocalPlayerProfile(UI_NetworkBase.m_localPlayerProfile.userID == UI_NetworkBase.m_localUserID);
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x00003022 File Offset: 0x00001222
	public void OnExitMenu(bool bUnderPopup)
	{
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0004E1E1 File Offset: 0x0004C3E1
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_UpdatedPlayerProfile && (long)eventData == (long)((ulong)UI_NetworkBase.m_localUserID))
		{
			this.DisplayLocalPlayerProfile(true);
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x00003022 File Offset: 0x00001222
	private void Update()
	{
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0004E204 File Offset: 0x0004C404
	private void DisplayLocalPlayerProfile(bool bVisible)
	{
		this.m_userNameLabel.enabled = bVisible;
		this.m_userRatingLabel.enabled = bVisible;
		if (bVisible)
		{
			this.m_userNameLabel.text = UI_NetworkBase.m_localPlayerProfile.displayName;
			this.m_userRatingLabel.text = UI_NetworkBase.m_localPlayerProfile.userGameStats.userRating.ToString();
			if (this.m_gamesCompletedNumLabel != null)
			{
				this.m_gamesCompletedNumLabel.text = UI_NetworkBase.m_localPlayerProfile.userGameStats.completedGames.ToString();
			}
			if (this.m_inProgressNumLabel != null)
			{
				this.m_inProgressNumLabel.text = UI_NetworkBase.m_localPlayerProfile.inProgressGames.ToString();
			}
			if (this.m_forfeitsNumLabel != null)
			{
				this.m_forfeitsNumLabel.text = UI_NetworkBase.m_localPlayerProfile.userGameStats.forfeits.ToString();
			}
			if (this.m_2PlayerRecordLabel != null)
			{
				this.m_2PlayerRecordLabel.text = string.Format("{0} - {1}", UI_NetworkBase.m_localPlayerProfile.userGameStats.wins[0], UI_NetworkBase.m_localPlayerProfile.userGameStats.losses[0]);
			}
			if (this.m_3PlayerRecordLabel != null)
			{
				this.m_3PlayerRecordLabel.text = string.Format("{0} - {1}", UI_NetworkBase.m_localPlayerProfile.userGameStats.wins[1], UI_NetworkBase.m_localPlayerProfile.userGameStats.losses[1]);
			}
			if (this.m_4PlayerRecordLabel != null)
			{
				this.m_4PlayerRecordLabel.text = string.Format("{0} - {1}", UI_NetworkBase.m_localPlayerProfile.userGameStats.wins[2], UI_NetworkBase.m_localPlayerProfile.userGameStats.losses[2]);
			}
			if (this.m_5PlayerRecordLabel != null)
			{
				this.m_5PlayerRecordLabel.text = string.Format("{0} - {1}", UI_NetworkBase.m_localPlayerProfile.userGameStats.wins[3], UI_NetworkBase.m_localPlayerProfile.userGameStats.losses[3]);
			}
			if (this.m_6PlayerRecordLabel != null)
			{
				this.m_6PlayerRecordLabel.text = string.Format("{0} - {1}", UI_NetworkBase.m_localPlayerProfile.userGameStats.wins[4], UI_NetworkBase.m_localPlayerProfile.userGameStats.losses[4]);
				return;
			}
		}
		else
		{
			base.RequestLocalPlayerProfile(null);
		}
	}

	// Token: 0x04000BFD RID: 3069
	public Animator m_backgroundAnimator;

	// Token: 0x04000BFE RID: 3070
	public TextMeshProUGUI m_userNameLabel;

	// Token: 0x04000BFF RID: 3071
	public TextMeshProUGUI m_userRatingLabel;

	// Token: 0x04000C00 RID: 3072
	public TextMeshProUGUI m_gamesCompletedNumLabel;

	// Token: 0x04000C01 RID: 3073
	public TextMeshProUGUI m_forfeitsNumLabel;

	// Token: 0x04000C02 RID: 3074
	public TextMeshProUGUI m_inProgressNumLabel;

	// Token: 0x04000C03 RID: 3075
	public TextMeshProUGUI m_2PlayerRecordLabel;

	// Token: 0x04000C04 RID: 3076
	public TextMeshProUGUI m_3PlayerRecordLabel;

	// Token: 0x04000C05 RID: 3077
	public TextMeshProUGUI m_4PlayerRecordLabel;

	// Token: 0x04000C06 RID: 3078
	public TextMeshProUGUI m_5PlayerRecordLabel;

	// Token: 0x04000C07 RID: 3079
	public TextMeshProUGUI m_6PlayerRecordLabel;
}
