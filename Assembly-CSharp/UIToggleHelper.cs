using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000106 RID: 262
[RequireComponent(typeof(Toggle))]
public class UIToggleHelper : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x00042A0A File Offset: 0x00040C0A
	private void Awake()
	{
		this.ToggleState(false);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00042A13 File Offset: 0x00040C13
	private void Init()
	{
		if (this.m_bInit)
		{
			return;
		}
		this.m_bInit = true;
		this.m_toggle = base.gameObject.GetComponent<Toggle>();
		this.m_toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleState));
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00042A54 File Offset: 0x00040C54
	private void ToggleState(bool bIsOn = false)
	{
		this.Init();
		bIsOn = this.m_toggle.isOn;
		if (this.m_objsToToggle != null && this.m_objsToToggle.Length != 0)
		{
			for (int i = 0; i < this.m_objsToToggle.Length; i++)
			{
				if (this.m_objsToToggle[i] != null)
				{
					this.m_objsToToggle[i].SetActive(bIsOn);
				}
			}
		}
		if (this.m_textToColor != null && this.m_textToColor.Length != 0)
		{
			for (int j = 0; j < this.m_textToColor.Length; j++)
			{
				if (this.m_textToColor[j] != null)
				{
					this.m_textToColor[j].color = (bIsOn ? this.m_textColorOn : this.m_textColorOff);
				}
			}
		}
		if (this.m_bUseObjectScale && this.m_objToScale != null)
		{
			this.m_objToScale.transform.localScale = (bIsOn ? this.m_objectScaleOn : this.m_objectScaleOff);
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00042B3F File Offset: 0x00040D3F
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Init();
		if (EventSystem.current.currentSelectedGameObject == this.m_toggle.gameObject)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	// Token: 0x04000A7F RID: 2687
	public GameObject[] m_objsToToggle;

	// Token: 0x04000A80 RID: 2688
	public TextMeshProUGUI[] m_textToColor;

	// Token: 0x04000A81 RID: 2689
	public Color m_textColorOn = new Color(1f, 1f, 1f);

	// Token: 0x04000A82 RID: 2690
	public Color m_textColorOff = new Color(0.5f, 0.5f, 0.5f);

	// Token: 0x04000A83 RID: 2691
	public bool m_bUseObjectScale;

	// Token: 0x04000A84 RID: 2692
	public GameObject m_objToScale;

	// Token: 0x04000A85 RID: 2693
	public Vector3 m_objectScaleOn = Vector3.one;

	// Token: 0x04000A86 RID: 2694
	public Vector3 m_objectScaleOff = Vector3.one;

	// Token: 0x04000A87 RID: 2695
	private Toggle m_toggle;

	// Token: 0x04000A88 RID: 2696
	private bool m_bInit;
}
