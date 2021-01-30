using System;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200061E RID: 1566
	public class AlertControllerButton : MonoBehaviour
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x0011E844 File Offset: 0x0011CA44
		// (set) Token: 0x0600399B RID: 14747 RVA: 0x0011E84C File Offset: 0x0011CA4C
		public AlertController.ButtonStyle Style
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
				this._UpdateStyle();
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0011E85C File Offset: 0x0011CA5C
		private void Awake()
		{
			this._button = base.GetComponent<Button>();
			if (this._button == null)
			{
				AsmoLogger.Error("AlertControllerButton", "Missing Button behavior", null);
			}
			if (this.cancelSprites == null)
			{
				this.cancelSprites = this.defaultSprites;
			}
			if (this.destructiveSprites == null)
			{
				this.destructiveSprites = this.defaultSprites;
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0011E8BB File Offset: 0x0011CABB
		private void OnEnable()
		{
			this._UpdateStyle();
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x0011E8C4 File Offset: 0x0011CAC4
		private void _UpdateStyle()
		{
			AlertController.ButtonStyle style = this._style;
			AlertControllerButton.FullSpriteState fullSpriteState;
			if (style != AlertController.ButtonStyle.Cancel)
			{
				if (style != AlertController.ButtonStyle.Destructive)
				{
					fullSpriteState = this.defaultSprites;
				}
				else
				{
					fullSpriteState = this.destructiveSprites;
				}
			}
			else
			{
				fullSpriteState = this.cancelSprites;
			}
			(this._button.targetGraphic as Image).sprite = fullSpriteState.idle;
			this._button.spriteState = fullSpriteState.spriteState;
		}

		// Token: 0x0400253E RID: 9534
		private const string _kModuleName = "AlertControllerButton";

		// Token: 0x0400253F RID: 9535
		public AlertControllerButton.FullSpriteState defaultSprites;

		// Token: 0x04002540 RID: 9536
		public AlertControllerButton.FullSpriteState cancelSprites;

		// Token: 0x04002541 RID: 9537
		public AlertControllerButton.FullSpriteState destructiveSprites;

		// Token: 0x04002542 RID: 9538
		private AlertController.ButtonStyle _style;

		// Token: 0x04002543 RID: 9539
		private Button _button;

		// Token: 0x02000916 RID: 2326
		[Serializable]
		public class FullSpriteState
		{
			// Token: 0x04003089 RID: 12425
			public Sprite idle;

			// Token: 0x0400308A RID: 12426
			public SpriteState spriteState;
		}
	}
}
