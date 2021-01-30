using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F2 RID: 498
	public class Example02ScrollViewCell : FancyScrollViewCell<Example02CellDto, Example02ScrollViewContext>
	{
		// Token: 0x0600127E RID: 4734 RVA: 0x00070C48 File Offset: 0x0006EE48
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
			this.button.onClick.AddListener(new UnityAction(this.OnPressedCell));
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00070CA7 File Offset: 0x0006EEA7
		public override void SetContext(Example02ScrollViewContext context)
		{
			this.context = context;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00070CB0 File Offset: 0x0006EEB0
		public override void UpdateContent(Example02CellDto itemData)
		{
			this.message.text = itemData.Message;
			if (this.context != null)
			{
				bool flag = this.context.SelectedIndex == base.DataIndex;
				this.image.color = (flag ? new Color32(0, byte.MaxValue, byte.MaxValue, 100) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77));
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00070D27 File Offset: 0x0006EF27
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00070D4C File Offset: 0x0006EF4C
		public void OnPressedCell()
		{
			if (this.context != null)
			{
				this.context.OnPressedCell(this);
			}
		}

		// Token: 0x040010B6 RID: 4278
		[SerializeField]
		private Animator animator;

		// Token: 0x040010B7 RID: 4279
		[SerializeField]
		private Text message;

		// Token: 0x040010B8 RID: 4280
		[SerializeField]
		private Image image;

		// Token: 0x040010B9 RID: 4281
		[SerializeField]
		private Button button;

		// Token: 0x040010BA RID: 4282
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");

		// Token: 0x040010BB RID: 4283
		private Example02ScrollViewContext context;
	}
}
