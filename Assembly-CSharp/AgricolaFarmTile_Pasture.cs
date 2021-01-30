using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003C RID: 60
public class AgricolaFarmTile_Pasture : AgricolaFarmTile_Base
{
	// Token: 0x06000326 RID: 806 RVA: 0x00014DED File Offset: 0x00012FED
	public int GetPastureIndex()
	{
		return this.m_pastureIndex;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00014DF5 File Offset: 0x00012FF5
	public int GetPastureCapacity()
	{
		return this.m_pastureCapacity;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00014DFD File Offset: 0x00012FFD
	public int GetProposedIndex()
	{
		return this.m_proposedPastureIndex;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00014E05 File Offset: 0x00013005
	public bool GetFencingNeedsUpdate()
	{
		return this.m_bFencingNeedsUpdate;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00014E0D File Offset: 0x0001300D
	public bool GetIsProposedChecked()
	{
		return this.m_bProposedChecked;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00014E15 File Offset: 0x00013015
	public bool GetNorthFenceOn()
	{
		return this.m_fenceNodeNorth != null && this.m_fenceNodeNorth.activeSelf;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00014E32 File Offset: 0x00013032
	public bool GetSouthFenceOn()
	{
		return this.m_fenceNodeSouth != null && this.m_fenceNodeSouth.activeSelf;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00014E4F File Offset: 0x0001304F
	public bool GetEastFenceOn()
	{
		return this.m_fenceNodeEast != null && this.m_fenceNodeEast.activeSelf;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00014E6C File Offset: 0x0001306C
	public bool GetWestFenceOn()
	{
		return this.m_fenceNodeWest != null && this.m_fenceNodeWest.activeSelf;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00014E89 File Offset: 0x00013089
	public int GetNorthFenceDataIndex()
	{
		return this.m_northFenceDataIndex;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00014E91 File Offset: 0x00013091
	public int GetSouthFenceDataIndex()
	{
		return this.m_southFenceDataIndex;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00014E99 File Offset: 0x00013099
	public int GetEastFenceDataIndex()
	{
		return this.m_eastFenceDataIndex;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00014EA1 File Offset: 0x000130A1
	public int GetWestFenceDataIndex()
	{
		return this.m_westFenceDataIndex;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00014EAC File Offset: 0x000130AC
	public override void SetParent(AgricolaFarmTile parent)
	{
		base.SetParent(parent);
		int tileIndex = parent.GetTileIndex();
		this.m_northFenceDataIndex = tileIndex;
		this.m_southFenceDataIndex = tileIndex + 5;
		this.m_westFenceDataIndex = tileIndex + 20;
		if (tileIndex >= 5 && tileIndex < 10)
		{
			this.m_westFenceDataIndex++;
		}
		else if (tileIndex >= 10)
		{
			this.m_westFenceDataIndex += 2;
		}
		this.m_eastFenceDataIndex = this.m_westFenceDataIndex + 1;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00014F1B File Offset: 0x0001311B
	public void SetFencingNeedsUpdate(bool bUpdate)
	{
		this.m_bFencingNeedsUpdate = bUpdate;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00014F24 File Offset: 0x00013124
	public void SetIsProposedChecked(bool bChecked)
	{
		this.m_bProposedChecked = bChecked;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00014F2D File Offset: 0x0001312D
	public void SetPastureIndex(int pastureIndex)
	{
		this.m_pastureIndex = pastureIndex;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00014F36 File Offset: 0x00013136
	public void SetProposedPastureIndex(int pastureIndex)
	{
		this.m_proposedPastureIndex = pastureIndex;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00014F3F File Offset: 0x0001313F
	public void SetPastureCapacity(int pastureCapacity)
	{
		this.m_pastureCapacity = pastureCapacity;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00014F48 File Offset: 0x00013148
	public void SetFencingMode(bool bFencingModeOn, bool[] premadeFenceBitfield)
	{
		if (this.m_fenceButtonNorth != null)
		{
			this.m_fenceButtonNorth.interactable = (bFencingModeOn && !premadeFenceBitfield[this.m_northFenceDataIndex]);
		}
		if (this.m_fenceButtonSouth != null)
		{
			this.m_fenceButtonSouth.interactable = (bFencingModeOn && !premadeFenceBitfield[this.m_southFenceDataIndex]);
		}
		if (this.m_fenceButtonEast != null)
		{
			this.m_fenceButtonEast.interactable = (bFencingModeOn && !premadeFenceBitfield[this.m_eastFenceDataIndex]);
		}
		if (this.m_fenceButtonWest != null)
		{
			this.m_fenceButtonWest.interactable = (bFencingModeOn && !premadeFenceBitfield[this.m_westFenceDataIndex]);
		}
		if (this.m_fenceImageNorth != null)
		{
			this.m_fenceImageNorth.raycastTarget = (bFencingModeOn && !premadeFenceBitfield[this.m_northFenceDataIndex]);
		}
		if (this.m_fenceImageSouth != null)
		{
			this.m_fenceImageSouth.raycastTarget = (bFencingModeOn && !premadeFenceBitfield[this.m_southFenceDataIndex]);
		}
		if (this.m_fenceImageEast != null)
		{
			this.m_fenceImageEast.raycastTarget = (bFencingModeOn && !premadeFenceBitfield[this.m_eastFenceDataIndex]);
		}
		if (this.m_fenceImageWest != null)
		{
			this.m_fenceImageWest.raycastTarget = (bFencingModeOn && !premadeFenceBitfield[this.m_westFenceDataIndex]);
		}
		if (!bFencingModeOn)
		{
			if (this.m_fenceNodeNorth != null)
			{
				this.m_fenceNodeNorth.SetActive(premadeFenceBitfield[this.m_northFenceDataIndex]);
			}
			if (this.m_fenceNodeSouth != null)
			{
				this.m_fenceNodeSouth.SetActive(premadeFenceBitfield[this.m_southFenceDataIndex]);
			}
			if (this.m_fenceNodeEast != null)
			{
				this.m_fenceNodeEast.SetActive(premadeFenceBitfield[this.m_eastFenceDataIndex]);
			}
			if (this.m_fenceNodeWest != null)
			{
				this.m_fenceNodeWest.SetActive(premadeFenceBitfield[this.m_westFenceDataIndex]);
			}
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00015130 File Offset: 0x00013330
	public void OnFenceTap(int fenceSide)
	{
		AgricolaFarm farm = this.m_parent.GetFarm();
		switch (fenceSide)
		{
		case 0:
			if (this.m_fenceNodeNorth == null)
			{
				return;
			}
			if (this.m_fenceNodeNorth.activeSelf && !this.m_bNorthFenceProposed)
			{
				return;
			}
			if (!this.m_fenceNodeNorth.activeSelf && !farm.PurchaseFence())
			{
				return;
			}
			if (this.m_fenceNodeNorth.activeSelf)
			{
				farm.ReturnFence();
			}
			this.SetNorthFence(!this.m_fenceNodeNorth.activeSelf, true);
			farm.CalculateProposedPastures();
			return;
		case 1:
			if (this.m_fenceNodeSouth == null)
			{
				return;
			}
			if (this.m_fenceNodeSouth.activeSelf && !this.m_bSouthFenceProposed)
			{
				return;
			}
			if (!this.m_fenceNodeSouth.activeSelf && !farm.PurchaseFence())
			{
				return;
			}
			if (this.m_fenceNodeSouth.activeSelf)
			{
				farm.ReturnFence();
			}
			this.SetSouthFence(!this.m_fenceNodeSouth.activeSelf, true);
			farm.CalculateProposedPastures();
			return;
		case 2:
			if (this.m_fenceNodeEast == null)
			{
				return;
			}
			if (this.m_fenceNodeEast.activeSelf && !this.m_bEastFenceProposed)
			{
				return;
			}
			if (!this.m_fenceNodeEast.activeSelf && !farm.PurchaseFence())
			{
				return;
			}
			if (this.m_fenceNodeEast.activeSelf)
			{
				farm.ReturnFence();
			}
			this.SetEastFence(!this.m_fenceNodeEast.activeSelf, true);
			farm.CalculateProposedPastures();
			return;
		case 3:
			if (this.m_fenceNodeWest == null)
			{
				return;
			}
			if (this.m_fenceNodeWest.activeSelf && !this.m_bWestFenceProposed)
			{
				return;
			}
			if (!this.m_fenceNodeWest.activeSelf && !farm.PurchaseFence())
			{
				return;
			}
			if (this.m_fenceNodeWest.activeSelf)
			{
				farm.ReturnFence();
			}
			this.SetWestFence(!this.m_fenceNodeWest.activeSelf, true);
			farm.CalculateProposedPastures();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00015307 File Offset: 0x00013507
	public void SetNorthFence(bool bOn, bool bProposed = false)
	{
		if (this.m_fenceNodeNorth != null)
		{
			this.m_fenceNodeNorth.SetActive(bOn);
		}
		this.m_bNorthFenceProposed = bProposed;
		this.m_parent.GetFarm().SetFenceData(this.m_northFenceDataIndex, bOn, bProposed);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00015342 File Offset: 0x00013542
	public void SetSouthFence(bool bOn, bool bProposed = false)
	{
		if (this.m_fenceNodeSouth != null)
		{
			this.m_fenceNodeSouth.SetActive(bOn);
		}
		this.m_bSouthFenceProposed = bProposed;
		this.m_parent.GetFarm().SetFenceData(this.m_southFenceDataIndex, bOn, bProposed);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001537D File Offset: 0x0001357D
	public void SetEastFence(bool bOn, bool bProposed = false)
	{
		if (this.m_fenceNodeEast != null)
		{
			this.m_fenceNodeEast.SetActive(bOn);
		}
		this.m_bEastFenceProposed = bProposed;
		this.m_parent.GetFarm().SetFenceData(this.m_eastFenceDataIndex, bOn, bProposed);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x000153B8 File Offset: 0x000135B8
	public void SetWestFence(bool bOn, bool bProposed = false)
	{
		if (this.m_fenceNodeWest != null)
		{
			this.m_fenceNodeWest.SetActive(bOn);
		}
		this.m_bWestFenceProposed = bProposed;
		this.m_parent.GetFarm().SetFenceData(this.m_westFenceDataIndex, bOn, bProposed);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x000153F3 File Offset: 0x000135F3
	public void SetNorthFenceGlow(bool bOn, bool bRed)
	{
		if (this.m_fenceNodeNorthGlow != null)
		{
			this.m_fenceNodeNorthGlow.gameObject.SetActive(bOn);
			this.m_fenceNodeNorthGlow.material = (bRed ? this.m_redGlowMaterial : this.m_whiteGlowMaterial);
		}
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00015430 File Offset: 0x00013630
	public void SetSouthFenceGlow(bool bOn, bool bRed)
	{
		if (this.m_fenceNodeSouthGlow != null)
		{
			this.m_fenceNodeSouthGlow.gameObject.SetActive(bOn);
			this.m_fenceNodeSouthGlow.material = (bRed ? this.m_redGlowMaterial : this.m_whiteGlowMaterial);
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0001546D File Offset: 0x0001366D
	public void SetEastFenceGlow(bool bOn, bool bRed)
	{
		if (this.m_fenceNodeEastGlow != null)
		{
			this.m_fenceNodeEastGlow.gameObject.SetActive(bOn);
			this.m_fenceNodeEastGlow.material = (bRed ? this.m_redGlowMaterial : this.m_whiteGlowMaterial);
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000154AA File Offset: 0x000136AA
	public void SetWestFenceGlow(bool bOn, bool bRed)
	{
		if (this.m_fenceNodeWestGlow != null)
		{
			this.m_fenceNodeWestGlow.gameObject.SetActive(bOn);
			this.m_fenceNodeWestGlow.material = (bRed ? this.m_redGlowMaterial : this.m_whiteGlowMaterial);
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x000154E8 File Offset: 0x000136E8
	public void HandleFenceGlows(bool[] validFenceBitfield, bool[] proposedFenceBitfield, bool[] premadeFenceBitfield)
	{
		if (validFenceBitfield == null || proposedFenceBitfield == null || premadeFenceBitfield == null)
		{
			this.SetNorthFenceGlow(false, false);
			this.SetSouthFenceGlow(false, false);
			this.SetEastFenceGlow(false, false);
			this.SetWestFenceGlow(false, false);
			return;
		}
		bool flag = proposedFenceBitfield[this.m_northFenceDataIndex] && !premadeFenceBitfield[this.m_northFenceDataIndex];
		bool flag2 = flag & validFenceBitfield[this.m_northFenceDataIndex];
		this.SetNorthFenceGlow(flag, !flag2);
		flag = (proposedFenceBitfield[this.m_southFenceDataIndex] && !premadeFenceBitfield[this.m_southFenceDataIndex]);
		flag2 = (flag & validFenceBitfield[this.m_southFenceDataIndex]);
		this.SetSouthFenceGlow(flag, !flag2);
		flag = (proposedFenceBitfield[this.m_eastFenceDataIndex] && !premadeFenceBitfield[this.m_eastFenceDataIndex]);
		flag2 = (flag & validFenceBitfield[this.m_eastFenceDataIndex]);
		this.SetEastFenceGlow(flag, !flag2);
		flag = (proposedFenceBitfield[this.m_westFenceDataIndex] && !premadeFenceBitfield[this.m_westFenceDataIndex]);
		flag2 = (flag & validFenceBitfield[this.m_westFenceDataIndex]);
		this.SetWestFenceGlow(flag, !flag2);
	}

	// Token: 0x06000344 RID: 836 RVA: 0x000155DB File Offset: 0x000137DB
	public void SetStable(bool bOn)
	{
		if (this.m_stableNode != null)
		{
			this.m_stableNode.SetActive(bOn);
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x000155F7 File Offset: 0x000137F7
	public bool GetHasStable()
	{
		return this.m_stableNode != null && this.m_stableNode.activeSelf;
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00015614 File Offset: 0x00013814
	public void FinalizeProposedPasture()
	{
		this.m_pastureIndex = this.m_proposedPastureIndex;
		this.m_proposedPastureIndex = -1;
		this.m_bNorthFenceProposed = false;
		this.m_bSouthFenceProposed = false;
		this.m_bEastFenceProposed = false;
		this.m_bWestFenceProposed = false;
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00015648 File Offset: 0x00013848
	public void Clear(bool bRespectNeighborFences = true)
	{
		base.gameObject.SetActive(true);
		if (bRespectNeighborFences)
		{
			AgricolaFarmTile agricolaFarmTile = this.m_parent.GetNorthNeighbor();
			if (agricolaFarmTile == null || agricolaFarmTile.GetTileType() != AgricolaFarmTile.FarmTileAssignment.Pasture || !agricolaFarmTile.GetPasture().GetSouthFenceOn())
			{
				this.SetNorthFence(false, false);
			}
			agricolaFarmTile = this.m_parent.GetSouthNeighbor();
			if (agricolaFarmTile == null || agricolaFarmTile.GetTileType() != AgricolaFarmTile.FarmTileAssignment.Pasture || !agricolaFarmTile.GetPasture().GetNorthFenceOn())
			{
				this.SetSouthFence(false, false);
			}
			agricolaFarmTile = this.m_parent.GetEastNeighbor();
			if (agricolaFarmTile == null || agricolaFarmTile.GetTileType() != AgricolaFarmTile.FarmTileAssignment.Pasture || !agricolaFarmTile.GetPasture().GetWestFenceOn())
			{
				this.SetEastFence(false, false);
			}
			agricolaFarmTile = this.m_parent.GetWestNeighbor();
			if (agricolaFarmTile == null || agricolaFarmTile.GetTileType() != AgricolaFarmTile.FarmTileAssignment.Pasture || !agricolaFarmTile.GetPasture().GetEastFenceOn())
			{
				this.SetWestFence(false, false);
			}
		}
		else
		{
			this.SetNorthFence(false, false);
			this.SetSouthFence(false, false);
			this.SetEastFence(false, false);
			this.SetWestFence(false, false);
		}
		this.m_pastureIndex = -1;
		this.m_proposedPastureIndex = -1;
		this.m_pastureCapacity = 0;
		if (this.m_fenceNodeNorthGlow != null)
		{
			this.m_fenceNodeNorthGlow.gameObject.SetActive(false);
		}
		if (this.m_fenceNodeSouthGlow != null)
		{
			this.m_fenceNodeSouthGlow.gameObject.SetActive(false);
		}
		if (this.m_fenceNodeEastGlow != null)
		{
			this.m_fenceNodeEastGlow.gameObject.SetActive(false);
		}
		if (this.m_fenceNodeWestGlow != null)
		{
			this.m_fenceNodeWestGlow.gameObject.SetActive(false);
		}
		this.m_stableNode.SetActive(false);
		this.m_stableNodeSheep.SetActive(false);
		this.m_stableNodeBoar.SetActive(false);
		this.m_stableNodeCattle.SetActive(false);
		this.m_signNode.SetActive(false);
	}

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private GameObject m_fenceNodeNorth;

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private GameObject m_fenceNodeSouth;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	private GameObject m_fenceNodeEast;

	// Token: 0x04000273 RID: 627
	[SerializeField]
	private GameObject m_fenceNodeWest;

	// Token: 0x04000274 RID: 628
	[SerializeField]
	private MeshRenderer m_fenceNodeNorthGlow;

	// Token: 0x04000275 RID: 629
	[SerializeField]
	private MeshRenderer m_fenceNodeSouthGlow;

	// Token: 0x04000276 RID: 630
	[SerializeField]
	private MeshRenderer m_fenceNodeEastGlow;

	// Token: 0x04000277 RID: 631
	[SerializeField]
	private MeshRenderer m_fenceNodeWestGlow;

	// Token: 0x04000278 RID: 632
	[SerializeField]
	private Material m_whiteGlowMaterial;

	// Token: 0x04000279 RID: 633
	[SerializeField]
	private Material m_redGlowMaterial;

	// Token: 0x0400027A RID: 634
	[SerializeField]
	private GameObject m_stableNode;

	// Token: 0x0400027B RID: 635
	[SerializeField]
	private GameObject m_stableNodeSheep;

	// Token: 0x0400027C RID: 636
	[SerializeField]
	private GameObject m_stableNodeBoar;

	// Token: 0x0400027D RID: 637
	[SerializeField]
	private GameObject m_stableNodeCattle;

	// Token: 0x0400027E RID: 638
	[SerializeField]
	private GameObject m_signNode;

	// Token: 0x0400027F RID: 639
	[SerializeField]
	private TextMeshProUGUI m_signText;

	// Token: 0x04000280 RID: 640
	[SerializeField]
	private Button m_fenceButtonNorth;

	// Token: 0x04000281 RID: 641
	[SerializeField]
	private Button m_fenceButtonSouth;

	// Token: 0x04000282 RID: 642
	[SerializeField]
	private Button m_fenceButtonEast;

	// Token: 0x04000283 RID: 643
	[SerializeField]
	private Button m_fenceButtonWest;

	// Token: 0x04000284 RID: 644
	[SerializeField]
	private Image m_fenceImageNorth;

	// Token: 0x04000285 RID: 645
	[SerializeField]
	private Image m_fenceImageSouth;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private Image m_fenceImageEast;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	private Image m_fenceImageWest;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private DragTargetZone m_fenceDropNorth;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	private DragTargetZone m_fenceDropSouth;

	// Token: 0x0400028A RID: 650
	[SerializeField]
	private DragTargetZone m_fenceDropEast;

	// Token: 0x0400028B RID: 651
	[SerializeField]
	private DragTargetZone m_fenceDropWest;

	// Token: 0x0400028C RID: 652
	[SerializeField]
	private int m_pastureIndex = -1;

	// Token: 0x0400028D RID: 653
	[SerializeField]
	private int m_proposedPastureIndex = -1;

	// Token: 0x0400028E RID: 654
	private int m_pastureCapacity;

	// Token: 0x0400028F RID: 655
	private int m_northFenceDataIndex;

	// Token: 0x04000290 RID: 656
	private int m_southFenceDataIndex;

	// Token: 0x04000291 RID: 657
	private int m_eastFenceDataIndex;

	// Token: 0x04000292 RID: 658
	private int m_westFenceDataIndex;

	// Token: 0x04000293 RID: 659
	private bool m_bFencingNeedsUpdate;

	// Token: 0x04000294 RID: 660
	private bool m_bNorthFenceProposed;

	// Token: 0x04000295 RID: 661
	private bool m_bSouthFenceProposed;

	// Token: 0x04000296 RID: 662
	private bool m_bEastFenceProposed;

	// Token: 0x04000297 RID: 663
	private bool m_bWestFenceProposed;

	// Token: 0x04000298 RID: 664
	private bool m_bProposedChecked;
}
