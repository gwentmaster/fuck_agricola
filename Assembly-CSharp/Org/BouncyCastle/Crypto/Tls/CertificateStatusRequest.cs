using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A3 RID: 931
	public class CertificateStatusRequest
	{
		// Token: 0x06002326 RID: 8998 RVA: 0x000B6BE5 File Offset: 0x000B4DE5
		public CertificateStatusRequest(byte statusType, object request)
		{
			if (!CertificateStatusRequest.IsCorrectType(statusType, request))
			{
				throw new ArgumentException("not an instance of the correct type", "request");
			}
			this.mStatusType = statusType;
			this.mRequest = request;
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x000B6C14 File Offset: 0x000B4E14
		public virtual byte StatusType
		{
			get
			{
				return this.mStatusType;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x000B6C1C File Offset: 0x000B4E1C
		public virtual object Request
		{
			get
			{
				return this.mRequest;
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000B6C24 File Offset: 0x000B4E24
		public virtual OcspStatusRequest GetOcspStatusRequest()
		{
			if (!CertificateStatusRequest.IsCorrectType(1, this.mRequest))
			{
				throw new InvalidOperationException("'request' is not an OCSPStatusRequest");
			}
			return (OcspStatusRequest)this.mRequest;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000B6C4C File Offset: 0x000B4E4C
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mStatusType, output);
			byte b = this.mStatusType;
			if (b == 1)
			{
				((OcspStatusRequest)this.mRequest).Encode(output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000B6C8C File Offset: 0x000B4E8C
		public static CertificateStatusRequest Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b == 1)
			{
				object request = OcspStatusRequest.Parse(input);
				return new CertificateStatusRequest(b, request);
			}
			throw new TlsFatalAlert(50);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000B6CBC File Offset: 0x000B4EBC
		protected static bool IsCorrectType(byte statusType, object request)
		{
			if (statusType == 1)
			{
				return request is OcspStatusRequest;
			}
			throw new ArgumentException("unsupported value", "statusType");
		}

		// Token: 0x040016E7 RID: 5863
		protected readonly byte mStatusType;

		// Token: 0x040016E8 RID: 5864
		protected readonly object mRequest;
	}
}
