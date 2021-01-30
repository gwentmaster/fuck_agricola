using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class AgricolaImprovementField : AgricolaImprovementBase
{
	// Token: 0x060003D3 RID: 979 RVA: 0x0001AA2B File Offset: 0x00018C2B
	public int GetSowingIndex()
	{
		return this.m_sowingIndex;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001AA34 File Offset: 0x00018C34
	private void LateUpdate()
	{
		if (this.m_fieldAnimator != null && this.m_fieldAnimator.GetInteger("Count") != this.m_resCount && this.m_fieldAnimator.GetInteger("Type") != this.m_resTypeAni)
		{
			this.m_fieldAnimator.SetInteger("Type", this.m_resTypeAni);
			this.m_fieldAnimator.SetInteger("Count", this.m_resCount);
			GameObject gameObject = (base.transform.parent != null) ? base.transform.parent.gameObject : null;
			if (gameObject != null)
			{
				AgricolaFarmTile_Base component = gameObject.GetComponent<AgricolaFarmTile_Base>();
				if (component != null)
				{
					this.m_fieldAnimator.SetBool("IsHarvest", component.GetCurrentSeason() == EAgricolaSeason.AUTUMN);
				}
			}
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001AB10 File Offset: 0x00018D10
	public override void UpdateFieldData(int resType, int resCount, int sowingIndex)
	{
		this.m_sowingIndex = sowingIndex;
		if (this.m_signObj != null && this.m_signText != null)
		{
			this.m_signObj.SetActive(resCount != 0);
			this.m_signText.text = resCount.ToString();
		}
		if (this.m_fieldAnimator != null)
		{
			int num = 0;
			if (resType != 1)
			{
				if (resType != 5)
				{
					if (resType == 6)
					{
						num = 2;
					}
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 3;
			}
			this.m_fieldAnimator.SetInteger("Type", num);
			this.m_fieldAnimator.SetInteger("Count", resCount);
			this.m_resType = resType;
			this.m_resTypeAni = num;
			this.m_resCount = resCount;
			GameObject gameObject = (base.transform.parent != null) ? base.transform.parent.gameObject : null;
			if (gameObject != null)
			{
				AgricolaFarmTile_Base component = gameObject.GetComponent<AgricolaFarmTile_Base>();
				if (component != null)
				{
					this.m_fieldAnimator.SetBool("IsHarvest", component.GetCurrentSeason() == EAgricolaSeason.AUTUMN);
				}
			}
		}
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001AC1C File Offset: 0x00018E1C
	public override void UpdateSelectionState(bool bHighlight)
	{
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		AgricolaFarm farm = component.GetFarm();
		if (!bHighlight)
		{
			return;
		}
		if (!GameOptions.IsSelectableHint(41017))
		{
			for (int i = 0; i < this.m_createdResources.Count; i++)
			{
				if (this.m_createdResources[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_createdResources[i].gameObject);
				}
			}
			this.m_createdResources.Clear();
		}
		if (farm.GetIsDisplayingLocalPlayer() && GameOptions.IsSelectableHint(41017) && this.m_createdResources.Count == 0)
		{
			PlayerInterface interfaceStatic = component.GetPlayerInterfaceByLocalPlayerOrder(0).GetInterfaceStatic();
			int instanceIDFromHint = (int)GameOptions.GetInstanceIDFromHint(41017);
			if (this.m_resCount >= instanceIDFromHint)
			{
				AgricolaResource agricolaResource = interfaceStatic.CreateResourceToken((EResourceType)this.m_resType, base.gameObject, this.m_sowingIndex + 1);
				agricolaResource.SetResourceValue(this.m_resType, 1);
				this.m_createdResources.Add(agricolaResource);
			}
		}
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001AD18 File Offset: 0x00018F18
	public override void DragStartCallback(DragObject dragObject)
	{
		if (this.m_linkedCardInPlay != null && dragObject != null)
		{
			AgricolaResource component = dragObject.GetComponent<AgricolaResource>();
			if (GameOptions.IsSelectableInstanceIDWithHint((ushort)this.m_linkedCardInPlay.GetCardInPlayInstanceID(), 40979) && AgricolaLib.GetSowLocationOptionIndex(this.m_sowingIndex, component.GetResourceType()) != -1)
			{
				base.SetDragSelectionHint(40979, Color.white, (ushort)(this.m_sowingIndex + 1));
			}
			if (component != null && GameOptions.IsSelectableHint(41017) && this.m_resCount == 0 && AgricolaLib.GetSowLocationOptionIndex(this.m_sowingIndex, component.GetResourceType()) != -1)
			{
				base.SetDragSelectionHint(41017, Color.white, (ushort)(this.m_sowingIndex + 1));
			}
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001ADD8 File Offset: 0x00018FD8
	public override void DragEndCallback(DragObject dragObject)
	{
		base.SetDragSelectionHint(0, Color.white, 0);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001ADE7 File Offset: 0x00018FE7
	public override void OnDropCallback(DragObject dragObject, ushort selectionHint)
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>().GetFarm().OnDropWorker(null, selectionHint, dragObject);
	}

	// Token: 0x04000315 RID: 789
	[SerializeField]
	private Animator m_fieldAnimator;

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private GameObject m_signObj;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private TextMeshProUGUI m_signText;

	// Token: 0x04000318 RID: 792
	private int m_sowingIndex = -1;

	// Token: 0x04000319 RID: 793
	private int m_resType;

	// Token: 0x0400031A RID: 794
	private int m_resTypeAni;

	// Token: 0x0400031B RID: 795
	private int m_resCount;

	// Token: 0x0400031C RID: 796
	private List<AgricolaResource> m_createdResources = new List<AgricolaResource>();
}
