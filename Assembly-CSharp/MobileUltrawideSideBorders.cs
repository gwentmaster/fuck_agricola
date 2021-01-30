using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EA RID: 234
public class MobileUltrawideSideBorders : MonoBehaviour
{
	// Token: 0x06000895 RID: 2197 RVA: 0x0003BAB4 File Offset: 0x00039CB4
	private void Update()
	{
		if (this.m_bRun)
		{
			return;
		}
		if (this.m_mainCanvasObj == null || !this.m_mainCanvasObj.gameObject.activeInHierarchy || this.m_mainCanvasObj.rect.width == 0f || this.m_mainCanvasObj.rect.height == 0f)
		{
			return;
		}
		this.m_bRun = true;
		if (this.m_sideBorderObjs == null || this.m_sideBorderObjs.Length == 0)
		{
			return;
		}
		if (this.m_bUseVertical)
		{
			this.SetVertical();
			return;
		}
		this.SetHorizontal();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0003BB50 File Offset: 0x00039D50
	private void SetHorizontal()
	{
		bool active = (float)Screen.width / (float)Screen.height >= this.m_minimumAspectRatioToTurnOn;
		float num = this.m_setAspectRatio * this.m_mainCanvasObj.rect.height;
		float minWidth = Mathf.Abs((this.m_mainCanvasObj.rect.width - num) / (float)this.m_sideBorderObjs.Length);
		if (this.m_sideBorderObjs != null && this.m_sideBorderObjs.Length != 0)
		{
			for (int i = 0; i < this.m_sideBorderObjs.Length; i++)
			{
				if (this.m_sideBorderObjs[i] != null)
				{
					this.m_sideBorderObjs[i].SetActive(active);
					LayoutElement component = this.m_sideBorderObjs[i].GetComponent<LayoutElement>();
					if (component != null)
					{
						component.minWidth = minWidth;
					}
				}
			}
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003BC24 File Offset: 0x00039E24
	private void SetVertical()
	{
		bool active = (float)Screen.height / (float)Screen.width >= this.m_minimumAspectRatioToTurnOn;
		float num = this.m_setAspectRatio * this.m_mainCanvasObj.rect.width;
		float minHeight = (this.m_mainCanvasObj.rect.height - num) / (float)this.m_sideBorderObjs.Length;
		if (this.m_sideBorderObjs != null && this.m_sideBorderObjs.Length != 0)
		{
			for (int i = 0; i < this.m_sideBorderObjs.Length; i++)
			{
				if (this.m_sideBorderObjs[i] != null)
				{
					this.m_sideBorderObjs[i].SetActive(active);
					LayoutElement component = this.m_sideBorderObjs[i].GetComponent<LayoutElement>();
					if (component != null)
					{
						component.minHeight = minHeight;
					}
				}
			}
		}
	}

	// Token: 0x04000958 RID: 2392
	[SerializeField]
	private GameObject[] m_sideBorderObjs;

	// Token: 0x04000959 RID: 2393
	[SerializeField]
	private RectTransform m_mainCanvasObj;

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private float m_minimumAspectRatioToTurnOn = 2f;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private float m_setAspectRatio = 1.7777778f;

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private bool m_bUseVertical;

	// Token: 0x0400095D RID: 2397
	private bool m_bRun;
}
