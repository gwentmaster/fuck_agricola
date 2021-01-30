using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class PlayerData : MonoBehaviour
{
	// Token: 0x06000542 RID: 1346 RVA: 0x0002874B File Offset: 0x0002694B
	public PlayerInterface GetInterfaceStatic()
	{
		return this.m_InterfaceStatic;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00028753 File Offset: 0x00026953
	public PlayerInterface GetInterfaceActivated()
	{
		return this.m_InterfaceActivated;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002875B File Offset: 0x0002695B
	public bool IsActivated()
	{
		return this.m_bActivated;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00028763 File Offset: 0x00026963
	public int GetPlayerIndex()
	{
		return this.m_PlayerIndex;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0002876B File Offset: 0x0002696B
	public int GetPlayerInstanceID()
	{
		return this.m_PlayerInstanceID;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00028773 File Offset: 0x00026973
	public int GetAvatarIndex()
	{
		return this.m_AvatarIndex;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002877B File Offset: 0x0002697B
	public int GetFactionIndex()
	{
		return this.m_FactionIndex;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00028783 File Offset: 0x00026983
	public void SetFactionIndex(int faction_index)
	{
		this.m_FactionIndex = faction_index;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002878C File Offset: 0x0002698C
	public void SetPlayerIndex(int player_index, int player_instance_id)
	{
		this.m_PlayerIndex = player_index;
		this.m_PlayerInstanceID = player_instance_id;
		if (this.m_InterfaceStatic != null)
		{
			this.m_InterfaceStatic.SetEnabled(this.m_PlayerIndex != 0);
			this.m_InterfaceStatic.SetAssignedPlayerData(this, null);
		}
		if (this.m_bActivated && this.m_InterfaceActivated != null)
		{
			this.m_InterfaceActivated.SetEnabled(this.m_PlayerIndex != 0);
		}
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00028800 File Offset: 0x00026A00
	public int GetCachedResourceCount(int resourceIndex)
	{
		if (this.m_CachedResourceCounts == null || resourceIndex < 0 || resourceIndex >= this.m_CachedResourceCounts.Length)
		{
			return 0;
		}
		return this.m_CachedResourceCounts[resourceIndex];
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00028824 File Offset: 0x00026A24
	public GameObject GetAnimationNodeResources(int resourceType)
	{
		if (this.m_bActivated && this.m_InterfaceActivated != null)
		{
			GameObject animationNodeResources = this.m_InterfaceActivated.GetAnimationNodeResources(resourceType);
			if (animationNodeResources != null)
			{
				return animationNodeResources;
			}
		}
		if (this.m_InterfaceStatic != null)
		{
			GameObject animationNodeResources = this.m_InterfaceStatic.GetAnimationNodeResources(resourceType);
			if (animationNodeResources != null)
			{
				return animationNodeResources;
			}
		}
		return base.gameObject;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0002888D File Offset: 0x00026A8D
	public void UpdateGameOptionsSelectionState(bool bHighlight)
	{
		if (this.m_InterfaceStatic != null)
		{
			this.m_InterfaceStatic.UpdateGameOptionsSelectionState(bHighlight);
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x000288A9 File Offset: 0x00026AA9
	public void SetActivated(bool bActivated, AnimationManager animation_manager)
	{
		if (this.m_bActivated != bActivated)
		{
			this.m_bActivated = bActivated;
			if (animation_manager != null)
			{
				this.RebuildAnimationManager(animation_manager);
			}
			this.UpdatePlayerData();
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x000288D4 File Offset: 0x00026AD4
	private void Awake()
	{
		GameObject gameObject = GameObject.Find("/Agricola Animation Manager");
		if (gameObject != null)
		{
			this.m_AnimationManager = gameObject.GetComponent<AgricolaAnimationManager>();
		}
		this.m_PlayerIndex = 0;
		this.m_PlayerInstanceID = 0;
		this.m_PlayerDataBuffer = new byte[512];
		this.m_PlayerDataHandle = GCHandle.Alloc(this.m_PlayerDataBuffer, GCHandleType.Pinned);
		this.m_CachedResourceCounts = new int[10];
		for (int i = 0; i < this.m_CachedResourceCounts.Length; i++)
		{
			this.m_CachedResourceCounts[i] = 0;
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0002895C File Offset: 0x00026B5C
	private void Start()
	{
		if (this.m_InterfaceStatic != null)
		{
			this.m_InterfaceStatic.SetEnabled(this.m_PlayerIndex != 0);
		}
		if (this.m_bActivated && this.m_InterfaceActivated != null)
		{
			this.m_InterfaceActivated.SetEnabled(this.m_PlayerIndex != 0);
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x000289B5 File Offset: 0x00026BB5
	private void OnDestroy()
	{
		this.m_PlayerDataHandle.Free();
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x000289C2 File Offset: 0x00026BC2
	private void Update()
	{
		this.UpdatePlayerData();
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000289CC File Offset: 0x00026BCC
	private void UpdatePlayerData()
	{
		if (this.m_PlayerIndex == 0 || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		IntPtr intPtr = this.m_PlayerDataHandle.AddrOfPinnedObject();
		bool gamePlayerState = AgricolaLib.GetGamePlayerState(this.m_PlayerIndex, intPtr, 512) != 0;
		GamePlayerState gamePlayerState2 = (GamePlayerState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerState));
		if (gamePlayerState && gamePlayerState2.playerFaction == this.m_FactionIndex && gamePlayerState2.playerIndex == this.m_PlayerIndex)
		{
			if (this.m_AnimationManager != null)
			{
				this.m_AnimationManager.SubtractAnimatingResources(ref gamePlayerState2, 14, this.m_PlayerInstanceID);
			}
			this.m_AvatarIndex = gamePlayerState2.playerAvatar;
			this.m_FactionIndex = gamePlayerState2.playerFaction;
			if (this.m_CachedResourceCounts != null)
			{
				this.m_CachedResourceCounts[0] = gamePlayerState2.resourceCountFood;
				this.m_CachedResourceCounts[1] = gamePlayerState2.resourceCountWood;
				this.m_CachedResourceCounts[2] = gamePlayerState2.resourceCountClay;
				this.m_CachedResourceCounts[3] = gamePlayerState2.resourceCountStone;
				this.m_CachedResourceCounts[4] = gamePlayerState2.resourceCountReed;
				this.m_CachedResourceCounts[5] = gamePlayerState2.resourceCountGrain;
				this.m_CachedResourceCounts[6] = gamePlayerState2.resourceCountVeggie;
				this.m_CachedResourceCounts[7] = gamePlayerState2.resourceCountSheep;
				this.m_CachedResourceCounts[8] = gamePlayerState2.resourceCountWildBoar;
				this.m_CachedResourceCounts[9] = gamePlayerState2.resourceCountCattle;
			}
			if (this.m_InterfaceStatic != null)
			{
				this.m_InterfaceStatic.SetPlayerData(gamePlayerState2);
				this.m_InterfaceStatic.SetResourceCalendarNode(this.m_PlayerIndex);
			}
			if (this.m_bActivated && this.m_InterfaceActivated != null)
			{
				this.m_InterfaceActivated.SetPlayerData(gamePlayerState2);
				this.m_InterfaceActivated.SetResourceCalendarNode(this.m_PlayerIndex);
			}
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00028B7C File Offset: 0x00026D7C
	public void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (this.m_PlayerIndex == 0 || animation_manager == null)
		{
			return;
		}
		if (this.m_InterfaceStatic != null)
		{
			this.m_InterfaceStatic.RebuildAnimationManager(animation_manager, this.m_PlayerInstanceID);
		}
		if (this.m_bActivated && this.m_InterfaceActivated != null)
		{
			this.m_InterfaceActivated.RebuildAnimationManager(animation_manager, this.m_PlayerInstanceID);
		}
	}

	// Token: 0x040004EF RID: 1263
	[SerializeField]
	private PlayerInterface m_InterfaceStatic;

	// Token: 0x040004F0 RID: 1264
	[SerializeField]
	private PlayerInterface m_InterfaceActivated;

	// Token: 0x040004F1 RID: 1265
	[SerializeField]
	private int m_PlayerIndex;

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	private int m_PlayerInstanceID;

	// Token: 0x040004F3 RID: 1267
	[SerializeField]
	private int m_AvatarIndex;

	// Token: 0x040004F4 RID: 1268
	[SerializeField]
	private int m_FactionIndex;

	// Token: 0x040004F5 RID: 1269
	private bool m_bActivated;

	// Token: 0x040004F6 RID: 1270
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x040004F7 RID: 1271
	private const int k_maxDataSize = 512;

	// Token: 0x040004F8 RID: 1272
	private const int k_maxCardCount = 64;

	// Token: 0x040004F9 RID: 1273
	private byte[] m_PlayerDataBuffer;

	// Token: 0x040004FA RID: 1274
	private GCHandle m_PlayerDataHandle;

	// Token: 0x040004FB RID: 1275
	private int[] m_CachedResourceCounts;
}
