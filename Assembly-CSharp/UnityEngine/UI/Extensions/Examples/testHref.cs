using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000202 RID: 514
	public class testHref : MonoBehaviour
	{
		// Token: 0x060012B3 RID: 4787 RVA: 0x000714A6 File Offset: 0x0006F6A6
		private void Awake()
		{
			this.textPic = base.GetComponent<TextPic>();
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000714B4 File Offset: 0x0006F6B4
		private void OnEnable()
		{
			this.textPic.onHrefClick.AddListener(new UnityAction<string>(this.OnHrefClick));
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x000714D2 File Offset: 0x0006F6D2
		private void OnDisable()
		{
			this.textPic.onHrefClick.RemoveListener(new UnityAction<string>(this.OnHrefClick));
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000714F0 File Offset: 0x0006F6F0
		private void OnHrefClick(string hrefName)
		{
			Debug.Log("Click on the " + hrefName);
		}

		// Token: 0x040010E8 RID: 4328
		public TextPic textPic;
	}
}
