using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000179 RID: 377
	[AddComponentMenu("UI/Extensions/Radial Slider")]
	[RequireComponent(typeof(Image))]
	public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0005C903 File Offset: 0x0005AB03
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x0005C916 File Offset: 0x0005AB16
		public float Angle
		{
			get
			{
				return this.RadialImage.fillAmount * 360f;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value / 360f);
					return;
				}
				this.UpdateRadialImage(value / 360f);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0005C93B File Offset: 0x0005AB3B
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x0005C948 File Offset: 0x0005AB48
		public float Value
		{
			get
			{
				return this.RadialImage.fillAmount;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value);
					return;
				}
				this.UpdateRadialImage(value);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0005C961 File Offset: 0x0005AB61
		// (set) Token: 0x06000E8E RID: 3726 RVA: 0x0005C969 File Offset: 0x0005AB69
		public Color EndColor
		{
			get
			{
				return this.m_endColor;
			}
			set
			{
				this.m_endColor = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0005C972 File Offset: 0x0005AB72
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x0005C97A File Offset: 0x0005AB7A
		public Color StartColor
		{
			get
			{
				return this.m_startColor;
			}
			set
			{
				this.m_startColor = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0005C983 File Offset: 0x0005AB83
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0005C98B File Offset: 0x0005AB8B
		public bool LerpToTarget
		{
			get
			{
				return this.m_lerpToTarget;
			}
			set
			{
				this.m_lerpToTarget = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0005C994 File Offset: 0x0005AB94
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x0005C99C File Offset: 0x0005AB9C
		public AnimationCurve LerpCurve
		{
			get
			{
				return this.m_lerpCurve;
			}
			set
			{
				this.m_lerpCurve = value;
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x0005C9D6 File Offset: 0x0005ABD6
		public bool LerpInProgress
		{
			get
			{
				return this.lerpInProgress;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0005C9E0 File Offset: 0x0005ABE0
		public Image RadialImage
		{
			get
			{
				if (this.m_image == null)
				{
					this.m_image = base.GetComponent<Image>();
					this.m_image.type = Image.Type.Filled;
					this.m_image.fillMethod = Image.FillMethod.Radial360;
					this.m_image.fillOrigin = 3;
					this.m_image.fillAmount = 0f;
				}
				return this.m_image;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0005CA41 File Offset: 0x0005AC41
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x0005CA49 File Offset: 0x0005AC49
		public RadialSlider.RadialSliderValueChangedEvent onValueChanged
		{
			get
			{
				return this._onValueChanged;
			}
			set
			{
				this._onValueChanged = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0005CA52 File Offset: 0x0005AC52
		// (set) Token: 0x06000E9A RID: 3738 RVA: 0x0005CA5A File Offset: 0x0005AC5A
		public RadialSlider.RadialSliderTextValueChangedEvent onTextValueChanged
		{
			get
			{
				return this._onTextValueChanged;
			}
			set
			{
				this._onTextValueChanged = value;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0005CA64 File Offset: 0x0005AC64
		private void Awake()
		{
			if (this.LerpCurve != null && this.LerpCurve.length > 0)
			{
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
				return;
			}
			this.m_lerpTime = 1f;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0005CABC File Offset: 0x0005ACBC
		private void Update()
		{
			if (this.isPointerDown)
			{
				this.m_targetAngle = this.GetAngleFromMousePoint();
				if (!this.lerpInProgress)
				{
					if (!this.LerpToTarget)
					{
						this.UpdateRadialImage(this.m_targetAngle);
						this.NotifyValueChanged();
					}
					else
					{
						if (this.isPointerReleased)
						{
							this.StartLerp(this.m_targetAngle);
						}
						this.isPointerReleased = false;
					}
				}
			}
			if (this.lerpInProgress && this.Value != this.m_lerpTargetAngle)
			{
				this.m_currentLerpTime += Time.deltaTime;
				float num = this.m_currentLerpTime / this.m_lerpTime;
				if (this.LerpCurve != null && this.LerpCurve.length > 0)
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, this.LerpCurve.Evaluate(num)));
				}
				else
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, num));
				}
			}
			if (this.m_currentLerpTime >= this.m_lerpTime || this.Value == this.m_lerpTargetAngle)
			{
				this.lerpInProgress = false;
				this.UpdateRadialImage(this.m_lerpTargetAngle);
				this.NotifyValueChanged();
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005CBDD File Offset: 0x0005ADDD
		private void StartLerp(float targetAngle)
		{
			if (!this.lerpInProgress)
			{
				this.m_startAngle = this.Value;
				this.m_lerpTargetAngle = targetAngle;
				this.m_currentLerpTime = 0f;
				this.lerpInProgress = true;
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0005CC0C File Offset: 0x0005AE0C
		private float GetAngleFromMousePoint()
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, this.m_eventCamera, out this.m_localPos);
			return (Mathf.Atan2(-this.m_localPos.y, this.m_localPos.x) * 180f / 3.1415927f + 180f) / 360f;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0005CC74 File Offset: 0x0005AE74
		private void UpdateRadialImage(float targetAngle)
		{
			this.RadialImage.fillAmount = targetAngle;
			this.RadialImage.color = Color.Lerp(this.m_startColor, this.m_endColor, targetAngle);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005CCA0 File Offset: 0x0005AEA0
		private void NotifyValueChanged()
		{
			this._onValueChanged.Invoke((int)(this.m_targetAngle * 360f));
			this._onTextValueChanged.Invoke(((int)(this.m_targetAngle * 360f)).ToString());
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0005CCE5 File Offset: 0x0005AEE5
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005CCF3 File Offset: 0x0005AEF3
		public void OnPointerDown(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
			this.isPointerDown = true;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005CD08 File Offset: 0x0005AF08
		public void OnPointerUp(PointerEventData eventData)
		{
			this.isPointerDown = false;
			this.isPointerReleased = true;
		}

		// Token: 0x04000E2E RID: 3630
		private bool isPointerDown;

		// Token: 0x04000E2F RID: 3631
		private bool isPointerReleased;

		// Token: 0x04000E30 RID: 3632
		private bool lerpInProgress;

		// Token: 0x04000E31 RID: 3633
		private Vector2 m_localPos;

		// Token: 0x04000E32 RID: 3634
		private float m_targetAngle;

		// Token: 0x04000E33 RID: 3635
		private float m_lerpTargetAngle;

		// Token: 0x04000E34 RID: 3636
		private float m_startAngle;

		// Token: 0x04000E35 RID: 3637
		private float m_currentLerpTime;

		// Token: 0x04000E36 RID: 3638
		private float m_lerpTime;

		// Token: 0x04000E37 RID: 3639
		private Camera m_eventCamera;

		// Token: 0x04000E38 RID: 3640
		private Image m_image;

		// Token: 0x04000E39 RID: 3641
		[SerializeField]
		[Tooltip("Radial Gradient Start Color")]
		private Color m_startColor = Color.green;

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		[Tooltip("Radial Gradient End Color")]
		private Color m_endColor = Color.red;

		// Token: 0x04000E3B RID: 3643
		[Tooltip("Move slider absolute or use Lerping?\nDragging only supported with absolute")]
		[SerializeField]
		private bool m_lerpToTarget;

		// Token: 0x04000E3C RID: 3644
		[Tooltip("Curve to apply to the Lerp\nMust be set to enable Lerp")]
		[SerializeField]
		private AnimationCurve m_lerpCurve;

		// Token: 0x04000E3D RID: 3645
		[Tooltip("Event fired when value of control changes, outputs an INT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderValueChangedEvent _onValueChanged = new RadialSlider.RadialSliderValueChangedEvent();

		// Token: 0x04000E3E RID: 3646
		[Tooltip("Event fired when value of control changes, outputs a TEXT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderTextValueChangedEvent _onTextValueChanged = new RadialSlider.RadialSliderTextValueChangedEvent();

		// Token: 0x02000843 RID: 2115
		[Serializable]
		public class RadialSliderValueChangedEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000844 RID: 2116
		[Serializable]
		public class RadialSliderTextValueChangedEvent : UnityEvent<string>
		{
		}
	}
}
