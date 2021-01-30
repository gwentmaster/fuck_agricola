using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200054B RID: 1355
	public class OcspResponseStatus : DerEnumerated
	{
		// Token: 0x06003156 RID: 12630 RVA: 0x000F67A1 File Offset: 0x000F49A1
		public OcspResponseStatus(int value) : base(value)
		{
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000F67AA File Offset: 0x000F49AA
		public OcspResponseStatus(DerEnumerated value) : base(value.Value.IntValue)
		{
		}

		// Token: 0x0400206F RID: 8303
		public const int Successful = 0;

		// Token: 0x04002070 RID: 8304
		public const int MalformedRequest = 1;

		// Token: 0x04002071 RID: 8305
		public const int InternalError = 2;

		// Token: 0x04002072 RID: 8306
		public const int TryLater = 3;

		// Token: 0x04002073 RID: 8307
		public const int SignatureRequired = 5;

		// Token: 0x04002074 RID: 8308
		public const int Unauthorized = 6;
	}
}
