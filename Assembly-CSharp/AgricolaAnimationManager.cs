using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200002B RID: 43
public class AgricolaAnimationManager : AnimationManager
{
	// Token: 0x060001B4 RID: 436 RVA: 0x0000906B File Offset: 0x0000726B
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(2, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAnimationCard));
		game_event_buffer.RegisterEventHandler(3, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAnimationWorker));
		game_event_buffer.RegisterEventHandler(4, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAnimationResource));
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000090AC File Offset: 0x000072AC
	protected override void Update()
	{
		base.Update();
		for (int i = 0; i < this.m_ResourceHitEffectList.Count; i++)
		{
			GameObject gameObject = this.m_ResourceHitEffectList[i];
			if (gameObject != null)
			{
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null && component.GetCurrentAnimatorStateInfo(0).IsName("Done"))
				{
					this.m_ResourceHitEffectList.Remove(gameObject);
					UnityEngine.Object.Destroy(gameObject);
					continue;
				}
			}
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00009128 File Offset: 0x00007328
	public void RebuildAnimationManager()
	{
		if (this.m_LocatorAnnounceVisible != null)
		{
			base.SetAnimationLocator(1, 0, this.m_LocatorAnnounceVisible);
			base.SetAnimationLocator(1, 1, this.m_LocatorAnnounceVisible);
		}
		if (this.m_LocatorAnnounceHidden != null)
		{
			base.SetAnimationLocator(1, 2, this.m_LocatorAnnounceHidden);
		}
		base.ResetAnimationManager();
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00009184 File Offset: 0x00007384
	public void SetAnimationRatesCard(AnimateObject animate_object)
	{
		float @float = PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f);
		float num = Mathf.Clamp(@float, 0f, 2f);
		num = num * 1.25f + 0.75f;
		float num2 = Mathf.Clamp(@float, 1.334f, 2f);
		num += (num2 - 1.334f) * 1.5f;
		animate_object.SetAnimateMovementRateXY(768f * num);
		animate_object.SetAnimateMovementRateZ(288f * num);
		animate_object.SetAnimateRotationRate(540f * num);
		animate_object.SetAnimateScaleRate(0.2f * num);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00009214 File Offset: 0x00007414
	public void SetAnimationRatesWorker(AnimateObject animate_object)
	{
		float num = Mathf.Clamp(PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f), 0f, 2f);
		num = num * 1f + 0.5f;
		animate_object.SetAnimateMovementRateXY(768f * num);
		animate_object.SetAnimateMovementRateZ(288f * num);
		animate_object.SetAnimateRotationRate(540f * num);
		animate_object.SetAnimateScaleRate(0.2f * num);
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00009284 File Offset: 0x00007484
	public void SetAnimationRatesResource(AnimateObject animate_object)
	{
		float num = Mathf.Clamp(PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f), 0f, 2f);
		num = num * 1f + 0.5f;
		animate_object.SetAnimateMovementRateXY(768f * num);
		animate_object.SetAnimateMovementRateZ(288f * num);
		animate_object.SetAnimateRotationRate(540f * num);
		animate_object.SetAnimateScaleRate(0.2f * num);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000092F4 File Offset: 0x000074F4
	public float AdjustAnimationPauseTime(float min_time, float max_time)
	{
		float num = Mathf.Clamp(PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f), 0f, 2f);
		num *= 0.5f;
		return max_time + (min_time - max_time) * num;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00009330 File Offset: 0x00007530
	private void HandleEventAnimationCard(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputEventAnimationCard outputEventAnimationCard = (OutputEventAnimationCard)Marshal.PtrToStructure(event_buffer, typeof(OutputEventAnimationCard));
		int animation_source_location = outputEventAnimationCard.animation_source_location;
		int animation_source_instance_id = outputEventAnimationCard.animation_source_instance_id;
		int num = outputEventAnimationCard.animation_destination_location;
		int animation_destination_instance_id = outputEventAnimationCard.animation_destination_instance_id;
		if (this.m_CardManager == null)
		{
			return;
		}
		if ((outputEventAnimationCard.animation_extra_data & 16) != 0 && AgricolaLib.GetIsTutorialGame())
		{
			return;
		}
		if ((outputEventAnimationCard.animation_extra_data & 8192) != 0)
		{
			int localPlayerInstanceID = AgricolaLib.GetLocalPlayerInstanceID();
			if (outputEventAnimationCard.animation_source_instance_id == localPlayerInstanceID || outputEventAnimationCard.animation_owner_instance_id == localPlayerInstanceID)
			{
				return;
			}
		}
		GameObject gameObject;
		if ((outputEventAnimationCard.animation_extra_data & 4096) != 0)
		{
			gameObject = this.m_CardManager.CreateTemporaryCardFromInstanceID(outputEventAnimationCard.card_instance_id);
		}
		else
		{
			gameObject = this.m_CardManager.GetCardFromInstanceID(outputEventAnimationCard.card_instance_id, true);
		}
		if (gameObject == null)
		{
			Debug.LogError("   OutputEventAnimationCard cardinstance not found: " + outputEventAnimationCard.card_instance_id.ToString());
			return;
		}
		if (this.m_AgricolaGame != null)
		{
			if ((outputEventAnimationCard.animation_extra_data & 2) != 0)
			{
				this.m_AgricolaGame.SetTown();
			}
			else if ((outputEventAnimationCard.animation_extra_data & 1) != 0)
			{
				if (outputEventAnimationCard.animation_owner_instance_id != 0)
				{
					int playerIndexFromInstanceID = AgricolaLib.GetPlayerIndexFromInstanceID(outputEventAnimationCard.animation_owner_instance_id);
					if (playerIndexFromInstanceID != 0)
					{
						this.m_AgricolaGame.SetFarmToPlayer(playerIndexFromInstanceID, outputEventAnimationCard.animation_owner_instance_id);
					}
				}
				else
				{
					int playerIndexFromInstanceID2 = AgricolaLib.GetPlayerIndexFromInstanceID(outputEventAnimationCard.animation_source_instance_id);
					if (playerIndexFromInstanceID2 != 0)
					{
						this.m_AgricolaGame.SetFarmToPlayer(playerIndexFromInstanceID2, outputEventAnimationCard.animation_source_instance_id);
					}
				}
			}
		}
		AgricolaCard component = gameObject.GetComponent<AgricolaCard>();
		AnimateObject component2 = gameObject.GetComponent<AnimateObject>();
		if ((outputEventAnimationCard.animation_extra_data & 4096) != 0)
		{
			component2.SetDestroyAfterAnimation();
		}
		float pauseAtDestination = 0f;
		if ((outputEventAnimationCard.animation_extra_data & 16384) == 0 || !(this.m_AgricolaGame != null) || outputEventAnimationCard.animation_owner_instance_id == this.m_AgricolaGame.GetLocalPlayerInstanceID())
		{
			if (component != null)
			{
				if (outputEventAnimationCard.effect_type > 0)
				{
					if (outputEventAnimationCard.effect_type < AgricolaAnimationManager.m_CardEffectColor.Length)
					{
						component.SetGlowOverride(true, AgricolaAnimationManager.m_CardEffectColor[outputEventAnimationCard.effect_type], false);
					}
					else
					{
						component.SetGlowOverride(true, AgricolaAnimationManager.m_CardEffectColor[0], false);
					}
				}
				else
				{
					component.SetGlowOverride(false, Color.white, false);
				}
			}
			uint animationFlags = 0U;
			if (animation_source_location == 12)
			{
				if (this.m_DraftInterface != null)
				{
					AgricolaAnimationLocator agricolaAnimationLocator = this.m_DraftInterface.FindDraftPileAnimationLocator(component, false);
					if (agricolaAnimationLocator != null)
					{
						base.SetAnimationLocator(12, animation_source_instance_id, agricolaAnimationLocator);
					}
				}
			}
			else if (animation_source_location == 20 && this.m_DraftInterface != null)
			{
				AgricolaAnimationLocator agricolaAnimationLocator2 = this.m_DraftInterface.FindDraftPileAnimationLocator(component, true);
				if (agricolaAnimationLocator2 != null)
				{
					base.SetAnimationLocator(20, animation_source_instance_id, agricolaAnimationLocator2);
				}
			}
			if (num == 12)
			{
				if (this.m_DraftInterface != null)
				{
					AgricolaAnimationLocator agricolaAnimationLocator3 = this.m_DraftInterface.FindDraftPileAnimationLocator(component, false);
					if (agricolaAnimationLocator3 != null)
					{
						base.SetAnimationLocator(12, animation_destination_instance_id, agricolaAnimationLocator3);
					}
				}
			}
			else if (num == 20 && this.m_DraftInterface != null)
			{
				AgricolaAnimationLocator agricolaAnimationLocator4 = this.m_DraftInterface.FindDraftPileAnimationLocator(component, true);
				if (agricolaAnimationLocator4 != null)
				{
					base.SetAnimationLocator(20, animation_destination_instance_id, agricolaAnimationLocator4);
				}
			}
			if (num == 1 && this.m_DraftInterface != null)
			{
				AgricolaAnimationLocator agricolaAnimationLocator5 = this.m_DraftInterface.FindDraftSelectedAnimationLocator(component);
				if (agricolaAnimationLocator5 != null)
				{
					num = 40;
					base.SetAnimationLocator(num, animation_destination_instance_id, agricolaAnimationLocator5);
				}
			}
			if (outputEventAnimationCard.animation_event_hint == 7 || (outputEventAnimationCard.animation_event_hint == 5 && outputEventAnimationCard.animation_source_instance_id != AgricolaLib.GetLocalPlayerInstanceID()))
			{
				int num2 = 1;
				float pauseAtDestination2 = this.AdjustAnimationPauseTime(0.75f, 3f);
				if (base.StartAnimation(component2, animation_source_location, animation_source_instance_id, 1, num2, animationFlags, Vector3.zero, 0f, pauseAtDestination2, true))
				{
					this.SetAnimationRatesCard(component2);
					base.QueueAnimation(component2, 1, num2, num, animation_destination_instance_id, 0U, Vector3.zero, 0f, pauseAtDestination);
					return;
				}
			}
			else if (base.StartAnimation(component2, animation_source_location, animation_source_instance_id, num, animation_destination_instance_id, animationFlags, Vector3.zero, 0f, pauseAtDestination, true))
			{
				this.SetAnimationRatesCard(component2);
				return;
			}
			gameObject.SetActive(false);
			return;
		}
		if (!component2.gameObject.activeInHierarchy)
		{
			gameObject.SetActive(true);
		}
		AnimationLocator animationLocator = base.GetAnimationLocator(num, animation_destination_instance_id);
		if (animationLocator != null)
		{
			animationLocator.PlaceAnimateObject(component2, true, true, false);
			return;
		}
		if (component != null)
		{
			this.m_CardManager.PlaceCardInCardLimbo(component);
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000977C File Offset: 0x0000797C
	private void HandleEventAnimationWorker(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputEventAnimationWorker outputEventAnimationWorker = (OutputEventAnimationWorker)Marshal.PtrToStructure(event_buffer, typeof(OutputEventAnimationWorker));
		if (this.m_WorkerManager != null)
		{
			GameObject workerFromInstanceID = this.m_WorkerManager.GetWorkerFromInstanceID(outputEventAnimationWorker.worker_instance_id, true);
			if (workerFromInstanceID != null)
			{
				AnimateObject component = workerFromInstanceID.GetComponent<AnimateObject>();
				float pauseAtDestination = 0f;
				if (base.StartAnimation(component, outputEventAnimationWorker.animation_source_location, outputEventAnimationWorker.animation_source_instance_id, outputEventAnimationWorker.animation_destination_location, outputEventAnimationWorker.animation_destination_instance_id, 0U, Vector3.zero, 0f, pauseAtDestination, true))
				{
					this.SetAnimationRatesWorker(component);
					return;
				}
				workerFromInstanceID.SetActive(false);
			}
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00009813 File Offset: 0x00007A13
	public void SetPendingResourceAnimation(AgricolaResource pending_resource)
	{
		this.m_PendingResourceAnimation = pending_resource;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000981C File Offset: 0x00007A1C
	private void HandleEventAnimationResource(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputEventAnimationResource outputEventAnimationResource = (OutputEventAnimationResource)Marshal.PtrToStructure(event_buffer, typeof(OutputEventAnimationResource));
		this.StartAnimationResource(outputEventAnimationResource.resource_type, (int)outputEventAnimationResource.resource_count, (int)outputEventAnimationResource.animation_source_location, (int)outputEventAnimationResource.animation_source_instance_id, (int)outputEventAnimationResource.animation_destination_location, (int)outputEventAnimationResource.animation_destination_instance_id, outputEventAnimationResource.animation_card_instance_id >> 8 & 255U);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00009878 File Offset: 0x00007A78
	public void StartAnimationResource(uint resource_type, int resource_count, int anim_source_location, int anim_source_instance_id, int anim_destination_location, int anim_destination_instance_id, uint delay_count)
	{
		GameObject gameObject = null;
		if (this.m_AnimateResourcePrefabs != null && resource_type >= 0U && (ulong)resource_type < (ulong)((long)this.m_AnimateResourcePrefabs.Length))
		{
			gameObject = this.m_AnimateResourcePrefabs[(int)resource_type];
		}
		if (gameObject == null)
		{
			return;
		}
		float currentScale = 1f;
		float targetScale = 1f;
		GameObject gameObject2 = null;
		if (anim_source_location == 3)
		{
			if (this.m_CardManager != null)
			{
				GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(anim_source_instance_id, false);
				if (cardFromInstanceID != null)
				{
					gameObject2 = cardFromInstanceID.gameObject;
				}
			}
		}
		else if (anim_source_location == 6)
		{
			if (this.m_BuildingManager != null)
			{
			}
		}
		else
		{
			AnimationLocator animationLocator = base.GetAnimationLocator(anim_source_location, anim_source_instance_id);
			if (animationLocator != null)
			{
				gameObject2 = animationLocator.gameObject;
			}
		}
		if (gameObject2 == null)
		{
			Debug.LogError("   OutputEventAnimationResource source not found: " + anim_source_location.ToString() + ", " + anim_source_instance_id.ToString());
			return;
		}
		GameObject gameObject3 = null;
		if (anim_destination_location == 14)
		{
			if (this.m_AgricolaGame != null && this.m_AgricolaGame.GetLocalPlayerInstanceID() == anim_destination_instance_id)
			{
				if (this.m_LocalPlayerDisplay != null)
				{
					gameObject3 = this.m_LocalPlayerDisplay.GetAnimationNodeResources((int)resource_type);
				}
			}
			else if (this.m_UpperHudTokens != null)
			{
				gameObject3 = this.m_UpperHudTokens.FindPlayerTokenByInstanceID(anim_destination_instance_id);
			}
		}
		else
		{
			AnimationLocator animationLocator2 = base.GetAnimationLocator(anim_destination_location, anim_destination_instance_id);
			if (animationLocator2 != null)
			{
				gameObject3 = animationLocator2.gameObject;
			}
		}
		if (gameObject3 == null)
		{
			Debug.LogError("   OutputEventAnimationResource destination not found: " + anim_destination_location.ToString() + ", " + anim_destination_instance_id.ToString());
			return;
		}
		float num = this.AdjustAnimationPauseTime(0.1f, 0.3f);
		while (resource_count != 0)
		{
			float delayAtStart = delay_count * num;
			float pauseAtDestination = 0.3f;
			int resValue = 1;
			GameObject original = gameObject;
			resource_count--;
			GameObject gameObject4;
			if (this.m_PendingResourceAnimation != null && (long)this.m_PendingResourceAnimation.GetResourceType() == (long)((ulong)resource_type))
			{
				gameObject4 = this.m_PendingResourceAnimation.gameObject;
				if (this.m_PendingResourceAnimation.GetResourceValue() > 1)
				{
					resource_count -= this.m_PendingResourceAnimation.GetResourceValue() - 1;
				}
			}
			else
			{
				gameObject4 = UnityEngine.Object.Instantiate<GameObject>(original);
			}
			if (gameObject4 == null)
			{
				return;
			}
			if (this.m_PendingResourceAnimation != null)
			{
				this.m_PendingResourceAnimation = null;
			}
			else if (gameObject2 != null)
			{
				gameObject4.transform.position = gameObject2.transform.position;
				gameObject4.transform.rotation = gameObject2.transform.rotation;
			}
			AgricolaResource component = gameObject4.GetComponent<AgricolaResource>();
			if (component != null)
			{
				component.SetResourceValue((int)resource_type, resValue);
				gameObject4.SetActive(true);
				component.SetAnimationManager(this);
				component.transform.SetParent(base.GetDefaultAnimationLayer().transform);
				GameObject placeholder = component.GetPlaceholder(true);
				placeholder.transform.SetParent(gameObject3.transform, false);
				component.SetCurrentScale(currentScale);
				component.SetTargetScale(targetScale);
				if (base.StartAnimationToPlaceholder(component, placeholder, anim_destination_location, anim_destination_instance_id, delayAtStart, pauseAtDestination, true))
				{
					this.SetAnimationRatesResource(component);
					this.m_AnimateResourceList.Add(component);
					component.SetDestroyAfterAnimation();
					component.AddOnReachDestinationCallback(new AnimateObject.AnimationCallback(this.EndResourceAnimationCallback));
					if (anim_destination_location == 20)
					{
						component.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndResourceAnimationAudioCallback));
					}
					delay_count += 1U;
					continue;
				}
			}
			UnityEngine.Object.DestroyImmediate(gameObject4);
			return;
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00009BD0 File Offset: 0x00007DD0
	private void EndResourceAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		AgricolaResource component = animateObject.GetComponent<AgricolaResource>();
		if (component != null)
		{
			this.m_AnimateResourceList.Remove(component);
			AnimationEntry currentAnimationEntry = base.GetCurrentAnimationEntry(animateObject);
			if (currentAnimationEntry != null && currentAnimationEntry.m_DestinationLocatorIndex == 20)
			{
				component.GetResourceType();
				null != null;
			}
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00003022 File Offset: 0x00001222
	private void EndResourceAnimationAudioCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00009C20 File Offset: 0x00007E20
	public void DestroyAllAnimatingResources()
	{
		foreach (AgricolaResource agricolaResource in this.m_AnimateResourceList)
		{
			base.RemoveUpdateAnimateList(agricolaResource);
			agricolaResource.DestroyAnimation();
		}
		this.m_AnimateResourceList.Clear();
		foreach (GameObject obj in this.m_ResourceHitEffectList)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_ResourceHitEffectList.Clear();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00009CD0 File Offset: 0x00007ED0
	public void SubtractAnimatingResources(ref GamePlayerState player_state, int destination_locator_index, int destination_locator_instance_id)
	{
		if (this.m_AnimateResourceList == null)
		{
			return;
		}
		foreach (AgricolaResource agricolaResource in this.m_AnimateResourceList)
		{
			AnimationEntry currentAnimationEntry = base.GetCurrentAnimationEntry(agricolaResource);
			if (currentAnimationEntry != null && currentAnimationEntry.m_DestinationLocatorIndex == destination_locator_index && currentAnimationEntry.m_DestinationLocatorInstanceID == destination_locator_instance_id)
			{
				int resourceValue = agricolaResource.GetResourceValue();
				switch (agricolaResource.GetResourceType())
				{
				case 0:
					player_state.resourceCountFood -= resourceValue;
					break;
				case 1:
					player_state.resourceCountWood -= resourceValue;
					break;
				case 2:
					player_state.resourceCountClay -= resourceValue;
					break;
				case 3:
					player_state.resourceCountStone -= resourceValue;
					break;
				case 4:
					player_state.resourceCountReed -= resourceValue;
					break;
				case 5:
					player_state.resourceCountGrain -= resourceValue;
					break;
				case 6:
					player_state.resourceCountVeggie -= resourceValue;
					break;
				case 7:
					player_state.resourceCountSheep -= resourceValue;
					break;
				case 8:
					player_state.resourceCountWildBoar -= resourceValue;
					break;
				case 9:
					player_state.resourceCountCattle -= resourceValue;
					break;
				}
			}
		}
	}

	// Token: 0x040000F2 RID: 242
	private const uint k_cardEffectPlay = 1U;

	// Token: 0x040000F3 RID: 243
	private const uint k_cardEffectUseAbility = 2U;

	// Token: 0x040000F4 RID: 244
	private const uint k_cardEffectDraw = 3U;

	// Token: 0x040000F5 RID: 245
	private const uint k_cardEffectDraft = 4U;

	// Token: 0x040000F6 RID: 246
	private const uint k_cardEffectDiscard = 5U;

	// Token: 0x040000F7 RID: 247
	private const uint k_cardEffectPass = 6U;

	// Token: 0x040000F8 RID: 248
	private const uint k_cardEffectComplete = 7U;

	// Token: 0x040000F9 RID: 249
	public const int k_animLocationPlayerHand = 1;

	// Token: 0x040000FA RID: 250
	public const int k_animLocationPlayerDeck = 2;

	// Token: 0x040000FB RID: 251
	public const int k_animLocationPlayerDiscard = 3;

	// Token: 0x040000FC RID: 252
	public const int k_animLocationPlayerRevealedCards = 4;

	// Token: 0x040000FD RID: 253
	public const int k_animLocationPlayerInPlay = 5;

	// Token: 0x040000FE RID: 254
	public const int k_animLocationCardStack = 6;

	// Token: 0x040000FF RID: 255
	public const int k_animLocationTrash = 7;

	// Token: 0x04000100 RID: 256
	public const int k_animLocationPlayedCards = 8;

	// Token: 0x04000101 RID: 257
	public const int k_animLocationResolvingCards = 9;

	// Token: 0x04000102 RID: 258
	public const int k_animLocationPublicRevealedCards = 10;

	// Token: 0x04000103 RID: 259
	public const int k_animLocationPlayerAvatar = 11;

	// Token: 0x04000104 RID: 260
	public const int k_animLocationPlayerDraftPile = 12;

	// Token: 0x04000105 RID: 261
	public const int k_animLocationActionSpace = 13;

	// Token: 0x04000106 RID: 262
	public const int k_animLocationHudResource = 14;

	// Token: 0x04000107 RID: 263
	public const int k_animLocationImprovement = 15;

	// Token: 0x04000108 RID: 264
	public const int k_animLocationHudToken = 16;

	// Token: 0x04000109 RID: 265
	public const int k_animLocationFarmTile = 17;

	// Token: 0x0400010A RID: 266
	public const int k_animLocationUnavailableWorkers = 18;

	// Token: 0x0400010B RID: 267
	public const int k_animLocationHudBottomToken = 19;

	// Token: 0x0400010C RID: 268
	public const int k_animLocationPlayerDraftPileMini = 20;

	// Token: 0x0400010D RID: 269
	public const int k_animLocationFarmWorkers = 21;

	// Token: 0x0400010E RID: 270
	public const int k_animLocationMinorImprovement = 22;

	// Token: 0x0400010F RID: 271
	public const int k_animLocationDraftPlayerHand = 40;

	// Token: 0x04000110 RID: 272
	public const int k_animHintAnnounceDraft = 4;

	// Token: 0x04000111 RID: 273
	public const int k_animHintAnnounceTarget = 5;

	// Token: 0x04000112 RID: 274
	public const int k_animHintAnnounceTargetAll = 7;

	// Token: 0x04000113 RID: 275
	public const int k_animHintAnnounceWait = 8;

	// Token: 0x04000114 RID: 276
	public const int k_animExtraAnimToFarm = 1;

	// Token: 0x04000115 RID: 277
	public const int k_animExtraAnimToTown = 2;

	// Token: 0x04000116 RID: 278
	public const int k_animExtraChangeToFarmAfterAnnounce = 4;

	// Token: 0x04000117 RID: 279
	public const int k_animExtraChangeToTownAfterAnnounce = 8;

	// Token: 0x04000118 RID: 280
	public const int k_animExtraIgnoreDuringTutorial = 16;

	// Token: 0x04000119 RID: 281
	public const int k_animExtraForceSyncEnd = 32;

	// Token: 0x0400011A RID: 282
	public const int k_animExtraAllowSameInstanceID = 4096;

	// Token: 0x0400011B RID: 283
	public const int k_animExtraIgnoreForActivePlayer = 8192;

	// Token: 0x0400011C RID: 284
	public const int k_animExtraIgnoreIfNotActivePlayer = 16384;

	// Token: 0x0400011D RID: 285
	public const int k_animExtraUseStartDelay = 65536;

	// Token: 0x0400011E RID: 286
	public const int k_animExtraResetStartDelay = 131072;

	// Token: 0x0400011F RID: 287
	public const int k_animExtraHideDuringStartDelay = 262144;

	// Token: 0x04000120 RID: 288
	public const int k_animExtraUseCardbackIfNotActivePlayer = 524288;

	// Token: 0x04000121 RID: 289
	public const int k_animExtraHideDuringStartDelayIfNotActivePlayer = 1048576;

	// Token: 0x04000122 RID: 290
	public const int k_animExtraSuppressPurchaseResourceCheck = 2097152;

	// Token: 0x04000123 RID: 291
	public const int k_animExtraHoldForConfirmedMove = 4194304;

	// Token: 0x04000124 RID: 292
	public const int k_animExtraUseOffsetInstanceID = 8388608;

	// Token: 0x04000125 RID: 293
	public static Color[] m_CardEffectColor = new Color[]
	{
		new Color(255f, 255f, 255f, 255f),
		new Color(255f, 255f, 255f, 255f),
		new Color(255f, 255f, 255f, 255f),
		new Color(0f, 0f, 255f, 255f),
		new Color(0f, 255f, 0f, 255f),
		new Color(255f, 0f, 0f, 255f),
		new Color(0f, 0f, 255f, 255f),
		new Color(160f, 73f, 204f, 255f)
	};

	// Token: 0x04000126 RID: 294
	private const float k_AnimationMovementRateXY = 768f;

	// Token: 0x04000127 RID: 295
	private const float k_AnimationMovementRateZ = 288f;

	// Token: 0x04000128 RID: 296
	private const float k_AnimationRotationRate = 540f;

	// Token: 0x04000129 RID: 297
	private const float k_AnimationScaleRate = 0.2f;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	private AgricolaCardManager m_CardManager;

	// Token: 0x0400012B RID: 299
	[SerializeField]
	private AgricolaBuildingManager m_BuildingManager;

	// Token: 0x0400012C RID: 300
	[SerializeField]
	private AgricolaWorkerManager m_WorkerManager;

	// Token: 0x0400012D RID: 301
	[SerializeField]
	private AgricolaGame m_AgricolaGame;

	// Token: 0x0400012E RID: 302
	[SerializeField]
	private DraftInterface m_DraftInterface;

	// Token: 0x0400012F RID: 303
	[SerializeField]
	private AgricolaAnimationLocator m_LocatorAnnounceVisible;

	// Token: 0x04000130 RID: 304
	[SerializeField]
	private AgricolaAnimationLocator m_LocatorAnnounceHidden;

	// Token: 0x04000131 RID: 305
	[SerializeField]
	private GameObject[] m_AnimateResourcePrefabs;

	// Token: 0x04000132 RID: 306
	[SerializeField]
	private PlayerData m_LocalPlayerDisplay;

	// Token: 0x04000133 RID: 307
	[SerializeField]
	private PlayerDisplay_UpperHud m_UpperHudTokens;

	// Token: 0x04000134 RID: 308
	public AudioClip[] m_ResourceAudioClips;

	// Token: 0x04000135 RID: 309
	private AgricolaResource m_PendingResourceAnimation;

	// Token: 0x04000136 RID: 310
	private List<AgricolaResource> m_AnimateResourceList = new List<AgricolaResource>();

	// Token: 0x04000137 RID: 311
	private List<GameObject> m_ResourceHitEffectList = new List<GameObject>();
}
