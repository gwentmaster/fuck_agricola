using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F5 RID: 501
	public class Example03Scene : MonoBehaviour
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x00070D80 File Offset: 0x0006EF80
		private void Start()
		{
			List<Example03CellDto> data = (from i in Enumerable.Range(0, 20)
			select new Example03CellDto
			{
				Message = "Cell " + i
			}).ToList<Example03CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x040010BF RID: 4287
		[SerializeField]
		private Example03ScrollView scrollView;
	}
}
