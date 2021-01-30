using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UTNotifications
{
	// Token: 0x0200013C RID: 316
	public static class SampleUtils
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00053EB0 File Offset: 0x000520B0
		public static string UniqueName(Transform transform)
		{
			if (transform == null)
			{
				return SceneManager.GetActiveScene().name;
			}
			return SampleUtils.UniqueName(transform.parent) + "." + transform.name;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00053EEF File Offset: 0x000520EF
		public static string GenerateDeviceUniqueIdentifier()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}
	}
}
