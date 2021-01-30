using System;
using System.Collections;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020001E7 RID: 487
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x000707A5 File Offset: 0x0006E9A5
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
			{
				yield break;
			}
			float elapsedTime = 0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += (tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
				float floatPercentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
				tweenInfo.TweenValue(floatPercentage);
				yield return null;
			}
			tweenInfo.TweenValue(1f);
			tweenInfo.Finished();
			yield break;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000707B4 File Offset: 0x0006E9B4
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000707C0 File Offset: 0x0006E9C0
		public void StartTween(T info)
		{
			if (this.m_CoroutineContainer == null)
			{
				Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
			if (!this.m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1f);
				return;
			}
			this.m_Tween = TweenRunner<!0>.Start(info);
			this.m_CoroutineContainer.StartCoroutine(this.m_Tween);
		}

		// Token: 0x040010A5 RID: 4261
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x040010A6 RID: 4262
		protected IEnumerator m_Tween;
	}
}
