using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003E RID: 62
public class AgricolaFeedingBubble : MonoBehaviour
{
	// Token: 0x06000350 RID: 848 RVA: 0x00015B01 File Offset: 0x00013D01
	public int GetFoodRequirement()
	{
		return this.m_foodRequired;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00015B09 File Offset: 0x00013D09
	public int GetFoodGiven()
	{
		return this.m_foodGiven;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00015B11 File Offset: 0x00013D11
	public bool GetIsDraggingFrom()
	{
		return this.m_bIsDraggingFood;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00015B1C File Offset: 0x00013D1C
	public void Init(AgricolaFarm farm, int tokenIndex, DragManager dragManager)
	{
		this.m_farm = farm;
		this.m_workerTokenIndex = tokenIndex;
		this.m_dragManager = dragManager;
		this.m_dragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.BeginDragCallback));
		this.m_dragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.EndDragCallback));
		this.m_foodToken.SetResourceValue(0, 1);
		this.m_foodToken.SetDragManager(this.m_dragManager);
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		this.m_foodToken.Colorize((uint)component.GetLocalPlayerColorIndex());
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015BAB File Offset: 0x00013DAB
	private void OnDestroy()
	{
		if (this.m_dragManager != null)
		{
			this.m_dragManager.RemoveOnBeginDragCallback(new DragManager.DragManagerCallback(this.BeginDragCallback));
			this.m_dragManager.RemoveOnEndDragCallback(new DragManager.DragManagerCallback(this.EndDragCallback));
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00015BEC File Offset: 0x00013DEC
	public void SetFoodData(int required, int given, AgricolaResource droppedRes = null)
	{
		this.m_bIsDraggingFood = false;
		this.m_foodRequired = required;
		this.m_foodGiven = given;
		for (int i = 0; i < this.m_foodRequiredIcons.Length; i++)
		{
			this.m_foodRequiredIcons[i].SetActive(i < this.m_foodRequired);
		}
		for (int j = 0; j < this.m_foodGivenIcons.Length; j++)
		{
			this.m_foodGivenIcons[j].SetActive(j < this.m_foodGiven);
		}
		for (int k = 0; k < this.m_foodGivenAnimators.Length; k++)
		{
			this.m_foodGivenAnimators[k].enabled = (k == this.m_foodGiven - 1);
		}
		this.m_foodToken.SetIsDraggable(this.m_foodGiven > 0);
		if (droppedRes != null && droppedRes == this.m_foodToken)
		{
			this.m_foodToken.transform.SetParent(this.m_foodTokenLocator.transform);
			this.m_foodToken.transform.localPosition = Vector3.zero;
			this.m_foodToken.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00015CFC File Offset: 0x00013EFC
	public void BeginDragCallback(DragObject dragObject)
	{
		if (this.m_foodToken != null && dragObject.gameObject == this.m_foodToken.gameObject)
		{
			this.m_bIsDraggingFood = true;
			this.m_foodGivenIcons[this.m_foodGiven - 1].SetActive(false);
		}
		AgricolaResource component = dragObject.GetComponent<AgricolaResource>();
		if (component != null && component.GetResourceType() == 0 && (this.m_bIsDraggingFood || this.m_foodRequired > this.m_foodGiven) && this.m_farm.GetIsFeedingMode())
		{
			this.m_dragTarget.SetDragSelectionHint(1, Color.clear, (ushort)(this.m_workerTokenIndex + 1));
			this.m_raycastImage.raycastTarget = true;
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00015DAC File Offset: 0x00013FAC
	public void EndDragCallback(DragObject dragObject)
	{
		this.m_dragTarget.SetDragSelectionHint(0, Color.clear, 0);
		this.m_raycastImage.raycastTarget = false;
	}

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	private Image m_raycastImage;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private DragTargetZone m_dragTarget;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private AgricolaResource m_foodToken;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private GameObject m_foodTokenLocator;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private GameObject[] m_foodRequiredIcons;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private GameObject[] m_foodGivenIcons;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private Animator[] m_foodGivenAnimators;

	// Token: 0x040002A8 RID: 680
	private AgricolaFarm m_farm;

	// Token: 0x040002A9 RID: 681
	private DragManager m_dragManager;

	// Token: 0x040002AA RID: 682
	private int m_workerTokenIndex;

	// Token: 0x040002AB RID: 683
	private int m_foodRequired;

	// Token: 0x040002AC RID: 684
	private int m_foodGiven;

	// Token: 0x040002AD RID: 685
	private bool m_bIsDraggingFood;
}
