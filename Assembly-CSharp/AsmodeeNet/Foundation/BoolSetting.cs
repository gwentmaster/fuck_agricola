using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006FE RID: 1790
	[Serializable]
	public class BoolSetting : BaseSetting<bool>
	{
		// Token: 0x06003F54 RID: 16212 RVA: 0x00134234 File Offset: 0x00132434
		public BoolSetting(string name) : base(name)
		{
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x0013423D File Offset: 0x0013243D
		protected override bool _ReadValue()
		{
			return this._IntToBool(KeyValueStore.GetInt(base._FullPath, this._BoolToInt(base.DefaultValue)));
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x0013425C File Offset: 0x0013245C
		protected override void _WriteValue(bool value)
		{
			KeyValueStore.SetInt(base._FullPath, this._BoolToInt(value));
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00134270 File Offset: 0x00132470
		private bool _IntToBool(int i)
		{
			return i > 0;
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00134276 File Offset: 0x00132476
		private int _BoolToInt(bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}
	}
}
