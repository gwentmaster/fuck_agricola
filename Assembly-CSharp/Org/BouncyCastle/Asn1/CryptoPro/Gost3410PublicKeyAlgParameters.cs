using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000559 RID: 1369
	public class Gost3410PublicKeyAlgParameters : Asn1Encodable
	{
		// Token: 0x06003193 RID: 12691 RVA: 0x000FE573 File Offset: 0x000FC773
		public static Gost3410PublicKeyAlgParameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Gost3410PublicKeyAlgParameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000FE581 File Offset: 0x000FC781
		public static Gost3410PublicKeyAlgParameters GetInstance(object obj)
		{
			if (obj == null || obj is Gost3410PublicKeyAlgParameters)
			{
				return (Gost3410PublicKeyAlgParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Gost3410PublicKeyAlgParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000FE5BE File Offset: 0x000FC7BE
		public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet) : this(publicKeyParamSet, digestParamSet, null)
		{
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000FE5C9 File Offset: 0x000FC7C9
		public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet, DerObjectIdentifier encryptionParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			if (digestParamSet == null)
			{
				throw new ArgumentNullException("digestParamSet");
			}
			this.publicKeyParamSet = publicKeyParamSet;
			this.digestParamSet = digestParamSet;
			this.encryptionParamSet = encryptionParamSet;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000FE604 File Offset: 0x000FC804
		public Gost3410PublicKeyAlgParameters(Asn1Sequence seq)
		{
			this.publicKeyParamSet = (DerObjectIdentifier)seq[0];
			this.digestParamSet = (DerObjectIdentifier)seq[1];
			if (seq.Count > 2)
			{
				this.encryptionParamSet = (DerObjectIdentifier)seq[2];
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x000FE656 File Offset: 0x000FC856
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x000FE65E File Offset: 0x000FC85E
		public DerObjectIdentifier DigestParamSet
		{
			get
			{
				return this.digestParamSet;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x000FE666 File Offset: 0x000FC866
		public DerObjectIdentifier EncryptionParamSet
		{
			get
			{
				return this.encryptionParamSet;
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000FE670 File Offset: 0x000FC870
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.publicKeyParamSet,
				this.digestParamSet
			});
			if (this.encryptionParamSet != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.encryptionParamSet
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040020F4 RID: 8436
		private DerObjectIdentifier publicKeyParamSet;

		// Token: 0x040020F5 RID: 8437
		private DerObjectIdentifier digestParamSet;

		// Token: 0x040020F6 RID: 8438
		private DerObjectIdentifier encryptionParamSet;
	}
}
