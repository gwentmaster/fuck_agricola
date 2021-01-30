using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043E RID: 1086
	public class ParametersWithIV : ICipherParameters
	{
		// Token: 0x060027D0 RID: 10192 RVA: 0x000C5C73 File Offset: 0x000C3E73
		public ParametersWithIV(ICipherParameters parameters, byte[] iv) : this(parameters, iv, 0, iv.Length)
		{
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000C5C81 File Offset: 0x000C3E81
		public ParametersWithIV(ICipherParameters parameters, byte[] iv, int ivOff, int ivLen)
		{
			if (iv == null)
			{
				throw new ArgumentNullException("iv");
			}
			this.parameters = parameters;
			this.iv = new byte[ivLen];
			Array.Copy(iv, ivOff, this.iv, 0, ivLen);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000C5CBB File Offset: 0x000C3EBB
		public byte[] GetIV()
		{
			return (byte[])this.iv.Clone();
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000C5CCD File Offset: 0x000C3ECD
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001A5F RID: 6751
		private readonly ICipherParameters parameters;

		// Token: 0x04001A60 RID: 6752
		private readonly byte[] iv;
	}
}
