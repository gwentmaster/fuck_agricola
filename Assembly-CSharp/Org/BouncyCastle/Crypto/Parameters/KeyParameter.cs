using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043B RID: 1083
	public class KeyParameter : ICipherParameters
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x000C5AA3 File Offset: 0x000C3CA3
		public KeyParameter(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.key = (byte[])key.Clone();
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000C5ACC File Offset: 0x000C3CCC
		public KeyParameter(byte[] key, int keyOff, int keyLen)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (keyOff < 0 || keyOff > key.Length)
			{
				throw new ArgumentOutOfRangeException("keyOff");
			}
			if (keyLen < 0 || keyOff + keyLen > key.Length)
			{
				throw new ArgumentOutOfRangeException("keyLen");
			}
			this.key = new byte[keyLen];
			Array.Copy(key, keyOff, this.key, 0, keyLen);
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000C5B34 File Offset: 0x000C3D34
		public byte[] GetKey()
		{
			return (byte[])this.key.Clone();
		}

		// Token: 0x04001A59 RID: 6745
		private readonly byte[] key;
	}
}
