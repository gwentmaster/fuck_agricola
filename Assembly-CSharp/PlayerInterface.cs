using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200006A RID: 106
public class PlayerInterface : MonoBehaviour
{
	// Token: 0x06000565 RID: 1381 RVA: 0x0002993E File Offset: 0x00027B3E
	public void SetAssignedPlayerData(PlayerData player_data, AnimationManager animation_manager)
	{
		if (this.m_AssignedToPlayerData == player_data)
		{
			return;
		}
		if (this.m_AssignedToPlayerData != null)
		{
			this.m_AssignedToPlayerData = null;
		}
		this.m_AssignedToPlayerData = player_data;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0002996B File Offset: 0x00027B6B
	public void SetEnabled(bool bEnabled)
	{
		this.m_bEnabled = bEnabled;
		if (this.m_NodeRoot != null)
		{
			this.m_NodeRoot.SetActive(this.m_bEnabled);
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00029993 File Offset: 0x00027B93
	private void Awake()
	{
		if (this.m_PrefabResourceTokens != null && this.m_PrefabResourceTokens.Length != 0)
		{
			this.m_ActiveResourceTokens = new AgricolaResource[this.m_PrefabResourceTokens.Length];
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000299BC File Offset: 0x00027BBC
	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		this.m_GameController = gameObject.GetComponent<AgricolaGame>();
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000299E0 File Offset: 0x00027BE0
	public GameObject GetAnimationNodeResources(int resourceType)
	{
		if (this.m_AnimationNodeResources != null && resourceType >= 0 && resourceType < this.m_AnimationNodeResources.Length)
		{
			return this.m_AnimationNodeResources[resourceType];
		}
		return null;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00029A03 File Offset: 0x00027C03
	public void SetResourceCalendarNode(int player_index)
	{
		if (this.m_ResCalendarNode != null)
		{
			this.m_ResCalendarNode.SetActive(AgricolaLib.GetDoesPlayerHaveCalendarResources(player_index));
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00029A24 File Offset: 0x00027C24
	public void SetPlayerData(GamePlayerState player_data)
	{
		if (this.m_TextPlayerName != null)
		{
			this.m_TextPlayerName.text = player_data.displayName;
		}
		if (base.gameObject.GetComponent<ColorByFaction>() != null)
		{
			base.gameObject.GetComponent<ColorByFaction>().Colorize((uint)player_data.playerFaction);
		}
		if (this.m_TextResourceCountFood != null)
		{
			this.m_TextResourceCountFood.text = player_data.resourceCountFood.ToString();
		}
		if (this.m_TextResourceCountWood != null)
		{
			this.m_TextResourceCountWood.text = player_data.resourceCountWood.ToString();
		}
		if (this.m_TextResourceCountClay != null)
		{
			this.m_TextResourceCountClay.text = player_data.resourceCountClay.ToString();
		}
		if (this.m_TextResourceCountStone != null)
		{
			this.m_TextResourceCountStone.text = player_data.resourceCountStone.ToString();
		}
		if (this.m_TextResourceCountReed != null)
		{
			this.m_TextResourceCountReed.text = player_data.resourceCountReed.ToString();
		}
		if (this.m_TextResourceCountGrain != null)
		{
			this.m_TextResourceCountGrain.text = player_data.resourceCountGrain.ToString();
		}
		if (this.m_TextResourceCountVeggie != null)
		{
			this.m_TextResourceCountVeggie.text = player_data.resourceCountVeggie.ToString();
		}
		if (this.m_TextResourceCountSheep != null)
		{
			this.m_TextResourceCountSheep.text = player_data.resourceCountSheep.ToString();
		}
		if (this.m_TextResourceCountWildBoar != null)
		{
			this.m_TextResourceCountWildBoar.text = player_data.resourceCountWildBoar.ToString();
		}
		if (this.m_TextResourceCountCattle != null)
		{
			this.m_TextResourceCountCattle.text = player_data.resourceCountCattle.ToString();
		}
		if (this.m_TextFoodRequirement != null)
		{
			this.m_TextFoodRequirement.text = player_data.foodRequirement.ToString();
		}
		if (this.m_TextFencesUsed != null)
		{
			this.m_TextFencesUsed.text = player_data.fencesCount.ToString();
		}
		if (this.m_TextMaxFences != null)
		{
			this.m_TextMaxFences.text = player_data.maxFences.ToString();
		}
		if (this.m_TextStablesUsed != null)
		{
			this.m_TextStablesUsed.text = player_data.stablesCount.ToString();
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00003022 File Offset: 0x00001222
	public void SetPlayerOnlineTimer(GamePlayerTimer playerTimerInfo)
	{
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0001944A File Offset: 0x0001764A
	public void RebuildAnimationManager(AnimationManager animation_manager, int player_instance_id)
	{
		animation_manager == null;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00029C80 File Offset: 0x00027E80
	private bool IsDraggableResourceType(int resourceType, uint convert_resource_flags)
	{
		if ((1L << (resourceType & 31) & (long)((ulong)convert_resource_flags)) != 0L)
		{
			return true;
		}
		if (resourceType != 0)
		{
			if (resourceType != 5)
			{
				if (resourceType == 6)
				{
					if (GameOptions.IsSelectableHint(40978) && this.m_AssignedToPlayerData != null && this.m_AssignedToPlayerData.GetCachedResourceCount(6) >= 1)
					{
						return true;
					}
				}
			}
			else if (GameOptions.IsSelectableHint(40978) && this.m_AssignedToPlayerData != null && this.m_AssignedToPlayerData.GetCachedResourceCount(5) >= 1)
			{
				return true;
			}
		}
		else if (this.m_GameController != null)
		{
			AgricolaFarm farm = this.m_GameController.GetFarm();
			if (farm.GetIsFeedingMode())
			{
				return farm.GetFeedingAmountNeeded() > farm.GetFeedingAmountUsed() && farm.GetFeedingAmountUsed() < Convert.ToInt32(this.m_TextResourceCountFood.text);
			}
		}
		return false;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00029D54 File Offset: 0x00027F54
	public void UpdateGameOptionsSelectionState(bool bHighlight)
	{
		if (this.m_ActiveResourceTokens != null)
		{
			uint num = 0U;
			if (this.m_CardInPlayManager != null)
			{
				num |= this.m_CardInPlayManager.GetConvertResourceFlags();
			}
			for (int i = 0; i < this.m_ActiveResourceTokens.Length; i++)
			{
				if (this.IsDraggableResourceType(i, num))
				{
					if (this.m_ActiveResourceTokens[i] == null && this.m_PrefabResourceTokens[i] != null)
					{
						this.m_ActiveResourceTokens[i] = this.CreatePlaceResource(this.m_PrefabResourceTokens[i], this.m_LocatorResourceTokens[i], i + 1);
						this.m_ActiveResourceTokens[i].SetResourceValue(i, 1);
					}
				}
				else if (this.m_ActiveResourceTokens[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_ActiveResourceTokens[i].gameObject);
					this.m_ActiveResourceTokens[i] = null;
				}
			}
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00029E2C File Offset: 0x0002802C
	public AgricolaResource CreateResourceToken(EResourceType resType, GameObject parent, int option_index)
	{
		if (resType >= EResourceType.FOOD && resType < (EResourceType)this.m_PrefabResourceTokens.Length)
		{
			return this.CreatePlaceResource(this.m_PrefabResourceTokens[(int)resType], parent, option_index);
		}
		return null;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00029E5C File Offset: 0x0002805C
	private AgricolaResource CreatePlaceResource(GameObject prefab, GameObject locator, int option_index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		AnimateObject component = gameObject.GetComponent<AnimateObject>();
		if (component != null && this.m_AnimationManager != null)
		{
			component.SetAnimationManager(this.m_AnimationManager);
		}
		if (locator != null)
		{
			gameObject.transform.SetParent(locator.transform, true);
			gameObject.transform.position = locator.transform.position;
			gameObject.transform.rotation = locator.transform.rotation;
		}
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(true);
		DragObject component2 = gameObject.GetComponent<DragObject>();
		if (component2 != null && this.m_DragManager != null)
		{
			component2.SetDragManager(this.m_DragManager);
		}
		AgricolaResource component3 = gameObject.GetComponent<AgricolaResource>();
		if (component3 != null)
		{
			component3.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragTavernResourceCallback));
			component3.SetGameOptionIndex(option_index);
			component3.SetIsDraggable(true);
			AgricolaGame component4 = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
			component3.Colorize((uint)component4.GetLocalPlayerColorIndex());
			component3.ActivateHighlight(true);
		}
		return component3;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00029F78 File Offset: 0x00028178
	private void EndDragTavernResourceCallback(DragObject dragObject, PointerEventData eventData)
	{
		if (dragObject == null)
		{
			return;
		}
		if (this.m_ActiveResourceTokens != null)
		{
			for (int i = 0; i < this.m_ActiveResourceTokens.Length; i++)
			{
				if (this.m_ActiveResourceTokens[i] != null && dragObject.gameObject == this.m_ActiveResourceTokens[i].gameObject)
				{
					dragObject.ClearReturnToParent();
					if (this.m_ActiveResourceTokens[i].WasDragToTarget() && this.m_AnimationManager != null && !this.m_ActiveResourceTokens[i].DestroyResourceOnDragEnd())
					{
						this.m_AnimationManager.SetPendingResourceAnimation(this.m_ActiveResourceTokens[i]);
					}
					else
					{
						UnityEngine.Object.Destroy(this.m_ActiveResourceTokens[i].gameObject);
					}
					this.m_ActiveResourceTokens[i] = null;
					this.UpdateGameOptionsSelectionState(true);
					return;
				}
			}
		}
	}

	// Token: 0x0400050B RID: 1291
	[SerializeField]
	private GameObject m_NodeRoot;

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private TextMeshProUGUI m_TextPlayerName;

	// Token: 0x0400050D RID: 1293
	[SerializeField]
	private GameObject m_ResCalendarNode;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountFood;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountWood;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountClay;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountStone;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountReed;

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountGrain;

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountVeggie;

	// Token: 0x04000515 RID: 1301
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountSheep;

	// Token: 0x04000516 RID: 1302
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountWildBoar;

	// Token: 0x04000517 RID: 1303
	[SerializeField]
	private TextMeshProUGUI m_TextResourceCountCattle;

	// Token: 0x04000518 RID: 1304
	[SerializeField]
	private TextMeshProUGUI m_TextFoodRequirement;

	// Token: 0x04000519 RID: 1305
	[SerializeField]
	private TextMeshProUGUI m_TextFencesUsed;

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	private TextMeshProUGUI m_TextMaxFences;

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private TextMeshProUGUI m_TextStablesUsed;

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private GameObject[] m_AnimationNodeResources;

	// Token: 0x0400051D RID: 1309
	[SerializeField]
	private GameObject[] m_PrefabResourceTokens;

	// Token: 0x0400051E RID: 1310
	[SerializeField]
	private GameObject[] m_LocatorResourceTokens;

	// Token: 0x0400051F RID: 1311
	private AgricolaResource[] m_ActiveResourceTokens;

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private DragManager m_DragManager;

	// Token: 0x04000521 RID: 1313
	[SerializeField]
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x04000522 RID: 1314
	[SerializeField]
	private AgricolaCardInPlayManager m_CardInPlayManager;

	// Token: 0x04000523 RID: 1315
	private bool m_bEnabled = true;

	// Token: 0x04000524 RID: 1316
	private PlayerData m_AssignedToPlayerData;

	// Token: 0x04000525 RID: 1317
	private AgricolaGame m_GameController;
}
