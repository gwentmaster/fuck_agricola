using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C9 RID: 457
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectLinker")]
	public class ScrollRectLinker : MonoBehaviour
	{
		// Token: 0x06001181 RID: 4481 RVA: 0x0006D228 File Offset: 0x0006B428
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			if (this.controllingScrollRect != null)
			{
				this.controllingScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.MirrorPos));
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0006D260 File Offset: 0x0006B460
		private void MirrorPos(Vector2 scrollPos)
		{
			if (this.clamp)
			{
				this.scrollRect.normalizedPosition = new Vector2(Mathf.Clamp01(scrollPos.x), Mathf.Clamp01(scrollPos.y));
				return;
			}
			this.scrollRect.normalizedPosition = scrollPos;
		}

		// Token: 0x0400100B RID: 4107
		public bool clamp = true;

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private ScrollRect controllingScrollRect;

		// Token: 0x0400100D RID: 4109
		private ScrollRect scrollRect;
	}
}
