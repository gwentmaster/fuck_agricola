using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001EE RID: 494
	public class Example01ScrollViewCell : FancyScrollViewCell<Example01CellDto>
	{
		// Token: 0x06001272 RID: 4722 RVA: 0x00070A9C File Offset: 0x0006EC9C
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00070AD4 File Offset: 0x0006ECD4
		public override void UpdateContent(Example01CellDto itemData)
		{
			this.message.text = itemData.Message;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00070AE7 File Offset: 0x0006ECE7
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x040010B0 RID: 4272
		[SerializeField]
		private Animator animator;

		// Token: 0x040010B1 RID: 4273
		[SerializeField]
		private Text message;

		// Token: 0x040010B2 RID: 4274
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");
	}
}
