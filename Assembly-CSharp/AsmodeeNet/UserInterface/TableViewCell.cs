using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000641 RID: 1601
	public class TableViewCell : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x001256CA File Offset: 0x001238CA
		public virtual string ReuseIdentifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Clean()
		{
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003B01 RID: 15105 RVA: 0x001256D7 File Offset: 0x001238D7
		// (set) Token: 0x06003B02 RID: 15106 RVA: 0x001256DF File Offset: 0x001238DF
		public Action<TableViewCell> OnSelection { private get; set; }

		// Token: 0x06003B03 RID: 15107 RVA: 0x001256E8 File Offset: 0x001238E8
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<TableViewCell> onSelection = this.OnSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}
	}
}
