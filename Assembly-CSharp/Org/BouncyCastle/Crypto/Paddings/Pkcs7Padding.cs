using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200044C RID: 1100
	public class Pkcs7Padding : IBlockCipherPadding
	{
		// Token: 0x06002814 RID: 10260 RVA: 0x00003022 File Offset: 0x00001222
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000C6674 File Offset: 0x000C4874
		public string PaddingName
		{
			get
			{
				return "PKCS7";
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000C667C File Offset: 0x000C487C
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length)
			{
				input[inOff] = b;
				inOff++;
			}
			return (int)b;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000C66A4 File Offset: 0x000C48A4
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1];
			int num = (int)b;
			if (num < 1 || num > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			for (int i = 2; i <= num; i++)
			{
				if (input[input.Length - i] != b)
				{
					throw new InvalidCipherTextException("pad block corrupted");
				}
			}
			return num;
		}
	}
}
