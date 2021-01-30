using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000204 RID: 516
	public class TestAddingPoints : MonoBehaviour
	{
		// Token: 0x060012C3 RID: 4803 RVA: 0x00071684 File Offset: 0x0006F884
		public void AddNewPoint()
		{
			Vector2 item = new Vector2
			{
				x = float.Parse(this.XValue.text),
				y = float.Parse(this.YValue.text)
			};
			List<Vector2> list = new List<Vector2>(this.LineRenderer.Points);
			list.Add(item);
			this.LineRenderer.Points = list.ToArray();
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000716F2 File Offset: 0x0006F8F2
		public void ClearPoints()
		{
			this.LineRenderer.Points = new Vector2[0];
		}

		// Token: 0x040010F1 RID: 4337
		public UILineRenderer LineRenderer;

		// Token: 0x040010F2 RID: 4338
		public Text XValue;

		// Token: 0x040010F3 RID: 4339
		public Text YValue;
	}
}
