using System;
using System.Runtime.InteropServices;
using GameData;
using TMPro;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class AgricolaScoringOverview : MonoBehaviour
{
	// Token: 0x0600045D RID: 1117 RVA: 0x00022788 File Offset: 0x00020988
	public void ShowPlayerScores()
	{
		this.ClearCards();
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		component.GetMagnifyManager().SetOverridePanelObject(this.m_magnifyPanel);
		component.GetMagnifyManager().SetUseOverrideLayer(true);
		component.GetMagnifyManager().SetOverrideLayerObject(this.m_animationLayer);
		for (int i = 0; i < 6; i++)
		{
			this.SetPlayerData(AgricolaLib.GetLocalOpponentPlayerIndex(i), i, component);
		}
		this.m_hDataBuffer.Free();
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00022834 File Offset: 0x00020A34
	private void SetPlayerData(int playerIndex, int slot, AgricolaGame gameController)
	{
		if (playerIndex <= 0)
		{
			for (int i = 0; i < this.m_playerSlots[slot].rootObjects.Length; i++)
			{
				this.m_playerSlots[slot].rootObjects[i].SetActive(false);
			}
			int num = 0;
			while (num < this.m_playerSlots[slot].scoreCatScore.Length && num < this.m_playerSlots[slot].scoreCatValue.Length)
			{
				this.m_playerSlots[slot].scoreCatScore[num].text = string.Empty;
				this.m_playerSlots[slot].scoreCatValue[num].text = string.Empty;
				num++;
			}
			this.m_playerSlots[slot].name.text = string.Empty;
			return;
		}
		AgricolaCardManager cardManager = gameController.GetCardManager();
		AgricolaLib.GetGamePlayerScoreState(playerIndex, this.m_bufPtr, 1024);
		GamePlayerScoreState gamePlayerScoreState = (GamePlayerScoreState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerScoreState));
		for (int j = 0; j < this.m_playerSlots[slot].rootObjects.Length; j++)
		{
			this.m_playerSlots[slot].rootObjects[j].SetActive(true);
		}
		if (this.m_playerSlots[slot].colorizer != null)
		{
			this.m_playerSlots[slot].colorizer.Colorize((uint)gamePlayerScoreState.playerFaction);
		}
		this.m_playerSlots[slot].avatar.SetAvatar(gamePlayerScoreState.workerAvatarID, true);
		this.m_playerSlots[slot].name.text = gamePlayerScoreState.displayName;
		this.m_playerSlots[slot].totalScore.text = gamePlayerScoreState.total_points.ToString();
		int num2 = 0;
		while (num2 < this.m_playerSlots[slot].scoreCatScore.Length && num2 < this.m_playerSlots[slot].scoreCatValue.Length)
		{
			this.m_playerSlots[slot].scoreCatScore[num2].text = gamePlayerScoreState.score[num2].ToString();
			this.m_playerSlots[slot].scoreCatValue[num2].text = gamePlayerScoreState.count[num2].ToString();
			num2++;
		}
		if (cardManager != null)
		{
			int num3 = 0;
			while (num3 < gamePlayerScoreState.bonus_point_entry_count && num3 < this.m_playerSlots[slot].bonusDetails.Length)
			{
				if (this.m_playerSlots[slot].bonusDetails[num3].score != null)
				{
					this.m_playerSlots[slot].bonusDetails[num3].score.text = gamePlayerScoreState.bonusPoints[num3].ToString();
				}
				if (this.m_playerSlots[slot].bonusDetails[num3].locator != null)
				{
					this.m_playerSlots[slot].bonusDetails[num3].cardObj = cardManager.CreateTemporaryCardFromInstanceID((int)gamePlayerScoreState.bonusIDs[num3]);
					if (this.m_playerSlots[slot].bonusDetails[num3].cardObj == null)
					{
						AgricolaLib.GetInstanceData(9, (int)gamePlayerScoreState.bonusIDs[num3], this.m_bufPtr, 1024);
						CardInPlayData cardInPlayData = (CardInPlayData)Marshal.PtrToStructure(this.m_bufPtr, typeof(CardInPlayData));
						if (cardInPlayData.cardinplay_instance_id == (short)gamePlayerScoreState.bonusIDs[num3])
						{
							this.m_playerSlots[slot].bonusDetails[num3].cardObj = cardManager.CreateTemporaryCardFromInstanceID((int)cardInPlayData.sourcecard_instance_id);
						}
					}
					if (this.m_playerSlots[slot].bonusDetails[num3].cardObj != null)
					{
						this.m_playerSlots[slot].bonusDetails[num3].cardObj.SetActive(true);
						AnimateObject component = this.m_playerSlots[slot].bonusDetails[num3].cardObj.GetComponent<AnimateObject>();
						if (component != null)
						{
							this.m_playerSlots[slot].bonusDetails[num3].locator.PlaceAnimateObject(component, true, true, false);
						}
					}
				}
				num3++;
			}
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00022CBC File Offset: 0x00020EBC
	public void CloseWindow()
	{
		this.ClearCards();
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		component.GetMagnifyManager().SetOverridePanelObject(null);
		component.GetMagnifyManager().SetUseOverrideLayer(false);
		component.GetMagnifyManager().SetOverrideLayerObject(null);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00022CF8 File Offset: 0x00020EF8
	public void ClearCards()
	{
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			for (int j = 0; j < this.m_playerSlots[i].bonusDetails.Length; j++)
			{
				if (this.m_playerSlots[i].bonusDetails[j].score != null)
				{
					this.m_playerSlots[i].bonusDetails[j].score.text = " ";
				}
				if (this.m_playerSlots[i].bonusDetails[j].cardObj != null)
				{
					UnityEngine.Object.Destroy(this.m_playerSlots[i].bonusDetails[j].cardObj);
					this.m_playerSlots[i].bonusDetails[j].cardObj = null;
				}
			}
		}
	}

	// Token: 0x040003ED RID: 1005
	private const int k_maxDataSize = 1024;

	// Token: 0x040003EE RID: 1006
	public AgricolaScoringOverview.PlayerSlot[] m_playerSlots;

	// Token: 0x040003EF RID: 1007
	public GameObject m_magnifyPanel;

	// Token: 0x040003F0 RID: 1008
	public GameObject m_animationLayer;

	// Token: 0x040003F1 RID: 1009
	private byte[] m_dataBuffer;

	// Token: 0x040003F2 RID: 1010
	private GCHandle m_hDataBuffer;

	// Token: 0x040003F3 RID: 1011
	private IntPtr m_bufPtr;

	// Token: 0x0200076E RID: 1902
	[Serializable]
	public struct BonusPointDetais
	{
		// Token: 0x04002B9D RID: 11165
		public TextMeshProUGUI score;

		// Token: 0x04002B9E RID: 11166
		public AgricolaAnimationLocator locator;

		// Token: 0x04002B9F RID: 11167
		[HideInInspector]
		public GameObject cardObj;
	}

	// Token: 0x0200076F RID: 1903
	[Serializable]
	public struct PlayerSlot
	{
		// Token: 0x04002BA0 RID: 11168
		public GameObject[] rootObjects;

		// Token: 0x04002BA1 RID: 11169
		public ColorByFaction colorizer;

		// Token: 0x04002BA2 RID: 11170
		public Avatar_UI avatar;

		// Token: 0x04002BA3 RID: 11171
		public TextMeshProUGUI name;

		// Token: 0x04002BA4 RID: 11172
		public TextMeshProUGUI totalScore;

		// Token: 0x04002BA5 RID: 11173
		public TextMeshProUGUI[] scoreCatValue;

		// Token: 0x04002BA6 RID: 11174
		public TextMeshProUGUI[] scoreCatScore;

		// Token: 0x04002BA7 RID: 11175
		public AgricolaScoringOverview.BonusPointDetais[] bonusDetails;

		// Token: 0x04002BA8 RID: 11176
		[HideInInspector]
		public int playerIndex;
	}
}
