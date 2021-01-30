using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043F RID: 1087
	public class ParametersWithRandom : ICipherParameters
	{
		// Token: 0x060027D4 RID: 10196 RVA: 0x000C5CD5 File Offset: 0x000C3ED5
		public ParametersWithRandom(ICipherParameters parameters, SecureRandom random)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			this.parameters = parameters;
			this.random = random;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000C5D07 File Offset: 0x000C3F07
		public ParametersWithRandom(ICipherParameters parameters) : this(parameters, new SecureRandom())
		{
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000C5D15 File Offset: 0x000C3F15
		[Obsolete("Use Random property instead")]
		public SecureRandom GetRandom()
		{
			return this.Random;
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000C5D1D File Offset: 0x000C3F1D
		public SecureRandom Random
		{
			get
			{
				return this.random;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060027D8 RID: 10200 RVA: 0x000C5D25 File Offset: 0x000C3F25
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001A61 RID: 6753
		private readonly ICipherParameters parameters;

		// Token: 0x04001A62 RID: 6754
		private readonly SecureRandom random;
	}
}
