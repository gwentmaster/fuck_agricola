using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F7 RID: 503
	public class Example03ScrollViewCell : FancyScrollViewCell<Example03CellDto, Example03ScrollViewContext>
	{
		// Token: 0x0600128D RID: 4749 RVA: 0x00070EA4 File Offset: 0x0006F0A4
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
			this.button.onClick.AddListener(new UnityAction(this.OnPressedCell));
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00070F03 File Offset: 0x0006F103
		public override void SetContext(Example03ScrollViewContext context)
		{
			this.context = context;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00070F0C File Offset: 0x0006F10C
		public override void UpdateContent(Example03CellDto itemData)
		{
			this.message.text = itemData.Message;
			if (this.context != null)
			{
				bool flag = this.context.SelectedIndex == base.DataIndex;
				this.image.color = (flag ? new Color32(0, byte.MaxValue, byte.MaxValue, 100) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77));
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00070F83 File Offset: 0x0006F183
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00070FA8 File Offset: 0x0006F1A8
		public void OnPressedCell()
		{
			if (this.context != null)
			{
				this.context.OnPressedCell(this);
			}
		}

		// Token: 0x040010C1 RID: 4289
		[SerializeField]
		private Animator animator;

		// Token: 0x040010C2 RID: 4290
		[SerializeField]
		private Text message;

		// Token: 0x040010C3 RID: 4291
		[SerializeField]
		private Image image;

		// Token: 0x040010C4 RID: 4292
		[SerializeField]
		private Button button;

		// Token: 0x040010C5 RID: 4293
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");

		// Token: 0x040010C6 RID: 4294
		private Example03ScrollViewContext context;
	}
}
