using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020001E5 RID: 485
	public struct FloatTween : ITweenValue
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x000706D7 File Offset: 0x0006E8D7
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x000706DF File Offset: 0x0006E8DF
		public float startFloat
		{
			get
			{
				return this.m_StartFloat;
			}
			set
			{
				this.m_StartFloat = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x000706E8 File Offset: 0x0006E8E8
		// (set) Token: 0x0600124A RID: 4682 RVA: 0x000706F0 File Offset: 0x0006E8F0
		public float targetFloat
		{
			get
			{
				return this.m_TargetFloat;
			}
			set
			{
				this.m_TargetFloat = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x000706F9 File Offset: 0x0006E8F9
		// (set) Token: 0x0600124C RID: 4684 RVA: 0x00070701 File Offset: 0x0006E901
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x0007070A File Offset: 0x0006E90A
		// (set) Token: 0x0600124E RID: 4686 RVA: 0x00070712 File Offset: 0x0006E912
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0007071B File Offset: 0x0006E91B
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Mathf.Lerp(this.m_StartFloat, this.m_TargetFloat, floatPercentage));
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00070743 File Offset: 0x0006E943
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new FloatTween.FloatTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00070764 File Offset: 0x0006E964
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new FloatTween.FloatFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0007070A File Offset: 0x0006E90A
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000706F9 File Offset: 0x0006E8F9
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00070785 File Offset: 0x0006E985
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00070790 File Offset: 0x0006E990
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x0400109F RID: 4255
		private float m_StartFloat;

		// Token: 0x040010A0 RID: 4256
		private float m_TargetFloat;

		// Token: 0x040010A1 RID: 4257
		private float m_Duration;

		// Token: 0x040010A2 RID: 4258
		private bool m_IgnoreTimeScale;

		// Token: 0x040010A3 RID: 4259
		private FloatTween.FloatTweenCallback m_Target;

		// Token: 0x040010A4 RID: 4260
		private FloatTween.FloatFinishCallback m_Finish;

		// Token: 0x0200086E RID: 2158
		public class FloatTweenCallback : UnityEvent<float>
		{
		}

		// Token: 0x0200086F RID: 2159
		public class FloatFinishCallback : UnityEvent
		{
		}
	}
}
