using System;
using TMPro;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class AgricolaFarmTile_Field : AgricolaFarmTile_Base
{
	// Token: 0x0600030A RID: 778 RVA: 0x00014628 File Offset: 0x00012828
	public int GetNumCropsPlanted()
	{
		return this.m_numCropsPlanted;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00014630 File Offset: 0x00012830
	public int GetTypeCropsPlanted()
	{
		return this.m_typeCropsPlanted;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00014638 File Offset: 0x00012838
	public int GetRulesSowingLocationIndex()
	{
		return this.m_rulesSowingIndex;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00014640 File Offset: 0x00012840
	public void SetActive(bool bActive, bool bClearField = true)
	{
		base.gameObject.SetActive(bActive);
		if (bClearField)
		{
			if (this.m_signObject != null)
			{
				this.m_signObject.SetActive(false);
			}
			if (this.m_plantedCropObj != null)
			{
				UnityEngine.Object.Destroy(this.m_plantedCropObj);
				this.m_plantedCropObj = null;
				this.m_cropAnimator = null;
			}
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000146A0 File Offset: 0x000128A0
	private void LateUpdate()
	{
		if (this.m_cropAnimator != null && this.m_cropAnimator.GetInteger(this.m_animatorCropCount) != this.m_numCropsPlanted)
		{
			this.m_cropAnimator.SetBool(this.m_animatorIsAutumn, this.m_currentSeason == EAgricolaSeason.AUTUMN);
			this.m_cropAnimator.SetInteger(this.m_animatorCropCount, this.m_numCropsPlanted);
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00014705 File Offset: 0x00012905
	public override void SetSeason(EAgricolaSeason season)
	{
		base.SetSeason(season);
		if (this.m_cropAnimator != null)
		{
			this.m_cropAnimator.SetBool(this.m_animatorIsAutumn, season == EAgricolaSeason.AUTUMN);
		}
		this.m_currentSeason = season;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00014738 File Offset: 0x00012938
	public void Clear()
	{
		this.m_numCropsPlanted = 0;
		this.m_typeCropsPlanted = 0;
		if (this.m_plantedCropObj != null)
		{
			UnityEngine.Object.Destroy(this.m_plantedCropObj);
		}
		this.m_plantedCropObj = null;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00014768 File Offset: 0x00012968
	public void SetFieldData(GameObject cropObject, int cropType, int cropCount, int sowingLocationIndex)
	{
		this.m_rulesSowingIndex = sowingLocationIndex;
		if (cropObject != null)
		{
			if (this.m_plantedCropObj != null)
			{
				UnityEngine.Object.Destroy(this.m_plantedCropObj);
			}
			this.m_plantedCropObj = cropObject;
			this.m_cropAnimator = cropObject.GetComponent<Animator>();
			this.m_plantedCropObj.transform.SetParent(this.m_cropLocator.transform, false);
			this.m_plantedCropObj.transform.localPosition = Vector3.zero;
		}
		if (this.m_numCropsPlanted == 0)
		{
			bool flag = cropCount != 0;
		}
		this.m_numCropsPlanted = cropCount;
		this.m_typeCropsPlanted = cropType;
		if (this.m_cropAnimator != null)
		{
			this.m_cropAnimator.SetBool(this.m_animatorIsAutumn, this.m_currentSeason == EAgricolaSeason.AUTUMN);
			this.m_cropAnimator.SetInteger(this.m_animatorCropCount, this.m_numCropsPlanted);
		}
		if (cropCount != 0)
		{
			if (this.m_signObject != null)
			{
				this.m_signObject.SetActive(true);
			}
			if (this.m_signText != null)
			{
				this.m_signText.text = cropCount.ToString();
				return;
			}
		}
		else if (this.m_signObject != null)
		{
			this.m_signObject.SetActive(false);
		}
	}

	// Token: 0x04000262 RID: 610
	[SerializeField]
	private GameObject m_signObject;

	// Token: 0x04000263 RID: 611
	[SerializeField]
	private TextMeshProUGUI m_signText;

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private GameObject m_cropLocator;

	// Token: 0x04000265 RID: 613
	private string m_animatorCropCount = "CropCount";

	// Token: 0x04000266 RID: 614
	private string m_animatorIsAutumn = "IsAutumn";

	// Token: 0x04000267 RID: 615
	private string m_animatorClearField = "ClearField";

	// Token: 0x04000268 RID: 616
	private GameObject m_plantedCropObj;

	// Token: 0x04000269 RID: 617
	private Animator m_cropAnimator;

	// Token: 0x0400026A RID: 618
	private int m_numCropsPlanted;

	// Token: 0x0400026B RID: 619
	private int m_typeCropsPlanted;

	// Token: 0x0400026C RID: 620
	private int m_rulesSowingIndex;
}
