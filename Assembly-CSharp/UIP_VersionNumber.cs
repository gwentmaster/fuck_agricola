using System;
using TMPro;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class UIP_VersionNumber : MonoBehaviour
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x000429E5 File Offset: 0x00040BE5
	private void Start()
	{
		if (this.m_versionNumber != null)
		{
			this.m_versionNumber.text = VersionManager.instance.GetVersionTextString();
		}
	}

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private TextMeshProUGUI m_versionNumber;
}
