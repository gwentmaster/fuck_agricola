using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000048 RID: 72
public class AgricolaOptionPopup_Resource : MonoBehaviour
{
	// Token: 0x06000419 RID: 1049 RVA: 0x00020E46 File Offset: 0x0001F046
	public int GetDisplayedResourceCount()
	{
		return this.m_resCountNum;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00020E4E File Offset: 0x0001F04E
	public void AddToggleChangeCallback(AgricolaOptionPopup_Resource.ToggleChangeCallback callback)
	{
		this.m_callback = (AgricolaOptionPopup_Resource.ToggleChangeCallback)Delegate.Combine(this.m_callback, callback);
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00020E67 File Offset: 0x0001F067
	public void RemoveToggleChangeCallback(AgricolaOptionPopup_Resource.ToggleChangeCallback callback)
	{
		this.m_callback = (AgricolaOptionPopup_Resource.ToggleChangeCallback)Delegate.Remove(this.m_callback, callback);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00020E80 File Offset: 0x0001F080
	public void SetActive(bool bActive)
	{
		base.gameObject.SetActive(bActive);
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00020E90 File Offset: 0x0001F090
	public void SetResourceData(EResourceType type, int count)
	{
		if (type < (EResourceType)this.m_resourceSprites.Length && this.m_resourceImage != null)
		{
			this.m_resourceImage.sprite = this.m_resourceSprites[(int)type];
		}
		if (this.m_resourceCount != null)
		{
			this.m_resourceCount.text = count.ToString();
		}
		this.m_resCountNum = count;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00020EF0 File Offset: 0x0001F0F0
	public void SetArrows(bool bLeftOn, bool bRightOn)
	{
		if (this.m_leftArrow != null)
		{
			this.m_leftArrow.SetActive(bLeftOn);
		}
		if (this.m_rightArrow != null)
		{
			this.m_rightArrow.SetActive(bRightOn);
		}
		if (this.m_leftBase != null && this.m_leftArrow != null && this.m_toggleObj != null)
		{
			this.m_leftBase.SetActive(this.m_leftArrow.activeSelf || this.m_toggleObj.activeSelf);
		}
		if (this.m_resourceCount != null && this.m_resourceCountGlow != null)
		{
			if (bLeftOn || bRightOn)
			{
				this.m_resourceCount.gameObject.SetActive(false);
				this.m_resourceCountGlow.gameObject.SetActive(false);
				return;
			}
			this.m_resourceCount.gameObject.SetActive(true);
			this.m_resourceCountGlow.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00020FE7 File Offset: 0x0001F1E7
	public void SetArrowInteractable(bool bLeftOn, bool bRightOn)
	{
		if (this.m_leftArrow != null)
		{
			this.m_leftArrow.GetComponent<Button>().interactable = bLeftOn;
		}
		if (this.m_rightArrow != null)
		{
			this.m_rightArrow.GetComponent<Button>().interactable = bRightOn;
		}
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00021028 File Offset: 0x0001F228
	public void SetInteractable(bool bIsOn)
	{
		if (this.m_toggle != null)
		{
			this.m_toggle.enabled = bIsOn;
			this.m_toggle.interactable = bIsOn;
		}
		if (this.m_toggleObj != null)
		{
			this.m_toggleObj.SetActive(bIsOn);
		}
		if (this.m_resourceImage != null)
		{
			this.m_resourceImage.raycastTarget = bIsOn;
		}
		if (this.m_leftBase != null && this.m_leftArrow != null && this.m_toggleObj != null)
		{
			this.m_leftBase.SetActive(this.m_leftArrow.activeSelf || this.m_toggleObj.activeSelf);
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000210DF File Offset: 0x0001F2DF
	public void SetOppTokenVisible(bool bVisible)
	{
		if (this.m_oppRoot != null)
		{
			this.m_oppRoot.SetActive(bVisible);
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000210FC File Offset: 0x0001F2FC
	public void SetOppTokenData(int oppInstanceID, AgricolaGame gameController)
	{
		if (this.m_oppAvatar != null && this.m_oppColorizer != null && gameController != null)
		{
			PlayerDisplay_UpperHud upperHud = gameController.GetUpperHud();
			if (upperHud != null)
			{
				this.m_oppAvatar.SetAvatar(upperHud.FindAvatarIndexByInstanceID(oppInstanceID), false);
				this.m_oppColorizer.Colorize((uint)upperHud.FindFactionIndexByInstanceID(oppInstanceID));
			}
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00021163 File Offset: 0x0001F363
	public bool GetIsSelected()
	{
		return this.m_toggle != null && this.m_toggle.interactable && this.m_toggle.isOn;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0002118D File Offset: 0x0001F38D
	public void SetIsSelected(bool bIsOn)
	{
		if (this.m_toggle != null)
		{
			this.m_toggle.isOn = bIsOn;
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000211AC File Offset: 0x0001F3AC
	public void HandleArrowLeft()
	{
		this.m_resCountNum--;
		if (this.m_resourceCount != null)
		{
			this.m_resourceCount.text = this.m_resCountNum.ToString();
		}
		if (this.m_callback != null)
		{
			this.m_callback(this, true);
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00021200 File Offset: 0x0001F400
	public void HandleArrowRight()
	{
		this.m_resCountNum++;
		if (this.m_resourceCount != null)
		{
			this.m_resourceCount.text = this.m_resCountNum.ToString();
		}
		if (this.m_callback != null)
		{
			this.m_callback(this, true);
		}
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00021254 File Offset: 0x0001F454
	public void HandleToggle(bool bState)
	{
		if (this.m_bIgnoreToggle)
		{
			return;
		}
		this.m_bIgnoreToggle = true;
		if (this.m_callback != null)
		{
			this.m_callback(this, bState);
		}
		this.m_bIgnoreToggle = false;
	}

	// Token: 0x040003AA RID: 938
	[SerializeField]
	private Image m_resourceImage;

	// Token: 0x040003AB RID: 939
	[SerializeField]
	private TextMeshProUGUI m_resourceCount;

	// Token: 0x040003AC RID: 940
	[SerializeField]
	private GameObject m_resourceCountGlow;

	// Token: 0x040003AD RID: 941
	[SerializeField]
	private Toggle m_toggle;

	// Token: 0x040003AE RID: 942
	[SerializeField]
	private GameObject m_toggleObj;

	// Token: 0x040003AF RID: 943
	[SerializeField]
	private GameObject m_leftArrow;

	// Token: 0x040003B0 RID: 944
	[SerializeField]
	private GameObject m_rightArrow;

	// Token: 0x040003B1 RID: 945
	[SerializeField]
	private GameObject m_leftBase;

	// Token: 0x040003B2 RID: 946
	[SerializeField]
	private GameObject m_oppRoot;

	// Token: 0x040003B3 RID: 947
	[SerializeField]
	private Avatar_UI m_oppAvatar;

	// Token: 0x040003B4 RID: 948
	[SerializeField]
	private ColorByFaction m_oppColorizer;

	// Token: 0x040003B5 RID: 949
	[SerializeField]
	private Sprite[] m_resourceSprites;

	// Token: 0x040003B6 RID: 950
	private AgricolaOptionPopup_Resource.ToggleChangeCallback m_callback;

	// Token: 0x040003B7 RID: 951
	private bool m_bIgnoreToggle;

	// Token: 0x040003B8 RID: 952
	private int m_resCountNum;

	// Token: 0x02000763 RID: 1891
	// (Invoke) Token: 0x060041C5 RID: 16837
	public delegate void ToggleChangeCallback(AgricolaOptionPopup_Resource res, bool bIsOn);
}
