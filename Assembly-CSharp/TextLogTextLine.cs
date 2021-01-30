using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000073 RID: 115
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLogTextLine : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x0002CFF9 File Offset: 0x0002B1F9
	private void Awake()
	{
		this.m_text = base.gameObject.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0002D00C File Offset: 0x0002B20C
	public void SetParent(Popup_TextLog parent)
	{
		this.m_parent = parent;
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0002D015 File Offset: 0x0002B215
	public uint GetIndex()
	{
		return this.m_Index;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002D01D File Offset: 0x0002B21D
	public void SetTextLine(string textLine, uint index)
	{
		if (this.m_text == null)
		{
			this.m_text = base.gameObject.GetComponent<TextMeshProUGUI>();
		}
		this.m_text.text = textLine;
		this.m_Index = index;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002D054 File Offset: 0x0002B254
	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(this.m_text, eventData.pressPosition, Camera.main);
		if (num != -1)
		{
			TMP_LinkInfo tmp_LinkInfo = this.m_text.textInfo.linkInfo[num];
			if (tmp_LinkInfo.GetLinkID() == "card")
			{
				string linkText = tmp_LinkInfo.GetLinkText();
				Debug.Log("Detected click on card name: " + linkText);
				this.m_parent.HandleClickOnCardName(base.gameObject, linkText);
				return;
			}
			if (tmp_LinkInfo.GetLinkID() == "action")
			{
				string linkText2 = tmp_LinkInfo.GetLinkText();
				Debug.Log("Detected click on action name: " + linkText2);
				this.m_parent.HandleClickOnActionName(linkText2);
			}
		}
	}

	// Token: 0x040005AE RID: 1454
	private Popup_TextLog m_parent;

	// Token: 0x040005AF RID: 1455
	private TextMeshProUGUI m_text;

	// Token: 0x040005B0 RID: 1456
	private uint m_Index;
}
