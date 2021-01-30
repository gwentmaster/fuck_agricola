using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000519 RID: 1305
	public class X9ECParameters : Asn1Encodable
	{
		// Token: 0x06002FB9 RID: 12217 RVA: 0x000F57A4 File Offset: 0x000F39A4
		public static X9ECParameters GetInstance(object obj)
		{
			if (obj is X9ECParameters)
			{
				return (X9ECParameters)obj;
			}
			if (obj != null)
			{
				return new X9ECParameters(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000F57C8 File Offset: 0x000F39C8
		public X9ECParameters(Asn1Sequence seq)
		{
			if (!(seq[0] is DerInteger) || !((DerInteger)seq[0]).Value.Equals(BigInteger.One))
			{
				throw new ArgumentException("bad version in X9ECParameters");
			}
			X9Curve x9Curve = new X9Curve(X9FieldID.GetInstance(seq[1]), Asn1Sequence.GetInstance(seq[2]));
			this.curve = x9Curve.Curve;
			object obj = seq[3];
			if (obj is X9ECPoint)
			{
				this.g = (X9ECPoint)obj;
			}
			else
			{
				this.g = new X9ECPoint(this.curve, (Asn1OctetString)obj);
			}
			this.n = ((DerInteger)seq[4]).Value;
			this.seed = x9Curve.GetSeed();
			if (seq.Count == 6)
			{
				this.h = ((DerInteger)seq[5]).Value;
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000F58B3 File Offset: 0x000F3AB3
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, null, null)
		{
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000F58C0 File Offset: 0x000F3AC0
		public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000F58CE File Offset: 0x000F3ACE
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000F58DC File Offset: 0x000F3ADC
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed) : this(curve, new X9ECPoint(g), n, h, seed)
		{
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000F58F0 File Offset: 0x000F3AF0
		public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h, byte[] seed)
		{
			this.curve = curve;
			this.g = g;
			this.n = n;
			this.h = h;
			this.seed = seed;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				this.fieldID = new X9FieldID(curve.Field.Characteristic);
				return;
			}
			if (!ECAlgorithms.IsF2mCurve(curve))
			{
				throw new ArgumentException("'curve' is of an unsupported type");
			}
			int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
			if (exponentsPresent.Length == 3)
			{
				this.fieldID = new X9FieldID(exponentsPresent[2], exponentsPresent[1]);
				return;
			}
			if (exponentsPresent.Length == 5)
			{
				this.fieldID = new X9FieldID(exponentsPresent[4], exponentsPresent[1], exponentsPresent[2], exponentsPresent[3]);
				return;
			}
			throw new ArgumentException("Only trinomial and pentomial curves are supported");
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000F59B0 File Offset: 0x000F3BB0
		public ECCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002FC1 RID: 12225 RVA: 0x000F59B8 File Offset: 0x000F3BB8
		public ECPoint G
		{
			get
			{
				return this.g.Point;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000F59C5 File Offset: 0x000F3BC5
		public BigInteger N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000F59CD File Offset: 0x000F3BCD
		public BigInteger H
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000F59D5 File Offset: 0x000F3BD5
		public byte[] GetSeed()
		{
			return this.seed;
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x000F59DD File Offset: 0x000F3BDD
		public X9Curve CurveEntry
		{
			get
			{
				return new X9Curve(this.curve, this.seed);
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000F59F0 File Offset: 0x000F3BF0
		public X9FieldID FieldIDEntry
		{
			get
			{
				return this.fieldID;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000F59F8 File Offset: 0x000F3BF8
		public X9ECPoint BaseEntry
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000F5A00 File Offset: 0x000F3C00
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(BigInteger.One),
				this.fieldID,
				new X9Curve(this.curve, this.seed),
				this.g,
				new DerInteger(this.n)
			});
			if (this.h != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerInteger(this.h)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04001E63 RID: 7779
		private X9FieldID fieldID;

		// Token: 0x04001E64 RID: 7780
		private ECCurve curve;

		// Token: 0x04001E65 RID: 7781
		private X9ECPoint g;

		// Token: 0x04001E66 RID: 7782
		private BigInteger n;

		// Token: 0x04001E67 RID: 7783
		private BigInteger h;

		// Token: 0x04001E68 RID: 7784
		private byte[] seed;
	}
}
