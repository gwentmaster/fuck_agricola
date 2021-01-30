using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CC RID: 972
	public sealed class ProtocolVersion
	{
		// Token: 0x060023C6 RID: 9158 RVA: 0x000B7FA2 File Offset: 0x000B61A2
		private ProtocolVersion(int v, string name)
		{
			this.version = (v & 65535);
			this.name = name;
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x000B7FBE File Offset: 0x000B61BE
		public int FullVersion
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000B7FC6 File Offset: 0x000B61C6
		public int MajorVersion
		{
			get
			{
				return this.version >> 8;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x000B7FD0 File Offset: 0x000B61D0
		public int MinorVersion
		{
			get
			{
				return this.version & 255;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x000B7FDE File Offset: 0x000B61DE
		public bool IsDtls
		{
			get
			{
				return this.MajorVersion == 254;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x000B7FED File Offset: 0x000B61ED
		public bool IsSsl
		{
			get
			{
				return this == ProtocolVersion.SSLv3;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000B7FF7 File Offset: 0x000B61F7
		public bool IsTls
		{
			get
			{
				return this.MajorVersion == 3;
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000B8002 File Offset: 0x000B6202
		public ProtocolVersion GetEquivalentTLSVersion()
		{
			if (!this.IsDtls)
			{
				return this;
			}
			if (this == ProtocolVersion.DTLSv10)
			{
				return ProtocolVersion.TLSv11;
			}
			return ProtocolVersion.TLSv12;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000B8024 File Offset: 0x000B6224
		public bool IsEqualOrEarlierVersionOf(ProtocolVersion version)
		{
			if (this.MajorVersion != version.MajorVersion)
			{
				return false;
			}
			int num = version.MinorVersion - this.MinorVersion;
			if (!this.IsDtls)
			{
				return num >= 0;
			}
			return num <= 0;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000B8068 File Offset: 0x000B6268
		public bool IsLaterVersionOf(ProtocolVersion version)
		{
			if (this.MajorVersion != version.MajorVersion)
			{
				return false;
			}
			int num = version.MinorVersion - this.MinorVersion;
			if (!this.IsDtls)
			{
				return num < 0;
			}
			return num > 0;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000B80A4 File Offset: 0x000B62A4
		public override bool Equals(object other)
		{
			return this == other || (other is ProtocolVersion && this.Equals((ProtocolVersion)other));
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000B80C2 File Offset: 0x000B62C2
		public bool Equals(ProtocolVersion other)
		{
			return other != null && this.version == other.version;
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000B7FBE File Offset: 0x000B61BE
		public override int GetHashCode()
		{
			return this.version;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000B80D8 File Offset: 0x000B62D8
		public static ProtocolVersion Get(int major, int minor)
		{
			if (major != 3)
			{
				if (major != 254)
				{
					throw new TlsFatalAlert(47);
				}
				switch (minor)
				{
				case 253:
					return ProtocolVersion.DTLSv12;
				case 254:
					throw new TlsFatalAlert(47);
				case 255:
					return ProtocolVersion.DTLSv10;
				default:
					return ProtocolVersion.GetUnknownVersion(major, minor, "DTLS");
				}
			}
			else
			{
				switch (minor)
				{
				case 0:
					return ProtocolVersion.SSLv3;
				case 1:
					return ProtocolVersion.TLSv10;
				case 2:
					return ProtocolVersion.TLSv11;
				case 3:
					return ProtocolVersion.TLSv12;
				default:
					return ProtocolVersion.GetUnknownVersion(major, minor, "TLS");
				}
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000B8172 File Offset: 0x000B6372
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000B817C File Offset: 0x000B637C
		private static ProtocolVersion GetUnknownVersion(int major, int minor, string prefix)
		{
			TlsUtilities.CheckUint8(major);
			TlsUtilities.CheckUint8(minor);
			int num = major << 8 | minor;
			string str = Platform.ToUpperInvariant(Convert.ToString(65536 | num, 16).Substring(1));
			return new ProtocolVersion(num, prefix + " 0x" + str);
		}

		// Token: 0x040018D6 RID: 6358
		public static readonly ProtocolVersion SSLv3 = new ProtocolVersion(768, "SSL 3.0");

		// Token: 0x040018D7 RID: 6359
		public static readonly ProtocolVersion TLSv10 = new ProtocolVersion(769, "TLS 1.0");

		// Token: 0x040018D8 RID: 6360
		public static readonly ProtocolVersion TLSv11 = new ProtocolVersion(770, "TLS 1.1");

		// Token: 0x040018D9 RID: 6361
		public static readonly ProtocolVersion TLSv12 = new ProtocolVersion(771, "TLS 1.2");

		// Token: 0x040018DA RID: 6362
		public static readonly ProtocolVersion DTLSv10 = new ProtocolVersion(65279, "DTLS 1.0");

		// Token: 0x040018DB RID: 6363
		public static readonly ProtocolVersion DTLSv12 = new ProtocolVersion(65277, "DTLS 1.2");

		// Token: 0x040018DC RID: 6364
		private readonly int version;

		// Token: 0x040018DD RID: 6365
		private readonly string name;
	}
}
