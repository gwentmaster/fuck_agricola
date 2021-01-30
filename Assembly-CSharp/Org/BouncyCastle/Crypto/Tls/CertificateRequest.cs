using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A1 RID: 929
	public class CertificateRequest
	{
		// Token: 0x06002319 RID: 8985 RVA: 0x000B68E8 File Offset: 0x000B4AE8
		public CertificateRequest(byte[] certificateTypes, IList supportedSignatureAlgorithms, IList certificateAuthorities)
		{
			this.mCertificateTypes = certificateTypes;
			this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
			this.mCertificateAuthorities = certificateAuthorities;
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000B6905 File Offset: 0x000B4B05
		public virtual byte[] CertificateTypes
		{
			get
			{
				return this.mCertificateTypes;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000B690D File Offset: 0x000B4B0D
		public virtual IList SupportedSignatureAlgorithms
		{
			get
			{
				return this.mSupportedSignatureAlgorithms;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000B6915 File Offset: 0x000B4B15
		public virtual IList CertificateAuthorities
		{
			get
			{
				return this.mCertificateAuthorities;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000B6920 File Offset: 0x000B4B20
		public virtual void Encode(Stream output)
		{
			if (this.mCertificateTypes == null || this.mCertificateTypes.Length == 0)
			{
				TlsUtilities.WriteUint8(0, output);
			}
			else
			{
				TlsUtilities.WriteUint8ArrayWithUint8Length(this.mCertificateTypes, output);
			}
			if (this.mSupportedSignatureAlgorithms != null)
			{
				TlsUtilities.EncodeSupportedSignatureAlgorithms(this.mSupportedSignatureAlgorithms, false, output);
			}
			if (this.mCertificateAuthorities == null || this.mCertificateAuthorities.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			IList list = Platform.CreateArrayList(this.mCertificateAuthorities.Count);
			int num = 0;
			foreach (object obj in this.mCertificateAuthorities)
			{
				byte[] encoded = ((Asn1Encodable)obj).GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 2;
			}
			TlsUtilities.CheckUint16(num);
			TlsUtilities.WriteUint16(num, output);
			foreach (object obj2 in list)
			{
				TlsUtilities.WriteOpaque16((byte[])obj2, output);
			}
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000B6A4C File Offset: 0x000B4C4C
		public static CertificateRequest Parse(TlsContext context, Stream input)
		{
			int num = (int)TlsUtilities.ReadUint8(input);
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = TlsUtilities.ReadUint8(input);
			}
			IList supportedSignatureAlgorithms = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				supportedSignatureAlgorithms = TlsUtilities.ParseSupportedSignatureAlgorithms(false, input);
			}
			IList list = Platform.CreateArrayList();
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadOpaque16(input), false);
			while (memoryStream.Position < memoryStream.Length)
			{
				Asn1Object obj = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream));
				list.Add(X509Name.GetInstance(obj));
			}
			return new CertificateRequest(array, supportedSignatureAlgorithms, list);
		}

		// Token: 0x040016E2 RID: 5858
		protected readonly byte[] mCertificateTypes;

		// Token: 0x040016E3 RID: 5859
		protected readonly IList mSupportedSignatureAlgorithms;

		// Token: 0x040016E4 RID: 5860
		protected readonly IList mCertificateAuthorities;
	}
}
