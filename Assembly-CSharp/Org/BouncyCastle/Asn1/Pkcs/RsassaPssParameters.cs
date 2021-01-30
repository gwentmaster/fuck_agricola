using System;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000546 RID: 1350
	public class RsassaPssParameters : Asn1Encodable
	{
		// Token: 0x06003134 RID: 12596 RVA: 0x000FCBBC File Offset: 0x000FADBC
		public static RsassaPssParameters GetInstance(object obj)
		{
			if (obj == null || obj is RsassaPssParameters)
			{
				return (RsassaPssParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RsassaPssParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000FCC09 File Offset: 0x000FAE09
		public RsassaPssParameters()
		{
			this.hashAlgorithm = RsassaPssParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsassaPssParameters.DefaultMaskGenFunction;
			this.saltLength = RsassaPssParameters.DefaultSaltLength;
			this.trailerField = RsassaPssParameters.DefaultTrailerField;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000FCC3D File Offset: 0x000FAE3D
		public RsassaPssParameters(AlgorithmIdentifier hashAlgorithm, AlgorithmIdentifier maskGenAlgorithm, DerInteger saltLength, DerInteger trailerField)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.maskGenAlgorithm = maskGenAlgorithm;
			this.saltLength = saltLength;
			this.trailerField = trailerField;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000FCC64 File Offset: 0x000FAE64
		public RsassaPssParameters(Asn1Sequence seq)
		{
			this.hashAlgorithm = RsassaPssParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsassaPssParameters.DefaultMaskGenFunction;
			this.saltLength = RsassaPssParameters.DefaultSaltLength;
			this.trailerField = RsassaPssParameters.DefaultTrailerField;
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[num];
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.hashAlgorithm = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				case 1:
					this.maskGenAlgorithm = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				case 2:
					this.saltLength = DerInteger.GetInstance(asn1TaggedObject, true);
					break;
				case 3:
					this.trailerField = DerInteger.GetInstance(asn1TaggedObject, true);
					break;
				default:
					throw new ArgumentException("unknown tag");
				}
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x000FCD27 File Offset: 0x000FAF27
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000FCD2F File Offset: 0x000FAF2F
		public AlgorithmIdentifier MaskGenAlgorithm
		{
			get
			{
				return this.maskGenAlgorithm;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x000FCD37 File Offset: 0x000FAF37
		public DerInteger SaltLength
		{
			get
			{
				return this.saltLength;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x000FCD3F File Offset: 0x000FAF3F
		public DerInteger TrailerField
		{
			get
			{
				return this.trailerField;
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000FCD48 File Offset: 0x000FAF48
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (!this.hashAlgorithm.Equals(RsassaPssParameters.DefaultHashAlgorithm))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.hashAlgorithm)
				});
			}
			if (!this.maskGenAlgorithm.Equals(RsassaPssParameters.DefaultMaskGenFunction))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.maskGenAlgorithm)
				});
			}
			if (!this.saltLength.Equals(RsassaPssParameters.DefaultSaltLength))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.saltLength)
				});
			}
			if (!this.trailerField.Equals(RsassaPssParameters.DefaultTrailerField))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 3, this.trailerField)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002051 RID: 8273
		private AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002052 RID: 8274
		private AlgorithmIdentifier maskGenAlgorithm;

		// Token: 0x04002053 RID: 8275
		private DerInteger saltLength;

		// Token: 0x04002054 RID: 8276
		private DerInteger trailerField;

		// Token: 0x04002055 RID: 8277
		public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);

		// Token: 0x04002056 RID: 8278
		public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, RsassaPssParameters.DefaultHashAlgorithm);

		// Token: 0x04002057 RID: 8279
		public static readonly DerInteger DefaultSaltLength = new DerInteger(20);

		// Token: 0x04002058 RID: 8280
		public static readonly DerInteger DefaultTrailerField = new DerInteger(1);
	}
}
