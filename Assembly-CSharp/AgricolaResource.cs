using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200004A RID: 74
public class AgricolaResource : DragObject, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
	// Token: 0x0600043A RID: 1082 RVA: 0x00021EFA File Offset: 0x000200FA
	public bool WasDragToTarget()
	{
		return this.m_bWasDragToTarget;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00021F02 File Offset: 0x00020102
	public bool DestroyResourceOnDragEnd()
	{
		return this.m_bDestroyOnDragEnd;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00021F0A File Offset: 0x0002010A
	public int GetResourceType()
	{
		return this.m_ResourceType;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00021F12 File Offset: 0x00020112
	public int GetResourceValue()
	{
		return this.m_ResourceValue;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00021F1A File Offset: 0x0002011A
	public void SetResourceValue(int resType, int resValue)
	{
		this.m_ResourceType = resType;
		this.m_ResourceValue = resValue;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00021F2A File Offset: 0x0002012A
	public void SetDestroyOnDragEnd(bool bDestroy)
	{
		this.m_bDestroyOnDragEnd = bDestroy;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00021F33 File Offset: 0x00020133
	private void Awake()
	{
		base.AddOnBeginDragCallback(new DragObject.DragObjectCallback(this.BeginDragCallback));
		base.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragCallback));
		base.AddOnDragHintCallback(new DragObject.DragObjectCallback(this.DragHintCallback));
		this.SetIsDraggable(false);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00021F72 File Offset: 0x00020172
	private void Start()
	{
		this.UpdateHighlight();
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00021F7C File Offset: 0x0002017C
	private void UpdateHighlight()
	{
		if (this.m_Highlight != null)
		{
			this.m_Highlight.SetActive(this.m_bActiveHighlight);
		}
		if (this.m_HighlightAnimator != null && this.m_HighlightAnimator.isInitialized)
		{
			this.m_HighlightAnimator.SetBool("IsActive", this.m_bActiveHighlight);
		}
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00021FD9 File Offset: 0x000201D9
	public void ActivateHighlight(bool bActiveHighlight)
	{
		this.m_bActiveHighlight = bActiveHighlight;
		this.UpdateHighlight();
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00021FE8 File Offset: 0x000201E8
	public override void SetIsDraggable(bool bDraggable)
	{
		base.SetIsDraggable(bDraggable);
		if (this.m_ResourceDraggableToken != null)
		{
			this.m_ResourceDraggableToken.SetActive(bDraggable);
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0002200B File Offset: 0x0002020B
	public void Colorize(uint factionIndex)
	{
		if (this.m_ResourceDraggableColorizer != null)
		{
			this.m_ResourceDraggableColorizer.Colorize(factionIndex);
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00022027 File Offset: 0x00020227
	public int GetGameOptionIndex()
	{
		return this.m_GameOptionIndex;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0002202F File Offset: 0x0002022F
	public void SetGameOptionIndex(int index)
	{
		this.m_GameOptionIndex = index;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00022038 File Offset: 0x00020238
	public void SetDisplayCount(int count)
	{
		this.m_DisplayCount = count;
		if (this.m_ResourceCountText != null)
		{
			this.m_ResourceCountText.text = this.m_DisplayCount.ToString();
			this.m_ResourceCountText.gameObject.SetActive(this.m_DisplayCount >= 0 || this.m_DisplayCount == -10);
		}
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00022098 File Offset: 0x00020298
	public void SetIconCount(int count)
	{
		this.m_IconCount = count;
		if (this.m_ResourceIcons != null)
		{
			for (int i = 0; i < this.m_ResourceIcons.Length; i++)
			{
				if (this.m_ResourceIcons[i] != null)
				{
					this.m_ResourceIcons[i].SetActive(i < this.m_IconCount);
				}
			}
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x000220ED File Offset: 0x000202ED
	public GameObject GetDragPilePrefab()
	{
		return this.m_DragPilePrefab;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000220F5 File Offset: 0x000202F5
	public void SetDragPilePrefab(GameObject dragPilePrefab)
	{
		this.m_DragPilePrefab = dragPilePrefab;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x000220FE File Offset: 0x000202FE
	public void AddOnResourceClickCallback(AgricolaResource.ResourceClickCallback callback)
	{
		this.m_OnResourceClickCallback = (AgricolaResource.ResourceClickCallback)Delegate.Combine(this.m_OnResourceClickCallback, callback);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00022117 File Offset: 0x00020317
	public void RemoveOnResourceClickCallback(AgricolaResource.ResourceClickCallback callback)
	{
		this.m_OnResourceClickCallback = (AgricolaResource.ResourceClickCallback)Delegate.Remove(this.m_OnResourceClickCallback, callback);
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00022130 File Offset: 0x00020330
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.m_OnResourceClickCallback != null)
		{
			this.m_OnResourceClickCallback(this);
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00022146 File Offset: 0x00020346
	public void AddOnResourceDropCallback(AgricolaResource.ResourceDropCallback callback)
	{
		this.m_OnResourceDropCallback = (AgricolaResource.ResourceDropCallback)Delegate.Combine(this.m_OnResourceDropCallback, callback);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0002215F File Offset: 0x0002035F
	public void RemoveOnResourceDropCallback(AgricolaResource.ResourceDropCallback callback)
	{
		this.m_OnResourceDropCallback = (AgricolaResource.ResourceDropCallback)Delegate.Remove(this.m_OnResourceDropCallback, callback);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00022178 File Offset: 0x00020378
	private void BeginDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = true;
		this.m_bWasDragToTarget = false;
		if (dragObject.GetDragSelectionHint() == 0)
		{
			this.ActivateHighlight(false);
		}
		GameObject.Find("/AgricolaGame").GetComponent<AgricolaGame>().UpdateGameOptionsSelectionState(false);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x000221AC File Offset: 0x000203AC
	private void EndDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = false;
		this.m_bWasDragToTarget = false;
		AgricolaGame component = GameObject.Find("/AgricolaGame").GetComponent<AgricolaGame>();
		if (eventData != null && eventData.pointerEnter != null)
		{
			ushort dragSelectionID = dragObject.GetDragSelectionID();
			ushort dragSelectionHint = dragObject.GetDragSelectionHint();
			if (dragSelectionID != 0 && dragSelectionHint != 0)
			{
				bool flag = false;
				if (dragSelectionHint == 40978)
				{
					int tileSowLocationIndex = AgricolaLib.GetTileSowLocationIndex((int)(dragSelectionID - 1));
					if (tileSowLocationIndex >= 0)
					{
						int sowLocationOptionIndex = AgricolaLib.GetSowLocationOptionIndex(tileSowLocationIndex, this.m_GameOptionIndex - 1);
						if (sowLocationOptionIndex >= 0)
						{
							flag = GameOptions.SelectOptionByHintWithData(40978, (uint)(sowLocationOptionIndex << 16 | tileSowLocationIndex));
							this.SetDestroyOnDragEnd(flag);
						}
					}
				}
				else if (dragSelectionHint == 41017)
				{
					int num = AgricolaLib.GetTileSowLocationIndex((int)(dragSelectionID - 1));
					if (num >= 0)
					{
						int num2 = AgricolaLib.GetTileSowLocationIndex(this.GetGameOptionIndex() - 1);
						if (num2 >= 0)
						{
							flag = GameOptions.SelectOptionByHintWithData(41017, (uint)(num << 16 | num2));
							this.SetDestroyOnDragEnd(flag);
						}
						else
						{
							num2 = this.GetGameOptionIndex() - 1;
							if (num2 >= 0)
							{
								flag = GameOptions.SelectOptionByHintWithData(41017, (uint)(num << 16 | num2));
								this.SetDestroyOnDragEnd(flag);
							}
						}
					}
					else
					{
						num = (int)(dragSelectionID - 1);
						if (num >= 0)
						{
							int num3 = AgricolaLib.GetTileSowLocationIndex(this.GetGameOptionIndex() - 1);
							if (num3 >= 0)
							{
								flag = GameOptions.SelectOptionByHintWithData(41017, (uint)(num << 16 | num3));
								this.SetDestroyOnDragEnd(flag);
							}
							else
							{
								num3 = this.GetGameOptionIndex() - 1;
								if (num3 >= 0)
								{
									flag = GameOptions.SelectOptionByHintWithData(41017, (uint)(num << 16 | num3));
									this.SetDestroyOnDragEnd(flag);
								}
							}
						}
					}
				}
				else if (dragSelectionHint == 40979)
				{
					int num4 = (int)(dragSelectionID - 1);
					if (num4 >= 0)
					{
						int resourceType = this.GetResourceType();
						flag = GameOptions.SelectOptionByHintWithData(40979, (uint)(resourceType << 16 | num4));
						this.SetDestroyOnDragEnd(flag);
					}
				}
				else if (this.GetResourceType() == 0 && dragSelectionHint == 1)
				{
					flag = true;
					component.GetFarm().HandleFeedingFoodDrop((dragSelectionID == 999) ? -1 : ((int)(dragSelectionID - 1)), true, this);
					this.SetDestroyOnDragEnd(true);
					component.UpdateGameOptionsSelectionState(true);
				}
				else if (dragSelectionHint == 40992 && this.GetConvertAbilityCount((int)dragSelectionID) > 1)
				{
					int cardInstanceIDFromSubID = AgricolaLib.GetCardInstanceIDFromSubID((int)dragSelectionID, false, true);
					component.SetOptionPopupRestriction(cardInstanceIDFromSubID, 40992, (EResourceType)this.m_ResourceType, -1);
					flag = true;
					int resourceType2 = this.GetResourceType();
					if (resourceType2 == 7 || resourceType2 == 8 || resourceType2 == 9)
					{
						component.GetFarm().RemoveAnimalFromContainer((EResourceType)resourceType2);
					}
					component.UpdateGameOptionsSelectionState(true);
				}
				else
				{
					flag = GameOptions.SelectOptionByInstanceIDAndHint(dragSelectionID, dragSelectionHint);
					if (flag)
					{
						int resourceType3 = this.GetResourceType();
						if (resourceType3 == 7 || resourceType3 == 8 || resourceType3 == 9)
						{
							component.GetFarm().RemoveAnimalFromContainer((EResourceType)resourceType3);
						}
					}
				}
				if (flag)
				{
					this.m_bWasDragToTarget = true;
					dragObject.ClearReturnToParent();
				}
			}
			else if (this.GetResourceType() == 0)
			{
				component.GetFarm().HandleFeedingFoodDrop(-1, true, this);
				dragObject.ClearReturnToParent();
			}
		}
		if (!this.m_bWasDragToTarget)
		{
			component.UpdateGameOptionsSelectionState(true);
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00022490 File Offset: 0x00020690
	private void DragHintCallback(DragObject dragObject, PointerEventData eventData)
	{
		if (this.m_bCurrentlyDragging && dragObject != null)
		{
			int dragSelectionHint = (int)dragObject.GetDragSelectionHint();
			if (dragSelectionHint != 0 && InterfaceSelectionHints.FindSelectionHintDefinition(dragSelectionHint) != null)
			{
				this.ActivateHighlight(true);
				return;
			}
			this.ActivateHighlight(false);
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x000224D0 File Offset: 0x000206D0
	private int GetConvertAbilityCount(int dragTargetSelectionID)
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[256], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		int cardInstanceIDFromSubID = AgricolaLib.GetCardInstanceIDFromSubID(dragTargetSelectionID, false, true);
		int num = 0;
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].isHidden == 0 && GameOptions.m_GameOption[i].selectionHint == 40992 && AgricolaLib.GetConvertDefinition((uint)GameOptions.m_GameOption[i].selectionID, intPtr, 256) != 0)
			{
				GameConvertDefinition gameConvertDefinition = (GameConvertDefinition)Marshal.PtrToStructure(intPtr, typeof(GameConvertDefinition));
				if (gameConvertDefinition.isHidden == 0 && gameConvertDefinition.resourceCostType == this.m_ResourceType && AgricolaLib.GetCardInstanceIDFromSubID((int)GameOptions.m_GameOption[i].selectionID, false, true) == cardInstanceIDFromSubID)
				{
					num++;
				}
			}
		}
		gchandle.Free();
		return num;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x000225C0 File Offset: 0x000207C0
	public void OnDrop(PointerEventData eventData)
	{
		Debug.Log("OnDrop to " + base.gameObject.name);
		GameObject pointerDrag = eventData.pointerDrag;
		if (pointerDrag != null)
		{
			AgricolaResource component = pointerDrag.GetComponent<AgricolaResource>();
			if (component != null && this.m_OnResourceDropCallback != null)
			{
				this.m_OnResourceDropCallback(this, component);
			}
		}
	}

	// Token: 0x040003CE RID: 974
	public const int k_ResourceTypeFood = 0;

	// Token: 0x040003CF RID: 975
	public const int k_ResourceTypeWood = 1;

	// Token: 0x040003D0 RID: 976
	public const int k_ResourceTypeClay = 2;

	// Token: 0x040003D1 RID: 977
	public const int k_ResourceTypeStone = 3;

	// Token: 0x040003D2 RID: 978
	public const int k_ResourceTypeReed = 4;

	// Token: 0x040003D3 RID: 979
	public const int k_ResourceTypeGrain = 5;

	// Token: 0x040003D4 RID: 980
	public const int k_ResourceTypeVegetable = 6;

	// Token: 0x040003D5 RID: 981
	public const int k_ResourceTypeSheep = 7;

	// Token: 0x040003D6 RID: 982
	public const int k_ResourceTypeWildBoar = 8;

	// Token: 0x040003D7 RID: 983
	public const int k_ResourceTypeCattle = 9;

	// Token: 0x040003D8 RID: 984
	private const int k_maxDataSize = 256;

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	private GameObject m_Highlight;

	// Token: 0x040003DA RID: 986
	[SerializeField]
	private Animator m_HighlightAnimator;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	private TextMeshProUGUI m_ResourceCountText;

	// Token: 0x040003DC RID: 988
	[SerializeField]
	private GameObject[] m_ResourceIcons;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	private GameObject m_ResourceDraggableToken;

	// Token: 0x040003DE RID: 990
	[SerializeField]
	private ColorByFaction m_ResourceDraggableColorizer;

	// Token: 0x040003DF RID: 991
	private AgricolaResource.ResourceClickCallback m_OnResourceClickCallback;

	// Token: 0x040003E0 RID: 992
	private AgricolaResource.ResourceDropCallback m_OnResourceDropCallback;

	// Token: 0x040003E1 RID: 993
	private int m_GameOptionIndex = -1;

	// Token: 0x040003E2 RID: 994
	private int m_DisplayCount = -1;

	// Token: 0x040003E3 RID: 995
	private int m_IconCount = -1;

	// Token: 0x040003E4 RID: 996
	private GameObject m_DragPilePrefab;

	// Token: 0x040003E5 RID: 997
	private bool m_bActiveHighlight;

	// Token: 0x040003E6 RID: 998
	private bool m_bCurrentlyDragging;

	// Token: 0x040003E7 RID: 999
	private bool m_bWasDragToTarget;

	// Token: 0x040003E8 RID: 1000
	private bool m_bDestroyOnDragEnd;

	// Token: 0x040003E9 RID: 1001
	private int m_ResourceType = -1;

	// Token: 0x040003EA RID: 1002
	private int m_ResourceValue;

	// Token: 0x0200076C RID: 1900
	// (Invoke) Token: 0x060041F9 RID: 16889
	public delegate void ResourceClickCallback(AgricolaResource resource);

	// Token: 0x0200076D RID: 1901
	// (Invoke) Token: 0x060041FD RID: 16893
	public delegate void ResourceDropCallback(AgricolaResource dropResource, AgricolaResource dragResource);
}
