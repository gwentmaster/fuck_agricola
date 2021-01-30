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
	// Token: 0x0200027C RID: 636
	public class X509CertificateParser
	{
		// Token: 0x060014DF RID: 5343 RVA: 0x00077A88 File Offset: 0x00075C88
		private X509Certificate ReadDerCertificate(Asn1InputStream dIn)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
			if (asn1Sequence.Count > 1 && asn1Sequence[0] is DerObjectIdentifier && asn1Sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
			{
				this.sData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject)asn1Sequence[1], true)).Certificates;
				return this.GetCertificate();
			}
			return this.CreateX509Certificate(X509CertificateStructure.GetInstance(asn1Sequence));
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00077B00 File Offset: 0x00075D00
		private X509Certificate GetCertificate()
		{
			if (this.sData != null)
			{
				while (this.sDataObjectCount < this.sData.Count)
				{
					Asn1Set asn1Set = this.sData;
					int num = this.sDataObjectCount;
					this.sDataObjectCount = num + 1;
					object obj = asn1Set[num];
					if (obj is Asn1Sequence)
					{
						return this.CreateX509Certificate(X509CertificateStructure.GetInstance(obj));
					}
				}
			}
			return null;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00077B60 File Offset: 0x00075D60
		private X509Certificate ReadPemCertificate(Stream inStream)
		{
			Asn1Sequence asn1Sequence = X509CertificateParser.PemCertParser.ReadPemObject(inStream);
			if (asn1Sequence != null)
			{
				return this.CreateX509Certificate(X509CertificateStructure.GetInstance(asn1Sequence));
			}
			return null;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00077B8A File Offset: 0x00075D8A
		protected virtual X509Certificate CreateX509Certificate(X509CertificateStructure c)
		{
			return new X509Certificate(c);
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00077B92 File Offset: 0x00075D92
		public X509Certificate ReadCertificate(byte[] input)
		{
			return this.ReadCertificate(new MemoryStream(input, false));
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00077BA1 File Offset: 0x00075DA1
		public ICollection ReadCertificates(byte[] input)
		{
			return this.ReadCertificates(new MemoryStream(input, false));
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00077BB0 File Offset: 0x00075DB0
		public X509Certificate ReadCertificate(Stream inStream)
		{
			if (inStream == null)
			{
				throw new ArgumentNullException("inStream");
			}
			if (!inStream.CanRead)
			{
				throw new ArgumentException("inStream must be read-able", "inStream");
			}
			if (this.currentStream == null)
			{
				this.currentStream = inStream;
				this.sData = null;
				this.sDataObjectCount = 0;
			}
			else if (this.currentStream != inStream)
			{
				this.currentStream = inStream;
				this.sData = null;
				this.sDataObjectCount = 0;
			}
			X509Certificate result;
			try
			{
				if (this.sData != null)
				{
					if (this.sDataObjectCount != this.sData.Count)
					{
						result = this.GetCertificate();
					}
					else
					{
						this.sData = null;
						this.sDataObjectCount = 0;
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
							result = this.ReadPemCertificate(pushbackStream);
						}
						else
						{
							result = this.ReadDerCertificate(new Asn1InputStream(pushbackStream));
						}
					}
				}
			}
			catch (Exception exception)
			{
				throw new CertificateException("Failed to read certificate", exception);
			}
			return result;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00077CB0 File Offset: 0x00075EB0
		public ICollection ReadCertificates(Stream inStream)
		{
			IList list = Platform.CreateArrayList();
			X509Certificate value;
			while ((value = this.ReadCertificate(inStream)) != null)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x04001382 RID: 4994
		private static readonly PemParser PemCertParser = new PemParser("CERTIFICATE");

		// Token: 0x04001383 RID: 4995
		private Asn1Set sData;

		// Token: 0x04001384 RID: 4996
		private int sDataObjectCount;

		// Token: 0x04001385 RID: 4997
		private Stream currentStream;
	}
}
