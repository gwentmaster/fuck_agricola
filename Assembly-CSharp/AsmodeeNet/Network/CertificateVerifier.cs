using System;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Tls;

namespace AsmodeeNet.Network
{
	// Token: 0x02000680 RID: 1664
	public class CertificateVerifier : ICertificateVerifyer
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x0012C615 File Offset: 0x0012A815
		// (set) Token: 0x06003CE7 RID: 15591 RVA: 0x0012C61D File Offset: 0x0012A81D
		public bool isValid { get; protected set; }

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0012C626 File Offset: 0x0012A826
		public CertificateVerifier(string[] publicKeys)
		{
			this._publicKeys = publicKeys;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0012C638 File Offset: 0x0012A838
		public bool IsValid(Uri serverUri, X509CertificateStructure[] certs)
		{
			this.isValid = false;
			int num = 0;
			while (num < certs.Length && !this.isValid)
			{
				string @string = certs[num].SubjectPublicKeyInfo.PublicKeyData.GetString();
				foreach (string b in this._publicKeys)
				{
					if (@string == b)
					{
						this.isValid = true;
						break;
					}
				}
				num++;
			}
			return this.isValid;
		}

		// Token: 0x04002711 RID: 10001
		private string[] _publicKeys;
	}
}
