using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C0 RID: 448
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("UI/Extensions/DragCorrector")]
	public class DragCorrector : MonoBehaviour
	{
		// Token: 0x0600115D RID: 4445 RVA: 0x0006CB00 File Offset: 0x0006AD00
		private void Start()
		{
			this.dragTH = this.baseTH * (int)Screen.dpi / this.basePPI;
			EventSystem component = base.GetComponent<EventSystem>();
			if (component)
			{
				component.pixelDragThreshold = this.dragTH;
			}
		}

		// Token: 0x04000FFD RID: 4093
		public int baseTH = 6;

		// Token: 0x04000FFE RID: 4094
		public int basePPI = 210;

		// Token: 0x04000FFF RID: 4095
		public int dragTH;
	}
}
