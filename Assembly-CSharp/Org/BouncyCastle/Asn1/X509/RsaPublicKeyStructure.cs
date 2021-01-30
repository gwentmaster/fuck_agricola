using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052E RID: 1326
	public class RsaPublicKeyStructure : Asn1Encodable
	{
		// Token: 0x06003062 RID: 12386 RVA: 0x000F7C67 File Offset: 0x000F5E67
		public static RsaPublicKeyStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return RsaPublicKeyStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000F7C75 File Offset: 0x000F5E75
		public static RsaPublicKeyStructure GetInstance(object obj)
		{
			if (obj == null || obj is RsaPublicKeyStructure)
			{
				return (RsaPublicKeyStructure)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RsaPublicKeyStructure((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid RsaPublicKeyStructure: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000F7CB4 File Offset: 0x000F5EB4
		public RsaPublicKeyStructure(BigInteger modulus, BigInteger publicExponent)
		{
			if (modulus == null)
			{
				throw new ArgumentNullException("modulus");
			}
			if (publicExponent == null)
			{
				throw new ArgumentNullException("publicExponent");
			}
			if (modulus.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA modulus", "modulus");
			}
			if (publicExponent.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA public exponent", "publicExponent");
			}
			this.modulus = modulus;
			this.publicExponent = publicExponent;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000F7D24 File Offset: 0x000F5F24
		private RsaPublicKeyStructure(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.modulus = DerInteger.GetInstance(seq[0]).PositiveValue;
			this.publicExponent = DerInteger.GetInstance(seq[1]).PositiveValue;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06003066 RID: 12390 RVA: 0x000F7D89 File Offset: 0x000F5F89
		public BigInteger Modulus
		{
			get
			{
				return this.modulus;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x000F7D91 File Offset: 0x000F5F91
		public BigInteger PublicExponent
		{
			get
			{
				return this.publicExponent;
			}
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000F7D99 File Offset: 0x000F5F99
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(this.Modulus),
				new DerInteger(this.PublicExponent)
			});
		}

		// Token: 0x04001EED RID: 7917
		private BigInteger modulus;

		// Token: 0x04001EEE RID: 7918
		private BigInteger publicExponent;
	}
}
