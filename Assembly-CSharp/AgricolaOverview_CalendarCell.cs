using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class AgricolaOverview_CalendarCell : MonoBehaviour
{
	// Token: 0x060004F4 RID: 1268 RVA: 0x00026B50 File Offset: 0x00024D50
	public int GetRoundNumber()
	{
		return this.m_roundNumber;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00026B58 File Offset: 0x00024D58
	public void SetupCell(int round, int numPlayers)
	{
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		if (this.m_roundText != null)
		{
			this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}") + " " + round.ToString();
		}
		this.m_roundNumber = round;
		EAgricolaSeason roundSeason = AgricolaLib.GetRoundSeason(round);
		for (int i = 0; i < this.m_cells.Length; i++)
		{
			this.m_cells[i].root.SetActive(i < numPlayers);
			if (this.m_cells[i].seasonSpring != null)
			{
				this.m_cells[i].seasonSpring.SetActive(roundSeason == EAgricolaSeason.SPRING);
			}
			if (this.m_cells[i].seasonSummer != null)
			{
				this.m_cells[i].seasonSummer.SetActive(roundSeason == EAgricolaSeason.SUMMER);
			}
			if (this.m_cells[i].seasonAutumn != null)
			{
				this.m_cells[i].seasonAutumn.SetActive(roundSeason == EAgricolaSeason.AUTUMN);
			}
			if (this.m_cells[i].seasonHarvest != null)
			{
				this.m_cells[i].seasonHarvest.SetActive(roundSeason == EAgricolaSeason.AUTUMN);
			}
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00026CDE File Offset: 0x00024EDE
	public void SetShowRecievedOnly(bool bShow)
	{
		this.m_bShowOnlyRecieved = bShow;
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00026CE8 File Offset: 0x00024EE8
	public bool SetCalendarData(int playerIndex, int slotIndex, AgricolaCardManager cardManager)
	{
		int gamePlayerCalendar = AgricolaLib.GetGamePlayerCalendar(playerIndex, this.m_roundNumber, this.m_bufPtr, 1024);
		GamePlayerCalendar gamePlayerCalendar2 = (GamePlayerCalendar)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerCalendar));
		int num = 0;
		for (int i = 0; i < gamePlayerCalendar; i++)
		{
			if (!this.m_bShowOnlyRecieved || (gamePlayerCalendar2.flags[i] & 1) != 0)
			{
				int num2 = (int)gamePlayerCalendar2.producedType[i];
				if (gamePlayerCalendar2.costType[i] != 0)
				{
					num2 = 12;
				}
				else if (num2 >= 10)
				{
					if (num2 == 241)
					{
						num2 = 10;
					}
					else if (num2 == 242)
					{
						num2 = 11;
					}
				}
				if (num2 >= this.m_cellResPrefabs.Length)
				{
					Debug.LogError("Calendar resource cell index out of range: " + num2.ToString());
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cellResPrefabs[num2]);
					if (gameObject != null)
					{
						this.m_createdObjs.Add(gameObject);
						gameObject.transform.SetParent(this.m_cells[slotIndex].resLocator.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						GameObject gameObject2 = cardManager.CreateTemporaryCardFromInstanceID((int)gamePlayerCalendar2.cardID[i]);
						if (gameObject2 != null)
						{
							this.m_createdObjs.Add(gameObject2);
						}
						gameObject.GetComponent<AgricolaOverview_CalendarResource>().SetData(gamePlayerCalendar2.costType[i], gamePlayerCalendar2.costAmount[i], gamePlayerCalendar2.producedType[i], gamePlayerCalendar2.producedAmount[i], gameObject2);
					}
					num++;
				}
			}
		}
		return num != 0;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00026E84 File Offset: 0x00025084
	public void Exit()
	{
		for (int i = 0; i < this.m_createdObjs.Count; i++)
		{
			if (this.m_createdObjs[i] != null)
			{
				UnityEngine.Object.Destroy(this.m_createdObjs[i]);
			}
		}
		this.m_createdObjs.Clear();
		this.m_hDataBuffer.Free();
	}

	// Token: 0x04000479 RID: 1145
	private const int k_maxDataSize = 1024;

	// Token: 0x0400047A RID: 1146
	public AgricolaOverview_CalendarCell.PlayerCell[] m_cells;

	// Token: 0x0400047B RID: 1147
	public TextMeshProUGUI m_roundText;

	// Token: 0x0400047C RID: 1148
	public GameObject[] m_cellResPrefabs;

	// Token: 0x0400047D RID: 1149
	private bool m_bShowOnlyRecieved;

	// Token: 0x0400047E RID: 1150
	private int m_roundNumber;

	// Token: 0x0400047F RID: 1151
	private List<GameObject> m_createdObjs = new List<GameObject>();

	// Token: 0x04000480 RID: 1152
	private byte[] m_dataBuffer;

	// Token: 0x04000481 RID: 1153
	private GCHandle m_hDataBuffer;

	// Token: 0x04000482 RID: 1154
	private IntPtr m_bufPtr;

	// Token: 0x02000776 RID: 1910
	[Serializable]
	public struct PlayerCell
	{
		// Token: 0x04002BCC RID: 11212
		public GameObject root;

		// Token: 0x04002BCD RID: 11213
		public GameObject seasonSpring;

		// Token: 0x04002BCE RID: 11214
		public GameObject seasonSummer;

		// Token: 0x04002BCF RID: 11215
		public GameObject seasonAutumn;

		// Token: 0x04002BD0 RID: 11216
		public GameObject seasonHarvest;

		// Token: 0x04002BD1 RID: 11217
		public GameObject resLocator;
	}
}
