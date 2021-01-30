using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F1 RID: 497
	public class Example02ScrollView : FancyScrollView<Example02CellDto, Example02ScrollViewContext>
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x00070B70 File Offset: 0x0006ED70
		private new void Awake()
		{
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
			this.scrollPositionController.OnItemSelected.AddListener(new UnityAction<int>(this.CellSelected));
			base.SetContext(new Example02ScrollViewContext
			{
				OnPressedCell = new Action<Example02ScrollViewCell>(this.OnPressedCell)
			});
			base.Awake();
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00070BD8 File Offset: 0x0006EDD8
		public void UpdateData(List<Example02CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00070BFD File Offset: 0x0006EDFD
		private void OnPressedCell(Example02ScrollViewCell cell)
		{
			this.scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			this.context.SelectedIndex = cell.DataIndex;
			base.UpdateContents();
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00070C2C File Offset: 0x0006EE2C
		private void CellSelected(int cellIndex)
		{
			this.context.SelectedIndex = cellIndex;
			base.UpdateContents();
		}

		// Token: 0x040010B5 RID: 4277
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
