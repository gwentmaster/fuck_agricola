using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x020002FB RID: 763
	public class GlvTypeBParameters
	{
		// Token: 0x06001AB6 RID: 6838 RVA: 0x0009A858 File Offset: 0x00098A58
		public GlvTypeBParameters(BigInteger beta, BigInteger lambda, BigInteger[] v1, BigInteger[] v2, BigInteger g1, BigInteger g2, int bits)
		{
			this.m_beta = beta;
			this.m_lambda = lambda;
			this.m_v1 = v1;
			this.m_v2 = v2;
			this.m_g1 = g1;
			this.m_g2 = g2;
			this.m_bits = bits;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0009A895 File Offset: 0x00098A95
		public virtual BigInteger Beta
		{
			get
			{
				return this.m_beta;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x0009A89D File Offset: 0x00098A9D
		public virtual BigInteger Lambda
		{
			get
			{
				return this.m_lambda;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0009A8A5 File Offset: 0x00098AA5
		public virtual BigInteger[] V1
		{
			get
			{
				return this.m_v1;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x0009A8AD File Offset: 0x00098AAD
		public virtual BigInteger[] V2
		{
			get
			{
				return this.m_v2;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x0009A8B5 File Offset: 0x00098AB5
		public virtual BigInteger G1
		{
			get
			{
				return this.m_g1;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x0009A8BD File Offset: 0x00098ABD
		public virtual BigInteger G2
		{
			get
			{
				return this.m_g2;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0009A8C5 File Offset: 0x00098AC5
		public virtual int Bits
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x040015A1 RID: 5537
		protected readonly BigInteger m_beta;

		// Token: 0x040015A2 RID: 5538
		protected readonly BigInteger m_lambda;

		// Token: 0x040015A3 RID: 5539
		protected readonly BigInteger[] m_v1;

		// Token: 0x040015A4 RID: 5540
		protected readonly BigInteger[] m_v2;

		// Token: 0x040015A5 RID: 5541
		protected readonly BigInteger m_g1;

		// Token: 0x040015A6 RID: 5542
		protected readonly BigInteger m_g2;

		// Token: 0x040015A7 RID: 5543
		protected readonly int m_bits;
	}
}
