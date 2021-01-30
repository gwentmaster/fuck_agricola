using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001EA RID: 490
	[RequireComponent(typeof(SoftMaskScript))]
	public class CooldownEffect_SAUIM : MonoBehaviour
	{
		// Token: 0x06001269 RID: 4713 RVA: 0x000709A4 File Offset: 0x0006EBA4
		private void Start()
		{
			if (this.cooldown == null)
			{
				Debug.LogError("Missing Cooldown Button assignment");
			}
			this.sauim = base.GetComponent<SoftMaskScript>();
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000709CA File Offset: 0x0006EBCA
		private void Update()
		{
			this.sauim.CutOff = Mathf.Lerp(0f, 1f, this.cooldown.CooldownTimeElapsed / this.cooldown.CooldownTimeout);
		}

		// Token: 0x040010AB RID: 4267
		public CooldownButton cooldown;

		// Token: 0x040010AC RID: 4268
		private SoftMaskScript sauim;
	}
}
