using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005F RID: 95
public class GlowColorAdjust : MonoBehaviour
{
	// Token: 0x06000513 RID: 1299 RVA: 0x00027A02 File Offset: 0x00025C02
	private void Awake()
	{
		this.SetDefaultColor();
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00027A0A File Offset: 0x00025C0A
	public void SetDefaultColor()
	{
		this.SetColor(this.m_DefaultColor);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00027A18 File Offset: 0x00025C18
	public void SetColor(Color newColor)
	{
		this.m_CurrentColor = newColor;
		if (this.m_GlowImageList != null)
		{
			for (int i = 0; i < this.m_GlowImageList.Length; i++)
			{
				Image image = this.m_GlowImageList[i];
				if (image != null)
				{
					Color color = image.color;
					color.r = this.m_CurrentColor.r;
					color.g = this.m_CurrentColor.g;
					color.b = this.m_CurrentColor.b;
					image.color = color;
				}
			}
		}
	}

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private Color m_DefaultColor = Color.white;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private Image[] m_GlowImageList;

	// Token: 0x040004BD RID: 1213
	private Color m_CurrentColor;
}
