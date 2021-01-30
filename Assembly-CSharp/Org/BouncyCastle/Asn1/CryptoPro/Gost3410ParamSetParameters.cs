using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000558 RID: 1368
	public class Gost3410ParamSetParameters : Asn1Encodable
	{
		// Token: 0x0600318A RID: 12682 RVA: 0x000FE411 File Offset: 0x000FC611
		public static Gost3410ParamSetParameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Gost3410ParamSetParameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000FE41F File Offset: 0x000FC61F
		public static Gost3410ParamSetParameters GetInstance(object obj)
		{
			if (obj == null || obj is Gost3410ParamSetParameters)
			{
				return (Gost3410ParamSetParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Gost3410ParamSetParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000FE45C File Offset: 0x000FC65C
		public Gost3410ParamSetParameters(int keySize, BigInteger p, BigInteger q, BigInteger a)
		{
			this.keySize = keySize;
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.a = new DerInteger(a);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000FE490 File Offset: 0x000FC690
		private Gost3410ParamSetParameters(Asn1Sequence seq)
		{
			if (seq.Count != 4)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.keySize = DerInteger.GetInstance(seq[0]).Value.IntValue;
			this.p = DerInteger.GetInstance(seq[1]);
			this.q = DerInteger.GetInstance(seq[2]);
			this.a = DerInteger.GetInstance(seq[3]);
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x000FE50E File Offset: 0x000FC70E
		public int KeySize
		{
			get
			{
				return this.keySize;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x000FE516 File Offset: 0x000FC716
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000FE523 File Offset: 0x000FC723
		public BigInteger Q
		{
			get
			{
				return this.q.PositiveValue;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06003191 RID: 12689 RVA: 0x000FE530 File Offset: 0x000FC730
		public BigInteger A
		{
			get
			{
				return this.a.PositiveValue;
			}
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000FE53D File Offset: 0x000FC73D
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(this.keySize),
				this.p,
				this.q,
				this.a
			});
		}

		// Token: 0x040020F0 RID: 8432
		private readonly int keySize;

		// Token: 0x040020F1 RID: 8433
		private readonly DerInteger p;

		// Token: 0x040020F2 RID: 8434
		private readonly DerInteger q;

		// Token: 0x040020F3 RID: 8435
		private readonly DerInteger a;
	}
}
