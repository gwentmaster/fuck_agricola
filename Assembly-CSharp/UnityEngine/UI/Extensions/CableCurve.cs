using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001BE RID: 446
	[Serializable]
	public class CableCurve
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x0006C4D5 File Offset: 0x0006A6D5
		// (set) Token: 0x06001142 RID: 4418 RVA: 0x0006C4DD File Offset: 0x0006A6DD
		public bool regenPoints
		{
			get
			{
				return this.m_regen;
			}
			set
			{
				this.m_regen = value;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0006C4E6 File Offset: 0x0006A6E6
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x0006C4EE File Offset: 0x0006A6EE
		public Vector2 start
		{
			get
			{
				return this.m_start;
			}
			set
			{
				if (value != this.m_start)
				{
					this.m_regen = true;
				}
				this.m_start = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x0006C50C File Offset: 0x0006A70C
		// (set) Token: 0x06001146 RID: 4422 RVA: 0x0006C514 File Offset: 0x0006A714
		public Vector2 end
		{
			get
			{
				return this.m_end;
			}
			set
			{
				if (value != this.m_end)
				{
					this.m_regen = true;
				}
				this.m_end = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x0006C532 File Offset: 0x0006A732
		// (set) Token: 0x06001148 RID: 4424 RVA: 0x0006C53A File Offset: 0x0006A73A
		public float slack
		{
			get
			{
				return this.m_slack;
			}
			set
			{
				if (value != this.m_slack)
				{
					this.m_regen = true;
				}
				this.m_slack = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x0006C55D File Offset: 0x0006A75D
		// (set) Token: 0x0600114A RID: 4426 RVA: 0x0006C565 File Offset: 0x0006A765
		public int steps
		{
			get
			{
				return this.m_steps;
			}
			set
			{
				if (value != this.m_steps)
				{
					this.m_regen = true;
				}
				this.m_steps = Mathf.Max(2, value);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x0006C584 File Offset: 0x0006A784
		public Vector2 midPoint
		{
			get
			{
				Vector2 result = Vector2.zero;
				if (this.m_steps == 2)
				{
					return (this.points[0] + this.points[1]) * 0.5f;
				}
				if (this.m_steps > 2)
				{
					int num = this.m_steps / 2;
					if (this.m_steps % 2 == 0)
					{
						result = (this.points[num] + this.points[num + 1]) * 0.5f;
					}
					else
					{
						result = this.points[num];
					}
				}
				return result;
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0006C620 File Offset: 0x0006A820
		public CableCurve()
		{
			this.points = CableCurve.emptyCurve;
			this.m_start = Vector2.up;
			this.m_end = Vector2.up + Vector2.right;
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0006C678 File Offset: 0x0006A878
		public CableCurve(Vector2[] inputPoints)
		{
			this.points = inputPoints;
			this.m_start = inputPoints[0];
			this.m_end = inputPoints[1];
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0006C6C8 File Offset: 0x0006A8C8
		public CableCurve(CableCurve v)
		{
			this.points = v.Points();
			this.m_start = v.start;
			this.m_end = v.end;
			this.m_slack = v.slack;
			this.m_steps = v.steps;
			this.m_regen = v.regenPoints;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0006C724 File Offset: 0x0006A924
		public Vector2[] Points()
		{
			if (!this.m_regen)
			{
				return this.points;
			}
			if (this.m_steps < 2)
			{
				return CableCurve.emptyCurve;
			}
			float num = Vector2.Distance(this.m_end, this.m_start);
			float num2 = Vector2.Distance(new Vector2(this.m_end.x, this.m_start.y), this.m_start);
			float num3 = num + Mathf.Max(0.0001f, this.m_slack);
			float num4 = 0f;
			float y = this.m_start.y;
			float num5 = num2;
			float y2 = this.end.y;
			if (num5 - num4 == 0f)
			{
				return CableCurve.emptyCurve;
			}
			float num6 = Mathf.Sqrt(Mathf.Pow(num3, 2f) - Mathf.Pow(y2 - y, 2f)) / (num5 - num4);
			int num7 = 30;
			int num8 = 0;
			int num9 = num7 * 10;
			bool flag = false;
			float num10 = 0f;
			float num11 = 100f;
			for (int i = 0; i < num7; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					num8++;
					float num12 = num10 + num11;
					float num13 = (float)Math.Sinh((double)num12) / num12;
					if (!float.IsInfinity(num13))
					{
						if (num13 == num6)
						{
							flag = true;
							num10 = num12;
							break;
						}
						if (num13 > num6)
						{
							break;
						}
						num10 = num12;
						if (num8 > num9)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
				num11 *= 0.1f;
			}
			float num14 = (num5 - num4) / 2f / num10;
			float num15 = (num4 + num5 - num14 * Mathf.Log((num3 + y2 - y) / (num3 - y2 + y))) / 2f;
			float num16 = (y2 + y - num3 * (float)Math.Cosh((double)num10) / (float)Math.Sinh((double)num10)) / 2f;
			this.points = new Vector2[this.m_steps];
			float num17 = (float)(this.m_steps - 1);
			for (int k = 0; k < this.m_steps; k++)
			{
				float num18 = (float)k / num17;
				Vector2 zero = Vector2.zero;
				zero.x = Mathf.Lerp(this.start.x, this.end.x, num18);
				zero.y = num14 * (float)Math.Cosh((double)((num18 * num2 - num15) / num14)) + num16;
				this.points[k] = zero;
			}
			this.m_regen = false;
			return this.points;
		}

		// Token: 0x04000FF3 RID: 4083
		[SerializeField]
		private Vector2 m_start;

		// Token: 0x04000FF4 RID: 4084
		[SerializeField]
		private Vector2 m_end;

		// Token: 0x04000FF5 RID: 4085
		[SerializeField]
		private float m_slack;

		// Token: 0x04000FF6 RID: 4086
		[SerializeField]
		private int m_steps;

		// Token: 0x04000FF7 RID: 4087
		[SerializeField]
		private bool m_regen;

		// Token: 0x04000FF8 RID: 4088
		private static Vector2[] emptyCurve = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 0f)
		};

		// Token: 0x04000FF9 RID: 4089
		[SerializeField]
		private Vector2[] points;
	}
}
