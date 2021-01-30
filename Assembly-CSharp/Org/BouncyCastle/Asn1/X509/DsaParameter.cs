using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000526 RID: 1318
	public class DsaParameter : Asn1Encodable
	{
		// Token: 0x06003016 RID: 12310 RVA: 0x000F699D File Offset: 0x000F4B9D
		public static DsaParameter GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DsaParameter.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000F69AB File Offset: 0x000F4BAB
		public static DsaParameter GetInstance(object obj)
		{
			if (obj == null || obj is DsaParameter)
			{
				return (DsaParameter)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DsaParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DsaParameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000F69E8 File Offset: 0x000F4BE8
		public DsaParameter(BigInteger p, BigInteger q, BigInteger g)
		{
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.g = new DerInteger(g);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000F6A14 File Offset: 0x000F4C14
		private DsaParameter(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.p = DerInteger.GetInstance(seq[0]);
			this.q = DerInteger.GetInstance(seq[1]);
			this.g = DerInteger.GetInstance(seq[2]);
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x000F6A86 File Offset: 0x000F4C86
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x000F6A93 File Offset: 0x000F4C93
		public BigInteger Q
		{
			get
			{
				return this.q.PositiveValue;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000F6AA0 File Offset: 0x000F4CA0
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000F6AAD File Offset: 0x000F4CAD
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.p,
				this.q,
				this.g
			});
		}

		// Token: 0x04001EC5 RID: 7877
		internal readonly DerInteger p;

		// Token: 0x04001EC6 RID: 7878
		internal readonly DerInteger q;

		// Token: 0x04001EC7 RID: 7879
		internal readonly DerInteger g;
	}
}
