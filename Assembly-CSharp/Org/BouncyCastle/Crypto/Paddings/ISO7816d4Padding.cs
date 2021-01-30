using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200044A RID: 1098
	public class ISO7816d4Padding : IBlockCipherPadding
	{
		// Token: 0x06002807 RID: 10247 RVA: 0x00003022 File Offset: 0x00001222
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x000C629B File Offset: 0x000C449B
		public string PaddingName
		{
			get
			{
				return "ISO7816-4";
			}
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000C62A4 File Offset: 0x000C44A4
		public int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			input[inOff] = 128;
			for (inOff++; inOff < input.Length; inOff++)
			{
				input[inOff] = 0;
			}
			return result;
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000C62D8 File Offset: 0x000C44D8
		public int PadCount(byte[] input)
		{
			int num = input.Length - 1;
			while (num > 0 && input[num] == 0)
			{
				num--;
			}
			if (input[num] != 128)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return input.Length - num;
		}
	}
}
