using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000023 RID: 35
[RequireComponent(typeof(DragTargetZone))]
public class AgricolaActionSpace : CardInPlay, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000167 RID: 359 RVA: 0x000079CC File Offset: 0x00005BCC
	public override void SetSourceCard(CardObject sourceCard)
	{
		if (this.m_SourceCard != null)
		{
			DragObject component = this.m_SourceCard.GetComponent<DragObject>();
			if (component != null)
			{
				component.SetDragSelectionOverrideID(0);
			}
		}
		base.SetSourceCard(sourceCard);
		if (sourceCard as AgricolaCard != null)
		{
			DragObject component2 = sourceCard.GetComponent<DragObject>();
			if (component2 != null)
			{
				component2.SetDragSelectionOverrideID((ushort)base.GetCardInPlayInstanceID());
				return;
			}
		}
		else
		{
			this.m_WorkerLocator = null;
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00007A40 File Offset: 0x00005C40
	private void Awake()
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndAnimationCallback));
		}
		this.ActivateSignPostGlowList(false);
		if (this.m_AnimatorPopupCard != null)
		{
			this.m_AnimatorPopupCard.SetBool("isOpen", false);
		}
		if (this.m_LockedPopupCard != null && this.m_LockedPopupCard.activeInHierarchy)
		{
			Animator component2 = this.m_LockedPopupCard.GetComponent<Animator>();
			if (component2 != null)
			{
				component2.SetBool("isOpen", false);
			}
		}
		if (this.m_BackgroundAvailable != null)
		{
			this.m_BackgroundAvailable.SetActive(true);
		}
		if (this.m_BackgroundDisable != null)
		{
			this.m_BackgroundDisable.SetActive(false);
		}
		if (this.m_BackgroundLocked != null)
		{
			this.m_BackgroundLocked.SetActive(false);
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00007B20 File Offset: 0x00005D20
	public bool IsActionSpaceName(string actionspace_name)
	{
		if (this.m_ActionSpaceNames != null)
		{
			string[] actionSpaceNames = this.m_ActionSpaceNames;
			for (int i = 0; i < actionSpaceNames.Length; i++)
			{
				if (actionSpaceNames[i] == actionspace_name)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00007B58 File Offset: 0x00005D58
	public AgricolaAnimationLocator GetWorkerLocator()
	{
		return this.m_WorkerLocator;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00007B60 File Offset: 0x00005D60
	public string GetActiveActionSpaceName()
	{
		return this.m_ActiveActionSpaceName;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00007B68 File Offset: 0x00005D68
	public bool SetActiveActionSpaceName(string actionspace_name)
	{
		if (this.m_ActionSpaceNames != null)
		{
			foreach (string text in this.m_ActionSpaceNames)
			{
				if (text == actionspace_name)
				{
					this.m_ActiveActionSpaceName = text;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00007BA9 File Offset: 0x00005DA9
	public int GetActionSpaceInstanceID()
	{
		return base.GetCardInPlayInstanceID();
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00007BB4 File Offset: 0x00005DB4
	public void SetActionSpaceInstanceID(int instanceID)
	{
		base.SetCardInPlayInstanceID(instanceID);
		DragTargetZone component = base.GetComponent<DragTargetZone>();
		if (component != null)
		{
			component.SetDragTargetInstanceID((ushort)instanceID);
			component.AddOnDropCallback(new DragTargetZone.OnDropCallback(this.OnDropCallback));
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00007BF2 File Offset: 0x00005DF2
	private void OnDropCallback(DragObject drag, ushort selectionHint)
	{
		this.ActivatePopupCard(false);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00007BFB File Offset: 0x00005DFB
	public void SetActionSpaceActive(bool bActive)
	{
		base.gameObject.SetActive(bActive);
		this.ShowActionSpaceSignPosts(bActive);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00007C10 File Offset: 0x00005E10
	public void ShowActionSpaceSignPosts(bool bShowSignPosts)
	{
		if (this.m_SignPostList != null)
		{
			for (int i = 0; i < this.m_SignPostList.Length; i++)
			{
				this.m_SignPostList[i].SetActive(bShowSignPosts);
			}
		}
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00007C48 File Offset: 0x00005E48
	public void SetLockedPopupCard(bool bLocked)
	{
		if (this.m_LockedPopupCard != null)
		{
			this.m_LockedPopupCard.SetActive(bLocked);
		}
		if (bLocked)
		{
			if (this.m_BackgroundAvailable != null)
			{
				this.m_BackgroundAvailable.SetActive(false);
			}
			if (this.m_BackgroundDisable != null)
			{
				this.m_BackgroundDisable.SetActive(false);
			}
			if (this.m_BackgroundLocked != null)
			{
				this.m_BackgroundLocked.SetActive(true);
				return;
			}
		}
		else
		{
			if (this.m_BackgroundLocked != null)
			{
				this.m_BackgroundLocked.SetActive(false);
			}
			if (this.m_BackgroundAvailable != null)
			{
				if (this.m_BackgroundDisable == null || !this.m_BackgroundDisable.activeSelf)
				{
					this.m_BackgroundAvailable.SetActive(true);
					return;
				}
			}
			else if (this.m_BackgroundDisable != null)
			{
				this.m_BackgroundDisable.SetActive(true);
			}
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00007D2C File Offset: 0x00005F2C
	public void SetAccumulateResourceCount(int resourceCount)
	{
		if (this.m_AccumulateResourceCount == resourceCount)
		{
			return;
		}
		this.m_AccumulateResourceCount = resourceCount;
		if (this.m_AccumulateResourceNodeOpen != null)
		{
			this.m_AccumulateResourceNodeOpen.SetActive(this.m_AccumulateResourceCount > 0);
		}
		if (this.m_AccumulateResourceTextOpen != null)
		{
			this.m_AccumulateResourceTextOpen.SetText(this.m_AccumulateResourceCount.ToString());
		}
		if (this.m_AccumulateResourceNodeClosed != null)
		{
			bool active = this.m_AccumulateResourceCount > 0;
			for (int i = 0; i < this.m_AccumulateResourceNodeClosed.Length; i++)
			{
				this.m_AccumulateResourceNodeClosed[i].SetActive(active);
			}
		}
		if (this.m_AccumulateResourceTextClosed != null)
		{
			string text = this.m_AccumulateResourceCount.ToString();
			for (int j = 0; j < this.m_AccumulateResourceNodeClosed.Length; j++)
			{
				this.m_AccumulateResourceTextClosed[j].SetText(text);
			}
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00007DF8 File Offset: 0x00005FF8
	public virtual void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (this.GetActionSpaceInstanceID() == 0 || animation_manager == null)
		{
			return;
		}
		if (this.m_WorkerLocator != null)
		{
			animation_manager.SetAnimationLocator(13, this.GetActionSpaceInstanceID(), this.m_WorkerLocator);
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00007E30 File Offset: 0x00006030
	private void EndAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		AgricolaAnimationLocator agricolaAnimationLocator = destinationLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator != null)
		{
			float cardDisplayScale = agricolaAnimationLocator.GetCardDisplayScale();
			if (cardDisplayScale >= 0f)
			{
				AnimateObject component = base.GetComponent<AnimateObject>();
				if (component != null)
				{
					component.SetCurrentScale(cardDisplayScale);
					component.SetTargetScale(cardDisplayScale);
				}
			}
			AgricolaAnimationLocator component2 = base.GetComponent<AgricolaAnimationLocator>();
			if (component2 != null)
			{
				component2.SetOverrideAnimationLayer(agricolaAnimationLocator.GetOverrideAnimationLayer());
				component2.SetInterceptHorizontalCardDrag(agricolaAnimationLocator.InterceptHorizontalCardDrag());
				component2.SetInterceptVerticalCardDrag(agricolaAnimationLocator.InterceptVerticalCardDrag());
				component2.SetCardDragType(agricolaAnimationLocator.GetCardDragType());
			}
		}
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00007EBC File Offset: 0x000060BC
	private void ActivateSignPostGlowList(bool bSetActive)
	{
		if (this.m_SignPostGlowList != null)
		{
			for (int i = 0; i < this.m_SignPostGlowList.Length; i++)
			{
				if (this.m_SignPostGlowList[i] != null && this.m_SignPostGlowList[i].activeSelf != bSetActive)
				{
					this.m_SignPostGlowList[i].SetActive(bSetActive);
				}
			}
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00007F14 File Offset: 0x00006114
	public override void UpdateSelectionState(bool bHighlight)
	{
		if (this.m_SourceCard != null && bHighlight)
		{
			DragObject component = this.m_SourceCard.GetComponent<DragObject>();
			if (component != null)
			{
				component.SetDragSelectionOverrideID((ushort)base.GetCardInPlayInstanceID());
			}
		}
		ushort num = (ushort)base.GetCardInPlayInstanceID();
		if (num != 0)
		{
			foreach (ushort selectionHint in GameOptions.GetSelectionHints(num))
			{
				DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
				if (dragSelectionHintDefinition != null)
				{
					if (dragSelectionHintDefinition.m_bUseWorkerSpaceGlow)
					{
						this.ActivateSignPostGlowList(true);
						if (this.m_WorkerSpaceGlow != null && !this.m_WorkerSpaceGlow.activeSelf)
						{
							this.m_WorkerSpaceGlow.SetActive(true);
						}
					}
					else
					{
						this.ActivateSignPostGlowList(false);
						if (this.m_WorkerSpaceGlow != null && this.m_WorkerSpaceGlow.activeSelf)
						{
							this.m_WorkerSpaceGlow.SetActive(false);
						}
						if (this.m_SourceCard != null && bHighlight)
						{
							this.m_SourceCard.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
						}
					}
					if (this.m_BackgroundLocked == null || !this.m_BackgroundLocked.activeSelf)
					{
						if (this.m_BackgroundAvailable != null)
						{
							this.m_BackgroundAvailable.SetActive(true);
						}
						if (this.m_BackgroundDisable != null)
						{
							this.m_BackgroundDisable.SetActive(false);
						}
					}
					return;
				}
			}
		}
		this.ActivateSignPostGlowList(false);
		if (this.m_WorkerSpaceGlow != null && this.m_WorkerSpaceGlow.activeSelf)
		{
			this.m_WorkerSpaceGlow.SetActive(false);
		}
		if (this.m_BackgroundLocked == null || !this.m_BackgroundLocked.activeSelf)
		{
			if (this.m_BackgroundDisable != null)
			{
				this.m_BackgroundDisable.SetActive(true);
			}
			if (this.m_BackgroundAvailable != null)
			{
				this.m_BackgroundAvailable.SetActive(false);
			}
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00008118 File Offset: 0x00006318
	public void AssignWorker(AgricolaWorker worker)
	{
		if (worker == null)
		{
			return;
		}
		AnimateObject component = worker.GetComponent<AnimateObject>();
		if (component == null)
		{
			return;
		}
		if (this.m_WorkerLocator != null)
		{
			this.m_WorkerLocator.PlaceAnimateObject(component, true, true, false);
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00008160 File Offset: 0x00006360
	public void MagnifySourceCard()
	{
		GameObject gameObject = GameObject.Find("/Agricola Card Manager");
		if (gameObject == null)
		{
			return;
		}
		AgricolaCardManager component = gameObject.GetComponent<AgricolaCardManager>();
		if (component == null)
		{
			return;
		}
		AgricolaCard agricolaCard = base.GetSourceCard() as AgricolaCard;
		GameObject gameObject2;
		if (agricolaCard != null)
		{
			gameObject2 = agricolaCard.gameObject;
		}
		else
		{
			gameObject2 = component.GetCardFromInstanceID(base.GetSourceCardInstanceID(), true);
			if (gameObject2 != null)
			{
				agricolaCard = gameObject2.GetComponent<AgricolaCard>();
				this.SetSourceCard(agricolaCard);
				DragObject component2 = gameObject2.GetComponent<DragObject>();
				if (component2 != null)
				{
					component2.SetDragSelectionOverrideID((ushort)base.GetCardInPlayInstanceID());
				}
			}
		}
		if (gameObject2 == null)
		{
			return;
		}
		AgricolaAnimationLocator component3 = base.GetComponent<AgricolaAnimationLocator>();
		if (component3 == null)
		{
			return;
		}
		gameObject2.SetActive(true);
		component3.PlaceAnimateObject(gameObject2.GetComponent<AnimateObject>(), true, true, true);
		agricolaCard.Magnify(false, false);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00008235 File Offset: 0x00006435
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			this.MagnifySourceCard();
			return;
		}
		this.TogglePopupCard();
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00008250 File Offset: 0x00006450
	public void TogglePopupCard()
	{
		bool value = false;
		if (this.m_AnimatorPopupCard != null)
		{
			value = !this.m_AnimatorPopupCard.GetBool("isOpen");
			this.m_AnimatorPopupCard.SetBool("isOpen", value);
		}
		if (this.m_LockedPopupCard != null && this.m_LockedPopupCard.activeInHierarchy)
		{
			Animator component = this.m_LockedPopupCard.GetComponent<Animator>();
			if (component != null)
			{
				component.SetBool("isOpen", value);
			}
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x000082D0 File Offset: 0x000064D0
	public void ActivatePopupCard(bool bActive)
	{
		if (this.m_AnimatorPopupCard != null)
		{
			this.m_AnimatorPopupCard.SetBool("isOpen", bActive);
		}
		if (this.m_LockedPopupCard != null && this.m_LockedPopupCard.activeInHierarchy)
		{
			Animator component = this.m_LockedPopupCard.GetComponent<Animator>();
			if (component != null)
			{
				component.SetBool("isOpen", bActive);
			}
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00008338 File Offset: 0x00006538
	private IEnumerator DelayedPressTrigger()
	{
		yield return new WaitForSeconds(this.m_delayedPressTriggerTimeLimit);
		this.m_delayedPressTrigger = null;
		this.TogglePopupCard();
		yield break;
	}

	// Token: 0x040000AA RID: 170
	[SerializeField]
	private string[] m_ActionSpaceNames;

	// Token: 0x040000AB RID: 171
	[SerializeField]
	private GameObject[] m_SignPostList;

	// Token: 0x040000AC RID: 172
	[SerializeField]
	protected AgricolaAnimationLocator m_WorkerLocator;

	// Token: 0x040000AD RID: 173
	[SerializeField]
	private GameObject m_WorkerSpaceGlow;

	// Token: 0x040000AE RID: 174
	[SerializeField]
	private GameObject[] m_SignPostGlowList;

	// Token: 0x040000AF RID: 175
	[SerializeField]
	private Animator m_AnimatorPopupCard;

	// Token: 0x040000B0 RID: 176
	[SerializeField]
	private GameObject m_LockedPopupCard;

	// Token: 0x040000B1 RID: 177
	[SerializeField]
	private GameObject m_BackgroundAvailable;

	// Token: 0x040000B2 RID: 178
	[SerializeField]
	private GameObject m_BackgroundDisable;

	// Token: 0x040000B3 RID: 179
	[SerializeField]
	private GameObject m_BackgroundLocked;

	// Token: 0x040000B4 RID: 180
	[SerializeField]
	private GameObject m_AccumulateResourceNodeOpen;

	// Token: 0x040000B5 RID: 181
	[SerializeField]
	private TextMeshProUGUI m_AccumulateResourceTextOpen;

	// Token: 0x040000B6 RID: 182
	[SerializeField]
	private GameObject[] m_AccumulateResourceNodeClosed;

	// Token: 0x040000B7 RID: 183
	[SerializeField]
	private TextMeshProUGUI[] m_AccumulateResourceTextClosed;

	// Token: 0x040000B8 RID: 184
	private string m_ActiveActionSpaceName;

	// Token: 0x040000B9 RID: 185
	private int m_AccumulateResourceCount = -1;

	// Token: 0x040000BA RID: 186
	private Coroutine m_delayedPressTrigger;

	// Token: 0x040000BB RID: 187
	private float m_delayedPressTriggerTimeLimit = 0.7f;
}
