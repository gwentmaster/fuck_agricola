using System;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051A RID: 1306
	public abstract class X9ECParametersHolder
	{
		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x000F5A84 File Offset: 0x000F3C84
		public X9ECParameters Parameters
		{
			get
			{
				X9ECParameters result;
				lock (this)
				{
					if (this.parameters == null)
					{
						this.parameters = this.CreateParameters();
					}
					result = this.parameters;
				}
				return result;
			}
		}

		// Token: 0x06002FCA RID: 12234
		protected abstract X9ECParameters CreateParameters();

		// Token: 0x04001E69 RID: 7785
		private X9ECParameters parameters;
	}
}
