using System;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F5 RID: 1781
	public class PlayerPrefsKeyValueStore : KeyValueStore
	{
		// Token: 0x06003EFB RID: 16123 RVA: 0x00133337 File Offset: 0x00131537
		protected override void _DeleteAll()
		{
			PlayerPrefs.DeleteAll();
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0013333E File Offset: 0x0013153E
		protected override void _DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00133346 File Offset: 0x00131546
		protected override bool _HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0013334E File Offset: 0x0013154E
		protected override void _Save()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00133355 File Offset: 0x00131555
		protected override int _GetInt(string key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0013335E File Offset: 0x0013155E
		protected override float _GetFloat(string key, float defaultValue)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x00133367 File Offset: 0x00131567
		protected override string _GetString(string key, string defaultValue)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x00133370 File Offset: 0x00131570
		protected override void _SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00133379 File Offset: 0x00131579
		protected override void _SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x00133382 File Offset: 0x00131582
		protected override void _SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}
	}
}
