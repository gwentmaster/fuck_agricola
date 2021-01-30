using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000678 RID: 1656
	public static class ColorExtension
	{
		// Token: 0x06003CD0 RID: 15568 RVA: 0x0012BF48 File Offset: 0x0012A148
		public static string ToHex(this Color32 color, bool includeAlpha = false)
		{
			string text = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
			if (includeAlpha)
			{
				text += color.a.ToString("X2");
			}
			return text;
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x0012BFAA File Offset: 0x0012A1AA
		public static string ToHex(this Color color, bool includeAlpha = false)
		{
			return color.ToHex(includeAlpha);
		}
	}
}
