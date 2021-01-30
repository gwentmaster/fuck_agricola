using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D5 RID: 725
	public interface IExtensionField : IFiniteField
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06001904 RID: 6404
		IFiniteField Subfield { get; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001905 RID: 6405
		int Degree { get; }
	}
}
