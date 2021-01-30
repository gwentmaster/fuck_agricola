using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200002C RID: 44
public class AgricolaAudioHandlerIngame : MonoBehaviour
{
	// Token: 0x060001C6 RID: 454 RVA: 0x00009F58 File Offset: 0x00008158
	private void Awake()
	{
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00009F90 File Offset: 0x00008190
	private void Start()
	{
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.AddOnBeginAnimationCallback(new AnimationManager.AnimationManagerCallback(this.HandleBeginAnimation));
			this.m_AnimationManager.AddOnEndAnimationCallback(new AnimationManager.AnimationManagerCallback(this.HandleEndAnimation));
		}
		if (this.m_DragManager != null)
		{
			this.m_DragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.HandleBeginDrag));
			this.m_DragManager.AddOnUpdateDragCallback(new DragManager.DragManagerCallback(this.HandleUpdateDrag));
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000A018 File Offset: 0x00008218
	private void OnDestroy()
	{
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.RemoveOnBeginAnimationCallback(new AnimationManager.AnimationManagerCallback(this.HandleBeginAnimation));
			this.m_AnimationManager.RemoveOnEndAnimationCallback(new AnimationManager.AnimationManagerCallback(this.HandleEndAnimation));
		}
		if (this.m_DragManager != null)
		{
			this.m_DragManager.RemoveOnBeginDragCallback(new DragManager.DragManagerCallback(this.HandleBeginDrag));
			this.m_DragManager.RemoveOnUpdateDragCallback(new DragManager.DragManagerCallback(this.HandleUpdateDrag));
		}
		this.m_hDataBuffer.Free();
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000A0A8 File Offset: 0x000082A8
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(20, new UnityAction<IntPtr, GameEventFeedback>(this.HandlePlayerSelectedOption));
		game_event_buffer.RegisterEventHandler(49, new UnityAction<IntPtr, GameEventFeedback>(this.HandleUseCardSoundEvent));
		game_event_buffer.RegisterEventHandler(50, new UnityAction<IntPtr, GameEventFeedback>(this.HandleUsedConvertOption));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000A0F5 File Offset: 0x000082F5
	public void RegisterMagnifyManager(AgricolaMagnifyManager magnify_manager)
	{
		if (magnify_manager != null)
		{
			magnify_manager.AddOnMagnifyCallback(new MagnifyManager.MagnifyCallback(this.HandleBeginMagnifyCard));
			magnify_manager.AddOnUnmagnifyCallback(new MagnifyManager.MagnifyCallback(this.HandleEndMagnifyCard));
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000A124 File Offset: 0x00008324
	public void PlayAudioActionSelect()
	{
		this.PlayRandomSoundFromList(this.m_ClipActionSelect);
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000A132 File Offset: 0x00008332
	public void PlayAudioTrayOpen()
	{
		this.PlayRandomSoundFromList(this.m_ClipTrayOpen);
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000A140 File Offset: 0x00008340
	public void PlayAudioTrayClose()
	{
		this.PlayRandomSoundFromList(this.m_ClipTrayClose);
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000A150 File Offset: 0x00008350
	private void PlayRandomSoundFromList(AudioClip[] audioClipList)
	{
		if (this.m_AudioSource == null || audioClipList == null || audioClipList.Length == 0)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, audioClipList.Length);
		this.m_AudioSource.PlayOneShot(audioClipList[num]);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000A18C File Offset: 0x0000838C
	private void PlayRandomSoundFromList(List<AudioClip> audioClipList)
	{
		if (this.m_AudioSource == null || audioClipList == null || audioClipList.Count <= 0)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, audioClipList.Count);
		this.m_AudioSource.PlayOneShot(audioClipList[index]);
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000A1D3 File Offset: 0x000083D3
	private void PlaySoundFromList(AudioClip[] audioClipList, int index)
	{
		if (this.m_AudioSource == null || audioClipList == null || audioClipList.Length == 0 || audioClipList.Length <= index)
		{
			return;
		}
		this.m_AudioSource.PlayOneShot(audioClipList[index]);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000A200 File Offset: 0x00008400
	private void HandleBeginAnimation(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		if (!(animateObject.GetComponent<AgricolaCard>() != null))
		{
			return;
		}
		switch (sourceLocatorIndex)
		{
		case 1:
		case 3:
		case 5:
		case 8:
			this.PlayRandomSoundFromList(this.m_ClipStartAnimCardSlide);
			return;
		case 2:
			this.PlayRandomSoundFromList(this.m_ClipStartAnimCardDeck);
			return;
		case 4:
		case 6:
		case 7:
			return;
		default:
			return;
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000A261 File Offset: 0x00008461
	private void HandleEndAnimation(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		if (animateObject.GetComponent<AgricolaResource>() != null)
		{
			return;
		}
		animateObject.GetComponent<AgricolaCard>() != null;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000A27F File Offset: 0x0000847F
	public void PlayCardSwipeSound()
	{
		this.PlayRandomSoundFromList(this.m_ClipCardSwipe);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000A28D File Offset: 0x0000848D
	public void PlayTownFarmSwapSound()
	{
		this.PlayRandomSoundFromList(this.m_ClipTownFarmButton);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000A29C File Offset: 0x0000849C
	public void HandleBeginDrag(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaCard>() != null && this.m_DragAudioSource != null && this.m_ClipUpdateCardDrag != null && this.m_ClipStartCardDrag.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.m_ClipStartCardDrag.Length);
			this.m_DragAudioSource.clip = this.m_ClipStartCardDrag[num];
			this.m_DragAudioSource.Play();
		}
		AgricolaAnimal component = dragObject.GetComponent<AgricolaAnimal>();
		if (component != null)
		{
			switch (component.GetAnimalType())
			{
			case EResourceType.SHEEP:
				this.PlayRandomSoundFromList(component.GetIsChild() ? this.m_ClipLambStartDrag : this.m_ClipSheepStartDrag);
				return;
			case EResourceType.WILDBOAR:
				this.PlayRandomSoundFromList(component.GetIsChild() ? this.m_ClipPigletStartDrag : this.m_ClipBoarStartDrag);
				return;
			case EResourceType.CATTLE:
				this.PlayRandomSoundFromList(component.GetIsChild() ? this.m_ClipCalfStartDrag : this.m_ClipCattleStartDrag);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000A388 File Offset: 0x00008588
	public void HandleUpdateDrag(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaCard>() != null && this.m_DragAudioSource != null && !this.m_DragAudioSource.isPlaying && this.m_ClipUpdateCardDrag != null && this.m_ClipUpdateCardDrag.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.m_ClipUpdateCardDrag.Length);
			this.m_DragAudioSource.clip = this.m_ClipUpdateCardDrag[num];
			this.m_DragAudioSource.Play();
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000A3FC File Offset: 0x000085FC
	private void HandleBeginMagnifyCard(CardObject magnifyCard)
	{
		this.PlayRandomSoundFromList(this.m_ClipActionActivateCard);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000A3FC File Offset: 0x000085FC
	private void HandleEndMagnifyCard(CardObject magnifyCard)
	{
		this.PlayRandomSoundFromList(this.m_ClipActionActivateCard);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000A40C File Offset: 0x0000860C
	private void HandleUsedConvertOption(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		UseConvertAbility useConvertAbility = (UseConvertAbility)Marshal.PtrToStructure(event_buffer, typeof(UseConvertAbility));
		if (useConvertAbility.playerIndex == AgricolaLib.GetLocalPlayerIndex() && useConvertAbility.resourceProducedType == 0)
		{
			int resourceCostType = useConvertAbility.resourceCostType;
			if (resourceCostType == 5)
			{
				this.PlayRandomSoundFromList(this.m_ClipUsedBakingConvert);
				return;
			}
			if (resourceCostType - 6 > 3)
			{
				return;
			}
			this.PlayRandomSoundFromList(this.m_ClipUsedCookingConvert);
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000A470 File Offset: 0x00008670
	private void HandlePlayerSelectedOption(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		PlayerSelectedOption playerSelectedOption = (PlayerSelectedOption)Marshal.PtrToStructure(event_buffer, typeof(PlayerSelectedOption));
		ushort selection_hint = playerSelectedOption.selection_hint;
		if (selection_hint <= 40991)
		{
			if (selection_hint != 40961)
			{
				switch (selection_hint)
				{
				case 40976:
					if (AgricolaLib.GetGamePlayerFarmState((int)playerSelectedOption.selection_player_index, this.m_bufPtr, 1024) != 0)
					{
						GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
						if (gamePlayerFarmState.houseType == 1)
						{
							this.PlayRandomSoundFromList(this.m_ClipBuildWoodRoom);
							return;
						}
						if (gamePlayerFarmState.houseType == 2)
						{
							this.PlayRandomSoundFromList(this.m_ClipBuildClayRoom);
							return;
						}
						if (gamePlayerFarmState.houseType == 3)
						{
							this.PlayRandomSoundFromList(this.m_ClipBuildStoneRoom);
							return;
						}
					}
					break;
				case 40977:
				case 40991:
					this.PlayRandomSoundFromList(this.m_ClipPlowField);
					return;
				case 40978:
				case 40979:
					goto IL_126;
				case 40980:
					this.PlayRandomSoundFromList(this.m_ClipBuildStable);
					return;
				case 40981:
				case 40985:
				case 40986:
				case 40987:
				case 40988:
				case 40989:
				case 40990:
					break;
				case 40982:
				case 40983:
					this.PlayRandomSoundFromList(this.m_ClipActionTakeCard);
					return;
				case 40984:
					this.PlayRandomSoundFromList(this.m_ClipBuildFence);
					return;
				default:
					return;
				}
			}
			else
			{
				this.PlayRandomSoundFromList(this.m_ClipActionEndTurn);
			}
			return;
		}
		if (selection_hint != 41019 && selection_hint != 41021)
		{
			return;
		}
		IL_126:
		this.PlayRandomSoundFromList(this.m_ClipSowField);
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000A5D8 File Offset: 0x000087D8
	private void HandleUseCardSoundEvent(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		UseCardSound useCardSound = (UseCardSound)Marshal.PtrToStructure(event_buffer, typeof(UseCardSound));
		if (useCardSound.soundName != string.Empty)
		{
			List<AudioClip> list = new List<AudioClip>();
			for (int i = 0; i < this.m_ClipUseCardSounds.Length; i++)
			{
				if (this.m_ClipUseCardSounds[i] != null && this.m_ClipUseCardSounds[i].name.Contains(useCardSound.soundName))
				{
					list.Add(this.m_ClipUseCardSounds[i]);
				}
			}
			this.PlayRandomSoundFromList(list);
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00003022 File Offset: 0x00001222
	private void HandleActionEvent(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
	}

	// Token: 0x04000138 RID: 312
	private const int k_maxDataSize = 1024;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	private AgricolaCardManager m_CardManager;

	// Token: 0x0400013B RID: 315
	[SerializeField]
	private DragManager m_DragManager;

	// Token: 0x0400013C RID: 316
	[SerializeField]
	private AudioSource m_AudioSource;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private AudioSource m_DragAudioSource;

	// Token: 0x0400013E RID: 318
	[SerializeField]
	private AudioClip[] m_ClipActionConfirm;

	// Token: 0x0400013F RID: 319
	[SerializeField]
	private AudioClip[] m_ClipActionBack;

	// Token: 0x04000140 RID: 320
	[SerializeField]
	private AudioClip[] m_ClipActionSelect;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private AudioClip[] m_ClipActionEndTurn;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private AudioClip[] m_ClipActionTakeCard;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private AudioClip[] m_ClipActionActivateCard;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private AudioClip[] m_ClipStartAnimCardDeck;

	// Token: 0x04000145 RID: 325
	[SerializeField]
	private AudioClip[] m_ClipStartAnimCardSlide;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private AudioClip[] m_ClipStartCardDrag;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	private AudioClip[] m_ClipUpdateCardDrag;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private AudioClip[] m_ClipCardSwipe;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	private AudioClip[] m_ClipTownFarmButton;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private AudioClip[] m_ClipUseCardSounds;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private AudioClip[] m_ClipSheepStartDrag;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private AudioClip[] m_ClipLambStartDrag;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private AudioClip[] m_ClipBoarStartDrag;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private AudioClip[] m_ClipPigletStartDrag;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private AudioClip[] m_ClipCattleStartDrag;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private AudioClip[] m_ClipCalfStartDrag;

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private AudioClip[] m_ClipBuildFence;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private AudioClip[] m_ClipBuildStable;

	// Token: 0x04000153 RID: 339
	[SerializeField]
	private AudioClip[] m_ClipBuildWoodRoom;

	// Token: 0x04000154 RID: 340
	[SerializeField]
	private AudioClip[] m_ClipBuildClayRoom;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private AudioClip[] m_ClipBuildStoneRoom;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private AudioClip[] m_ClipSowField;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private AudioClip[] m_ClipPlowField;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private AudioClip[] m_ClipUsedBakingConvert;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private AudioClip[] m_ClipUsedCookingConvert;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private AudioClip[] m_ClipTrayOpen;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private AudioClip[] m_ClipTrayClose;

	// Token: 0x0400015C RID: 348
	private byte[] m_dataBuffer;

	// Token: 0x0400015D RID: 349
	private GCHandle m_hDataBuffer;

	// Token: 0x0400015E RID: 350
	private IntPtr m_bufPtr;
}
