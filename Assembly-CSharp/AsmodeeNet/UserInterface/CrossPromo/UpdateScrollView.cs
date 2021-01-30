using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200065C RID: 1628
	public class UpdateScrollView : MonoBehaviour
	{
		// Token: 0x06003C16 RID: 15382 RVA: 0x00129FA4 File Offset: 0x001281A4
		private void Awake()
		{
			this.rt = (base.transform as RectTransform);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x00129FB8 File Offset: 0x001281B8
		private void Update()
		{
			if (!Mathf.Approximately(this.rt.sizeDelta.y, 0f))
			{
				this.rt.sizeDelta = Vector2.zero;
				for (int i = 0; i < this.rt.childCount; i++)
				{
					LayoutElement component = this.rt.GetChild(i).GetComponent<LayoutElement>();
					component.preferredHeight = this.rt.rect.height;
					component.preferredWidth = this.rt.rect.height;
				}
			}
		}

		// Token: 0x040026DB RID: 9947
		private RectTransform rt;
	}
}
