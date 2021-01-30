using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200044F RID: 1103
	public class ZeroBytePadding : IBlockCipherPadding
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000C67D9 File Offset: 0x000C49D9
		public string PaddingName
		{
			get
			{
				return "ZeroBytePadding";
			}
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x00003022 File Offset: 0x00001222
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000C67E0 File Offset: 0x000C49E0
		public int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			while (inOff < input.Length)
			{
				input[inOff] = 0;
				inOff++;
			}
			return result;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000C6808 File Offset: 0x000C4A08
		public int PadCount(byte[] input)
		{
			int num = input.Length;
			while (num > 0 && input[num - 1] == 0)
			{
				num--;
			}
			return input.Length - num;
		}
	}
}
