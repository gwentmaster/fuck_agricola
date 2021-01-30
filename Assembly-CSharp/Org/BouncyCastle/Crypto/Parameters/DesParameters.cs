using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000421 RID: 1057
	public class DesParameters : KeyParameter
	{
		// Token: 0x0600272F RID: 10031 RVA: 0x000C499A File Offset: 0x000C2B9A
		public DesParameters(byte[] key) : base(key)
		{
			if (DesParameters.IsWeakKey(key))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000C49B6 File Offset: 0x000C2BB6
		public DesParameters(byte[] key, int keyOff, int keyLen) : base(key, keyOff, keyLen)
		{
			if (DesParameters.IsWeakKey(key, keyOff))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000C49D8 File Offset: 0x000C2BD8
		public static bool IsWeakKey(byte[] key, int offset)
		{
			if (key.Length - offset < 8)
			{
				throw new ArgumentException("key material too short.");
			}
			for (int i = 0; i < 16; i++)
			{
				bool flag = false;
				for (int j = 0; j < 8; j++)
				{
					if (key[j + offset] != DesParameters.DES_weak_keys[i * 8 + j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000C4A2F File Offset: 0x000C2C2F
		public static bool IsWeakKey(byte[] key)
		{
			return DesParameters.IsWeakKey(key, 0);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000C4A38 File Offset: 0x000C2C38
		public static byte SetOddParity(byte b)
		{
			uint num = (uint)(b ^ 1);
			num ^= num >> 4;
			num ^= num >> 2;
			num ^= num >> 1;
			num &= 1U;
			return (byte)((uint)b ^ num);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000C4A64 File Offset: 0x000C2C64
		public static void SetOddParity(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = DesParameters.SetOddParity(bytes[i]);
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000C4A8C File Offset: 0x000C2C8C
		public static void SetOddParity(byte[] bytes, int off, int len)
		{
			for (int i = 0; i < len; i++)
			{
				bytes[off + i] = DesParameters.SetOddParity(bytes[off + i]);
			}
		}

		// Token: 0x04001A24 RID: 6692
		public const int DesKeyLength = 8;

		// Token: 0x04001A25 RID: 6693
		private const int N_DES_WEAK_KEYS = 16;

		// Token: 0x04001A26 RID: 6694
		private static readonly byte[] DES_weak_keys = new byte[]
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			31,
			31,
			31,
			31,
			14,
			14,
			14,
			14,
			224,
			224,
			224,
			224,
			241,
			241,
			241,
			241,
			254,
			254,
			254,
			254,
			254,
			254,
			254,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			31,
			224,
			31,
			224,
			14,
			241,
			14,
			241,
			1,
			224,
			1,
			224,
			1,
			241,
			1,
			241,
			31,
			254,
			31,
			254,
			14,
			254,
			14,
			254,
			1,
			31,
			1,
			31,
			1,
			14,
			1,
			14,
			224,
			254,
			224,
			254,
			241,
			254,
			241,
			254,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			1,
			224,
			31,
			224,
			31,
			241,
			14,
			241,
			14,
			224,
			1,
			224,
			1,
			241,
			1,
			241,
			1,
			254,
			31,
			254,
			31,
			254,
			14,
			254,
			14,
			31,
			1,
			31,
			1,
			14,
			1,
			14,
			1,
			254,
			224,
			254,
			224,
			254,
			241,
			254,
			241
		};
	}
}
