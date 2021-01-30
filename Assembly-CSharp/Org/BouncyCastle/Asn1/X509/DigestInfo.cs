using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000527 RID: 1319
	public class DigestInfo : Asn1Encodable
	{
		// Token: 0x0600301E RID: 12318 RVA: 0x000F6AD5 File Offset: 0x000F4CD5
		public static DigestInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DigestInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x000F6AE3 File Offset: 0x000F4CE3
		public static DigestInfo GetInstance(object obj)
		{
			if (obj is DigestInfo)
			{
				return (DigestInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DigestInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x000F6B22 File Offset: 0x000F4D22
		public DigestInfo(AlgorithmIdentifier algID, byte[] digest)
		{
			this.digest = digest;
			this.algID = algID;
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x000F6B38 File Offset: 0x000F4D38
		private DigestInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
			this.digest = Asn1OctetString.GetInstance(seq[1]).GetOctets();
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000F6B8D File Offset: 0x000F4D8D
		public AlgorithmIdentifier AlgorithmID
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x000F6B95 File Offset: 0x000F4D95
		public byte[] GetDigest()
		{
			return this.digest;
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000F6B9D File Offset: 0x000F4D9D
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algID,
				new DerOctetString(this.digest)
			});
		}

		// Token: 0x04001EC8 RID: 7880
		private readonly byte[] digest;

		// Token: 0x04001EC9 RID: 7881
		private readonly AlgorithmIdentifier algID;
	}
}
