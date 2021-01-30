using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class UIP_SoloSeriesGraphX : MonoBehaviour
{
	// Token: 0x060009E8 RID: 2536 RVA: 0x00020E80 File Offset: 0x0001F080
	public void SetActive(bool bActive)
	{
		base.gameObject.SetActive(bActive);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x000428D4 File Offset: 0x00040AD4
	public void SetIsCurrentGame(bool bCurrent)
	{
		this.m_labelText.color = (bCurrent ? this.m_selectedLabelColor : this.m_unselectedColor);
		this.m_dotText.color = (bCurrent ? this.m_selectedDotColor : this.m_unselectedColor);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0004290E File Offset: 0x00040B0E
	public void SetLabelText(string text)
	{
		this.m_labelText.text = text;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0004291C File Offset: 0x00040B1C
	public void ShowDot(bool bShow)
	{
		this.m_dotObj.SetActive(bShow);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0004292A File Offset: 0x00040B2A
	public void SetDotText(string text)
	{
		this.m_dotText.text = text;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00042938 File Offset: 0x00040B38
	public void Reposition(float relativeVal)
	{
		if (this.m_root.rect.height == 0f && relativeVal != 0f)
		{
			base.StartCoroutine(this.Delay(relativeVal));
		}
		float y = this.m_root.rect.height * relativeVal;
		this.m_dotObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x000429A6 File Offset: 0x00040BA6
	private IEnumerator Delay(float relativeVal)
	{
		while (this.m_root.rect.height == 0f)
		{
			yield return new WaitForEndOfFrame();
		}
		float y = this.m_root.rect.height * relativeVal;
		this.m_dotObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);
		yield break;
	}

	// Token: 0x04000A77 RID: 2679
	public RectTransform m_root;

	// Token: 0x04000A78 RID: 2680
	public GameObject m_dotObj;

	// Token: 0x04000A79 RID: 2681
	public TextMeshProUGUI m_dotText;

	// Token: 0x04000A7A RID: 2682
	public TextMeshProUGUI m_labelText;

	// Token: 0x04000A7B RID: 2683
	public Color m_selectedDotColor = Color.green;

	// Token: 0x04000A7C RID: 2684
	public Color m_selectedLabelColor = Color.yellow;

	// Token: 0x04000A7D RID: 2685
	public Color m_unselectedColor = Color.white;
}
