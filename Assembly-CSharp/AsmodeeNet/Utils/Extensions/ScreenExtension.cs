using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x0200067C RID: 1660
	public static class ScreenExtension
	{
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x0012C1E8 File Offset: 0x0012A3E8
		public static float DiagonalLengthInch
		{
			get
			{
				float num = Screen.dpi;
				if (num < 25f || num > 1000f)
				{
					num = 150f;
				}
				return Mathf.Sqrt((float)(Screen.width * Screen.width + Screen.height * Screen.height)) / num;
			}
		}
	}
}
