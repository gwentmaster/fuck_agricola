using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000440 RID: 1088
	public class ParametersWithSBox : ICipherParameters
	{
		// Token: 0x060027D9 RID: 10201 RVA: 0x000C5D2D File Offset: 0x000C3F2D
		public ParametersWithSBox(ICipherParameters parameters, byte[] sBox)
		{
			this.parameters = parameters;
			this.sBox = sBox;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000C5D43 File Offset: 0x000C3F43
		public byte[] GetSBox()
		{
			return this.sBox;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000C5D4B File Offset: 0x000C3F4B
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001A63 RID: 6755
		private ICipherParameters parameters;

		// Token: 0x04001A64 RID: 6756
		private byte[] sBox;
	}
}
