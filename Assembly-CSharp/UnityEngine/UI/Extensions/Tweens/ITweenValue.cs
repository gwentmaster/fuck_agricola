using System;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020001E6 RID: 486
	internal interface ITweenValue
	{
		// Token: 0x06001256 RID: 4694
		void TweenValue(float floatPercentage);

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06001257 RID: 4695
		bool ignoreTimeScale { get; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06001258 RID: 4696
		float duration { get; }

		// Token: 0x06001259 RID: 4697
		bool ValidTarget();

		// Token: 0x0600125A RID: 4698
		void Finished();
	}
}
