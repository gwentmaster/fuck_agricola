using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F0 RID: 496
	public class Example02Scene : MonoBehaviour
	{
		// Token: 0x06001277 RID: 4727 RVA: 0x00070B24 File Offset: 0x0006ED24
		private void Start()
		{
			List<Example02CellDto> data = (from i in Enumerable.Range(0, 20)
			select new Example02CellDto
			{
				Message = "Cell " + i
			}).ToList<Example02CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x040010B4 RID: 4276
		[SerializeField]
		private Example02ScrollView scrollView;
	}
}
