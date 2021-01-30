using System;
using System.Collections;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000532 RID: 1330
	public class TbsCertificateList : Asn1Encodable
	{
		// Token: 0x06003079 RID: 12409 RVA: 0x000F7F84 File Offset: 0x000F6184
		public static TbsCertificateList GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return TbsCertificateList.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000F7F94 File Offset: 0x000F6194
		public static TbsCertificateList GetInstance(object obj)
		{
			TbsCertificateList tbsCertificateList = obj as TbsCertificateList;
			if (obj == null || tbsCertificateList != null)
			{
				return tbsCertificateList;
			}
			if (obj is Asn1Sequence)
			{
				return new TbsCertificateList((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000F7FE0 File Offset: 0x000F61E0
		internal TbsCertificateList(Asn1Sequence seq)
		{
			if (seq.Count < 3 || seq.Count > 7)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			int num = 0;
			this.seq = seq;
			if (seq[num] is DerInteger)
			{
				this.version = DerInteger.GetInstance(seq[num++]);
			}
			else
			{
				this.version = new DerInteger(0);
			}
			this.signature = AlgorithmIdentifier.GetInstance(seq[num++]);
			this.issuer = X509Name.GetInstance(seq[num++]);
			this.thisUpdate = Time.GetInstance(seq[num++]);
			if (num < seq.Count && (seq[num] is DerUtcTime || seq[num] is DerGeneralizedTime || seq[num] is Time))
			{
				this.nextUpdate = Time.GetInstance(seq[num++]);
			}
			if (num < seq.Count && !(seq[num] is DerTaggedObject))
			{
				this.revokedCertificates = Asn1Sequence.GetInstance(seq[num++]);
			}
			if (num < seq.Count && seq[num] is DerTaggedObject)
			{
				this.crlExtensions = X509Extensions.GetInstance(seq[num]);
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000F813C File Offset: 0x000F633C
		public int Version
		{
			get
			{
				return this.version.Value.IntValue + 1;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x000F8150 File Offset: 0x000F6350
		public DerInteger VersionNumber
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x000F8158 File Offset: 0x000F6358
		public AlgorithmIdentifier Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x000F8160 File Offset: 0x000F6360
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000F8168 File Offset: 0x000F6368
		public Time ThisUpdate
		{
			get
			{
				return this.thisUpdate;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000F8170 File Offset: 0x000F6370
		public Time NextUpdate
		{
			get
			{
				return this.nextUpdate;
			}
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x000F8178 File Offset: 0x000F6378
		public CrlEntry[] GetRevokedCertificates()
		{
			if (this.revokedCertificates == null)
			{
				return new CrlEntry[0];
			}
			CrlEntry[] array = new CrlEntry[this.revokedCertificates.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new CrlEntry(Asn1Sequence.GetInstance(this.revokedCertificates[i]));
			}
			return array;
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000F81CD File Offset: 0x000F63CD
		public IEnumerable GetRevokedCertificateEnumeration()
		{
			if (this.revokedCertificates == null)
			{
				return EmptyEnumerable.Instance;
			}
			return new TbsCertificateList.RevokedCertificatesEnumeration(this.revokedCertificates);
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000F81E8 File Offset: 0x000F63E8
		public X509Extensions Extensions
		{
			get
			{
				return this.crlExtensions;
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000F81F0 File Offset: 0x000F63F0
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04001EFE RID: 7934
		internal Asn1Sequence seq;

		// Token: 0x04001EFF RID: 7935
		internal DerInteger version;

		// Token: 0x04001F00 RID: 7936
		internal AlgorithmIdentifier signature;

		// Token: 0x04001F01 RID: 7937
		internal X509Name issuer;

		// Token: 0x04001F02 RID: 7938
		internal Time thisUpdate;

		// Token: 0x04001F03 RID: 7939
		internal Time nextUpdate;

		// Token: 0x04001F04 RID: 7940
		internal Asn1Sequence revokedCertificates;

		// Token: 0x04001F05 RID: 7941
		internal X509Extensions crlExtensions;

		// Token: 0x020008C1 RID: 2241
		private class RevokedCertificatesEnumeration : IEnumerable
		{
			// Token: 0x0600461A RID: 17946 RVA: 0x00146338 File Offset: 0x00144538
			internal RevokedCertificatesEnumeration(IEnumerable en)
			{
				this.en = en;
			}

			// Token: 0x0600461B RID: 17947 RVA: 0x00146347 File Offset: 0x00144547
			public IEnumerator GetEnumerator()
			{
				return new TbsCertificateList.RevokedCertificatesEnumeration.RevokedCertificatesEnumerator(this.en.GetEnumerator());
			}

			// Token: 0x04002FA7 RID: 12199
			private readonly IEnumerable en;

			// Token: 0x02000A60 RID: 2656
			private class RevokedCertificatesEnumerator : IEnumerator
			{
				// Token: 0x06004A67 RID: 19047 RVA: 0x0014F2C2 File Offset: 0x0014D4C2
				internal RevokedCertificatesEnumerator(IEnumerator e)
				{
					this.e = e;
				}

				// Token: 0x06004A68 RID: 19048 RVA: 0x0014F2D1 File Offset: 0x0014D4D1
				public bool MoveNext()
				{
					return this.e.MoveNext();
				}

				// Token: 0x06004A69 RID: 19049 RVA: 0x0014F2DE File Offset: 0x0014D4DE
				public void Reset()
				{
					this.e.Reset();
				}

				// Token: 0x17000AA5 RID: 2725
				// (get) Token: 0x06004A6A RID: 19050 RVA: 0x0014F2EB File Offset: 0x0014D4EB
				public object Current
				{
					get
					{
						return new CrlEntry(Asn1Sequence.GetInstance(this.e.Current));
					}
				}

				// Token: 0x040034B1 RID: 13489
				private readonly IEnumerator e;
			}
		}
	}
}
