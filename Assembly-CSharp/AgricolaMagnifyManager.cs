using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class AgricolaMagnifyManager : MagnifyManager
{
	// Token: 0x060003DB RID: 987 RVA: 0x0001AE20 File Offset: 0x00019020
	private void SetMagnifyRates(CardObject magnifyCard)
	{
		AnimateObject component = magnifyCard.GetComponent<AnimateObject>();
		if (component != null)
		{
			float num = 1f;
			component.SetAnimateMovementRateXY(this.m_MagnifyMovementRateXY * num);
			component.SetAnimateMovementRateZ(this.m_MagnifyMovementRateZ * num);
			component.SetAnimateRotationRate(this.m_MagnifyRotationRate * num);
			component.SetAnimateScaleRate(this.m_MagnifyScaleRate * num);
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001AE7C File Offset: 0x0001907C
	public override AnimationLocator GetMagnifyCardLocator(CardObject magnifyCard)
	{
		if (this.m_AgricolaGame != null && this.m_OverviewMagnifyCardLocator != null && this.m_AgricolaGame.GetIsPaused())
		{
			return this.m_OverviewMagnifyCardLocator;
		}
		if (this.m_AgricolaGame != null && this.m_EndGameMagnifyCardLocator != null && this.m_AgricolaGame.GetIsEndGamePopupActive())
		{
			return this.m_EndGameMagnifyCardLocator;
		}
		return base.GetMagnifyCardLocator(magnifyCard);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001AEF0 File Offset: 0x000190F0
	public void SetEnableNeighborButtons(bool bEnable)
	{
		this.m_bEnableNeighborButtons = bEnable;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001AEF9 File Offset: 0x000190F9
	public void SetEnableMagnifyActionButtons(bool bEnable)
	{
		this.m_bEnableMagnifyActionButtons = bEnable;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001AF04 File Offset: 0x00019104
	public override bool StartMagnifyCard(CardObject magnifyCard)
	{
		if (this.m_MagnifiedCardActionButtonPanel != null)
		{
			this.m_MagnifiedCardActionButtonPanel.SetActive(false);
		}
		this.m_MagnifiedCardNeighborLeft = null;
		this.m_MagnifiedCardNeighborRight = null;
		if (this.m_bEnableNeighborButtons)
		{
			Transform parent = magnifyCard.gameObject.transform.parent;
			int i = magnifyCard.transform.GetSiblingIndex();
			bool flag = false;
			AgricolaBuilding component = parent.gameObject.GetComponent<AgricolaBuilding>();
			if (component != null)
			{
				flag = true;
				parent = component.transform.parent;
				i = component.transform.GetSiblingIndex();
			}
			MagnifyNeighborConnection component2 = parent.gameObject.GetComponent<MagnifyNeighborConnection>();
			if (component2 != null)
			{
				MagnifyNeighborConnection neighborLeft = component2.GetNeighborLeft();
				MagnifyNeighborConnection neighborRight = component2.GetNeighborRight();
				if (neighborLeft != null)
				{
					this.m_MagnifiedCardNeighborLeft = neighborLeft.FindNextComponentInLine<AgricolaCard>(MagnifyNeighborConnection.TravelDirection.Left, false);
					if (this.m_MagnifiedCardNeighborLeft == null)
					{
						AnimatePlaceholder animatePlaceholder = neighborLeft.FindNextComponentInLine<AnimatePlaceholder>(MagnifyNeighborConnection.TravelDirection.Left, false);
						if (animatePlaceholder != null)
						{
							AnimateObject owner = animatePlaceholder.GetOwner();
							if (owner != null)
							{
								this.m_MagnifiedCardNeighborLeft = owner.GetComponent<AgricolaCard>();
							}
						}
					}
					if (this.m_MagnifiedCardNeighborLeft == null)
					{
						this.m_MagnifiedCardNeighborLeft = neighborLeft.FindNextComponentInLine<AgricolaCard>(MagnifyNeighborConnection.TravelDirection.Left, true);
						if (this.m_MagnifiedCardNeighborLeft == null)
						{
							AnimatePlaceholder animatePlaceholder2 = neighborLeft.FindNextComponentInLine<AnimatePlaceholder>(MagnifyNeighborConnection.TravelDirection.Left, true);
							if (animatePlaceholder2 != null)
							{
								AnimateObject owner2 = animatePlaceholder2.GetOwner();
								if (owner2 != null)
								{
									this.m_MagnifiedCardNeighborLeft = owner2.GetComponent<AgricolaCard>();
								}
							}
						}
					}
				}
				if (neighborRight != null)
				{
					this.m_MagnifiedCardNeighborRight = neighborRight.FindNextComponentInLine<AgricolaCard>(MagnifyNeighborConnection.TravelDirection.Right, false);
					if (this.m_MagnifiedCardNeighborRight == null)
					{
						AnimatePlaceholder animatePlaceholder3 = neighborRight.FindNextComponentInLine<AnimatePlaceholder>(MagnifyNeighborConnection.TravelDirection.Right, false);
						if (animatePlaceholder3 != null)
						{
							AnimateObject owner3 = animatePlaceholder3.GetOwner();
							if (owner3 != null)
							{
								this.m_MagnifiedCardNeighborRight = owner3.GetComponent<AgricolaCard>();
							}
						}
					}
					if (this.m_MagnifiedCardNeighborRight == null)
					{
						this.m_MagnifiedCardNeighborRight = neighborRight.FindNextComponentInLine<AgricolaCard>(MagnifyNeighborConnection.TravelDirection.Right, false);
						if (this.m_MagnifiedCardNeighborRight == null)
						{
							AnimatePlaceholder animatePlaceholder4 = neighborRight.FindNextComponentInLine<AnimatePlaceholder>(MagnifyNeighborConnection.TravelDirection.Right, false);
							if (animatePlaceholder4 != null)
							{
								AnimateObject owner4 = animatePlaceholder4.GetOwner();
								if (owner4 != null)
								{
									this.m_MagnifiedCardNeighborRight = owner4.GetComponent<AgricolaCard>();
								}
							}
						}
					}
				}
			}
			else
			{
				while (i > 0)
				{
					Transform child = parent.GetChild(i - 1);
					if (child != null)
					{
						GameObject gameObject = child.gameObject;
						if (gameObject != null && gameObject.activeInHierarchy)
						{
							if (flag)
							{
								AgricolaBuilding component3 = gameObject.GetComponent<AgricolaBuilding>();
								if (component3 != null && component3.transform.childCount == 1)
								{
									gameObject = component3.transform.GetChild(0).gameObject;
								}
							}
							this.m_MagnifiedCardNeighborLeft = gameObject.GetComponent<AgricolaCard>();
							if (this.m_MagnifiedCardNeighborLeft == null)
							{
								AnimatePlaceholder component4 = gameObject.GetComponent<AnimatePlaceholder>();
								if (component4 != null)
								{
									AnimateObject owner5 = component4.GetOwner();
									if (owner5 != null)
									{
										this.m_MagnifiedCardNeighborLeft = owner5.GetComponent<AgricolaCard>();
									}
								}
							}
						}
					}
					if (this.m_MagnifiedCardNeighborLeft != null)
					{
						if (!this.m_MagnifiedCardNeighborLeft.IsShowingCardBack())
						{
							break;
						}
						this.m_MagnifiedCardNeighborLeft = null;
					}
					i--;
				}
				i = magnifyCard.transform.GetSiblingIndex();
				if (flag)
				{
					i = component.transform.GetSiblingIndex();
				}
				while (i + 1 < parent.childCount)
				{
					Transform child2 = parent.GetChild(i + 1);
					if (child2 != null)
					{
						GameObject gameObject2 = child2.gameObject;
						if (gameObject2 != null && gameObject2.activeInHierarchy)
						{
							if (flag)
							{
								AgricolaBuilding component5 = gameObject2.GetComponent<AgricolaBuilding>();
								if (component5 != null && component5.transform.childCount == 1)
								{
									gameObject2 = component5.transform.GetChild(0).gameObject;
								}
							}
							this.m_MagnifiedCardNeighborRight = gameObject2.GetComponent<AgricolaCard>();
							if (this.m_MagnifiedCardNeighborRight == null)
							{
								AnimatePlaceholder component6 = gameObject2.GetComponent<AnimatePlaceholder>();
								if (component6 != null)
								{
									AnimateObject owner6 = component6.GetOwner();
									if (owner6 != null)
									{
										this.m_MagnifiedCardNeighborRight = owner6.GetComponent<AgricolaCard>();
									}
								}
							}
						}
					}
					if (this.m_MagnifiedCardNeighborRight != null)
					{
						if (!this.m_MagnifiedCardNeighborRight.IsShowingCardBack())
						{
							break;
						}
						this.m_MagnifiedCardNeighborRight = null;
					}
					i++;
				}
			}
		}
		if (this.m_MagnifiedCardButtonLeft != null)
		{
			this.m_MagnifiedCardButtonLeft.SetActive(this.m_MagnifiedCardNeighborLeft != null);
		}
		if (this.m_MagnifiedCardButtonRight != null)
		{
			this.m_MagnifiedCardButtonRight.SetActive(this.m_MagnifiedCardNeighborRight != null);
		}
		DragObject component7 = magnifyCard.GetComponent<DragObject>();
		if (this.m_bEnableMagnifyActionButtons)
		{
			ushort instanceID = (ushort)magnifyCard.GetCardInstanceID();
			if (component7 != null && component7.GetDragSelectionID() != 0)
			{
				instanceID = component7.GetDragSelectionID();
			}
			List<ushort> selectionHints = GameOptions.GetSelectionHints(instanceID);
			if (GameOptions.IsSelectableHint(40998))
			{
				for (int j = 0; j < GameOptions.m_OptionCount; j++)
				{
					if (GameOptions.m_GameOption[j].selectionHint == 40998 && GameOptions.m_GameOption[j].isHidden == 0 && AgricolaLib.GetCardInstanceIDFromSubID((int)GameOptions.m_GameOption[j].selectionID, false, true) == magnifyCard.GetCardInstanceID())
					{
						selectionHints.Add(40998);
						instanceID = GameOptions.m_GameOption[j].selectionID;
						break;
					}
				}
			}
			if (selectionHints.Count > 0)
			{
				if (this.m_MagnifiedCardActionButtonPanel != null)
				{
					this.m_MagnifiedCardActionButtonPanel.SetActive(true);
				}
				int num = 0;
				using (List<ushort>.Enumerator enumerator = selectionHints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ushort selectionHint = enumerator.Current;
						if (num >= this.m_MagnifiedCardActionButtons.Length)
						{
							break;
						}
						DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
						if (dragSelectionHintDefinition != null && dragSelectionHintDefinition.m_OptionTextDisplay != string.Empty)
						{
							GameObject gameObject3 = this.m_MagnifiedCardActionButtons[num++];
							gameObject3.SetActive(true);
							Transform transform = gameObject3.transform.Find("Button00/Label");
							if (transform != null)
							{
								UILocalizedText component8 = transform.gameObject.GetComponent<UILocalizedText>();
								if (component8 != null)
								{
									component8.KeyText = dragSelectionHintDefinition.m_OptionTextDisplay;
								}
							}
						}
					}
					goto IL_655;
				}
				IL_641:
				this.m_MagnifiedCardActionButtons[num++].SetActive(false);
				IL_655:
				if (num < this.m_MagnifiedCardActionButtons.Length)
				{
					goto IL_641;
				}
			}
		}
		if (base.StartMagnifyCard(magnifyCard))
		{
			this.SetMagnifyRates(magnifyCard);
			return true;
		}
		return false;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001B598 File Offset: 0x00019798
	public override bool RemoveMagnifiedCard(CardObject magnifyCard)
	{
		if (!base.RemoveMagnifiedCard(magnifyCard))
		{
			return false;
		}
		this.SetMagnifyRates(magnifyCard);
		if (this.m_MagnifiedCardActionButtonPanel != null)
		{
			this.m_MagnifiedCardActionButtonPanel.SetActive(false);
		}
		if (this.m_MagnifiedCardButtonLeft != null)
		{
			this.m_MagnifiedCardButtonLeft.SetActive(false);
		}
		if (this.m_MagnifiedCardButtonRight != null)
		{
			this.m_MagnifiedCardButtonRight.SetActive(false);
		}
		this.m_MagnifiedCardNeighborLeft = null;
		this.m_MagnifiedCardNeighborRight = null;
		return true;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001B614 File Offset: 0x00019814
	public void OnMagnifiedCardButtonLeftPressed()
	{
		AgricolaCard agricolaCard = base.GetMagnifiedCard() as AgricolaCard;
		if (agricolaCard == null || this.m_MagnifiedCardNeighborLeft == null)
		{
			return;
		}
		if (agricolaCard.GetComponent<AnimateObject>().IsAnimating())
		{
			return;
		}
		AgricolaCard magnifiedCardNeighborLeft = this.m_MagnifiedCardNeighborLeft;
		if (magnifiedCardNeighborLeft == null)
		{
			return;
		}
		if (magnifiedCardNeighborLeft.GetComponent<AnimateObject>().IsAnimating())
		{
			return;
		}
		agricolaCard.Unmagnify(false, true);
		magnifiedCardNeighborLeft.Magnify(false, false);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001B684 File Offset: 0x00019884
	public void OnMagnifiedCardButtonRightPressed()
	{
		AgricolaCard agricolaCard = base.GetMagnifiedCard() as AgricolaCard;
		if (agricolaCard == null || this.m_MagnifiedCardNeighborRight == null)
		{
			return;
		}
		if (agricolaCard.GetComponent<AnimateObject>().IsAnimating())
		{
			return;
		}
		AgricolaCard magnifiedCardNeighborRight = this.m_MagnifiedCardNeighborRight;
		if (magnifiedCardNeighborRight == null)
		{
			return;
		}
		if (magnifiedCardNeighborRight.GetComponent<AnimateObject>().IsAnimating())
		{
			return;
		}
		agricolaCard.Unmagnify(false, true);
		magnifiedCardNeighborRight.Magnify(false, false);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001B6F4 File Offset: 0x000198F4
	public void OnMagnifiedCardActionButtonPressed(GameObject button_pressed)
	{
		if (!this.m_bEnableMagnifyActionButtons)
		{
			return;
		}
		CardObject magnifiedCard = base.GetMagnifiedCard();
		if (magnifiedCard == null)
		{
			return;
		}
		ushort instanceID = (ushort)magnifiedCard.GetCardInstanceID();
		DragObject component = magnifiedCard.GetComponent<DragObject>();
		if (component != null && component.GetDragSelectionID() != 0)
		{
			instanceID = component.GetDragSelectionID();
		}
		int num = 0;
		List<ushort> selectionHints = GameOptions.GetSelectionHints(instanceID);
		if (GameOptions.IsSelectableHint(40998))
		{
			for (int i = 0; i < GameOptions.m_OptionCount; i++)
			{
				if (GameOptions.m_GameOption[i].selectionHint == 40998 && GameOptions.m_GameOption[i].isHidden == 0 && (int)((ushort)AgricolaLib.GetCardInstanceIDFromSubID((int)GameOptions.m_GameOption[i].selectionID, false, true)) == magnifiedCard.GetCardInstanceID())
				{
					selectionHints.Add(40998);
					instanceID = GameOptions.m_GameOption[i].selectionID;
					break;
				}
			}
		}
		foreach (ushort selectionHint in selectionHints)
		{
			if (num >= this.m_MagnifiedCardActionButtons.Length)
			{
				break;
			}
			bool flag = false;
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
			if (dragSelectionHintDefinition != null && dragSelectionHintDefinition.m_OptionTextDisplay != string.Empty)
			{
				flag = true;
			}
			if (flag && !(this.m_MagnifiedCardActionButtons[num++] != button_pressed) && GameOptions.SelectOptionByInstanceIDAndHint(instanceID, selectionHint))
			{
				AgricolaCard agricolaCard = magnifiedCard as AgricolaCard;
				if (agricolaCard != null)
				{
					agricolaCard.Unmagnify(false, true);
				}
				break;
			}
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001B894 File Offset: 0x00019A94
	public void OnUnmagnifyButtonPressed()
	{
		AgricolaCard agricolaCard = base.GetMagnifiedCard() as AgricolaCard;
		if (agricolaCard == null)
		{
			return;
		}
		if (agricolaCard.GetComponent<AnimateObject>().IsAnimating())
		{
			return;
		}
		agricolaCard.Unmagnify(false, true);
	}

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private AgricolaGame m_AgricolaGame;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private AnimationLocator m_TutorialMagnifyCardLocator;

	// Token: 0x0400031F RID: 799
	[SerializeField]
	private AnimationLocator m_OverviewMagnifyCardLocator;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private AnimationLocator m_EndGameMagnifyCardLocator;

	// Token: 0x04000321 RID: 801
	[SerializeField]
	private GameObject m_MagnifiedCardButtonLeft;

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private GameObject m_MagnifiedCardButtonRight;

	// Token: 0x04000323 RID: 803
	[SerializeField]
	private GameObject m_MagnifiedCardActionButtonPanel;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private GameObject[] m_MagnifiedCardActionButtons;

	// Token: 0x04000325 RID: 805
	private const float k_DefaultMovementRateXY = 32f;

	// Token: 0x04000326 RID: 806
	private const float k_DefaultMovementRateZ = 384f;

	// Token: 0x04000327 RID: 807
	private const float k_DefaultRotationRate = 720f;

	// Token: 0x04000328 RID: 808
	private const float k_DefaultScaleRate = 1.8f;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	private float m_MagnifyMovementRateXY = 32f;

	// Token: 0x0400032A RID: 810
	[SerializeField]
	private float m_MagnifyMovementRateZ = 384f;

	// Token: 0x0400032B RID: 811
	[SerializeField]
	private float m_MagnifyRotationRate = 720f;

	// Token: 0x0400032C RID: 812
	[SerializeField]
	private float m_MagnifyScaleRate = 1.8f;

	// Token: 0x0400032D RID: 813
	private AgricolaCard m_MagnifiedCardNeighborLeft;

	// Token: 0x0400032E RID: 814
	private AgricolaCard m_MagnifiedCardNeighborRight;

	// Token: 0x0400032F RID: 815
	private bool m_bEnableNeighborButtons = true;

	// Token: 0x04000330 RID: 816
	private bool m_bEnableMagnifyActionButtons = true;
}
