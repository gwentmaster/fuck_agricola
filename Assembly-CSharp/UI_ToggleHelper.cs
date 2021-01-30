using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
public class UI_ToggleHelper : MonoBehaviour
{
	// Token: 0x06000A22 RID: 2594 RVA: 0x000436F8 File Offset: 0x000418F8
	private void Start()
	{
		this.ToggleState();
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00043700 File Offset: 0x00041900
	public void ToggleState()
	{
		if (this.toggle.isOn)
		{
			if (this.toToggle != null)
			{
				this.toToggle.SetActive(true);
			}
			if (this.text != null)
			{
				this.text.color = this.colorOn;
				return;
			}
		}
		else if (!this.toggle.isOn)
		{
			if (this.toToggle != null)
			{
				this.toToggle.SetActive(false);
			}
			if (this.text != null)
			{
				this.text.color = this.colorOff;
			}
		}
	}

	// Token: 0x04000AAD RID: 2733
	public Toggle toggle;

	// Token: 0x04000AAE RID: 2734
	public GameObject toToggle;

	// Token: 0x04000AAF RID: 2735
	public TextMeshProUGUI text;

	// Token: 0x04000AB0 RID: 2736
	public Color colorOn = new Color(1f, 1f, 1f);

	// Token: 0x04000AB1 RID: 2737
	public Color colorOff = new Color(0.5f, 0.5f, 0.5f);
}
