using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000441 RID: 1089
	public class ParametersWithSalt : ICipherParameters
	{
		// Token: 0x060027DC RID: 10204 RVA: 0x000C5D53 File Offset: 0x000C3F53
		public ParametersWithSalt(ICipherParameters parameters, byte[] salt) : this(parameters, salt, 0, salt.Length)
		{
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000C5D61 File Offset: 0x000C3F61
		public ParametersWithSalt(ICipherParameters parameters, byte[] salt, int saltOff, int saltLen)
		{
			this.salt = new byte[saltLen];
			this.parameters = parameters;
			Array.Copy(salt, saltOff, this.salt, 0, saltLen);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000C5D8D File Offset: 0x000C3F8D
		public byte[] GetSalt()
		{
			return this.salt;
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000C5D95 File Offset: 0x000C3F95
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001A65 RID: 6757
		private byte[] salt;

		// Token: 0x04001A66 RID: 6758
		private ICipherParameters parameters;
	}
}
