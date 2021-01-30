using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D8 RID: 728
	public interface IPolynomialExtensionField : IExtensionField, IFiniteField
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600190A RID: 6410
		IPolynomial MinimalPolynomial { get; }
	}
}
