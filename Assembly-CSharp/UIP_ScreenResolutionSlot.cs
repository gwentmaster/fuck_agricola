using System;
using TMPro;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class UIP_ScreenResolutionSlot : MonoBehaviour
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00042889 File Offset: 0x00040A89
	// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00042891 File Offset: 0x00040A91
	public int ResolutionIndex
	{
		get
		{
			return this.m_resolutionIndex;
		}
		set
		{
			this.m_resolutionIndex = value;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0004289A File Offset: 0x00040A9A
	// (set) Token: 0x060009E4 RID: 2532 RVA: 0x000428A7 File Offset: 0x00040AA7
	public string ResolutionTextString
	{
		get
		{
			return this.m_resolutionText.text;
		}
		set
		{
			this.m_resolutionText.text = value;
		}
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x000428B5 File Offset: 0x00040AB5
	public void SetClickListener(UIP_ScreenResolutionSlot.ClickCallback cb)
	{
		this.m_clickCallback = cb;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x000428BE File Offset: 0x00040ABE
	public void OnSelect()
	{
		if (this.m_clickCallback != null)
		{
			this.m_clickCallback(this);
		}
	}

	// Token: 0x04000A73 RID: 2675
	public ColorByFaction m_colorizer;

	// Token: 0x04000A74 RID: 2676
	private UIP_ScreenResolutionSlot.ClickCallback m_clickCallback;

	// Token: 0x04000A75 RID: 2677
	public TextMeshProUGUI m_resolutionText;

	// Token: 0x04000A76 RID: 2678
	private int m_resolutionIndex;

	// Token: 0x020007D9 RID: 2009
	// (Invoke) Token: 0x06004349 RID: 17225
	public delegate void ClickCallback(UIP_ScreenResolutionSlot slot);
}
