using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000183 RID: 387
	public class ExampleSelectable : MonoBehaviour, IBoxSelectable
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0005E7A2 File Offset: 0x0005C9A2
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x0005E7AA File Offset: 0x0005C9AA
		public bool selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0005E7B3 File Offset: 0x0005C9B3
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0005E7BB File Offset: 0x0005C9BB
		public bool preSelected
		{
			get
			{
				return this._preSelected;
			}
			set
			{
				this._preSelected = value;
			}
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0005E7C4 File Offset: 0x0005C9C4
		private void Start()
		{
			this.spriteRenderer = base.transform.GetComponent<SpriteRenderer>();
			this.image = base.transform.GetComponent<Image>();
			this.text = base.transform.GetComponent<Text>();
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0005E7FC File Offset: 0x0005C9FC
		private void Update()
		{
			Color color = Color.white;
			if (this.preSelected)
			{
				color = Color.yellow;
			}
			if (this.selected)
			{
				color = Color.green;
			}
			if (this.spriteRenderer)
			{
				this.spriteRenderer.color = color;
				return;
			}
			if (this.text)
			{
				this.text.color = color;
				return;
			}
			if (this.image)
			{
				this.image.color = color;
				return;
			}
			if (base.GetComponent<Renderer>())
			{
				base.GetComponent<Renderer>().material.color = color;
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00057B84 File Offset: 0x00055D84
		Transform IBoxSelectable.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000E7B RID: 3707
		private bool _selected;

		// Token: 0x04000E7C RID: 3708
		private bool _preSelected;

		// Token: 0x04000E7D RID: 3709
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000E7E RID: 3710
		private Image image;

		// Token: 0x04000E7F RID: 3711
		private Text text;
	}
}
