using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F3 RID: 1779
	public abstract class KeyValueStore
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x001330C9 File Offset: 0x001312C9
		// (set) Token: 0x06003ED8 RID: 16088 RVA: 0x001330D0 File Offset: 0x001312D0
		public static KeyValueStore Instance { get; set; }

		// Token: 0x06003ED9 RID: 16089 RVA: 0x001330D8 File Offset: 0x001312D8
		public static void ResetInstance()
		{
			KeyValueStore.Instance = null;
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x001330E0 File Offset: 0x001312E0
		public static void DeleteAll()
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._DeleteAll();
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x001330F6 File Offset: 0x001312F6
		public static void DeleteKey(string key)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._DeleteKey(key);
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0013310D File Offset: 0x0013130D
		public static bool HasKey(string key)
		{
			KeyValueStore._CheckInstance();
			return KeyValueStore.Instance != null && KeyValueStore.Instance._HasKey(key);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00133128 File Offset: 0x00131328
		public static void Save()
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._Save();
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0013313E File Offset: 0x0013133E
		public static int GetInt(string key, int defaultValue = 0)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetInt(key, defaultValue);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0013315A File Offset: 0x0013135A
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetFloat(key, defaultValue);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00133176 File Offset: 0x00131376
		public static string GetString(string key, string defaultValue = "")
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetString(key, defaultValue);
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x00133192 File Offset: 0x00131392
		public static void SetInt(string key, int value)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._SetInt(key, value);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x001331AA File Offset: 0x001313AA
		public static void SetFloat(string key, float value)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._SetFloat(key, value);
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x001331C2 File Offset: 0x001313C2
		public static void SetString(string key, string value)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._SetString(key, value);
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x001331DA File Offset: 0x001313DA
		private static void _CheckInstance()
		{
			if (KeyValueStore.Instance == null)
			{
				AsmoLogger.Trace("KeyValueStore", "KeyValueStore is not set", null);
			}
		}

		// Token: 0x06003EE5 RID: 16101
		protected abstract void _DeleteAll();

		// Token: 0x06003EE6 RID: 16102
		protected abstract void _DeleteKey(string key);

		// Token: 0x06003EE7 RID: 16103
		protected abstract bool _HasKey(string key);

		// Token: 0x06003EE8 RID: 16104
		protected abstract void _Save();

		// Token: 0x06003EE9 RID: 16105
		protected abstract int _GetInt(string key, int defaultValue);

		// Token: 0x06003EEA RID: 16106
		protected abstract float _GetFloat(string key, float defaultValue);

		// Token: 0x06003EEB RID: 16107
		protected abstract string _GetString(string key, string defaultValue);

		// Token: 0x06003EEC RID: 16108
		protected abstract void _SetInt(string key, int value);

		// Token: 0x06003EED RID: 16109
		protected abstract void _SetFloat(string key, float value);

		// Token: 0x06003EEE RID: 16110
		protected abstract void _SetString(string key, string value);

		// Token: 0x04002873 RID: 10355
		private const string _kModuleName = "KeyValueStore";
	}
}
