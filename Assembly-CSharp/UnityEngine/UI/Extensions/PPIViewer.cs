using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C3 RID: 451
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Extensions/PPIViewer")]
	public class PPIViewer : MonoBehaviour
	{
		// Token: 0x06001166 RID: 4454 RVA: 0x0006CBBA File Offset: 0x0006ADBA
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0006CBC8 File Offset: 0x0006ADC8
		private void Start()
		{
			if (this.label != null)
			{
				this.label.text = "PPI: " + Screen.dpi.ToString();
			}
		}

		// Token: 0x04001002 RID: 4098
		private Text label;
	}
}
