using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F9 RID: 249
[Serializable]
public class UIC_OfflineGameList
{
	// Token: 0x0600094B RID: 2379 RVA: 0x0003EC7C File Offset: 0x0003CE7C
	public void Initialize(string profileName, UIC_OfflineGameList.ClickCallback cb, MonoBehaviour monoBehaviourInstance)
	{
		this.m_Callback = cb;
		this.m_gameSlots = new List<UIP_GameSlot>();
		this.m_monoBehaviourInstance = monoBehaviourInstance;
		this.BuildGameList();
		this.m_scrollView.verticalNormalizedPosition = 1f;
		this.m_monoBehaviourInstance.StartCoroutine(this.Update());
		if (this.m_noGamesDisplay != null)
		{
			this.m_noGamesDisplay.SetActive(this.m_gameSlots.Count == 0);
		}
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003ECF1 File Offset: 0x0003CEF1
	public void Destroy()
	{
		this.ClearGameList();
		this.m_Callback = null;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003ED00 File Offset: 0x0003CF00
	private void HandleClickOnSlot(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt)
	{
		if (this.m_Callback != null)
		{
			this.m_Callback(slot, evt);
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003ED17 File Offset: 0x0003CF17
	public int GetNumGames()
	{
		return this.m_gameSlots.Count;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003ED24 File Offset: 0x0003CF24
	private IEnumerator Update()
	{
		yield return new WaitForEndOfFrame();
		this.m_scrollView.horizontalNormalizedPosition = 1f;
		yield break;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003ED34 File Offset: 0x0003CF34
	public void LoadGame(int slotIndex)
	{
		UIP_GameSlot gameFromID = this.GetGameFromID(slotIndex);
		if (gameFromID != null)
		{
			byte[] array = File.ReadAllBytes(gameFromID.GetFullSavePath());
			if (array.Length == gameFromID.GetDataSize())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				AgricolaLib.ResumeGame(intPtr, array.Length, gameFromID.GetDataVersion());
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0003ED94 File Offset: 0x0003CF94
	public void DeleteGame(int slotIndex)
	{
		UIP_GameSlot gameFromID = this.GetGameFromID(slotIndex);
		if (gameFromID != null)
		{
			if (gameFromID.GetShortSavePath() != string.Empty)
			{
				File.Delete(gameFromID.GetShortSavePath());
			}
			if (gameFromID.GetFullSavePath() != string.Empty)
			{
				File.Delete(gameFromID.GetFullSavePath());
			}
			UnityEngine.Object.Destroy(gameFromID.gameObject);
			this.m_gameSlots.Remove(gameFromID);
		}
		if (this.m_noGamesDisplay != null)
		{
			this.m_noGamesDisplay.SetActive(this.m_gameSlots.Count == 0);
		}
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0003EE2C File Offset: 0x0003D02C
	public UIP_GameSlot GetGameFromID(int slotIndex)
	{
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			if (this.m_gameSlots[i].GetSlotIndex() == slotIndex)
			{
				return this.m_gameSlots[i];
			}
		}
		return null;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003EE71 File Offset: 0x0003D071
	public UIP_GameSlot GetGameFromIndex(int index)
	{
		if (index < 0 || index >= this.m_gameSlots.Count)
		{
			return null;
		}
		return this.m_gameSlots[index];
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003EE94 File Offset: 0x0003D094
	public void SetAllDeleteMode(bool bDeleteMode)
	{
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			this.m_gameSlots[i].TurnOnDeleteMode(bDeleteMode);
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0003EECC File Offset: 0x0003D0CC
	private void ClearGameList()
	{
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_gameSlots[i].gameObject);
		}
		this.m_gameSlots.Clear();
		if (this.m_noGamesDisplay != null)
		{
			this.m_noGamesDisplay.SetActive(this.m_gameSlots.Count == 0);
		}
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003EF38 File Offset: 0x0003D138
	public static string GetFullFileName(int slot)
	{
		string path = string.Format("Save{0}Full.dat", slot);
		return Path.Combine(Application.persistentDataPath, path);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0003EF64 File Offset: 0x0003D164
	public static string GetShortFileName(int slot)
	{
		string path = string.Format("Save{0}Short.dat", slot);
		return Path.Combine(Application.persistentDataPath, path);
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003EF90 File Offset: 0x0003D190
	public static string GetFullFileName_Solo(long profileID)
	{
		string path = string.Format("SoloSave{0}Full.dat", profileID);
		return Path.Combine(Application.persistentDataPath, path);
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0003EFBC File Offset: 0x0003D1BC
	public static string GetShortFileName_Solo(long profileID)
	{
		string path = string.Format("SoloSave{0}Short.dat", profileID);
		return Path.Combine(Application.persistentDataPath, path);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003EFE8 File Offset: 0x0003D1E8
	public static int GetEmptySlot()
	{
		for (int i = 1; i < 32; i++)
		{
			string shortFileName = UIC_OfflineGameList.GetShortFileName(i);
			if (!File.Exists(UIC_OfflineGameList.GetFullFileName(i)) || !File.Exists(shortFileName))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002A062 File Offset: 0x00028262
	public static int GetTutorialSlot()
	{
		return 0;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003F024 File Offset: 0x0003D224
	private void BuildGameList()
	{
		this.ClearGameList();
		if (GameObject.FindGameObjectWithTag("IAP Manager") == null)
		{
			Debug.LogError("Unable to get IAP manager object");
			return;
		}
		for (int i = 0; i < 32; i++)
		{
			string shortFileName = UIC_OfflineGameList.GetShortFileName(i);
			string fullFileName = UIC_OfflineGameList.GetFullFileName(i);
			if (File.Exists(fullFileName) && File.Exists(shortFileName))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(shortFileName, FileMode.Open);
				ShortSaveStruct shortSaveStruct;
				try
				{
					shortSaveStruct = (ShortSaveStruct)binaryFormatter.Deserialize(fileStream);
				}
				catch (SerializationException)
				{
					fileStream.Close();
					goto IL_17A;
				}
				if (shortSaveStruct.playdekHeader != "PLAYDEK" || shortSaveStruct.playdekFooter != "PLAYDEK" || shortSaveStruct.saveFileVersionNumber < 1U)
				{
					fileStream.Close();
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_gameSlotPrefab);
					UIP_GameSlot component = gameObject.GetComponent<UIP_GameSlot>();
					try
					{
						component.SetData(shortSaveStruct, UIP_GameSlot.ESlotType.Active_Offline, null);
					}
					catch
					{
						goto IL_17A;
					}
					component.SetFullSavePath(fullFileName);
					component.SetShortSavePath(shortFileName);
					component.SetSlotIndex(i);
					component.SetClickListener(new UIP_GameSlot.ClickCallback(this.HandleClickOnSlot));
					this.m_gameSlots.Add(component);
					gameObject.transform.SetParent(this.m_contentContainer);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
					fileStream.Close();
				}
			}
			IL_17A:;
		}
		if (this.m_noGamesDisplay != null)
		{
			this.m_noGamesDisplay.SetActive(this.m_gameSlots.Count == 0);
		}
	}

	// Token: 0x040009D7 RID: 2519
	private const int k_MaxOfflineGameCount = 32;

	// Token: 0x040009D8 RID: 2520
	public GameObject m_gameSlotPrefab;

	// Token: 0x040009D9 RID: 2521
	public Transform m_contentContainer;

	// Token: 0x040009DA RID: 2522
	public ScrollRect m_scrollView;

	// Token: 0x040009DB RID: 2523
	private MonoBehaviour m_monoBehaviourInstance;

	// Token: 0x040009DC RID: 2524
	private UIC_OfflineGameList.ClickCallback m_Callback;

	// Token: 0x040009DD RID: 2525
	private List<UIP_GameSlot> m_gameSlots;

	// Token: 0x040009DE RID: 2526
	public GameObject m_noGamesDisplay;

	// Token: 0x020007C3 RID: 1987
	// (Invoke) Token: 0x06004309 RID: 17161
	public delegate void ClickCallback(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt);
}
