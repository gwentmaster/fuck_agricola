using System;

namespace Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000551 RID: 1361
	public class NetscapeCertType : DerBitString
	{
		// Token: 0x06003172 RID: 12658 RVA: 0x000F0A6C File Offset: 0x000EEC6C
		public NetscapeCertType(int usage) : base(usage)
		{
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000F7BE9 File Offset: 0x000F5DE9
		public NetscapeCertType(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
		{
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000FDCC4 File Offset: 0x000FBEC4
		public override string ToString()
		{
			byte[] bytes = this.GetBytes();
			return "NetscapeCertType: 0x" + ((int)(bytes[0] & byte.MaxValue)).ToString("X");
		}

		// Token: 0x040020C3 RID: 8387
		public const int SslClient = 128;

		// Token: 0x040020C4 RID: 8388
		public const int SslServer = 64;

		// Token: 0x040020C5 RID: 8389
		public const int Smime = 32;

		// Token: 0x040020C6 RID: 8390
		public const int ObjectSigning = 16;

		// Token: 0x040020C7 RID: 8391
		public const int Reserved = 8;

		// Token: 0x040020C8 RID: 8392
		public const int SslCA = 4;

		// Token: 0x040020C9 RID: 8393
		public const int SmimeCA = 2;

		// Token: 0x040020CA RID: 8394
		public const int ObjectSigningCA = 1;
	}
}
