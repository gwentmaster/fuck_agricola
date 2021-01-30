using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F6 RID: 502
	public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00070DCC File Offset: 0x0006EFCC
		private new void Awake()
		{
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
			this.scrollPositionController.OnItemSelected.AddListener(new UnityAction<int>(this.CellSelected));
			base.SetContext(new Example03ScrollViewContext
			{
				OnPressedCell = new Action<Example03ScrollViewCell>(this.OnPressedCell)
			});
			base.Awake();
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00070E34 File Offset: 0x0006F034
		public void UpdateData(List<Example03CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00070E59 File Offset: 0x0006F059
		private void OnPressedCell(Example03ScrollViewCell cell)
		{
			this.scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			this.context.SelectedIndex = cell.DataIndex;
			base.UpdateContents();
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00070E88 File Offset: 0x0006F088
		private void CellSelected(int cellIndex)
		{
			this.context.SelectedIndex = cellIndex;
			base.UpdateContents();
		}

		// Token: 0x040010C0 RID: 4288
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
