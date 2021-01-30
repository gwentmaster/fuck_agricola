using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000436 RID: 1078
	public class Gost3410ValidationParameters
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x000C5946 File Offset: 0x000C3B46
		public Gost3410ValidationParameters(int x0, int c)
		{
			this.x0 = x0;
			this.c = c;
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000C595C File Offset: 0x000C3B5C
		public Gost3410ValidationParameters(long x0L, long cL)
		{
			this.x0L = x0L;
			this.cL = cL;
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x000C5972 File Offset: 0x000C3B72
		public int C
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x000C597A File Offset: 0x000C3B7A
		public int X0
		{
			get
			{
				return this.x0;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x000C5982 File Offset: 0x000C3B82
		public long CL
		{
			get
			{
				return this.cL;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x000C598A File Offset: 0x000C3B8A
		public long X0L
		{
			get
			{
				return this.x0L;
			}
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000C5994 File Offset: 0x000C3B94
		public override bool Equals(object obj)
		{
			Gost3410ValidationParameters gost3410ValidationParameters = obj as Gost3410ValidationParameters;
			return gost3410ValidationParameters != null && gost3410ValidationParameters.c == this.c && gost3410ValidationParameters.x0 == this.x0 && gost3410ValidationParameters.cL == this.cL && gost3410ValidationParameters.x0L == this.x0L;
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000C59E5 File Offset: 0x000C3BE5
		public override int GetHashCode()
		{
			return this.c.GetHashCode() ^ this.x0.GetHashCode() ^ this.cL.GetHashCode() ^ this.x0L.GetHashCode();
		}

		// Token: 0x04001A4E RID: 6734
		private int x0;

		// Token: 0x04001A4F RID: 6735
		private int c;

		// Token: 0x04001A50 RID: 6736
		private long x0L;

		// Token: 0x04001A51 RID: 6737
		private long cL;
	}
}
