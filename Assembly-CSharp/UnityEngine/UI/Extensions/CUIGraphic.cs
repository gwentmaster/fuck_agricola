using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200018C RID: 396
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Graphic")]
	public class CUIGraphic : BaseMeshEffect
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x000608DD File Offset: 0x0005EADD
		public bool IsCurved
		{
			get
			{
				return this.isCurved;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x000608E5 File Offset: 0x0005EAE5
		public bool IsLockWithRatio
		{
			get
			{
				return this.isLockWithRatio;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x000608ED File Offset: 0x0005EAED
		public RectTransform RectTrans
		{
			get
			{
				return this.rectTrans;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x000608F5 File Offset: 0x0005EAF5
		public Graphic UIGraphic
		{
			get
			{
				return this.uiGraphic;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x000608FD File Offset: 0x0005EAFD
		public CUIGraphic RefCUIGraphic
		{
			get
			{
				return this.refCUIGraphic;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00060905 File Offset: 0x0005EB05
		public CUIBezierCurve[] RefCurves
		{
			get
			{
				return this.refCurves;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0006090D File Offset: 0x0005EB0D
		public Vector3_Array2D[] RefCurvesControlRatioPoints
		{
			get
			{
				return this.refCurvesControlRatioPoints;
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00060918 File Offset: 0x0005EB18
		protected void solveDoubleEquationWithVector(float _x_1, float _y_1, float _x_2, float _y_2, Vector3 _constant_1, Vector3 _contant_2, out Vector3 _x, out Vector3 _y)
		{
			if (Mathf.Abs(_x_1) > Mathf.Abs(_x_2))
			{
				Vector3 vector = _constant_1 * _x_2 / _x_1;
				float num = _y_1 * _x_2 / _x_1;
				_y = (_contant_2 - vector) / (_y_2 - num);
				if (_x_2 != 0f)
				{
					_x = (vector - num * _y) / _x_2;
					return;
				}
				_x = (_constant_1 - _y_1 * _y) / _x_1;
				return;
			}
			else
			{
				Vector3 vector = _contant_2 * _x_1 / _x_2;
				float num = _y_2 * _x_1 / _x_2;
				_x = (_constant_1 - vector) / (_y_1 - num);
				if (_x_1 != 0f)
				{
					_y = (vector - num * _x) / _x_1;
					return;
				}
				_y = (_contant_2 - _y_2 * _x) / _x_2;
				return;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00060A24 File Offset: 0x0005EC24
		protected UIVertex uiVertexLerp(UIVertex _a, UIVertex _b, float _time)
		{
			return new UIVertex
			{
				position = Vector3.Lerp(_a.position, _b.position, _time),
				normal = Vector3.Lerp(_a.normal, _b.normal, _time),
				tangent = Vector3.Lerp(_a.tangent, _b.tangent, _time),
				uv0 = Vector2.Lerp(_a.uv0, _b.uv0, _time),
				uv1 = Vector2.Lerp(_a.uv1, _b.uv1, _time),
				color = Color.Lerp(_a.color, _b.color, _time)
			};
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00060AF0 File Offset: 0x0005ECF0
		protected UIVertex uiVertexBerp(UIVertex v_bottomLeft, UIVertex v_topLeft, UIVertex v_topRight, UIVertex v_bottomRight, float _xTime, float _yTime)
		{
			UIVertex b = this.uiVertexLerp(v_topLeft, v_topRight, _xTime);
			UIVertex a = this.uiVertexLerp(v_bottomLeft, v_bottomRight, _xTime);
			return this.uiVertexLerp(a, b, _yTime);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00060B20 File Offset: 0x0005ED20
		protected void tessellateQuad(List<UIVertex> _quads, int _thisQuadIdx)
		{
			UIVertex uivertex = _quads[_thisQuadIdx];
			UIVertex uivertex2 = _quads[_thisQuadIdx + 1];
			UIVertex uivertex3 = _quads[_thisQuadIdx + 2];
			UIVertex v_bottomRight = _quads[_thisQuadIdx + 3];
			float num = 100f / this.resolution;
			int num2 = Mathf.Max(1, Mathf.CeilToInt((uivertex2.position - uivertex.position).magnitude / num));
			int num3 = Mathf.Max(1, Mathf.CeilToInt((uivertex3.position - uivertex2.position).magnitude / num));
			int num4 = 0;
			for (int i = 0; i < num3; i++)
			{
				int j = 0;
				while (j < num2)
				{
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					float xTime = (float)i / (float)num3;
					float yTime = (float)j / (float)num2;
					float xTime2 = (float)(i + 1) / (float)num3;
					float yTime2 = (float)(j + 1) / (float)num2;
					_quads[_quads.Count - 4] = this.uiVertexBerp(uivertex, uivertex2, uivertex3, v_bottomRight, xTime, yTime);
					_quads[_quads.Count - 3] = this.uiVertexBerp(uivertex, uivertex2, uivertex3, v_bottomRight, xTime, yTime2);
					_quads[_quads.Count - 2] = this.uiVertexBerp(uivertex, uivertex2, uivertex3, v_bottomRight, xTime2, yTime2);
					_quads[_quads.Count - 1] = this.uiVertexBerp(uivertex, uivertex2, uivertex3, v_bottomRight, xTime2, yTime);
					j++;
					num4++;
				}
			}
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00060CC4 File Offset: 0x0005EEC4
		protected void tessellateGraphic(List<UIVertex> _verts)
		{
			for (int i = 0; i < _verts.Count; i += 6)
			{
				this.reuse_quads.Add(_verts[i]);
				this.reuse_quads.Add(_verts[i + 1]);
				this.reuse_quads.Add(_verts[i + 2]);
				this.reuse_quads.Add(_verts[i + 4]);
			}
			int num = this.reuse_quads.Count / 4;
			for (int j = 0; j < num; j++)
			{
				this.tessellateQuad(this.reuse_quads, j * 4);
			}
			this.reuse_quads.RemoveRange(0, num * 4);
			_verts.Clear();
			for (int k = 0; k < this.reuse_quads.Count; k += 4)
			{
				_verts.Add(this.reuse_quads[k]);
				_verts.Add(this.reuse_quads[k + 1]);
				_verts.Add(this.reuse_quads[k + 2]);
				_verts.Add(this.reuse_quads[k + 2]);
				_verts.Add(this.reuse_quads[k + 3]);
				_verts.Add(this.reuse_quads[k]);
			}
			this.reuse_quads.Clear();
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00060E06 File Offset: 0x0005F006
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.isLockWithRatio)
			{
				this.UpdateCurveControlPointPositions();
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00060E18 File Offset: 0x0005F018
		public void Refresh()
		{
			this.ReportSet();
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				if (cuibezierCurve.ControlPoints != null)
				{
					Vector3[] controlPoints = cuibezierCurve.ControlPoints;
					for (int j = 0; j < CUIBezierCurve.CubicBezierCurvePtNum; j++)
					{
						Vector3 vector = controlPoints[j];
						vector.x = (vector.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
						vector.y = (vector.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
						this.refCurvesControlRatioPoints[i][j] = vector;
					}
				}
			}
			if (this.uiGraphic != null)
			{
				this.uiGraphic.enabled = false;
				this.uiGraphic.enabled = true;
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00060F4A File Offset: 0x0005F14A
		protected override void Awake()
		{
			base.Awake();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00060F58 File Offset: 0x0005F158
		protected override void OnEnable()
		{
			base.OnEnable();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00060F68 File Offset: 0x0005F168
		public virtual void ReportSet()
		{
			if (this.rectTrans == null)
			{
				this.rectTrans = base.GetComponent<RectTransform>();
			}
			if (this.refCurves == null)
			{
				this.refCurves = new CUIBezierCurve[2];
			}
			bool flag = true;
			for (int i = 0; i < 2; i++)
			{
				flag &= (this.refCurves[i] != null);
			}
			if (!(flag & this.refCurves.Length == 2))
			{
				CUIBezierCurve[] array = this.refCurves;
				for (int j = 0; j < 2; j++)
				{
					if (this.refCurves[j] == null)
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.SetParent(base.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localEulerAngles = Vector3.zero;
						if (j == 0)
						{
							gameObject.name = "BottomRefCurve";
						}
						else
						{
							gameObject.name = "TopRefCurve";
						}
						array[j] = gameObject.AddComponent<CUIBezierCurve>();
					}
					else
					{
						array[j] = this.refCurves[j];
					}
					array[j].ReportSet();
				}
				this.refCurves = array;
			}
			if (this.refCurvesControlRatioPoints == null)
			{
				this.refCurvesControlRatioPoints = new Vector3_Array2D[this.refCurves.Length];
				for (int k = 0; k < this.refCurves.Length; k++)
				{
					this.refCurvesControlRatioPoints[k].array = new Vector3[this.refCurves[k].ControlPoints.Length];
				}
				this.FixTextToRectTrans();
				this.Refresh();
			}
			for (int l = 0; l < 2; l++)
			{
				this.refCurves[l].OnRefresh = new Action(this.Refresh);
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0006110C File Offset: 0x0005F30C
		public void FixTextToRectTrans()
		{
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				for (int j = 0; j < CUIBezierCurve.CubicBezierCurvePtNum; j++)
				{
					if (cuibezierCurve.ControlPoints != null)
					{
						Vector3[] controlPoints = cuibezierCurve.ControlPoints;
						if (i == 0)
						{
							controlPoints[j].y = -this.rectTrans.rect.height * this.rectTrans.pivot.y;
						}
						else
						{
							controlPoints[j].y = this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
						}
						controlPoints[j].x = this.rectTrans.rect.width * (float)j / (float)(CUIBezierCurve.CubicBezierCurvePtNum - 1);
						Vector3[] array = controlPoints;
						int num = j;
						array[num].x = array[num].x - this.rectTrans.rect.width * this.rectTrans.pivot.x;
						controlPoints[j].z = 0f;
					}
				}
			}
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00061254 File Offset: 0x0005F454
		public void ReferenceCUIForBCurves()
		{
			Vector3 localPosition = this.rectTrans.localPosition;
			localPosition.x += -this.rectTrans.rect.width * this.rectTrans.pivot.x + this.refCUIGraphic.rectTrans.rect.width * this.refCUIGraphic.rectTrans.pivot.x;
			localPosition.y += -this.rectTrans.rect.height * this.rectTrans.pivot.y + this.refCUIGraphic.rectTrans.rect.height * this.refCUIGraphic.rectTrans.pivot.y;
			Vector3 vector = new Vector3(localPosition.x / this.refCUIGraphic.RectTrans.rect.width, localPosition.y / this.refCUIGraphic.RectTrans.rect.height, localPosition.z);
			Vector3 vector2 = new Vector3((localPosition.x + this.rectTrans.rect.width) / this.refCUIGraphic.RectTrans.rect.width, (localPosition.y + this.rectTrans.rect.height) / this.refCUIGraphic.RectTrans.rect.height, localPosition.z);
			this.refCurves[0].ControlPoints[0] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, vector.y) - this.rectTrans.localPosition;
			this.refCurves[0].ControlPoints[3] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, vector.y) - this.rectTrans.localPosition;
			this.refCurves[1].ControlPoints[0] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, vector2.y) - this.rectTrans.localPosition;
			this.refCurves[1].ControlPoints[3] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, vector2.y) - this.rectTrans.localPosition;
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				float yTime = (i == 0) ? vector.y : vector2.y;
				Vector3 bcurveSandwichSpacePoint = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, yTime);
				Vector3 bcurveSandwichSpacePoint2 = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, yTime);
				float num = 0.25f;
				float num2 = 0.75f;
				Vector3 bcurveSandwichSpacePoint3 = this.refCUIGraphic.GetBCurveSandwichSpacePoint((vector.x * 0.75f + vector2.x * 0.25f) / 1f, yTime);
				Vector3 bcurveSandwichSpacePoint4 = this.refCUIGraphic.GetBCurveSandwichSpacePoint((vector.x * 0.25f + vector2.x * 0.75f) / 1f, yTime);
				float x_ = 3f * num2 * num2 * num;
				float y_ = 3f * num2 * num * num;
				float x_2 = 3f * num * num * num2;
				float y_2 = 3f * num * num2 * num2;
				Vector3 constant_ = bcurveSandwichSpacePoint3 - Mathf.Pow(num2, 3f) * bcurveSandwichSpacePoint - Mathf.Pow(num, 3f) * bcurveSandwichSpacePoint2;
				Vector3 contant_ = bcurveSandwichSpacePoint4 - Mathf.Pow(num, 3f) * bcurveSandwichSpacePoint - Mathf.Pow(num2, 3f) * bcurveSandwichSpacePoint2;
				Vector3 a;
				Vector3 a2;
				this.solveDoubleEquationWithVector(x_, y_, x_2, y_2, constant_, contant_, out a, out a2);
				cuibezierCurve.ControlPoints[1] = a - this.rectTrans.localPosition;
				cuibezierCurve.ControlPoints[2] = a2 - this.rectTrans.localPosition;
			}
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00061694 File Offset: 0x0005F894
		public override void ModifyMesh(Mesh _mesh)
		{
			if (!this.IsActive())
			{
				return;
			}
			using (VertexHelper vertexHelper = new VertexHelper(_mesh))
			{
				this.ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(_mesh);
			}
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x000616DC File Offset: 0x0005F8DC
		public override void ModifyMesh(VertexHelper _vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			_vh.GetUIVertexStream(list);
			this.modifyVertices(list);
			_vh.Clear();
			_vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00061714 File Offset: 0x0005F914
		protected virtual void modifyVertices(List<UIVertex> _verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.tessellateGraphic(_verts);
			if (!this.isCurved)
			{
				return;
			}
			for (int i = 0; i < _verts.Count; i++)
			{
				UIVertex uivertex = _verts[i];
				float xTime = (uivertex.position.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
				float yTime = (uivertex.position.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
				Vector3 bcurveSandwichSpacePoint = this.GetBCurveSandwichSpacePoint(xTime, yTime);
				uivertex.position.x = bcurveSandwichSpacePoint.x;
				uivertex.position.y = bcurveSandwichSpacePoint.y;
				uivertex.position.z = bcurveSandwichSpacePoint.z;
				_verts[i] = uivertex;
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00061834 File Offset: 0x0005FA34
		public void UpdateCurveControlPointPositions()
		{
			this.ReportSet();
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				for (int j = 0; j < this.refCurves[i].ControlPoints.Length; j++)
				{
					Vector3 vector = this.refCurvesControlRatioPoints[i][j];
					vector.x = vector.x * this.rectTrans.rect.width - this.rectTrans.rect.width * this.rectTrans.pivot.x;
					vector.y = vector.y * this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
					cuibezierCurve.ControlPoints[j] = vector;
				}
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00061939 File Offset: 0x0005FB39
		public Vector3 GetBCurveSandwichSpacePoint(float _xTime, float _yTime)
		{
			return this.refCurves[0].GetPoint(_xTime) * (1f - _yTime) + this.refCurves[1].GetPoint(_xTime) * _yTime;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0006196E File Offset: 0x0005FB6E
		public Vector3 GetBCurveSandwichSpaceTangent(float _xTime, float _yTime)
		{
			return this.refCurves[0].GetTangent(_xTime) * (1f - _yTime) + this.refCurves[1].GetTangent(_xTime) * _yTime;
		}

		// Token: 0x04000EBB RID: 3771
		public static readonly int bottomCurveIdx = 0;

		// Token: 0x04000EBC RID: 3772
		public static readonly int topCurveIdx = 1;

		// Token: 0x04000EBD RID: 3773
		[Tooltip("Set true to make the curve/morph to work. Set false to quickly see the original UI.")]
		[SerializeField]
		protected bool isCurved = true;

		// Token: 0x04000EBE RID: 3774
		[Tooltip("Set true to dynamically change the curve according to the dynamic change of the UI layout")]
		[SerializeField]
		protected bool isLockWithRatio = true;

		// Token: 0x04000EBF RID: 3775
		[Tooltip("Pick a higher resolution to improve the quality of the curved graphic.")]
		[SerializeField]
		[Range(0.01f, 30f)]
		protected float resolution = 5f;

		// Token: 0x04000EC0 RID: 3776
		protected RectTransform rectTrans;

		// Token: 0x04000EC1 RID: 3777
		[Tooltip("Put in the Graphic you want to curve/morph here.")]
		[SerializeField]
		protected Graphic uiGraphic;

		// Token: 0x04000EC2 RID: 3778
		[Tooltip("Put in the reference Graphic that will be used to tune the bezier curves. Think button image and text.")]
		[SerializeField]
		protected CUIGraphic refCUIGraphic;

		// Token: 0x04000EC3 RID: 3779
		[Tooltip("Do not touch this unless you are sure what you are doing. The curves are (re)generated automatically.")]
		[SerializeField]
		protected CUIBezierCurve[] refCurves;

		// Token: 0x04000EC4 RID: 3780
		[HideInInspector]
		[SerializeField]
		protected Vector3_Array2D[] refCurvesControlRatioPoints;

		// Token: 0x04000EC5 RID: 3781
		protected List<UIVertex> reuse_quads = new List<UIVertex>();
	}
}
