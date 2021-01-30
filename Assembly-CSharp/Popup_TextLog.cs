using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameEvent;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000070 RID: 112
public class Popup_TextLog : MonoBehaviour
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x0002C5CF File Offset: 0x0002A7CF
	private void Awake()
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0002C5DF File Offset: 0x0002A7DF
	private void OnDestroy()
	{
		this.ClearTextList();
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
	private void Init()
	{
		this.m_bInitialized = true;
		this.m_textLogList = new List<GameObject>();
		this.ResetColorStrings();
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		this.m_CardManager = component.GetCardManager();
		this.m_MagnifyManager = component.GetMagnifyManager();
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0002C638 File Offset: 0x0002A838
	private void LateUpdate()
	{
		if (this.m_bInitialized && this.m_bDestroyCardCheck && this.m_magnifiedCard != null && !this.m_magnifiedCard.IsMagnifying())
		{
			this.m_CardManager.DestroyCard(this.m_magnifiedCard);
			this.m_magnifiedCard = null;
			this.m_bDestroyCardCheck = false;
			this.m_MagnifyManager.RemoveOnUnmagnifyCallback(new MagnifyManager.MagnifyCallback(this.OnUnmagnifyCallback));
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0002C5CF File Offset: 0x0002A7CF
	public void HandleClickOnCardName(GameObject rootObj, string cardName)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0002C5CF File Offset: 0x0002A7CF
	public void HandleClickOnActionName(string actionName)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0002C6A6 File Offset: 0x0002A8A6
	public void OnUnmagnifyCallback(CardObject cardObj)
	{
		if (cardObj == this.m_magnifiedCard)
		{
			this.m_bDestroyCardCheck = true;
		}
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0002C6C0 File Offset: 0x0002A8C0
	public void AddTextLogLine(string logLine, uint expectedIndex)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		if ((long)this.m_textLogList.Count != (long)((ulong)expectedIndex))
		{
			Debug.Log("Rebuilding Popup_TextLog due to unexpected index");
			this.ClearTextList();
			uint num = AgricolaLib.GetOutputMessageLogCount() - 1U;
			GCHandle gchandle = GCHandle.Alloc(new byte[256], GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				if (AgricolaLib.GetOutputMessageLogAtIndex(num2, intPtr, 256) != 0)
				{
					OutputMessage outputMessage = (OutputMessage)Marshal.PtrToStructure(intPtr, typeof(OutputMessage));
					this.AddStringToLog_End(outputMessage.message, outputMessage.messageIndex);
				}
				else
				{
					Debug.LogError("Popup_TextLog: Unable to retrieve output message at index " + num2.ToString());
				}
			}
			gchandle.Free();
		}
		this.AddStringToLog_End(logLine, expectedIndex);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002C78C File Offset: 0x0002A98C
	public void RebuildTextList()
	{
		this.ClearTextList();
		uint outputMessageLogCount = AgricolaLib.GetOutputMessageLogCount();
		GCHandle gchandle = GCHandle.Alloc(new byte[256], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (uint num = 0U; num < outputMessageLogCount; num += 1U)
		{
			if (AgricolaLib.GetOutputMessageLogAtIndex(num, intPtr, 256) != 0)
			{
				OutputMessage outputMessage = (OutputMessage)Marshal.PtrToStructure(intPtr, typeof(OutputMessage));
				this.AddStringToLog_End(outputMessage.message, outputMessage.messageIndex);
			}
			else
			{
				Debug.LogError("Popup_TextLog: Unable to retrieve output message at index " + num.ToString());
			}
		}
		gchandle.Free();
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0002C824 File Offset: 0x0002AA24
	public void ClearTextList()
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		for (int i = 0; i < this.m_textLogList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_textLogList[i]);
		}
		this.m_textLogList.Clear();
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002C874 File Offset: 0x0002AA74
	private void AddStringToLog_End(string logLine, uint index)
	{
		logLine = logLine.Replace("<Card>", this.m_cardTextColorString + "<link=\"card\">");
		logLine = logLine.Replace("</Card>", "</link></color>");
		logLine = logLine.Replace("<Action>", this.m_actionTextColorString + "<link=\"action\">");
		logLine = logLine.Replace("</Action>", "</link></color>");
		logLine = logLine.Replace("\n", "");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_TextLinePrefab);
		gameObject.transform.SetParent(this.m_ContentTextPanel.transform, false);
		TextLogTextLine component = gameObject.GetComponent<TextLogTextLine>();
		this.m_textLogList.Add(gameObject);
		if (component != null)
		{
			component.SetParent(this);
			component.SetTextLine(logLine, index);
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0002C940 File Offset: 0x0002AB40
	public void ResetColorStrings()
	{
		this.m_actionTextColorString = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_countryTextColor.r * 255f), (int)(this.m_countryTextColor.g * 255f), (int)(this.m_countryTextColor.b * 255f)) + "ff>";
		this.m_cardTextColorString = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_cardTextColor.r * 255f), (int)(this.m_cardTextColor.g * 255f), (int)(this.m_cardTextColor.b * 255f)) + "ff>";
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0002CA15 File Offset: 0x0002AC15
	public void OpenPopup()
	{
		if (this.m_PopupWindow != null)
		{
			this.m_PopupWindow.SetActive(true);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0002CA31 File Offset: 0x0002AC31
	public void ClosePopup()
	{
		if (this.m_PopupWindow != null)
		{
			this.m_PopupWindow.SetActive(false);
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002CA4D File Offset: 0x0002AC4D
	public void TogglePopup()
	{
		if (this.m_PopupWindow != null)
		{
			this.m_PopupWindow.SetActive(!this.m_PopupWindow.activeSelf);
		}
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0002CA76 File Offset: 0x0002AC76
	private IEnumerator ResetScrollPosition()
	{
		yield return new WaitForEndOfFrame();
		this.m_TextScrollView.verticalNormalizedPosition = 0f;
		yield break;
	}

	// Token: 0x0400058C RID: 1420
	public GameObject m_PopupWindow;

	// Token: 0x0400058D RID: 1421
	public ScrollRect m_TextScrollView;

	// Token: 0x0400058E RID: 1422
	public GameObject m_ContentTextPanel;

	// Token: 0x0400058F RID: 1423
	public GameObject m_TextLinePrefab;

	// Token: 0x04000590 RID: 1424
	public Color m_countryTextColor;

	// Token: 0x04000591 RID: 1425
	public Color m_cardTextColor;

	// Token: 0x04000592 RID: 1426
	private bool m_bInitialized;

	// Token: 0x04000593 RID: 1427
	private string m_actionTextColorString;

	// Token: 0x04000594 RID: 1428
	private string m_cardTextColorString;

	// Token: 0x04000595 RID: 1429
	private List<GameObject> m_textLogList;

	// Token: 0x04000596 RID: 1430
	private AgricolaCardManager m_CardManager;

	// Token: 0x04000597 RID: 1431
	private AgricolaMagnifyManager m_MagnifyManager;

	// Token: 0x04000598 RID: 1432
	private CardObject m_magnifiedCard;

	// Token: 0x04000599 RID: 1433
	private bool m_bDestroyCardCheck;
}
