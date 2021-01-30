using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.X509
{
	// Token: 0x0200027F RID: 639
	public class X509CrlParser
	{
		// Token: 0x0600150A RID: 5386 RVA: 0x00078820 File Offset: 0x00076A20
		public X509CrlParser() : this(false)
		{
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00078829 File Offset: 0x00076A29
		public X509CrlParser(bool lazyAsn1)
		{
			this.lazyAsn1 = lazyAsn1;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00078838 File Offset: 0x00076A38
		private X509Crl ReadPemCrl(Stream inStream)
		{
			Asn1Sequence asn1Sequence = X509CrlParser.PemCrlParser.ReadPemObject(inStream);
			if (asn1Sequence != null)
			{
				return this.CreateX509Crl(CertificateList.GetInstance(asn1Sequence));
			}
			return null;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00078864 File Offset: 0x00076A64
		private X509Crl ReadDerCrl(Asn1InputStream dIn)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
			if (asn1Sequence.Count > 1 && asn1Sequence[0] is DerObjectIdentifier && asn1Sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
			{
				this.sCrlData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject)asn1Sequence[1], true)).Crls;
				return this.GetCrl();
			}
			return this.CreateX509Crl(CertificateList.GetInstance(asn1Sequence));
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x000788DC File Offset: 0x00076ADC
		private X509Crl GetCrl()
		{
			if (this.sCrlData == null || this.sCrlDataObjectCount >= this.sCrlData.Count)
			{
				return null;
			}
			Asn1Set asn1Set = this.sCrlData;
			int num = this.sCrlDataObjectCount;
			this.sCrlDataObjectCount = num + 1;
			return this.CreateX509Crl(CertificateList.GetInstance(asn1Set[num]));
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0007892D File Offset: 0x00076B2D
		protected virtual X509Crl CreateX509Crl(CertificateList c)
		{
			return new X509Crl(c);
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00078935 File Offset: 0x00076B35
		public X509Crl ReadCrl(byte[] input)
		{
			return this.ReadCrl(new MemoryStream(input, false));
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00078944 File Offset: 0x00076B44
		public ICollection ReadCrls(byte[] input)
		{
			return this.ReadCrls(new MemoryStream(input, false));
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00078954 File Offset: 0x00076B54
		public X509Crl ReadCrl(Stream inStream)
		{
			if (inStream == null)
			{
				throw new ArgumentNullException("inStream");
			}
			if (!inStream.CanRead)
			{
				throw new ArgumentException("inStream must be read-able", "inStream");
			}
			if (this.currentCrlStream == null)
			{
				this.currentCrlStream = inStream;
				this.sCrlData = null;
				this.sCrlDataObjectCount = 0;
			}
			else if (this.currentCrlStream != inStream)
			{
				this.currentCrlStream = inStream;
				this.sCrlData = null;
				this.sCrlDataObjectCount = 0;
			}
			X509Crl result;
			try
			{
				if (this.sCrlData != null)
				{
					if (this.sCrlDataObjectCount != this.sCrlData.Count)
					{
						result = this.GetCrl();
					}
					else
					{
						this.sCrlData = null;
						this.sCrlDataObjectCount = 0;
						result = null;
					}
				}
				else
				{
					PushbackStream pushbackStream = new PushbackStream(inStream);
					int num = pushbackStream.ReadByte();
					if (num < 0)
					{
						result = null;
					}
					else
					{
						pushbackStream.Unread(num);
						if (num != 48)
						{
							result = this.ReadPemCrl(pushbackStream);
						}
						else
						{
							Asn1InputStream dIn = this.lazyAsn1 ? new LazyAsn1InputStream(pushbackStream) : new Asn1InputStream(pushbackStream);
							result = this.ReadDerCrl(dIn);
						}
					}
				}
			}
			catch (CrlException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new CrlException(ex2.ToString());
			}
			return result;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00078A74 File Offset: 0x00076C74
		public ICollection ReadCrls(Stream inStream)
		{
			IList list = Platform.CreateArrayList();
			X509Crl value;
			while ((value = this.ReadCrl(inStream)) != null)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x0400138E RID: 5006
		private static readonly PemParser PemCrlParser = new PemParser("CRL");

		// Token: 0x0400138F RID: 5007
		private readonly bool lazyAsn1;

		// Token: 0x04001390 RID: 5008
		private Asn1Set sCrlData;

		// Token: 0x04001391 RID: 5009
		private int sCrlDataObjectCount;

		// Token: 0x04001392 RID: 5010
		private Stream currentCrlStream;
	}
}
