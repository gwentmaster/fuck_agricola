using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200044D RID: 1101
	public class TbcPadding : IBlockCipherPadding
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x000C66F2 File Offset: 0x000C48F2
		public string PaddingName
		{
			get
			{
				return "TBC";
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Init(SecureRandom random)
		{
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000C66FC File Offset: 0x000C48FC
		public virtual int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			byte b;
			if (inOff > 0)
			{
				b = (((input[inOff - 1] & 1) == 0) ? byte.MaxValue : 0);
			}
			else
			{
				b = (((input[input.Length - 1] & 1) == 0) ? byte.MaxValue : 0);
			}
			while (inOff < input.Length)
			{
				input[inOff] = b;
				inOff++;
			}
			return result;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000C6750 File Offset: 0x000C4950
		public virtual int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1];
			int num = input.Length - 1;
			while (num > 0 && input[num - 1] == b)
			{
				num--;
			}
			return input.Length - num;
		}
	}
}
