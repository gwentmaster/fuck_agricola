using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class HotseatOverlay : MonoBehaviour
{
	// Token: 0x06000517 RID: 1303 RVA: 0x00003022 File Offset: 0x00001222
	private void Awake()
	{
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00027AB0 File Offset: 0x00025CB0
	private void OnDestroy()
	{
		if (this.m_bFree)
		{
			this.m_hDataBuffer.Free();
			this.m_bFree = false;
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00027ACC File Offset: 0x00025CCC
	public bool IsVisible()
	{
		return this.m_HotseatBase.activeInHierarchy;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00027AD9 File Offset: 0x00025CD9
	public void SetIsVisible(bool bVisible)
	{
		base.StopAllCoroutines();
		this.m_HotseatBase.SetActive(bVisible);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00027AED File Offset: 0x00025CED
	private IEnumerator TurnOn()
	{
		yield return new WaitForEndOfFrame();
		this.m_Animator.SetBool("isHidden", false);
		yield break;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00027AFC File Offset: 0x00025CFC
	private IEnumerator TurnOff()
	{
		yield return new WaitForSeconds(this.m_waitTimeToDisable);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00027B0C File Offset: 0x00025D0C
	public void SetData(string name, int playerIndex)
	{
		if (!this.m_bFree)
		{
			this.m_bFree = true;
			this.m_dataBuffer = new byte[1024];
			this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
			this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		}
		for (int i = 0; i < this.m_workerList.Count; i++)
		{
			if (this.m_workerList[i] != null)
			{
				UnityEngine.Object.Destroy(this.m_workerList[i].gameObject);
			}
		}
		this.m_workerList.Clear();
		AgricolaLib.GetGamePlayerFarmState(playerIndex, this.m_bufPtr, 1024);
		GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
		if (this.m_workerManager != null && this.m_workerLocator != null)
		{
			for (int j = 0; j < gamePlayerFarmState.workerCount; j++)
			{
				AgricolaWorker agricolaWorker = this.m_workerManager.CreateTemporaryWorker(0, gamePlayerFarmState.playerFaction);
				if (agricolaWorker != null)
				{
					agricolaWorker.gameObject.SetActive(true);
					agricolaWorker.SetAvatar(gamePlayerFarmState.workerAvatarIDs[j]);
					agricolaWorker.SetDragType(ECardDragType.Never);
					agricolaWorker.SetSelectable(false, Color.white);
					agricolaWorker.transform.localScale = new Vector3(this.m_workerScale, this.m_workerScale, 1f);
					agricolaWorker.transform.SetParent(this.m_workerLocator.transform, false);
					agricolaWorker.SetShadeOverlayVisible((gamePlayerFarmState.unusedWorkerFlags & 1 << j) == 0);
					this.m_workerList.Add(agricolaWorker);
				}
			}
		}
		this.m_HotseatName.text = name;
		if (this.m_HotseatColorizer != null)
		{
			this.m_HotseatColorizer.Colorize((uint)gamePlayerFarmState.playerFaction);
		}
	}

	// Token: 0x040004BE RID: 1214
	private const int k_maxDataSize = 1024;

	// Token: 0x040004BF RID: 1215
	public GameObject m_HotseatBase;

	// Token: 0x040004C0 RID: 1216
	public Animator m_Animator;

	// Token: 0x040004C1 RID: 1217
	public float m_waitTimeToDisable = 0.75f;

	// Token: 0x040004C2 RID: 1218
	public TextMeshProUGUI m_HotseatName;

	// Token: 0x040004C3 RID: 1219
	public GameObject[] m_FactionObjects;

	// Token: 0x040004C4 RID: 1220
	public ColorByFaction m_HotseatColorizer;

	// Token: 0x040004C5 RID: 1221
	private bool m_bIsHidden = true;

	// Token: 0x040004C6 RID: 1222
	public AgricolaWorkerManager m_workerManager;

	// Token: 0x040004C7 RID: 1223
	public GameObject m_workerLocator;

	// Token: 0x040004C8 RID: 1224
	public float m_workerScale = 1f;

	// Token: 0x040004C9 RID: 1225
	private List<AgricolaWorker> m_workerList = new List<AgricolaWorker>();

	// Token: 0x040004CA RID: 1226
	private byte[] m_dataBuffer;

	// Token: 0x040004CB RID: 1227
	private GCHandle m_hDataBuffer;

	// Token: 0x040004CC RID: 1228
	private IntPtr m_bufPtr;

	// Token: 0x040004CD RID: 1229
	private bool m_bFree;
}
