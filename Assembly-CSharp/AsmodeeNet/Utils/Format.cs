using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000667 RID: 1639
	public static class Format
	{
		// Token: 0x06003C7D RID: 15485 RVA: 0x0012AFB8 File Offset: 0x001291B8
		public static string HexStringFromString(string str)
		{
			return Format.HexStringFromUInt64(ulong.Parse(str));
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x0012AFC5 File Offset: 0x001291C5
		public static string HexStringFromUInt64(ulong i)
		{
			return string.Format("{0:X}", i).ToLower();
		}
	}
}
