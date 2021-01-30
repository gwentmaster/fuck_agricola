using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000181 RID: 385
	[AddComponentMenu("UI/Extensions/Segmented Control")]
	[RequireComponent(typeof(RectTransform))]
	public class SegmentedControl : UIBehaviour
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x0005DE7C File Offset: 0x0005C07C
		protected float SeparatorWidth
		{
			get
			{
				if (this.m_separatorWidth == 0f && this.separator)
				{
					this.m_separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this.m_separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this.m_separatorWidth;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0005DEEE File Offset: 0x0005C0EE
		public Selectable[] segments
		{
			get
			{
				if (this.m_segments == null || this.m_segments.Length == 0)
				{
					this.m_segments = this.GetChildSegments();
				}
				return this.m_segments;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0005DF13 File Offset: 0x0005C113
		// (set) Token: 0x06000EC9 RID: 3785 RVA: 0x0005DF1B File Offset: 0x0005C11B
		public Graphic separator
		{
			get
			{
				return this.m_separator;
			}
			set
			{
				this.m_separator = value;
				this.m_separatorWidth = 0f;
				this.LayoutSegments();
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0005DF35 File Offset: 0x0005C135
		// (set) Token: 0x06000ECB RID: 3787 RVA: 0x0005DF3D File Offset: 0x0005C13D
		public bool allowSwitchingOff
		{
			get
			{
				return this.m_allowSwitchingOff;
			}
			set
			{
				this.m_allowSwitchingOff = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0005DF46 File Offset: 0x0005C146
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x0005DF5C File Offset: 0x0005C15C
		public int selectedSegmentIndex
		{
			get
			{
				return Array.IndexOf<Selectable>(this.segments, this.selectedSegment);
			}
			set
			{
				value = Math.Max(value, -1);
				value = Math.Min(value, this.segments.Length - 1);
				this.m_selectedSegmentIndex = value;
				if (value == -1)
				{
					if (this.selectedSegment)
					{
						this.selectedSegment.GetComponent<Segment>().selected = false;
						this.selectedSegment = null;
						return;
					}
				}
				else
				{
					this.segments[value].GetComponent<Segment>().selected = true;
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0005DFC8 File Offset: 0x0005C1C8
		// (set) Token: 0x06000ECF RID: 3791 RVA: 0x0005DFD0 File Offset: 0x0005C1D0
		public SegmentedControl.SegmentSelectedEvent onValueChanged
		{
			get
			{
				return this.m_onValueChanged;
			}
			set
			{
				this.m_onValueChanged = value;
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0005DFD9 File Offset: 0x0005C1D9
		protected SegmentedControl()
		{
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0005DFF3 File Offset: 0x0005C1F3
		protected override void Start()
		{
			base.Start();
			this.LayoutSegments();
			if (this.m_selectedSegmentIndex != -1)
			{
				this.selectedSegmentIndex = this.m_selectedSegmentIndex;
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0005E018 File Offset: 0x0005C218
		private Selectable[] GetChildSegments()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length < 2)
			{
				throw new InvalidOperationException("A segmented control must have at least two Button children");
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Segment segment = componentsInChildren[i].GetComponent<Segment>();
				if (segment == null)
				{
					segment = componentsInChildren[i].gameObject.AddComponent<Segment>();
				}
				segment.index = i;
			}
			return componentsInChildren;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0005E073 File Offset: 0x0005C273
		public void SetAllSegmentsOff()
		{
			this.selectedSegment = null;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0005E07C File Offset: 0x0005C27C
		private void RecreateSprites()
		{
			for (int i = 0; i < this.segments.Length; i++)
			{
				if (!(this.segments[i].image == null))
				{
					Sprite sprite = this.segments[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i > 0)
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						if (i < this.segments.Length - 1)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						this.segments[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0005E174 File Offset: 0x0005C374
		public void LayoutSegments()
		{
			this.RecreateSprites();
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / (float)this.segments.Length - this.SeparatorWidth * (float)(this.segments.Length - 1);
			for (int i = 0; i < this.segments.Length; i++)
			{
				float num2 = (num + this.SeparatorWidth) * (float)i;
				RectTransform component = this.segments[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				if (this.separator && i > 0)
				{
					Transform transform = base.gameObject.transform.Find("Separator " + i);
					Graphic graphic = (transform != null) ? transform.GetComponent<Graphic>() : Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>();
					graphic.gameObject.name = "Separator " + i;
					graphic.gameObject.SetActive(true);
					graphic.rectTransform.SetParent(base.transform, false);
					graphic.rectTransform.anchorMin = Vector2.zero;
					graphic.rectTransform.anchorMax = Vector2.zero;
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2 - this.SeparatorWidth, this.SeparatorWidth);
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				}
			}
		}

		// Token: 0x04000E71 RID: 3697
		private Selectable[] m_segments;

		// Token: 0x04000E72 RID: 3698
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic m_separator;

		// Token: 0x04000E73 RID: 3699
		private float m_separatorWidth;

		// Token: 0x04000E74 RID: 3700
		[SerializeField]
		[Tooltip("When True, it allows each button to be toggled on/off")]
		private bool m_allowSwitchingOff;

		// Token: 0x04000E75 RID: 3701
		[SerializeField]
		[Tooltip("The selected default for the control (zero indexed array)")]
		private int m_selectedSegmentIndex = -1;

		// Token: 0x04000E76 RID: 3702
		[SerializeField]
		[Tooltip("Event to fire once the selection has been changed")]
		private SegmentedControl.SegmentSelectedEvent m_onValueChanged = new SegmentedControl.SegmentSelectedEvent();

		// Token: 0x04000E77 RID: 3703
		protected internal Selectable selectedSegment;

		// Token: 0x04000E78 RID: 3704
		[SerializeField]
		public Color selectedColor;

		// Token: 0x02000848 RID: 2120
		[Serializable]
		public class SegmentSelectedEvent : UnityEvent<int>
		{
		}
	}
}
