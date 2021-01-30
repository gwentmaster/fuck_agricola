using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000203 RID: 515
	[RequireComponent(typeof(UILineRenderer))]
	public class LineRendererOrbit : MonoBehaviour
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00071502 File Offset: 0x0006F702
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x0007150A File Offset: 0x0006F70A
		public float xAxis
		{
			get
			{
				return this._xAxis;
			}
			set
			{
				this._xAxis = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x00071519 File Offset: 0x0006F719
		// (set) Token: 0x060012BB RID: 4795 RVA: 0x00071521 File Offset: 0x0006F721
		public float yAxis
		{
			get
			{
				return this._yAxis;
			}
			set
			{
				this._yAxis = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x00071530 File Offset: 0x0006F730
		// (set) Token: 0x060012BD RID: 4797 RVA: 0x00071538 File Offset: 0x0006F738
		public int Steps
		{
			get
			{
				return this._steps;
			}
			set
			{
				this._steps = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00071547 File Offset: 0x0006F747
		private void Awake()
		{
			this.lr = base.GetComponent<UILineRenderer>();
			this.orbitGOrt = this.OrbitGO.GetComponent<RectTransform>();
			this.GenerateOrbit();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0007156C File Offset: 0x0006F76C
		private void Update()
		{
			this.orbitTime = ((this.orbitTime > (float)this._steps) ? (this.orbitTime = 0f) : (this.orbitTime + Time.deltaTime));
			this.orbitGOrt.localPosition = this.circle.Evaluate(this.orbitTime);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000715CC File Offset: 0x0006F7CC
		private void GenerateOrbit()
		{
			this.circle = new Circle(this._xAxis, this._yAxis, this._steps);
			List<Vector2> list = new List<Vector2>();
			for (int i = 0; i < this._steps; i++)
			{
				list.Add(this.circle.Evaluate((float)i));
			}
			list.Add(this.circle.Evaluate(0f));
			this.lr.Points = list.ToArray();
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00071647 File Offset: 0x0006F847
		private void OnValidate()
		{
			if (this.lr != null)
			{
				this.GenerateOrbit();
			}
		}

		// Token: 0x040010E9 RID: 4329
		private UILineRenderer lr;

		// Token: 0x040010EA RID: 4330
		private Circle circle;

		// Token: 0x040010EB RID: 4331
		public GameObject OrbitGO;

		// Token: 0x040010EC RID: 4332
		private RectTransform orbitGOrt;

		// Token: 0x040010ED RID: 4333
		private float orbitTime;

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private float _xAxis = 3f;

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private float _yAxis = 3f;

		// Token: 0x040010F0 RID: 4336
		[SerializeField]
		private int _steps = 10;
	}
}
