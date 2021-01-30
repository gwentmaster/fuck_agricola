using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000176 RID: 374
	[AddComponentMenu("UI/Extensions/Cooldown Button")]
	public class CooldownButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0005C5C6 File Offset: 0x0005A7C6
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x0005C5CE File Offset: 0x0005A7CE
		public float CooldownTimeout
		{
			get
			{
				return this.cooldownTimeout;
			}
			set
			{
				this.cooldownTimeout = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0005C5D7 File Offset: 0x0005A7D7
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x0005C5DF File Offset: 0x0005A7DF
		public float CooldownSpeed
		{
			get
			{
				return this.cooldownSpeed;
			}
			set
			{
				this.cooldownSpeed = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0005C5E8 File Offset: 0x0005A7E8
		public bool CooldownInEffect
		{
			get
			{
				return this.cooldownInEffect;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0005C5F8 File Offset: 0x0005A7F8
		public bool CooldownActive
		{
			get
			{
				return this.cooldownActive;
			}
			set
			{
				this.cooldownActive = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0005C601 File Offset: 0x0005A801
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0005C609 File Offset: 0x0005A809
		public float CooldownTimeElapsed
		{
			get
			{
				return this.cooldownTimeElapsed;
			}
			set
			{
				this.cooldownTimeElapsed = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0005C612 File Offset: 0x0005A812
		public float CooldownTimeRemaining
		{
			get
			{
				return this.cooldownTimeRemaining;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0005C61A File Offset: 0x0005A81A
		public int CooldownPercentRemaining
		{
			get
			{
				return this.cooldownPercentRemaining;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0005C622 File Offset: 0x0005A822
		public int CooldownPercentComplete
		{
			get
			{
				return this.cooldownPercentComplete;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0005C62C File Offset: 0x0005A82C
		private void Update()
		{
			if (this.CooldownActive)
			{
				this.cooldownTimeRemaining -= Time.deltaTime * this.cooldownSpeed;
				this.cooldownTimeElapsed = this.CooldownTimeout - this.CooldownTimeRemaining;
				if (this.cooldownTimeRemaining < 0f)
				{
					this.StopCooldown();
					return;
				}
				this.cooldownPercentRemaining = (int)(100f * this.cooldownTimeRemaining * this.CooldownTimeout / 100f);
				this.cooldownPercentComplete = (int)((this.CooldownTimeout - this.cooldownTimeRemaining) / this.CooldownTimeout * 100f);
			}
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0005C6C5 File Offset: 0x0005A8C5
		public void PauseCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = false;
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0005C6D6 File Offset: 0x0005A8D6
		public void RestartCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = true;
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0005C6E8 File Offset: 0x0005A8E8
		public void StopCooldown()
		{
			this.cooldownTimeElapsed = this.CooldownTimeout;
			this.cooldownTimeRemaining = 0f;
			this.cooldownPercentRemaining = 0;
			this.cooldownPercentComplete = 100;
			this.cooldownActive = (this.cooldownInEffect = false);
			if (this.OnCoolDownFinish != null)
			{
				this.OnCoolDownFinish.Invoke(this.buttonSource.button);
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0005C74C File Offset: 0x0005A94C
		public void CancelCooldown()
		{
			this.cooldownActive = (this.cooldownInEffect = false);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0005C76C File Offset: 0x0005A96C
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.buttonSource = eventData;
			if (this.CooldownInEffect && this.OnButtonClickDuringCooldown != null)
			{
				this.OnButtonClickDuringCooldown.Invoke(eventData.button);
			}
			if (!this.CooldownInEffect)
			{
				if (this.OnCooldownStart != null)
				{
					this.OnCooldownStart.Invoke(eventData.button);
				}
				this.cooldownTimeRemaining = this.cooldownTimeout;
				this.cooldownActive = (this.cooldownInEffect = true);
			}
		}

		// Token: 0x04000E1F RID: 3615
		[SerializeField]
		private float cooldownTimeout;

		// Token: 0x04000E20 RID: 3616
		[SerializeField]
		private float cooldownSpeed = 1f;

		// Token: 0x04000E21 RID: 3617
		[SerializeField]
		[ReadOnly]
		private bool cooldownActive;

		// Token: 0x04000E22 RID: 3618
		[SerializeField]
		[ReadOnly]
		private bool cooldownInEffect;

		// Token: 0x04000E23 RID: 3619
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeElapsed;

		// Token: 0x04000E24 RID: 3620
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeRemaining;

		// Token: 0x04000E25 RID: 3621
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentRemaining;

		// Token: 0x04000E26 RID: 3622
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentComplete;

		// Token: 0x04000E27 RID: 3623
		private PointerEventData buttonSource;

		// Token: 0x04000E28 RID: 3624
		[Tooltip("Event that fires when a button is initially pressed down")]
		public CooldownButton.CooldownButtonEvent OnCooldownStart;

		// Token: 0x04000E29 RID: 3625
		[Tooltip("Event that fires when a button is released")]
		public CooldownButton.CooldownButtonEvent OnButtonClickDuringCooldown;

		// Token: 0x04000E2A RID: 3626
		[Tooltip("Event that continually fires while a button is held down")]
		public CooldownButton.CooldownButtonEvent OnCoolDownFinish;

		// Token: 0x02000842 RID: 2114
		[Serializable]
		public class CooldownButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
		}
	}
}
