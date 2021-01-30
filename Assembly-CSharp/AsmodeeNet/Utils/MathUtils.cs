using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066B RID: 1643
	public struct MathUtils
	{
		// Token: 0x06003C87 RID: 15495 RVA: 0x0012B30C File Offset: 0x0012950C
		public static bool Approximately(float value1, float value2, float epsilon)
		{
			float num = value1 - value2;
			if (num >= 0f)
			{
				return num <= epsilon;
			}
			return num >= -epsilon;
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0012B335 File Offset: 0x00129535
		public static bool Approximately(Vector2 value1, Vector2 value2, float epsilon)
		{
			return MathUtils.Approximately(value1.x, value2.x, epsilon) && MathUtils.Approximately(value1.y, value2.y, epsilon);
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x0012B360 File Offset: 0x00129560
		public static bool Approximately(Rect value1, Rect value2, float epsilon)
		{
			return MathUtils.Approximately(value1.x, value2.x, epsilon) && MathUtils.Approximately(value1.y, value2.y, epsilon) && MathUtils.Approximately(value1.width, value2.width, epsilon) && MathUtils.Approximately(value1.height, value2.height, epsilon);
		}
	}
}
