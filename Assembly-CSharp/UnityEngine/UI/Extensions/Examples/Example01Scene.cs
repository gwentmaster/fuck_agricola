using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001EC RID: 492
	public class Example01Scene : MonoBehaviour
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x00070A00 File Offset: 0x0006EC00
		private void Start()
		{
			List<Example01CellDto> data = (from i in Enumerable.Range(0, 20)
			select new Example01CellDto
			{
				Message = "Cell " + i
			}).ToList<Example01CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x040010AE RID: 4270
		[SerializeField]
		private Example01ScrollView scrollView;
	}
}
