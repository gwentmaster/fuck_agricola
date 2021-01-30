using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200002D RID: 45
public class AgricolaBuilding : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x060001DE RID: 478 RVA: 0x0000A668 File Offset: 0x00008868
	private void Awake()
	{
		this.m_ActiveActionSpaces = new List<AgricolaActionSpace>();
		if (this.m_MapController == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("TownMapController");
			if (gameObject != null)
			{
				this.m_MapController = gameObject.GetComponent<TransformMap>();
			}
		}
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000A6B0 File Offset: 0x000088B0
	private void ShowBuildingVisuals(bool bActive, int playerCount, bool bIsFamilyGame)
	{
		GameObject gameObject = null;
		if (this.m_BackgroundFamilyPlayerCount != null)
		{
			if (bIsFamilyGame && playerCount < this.m_BackgroundFamilyPlayerCount.Length)
			{
				gameObject = this.m_BackgroundFamilyPlayerCount[playerCount];
			}
			for (int i = 0; i < this.m_BackgroundFamilyPlayerCount.Length; i++)
			{
				if (this.m_BackgroundFamilyPlayerCount[i] != null)
				{
					this.m_BackgroundFamilyPlayerCount[i].SetActive(bActive && this.m_BackgroundFamilyPlayerCount[i] == gameObject);
				}
			}
		}
		if (this.m_BackgroundPlayerCount != null)
		{
			if (gameObject == null && !bIsFamilyGame && playerCount < this.m_BackgroundPlayerCount.Length)
			{
				gameObject = this.m_BackgroundPlayerCount[playerCount];
			}
			for (int j = 0; j < this.m_BackgroundPlayerCount.Length; j++)
			{
				if (this.m_BackgroundPlayerCount[j] != null)
				{
					this.m_BackgroundPlayerCount[j].SetActive(bActive && this.m_BackgroundPlayerCount[j] == gameObject);
				}
			}
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000A790 File Offset: 0x00008990
	public void SetBuildingActive(bool bActive)
	{
		base.gameObject.SetActive(bActive);
		if (this.m_BackgroundVisuals != null)
		{
			for (int i = 0; i < this.m_BackgroundVisuals.Length; i++)
			{
				if (this.m_BackgroundVisuals[i] != null)
				{
					this.m_BackgroundVisuals[i].SetActive(bActive);
				}
			}
		}
		int gamePlayerCount = AgricolaLib.GetGamePlayerCount();
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		bool bIsFamilyGame = gameParameters.gameType == 0 || gameParameters.gameType == 7;
		this.ShowBuildingVisuals(bActive, gamePlayerCount, bIsFamilyGame);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000A820 File Offset: 0x00008A20
	public AgricolaActionSpace FindActionSpace(string actionspace_name)
	{
		if (this.m_BuildingActionSpaceList == null)
		{
			return null;
		}
		for (int i = 0; i < this.m_BuildingActionSpaceList.Length; i++)
		{
			if (this.m_BuildingActionSpaceList[i] != null && this.m_BuildingActionSpaceList[i].IsActionSpaceName(actionspace_name))
			{
				return this.m_BuildingActionSpaceList[i];
			}
		}
		return null;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000A874 File Offset: 0x00008A74
	public void ClearActiveActionSpaces()
	{
		if (this.m_BuildingActionSpaceList != null)
		{
			for (int i = 0; i < this.m_BuildingActionSpaceList.Length; i++)
			{
				if (this.m_BuildingActionSpaceList[i] != null)
				{
					this.m_BuildingActionSpaceList[i].SetLockedPopupCard(false);
					this.m_BuildingActionSpaceList[i].SetActionSpaceActive(false);
				}
			}
		}
		if (this.m_ActiveActionSpaces != null)
		{
			this.m_ActiveActionSpaces.Clear();
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000A8DC File Offset: 0x00008ADC
	public bool AddActiveActionSpace(AgricolaActionSpace actionspace)
	{
		bool flag = false;
		if (this.m_BuildingActionSpaceList != null)
		{
			for (int i = 0; i < this.m_BuildingActionSpaceList.Length; i++)
			{
				if (this.m_BuildingActionSpaceList[i] == actionspace)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		if (this.m_ActiveActionSpaces != null && !this.m_ActiveActionSpaces.Contains(actionspace))
		{
			this.m_ActiveActionSpaces.Add(actionspace);
			actionspace.SetActionSpaceActive(true);
			return true;
		}
		return false;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000A94C File Offset: 0x00008B4C
	public AgricolaActionSpace SetLockedActionSpaces()
	{
		if (this.m_LockedActionSpace == null)
		{
			return null;
		}
		if (this.m_ActiveActionSpaces != null && this.m_ActiveActionSpaces.Count > 0)
		{
			return null;
		}
		this.SetBuildingActive(true);
		AgricolaActionSpace agricolaActionSpace = this.m_LockedActionSpace;
		if (this.m_BuildingActionSpaceList != null && this.m_BuildingActionSpaceList.Length > 1)
		{
			GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
			if (gameParameters.gameType != 0 && gameParameters.gameType != 7)
			{
				agricolaActionSpace = this.m_BuildingActionSpaceList[1];
			}
		}
		this.AddActiveActionSpace(agricolaActionSpace);
		agricolaActionSpace.ShowActionSpaceSignPosts(false);
		agricolaActionSpace.SetLockedPopupCard(true);
		agricolaActionSpace.SetAccumulateResourceCount(0);
		this.ShowBuildingVisuals(false, 0, false);
		return agricolaActionSpace;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000AA04 File Offset: 0x00008C04
	public void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (animation_manager == null)
		{
			return;
		}
		if (this.m_ActiveActionSpaces != null)
		{
			foreach (AgricolaActionSpace agricolaActionSpace in this.m_ActiveActionSpaces)
			{
				if (agricolaActionSpace != null)
				{
					agricolaActionSpace.RebuildAnimationManager(animation_manager);
				}
			}
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000AA74 File Offset: 0x00008C74
	public void ToggleHelpObject(bool bSetState)
	{
		if (this.m_ActiveActionSpaces != null)
		{
			for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
			{
				if (this.m_ActiveActionSpaces[i] != null)
				{
					this.m_ActiveActionSpaces[i].ActivatePopupCard(bSetState);
				}
			}
		}
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000AAC8 File Offset: 0x00008CC8
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.m_ActiveActionSpaces != null && eventData.pointerDrag == null)
		{
			for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
			{
				if (this.m_ActiveActionSpaces[i] != null)
				{
					this.m_ActiveActionSpaces[i].TogglePopupCard();
				}
			}
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000AB26 File Offset: 0x00008D26
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (this.m_MapController != null)
			{
				this.m_MapController.OnDragStart();
				this.m_bIsDraggingMap = true;
			}
			this.m_pointerDownTime = Time.fixedTime;
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000AB5C File Offset: 0x00008D5C
	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (this.m_MapController != null && this.m_bIsDraggingMap)
			{
				this.m_MapController.OnDragEnd();
				this.m_bIsDraggingMap = false;
			}
			if (Time.fixedTime - this.m_pointerDownTime <= this.m_deltaTimeForClick && this.m_ActiveActionSpaces != null)
			{
				for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
				{
					if (this.m_ActiveActionSpaces[i] != null)
					{
						this.m_ActiveActionSpaces[i].TogglePopupCard();
					}
				}
			}
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000ABF0 File Offset: 0x00008DF0
	public void OnDrag(PointerEventData eventData)
	{
		if (this.m_MapController != null && this.m_bIsDraggingMap)
		{
			this.m_MapController.OnDrag();
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000AC14 File Offset: 0x00008E14
	public void OnPointerEnter(PointerEventData eventData)
	{
		AgricolaWorker x = null;
		if (eventData.pointerDrag != null)
		{
			x = eventData.pointerDrag.GetComponent<AgricolaWorker>();
		}
		if (x != null && this.m_ActiveActionSpaces != null)
		{
			for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
			{
				if (this.m_ActiveActionSpaces[i] != null)
				{
					this.m_ActiveActionSpaces[i].ActivatePopupCard(true);
				}
			}
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000AC8C File Offset: 0x00008E8C
	public void OnPointerExit(PointerEventData eventData)
	{
		AgricolaWorker x = null;
		if (eventData.pointerDrag != null)
		{
			x = eventData.pointerDrag.GetComponent<AgricolaWorker>();
		}
		if (x != null && this.m_ActiveActionSpaces != null)
		{
			for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
			{
				if (this.m_ActiveActionSpaces[i] != null)
				{
					this.m_ActiveActionSpaces[i].ActivatePopupCard(false);
				}
			}
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000AD04 File Offset: 0x00008F04
	public void OnDrop(PointerEventData eventData)
	{
		AgricolaWorker x = null;
		if (eventData.pointerDrag != null)
		{
			x = eventData.pointerDrag.GetComponent<AgricolaWorker>();
		}
		if (x != null && this.m_ActiveActionSpaces != null)
		{
			for (int i = 0; i < this.m_ActiveActionSpaces.Count; i++)
			{
				if (this.m_ActiveActionSpaces[i] != null)
				{
					this.m_ActiveActionSpaces[i].ActivatePopupCard(false);
				}
			}
		}
	}

	// Token: 0x0400015F RID: 351
	[SerializeField]
	private GameObject m_WorkerSpaceGlow;

	// Token: 0x04000160 RID: 352
	[SerializeField]
	private AgricolaActionSpace[] m_BuildingActionSpaceList;

	// Token: 0x04000161 RID: 353
	[SerializeField]
	private AgricolaActionSpace m_LockedActionSpace;

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private GameObject[] m_BackgroundVisuals;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	private GameObject[] m_BackgroundFamilyPlayerCount;

	// Token: 0x04000164 RID: 356
	[SerializeField]
	private GameObject[] m_BackgroundPlayerCount;

	// Token: 0x04000165 RID: 357
	private List<AgricolaActionSpace> m_ActiveActionSpaces;

	// Token: 0x04000166 RID: 358
	private TransformMap m_MapController;

	// Token: 0x04000167 RID: 359
	private bool m_bIsDraggingMap;

	// Token: 0x04000168 RID: 360
	private float m_pointerDownTime;

	// Token: 0x04000169 RID: 361
	private float m_deltaTimeForClick = 0.5f;
}
