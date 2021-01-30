using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001E9 RID: 489
	[RequireComponent(typeof(Image))]
	public class CooldownEffect_Image : MonoBehaviour
	{
		// Token: 0x06001264 RID: 4708 RVA: 0x000708CB File Offset: 0x0006EACB
		private void Start()
		{
			if (this.cooldown == null)
			{
				Debug.LogError("Missing Cooldown Button assignment");
			}
			this.target = base.GetComponent<Image>();
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x000708F4 File Offset: 0x0006EAF4
		private void Update()
		{
			this.target.fillAmount = Mathf.Lerp(0f, 1f, this.cooldown.CooldownTimeRemaining / this.cooldown.CooldownTimeout);
			if (this.displayText)
			{
				this.displayText.text = string.Format("{0}%", this.cooldown.CooldownPercentComplete);
			}
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00070964 File Offset: 0x0006EB64
		private void OnDisable()
		{
			if (this.displayText)
			{
				this.displayText.text = this.originalText;
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00070984 File Offset: 0x0006EB84
		private void OnEnable()
		{
			if (this.displayText)
			{
				this.originalText = this.displayText.text;
			}
		}

		// Token: 0x040010A7 RID: 4263
		public CooldownButton cooldown;

		// Token: 0x040010A8 RID: 4264
		public Text displayText;

		// Token: 0x040010A9 RID: 4265
		private Image target;

		// Token: 0x040010AA RID: 4266
		private string originalText;
	}
}
