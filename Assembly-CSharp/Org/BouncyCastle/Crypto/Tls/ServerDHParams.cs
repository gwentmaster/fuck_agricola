using System;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CF RID: 975
	public class ServerDHParams
	{
		// Token: 0x060023FC RID: 9212 RVA: 0x000B8830 File Offset: 0x000B6A30
		public ServerDHParams(DHPublicKeyParameters publicKey)
		{
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			this.mPublicKey = publicKey;
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x000B884D File Offset: 0x000B6A4D
		public virtual DHPublicKeyParameters PublicKey
		{
			get
			{
				return this.mPublicKey;
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000B8858 File Offset: 0x000B6A58
		public virtual void Encode(Stream output)
		{
			DHParameters parameters = this.mPublicKey.Parameters;
			BigInteger y = this.mPublicKey.Y;
			TlsDHUtilities.WriteDHParameter(parameters.P, output);
			TlsDHUtilities.WriteDHParameter(parameters.G, output);
			TlsDHUtilities.WriteDHParameter(y, output);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000B889C File Offset: 0x000B6A9C
		public static ServerDHParams Parse(Stream input)
		{
			BigInteger p = TlsDHUtilities.ReadDHParameter(input);
			BigInteger g = TlsDHUtilities.ReadDHParameter(input);
			return new ServerDHParams(TlsDHUtilities.ValidateDHPublicKey(new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), new DHParameters(p, g))));
		}

		// Token: 0x04001905 RID: 6405
		protected readonly DHPublicKeyParameters mPublicKey;
	}
}
