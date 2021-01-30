using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C4 RID: 452
	[AddComponentMenu("UI/Extensions/Pagination Manager")]
	public class PaginationManager : ToggleGroup
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x0006CC05 File Offset: 0x0006AE05
		public int CurrentPage
		{
			get
			{
				return this.scrollSnap.CurrentPage;
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0006CC12 File Offset: 0x0006AE12
		protected PaginationManager()
		{
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0006CC1C File Offset: 0x0006AE1C
		protected override void Start()
		{
			base.Start();
			if (this.scrollSnap == null)
			{
				Debug.LogError("A ScrollSnap script must be attached");
				return;
			}
			if (this.scrollSnap.Pagination)
			{
				this.scrollSnap.Pagination = null;
			}
			this.scrollSnap.OnSelectionPageChangedEvent.AddListener(new UnityAction<int>(this.SetToggleGraphics));
			this.scrollSnap.OnSelectionChangeEndEvent.AddListener(new UnityAction<int>(this.OnPageChangeEnd));
			this.m_PaginationChildren = base.GetComponentsInChildren<Toggle>().ToList<Toggle>();
			for (int i = 0; i < this.m_PaginationChildren.Count; i++)
			{
				this.m_PaginationChildren[i].onValueChanged.AddListener(new UnityAction<bool>(this.ToggleClick));
				this.m_PaginationChildren[i].group = this;
				this.m_PaginationChildren[i].isOn = false;
			}
			this.SetToggleGraphics(this.CurrentPage);
			if (this.m_PaginationChildren.Count != this.scrollSnap._scroll_rect.content.childCount)
			{
				Debug.LogWarning("Uneven pagination icon to page count");
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0006CD43 File Offset: 0x0006AF43
		public void GoToScreen(int pageNo)
		{
			this.scrollSnap.GoToScreen(pageNo);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0006CD51 File Offset: 0x0006AF51
		private void ToggleClick(Toggle target)
		{
			if (!target.isOn)
			{
				this.isAClick = true;
				this.GoToScreen(this.m_PaginationChildren.IndexOf(target));
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0006CD74 File Offset: 0x0006AF74
		private void ToggleClick(bool toggle)
		{
			if (toggle)
			{
				for (int i = 0; i < this.m_PaginationChildren.Count; i++)
				{
					if (this.m_PaginationChildren[i].isOn)
					{
						this.GoToScreen(i);
						return;
					}
				}
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0006CDB5 File Offset: 0x0006AFB5
		private void ToggleClick(int target)
		{
			this.isAClick = true;
			this.GoToScreen(target);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0006CDC5 File Offset: 0x0006AFC5
		private void SetToggleGraphics(int pageNo)
		{
			if (!this.isAClick)
			{
				this.m_PaginationChildren[pageNo].isOn = true;
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0006CDE1 File Offset: 0x0006AFE1
		private void OnPageChangeEnd(int pageNo)
		{
			this.isAClick = false;
		}

		// Token: 0x04001003 RID: 4099
		private List<Toggle> m_PaginationChildren;

		// Token: 0x04001004 RID: 4100
		[SerializeField]
		private ScrollSnapBase scrollSnap;

		// Token: 0x04001005 RID: 4101
		private bool isAClick;
	}
}
