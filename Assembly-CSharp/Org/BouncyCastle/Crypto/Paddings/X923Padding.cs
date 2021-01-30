using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200044E RID: 1102
	public class X923Padding : IBlockCipherPadding
	{
		// Token: 0x0600281E RID: 10270 RVA: 0x000C6782 File Offset: 0x000C4982
		public void Init(SecureRandom random)
		{
			this.random = random;
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600281F RID: 10271 RVA: 0x000C678B File Offset: 0x000C498B
		public string PaddingName
		{
			get
			{
				return "X9.23";
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000C6794 File Offset: 0x000C4994
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length - 1)
			{
				if (this.random == null)
				{
					input[inOff] = 0;
				}
				else
				{
					input[inOff] = (byte)this.random.NextInt();
				}
				inOff++;
			}
			input[inOff] = b;
			return (int)b;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000C627B File Offset: 0x000C447B
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1] & byte.MaxValue;
			if ((int)b > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return (int)b;
		}

		// Token: 0x04001A77 RID: 6775
		private SecureRandom random;
	}
}
