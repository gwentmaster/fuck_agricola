using System;

namespace LitJson
{
	// Token: 0x02000271 RID: 625
	public enum JsonToken
	{
		// Token: 0x04001322 RID: 4898
		None,
		// Token: 0x04001323 RID: 4899
		ObjectStart,
		// Token: 0x04001324 RID: 4900
		PropertyName,
		// Token: 0x04001325 RID: 4901
		ObjectEnd,
		// Token: 0x04001326 RID: 4902
		ArrayStart,
		// Token: 0x04001327 RID: 4903
		ArrayEnd,
		// Token: 0x04001328 RID: 4904
		Int,
		// Token: 0x04001329 RID: 4905
		Long,
		// Token: 0x0400132A RID: 4906
		Double,
		// Token: 0x0400132B RID: 4907
		String,
		// Token: 0x0400132C RID: 4908
		Boolean,
		// Token: 0x0400132D RID: 4909
		Null
	}
}
