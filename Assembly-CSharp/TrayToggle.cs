using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class TrayToggle : MonoBehaviour
{
	// Token: 0x06000BC2 RID: 3010 RVA: 0x000536D3 File Offset: 0x000518D3
	public int GetOwnerIndex()
	{
		return this.m_ownerIndex;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000536DB File Offset: 0x000518DB
	public bool IsTrayOpen()
	{
		return this.m_bTrayOpen;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x000536E4 File Offset: 0x000518E4
	private void Start()
	{
		this.UpdateTrayState(false);
		GameObject gameObject = GameObject.FindGameObjectWithTag("Audio Manager");
		if (gameObject != null)
		{
			this.m_audioManager = gameObject.GetComponent<AgricolaAudioHandlerIngame>();
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00053718 File Offset: 0x00051918
	public void TrayState()
	{
		this.m_bTrayOpen = !this.m_bTrayOpen;
		this.UpdateTrayState(true);
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00053730 File Offset: 0x00051930
	public void SetTrayState(bool bOpen)
	{
		if (this.m_bTrayOpen == bOpen)
		{
			return;
		}
		this.m_bTrayOpen = bOpen;
		this.UpdateTrayState(true);
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0005374C File Offset: 0x0005194C
	protected virtual void UpdateTrayState(bool bPlayAudio = true)
	{
		if (this.tray != null)
		{
			this.tray.SetBool("isHidden", !this.m_bTrayOpen);
		}
		if (bPlayAudio && this.m_audioManager != null && this.m_bPlayTraySounds)
		{
			if (this.m_bTrayOpen)
			{
				this.m_audioManager.PlayAudioTrayOpen();
			}
			else
			{
				this.m_audioManager.PlayAudioTrayClose();
			}
		}
		this.CheckCloseTrays();
		if (this.m_bTrayOpen && this.m_resetPositionOnOpen != TrayToggle.ResetScrollPositionEnum.None)
		{
			base.StartCoroutine(this.ResetScrollPosition());
		}
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x000537DC File Offset: 0x000519DC
	private void CheckCloseTrays()
	{
		if (!this.m_bTrayOpen)
		{
			return;
		}
		this.CloseOtherTrays();
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x000537F0 File Offset: 0x000519F0
	public void CloseOtherTrays()
	{
		if (this.m_CloseOtherTrays != null)
		{
			for (int i = 0; i < this.m_CloseOtherTrays.Length; i++)
			{
				this.m_CloseOtherTrays[i].SetTrayState(false);
			}
		}
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00053826 File Offset: 0x00051A26
	private IEnumerator ResetScrollPosition()
	{
		yield return new WaitForSeconds(0.05f);
		if (this.m_scrollRects != null)
		{
			for (int i = 0; i < this.m_scrollRects.Length; i++)
			{
				if (this.m_scrollRects[i] != null)
				{
					this.m_scrollRects[i].horizontalNormalizedPosition = ((this.m_resetPositionOnOpen == TrayToggle.ResetScrollPositionEnum.Left) ? 0f : 1f);
				}
			}
		}
		yield break;
	}

	// Token: 0x04000CCA RID: 3274
	[SerializeField]
	protected Animator tray;

	// Token: 0x04000CCB RID: 3275
	[SerializeField]
	protected TrayToggle[] m_CloseOtherTrays;

	// Token: 0x04000CCC RID: 3276
	[SerializeField]
	private int m_ownerIndex = -1;

	// Token: 0x04000CCD RID: 3277
	[SerializeField]
	protected TrayToggle.ResetScrollPositionEnum m_resetPositionOnOpen;

	// Token: 0x04000CCE RID: 3278
	[SerializeField]
	protected bool m_bPlayTraySounds = true;

	// Token: 0x04000CCF RID: 3279
	[SerializeField]
	protected ScrollRect[] m_scrollRects;

	// Token: 0x04000CD0 RID: 3280
	private AgricolaAudioHandlerIngame m_audioManager;

	// Token: 0x04000CD1 RID: 3281
	protected bool m_bTrayOpen;

	// Token: 0x02000810 RID: 2064
	public enum ResetScrollPositionEnum
	{
		// Token: 0x04002E24 RID: 11812
		None,
		// Token: 0x04002E25 RID: 11813
		Left,
		// Token: 0x04002E26 RID: 11814
		Right
	}
}
