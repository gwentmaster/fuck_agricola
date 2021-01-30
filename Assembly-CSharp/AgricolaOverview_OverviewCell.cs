using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005E RID: 94
public class AgricolaOverview_OverviewCell : MonoBehaviour
{
	// Token: 0x06000510 RID: 1296 RVA: 0x000273BC File Offset: 0x000255BC
	public void SetupDisplay(int playerIndex, AgricolaCardManager cardManager)
	{
		this.Exit();
		this.m_playerIndex = playerIndex;
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetGamePlayerState(playerIndex, intPtr, 1024);
		GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerState));
		AgricolaLib.GetGamePlayerScoreState(playerIndex, intPtr, 1024);
		GamePlayerScoreState gamePlayerScoreState = (GamePlayerScoreState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerScoreState));
		this.m_stableText.text = gamePlayerState.stablesCount.ToString();
		this.m_fenceText.text = gamePlayerState.fencesCount.ToString();
		this.m_scoreText.text = gamePlayerScoreState.total_points.ToString();
		this.m_beggingText.text = gamePlayerScoreState.count[11].ToString();
		this.m_grainText.text = gamePlayerState.resourceCountGrain.ToString();
		this.m_veggieText.text = gamePlayerState.resourceCountVeggie.ToString();
		this.m_sheepText.text = gamePlayerState.resourceCountSheep.ToString();
		this.m_boarText.text = gamePlayerState.resourceCountWildBoar.ToString();
		this.m_cattleText.text = gamePlayerState.resourceCountCattle.ToString();
		this.m_woodText.text = gamePlayerState.resourceCountWood.ToString();
		this.m_clayText.text = gamePlayerState.resourceCountClay.ToString();
		this.m_reedText.text = gamePlayerState.resourceCountReed.ToString();
		this.m_stoneText.text = gamePlayerState.resourceCountStone.ToString();
		this.m_foodText.text = gamePlayerState.resourceCountFood.ToString();
		for (int i = 0; i < this.m_farmCells.Length; i++)
		{
			AgricolaLib.GetGamePlayerFarmTileState(playerIndex, i, intPtr, 1024);
			GamePlayerFarmTileState gamePlayerFarmTileState = (GamePlayerFarmTileState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerFarmTileState));
			if (gamePlayerFarmTileState.tileType == 1)
			{
				if (gamePlayerFarmTileState.data0[0] == 3)
				{
					this.m_farmCells[i].baseImage.color = this.m_farmCellStoneHouseColor;
				}
				else if (gamePlayerFarmTileState.data0[0] == 2)
				{
					this.m_farmCells[i].baseImage.color = this.m_farmCellClayHouseColor;
				}
				else if (gamePlayerFarmTileState.data0[0] == 1)
				{
					this.m_farmCells[i].baseImage.color = this.m_farmCellWoodHouseColor;
				}
			}
			else
			{
				this.m_farmCells[i].baseImage.color = ((gamePlayerFarmTileState.tileType < 3) ? this.m_farmCellEmptyColor : this.m_farmCellPastureColor);
			}
			if (this.m_farmCells[i].fieldBase != null)
			{
				if (gamePlayerFarmTileState.tileType == 2)
				{
					this.m_farmCells[i].fieldBase.SetActive(true);
					this.m_farmCells[i].fieldGrain.SetActive(gamePlayerFarmTileState.data0[0] == 5);
					this.m_farmCells[i].fieldVeggie.SetActive(gamePlayerFarmTileState.data0[0] == 6);
				}
				else
				{
					this.m_farmCells[i].fieldBase.SetActive(false);
				}
			}
			if (this.m_farmCells[i].stable != null)
			{
				this.m_farmCells[i].stable.SetActive(gamePlayerFarmTileState.hasStable != 0);
			}
		}
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		bool flag = gameParameters.gameType == 0 || gameParameters.gameType == 7;
		int[] array = new int[32];
		GCHandle gchandle2 = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle2.AddrOfPinnedObject();
		if (flag)
		{
			this.m_familyRoot.SetActive(true);
			this.m_fullRoot.SetActive(false);
			int instanceList = AgricolaLib.GetInstanceList(11, playerIndex, pInstanceIDs, 32);
			this.m_familyImpCount.text = instanceList.ToString();
			for (int j = 0; j < instanceList; j++)
			{
				if (j >= this.m_familyImpLocators.Length)
				{
					break;
				}
				int instanceID = array[j];
				GameObject gameObject = cardManager.CreateCardFromInstanceID(instanceID, false);
				if (gameObject != null)
				{
					this.m_createdCards.Add(gameObject);
					gameObject.SetActive(true);
					AnimateObject component = gameObject.GetComponent<AnimateObject>();
					if (component != null)
					{
						this.m_familyImpLocators[j].PlaceAnimateObject(component, true, true, false);
					}
				}
			}
		}
		else
		{
			this.m_familyRoot.SetActive(false);
			this.m_fullRoot.SetActive(true);
			int instanceList2 = AgricolaLib.GetInstanceList(11, playerIndex, pInstanceIDs, 32);
			this.m_fullImpCount.text = instanceList2.ToString();
			int num = 0;
			while (num < instanceList2 && num < this.m_fullImpLocators.Length)
			{
				int instanceID2 = array[num];
				GameObject gameObject2 = cardManager.CreateCardFromInstanceID(instanceID2, false);
				if (gameObject2 != null)
				{
					this.m_createdCards.Add(gameObject2);
					gameObject2.SetActive(true);
					AnimateObject component2 = gameObject2.GetComponent<AnimateObject>();
					if (component2 != null)
					{
						this.m_fullImpLocators[num].PlaceAnimateObject(component2, true, true, false);
					}
				}
				num++;
			}
			instanceList2 = AgricolaLib.GetInstanceList(10, playerIndex, pInstanceIDs, 32);
			this.m_fullOccCount.text = instanceList2.ToString();
			int num2 = 0;
			while (num2 < instanceList2 && num2 < this.m_fullOccLocators.Length)
			{
				int instanceID3 = array[num2];
				GameObject gameObject3 = cardManager.CreateCardFromInstanceID(instanceID3, false);
				if (gameObject3 != null)
				{
					this.m_createdCards.Add(gameObject3);
					gameObject3.SetActive(true);
					AnimateObject component3 = gameObject3.GetComponent<AnimateObject>();
					if (component3 != null)
					{
						this.m_fullOccLocators[num2].PlaceAnimateObject(component3, true, true, false);
					}
				}
				num2++;
			}
		}
		gchandle2.Free();
		gchandle.Free();
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000279B0 File Offset: 0x00025BB0
	public void Exit()
	{
		for (int i = 0; i < this.m_createdCards.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_createdCards[i]);
		}
		this.m_createdCards.Clear();
	}

	// Token: 0x0400049C RID: 1180
	private const int k_maxDataSize = 1024;

	// Token: 0x0400049D RID: 1181
	public Color m_farmCellEmptyColor;

	// Token: 0x0400049E RID: 1182
	public Color m_farmCellPastureColor;

	// Token: 0x0400049F RID: 1183
	public Color m_farmCellWoodHouseColor;

	// Token: 0x040004A0 RID: 1184
	public Color m_farmCellClayHouseColor;

	// Token: 0x040004A1 RID: 1185
	public Color m_farmCellStoneHouseColor;

	// Token: 0x040004A2 RID: 1186
	public AgricolaOverview_OverviewCell.FarmCell[] m_farmCells;

	// Token: 0x040004A3 RID: 1187
	[Space(10f)]
	public TextMeshProUGUI m_stableText;

	// Token: 0x040004A4 RID: 1188
	public TextMeshProUGUI m_fenceText;

	// Token: 0x040004A5 RID: 1189
	public TextMeshProUGUI m_scoreText;

	// Token: 0x040004A6 RID: 1190
	public TextMeshProUGUI m_beggingText;

	// Token: 0x040004A7 RID: 1191
	public TextMeshProUGUI m_grainText;

	// Token: 0x040004A8 RID: 1192
	public TextMeshProUGUI m_veggieText;

	// Token: 0x040004A9 RID: 1193
	public TextMeshProUGUI m_sheepText;

	// Token: 0x040004AA RID: 1194
	public TextMeshProUGUI m_boarText;

	// Token: 0x040004AB RID: 1195
	public TextMeshProUGUI m_cattleText;

	// Token: 0x040004AC RID: 1196
	public TextMeshProUGUI m_woodText;

	// Token: 0x040004AD RID: 1197
	public TextMeshProUGUI m_clayText;

	// Token: 0x040004AE RID: 1198
	public TextMeshProUGUI m_reedText;

	// Token: 0x040004AF RID: 1199
	public TextMeshProUGUI m_stoneText;

	// Token: 0x040004B0 RID: 1200
	public TextMeshProUGUI m_foodText;

	// Token: 0x040004B1 RID: 1201
	[Space(10f)]
	public GameObject m_familyRoot;

	// Token: 0x040004B2 RID: 1202
	public TextMeshProUGUI m_familyImpCount;

	// Token: 0x040004B3 RID: 1203
	public AgricolaAnimationLocator[] m_familyImpLocators;

	// Token: 0x040004B4 RID: 1204
	[Space(10f)]
	public GameObject m_fullRoot;

	// Token: 0x040004B5 RID: 1205
	public TextMeshProUGUI m_fullOccCount;

	// Token: 0x040004B6 RID: 1206
	public AgricolaAnimationLocator[] m_fullOccLocators;

	// Token: 0x040004B7 RID: 1207
	public TextMeshProUGUI m_fullImpCount;

	// Token: 0x040004B8 RID: 1208
	public AgricolaAnimationLocator[] m_fullImpLocators;

	// Token: 0x040004B9 RID: 1209
	[HideInInspector]
	public int m_playerIndex;

	// Token: 0x040004BA RID: 1210
	private List<GameObject> m_createdCards = new List<GameObject>();

	// Token: 0x02000778 RID: 1912
	[Serializable]
	public struct FarmCell
	{
		// Token: 0x04002BD5 RID: 11221
		public Image baseImage;

		// Token: 0x04002BD6 RID: 11222
		public GameObject stable;

		// Token: 0x04002BD7 RID: 11223
		public GameObject fieldBase;

		// Token: 0x04002BD8 RID: 11224
		public GameObject fieldGrain;

		// Token: 0x04002BD9 RID: 11225
		public GameObject fieldVeggie;
	}
}
