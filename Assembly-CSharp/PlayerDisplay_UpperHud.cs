using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000069 RID: 105
public class PlayerDisplay_UpperHud : MonoBehaviour
{
	// Token: 0x06000556 RID: 1366 RVA: 0x00028BE4 File Offset: 0x00026DE4
	public void Init()
	{
		base.StopAllCoroutines();
		for (int i = 0; i < this.m_playerTokens.Length; i++)
		{
			this.m_playerTokens[i].playerIndex = 0;
			this.m_playerTokens[i].playerInstanceID = 0;
			if (this.m_playerTokens[i].root != null)
			{
				this.m_playerTokens[i].root.SetActive(false);
			}
		}
		if (this.m_gameController == null)
		{
			this.m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		}
		if (this.m_dataBuffer == null)
		{
			this.m_dataBuffer = new byte[1024];
			this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
			this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		}
		if (this.m_LocalPlayerTimerObj != null)
		{
			this.m_LocalPlayerTimerObj.SetActive(AgricolaLib.GetIsOnlineGame());
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00028CDA File Offset: 0x00026EDA
	private void OnDestroy()
	{
		if (this.m_dataBuffer != null)
		{
			this.m_hDataBuffer.Free();
			this.m_dataBuffer = null;
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00028CF6 File Offset: 0x00026EF6
	public void SetLocalPlayerName(string playerName)
	{
		if (this.m_localPlayerName != null)
		{
			this.m_localPlayerName.text = playerName;
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00028D14 File Offset: 0x00026F14
	public GameObject FindPlayerTokenByInstanceID(int playerInstanceID)
	{
		if (this.m_playerTokens != null)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerInstanceID == playerInstanceID)
				{
					return this.m_playerTokens[i].root;
				}
			}
		}
		return null;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00028D64 File Offset: 0x00026F64
	public int FindAvatarIndexByInstanceID(int playerInstanceID)
	{
		if (this.m_playerTokens != null)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerInstanceID == playerInstanceID)
				{
					return this.m_playerTokens[i].avatar.GetIndex();
				}
			}
		}
		return 0;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00028DB8 File Offset: 0x00026FB8
	public int FindFactionIndexByInstanceID(int playerInstanceID)
	{
		if (this.m_playerTokens != null)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerInstanceID == playerInstanceID)
				{
					return this.m_playerTokens[i].factionIndex;
				}
			}
		}
		return 0;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00028E08 File Offset: 0x00027008
	public int FindPlayerIndexByInstanceID(int playerInstanceID)
	{
		if (this.m_playerTokens != null)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerInstanceID == playerInstanceID)
				{
					return this.m_playerTokens[i].playerIndex;
				}
			}
		}
		return 0;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00028E58 File Offset: 0x00027058
	public int FindPlayerTurnOrderByPlayerIndex(int playerIndex)
	{
		if (this.m_playerTokens != null)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerIndex == playerIndex)
				{
					return this.m_playerTokens[i].root.transform.GetSiblingIndex() + 1;
				}
			}
		}
		return 0;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00028EB4 File Offset: 0x000270B4
	public bool AddPlayer(int playerIndex, int playerInstanceID)
	{
		int num = -1;
		for (int i = 0; i < this.m_playerTokens.Length; i++)
		{
			if (this.m_playerTokens[i].playerIndex == playerIndex)
			{
				return false;
			}
			if (this.m_playerTokens[i].playerIndex == 0 && num == -1)
			{
				num = i;
			}
		}
		if (num == -1)
		{
			return false;
		}
		if (num == 0 && AgricolaLib.GetIsOnlineGame())
		{
			base.StartCoroutine(this.UpdateOnlineStatus());
		}
		if (this.m_playerTokens[num].onlineIndicatorBase != null)
		{
			this.m_playerTokens[num].onlineIndicatorBase.SetActive(AgricolaLib.GetIsOnlineGame() && playerIndex != AgricolaLib.GetLocalPlayerIndex());
		}
		AgricolaLib.GetGamePlayerFarmState(playerIndex, this.m_bufPtr, 1024);
		GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
		if (this.m_playerTokens[num].root != null)
		{
			this.m_playerTokens[num].root.SetActive(true);
		}
		if (this.m_playerTokens[num].colorizer != null)
		{
			this.m_playerTokens[num].colorizer.Colorize((uint)gamePlayerFarmState.playerFaction);
		}
		if (this.m_playerTokens[num].avatar != null)
		{
			this.m_playerTokens[num].avatar.SetAvatar(gamePlayerFarmState.workerAvatarIDs[0], true);
		}
		this.m_playerTokens[num].playerIndex = playerIndex;
		this.m_playerTokens[num].playerInstanceID = playerInstanceID;
		this.m_playerTokens[num].factionIndex = gamePlayerFarmState.playerFaction;
		this.ReorderTokensBasedOnTurnOrder();
		return true;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00029074 File Offset: 0x00027274
	public void ReorderTokensBasedOnTurnOrder()
	{
		int startingPlayerIndex = AgricolaLib.GetStartingPlayerIndex();
		int num = -1;
		if (startingPlayerIndex == 0)
		{
			return;
		}
		for (int i = 0; i < this.m_playerTokens.Length; i++)
		{
			if (this.m_playerTokens[i].playerIndex == startingPlayerIndex)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			int num2 = num;
			do
			{
				this.m_playerTokens[num2++].root.transform.SetAsLastSibling();
				if (num2 >= this.m_playerTokens.Length)
				{
					num2 = 0;
				}
			}
			while (num2 != num);
		}
		for (int j = 0; j < this.m_playerTokens.Length; j++)
		{
			if (!this.m_playerTokens[j].root.activeSelf)
			{
				this.m_playerTokens[j].root.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0002913C File Offset: 0x0002733C
	public void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (animation_manager == null)
		{
			return;
		}
		for (int i = 0; i < this.m_playerTokens.Length; i++)
		{
			if (this.m_playerTokens[i].playerInstanceID != 0 && this.m_playerTokens[i].root != null)
			{
				AgricolaAnimationLocator component = this.m_playerTokens[i].root.GetComponent<AgricolaAnimationLocator>();
				if (component != null)
				{
					animation_manager.SetAnimationLocator(16, this.m_playerTokens[i].playerInstanceID, component);
					animation_manager.SetAnimationLocator(1, this.m_playerTokens[i].playerInstanceID, component);
					if (this.m_gameController != null && this.m_gameController.GetLocalPlayerInstanceID() != this.m_playerTokens[i].playerInstanceID)
					{
						animation_manager.SetAnimationLocator(19, this.m_playerTokens[i].playerInstanceID, component);
					}
				}
			}
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0002923C File Offset: 0x0002743C
	public void OnPlayerTokenClick(int index)
	{
		if (this.m_gameController != null && this.m_gameController.GetAnimationManager().GetHasAnimatingObject())
		{
			return;
		}
		if (index < this.m_playerTokens.Length && this.m_playerTokens[index].playerIndex != 0 && this.m_gameController != null)
		{
			this.m_gameController.SetFarmToPlayer(this.m_playerTokens[index].playerIndex, this.m_playerTokens[index].playerInstanceID);
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x000292C4 File Offset: 0x000274C4
	private void Update()
	{
		int currentRound = AgricolaLib.GetCurrentRound();
		int currentHarvestMode = AgricolaLib.GetCurrentHarvestMode();
		if (this.m_roundNumber != currentRound || this.m_harvestMode != currentHarvestMode)
		{
			this.m_roundNumber = currentRound;
			this.m_harvestMode = currentHarvestMode;
			int num = 1;
			if (currentRound >= 5 && currentRound <= 7)
			{
				num = 2;
			}
			else if (currentRound >= 8 && currentRound <= 9)
			{
				num = 3;
			}
			else if (currentRound >= 10 && currentRound <= 11)
			{
				num = 4;
			}
			else if (currentRound >= 12 && currentRound <= 13)
			{
				num = 5;
			}
			else if (currentRound == 14)
			{
				num = 6;
			}
			if (this.m_StageRoundText != null)
			{
				this.m_StageRoundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Stage_Shorthand}") + " " + num.ToString() + " ";
				if (currentHarvestMode == 0)
				{
					TextMeshProUGUI stageRoundText = this.m_StageRoundText;
					stageRoundText.text = stageRoundText.text + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}") + " " + currentRound.ToString();
				}
				else if (currentHarvestMode <= 2)
				{
					TextMeshProUGUI stageRoundText2 = this.m_StageRoundText;
					stageRoundText2.text += LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_HarvestField}");
				}
				else if (currentHarvestMode == 3)
				{
					TextMeshProUGUI stageRoundText3 = this.m_StageRoundText;
					stageRoundText3.text += LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_HarvestFeeding}");
				}
				else if (currentHarvestMode >= 4)
				{
					TextMeshProUGUI stageRoundText4 = this.m_StageRoundText;
					stageRoundText4.text += LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_HarvestBreeding}");
				}
			}
			this.m_StageRoundIcons[0].gameObject.SetActive(num == 1);
			this.m_StageRoundIcons[1].gameObject.SetActive(num <= 2);
			this.m_StageRoundIcons[2].gameObject.SetActive(num <= 5);
			if (currentHarvestMode == -1)
			{
				for (int i = 0; i < this.m_StageRoundIcons.Length; i++)
				{
					this.m_StageRoundIcons[i].color = this.m_inactiveRoundIconColor;
				}
			}
			else
			{
				this.m_StageRoundIcons[0].color = ((currentRound == 1) ? Color.white : this.m_inactiveRoundIconColor);
				if (currentRound == 2 || currentRound == 5)
				{
					this.m_StageRoundIcons[1].color = Color.white;
				}
				else
				{
					this.m_StageRoundIcons[1].color = this.m_inactiveRoundIconColor;
				}
				if (currentRound == 3 || currentRound == 6 || currentRound == 8 || currentRound == 10 || currentRound == 12)
				{
					this.m_StageRoundIcons[2].color = Color.white;
				}
				else
				{
					this.m_StageRoundIcons[2].color = this.m_inactiveRoundIconColor;
				}
				if (currentRound == 4 || currentRound == 7 || currentRound == 9 || currentRound == 11 || currentRound >= 13)
				{
					this.m_StageRoundIcons[3].color = Color.white;
				}
				else
				{
					this.m_StageRoundIcons[3].color = this.m_inactiveRoundIconColor;
				}
			}
		}
		for (int j = 0; j < this.m_playerTokens.Length; j++)
		{
			if (this.m_playerTokens[j].playerIndex != 0)
			{
				if (!AgricolaLib.GetIsOnlineGame() && this.m_playerTokens[j].aiThinkingIndicator != null)
				{
					AgricolaLib.GetGamePlayerAIState(this.m_playerTokens[j].playerIndex, this.m_bufPtr, 1024);
					GamePlayerAIState gamePlayerAIState = (GamePlayerAIState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerAIState));
					this.m_playerTokens[j].aiThinkingIndicator.SetActive(gamePlayerAIState.isAIThinking == 1);
				}
				AgricolaLib.GetGamePlayerFarmState(this.m_playerTokens[j].playerIndex, this.m_bufPtr, 1024);
				GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
				for (int k = 0; k < this.m_playerTokens[j].m_workerBaseNodes.Length; k++)
				{
					if (this.m_playerTokens[j].m_workerBaseNodes[k] != null)
					{
						this.m_playerTokens[j].m_workerBaseNodes[k].SetActive(gamePlayerFarmState.workerCount > k);
					}
					if (this.m_playerTokens[j].m_workerAvailableNodes[k] != null)
					{
						this.m_playerTokens[j].m_workerAvailableNodes[k].SetActive((gamePlayerFarmState.unusedWorkerFlags & 1 << k) != 0);
					}
				}
				if (this.m_playerTokens[j].startingPlayerRootNode != null)
				{
					this.m_playerTokens[j].startingPlayerRootNode.SetActive(gamePlayerFarmState.isNextStartPlayer != 0 || gamePlayerFarmState.harvestPhase > 0);
				}
				if (this.m_playerTokens[j].startingPlayerNode != null)
				{
					this.m_playerTokens[j].startingPlayerNode.SetActive(gamePlayerFarmState.isNextStartPlayer > 0);
				}
				if (this.m_playerTokens[j].harvestPlayerNodes != null && this.m_playerTokens[j].harvestPlayerNodes.Length == 3)
				{
					for (int l = 0; l < 3; l++)
					{
						this.m_playerTokens[j].harvestPlayerNodes[l].SetActive((int)gamePlayerFarmState.harvestPhase == l + 1);
					}
				}
			}
		}
		if (AgricolaLib.GetIsOnlineGame() && this.m_LocalPlayerTimer != null)
		{
			AgricolaLib.GetGamePlayerTimer(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
			GamePlayerTimer gamePlayerTimer = (GamePlayerTimer)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerTimer));
			if (gamePlayerTimer.timerHours >= 24)
			{
				int num2 = (int)(gamePlayerTimer.timerHours / 24);
				int num3 = (int)gamePlayerTimer.timerHours - num2 * 24;
				this.m_LocalPlayerTimer.text = string.Concat(new object[]
				{
					num2,
					"d ",
					num3,
					"h"
				});
				return;
			}
			this.m_LocalPlayerTimer.text = string.Concat(new string[]
			{
				gamePlayerTimer.timerHours.ToString().PadLeft(2, '0'),
				":",
				gamePlayerTimer.timerMinutes.ToString().PadLeft(2, '0'),
				":",
				gamePlayerTimer.timerSeconds.ToString().PadLeft(2, '0')
			});
		}
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00029918 File Offset: 0x00027B18
	private IEnumerator UpdateOnlineStatus()
	{
		for (;;)
		{
			for (int i = 0; i < this.m_playerTokens.Length; i++)
			{
				if (this.m_playerTokens[i].playerIndex != 0 && this.m_playerTokens[i].playerIndex != AgricolaLib.GetLocalPlayerIndex())
				{
					int num = AgricolaLib.NetworkGetUserOnlineStatus((uint)this.m_playerTokens[i].playerIndex);
					if (num < 0 && num > 3)
					{
						num = 0;
					}
					if (this.m_playerTokens[i].onlineIndicatorImage != null)
					{
						this.m_playerTokens[i].onlineIndicatorImage.color = this.m_onlineStatusColors[num];
					}
				}
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x040004FC RID: 1276
	private const int k_maxDataSize = 1024;

	// Token: 0x040004FD RID: 1277
	[SerializeField]
	private PlayerDisplay_UpperHud.PlayerToken[] m_playerTokens;

	// Token: 0x040004FE RID: 1278
	[SerializeField]
	private AgricolaGame m_gameController;

	// Token: 0x040004FF RID: 1279
	[SerializeField]
	private TextMeshProUGUI m_localPlayerName;

	// Token: 0x04000500 RID: 1280
	[SerializeField]
	private TextMeshProUGUI m_StageRoundText;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private Image[] m_StageRoundIcons;

	// Token: 0x04000502 RID: 1282
	[SerializeField]
	private Color m_inactiveRoundIconColor;

	// Token: 0x04000503 RID: 1283
	[SerializeField]
	private Color[] m_onlineStatusColors;

	// Token: 0x04000504 RID: 1284
	[SerializeField]
	private GameObject m_LocalPlayerTimerObj;

	// Token: 0x04000505 RID: 1285
	[SerializeField]
	private TextMeshProUGUI m_LocalPlayerTimer;

	// Token: 0x04000506 RID: 1286
	private byte[] m_dataBuffer;

	// Token: 0x04000507 RID: 1287
	private GCHandle m_hDataBuffer;

	// Token: 0x04000508 RID: 1288
	private IntPtr m_bufPtr;

	// Token: 0x04000509 RID: 1289
	private int m_roundNumber = -1;

	// Token: 0x0400050A RID: 1290
	private int m_harvestMode = -2;

	// Token: 0x0200077D RID: 1917
	[Serializable]
	public struct PlayerToken
	{
		// Token: 0x04002BE9 RID: 11241
		public GameObject root;

		// Token: 0x04002BEA RID: 11242
		public ColorByFaction colorizer;

		// Token: 0x04002BEB RID: 11243
		public Avatar_UI avatar;

		// Token: 0x04002BEC RID: 11244
		public GameObject[] m_workerBaseNodes;

		// Token: 0x04002BED RID: 11245
		public GameObject[] m_workerAvailableNodes;

		// Token: 0x04002BEE RID: 11246
		public GameObject startingPlayerRootNode;

		// Token: 0x04002BEF RID: 11247
		public GameObject startingPlayerNode;

		// Token: 0x04002BF0 RID: 11248
		public GameObject[] harvestPlayerNodes;

		// Token: 0x04002BF1 RID: 11249
		public GameObject aiThinkingIndicator;

		// Token: 0x04002BF2 RID: 11250
		public GameObject onlineIndicatorBase;

		// Token: 0x04002BF3 RID: 11251
		public Image onlineIndicatorImage;

		// Token: 0x04002BF4 RID: 11252
		[HideInInspector]
		public int playerIndex;

		// Token: 0x04002BF5 RID: 11253
		[HideInInspector]
		public int playerInstanceID;

		// Token: 0x04002BF6 RID: 11254
		[HideInInspector]
		public int factionIndex;
	}
}
