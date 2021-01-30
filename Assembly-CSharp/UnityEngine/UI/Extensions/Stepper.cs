using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000186 RID: 390
	[AddComponentMenu("UI/Extensions/Stepper")]
	[RequireComponent(typeof(RectTransform))]
	public class Stepper : UIBehaviour
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0005EFA4 File Offset: 0x0005D1A4
		private float separatorWidth
		{
			get
			{
				if (this._separatorWidth == 0f && this.separator)
				{
					this._separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this._separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this._separatorWidth;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0005F016 File Offset: 0x0005D216
		public Selectable[] sides
		{
			get
			{
				if (this._sides == null || this._sides.Length == 0)
				{
					this._sides = this.GetSides();
				}
				return this._sides;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0005F03B File Offset: 0x0005D23B
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0005F043 File Offset: 0x0005D243
		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0005F04C File Offset: 0x0005D24C
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x0005F054 File Offset: 0x0005D254
		public int minimum
		{
			get
			{
				return this._minimum;
			}
			set
			{
				this._minimum = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0005F05D File Offset: 0x0005D25D
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0005F065 File Offset: 0x0005D265
		public int maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				this._maximum = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0005F06E File Offset: 0x0005D26E
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0005F076 File Offset: 0x0005D276
		public int step
		{
			get
			{
				return this._step;
			}
			set
			{
				this._step = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0005F07F File Offset: 0x0005D27F
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0005F087 File Offset: 0x0005D287
		public bool wrap
		{
			get
			{
				return this._wrap;
			}
			set
			{
				this._wrap = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0005F090 File Offset: 0x0005D290
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0005F098 File Offset: 0x0005D298
		public Graphic separator
		{
			get
			{
				return this._separator;
			}
			set
			{
				this._separator = value;
				this._separatorWidth = 0f;
				this.LayoutSides(this.sides);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0005F0B8 File Offset: 0x0005D2B8
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x0005F0C0 File Offset: 0x0005D2C0
		public Stepper.StepperValueChangedEvent onValueChanged
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

		// Token: 0x06000F1D RID: 3869 RVA: 0x0005F0C9 File Offset: 0x0005D2C9
		protected Stepper()
		{
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0005F0EC File Offset: 0x0005D2EC
		private Selectable[] GetSides()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length != 2)
			{
				throw new InvalidOperationException("A stepper must have two Button children");
			}
			for (int i = 0; i < 2; i++)
			{
				if (componentsInChildren[i].GetComponent<StepperSide>() == null)
				{
					componentsInChildren[i].gameObject.AddComponent<StepperSide>();
				}
			}
			if (!this.wrap)
			{
				this.DisableAtExtremes(componentsInChildren);
			}
			this.LayoutSides(componentsInChildren);
			return componentsInChildren;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0005F152 File Offset: 0x0005D352
		public void StepUp()
		{
			this.Step(this.step);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0005F160 File Offset: 0x0005D360
		public void StepDown()
		{
			this.Step(-this.step);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0005F170 File Offset: 0x0005D370
		private void Step(int amount)
		{
			this.value += amount;
			if (this.wrap)
			{
				if (this.value > this.maximum)
				{
					this.value = this.minimum;
				}
				if (this.value < this.minimum)
				{
					this.value = this.maximum;
				}
			}
			else
			{
				this.value = Math.Max(this.minimum, this.value);
				this.value = Math.Min(this.maximum, this.value);
				this.DisableAtExtremes(this.sides);
			}
			this._onValueChanged.Invoke(this.value);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0005F214 File Offset: 0x0005D414
		private void DisableAtExtremes(Selectable[] sides)
		{
			sides[0].interactable = (this.wrap || this.value > this.minimum);
			sides[1].interactable = (this.wrap || this.value < this.maximum);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0005F264 File Offset: 0x0005D464
		private void RecreateSprites(Selectable[] sides)
		{
			for (int i = 0; i < 2; i++)
			{
				if (!(sides[i].image == null))
				{
					Sprite sprite = sides[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i == 0)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						else
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						sides[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0005F338 File Offset: 0x0005D538
		public void LayoutSides(Selectable[] sides = null)
		{
			sides = (sides ?? this.sides);
			this.RecreateSprites(sides);
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / 2f - this.separatorWidth;
			for (int i = 0; i < 2; i++)
			{
				float inset = (i == 0) ? 0f : (num + this.separatorWidth);
				RectTransform component = sides[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
			if (this.separator)
			{
				Transform transform = base.gameObject.transform.Find("Separator");
				Graphic graphic = (transform != null) ? transform.GetComponent<Graphic>() : Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>();
				graphic.gameObject.name = "Separator";
				graphic.gameObject.SetActive(true);
				graphic.rectTransform.SetParent(base.transform, false);
				graphic.rectTransform.anchorMin = Vector2.zero;
				graphic.rectTransform.anchorMax = Vector2.zero;
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num, this.separatorWidth);
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
		}

		// Token: 0x04000E8A RID: 3722
		private Selectable[] _sides;

		// Token: 0x04000E8B RID: 3723
		[SerializeField]
		[Tooltip("The current step value of the control")]
		private int _value;

		// Token: 0x04000E8C RID: 3724
		[SerializeField]
		[Tooltip("The minimum step value allowed by the control. When reached it will disable the '-' button")]
		private int _minimum;

		// Token: 0x04000E8D RID: 3725
		[SerializeField]
		[Tooltip("The maximum step value allowed by the control. When reached it will disable the '+' button")]
		private int _maximum = 100;

		// Token: 0x04000E8E RID: 3726
		[SerializeField]
		[Tooltip("The step increment used to increment / decrement the step value")]
		private int _step = 1;

		// Token: 0x04000E8F RID: 3727
		[SerializeField]
		[Tooltip("Does the step value loop around from end to end")]
		private bool _wrap;

		// Token: 0x04000E90 RID: 3728
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic _separator;

		// Token: 0x04000E91 RID: 3729
		private float _separatorWidth;

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		private Stepper.StepperValueChangedEvent _onValueChanged = new Stepper.StepperValueChangedEvent();

		// Token: 0x0200084A RID: 2122
		[Serializable]
		public class StepperValueChangedEvent : UnityEvent<int>
		{
		}
	}
}
