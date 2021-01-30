using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200019A RID: 410
	public class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x06000FB1 RID: 4017 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SetContext(TContext context)
		{
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void UpdateContent(TData itemData)
		{
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void UpdatePosition(float position)
		{
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00020E80 File Offset: 0x0001F080
		public virtual void SetVisible(bool visible)
		{
			base.gameObject.SetActive(visible);
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00063980 File Offset: 0x00061B80
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x00063988 File Offset: 0x00061B88
		public int DataIndex { get; set; }
	}
}
