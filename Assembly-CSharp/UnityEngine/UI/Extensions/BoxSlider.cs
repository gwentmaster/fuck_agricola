using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016E RID: 366
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/BoxSlider")]
	public class BoxSlider : Selectable, IDragHandler, IEventSystemHandler, IInitializePotentialDragHandler, ICanvasElement
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0005A278 File Offset: 0x00058478
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x0005A280 File Offset: 0x00058480
		public RectTransform HandleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (BoxSlider.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0005A29C File Offset: 0x0005849C
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x0005A2A4 File Offset: 0x000584A4
		public float MinValue
		{
			get
			{
				return this.m_MinValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0005A2D2 File Offset: 0x000584D2
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0005A2DA File Offset: 0x000584DA
		public float MaxValue
		{
			get
			{
				return this.m_MaxValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0005A308 File Offset: 0x00058508
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x0005A310 File Offset: 0x00058510
		public bool WholeNumbers
		{
			get
			{
				return this.m_WholeNumbers;
			}
			set
			{
				if (BoxSlider.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0005A33E File Offset: 0x0005853E
		// (set) Token: 0x06000E0C RID: 3596 RVA: 0x0005A35A File Offset: 0x0005855A
		public float ValueX
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueX);
				}
				return this.m_ValueX;
			}
			set
			{
				this.SetX(value);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0005A363 File Offset: 0x00058563
		// (set) Token: 0x06000E0E RID: 3598 RVA: 0x0005A395 File Offset: 0x00058595
		public float NormalizedValueX
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueX);
			}
			set
			{
				this.ValueX = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0005A3AF File Offset: 0x000585AF
		// (set) Token: 0x06000E10 RID: 3600 RVA: 0x0005A3CB File Offset: 0x000585CB
		public float ValueY
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueY);
				}
				return this.m_ValueY;
			}
			set
			{
				this.SetY(value);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x0005A3D4 File Offset: 0x000585D4
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x0005A406 File Offset: 0x00058606
		public float NormalizedValueY
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueY);
			}
			set
			{
				this.ValueY = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0005A420 File Offset: 0x00058620
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x0005A428 File Offset: 0x00058628
		public BoxSlider.BoxSliderEvent OnValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0005A431 File Offset: 0x00058631
		private float StepSize
		{
			get
			{
				if (!this.WholeNumbers)
				{
					return (this.MaxValue - this.MinValue) * 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0005A454 File Offset: 0x00058654
		protected BoxSlider()
		{
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00003022 File Offset: 0x00001222
		public void LayoutComplete()
		{
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00003022 File Offset: 0x00001222
		public void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0005A494 File Offset: 0x00058694
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0005A4E1 File Offset: 0x000586E1
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0005A501 File Offset: 0x00058701
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.SetX(this.m_ValueX, false);
			this.SetY(this.m_ValueY, false);
			this.UpdateVisuals();
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0005A52F File Offset: 0x0005872F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0005A544 File Offset: 0x00058744
		private void UpdateCachedReferences()
		{
			if (this.m_HandleRect)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
					return;
				}
			}
			else
			{
				this.m_HandleContainerRect = null;
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0005A5A0 File Offset: 0x000587A0
		private void SetX(float input)
		{
			this.SetX(input, true);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0005A5AC File Offset: 0x000587AC
		private void SetX(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueX == num)
			{
				return;
			}
			this.m_ValueX = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(num, this.ValueY);
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0005A607 File Offset: 0x00058807
		private void SetY(float input)
		{
			this.SetY(input, true);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0005A614 File Offset: 0x00058814
		private void SetY(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueY == num)
			{
				return;
			}
			this.m_ValueY = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(this.ValueX, num);
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0005A66F File Offset: 0x0005886F
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.UpdateVisuals();
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0005A680 File Offset: 0x00058880
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				zero[0] = (one[0] = this.NormalizedValueX);
				zero[1] = (one[1] = this.NormalizedValueY);
				this.m_HandleRect.anchorMin = zero;
				this.m_HandleRect.anchorMax = one;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0005A714 File Offset: 0x00058914
		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform handleContainerRect = this.m_HandleContainerRect;
			if (handleContainerRect != null && handleContainerRect.rect.size[0] > 0f)
			{
				Vector2 a;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(handleContainerRect, eventData.position, cam, out a))
				{
					return;
				}
				a -= handleContainerRect.rect.position;
				float normalizedValueX = Mathf.Clamp01((a - this.m_Offset)[0] / handleContainerRect.rect.size[0]);
				this.NormalizedValueX = normalizedValueX;
				float normalizedValueY = Mathf.Clamp01((a - this.m_Offset)[1] / handleContainerRect.rect.size[1]);
				this.NormalizedValueY = normalizedValueY;
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0005A7F4 File Offset: 0x000589F4
		private bool CanDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0005A814 File Offset: 0x00058A14
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera))
			{
				Vector2 offset;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out offset))
				{
					this.m_Offset = offset;
				}
				this.m_Offset.y = -this.m_Offset.y;
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0005A8AB File Offset: 0x00058AAB
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0005A8C4 File Offset: 0x00058AC4
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00057B84 File Offset: 0x00055D84
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000DB2 RID: 3506
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x04000DB3 RID: 3507
		[Space(6f)]
		[SerializeField]
		private float m_MinValue;

		// Token: 0x04000DB4 RID: 3508
		[SerializeField]
		private float m_MaxValue = 1f;

		// Token: 0x04000DB5 RID: 3509
		[SerializeField]
		private bool m_WholeNumbers;

		// Token: 0x04000DB6 RID: 3510
		[SerializeField]
		private float m_ValueX = 1f;

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private float m_ValueY = 1f;

		// Token: 0x04000DB8 RID: 3512
		[Space(6f)]
		[SerializeField]
		private BoxSlider.BoxSliderEvent m_OnValueChanged = new BoxSlider.BoxSliderEvent();

		// Token: 0x04000DB9 RID: 3513
		private Transform m_HandleTransform;

		// Token: 0x04000DBA RID: 3514
		private RectTransform m_HandleContainerRect;

		// Token: 0x04000DBB RID: 3515
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04000DBC RID: 3516
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x02000836 RID: 2102
		public enum Direction
		{
			// Token: 0x04002E94 RID: 11924
			LeftToRight,
			// Token: 0x04002E95 RID: 11925
			RightToLeft,
			// Token: 0x04002E96 RID: 11926
			BottomToTop,
			// Token: 0x04002E97 RID: 11927
			TopToBottom
		}

		// Token: 0x02000837 RID: 2103
		[Serializable]
		public class BoxSliderEvent : UnityEvent<float, float>
		{
		}

		// Token: 0x02000838 RID: 2104
		private enum Axis
		{
			// Token: 0x04002E99 RID: 11929
			Horizontal,
			// Token: 0x04002E9A RID: 11930
			Vertical
		}
	}
}
