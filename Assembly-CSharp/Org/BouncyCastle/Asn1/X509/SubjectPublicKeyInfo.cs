using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000530 RID: 1328
	public class SubjectPublicKeyInfo : Asn1Encodable
	{
		// Token: 0x0600306B RID: 12395 RVA: 0x000F7DC2 File Offset: 0x000F5FC2
		public static SubjectPublicKeyInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return SubjectPublicKeyInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000F7DD0 File Offset: 0x000F5FD0
		public static SubjectPublicKeyInfo GetInstance(object obj)
		{
			if (obj is SubjectPublicKeyInfo)
			{
				return (SubjectPublicKeyInfo)obj;
			}
			if (obj != null)
			{
				return new SubjectPublicKeyInfo(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000F7DF1 File Offset: 0x000F5FF1
		public SubjectPublicKeyInfo(AlgorithmIdentifier algID, Asn1Encodable publicKey)
		{
			this.keyData = new DerBitString(publicKey);
			this.algID = algID;
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000F7E0C File Offset: 0x000F600C
		public SubjectPublicKeyInfo(AlgorithmIdentifier algID, byte[] publicKey)
		{
			this.keyData = new DerBitString(publicKey);
			this.algID = algID;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000F7E28 File Offset: 0x000F6028
		private SubjectPublicKeyInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
			this.keyData = DerBitString.GetInstance(seq[1]);
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x000F7E88 File Offset: 0x000F6088
		public AlgorithmIdentifier AlgorithmID
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000F7E90 File Offset: 0x000F6090
		public Asn1Object GetPublicKey()
		{
			return Asn1Object.FromByteArray(this.keyData.GetOctets());
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x000F7EA2 File Offset: 0x000F60A2
		public DerBitString PublicKeyData
		{
			get
			{
				return this.keyData;
			}
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000F7EAA File Offset: 0x000F60AA
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algID,
				this.keyData
			});
		}

		// Token: 0x04001EF8 RID: 7928
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04001EF9 RID: 7929
		private readonly DerBitString keyData;
	}
}
