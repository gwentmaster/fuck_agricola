using System;
using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A2 RID: 930
	public class CertificateStatus
	{
		// Token: 0x0600231F RID: 8991 RVA: 0x000B6ADD File Offset: 0x000B4CDD
		public CertificateStatus(byte statusType, object response)
		{
			if (!CertificateStatus.IsCorrectType(statusType, response))
			{
				throw new ArgumentException("not an instance of the correct type", "response");
			}
			this.mStatusType = statusType;
			this.mResponse = response;
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x000B6B0C File Offset: 0x000B4D0C
		public virtual byte StatusType
		{
			get
			{
				return this.mStatusType;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x000B6B14 File Offset: 0x000B4D14
		public virtual object Response
		{
			get
			{
				return this.mResponse;
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000B6B1C File Offset: 0x000B4D1C
		public virtual OcspResponse GetOcspResponse()
		{
			if (!CertificateStatus.IsCorrectType(1, this.mResponse))
			{
				throw new InvalidOperationException("'response' is not an OcspResponse");
			}
			return (OcspResponse)this.mResponse;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000B6B44 File Offset: 0x000B4D44
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mStatusType, output);
			byte b = this.mStatusType;
			if (b == 1)
			{
				TlsUtilities.WriteOpaque24(((OcspResponse)this.mResponse).GetEncoded("DER"), output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000B6B8C File Offset: 0x000B4D8C
		public static CertificateStatus Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b == 1)
			{
				object instance = OcspResponse.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(input)));
				return new CertificateStatus(b, instance);
			}
			throw new TlsFatalAlert(50);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000B6BC6 File Offset: 0x000B4DC6
		protected static bool IsCorrectType(byte statusType, object response)
		{
			if (statusType == 1)
			{
				return response is OcspResponse;
			}
			throw new ArgumentException("unsupported value", "statusType");
		}

		// Token: 0x040016E5 RID: 5861
		protected readonly byte mStatusType;

		// Token: 0x040016E6 RID: 5862
		protected readonly object mResponse;
	}
}
