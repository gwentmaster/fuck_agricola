using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class AgricolaAnimationLocator : AnimationLocator
{
	// Token: 0x060001A2 RID: 418 RVA: 0x00008E12 File Offset: 0x00007012
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00008E1C File Offset: 0x0000701C
	private void Init()
	{
		this.m_bInitialized = true;
		if (PlatformManager.s_instance != null)
		{
			switch (PlatformManager.s_instance.GetDeviceType())
			{
			case PlatformManager.DeviceType.DESKTOP:
				this.m_CardDisplayState = this.m_CardDisplayStateDesktop;
				this.m_CardDisplayScale = this.m_CardDisplayScaleDesktop;
				break;
			case PlatformManager.DeviceType.TABLET:
				this.m_CardDisplayState = this.m_CardDisplayStateTablet;
				this.m_CardDisplayScale = this.m_CardDisplayScaleTablet;
				break;
			case PlatformManager.DeviceType.PHONE:
				this.m_CardDisplayState = this.m_CardDisplayStatePhone;
				this.m_CardDisplayScale = this.m_CardDisplayScalePhone;
				break;
			}
		}
		AdjustCardScaleForAspectRatio component = base.GetComponent<AdjustCardScaleForAspectRatio>();
		if (component != null)
		{
			component.Start();
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00008EBE File Offset: 0x000070BE
	public ELocatorHiddenStatus HiddenToLocalPlayer()
	{
		if (this.m_bVisibleToLocalPlayer)
		{
			return ELocatorHiddenStatus.VISIBLE;
		}
		return this.m_HiddenStatus;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00008ED0 File Offset: 0x000070D0
	public bool InterceptHorizontalCardDrag()
	{
		return this.m_InterceptHorizontalCardDrag;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00008ED8 File Offset: 0x000070D8
	public bool InterceptVerticalCardDrag()
	{
		return this.m_InterceptVerticalCardDrag;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00008EE0 File Offset: 0x000070E0
	public ECardDisplayState GetCardDisplayState()
	{
		return this.m_CardDisplayState;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00008EE8 File Offset: 0x000070E8
	public float GetCardDisplayScale()
	{
		return this.m_CardDisplayScale;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00008EF0 File Offset: 0x000070F0
	public ECardDragType GetCardDragType()
	{
		return this.m_CardDragType;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00008EF8 File Offset: 0x000070F8
	public void SetInterceptHorizontalCardDrag(bool bIntercept)
	{
		this.m_InterceptHorizontalCardDrag = bIntercept;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00008F01 File Offset: 0x00007101
	public void SetInterceptVerticalCardDrag(bool bIntercept)
	{
		this.m_InterceptVerticalCardDrag = bIntercept;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00008F0A File Offset: 0x0000710A
	public void SetCardDisplayScale(float cardDisplayScale)
	{
		this.m_CardDisplayScale = cardDisplayScale;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00008F13 File Offset: 0x00007113
	public void SetCardDragType(ECardDragType cardDragType)
	{
		this.m_CardDragType = cardDragType;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00008F1C File Offset: 0x0000711C
	public override bool AnimateToLocator(AnimateObject animate_object, AnimationLocator sourceLocator, float delayAtStart = 0f, float pauseAtDestination = 0f)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		return base.AnimateToLocator(animate_object, sourceLocator, delayAtStart, pauseAtDestination);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00008F38 File Offset: 0x00007138
	public override void AnimateFromLocator(AnimateObject animate_object)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		if (animate_object == null)
		{
			return;
		}
		animate_object.SetCurrentScale(this.GetCardDisplayScale());
		animate_object.SetTargetScale(this.GetCardDisplayScale());
		AgricolaCard component = animate_object.GetComponent<AgricolaCard>();
		if (component != null)
		{
			switch (this.HiddenToLocalPlayer())
			{
			case ELocatorHiddenStatus.VISIBLE:
				component.DisplayCardBack(false, true);
				break;
			case ELocatorHiddenStatus.HIDDEN:
				component.DisplayCardBack(true, true);
				break;
			case ELocatorHiddenStatus.CUSTOM:
			{
				bool bCardBackEnabled = this.IsHidden(component);
				component.DisplayCardBack(bCardBackEnabled, true);
				break;
			}
			}
		}
		animate_object.transform.position = base.transform.position;
		animate_object.transform.rotation = base.transform.rotation;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00008FF0 File Offset: 0x000071F0
	public override void PlaceAnimateObject(AnimateObject animate_object, bool bSetPosition = false, bool bSetRotation = false, bool bIgnoreOverride = false)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		base.PlaceAnimateObject(animate_object, bSetPosition, bSetRotation, bIgnoreOverride);
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000900B File Offset: 0x0000720B
	public virtual bool IsHidden(AgricolaCard card)
	{
		return true;
	}

	// Token: 0x040000DE RID: 222
	[Header("VISIBILITY")]
	[SerializeField]
	private ELocatorHiddenStatus m_HiddenStatus;

	// Token: 0x040000DF RID: 223
	[SerializeField]
	private bool m_bVisibleToLocalPlayer;

	// Token: 0x040000E0 RID: 224
	[Space(10f)]
	[Header("DRAG")]
	[SerializeField]
	private ECardDragType m_CardDragType;

	// Token: 0x040000E1 RID: 225
	[SerializeField]
	private bool m_InterceptHorizontalCardDrag;

	// Token: 0x040000E2 RID: 226
	[SerializeField]
	private bool m_InterceptVerticalCardDrag;

	// Token: 0x040000E3 RID: 227
	[Space(10f)]
	[Header("DESKTOP")]
	[SerializeField]
	private ECardDisplayState m_CardDisplayStateDesktop = ECardDisplayState.DISPLAY_FULL;

	// Token: 0x040000E4 RID: 228
	[SerializeField]
	private float m_CardDisplayScaleDesktop = 1f;

	// Token: 0x040000E5 RID: 229
	[Space(10f)]
	[Header("TABLET")]
	[SerializeField]
	private ECardDisplayState m_CardDisplayStateTablet = ECardDisplayState.DISPLAY_FULL;

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	private float m_CardDisplayScaleTablet = 1f;

	// Token: 0x040000E7 RID: 231
	[Space(10f)]
	[Header("PHONE")]
	[SerializeField]
	private ECardDisplayState m_CardDisplayStatePhone = ECardDisplayState.DISPLAY_HALF;

	// Token: 0x040000E8 RID: 232
	[SerializeField]
	private float m_CardDisplayScalePhone = 1f;

	// Token: 0x040000E9 RID: 233
	private ECardDisplayState m_CardDisplayState = ECardDisplayState.DISPLAY_FULL;

	// Token: 0x040000EA RID: 234
	private float m_CardDisplayScale = 1f;

	// Token: 0x040000EB RID: 235
	private bool m_bInitialized;
}
