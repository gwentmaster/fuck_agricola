using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000638 RID: 1592
	[RequireComponent(typeof(Image))]
	public class ImageModifier : MonoBehaviour
	{
		// Token: 0x06003A99 RID: 15001 RVA: 0x001233F9 File Offset: 0x001215F9
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00123407 File Offset: 0x00121607
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._Display;
			this._Display();
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x0012342A File Offset: 0x0012162A
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._Display;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00123450 File Offset: 0x00121650
		private void _Display()
		{
			Preferences.DisplayMode currentDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			if (!this.displayModeToSprite.Any((ImageModifier.DisplayModeToSprite x) => x.displayMode == currentDisplayMode))
			{
				currentDisplayMode = Preferences.DisplayMode.Unknown;
			}
			this._image.sprite = this.displayModeToSprite.Single((ImageModifier.DisplayModeToSprite x) => x.displayMode == currentDisplayMode).sprite;
		}

		// Token: 0x040025F5 RID: 9717
		private const string _documentation = "Allow you to set a different sprite for the image which will be display according to the current display mode (small, regular, big).\nIf none is specified for a specific display mode then default will be used instead";

		// Token: 0x040025F6 RID: 9718
		private Image _image;

		// Token: 0x040025F7 RID: 9719
		public List<ImageModifier.DisplayModeToSprite> displayModeToSprite = new List<ImageModifier.DisplayModeToSprite>();

		// Token: 0x02000932 RID: 2354
		[Serializable]
		public class DisplayModeToSprite
		{
			// Token: 0x040030DC RID: 12508
			public Preferences.DisplayMode displayMode;

			// Token: 0x040030DD RID: 12509
			public Sprite sprite;
		}
	}
}
