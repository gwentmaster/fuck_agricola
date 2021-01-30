using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000054 RID: 84
public class ExcessAnimalTray : MonoBehaviour
{
	// Token: 0x060004BD RID: 1213 RVA: 0x00025260 File Offset: 0x00023460
	public void Reset()
	{
		this.Init();
		this.m_animalCounts[0] = (this.m_animalCounts[1] = (this.m_animalCounts[2] = 0));
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00025294 File Offset: 0x00023494
	public void ModifyExcessAnimalCount(EResourceType animalType, int modifier)
	{
		this.Init();
		if (animalType == EResourceType.SHEEP)
		{
			this.m_animalCounts[0] += modifier;
			return;
		}
		if (animalType == EResourceType.WILDBOAR)
		{
			this.m_animalCounts[1] += modifier;
			return;
		}
		if (animalType == EResourceType.CATTLE)
		{
			this.m_animalCounts[2] += modifier;
		}
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000252E9 File Offset: 0x000234E9
	public bool GetHasExcessAnimals()
	{
		this.Init();
		return this.m_animalCounts[0] != 0 || this.m_animalCounts[1] != 0 || this.m_animalCounts[2] != 0;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00025312 File Offset: 0x00023512
	public int GetExcessAnimalCount(EResourceType animalType)
	{
		if (animalType == EResourceType.SHEEP)
		{
			return this.m_animalCounts[0];
		}
		if (animalType == EResourceType.WILDBOAR)
		{
			return this.m_animalCounts[1];
		}
		if (animalType == EResourceType.CATTLE)
		{
			return this.m_animalCounts[2];
		}
		return 0;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00025340 File Offset: 0x00023540
	public bool GetIfAnimalIsInExcessTray(AgricolaAnimal animal)
	{
		this.Init();
		if (this.m_animalLocators == null)
		{
			return false;
		}
		GameObject gameObject = animal.gameObject;
		for (int i = 0; i < this.m_animalLocators.Length; i++)
		{
			if (this.m_animalLocators[i].transform.childCount > 0 && this.m_animalLocators[i].transform.GetChild(0).gameObject == gameObject)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000253B0 File Offset: 0x000235B0
	private void Init()
	{
		if (this.m_bInit)
		{
			return;
		}
		this.m_bInit = true;
		this.m_tray = base.gameObject.GetComponent<TrayToggle>();
		if (this.m_dragManager != null)
		{
			this.m_dragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.OnDragStart));
			this.m_dragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.OnDragEnd));
		}
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0002541C File Offset: 0x0002361C
	private void OnDragStart(DragObject dragObject)
	{
		this.m_dragTargetImage.raycastTarget = true;
		if (dragObject.GetComponent<AgricolaAnimal>() != null)
		{
			DragTargetZone component = base.gameObject.GetComponent<DragTargetZone>();
			if (component != null)
			{
				component.SetDragSelectionHint(1, Color.clear, 999);
			}
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0002546C File Offset: 0x0002366C
	private void OnDragEnd(DragObject dragObject)
	{
		this.m_dragTargetImage.raycastTarget = false;
		DragTargetZone component = base.gameObject.GetComponent<DragTargetZone>();
		if (component != null)
		{
			component.SetDragSelectionHint(0, Color.clear, 0);
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000254A8 File Offset: 0x000236A8
	private void Update()
	{
		if (!this.m_tray.IsTrayOpen())
		{
			return;
		}
		if (this.m_animalCountsText != null)
		{
			int num = 0;
			while (num < this.m_animalCounts.Length && num < this.m_animalCountsText.Length)
			{
				this.m_animalCountsText[num].text = this.m_animalCounts[num].ToString();
				num++;
			}
		}
		if (this.m_animalLocators != null && this.m_agricolaFarm != null && !this.m_agricolaFarm.GetIsDraggingAnimal())
		{
			if (this.m_animalLocators[0] != null && this.m_animalLocators[0].transform.childCount == 0 && this.m_animalCounts[0] > 0)
			{
				AgricolaAnimal animalInLimbo = this.m_agricolaFarm.GetAnimalInLimbo(EResourceType.SHEEP, true);
				animalInLimbo.transform.SetParent(this.m_animalLocators[0].transform);
				animalInLimbo.transform.localPosition = Vector3.zero;
				animalInLimbo.SetContainerIndex(-1);
			}
			if (this.m_animalLocators[1] != null && this.m_animalLocators[1].transform.childCount == 0 && this.m_animalCounts[1] > 0)
			{
				AgricolaAnimal animalInLimbo2 = this.m_agricolaFarm.GetAnimalInLimbo(EResourceType.WILDBOAR, true);
				animalInLimbo2.transform.SetParent(this.m_animalLocators[1].transform);
				animalInLimbo2.transform.localPosition = Vector3.zero;
				animalInLimbo2.SetContainerIndex(-1);
			}
			if (this.m_animalLocators[2] != null && this.m_animalLocators[2].transform.childCount == 0 && this.m_animalCounts[2] > 0)
			{
				AgricolaAnimal animalInLimbo3 = this.m_agricolaFarm.GetAnimalInLimbo(EResourceType.CATTLE, true);
				animalInLimbo3.transform.SetParent(this.m_animalLocators[2].transform);
				animalInLimbo3.transform.localPosition = Vector3.zero;
				animalInLimbo3.SetContainerIndex(-1);
			}
		}
	}

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	private AgricolaFarm m_agricolaFarm;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private DragManager m_dragManager;

	// Token: 0x04000440 RID: 1088
	[SerializeField]
	private Image m_dragTargetImage;

	// Token: 0x04000441 RID: 1089
	[Header("0 = sheep, 1 = boar, 2 = cattle")]
	[SerializeField]
	private GameObject[] m_animalLocators;

	// Token: 0x04000442 RID: 1090
	[SerializeField]
	private TextMeshProUGUI[] m_animalCountsText;

	// Token: 0x04000443 RID: 1091
	private int[] m_animalCounts = new int[3];

	// Token: 0x04000444 RID: 1092
	private bool m_bInit;

	// Token: 0x04000445 RID: 1093
	private TrayToggle m_tray;
}
