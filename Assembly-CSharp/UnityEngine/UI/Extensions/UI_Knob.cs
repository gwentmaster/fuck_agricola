using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000189 RID: 393
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Extensions/UI_Knob")]
	public class UI_Knob : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x000602A1 File Offset: 0x0005E4A1
		public void OnPointerDown(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000602AA File Offset: 0x0005E4AA
		public void OnPointerUp(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000602A1 File Offset: 0x0005E4A1
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000602AA File Offset: 0x0005E4AA
		public void OnPointerExit(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000602B3 File Offset: 0x0005E4B3
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.SetInitPointerData(eventData);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000602BC File Offset: 0x0005E4BC
		private void SetInitPointerData(PointerEventData eventData)
		{
			this._initRotation = base.transform.rotation;
			this._currentVector = eventData.position - base.transform.position;
			this._initAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00060324 File Offset: 0x0005E524
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._canDrag)
			{
				this.SetInitPointerData(eventData);
				return;
			}
			this._currentVector = eventData.position - base.transform.position;
			this._currentAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
			Quaternion rhs = Quaternion.AngleAxis(this._currentAngle - this._initAngle, base.transform.forward);
			rhs.eulerAngles = new Vector3(0f, 0f, rhs.eulerAngles.z);
			Quaternion rotation = this._initRotation * rhs;
			if (this.direction == UI_Knob.Direction.CW)
			{
				this.knobValue = 1f - rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f - 360f * this.knobValue);
				}
			}
			else
			{
				this.knobValue = rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f * this.knobValue);
				}
			}
			if (Mathf.Abs(this.knobValue - this._previousValue) > 0.5f)
			{
				if (this.knobValue < 0.5f && this.loops > 1 && this._currentLoops < (float)(this.loops - 1))
				{
					this._currentLoops += 1f;
				}
				else if (this.knobValue > 0.5f && this._currentLoops >= 1f)
				{
					this._currentLoops -= 1f;
				}
				else
				{
					if (this.knobValue > 0.5f && this._currentLoops == 0f)
					{
						this.knobValue = 0f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
					if (this.knobValue < 0.5f && this._currentLoops == (float)(this.loops - 1))
					{
						this.knobValue = 1f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
				}
			}
			if (this.maxValue > 0f && this.knobValue + this._currentLoops > this.maxValue)
			{
				this.knobValue = this.maxValue;
				float z = (this.direction == UI_Knob.Direction.CW) ? (360f - 360f * this.maxValue) : (360f * this.maxValue);
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
				this.SetInitPointerData(eventData);
				this.InvokeEvents(this.knobValue);
				return;
			}
			base.transform.rotation = rotation;
			this.InvokeEvents(this.knobValue + this._currentLoops);
			this._previousValue = this.knobValue;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0006066C File Offset: 0x0005E86C
		private void SnapToPosition(ref float knobValue)
		{
			float num = 1f / (float)this.snapStepsPerLoop;
			float num2 = Mathf.Round(knobValue / num) * num;
			knobValue = num2;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00060696 File Offset: 0x0005E896
		private void InvokeEvents(float value)
		{
			if (this.clampOutput01)
			{
				value /= (float)this.loops;
			}
			this.OnValueChanged.Invoke(value);
		}

		// Token: 0x04000EA9 RID: 3753
		[Tooltip("Direction of rotation CW - clockwise, CCW - counterClockwise")]
		public UI_Knob.Direction direction;

		// Token: 0x04000EAA RID: 3754
		[HideInInspector]
		public float knobValue;

		// Token: 0x04000EAB RID: 3755
		[Tooltip("Max value of the knob, maximum RAW output value knob can reach, overrides snap step, IF set to 0 or higher than loops, max value will be set by loops")]
		public float maxValue;

		// Token: 0x04000EAC RID: 3756
		[Tooltip("How many rotations knob can do, if higher than max value, the latter will limit max value")]
		public int loops = 1;

		// Token: 0x04000EAD RID: 3757
		[Tooltip("Clamp output value between 0 and 1, usefull with loops > 1")]
		public bool clampOutput01;

		// Token: 0x04000EAE RID: 3758
		[Tooltip("snap to position?")]
		public bool snapToPosition;

		// Token: 0x04000EAF RID: 3759
		[Tooltip("Number of positions to snap")]
		public int snapStepsPerLoop = 10;

		// Token: 0x04000EB0 RID: 3760
		[Space(30f)]
		public KnobFloatValueEvent OnValueChanged;

		// Token: 0x04000EB1 RID: 3761
		private float _currentLoops;

		// Token: 0x04000EB2 RID: 3762
		private float _previousValue;

		// Token: 0x04000EB3 RID: 3763
		private float _initAngle;

		// Token: 0x04000EB4 RID: 3764
		private float _currentAngle;

		// Token: 0x04000EB5 RID: 3765
		private Vector2 _currentVector;

		// Token: 0x04000EB6 RID: 3766
		private Quaternion _initRotation;

		// Token: 0x04000EB7 RID: 3767
		private bool _canDrag;

		// Token: 0x0200084F RID: 2127
		public enum Direction
		{
			// Token: 0x04002EB8 RID: 11960
			CW,
			// Token: 0x04002EB9 RID: 11961
			CCW
		}
	}
}
