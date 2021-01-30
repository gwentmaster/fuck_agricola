using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A0 RID: 416
	public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x000646C0 File Offset: 0x000628C0
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.pointerStartLocalPosition = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out this.pointerStartLocalPosition);
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.dragging = true;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00064714 File Offset: 0x00062914
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.dragging)
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			Vector2 vector = a - this.pointerStartLocalPosition;
			float num = ((this.directionOfRecognize == ScrollPositionController.ScrollDirection.Horizontal) ? (-vector.x) : vector.y) / this.GetViewportSize() * this.scrollSensitivity + this.dragStartScrollPosition;
			float num2 = this.CalculateOffset(num);
			num += num2;
			if (this.movementType == ScrollPositionController.MovementType.Elastic && num2 != 0f)
			{
				num -= this.RubberDelta(num2, this.scrollSensitivity);
			}
			this.UpdatePosition(num);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x000647BE File Offset: 0x000629BE
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.dragging = false;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x000647D0 File Offset: 0x000629D0
		private float GetViewportSize()
		{
			if (this.directionOfRecognize != ScrollPositionController.ScrollDirection.Horizontal)
			{
				return this.viewport.rect.size.y;
			}
			return this.viewport.rect.size.x;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00064817 File Offset: 0x00062A17
		private float CalculateOffset(float position)
		{
			if (this.movementType == ScrollPositionController.MovementType.Unrestricted)
			{
				return 0f;
			}
			if (position < 0f)
			{
				return -position;
			}
			if (position > (float)(this.dataCount - 1))
			{
				return (float)(this.dataCount - 1) - position;
			}
			return 0f;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0006484F File Offset: 0x00062A4F
		private void UpdatePosition(float position)
		{
			this.currentScrollPosition = position;
			if (this.OnUpdatePosition != null)
			{
				this.OnUpdatePosition.Invoke(this.currentScrollPosition);
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00064871 File Offset: 0x00062A71
		private float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0006489C File Offset: 0x00062A9C
		public void SetDataCount(int dataCont)
		{
			this.dataCount = dataCont;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000648A8 File Offset: 0x00062AA8
		private void Update()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float num = this.CalculateOffset(this.currentScrollPosition);
			if (this.autoScrolling)
			{
				float num2 = Mathf.Clamp01((Time.unscaledTime - this.autoScrollStartTime) / Mathf.Max(this.autoScrollDuration, float.Epsilon));
				float position = Mathf.Lerp(this.dragStartScrollPosition, this.autoScrollPosition, this.EaseInOutCubic(0f, 1f, num2));
				this.UpdatePosition(position);
				if (Mathf.Approximately(num2, 1f))
				{
					this.autoScrolling = false;
					if (this.OnItemSelected != null)
					{
						this.OnItemSelected.Invoke(Mathf.RoundToInt(this.GetLoopPosition(this.autoScrollPosition, this.dataCount)));
					}
				}
			}
			else if (!this.dragging && (num != 0f || this.velocity != 0f))
			{
				float num3 = this.currentScrollPosition;
				if (this.movementType == ScrollPositionController.MovementType.Elastic && num != 0f)
				{
					float num4 = this.velocity;
					num3 = Mathf.SmoothDamp(this.currentScrollPosition, this.currentScrollPosition + num, ref num4, this.elasticity, float.PositiveInfinity, unscaledDeltaTime);
					this.velocity = num4;
				}
				else if (this.inertia)
				{
					this.velocity *= Mathf.Pow(this.decelerationRate, unscaledDeltaTime);
					if (Mathf.Abs(this.velocity) < 0.001f)
					{
						this.velocity = 0f;
					}
					num3 += this.velocity * unscaledDeltaTime;
					if (this.snap.Enable && Mathf.Abs(this.velocity) < this.snap.VelocityThreshold)
					{
						this.ScrollTo(Mathf.RoundToInt(this.currentScrollPosition), this.snap.Duration);
					}
				}
				else
				{
					this.velocity = 0f;
				}
				if (this.velocity != 0f)
				{
					if (this.movementType == ScrollPositionController.MovementType.Clamped)
					{
						num = this.CalculateOffset(num3);
						num3 += num;
					}
					this.UpdatePosition(num3);
				}
			}
			if (!this.autoScrolling && this.dragging && this.inertia)
			{
				float b = (this.currentScrollPosition - this.prevScrollPosition) / unscaledDeltaTime;
				this.velocity = Mathf.Lerp(this.velocity, b, unscaledDeltaTime * 10f);
			}
			if (this.currentScrollPosition != this.prevScrollPosition)
			{
				this.prevScrollPosition = this.currentScrollPosition;
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00064B04 File Offset: 0x00062D04
		public void ScrollTo(int index, float duration)
		{
			this.velocity = 0f;
			this.autoScrolling = true;
			this.autoScrollDuration = duration;
			this.autoScrollStartTime = Time.unscaledTime;
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.autoScrollPosition = ((this.movementType == ScrollPositionController.MovementType.Unrestricted) ? this.CalculateClosestPosition(index) : ((float)index));
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00064B5C File Offset: 0x00062D5C
		private float CalculateClosestPosition(int index)
		{
			float num = this.GetLoopPosition((float)index, this.dataCount) - this.GetLoopPosition(this.currentScrollPosition, this.dataCount);
			if (Mathf.Abs(num) > (float)this.dataCount * 0.5f)
			{
				num = Mathf.Sign(-num) * ((float)this.dataCount - Mathf.Abs(num));
			}
			return num + this.currentScrollPosition;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00064BBF File Offset: 0x00062DBF
		private float GetLoopPosition(float position, int length)
		{
			if (position < 0f)
			{
				position = (float)(length - 1) + (position + 1f) % (float)length;
			}
			else if (position > (float)(length - 1))
			{
				position %= (float)length;
			}
			return position;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00064BEC File Offset: 0x00062DEC
		private float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end * 0.5f * value * value * value + start;
			}
			value -= 2f;
			return end * 0.5f * (value * value * value + 2f) + start;
		}

		// Token: 0x04000EF8 RID: 3832
		[SerializeField]
		private RectTransform viewport;

		// Token: 0x04000EF9 RID: 3833
		[SerializeField]
		private ScrollPositionController.ScrollDirection directionOfRecognize;

		// Token: 0x04000EFA RID: 3834
		[SerializeField]
		private ScrollPositionController.MovementType movementType = ScrollPositionController.MovementType.Elastic;

		// Token: 0x04000EFB RID: 3835
		[SerializeField]
		private float elasticity = 0.1f;

		// Token: 0x04000EFC RID: 3836
		[SerializeField]
		private float scrollSensitivity = 1f;

		// Token: 0x04000EFD RID: 3837
		[SerializeField]
		private bool inertia = true;

		// Token: 0x04000EFE RID: 3838
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private float decelerationRate = 0.03f;

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private ScrollPositionController.Snap snap = new ScrollPositionController.Snap
		{
			Enable = true,
			VelocityThreshold = 0.5f,
			Duration = 0.3f
		};

		// Token: 0x04000F00 RID: 3840
		[SerializeField]
		private int dataCount;

		// Token: 0x04000F01 RID: 3841
		[Tooltip("Event that fires when the position of an item changes")]
		public ScrollPositionController.UpdatePositionEvent OnUpdatePosition;

		// Token: 0x04000F02 RID: 3842
		[Tooltip("Event that fires when an item is selected/focused")]
		public ScrollPositionController.ItemSelectedEvent OnItemSelected;

		// Token: 0x04000F03 RID: 3843
		private Vector2 pointerStartLocalPosition;

		// Token: 0x04000F04 RID: 3844
		private float dragStartScrollPosition;

		// Token: 0x04000F05 RID: 3845
		private float currentScrollPosition;

		// Token: 0x04000F06 RID: 3846
		private bool dragging;

		// Token: 0x04000F07 RID: 3847
		private float velocity;

		// Token: 0x04000F08 RID: 3848
		private float prevScrollPosition;

		// Token: 0x04000F09 RID: 3849
		private bool autoScrolling;

		// Token: 0x04000F0A RID: 3850
		private float autoScrollDuration;

		// Token: 0x04000F0B RID: 3851
		private float autoScrollStartTime;

		// Token: 0x04000F0C RID: 3852
		private float autoScrollPosition;

		// Token: 0x02000852 RID: 2130
		[Serializable]
		public class UpdatePositionEvent : UnityEvent<float>
		{
		}

		// Token: 0x02000853 RID: 2131
		[Serializable]
		public class ItemSelectedEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000854 RID: 2132
		[Serializable]
		private struct Snap
		{
			// Token: 0x04002EC3 RID: 11971
			public bool Enable;

			// Token: 0x04002EC4 RID: 11972
			public float VelocityThreshold;

			// Token: 0x04002EC5 RID: 11973
			public float Duration;
		}

		// Token: 0x02000855 RID: 2133
		private enum ScrollDirection
		{
			// Token: 0x04002EC7 RID: 11975
			Vertical,
			// Token: 0x04002EC8 RID: 11976
			Horizontal
		}

		// Token: 0x02000856 RID: 2134
		private enum MovementType
		{
			// Token: 0x04002ECA RID: 11978
			Unrestricted,
			// Token: 0x04002ECB RID: 11979
			Elastic,
			// Token: 0x04002ECC RID: 11980
			Clamped
		}
	}
}
