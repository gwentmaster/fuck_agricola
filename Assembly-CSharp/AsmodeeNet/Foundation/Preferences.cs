using System;
using AsmodeeNet.Utils.Extensions;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F6 RID: 1782
	public class Preferences
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003F06 RID: 16134 RVA: 0x00133393 File Offset: 0x00131593
		public float Aspect
		{
			get
			{
				this.UpdateAspect();
				return this._aspect;
			}
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06003F07 RID: 16135 RVA: 0x001333A4 File Offset: 0x001315A4
		// (remove) Token: 0x06003F08 RID: 16136 RVA: 0x001333DC File Offset: 0x001315DC
		public event Action AspectDidChange;

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003F09 RID: 16137 RVA: 0x00133411 File Offset: 0x00131611
		public Preferences.Orientation InterfaceOrientation
		{
			get
			{
				this.UpdateInterfaceOrientation();
				return this._interfaceOrientation;
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06003F0A RID: 16138 RVA: 0x00133420 File Offset: 0x00131620
		// (remove) Token: 0x06003F0B RID: 16139 RVA: 0x00133458 File Offset: 0x00131658
		public event Action InterfaceOrientationDidChange;

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003F0C RID: 16140 RVA: 0x00133490 File Offset: 0x00131690
		// (set) Token: 0x06003F0D RID: 16141 RVA: 0x00133558 File Offset: 0x00131758
		public Preferences.DisplayMode InterfaceDisplayMode
		{
			get
			{
				if (this._displayMode == Preferences.DisplayMode.Unknown)
				{
					if (KeyValueStore.HasKey("DisplayMode"))
					{
						try
						{
							string @string = KeyValueStore.GetString("DisplayMode", "");
							this._displayMode = (Preferences.DisplayMode)Enum.Parse(typeof(Preferences.DisplayMode), @string);
						}
						catch
						{
						}
					}
					if (this._displayMode == Preferences.DisplayMode.Unknown)
					{
						switch (SystemInfo.deviceType)
						{
						case DeviceType.Handheld:
							this._displayMode = ((ScreenExtension.DiagonalLengthInch < 8f) ? Preferences.DisplayMode.Small : Preferences.DisplayMode.Regular);
							break;
						case DeviceType.Console:
							this._displayMode = Preferences.DisplayMode.Small;
							break;
						case DeviceType.Desktop:
							this._displayMode = ((ScreenExtension.DiagonalLengthInch < 16f) ? Preferences.DisplayMode.Regular : Preferences.DisplayMode.Big);
							break;
						}
					}
				}
				return this._displayMode;
			}
			set
			{
				this._displayMode = value;
				KeyValueStore.SetString("DisplayMode", this._displayMode.ToString());
				if (this.InterfaceDisplayModeDidChange != null)
				{
					this.InterfaceDisplayModeDidChange();
				}
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06003F0E RID: 16142 RVA: 0x00133590 File Offset: 0x00131790
		// (remove) Token: 0x06003F0F RID: 16143 RVA: 0x001335C8 File Offset: 0x001317C8
		public event Action InterfaceDisplayModeDidChange;

		// Token: 0x06003F10 RID: 16144 RVA: 0x001335FD File Offset: 0x001317FD
		public void Update()
		{
			this.UpdateAspect();
			this.UpdateInterfaceOrientation();
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0013360C File Offset: 0x0013180C
		private void UpdateAspect()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			float aspect = main.aspect;
			if (!Mathf.Approximately(aspect, this._aspect))
			{
				this._aspect = aspect;
				if (this.AspectDidChange != null)
				{
					this.AspectDidChange();
				}
			}
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00133658 File Offset: 0x00131858
		private void UpdateInterfaceOrientation()
		{
			Preferences.Orientation orientation = (this.Aspect < 1f) ? Preferences.Orientation.Vertical : Preferences.Orientation.Horizontal;
			if (orientation != this._interfaceOrientation)
			{
				this._interfaceOrientation = orientation;
				if (this.InterfaceOrientationDidChange != null)
				{
					this.InterfaceOrientationDidChange();
				}
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x0013369A File Offset: 0x0013189A
		public bool IsDesktop
		{
			get
			{
				return SystemInfo.deviceType == DeviceType.Desktop;
			}
		}

		// Token: 0x04002878 RID: 10360
		private float _aspect;

		// Token: 0x0400287A RID: 10362
		private Preferences.Orientation _interfaceOrientation;

		// Token: 0x0400287C RID: 10364
		private const string _kDisplayModeKey = "DisplayMode";

		// Token: 0x0400287D RID: 10365
		private Preferences.DisplayMode _displayMode;

		// Token: 0x020009E0 RID: 2528
		public enum Orientation
		{
			// Token: 0x04003369 RID: 13161
			Unknown,
			// Token: 0x0400336A RID: 13162
			Horizontal,
			// Token: 0x0400336B RID: 13163
			Vertical
		}

		// Token: 0x020009E1 RID: 2529
		public enum DisplayMode
		{
			// Token: 0x0400336D RID: 13165
			Unknown,
			// Token: 0x0400336E RID: 13166
			Small,
			// Token: 0x0400336F RID: 13167
			Regular,
			// Token: 0x04003370 RID: 13168
			Big
		}
	}
}
