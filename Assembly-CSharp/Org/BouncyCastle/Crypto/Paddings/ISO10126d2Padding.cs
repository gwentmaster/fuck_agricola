using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x02000449 RID: 1097
	public class ISO10126d2Padding : IBlockCipherPadding
	{
		// Token: 0x06002802 RID: 10242 RVA: 0x000C6227 File Offset: 0x000C4427
		public void Init(SecureRandom random)
		{
			this.random = ((random != null) ? random : new SecureRandom());
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000C623A File Offset: 0x000C443A
		public string PaddingName
		{
			get
			{
				return "ISO10126-2";
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000C6244 File Offset: 0x000C4444
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length - 1)
			{
				input[inOff] = (byte)this.random.NextInt();
				inOff++;
			}
			input[inOff] = b;
			return (int)b;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000C627B File Offset: 0x000C447B
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1] & byte.MaxValue;
			if ((int)b > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return (int)b;
		}

		// Token: 0x04001A75 RID: 6773
		private SecureRandom random;
	}
}
