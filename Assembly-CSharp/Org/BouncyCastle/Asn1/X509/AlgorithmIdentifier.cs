using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000520 RID: 1312
	public class AlgorithmIdentifier : Asn1Encodable
	{
		// Token: 0x06002FE7 RID: 12263 RVA: 0x000F62F0 File Offset: 0x000F44F0
		public static AlgorithmIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return AlgorithmIdentifier.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000F62FE File Offset: 0x000F44FE
		public static AlgorithmIdentifier GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is AlgorithmIdentifier)
			{
				return (AlgorithmIdentifier)obj;
			}
			return new AlgorithmIdentifier(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000F631F File Offset: 0x000F451F
		public AlgorithmIdentifier(DerObjectIdentifier algorithm)
		{
			this.algorithm = algorithm;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000F632E File Offset: 0x000F452E
		[Obsolete("Use version taking a DerObjectIdentifier")]
		public AlgorithmIdentifier(string algorithm)
		{
			this.algorithm = new DerObjectIdentifier(algorithm);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000F6342 File Offset: 0x000F4542
		public AlgorithmIdentifier(DerObjectIdentifier algorithm, Asn1Encodable parameters)
		{
			this.algorithm = algorithm;
			this.parameters = parameters;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000F6358 File Offset: 0x000F4558
		internal AlgorithmIdentifier(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.algorithm = DerObjectIdentifier.GetInstance(seq[0]);
			this.parameters = ((seq.Count < 2) ? null : seq[1]);
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x000F63C3 File Offset: 0x000F45C3
		public virtual DerObjectIdentifier Algorithm
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x000F63C3 File Offset: 0x000F45C3
		[Obsolete("Use 'Algorithm' property instead")]
		public virtual DerObjectIdentifier ObjectID
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002FEF RID: 12271 RVA: 0x000F63CB File Offset: 0x000F45CB
		public virtual Asn1Encodable Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000F63D4 File Offset: 0x000F45D4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.algorithm
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.parameters
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04001EB2 RID: 7858
		private readonly DerObjectIdentifier algorithm;

		// Token: 0x04001EB3 RID: 7859
		private readonly Asn1Encodable parameters;
	}
}
