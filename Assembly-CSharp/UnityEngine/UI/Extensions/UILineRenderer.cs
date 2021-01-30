using System;
using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B0 RID: 432
	[AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
	[RequireComponent(typeof(RectTransform))]
	public class UILineRenderer : UIPrimitiveBase
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x00068B09 File Offset: 0x00066D09
		// (set) Token: 0x060010BE RID: 4286 RVA: 0x00068B11 File Offset: 0x00066D11
		public float LineThickness
		{
			get
			{
				return this.lineThickness;
			}
			set
			{
				this.lineThickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x00068B20 File Offset: 0x00066D20
		// (set) Token: 0x060010C0 RID: 4288 RVA: 0x00068B28 File Offset: 0x00066D28
		public bool RelativeSize
		{
			get
			{
				return this.relativeSize;
			}
			set
			{
				this.relativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x00068B37 File Offset: 0x00066D37
		// (set) Token: 0x060010C2 RID: 4290 RVA: 0x00068B3F File Offset: 0x00066D3F
		public bool LineList
		{
			get
			{
				return this.lineList;
			}
			set
			{
				this.lineList = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00068B4E File Offset: 0x00066D4E
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x00068B56 File Offset: 0x00066D56
		public bool LineCaps
		{
			get
			{
				return this.lineCaps;
			}
			set
			{
				this.lineCaps = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x00068B65 File Offset: 0x00066D65
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x00068B6D File Offset: 0x00066D6D
		public int BezierSegmentsPerCurve
		{
			get
			{
				return this.bezierSegmentsPerCurve;
			}
			set
			{
				this.bezierSegmentsPerCurve = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00068B76 File Offset: 0x00066D76
		// (set) Token: 0x060010C8 RID: 4296 RVA: 0x00068B7E File Offset: 0x00066D7E
		public Vector2[] Points
		{
			get
			{
				return this.m_points;
			}
			set
			{
				if (this.m_points == value)
				{
					return;
				}
				this.m_points = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00068B98 File Offset: 0x00066D98
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.m_points == null)
			{
				return;
			}
			this.GeneratedUVs();
			Vector2[] array = this.m_points;
			if (this.BezierMode != UILineRenderer.BezierType.None && this.BezierMode != UILineRenderer.BezierType.Catenary && this.m_points.Length > 3)
			{
				BezierPath bezierPath = new BezierPath();
				bezierPath.SetControlPoints(array);
				bezierPath.SegmentsPerCurve = this.bezierSegmentsPerCurve;
				UILineRenderer.BezierType bezierMode = this.BezierMode;
				List<Vector2> list;
				if (bezierMode != UILineRenderer.BezierType.Basic)
				{
					if (bezierMode != UILineRenderer.BezierType.Improved)
					{
						list = bezierPath.GetDrawingPoints2();
					}
					else
					{
						list = bezierPath.GetDrawingPoints1();
					}
				}
				else
				{
					list = bezierPath.GetDrawingPoints0();
				}
				array = list.ToArray();
			}
			if (this.BezierMode == UILineRenderer.BezierType.Catenary && this.m_points.Length == 2)
			{
				array = new CableCurve(array)
				{
					slack = base.Resoloution,
					steps = this.BezierSegmentsPerCurve
				}.Points();
			}
			if (base.ImproveResolution != ResolutionMode.None)
			{
				array = base.IncreaseResolution(array);
			}
			float num = (!this.relativeSize) ? 1f : base.rectTransform.rect.width;
			float num2 = (!this.relativeSize) ? 1f : base.rectTransform.rect.height;
			float num3 = -base.rectTransform.pivot.x * num;
			float num4 = -base.rectTransform.pivot.y * num2;
			vh.Clear();
			List<UIVertex[]> list2 = new List<UIVertex[]>();
			if (this.lineList)
			{
				for (int i = 1; i < array.Length; i += 2)
				{
					Vector2 vector = array[i - 1];
					Vector2 vector2 = array[i];
					vector = new Vector2(vector.x * num + num3, vector.y * num2 + num4);
					vector2 = new Vector2(vector2.x * num + num3, vector2.y * num2 + num4);
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(vector, vector2, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(vector, vector2, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(vector, vector2, UILineRenderer.SegmentType.End));
					}
				}
			}
			else
			{
				for (int j = 1; j < array.Length; j++)
				{
					Vector2 vector3 = array[j - 1];
					Vector2 vector4 = array[j];
					vector3 = new Vector2(vector3.x * num + num3, vector3.y * num2 + num4);
					vector4 = new Vector2(vector4.x * num + num3, vector4.y * num2 + num4);
					if (this.lineCaps && j == 1)
					{
						list2.Add(this.CreateLineCap(vector3, vector4, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(vector3, vector4, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps && j == array.Length - 1)
					{
						list2.Add(this.CreateLineCap(vector3, vector4, UILineRenderer.SegmentType.End));
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				if (!this.lineList && k < list2.Count - 1)
				{
					Vector3 v = list2[k][1].position - list2[k][2].position;
					Vector3 v2 = list2[k + 1][2].position - list2[k + 1][1].position;
					float num5 = Vector2.Angle(v, v2) * 0.017453292f;
					float num6 = Mathf.Sign(Vector3.Cross(v.normalized, v2.normalized).z);
					float num7 = this.lineThickness / (2f * Mathf.Tan(num5 / 2f));
					Vector3 position = list2[k][2].position - v.normalized * num7 * num6;
					Vector3 position2 = list2[k][3].position + v.normalized * num7 * num6;
					UILineRenderer.JoinType joinType = this.LineJoins;
					if (joinType == UILineRenderer.JoinType.Miter)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.2617994f)
						{
							list2[k][2].position = position;
							list2[k][3].position = position2;
							list2[k + 1][0].position = position2;
							list2[k + 1][1].position = position;
						}
						else
						{
							joinType = UILineRenderer.JoinType.Bevel;
						}
					}
					if (joinType == UILineRenderer.JoinType.Bevel)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.5235988f)
						{
							if (num6 < 0f)
							{
								list2[k][2].position = position;
								list2[k + 1][1].position = position;
							}
							else
							{
								list2[k][3].position = position2;
								list2[k + 1][0].position = position2;
							}
						}
						UIVertex[] verts = new UIVertex[]
						{
							list2[k][2],
							list2[k][3],
							list2[k + 1][0],
							list2[k + 1][1]
						};
						vh.AddUIVertexQuad(verts);
					}
				}
				vh.AddUIVertexQuad(list2[k]);
			}
			if (vh.currentVertCount > 64000)
			{
				Debug.LogError("Max Verticies size is 64000, current mesh vertcies count is [" + vh.currentVertCount + "] - Cannot Draw");
				vh.Clear();
				return;
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x000691A8 File Offset: 0x000673A8
		private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			if (type == UILineRenderer.SegmentType.Start)
			{
				Vector2 start2 = start - (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(start2, start, UILineRenderer.SegmentType.Start);
			}
			if (type == UILineRenderer.SegmentType.End)
			{
				Vector2 end2 = end + (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(end, end2, UILineRenderer.SegmentType.End);
			}
			Debug.LogError("Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
			return null;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00069234 File Offset: 0x00067434
		private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			Vector2 b = new Vector2(start.y - end.y, end.x - start.x).normalized * this.lineThickness / 2f;
			Vector2 vector = start - b;
			Vector2 vector2 = start + b;
			Vector2 vector3 = end + b;
			Vector2 vector4 = end - b;
			switch (type)
			{
			case UILineRenderer.SegmentType.Start:
				return base.SetVbo(new Vector2[]
				{
					vector,
					vector2,
					vector3,
					vector4
				}, UILineRenderer.startUvs);
			case UILineRenderer.SegmentType.End:
				return base.SetVbo(new Vector2[]
				{
					vector,
					vector2,
					vector3,
					vector4
				}, UILineRenderer.endUvs);
			case UILineRenderer.SegmentType.Full:
				return base.SetVbo(new Vector2[]
				{
					vector,
					vector2,
					vector3,
					vector4
				}, UILineRenderer.fullUvs);
			}
			return base.SetVbo(new Vector2[]
			{
				vector,
				vector2,
				vector3,
				vector4
			}, UILineRenderer.middleUvs);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00069388 File Offset: 0x00067588
		protected override void GeneratedUVs()
		{
			if (base.activeSprite != null)
			{
				Vector4 outerUV = DataUtility.GetOuterUV(base.activeSprite);
				Vector4 innerUV = DataUtility.GetInnerUV(base.activeSprite);
				UILineRenderer.UV_TOP_LEFT = new Vector2(outerUV.x, outerUV.y);
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(outerUV.x, outerUV.w);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(innerUV.x, innerUV.y);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.y);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(innerUV.x, innerUV.w);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.w);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(outerUV.z, outerUV.y);
				UILineRenderer.UV_BOTTOM_RIGHT = new Vector2(outerUV.z, outerUV.w);
			}
			else
			{
				UILineRenderer.UV_TOP_LEFT = Vector2.zero;
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(0f, 1f);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(1f, 0f);
				UILineRenderer.UV_BOTTOM_RIGHT = Vector2.one;
			}
			UILineRenderer.startUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_TOP_CENTER_LEFT
			};
			UILineRenderer.middleUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_TOP_CENTER_RIGHT
			};
			UILineRenderer.endUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
			UILineRenderer.fullUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000695EC File Offset: 0x000677EC
		protected override void ResolutionToNativeSize(float distance)
		{
			if (base.UseNativeSize)
			{
				this.m_Resolution = distance / (base.activeSprite.rect.width / base.pixelsPerUnit);
				this.lineThickness = base.activeSprite.rect.height / base.pixelsPerUnit;
			}
		}

		// Token: 0x04000F89 RID: 3977
		private const float MIN_MITER_JOIN = 0.2617994f;

		// Token: 0x04000F8A RID: 3978
		private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;

		// Token: 0x04000F8B RID: 3979
		private static Vector2 UV_TOP_LEFT;

		// Token: 0x04000F8C RID: 3980
		private static Vector2 UV_BOTTOM_LEFT;

		// Token: 0x04000F8D RID: 3981
		private static Vector2 UV_TOP_CENTER_LEFT;

		// Token: 0x04000F8E RID: 3982
		private static Vector2 UV_TOP_CENTER_RIGHT;

		// Token: 0x04000F8F RID: 3983
		private static Vector2 UV_BOTTOM_CENTER_LEFT;

		// Token: 0x04000F90 RID: 3984
		private static Vector2 UV_BOTTOM_CENTER_RIGHT;

		// Token: 0x04000F91 RID: 3985
		private static Vector2 UV_TOP_RIGHT;

		// Token: 0x04000F92 RID: 3986
		private static Vector2 UV_BOTTOM_RIGHT;

		// Token: 0x04000F93 RID: 3987
		private static Vector2[] startUvs;

		// Token: 0x04000F94 RID: 3988
		private static Vector2[] middleUvs;

		// Token: 0x04000F95 RID: 3989
		private static Vector2[] endUvs;

		// Token: 0x04000F96 RID: 3990
		private static Vector2[] fullUvs;

		// Token: 0x04000F97 RID: 3991
		[SerializeField]
		[Tooltip("Points to draw lines between\n Can be improved using the Resolution Option")]
		internal Vector2[] m_points;

		// Token: 0x04000F98 RID: 3992
		[SerializeField]
		[Tooltip("Thickness of the line")]
		internal float lineThickness = 2f;

		// Token: 0x04000F99 RID: 3993
		[SerializeField]
		[Tooltip("Use the relative bounds of the Rect Transform (0,0 -> 0,1) or screen space coordinates")]
		internal bool relativeSize;

		// Token: 0x04000F9A RID: 3994
		[SerializeField]
		[Tooltip("Do the points identify a single line or split pairs of lines")]
		internal bool lineList;

		// Token: 0x04000F9B RID: 3995
		[SerializeField]
		[Tooltip("Add end caps to each line\nMultiple caps when used with Line List")]
		internal bool lineCaps;

		// Token: 0x04000F9C RID: 3996
		[SerializeField]
		[Tooltip("Resolution of the Bezier curve, different to line Resolution")]
		internal int bezierSegmentsPerCurve = 10;

		// Token: 0x04000F9D RID: 3997
		[Tooltip("The type of Join used between lines, Square/Mitre or Curved/Bevel")]
		public UILineRenderer.JoinType LineJoins;

		// Token: 0x04000F9E RID: 3998
		[Tooltip("Bezier method to apply to line, see docs for options\nCan't be used in conjunction with Resolution as Bezier already changes the resolution")]
		public UILineRenderer.BezierType BezierMode;

		// Token: 0x04000F9F RID: 3999
		[HideInInspector]
		public bool drivenExternally;

		// Token: 0x0200085E RID: 2142
		private enum SegmentType
		{
			// Token: 0x04002ED8 RID: 11992
			Start,
			// Token: 0x04002ED9 RID: 11993
			Middle,
			// Token: 0x04002EDA RID: 11994
			End,
			// Token: 0x04002EDB RID: 11995
			Full
		}

		// Token: 0x0200085F RID: 2143
		public enum JoinType
		{
			// Token: 0x04002EDD RID: 11997
			Bevel,
			// Token: 0x04002EDE RID: 11998
			Miter
		}

		// Token: 0x02000860 RID: 2144
		public enum BezierType
		{
			// Token: 0x04002EE0 RID: 12000
			None,
			// Token: 0x04002EE1 RID: 12001
			Quick,
			// Token: 0x04002EE2 RID: 12002
			Basic,
			// Token: 0x04002EE3 RID: 12003
			Improved,
			// Token: 0x04002EE4 RID: 12004
			Catenary
		}
	}
}
