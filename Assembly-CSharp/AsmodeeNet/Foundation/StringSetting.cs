using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006FF RID: 1791
	[Serializable]
	public class StringSetting : BaseSetting<string>
	{
		// Token: 0x06003F59 RID: 16217 RVA: 0x0013427E File Offset: 0x0013247E
		public StringSetting(string name) : base(name)
		{
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00134287 File Offset: 0x00132487
		protected override string _ReadValue()
		{
			return KeyValueStore.GetString(base._FullPath, base.DefaultValue);
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x0013429A File Offset: 0x0013249A
		protected override void _WriteValue(string value)
		{
			KeyValueStore.SetString(base._FullPath, value);
		}
	}
}
