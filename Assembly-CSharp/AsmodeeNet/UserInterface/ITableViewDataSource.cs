using System;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063E RID: 1598
	public interface ITableViewDataSource
	{
		// Token: 0x06003ABF RID: 15039
		int GetNumberOfCellsInTableView(TableView tableView);

		// Token: 0x06003AC0 RID: 15040
		float GetHeightForCellIndexInTableView(TableView tableView, int index);

		// Token: 0x06003AC1 RID: 15041
		TableViewCell GetCellForIndexInTableView(TableView tableView, int index);
	}
}
