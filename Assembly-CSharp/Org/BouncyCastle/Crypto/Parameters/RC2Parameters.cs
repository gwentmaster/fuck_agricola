using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000442 RID: 1090
	public class RC2Parameters : KeyParameter
	{
		// Token: 0x060027E0 RID: 10208 RVA: 0x000C5D9D File Offset: 0x000C3F9D
		public RC2Parameters(byte[] key) : this(key, (key.Length > 128) ? 1024 : (key.Length * 8))
		{
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000C5DBC File Offset: 0x000C3FBC
		public RC2Parameters(byte[] key, int keyOff, int keyLen) : this(key, keyOff, keyLen, (keyLen > 128) ? 1024 : (keyLen * 8))
		{
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000C5DD9 File Offset: 0x000C3FD9
		public RC2Parameters(byte[] key, int bits) : base(key)
		{
			this.bits = bits;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000C5DE9 File Offset: 0x000C3FE9
		public RC2Parameters(byte[] key, int keyOff, int keyLen, int bits) : base(key, keyOff, keyLen)
		{
			this.bits = bits;
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x000C5DFC File Offset: 0x000C3FFC
		public int EffectiveKeyBits
		{
			get
			{
				return this.bits;
			}
		}

		// Token: 0x04001A67 RID: 6759
		private readonly int bits;
	}
}
