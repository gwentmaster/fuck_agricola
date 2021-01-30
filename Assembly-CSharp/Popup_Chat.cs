using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F1 RID: 241
public class Popup_Chat : MonoBehaviour
{
	// Token: 0x060008AD RID: 2221 RVA: 0x0003C06F File Offset: 0x0003A26F
	private void Awake()
	{
		if (this.m_chatObjList == null)
		{
			this.m_chatObjList = new List<ChatTextLine>();
		}
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0003C084 File Offset: 0x0003A284
	private void Start()
	{
		if (this.m_PopupWindowIngamePanel != null)
		{
			this.m_PopupWindowIngamePanel.SetOnChatPanelOpenCallback(new Chat_InGame_Panel.OnChatPanelOpen(this.UpdateChatPosition));
		}
		this.ResetColorStrings();
		if (this.m_bProfileAlwaysVisible)
		{
			this.DisplayLocalProfile();
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003C0C0 File Offset: 0x0003A2C0
	private void Update()
	{
		if (string.IsNullOrEmpty(this.m_localUsername))
		{
			return;
		}
		this.UpdateChatMessageList();
		if (this.m_bMoveScrollRectToBottom)
		{
			this.m_bMoveScrollRectToBottom = false;
			if (this.m_ChatTextScrollView != null)
			{
				Canvas.ForceUpdateCanvases();
				this.m_ChatTextScrollView.verticalNormalizedPosition = 0f;
			}
		}
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0003C114 File Offset: 0x0003A314
	public void ResetColorStrings()
	{
		this.m_localColorString = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_localChatColor.r * 255f), (int)(this.m_localChatColor.g * 255f), (int)(this.m_localChatColor.b * 255f)) + "ff>";
		this.m_otherColorString1 = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_otherChatColor1.r * 255f), (int)(this.m_otherChatColor1.g * 255f), (int)(this.m_otherChatColor1.b * 255f)) + "ff>";
		this.m_otherColorString2 = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_otherChatColor2.r * 255f), (int)(this.m_otherChatColor2.g * 255f), (int)(this.m_otherChatColor2.b * 255f)) + "ff>";
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0003C250 File Offset: 0x0003A450
	public void Recolor(uint factionIndex)
	{
		ColorByFaction component = base.GetComponent<ColorByFaction>();
		if (component != null)
		{
			component.Colorize(factionIndex);
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0003C274 File Offset: 0x0003A474
	public void SetLocalUsername(string username)
	{
		this.m_localUsername = username;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003C280 File Offset: 0x0003A480
	public void UpdateDisplay(uint channel_id)
	{
		this.m_ChatChannelID = channel_id;
		this.m_LastChatMessageIndex = 0U;
		if (this.m_ChatTextPanel != null)
		{
			while (this.m_ChatTextPanel.transform.childCount > 0)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ChatTextPanel.transform.GetChild(0).gameObject);
			}
			if (this.m_chatObjList == null)
			{
				this.m_chatObjList = new List<ChatTextLine>();
			}
			this.m_chatObjList.Clear();
		}
		if (this.m_PopupHeader != null)
		{
			if (this.m_ChatChannelID == 0U)
			{
				this.m_PopupHeader.text = "ONLINE LOBBY";
			}
			else
			{
				this.m_PopupHeader.text = "INGAME CHAT #" + this.m_ChatChannelID;
			}
		}
		this.UpdateChatPosition();
		if (string.IsNullOrEmpty(this.m_localUsername))
		{
			return;
		}
		this.UpdateChatMessageList();
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0003C35C File Offset: 0x0003A55C
	public void UpdateChatMessageList()
	{
		uint num = AgricolaLib.NetworkGetChatChannelMessageCount(this.m_ChatChannelID);
		if (this.m_LastChatMessageIndex >= num)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.m_localColorString))
		{
			this.ResetColorStrings();
		}
		GCHandle gchandle = GCHandle.Alloc(new byte[8192], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		while (this.m_LastChatMessageIndex < num)
		{
			uint num2 = AgricolaLib.NetworkGetChatChannelMessageList(this.m_ChatChannelID, this.m_LastChatMessageIndex + 1U, 32U, intPtr, 8192);
			if (num2 == 0U)
			{
				break;
			}
			ChatChannelMessageList chatChannelMessageList = (ChatChannelMessageList)Marshal.PtrToStructure(intPtr, typeof(ChatChannelMessageList));
			for (uint num3 = 0U; num3 < num2; num3 += 1U)
			{
				if (chatChannelMessageList.entries[(int)num3].chatMessageIndex > this.m_LastChatMessageIndex)
				{
					this.m_LastChatMessageIndex = chatChannelMessageList.entries[(int)num3].chatMessageIndex;
					if (!(this.m_ChatTextPanel == null))
					{
						GameObject gameObject = this.m_ChatTextLinePrefab;
						if (this.m_ChatTextLinePhonePrefab != null && PlatformManager.s_instance.GetDeviceType() == PlatformManager.DeviceType.PHONE)
						{
							gameObject = this.m_ChatTextLinePhonePrefab;
						}
						if (!(gameObject == null))
						{
							string color_string = string.Empty;
							if (chatChannelMessageList.entries[(int)num3].chatUserDisplayName == this.m_localUsername)
							{
								color_string = this.m_localColorString;
							}
							else
							{
								if (chatChannelMessageList.entries[(int)num3].chatUserDisplayName != this.m_lastUsername)
								{
									this.m_bUseColor2 = !this.m_bUseColor2;
									this.m_lastUsername = chatChannelMessageList.entries[(int)num3].chatUserDisplayName;
								}
								color_string = (this.m_bUseColor2 ? this.m_otherColorString2 : this.m_otherColorString1);
							}
							GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
							gameObject2.transform.SetParent(this.m_ChatTextPanel.transform, false);
							ChatTextLine component = gameObject2.GetComponent<ChatTextLine>();
							if (component != null)
							{
								component.SetParent(this);
								component.SetChatTextLine(chatChannelMessageList.entries[(int)num3], color_string);
								this.m_chatObjList.Add(component);
							}
						}
					}
				}
			}
		}
		gchandle.Free();
		this.m_bMoveScrollRectToBottom = true;
		this.UpdateChatPosition();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0003C58B File Offset: 0x0003A78B
	public ChatTextLine GetLastChat()
	{
		if (this.m_chatObjList == null || this.m_chatObjList.Count < 1)
		{
			return null;
		}
		return this.m_chatObjList[this.m_chatObjList.Count - 1];
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0003C5C0 File Offset: 0x0003A7C0
	public void OnChatEntryClicked(string username, uint userID)
	{
		if (username != this.m_localUsername && username.Length > 0)
		{
			GameObject scene = ScreenManager.instance.GetScene("OnlineProfile");
			if (scene != null)
			{
				UI_OnlineProfile component = scene.GetComponent<UI_OnlineProfile>();
				if (component != null)
				{
					component.SetDisplayedPlayerID(userID);
					ScreenManager.instance.PushScene("OnlineProfile");
				}
			}
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00003022 File Offset: 0x00001222
	public void DisplayLocalProfile()
	{
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003C624 File Offset: 0x0003A824
	public void SubmitChat()
	{
		if (this.m_ChatEntryField == null)
		{
			return;
		}
		if (!Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.KeypadEnter))
		{
			return;
		}
		string text = this.m_ChatEntryField.text;
		if (text != string.Empty)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			uint chatLength = (uint)bytes.Length;
			AgricolaLib.NetworkSubmitChatMessage(this.m_ChatChannelID, chatLength, bytes);
			this.m_ChatEntryField.text = string.Empty;
		}
		this.m_ChatEntryField.Select();
		this.m_ChatEntryField.ActivateInputField();
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0003C6B4 File Offset: 0x0003A8B4
	public void UpdateChatPosition()
	{
		if (this.m_PopupWindowIngamePanel != null && this.m_PopupWindowIngamePanel.IsShowing)
		{
			AgricolaLib.NetworkSubmitChatPosition(this.m_ChatChannelID, this.m_LastChatMessageIndex);
		}
		uint num = AgricolaLib.NetworkGetChatPosition(this.m_ChatChannelID);
		if (this.m_NewChatIndicator != null)
		{
			this.m_NewChatIndicator.SetActive(num < this.m_LastChatMessageIndex);
		}
		if (this.m_NewChatIndicatorCount != null)
		{
			this.m_NewChatIndicatorCount.text = Mathf.Abs(this.m_LastChatMessageIndex - num).ToString();
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0003C74B File Offset: 0x0003A94B
	public void ClosePopup()
	{
		if (this.m_PopupWindow != null)
		{
			this.m_PopupWindow.SetActive(false);
		}
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003C767 File Offset: 0x0003A967
	public int GetFactionIndex(uint playerID)
	{
		if (this.m_factionDictionary == null)
		{
			this.m_factionDictionary = new Dictionary<uint, int>();
		}
		if (this.m_factionDictionary.ContainsKey(playerID))
		{
			return this.m_factionDictionary[playerID];
		}
		return 0;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0003C798 File Offset: 0x0003A998
	public void AddFactionIndex(uint playerID, int factionIndex)
	{
		if (this.m_factionDictionary == null)
		{
			this.m_factionDictionary = new Dictionary<uint, int>();
		}
		this.m_factionDictionary.Add(playerID, factionIndex);
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0003C7BA File Offset: 0x0003A9BA
	public void ClearFactionDictionary()
	{
		if (this.m_factionDictionary == null)
		{
			this.m_factionDictionary = new Dictionary<uint, int>();
		}
		this.m_factionDictionary.Clear();
	}

	// Token: 0x0400096D RID: 2413
	public const int k_maxDataSize = 8192;

	// Token: 0x0400096E RID: 2414
	public GameObject m_PopupWindow;

	// Token: 0x0400096F RID: 2415
	public Chat_InGame_Panel m_PopupWindowIngamePanel;

	// Token: 0x04000970 RID: 2416
	public GameObject m_NewChatIndicator;

	// Token: 0x04000971 RID: 2417
	public TextMeshProUGUI m_NewChatIndicatorCount;

	// Token: 0x04000972 RID: 2418
	public TextMeshProUGUI m_PopupHeader;

	// Token: 0x04000973 RID: 2419
	public ScrollRect m_ChatTextScrollView;

	// Token: 0x04000974 RID: 2420
	public GameObject m_ChatTextPanel;

	// Token: 0x04000975 RID: 2421
	public GameObject m_ChatTextLinePrefab;

	// Token: 0x04000976 RID: 2422
	public GameObject m_ChatTextLinePhonePrefab;

	// Token: 0x04000977 RID: 2423
	public TMP_InputField m_ChatEntryField;

	// Token: 0x04000978 RID: 2424
	public Color m_localChatColor;

	// Token: 0x04000979 RID: 2425
	public Color m_otherChatColor1;

	// Token: 0x0400097A RID: 2426
	public Color m_otherChatColor2;

	// Token: 0x0400097B RID: 2427
	public bool m_bProfileAlwaysVisible;

	// Token: 0x0400097C RID: 2428
	private uint m_ChatChannelID;

	// Token: 0x0400097D RID: 2429
	private uint m_LastChatMessageIndex;

	// Token: 0x0400097E RID: 2430
	private bool m_bMoveScrollRectToBottom;

	// Token: 0x0400097F RID: 2431
	private string m_localColorString;

	// Token: 0x04000980 RID: 2432
	private string m_otherColorString1;

	// Token: 0x04000981 RID: 2433
	private string m_otherColorString2;

	// Token: 0x04000982 RID: 2434
	private string m_lastUsername = string.Empty;

	// Token: 0x04000983 RID: 2435
	private string m_localUsername;

	// Token: 0x04000984 RID: 2436
	private bool m_bUseColor2;

	// Token: 0x04000985 RID: 2437
	private List<ChatTextLine> m_chatObjList;

	// Token: 0x04000986 RID: 2438
	private Dictionary<uint, int> m_factionDictionary;
}
