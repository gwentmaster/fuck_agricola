using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051D RID: 1309
	public class X9FieldID : Asn1Encodable
	{
		// Token: 0x06002FD9 RID: 12249 RVA: 0x000F5C33 File Offset: 0x000F3E33
		public X9FieldID(BigInteger primeP)
		{
			this.id = X9ObjectIdentifiers.PrimeField;
			this.parameters = new DerInteger(primeP);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000F5C52 File Offset: 0x000F3E52
		public X9FieldID(int m, int k1) : this(m, k1, 0, 0)
		{
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000F5C60 File Offset: 0x000F3E60
		public X9FieldID(int m, int k1, int k2, int k3)
		{
			this.id = X9ObjectIdentifiers.CharacteristicTwoField;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(m)
			});
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("inconsistent k values");
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					X9ObjectIdentifiers.TPBasis,
					new DerInteger(k1)
				});
			}
			else
			{
				if (k2 <= k1 || k3 <= k2)
				{
					throw new ArgumentException("inconsistent k values");
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					X9ObjectIdentifiers.PPBasis,
					new DerSequence(new Asn1Encodable[]
					{
						new DerInteger(k1),
						new DerInteger(k2),
						new DerInteger(k3)
					})
				});
			}
			this.parameters = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000F5D22 File Offset: 0x000F3F22
		private X9FieldID(Asn1Sequence seq)
		{
			this.id = DerObjectIdentifier.GetInstance(seq[0]);
			this.parameters = seq[1].ToAsn1Object();
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000F5D4E File Offset: 0x000F3F4E
		public static X9FieldID GetInstance(object obj)
		{
			if (obj is X9FieldID)
			{
				return (X9FieldID)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new X9FieldID(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000F5D6F File Offset: 0x000F3F6F
		public DerObjectIdentifier Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000F5D77 File Offset: 0x000F3F77
		public Asn1Object Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000F5D7F File Offset: 0x000F3F7F
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.id,
				this.parameters
			});
		}

		// Token: 0x04001E6E RID: 7790
		private readonly DerObjectIdentifier id;

		// Token: 0x04001E6F RID: 7791
		private readonly Asn1Object parameters;
	}
}
