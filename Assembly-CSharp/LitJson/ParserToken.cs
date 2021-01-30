using System;

namespace LitJson
{
	// Token: 0x02000278 RID: 632
	internal enum ParserToken
	{
		// Token: 0x04001366 RID: 4966
		None = 65536,
		// Token: 0x04001367 RID: 4967
		Number,
		// Token: 0x04001368 RID: 4968
		True,
		// Token: 0x04001369 RID: 4969
		False,
		// Token: 0x0400136A RID: 4970
		Null,
		// Token: 0x0400136B RID: 4971
		CharSeq,
		// Token: 0x0400136C RID: 4972
		Char,
		// Token: 0x0400136D RID: 4973
		Text,
		// Token: 0x0400136E RID: 4974
		Object,
		// Token: 0x0400136F RID: 4975
		ObjectPrime,
		// Token: 0x04001370 RID: 4976
		Pair,
		// Token: 0x04001371 RID: 4977
		PairRest,
		// Token: 0x04001372 RID: 4978
		Array,
		// Token: 0x04001373 RID: 4979
		ArrayPrime,
		// Token: 0x04001374 RID: 4980
		Value,
		// Token: 0x04001375 RID: 4981
		ValueRest,
		// Token: 0x04001376 RID: 4982
		String,
		// Token: 0x04001377 RID: 4983
		End,
		// Token: 0x04001378 RID: 4984
		Epsilon
	}
}
