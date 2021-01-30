using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200018B RID: 395
	public class CUIBezierCurve : MonoBehaviour
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x000606D6 File Offset: 0x0005E8D6
		public Vector3[] ControlPoints
		{
			get
			{
				return this.controlPoints;
			}
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x000606DE File Offset: 0x0005E8DE
		public void Refresh()
		{
			if (this.OnRefresh != null)
			{
				this.OnRefresh();
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000606F4 File Offset: 0x0005E8F4
		public Vector3 GetPoint(float _time)
		{
			float num = 1f - _time;
			return num * num * num * this.controlPoints[0] + 3f * num * num * _time * this.controlPoints[1] + 3f * num * _time * _time * this.controlPoints[2] + _time * _time * _time * this.controlPoints[3];
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0006077C File Offset: 0x0005E97C
		public Vector3 GetTangent(float _time)
		{
			float num = 1f - _time;
			return 3f * num * num * (this.controlPoints[1] - this.controlPoints[0]) + 6f * num * _time * (this.controlPoints[2] - this.controlPoints[1]) + 3f * _time * _time * (this.controlPoints[3] - this.controlPoints[2]);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0006081C File Offset: 0x0005EA1C
		public void ReportSet()
		{
			if (this.controlPoints == null)
			{
				this.controlPoints = new Vector3[CUIBezierCurve.CubicBezierCurvePtNum];
				this.controlPoints[0] = new Vector3(0f, 0f, 0f);
				this.controlPoints[1] = new Vector3(0f, 1f, 0f);
				this.controlPoints[2] = new Vector3(1f, 1f, 0f);
				this.controlPoints[3] = new Vector3(1f, 0f, 0f);
			}
			int num = this.controlPoints.Length;
			int cubicBezierCurvePtNum = CUIBezierCurve.CubicBezierCurvePtNum;
		}

		// Token: 0x04000EB8 RID: 3768
		public static readonly int CubicBezierCurvePtNum = 4;

		// Token: 0x04000EB9 RID: 3769
		[SerializeField]
		protected Vector3[] controlPoints;

		// Token: 0x04000EBA RID: 3770
		public Action OnRefresh;
	}
}
