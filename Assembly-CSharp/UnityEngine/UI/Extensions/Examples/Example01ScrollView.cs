using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001ED RID: 493
	public class Example01ScrollView : FancyScrollView<Example01CellDto>
	{
		// Token: 0x0600126F RID: 4719 RVA: 0x00070A4B File Offset: 0x0006EC4B
		private new void Awake()
		{
			base.Awake();
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00070A6F File Offset: 0x0006EC6F
		public void UpdateData(List<Example01CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x040010AF RID: 4271
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
