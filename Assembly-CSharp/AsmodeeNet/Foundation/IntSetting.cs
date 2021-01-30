using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006FD RID: 1789
	[Serializable]
	public class IntSetting : BaseSetting<int>
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x0013420A File Offset: 0x0013240A
		public IntSetting(string name) : base(name)
		{
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00134213 File Offset: 0x00132413
		protected override int _ReadValue()
		{
			return KeyValueStore.GetInt(base._FullPath, base.DefaultValue);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x00134226 File Offset: 0x00132426
		protected override void _WriteValue(int value)
		{
			KeyValueStore.SetInt(base._FullPath, value);
		}
	}
}
