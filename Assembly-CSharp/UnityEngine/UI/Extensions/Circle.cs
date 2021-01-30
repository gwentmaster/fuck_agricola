using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001BF RID: 447
	public class Circle
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0006C9C1 File Offset: 0x0006ABC1
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x0006C9C9 File Offset: 0x0006ABC9
		public float X
		{
			get
			{
				return this.xAxis;
			}
			set
			{
				this.xAxis = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0006C9D2 File Offset: 0x0006ABD2
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x0006C9DA File Offset: 0x0006ABDA
		public float Y
		{
			get
			{
				return this.yAxis;
			}
			set
			{
				this.yAxis = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0006C9E3 File Offset: 0x0006ABE3
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x0006C9EB File Offset: 0x0006ABEB
		public int Steps
		{
			get
			{
				return this.steps;
			}
			set
			{
				this.steps = value;
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0006C9F4 File Offset: 0x0006ABF4
		public Circle(float radius)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = 1;
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0006CA11 File Offset: 0x0006AC11
		public Circle(float radius, int steps)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = steps;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0006CA2E File Offset: 0x0006AC2E
		public Circle(float xAxis, float yAxis)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = 10;
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0006CA4C File Offset: 0x0006AC4C
		public Circle(float xAxis, float yAxis, int steps)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = steps;
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0006CA6C File Offset: 0x0006AC6C
		public Vector2 Evaluate(float t)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			float x = Mathf.Sin(f) * this.xAxis;
			float y = Mathf.Cos(f) * this.yAxis;
			return new Vector2(x, y);
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0006CAB4 File Offset: 0x0006ACB4
		public void Evaluate(float t, out Vector2 eval)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			eval.x = Mathf.Sin(f) * this.xAxis;
			eval.y = Mathf.Cos(f) * this.yAxis;
		}

		// Token: 0x04000FFA RID: 4090
		[SerializeField]
		private float xAxis;

		// Token: 0x04000FFB RID: 4091
		[SerializeField]
		private float yAxis;

		// Token: 0x04000FFC RID: 4092
		[SerializeField]
		private int steps;
	}
}
