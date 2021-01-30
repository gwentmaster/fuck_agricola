using System;
using UnityEngine;

namespace UTNotifications
{
	// Token: 0x02000140 RID: 320
	public class WaitForInitialized : MonoBehaviour
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x00054132 File Offset: 0x00052332
		private void Start()
		{
			if (!Manager.Instance.Initialized)
			{
				Manager.Instance.OnInitialized += this.OnInitialized;
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00054162 File Offset: 0x00052362
		private void OnDestroy()
		{
			if (Manager.Instance != null)
			{
				Manager.Instance.OnInitialized -= this.OnInitialized;
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00054187 File Offset: 0x00052387
		private void OnInitialized()
		{
			base.gameObject.SetActive(true);
			Manager.Instance.OnInitialized -= this.OnInitialized;
		}
	}
}
