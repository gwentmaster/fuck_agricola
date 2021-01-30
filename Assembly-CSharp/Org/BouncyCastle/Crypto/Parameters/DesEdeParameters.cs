using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000420 RID: 1056
	public class DesEdeParameters : DesParameters
	{
		// Token: 0x06002726 RID: 10022 RVA: 0x000C4820 File Offset: 0x000C2A20
		private static byte[] FixKey(byte[] key, int keyOff, int keyLen)
		{
			byte[] array = new byte[24];
			if (keyLen != 16)
			{
				if (keyLen != 24)
				{
					throw new ArgumentException("Bad length for DESede key: " + keyLen, "keyLen");
				}
				Array.Copy(key, keyOff, array, 0, 24);
			}
			else
			{
				Array.Copy(key, keyOff, array, 0, 16);
				Array.Copy(key, keyOff, array, 16, 8);
			}
			if (DesEdeParameters.IsWeakKey(array))
			{
				throw new ArgumentException("attempt to create weak DESede key");
			}
			return array;
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000C4895 File Offset: 0x000C2A95
		public DesEdeParameters(byte[] key) : base(DesEdeParameters.FixKey(key, 0, key.Length))
		{
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000C48A7 File Offset: 0x000C2AA7
		public DesEdeParameters(byte[] key, int keyOff, int keyLen) : base(DesEdeParameters.FixKey(key, keyOff, keyLen))
		{
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000C48B8 File Offset: 0x000C2AB8
		public static bool IsWeakKey(byte[] key, int offset, int length)
		{
			for (int i = offset; i < length; i += 8)
			{
				if (DesParameters.IsWeakKey(key, i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000C48DD File Offset: 0x000C2ADD
		public new static bool IsWeakKey(byte[] key, int offset)
		{
			return DesEdeParameters.IsWeakKey(key, offset, key.Length - offset);
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000C48EB File Offset: 0x000C2AEB
		public new static bool IsWeakKey(byte[] key)
		{
			return DesEdeParameters.IsWeakKey(key, 0, key.Length);
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000C48F7 File Offset: 0x000C2AF7
		public static bool IsRealEdeKey(byte[] key, int offset)
		{
			if (key.Length != 16)
			{
				return DesEdeParameters.IsReal3Key(key, offset);
			}
			return DesEdeParameters.IsReal2Key(key, offset);
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000C4910 File Offset: 0x000C2B10
		public static bool IsReal2Key(byte[] key, int offset)
		{
			bool flag = false;
			for (int num = offset; num != offset + 8; num++)
			{
				flag |= (key[num] != key[num + 8]);
			}
			return flag;
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000C4940 File Offset: 0x000C2B40
		public static bool IsReal3Key(byte[] key, int offset)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num = offset; num != offset + 8; num++)
			{
				flag |= (key[num] != key[num + 8]);
				flag2 |= (key[num] != key[num + 16]);
				flag3 |= (key[num + 8] != key[num + 16]);
			}
			return flag && flag2 && flag3;
		}

		// Token: 0x04001A23 RID: 6691
		public const int DesEdeKeyLength = 24;
	}
}
