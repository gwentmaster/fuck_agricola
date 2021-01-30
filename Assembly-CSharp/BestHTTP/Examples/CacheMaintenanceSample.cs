using System;
using BestHTTP.Caching;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000581 RID: 1409
	public sealed class CacheMaintenanceSample : MonoBehaviour
	{
		// Token: 0x060033AE RID: 13230 RVA: 0x0010575F File Offset: 0x0010395F
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Delete cached entities older then", Array.Empty<GUILayoutOption>());
				GUILayout.Label(this.value.ToString(), new GUILayoutOption[]
				{
					GUILayout.MinWidth(50f)
				});
				this.value = (int)GUILayout.HorizontalSlider((float)this.value, 1f, 60f, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.Space(10f);
				this.deleteOlderType = (CacheMaintenanceSample.DeleteOlderTypes)GUILayout.SelectionGrid((int)this.deleteOlderType, new string[]
				{
					"Days",
					"Hours",
					"Mins",
					"Secs"
				}, 4, Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Cache Size (bytes): ", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				});
				GUILayout.Label(this.maxCacheSize.ToString("N0"), new GUILayoutOption[]
				{
					GUILayout.Width(70f)
				});
				this.maxCacheSize = (int)GUILayout.HorizontalSlider((float)this.maxCacheSize, 1024f, 10485760f, Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
				if (GUILayout.Button("Maintenance", Array.Empty<GUILayoutOption>()))
				{
					TimeSpan deleteOlder = TimeSpan.FromDays(14.0);
					switch (this.deleteOlderType)
					{
					case CacheMaintenanceSample.DeleteOlderTypes.Days:
						deleteOlder = TimeSpan.FromDays((double)this.value);
						break;
					case CacheMaintenanceSample.DeleteOlderTypes.Hours:
						deleteOlder = TimeSpan.FromHours((double)this.value);
						break;
					case CacheMaintenanceSample.DeleteOlderTypes.Mins:
						deleteOlder = TimeSpan.FromMinutes((double)this.value);
						break;
					case CacheMaintenanceSample.DeleteOlderTypes.Secs:
						deleteOlder = TimeSpan.FromSeconds((double)this.value);
						break;
					}
					HTTPCacheService.BeginMaintainence(new HTTPCacheMaintananceParams(deleteOlder, (ulong)((long)this.maxCacheSize)));
				}
			});
		}

		// Token: 0x040021E9 RID: 8681
		private CacheMaintenanceSample.DeleteOlderTypes deleteOlderType = CacheMaintenanceSample.DeleteOlderTypes.Secs;

		// Token: 0x040021EA RID: 8682
		private int value = 10;

		// Token: 0x040021EB RID: 8683
		private int maxCacheSize = 5242880;

		// Token: 0x020008F8 RID: 2296
		private enum DeleteOlderTypes
		{
			// Token: 0x0400301F RID: 12319
			Days,
			// Token: 0x04003020 RID: 12320
			Hours,
			// Token: 0x04003021 RID: 12321
			Mins,
			// Token: 0x04003022 RID: 12322
			Secs
		}
	}
}
