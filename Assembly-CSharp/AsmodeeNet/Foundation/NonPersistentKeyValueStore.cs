using System;
using System.Collections.Generic;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F4 RID: 1780
	public class NonPersistentKeyValueStore : KeyValueStore
	{
		// Token: 0x06003EF0 RID: 16112 RVA: 0x001331F3 File Offset: 0x001313F3
		protected override void _DeleteAll()
		{
			this._ints.Clear();
			this._floats.Clear();
			this._strings.Clear();
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x00133216 File Offset: 0x00131416
		protected override void _DeleteKey(string key)
		{
			this._ints.Remove(key);
			this._floats.Remove(key);
			this._strings.Remove(key);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0013323F File Offset: 0x0013143F
		protected override bool _HasKey(string key)
		{
			return this._ints.ContainsKey(key) || this._floats.ContainsKey(key) || this._strings.ContainsKey(key);
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00003022 File Offset: 0x00001222
		protected override void _Save()
		{
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0013326C File Offset: 0x0013146C
		protected override int _GetInt(string key, int defaultValue)
		{
			int result;
			if (this._ints.TryGetValue(key, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0013328C File Offset: 0x0013148C
		protected override float _GetFloat(string key, float defaultValue)
		{
			float result;
			if (this._floats.TryGetValue(key, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x001332AC File Offset: 0x001314AC
		protected override string _GetString(string key, string defaultValue)
		{
			string result;
			if (this._strings.TryGetValue(key, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x001332CC File Offset: 0x001314CC
		protected override void _SetInt(string key, int value)
		{
			this._DeleteKey(key);
			this._ints.Add(key, value);
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001332E2 File Offset: 0x001314E2
		protected override void _SetFloat(string key, float value)
		{
			this._DeleteKey(key);
			this._floats.Add(key, value);
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x001332F8 File Offset: 0x001314F8
		protected override void _SetString(string key, string value)
		{
			this._DeleteKey(key);
			this._strings.Add(key, value);
		}

		// Token: 0x04002875 RID: 10357
		private Dictionary<string, int> _ints = new Dictionary<string, int>();

		// Token: 0x04002876 RID: 10358
		private Dictionary<string, float> _floats = new Dictionary<string, float>();

		// Token: 0x04002877 RID: 10359
		private Dictionary<string, string> _strings = new Dictionary<string, string>();
	}
}
