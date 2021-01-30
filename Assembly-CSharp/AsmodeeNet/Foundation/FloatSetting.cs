using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006FC RID: 1788
	[Serializable]
	public class FloatSetting : BaseSetting<float>
	{
		// Token: 0x06003F4E RID: 16206 RVA: 0x001341E0 File Offset: 0x001323E0
		public FloatSetting(string name) : base(name)
		{
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x001341E9 File Offset: 0x001323E9
		protected override float _ReadValue()
		{
			return KeyValueStore.GetFloat(base._FullPath, base.DefaultValue);
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x001341FC File Offset: 0x001323FC
		protected override void _WriteValue(float value)
		{
			KeyValueStore.SetFloat(base._FullPath, value);
		}
	}
}
