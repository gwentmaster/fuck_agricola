using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CF RID: 463
	[AddComponentMenu("UI/Extensions/UI Line Connector")]
	[RequireComponent(typeof(UILineRenderer))]
	[ExecuteInEditMode]
	public class UILineConnector : MonoBehaviour
	{
		// Token: 0x060011AC RID: 4524 RVA: 0x0006D865 File Offset: 0x0006BA65
		private void Awake()
		{
			this.canvas = base.GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
			this.rt = base.GetComponent<RectTransform>();
			this.lr = base.GetComponent<UILineRenderer>();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0006D898 File Offset: 0x0006BA98
		private void Update()
		{
			if (this.transforms == null || this.transforms.Length < 1)
			{
				return;
			}
			if (this.previousPositions != null && this.previousPositions.Length == this.transforms.Length)
			{
				bool flag = false;
				for (int i = 0; i < this.transforms.Length; i++)
				{
					if (!flag && this.previousPositions[i] != this.transforms[i].anchoredPosition)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			Vector2 pivot = this.rt.pivot;
			Vector2 pivot2 = this.canvas.pivot;
			Vector3[] array = new Vector3[this.transforms.Length];
			Vector3[] array2 = new Vector3[this.transforms.Length];
			Vector2[] array3 = new Vector2[this.transforms.Length];
			for (int j = 0; j < this.transforms.Length; j++)
			{
				array[j] = this.transforms[j].TransformPoint(pivot);
			}
			for (int k = 0; k < this.transforms.Length; k++)
			{
				array2[k] = this.canvas.InverseTransformPoint(array[k]);
			}
			for (int l = 0; l < this.transforms.Length; l++)
			{
				array3[l] = new Vector2(array2[l].x, array2[l].y);
			}
			this.lr.Points = array3;
			this.lr.RelativeSize = false;
			this.lr.drivenExternally = true;
			this.previousPositions = new Vector2[this.transforms.Length];
			for (int m = 0; m < this.transforms.Length; m++)
			{
				this.previousPositions[m] = this.transforms[m].anchoredPosition;
			}
		}

		// Token: 0x04001024 RID: 4132
		public RectTransform[] transforms;

		// Token: 0x04001025 RID: 4133
		private Vector2[] previousPositions;

		// Token: 0x04001026 RID: 4134
		private RectTransform canvas;

		// Token: 0x04001027 RID: 4135
		private RectTransform rt;

		// Token: 0x04001028 RID: 4136
		private UILineRenderer lr;
	}
}
