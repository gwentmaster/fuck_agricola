using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F9 RID: 1785
	[CreateAssetMenu]
	public class ApplicationSettings : ScriptableObject
	{
		// Token: 0x06003F35 RID: 16181 RVA: 0x00133E59 File Offset: 0x00132059
		private void OnEnable()
		{
			this._baseSettings = new List<IBaseSetting>();
			this._PopulateCoreSettingsList();
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x00133E6C File Offset: 0x0013206C
		private void _PopulateCoreSettingsList()
		{
			this._music = (this._music ?? new FloatSetting("Core.Music"));
			this._baseSettings.Add(this._music);
			this._musicState = (this._musicState ?? new BoolSetting("Core.MusicState"));
			this._baseSettings.Add(this._musicState);
			this._sfx = (this._sfx ?? new FloatSetting("Core.Sfx"));
			this._baseSettings.Add(this._sfx);
			this._sfxState = (this._sfxState ?? new BoolSetting("Core.SfxState"));
			this._baseSettings.Add(this._sfxState);
			this._animationLevel = (this._animationLevel ?? new IntSetting("Core.AnimationLevel"));
			this._baseSettings.Add(this._animationLevel);
			this._fullScreen = (this._fullScreen ?? new BoolSetting("Core.FullScreen"));
			this._baseSettings.Add(this._fullScreen);
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00133F7C File Offset: 0x0013217C
		public void Clear()
		{
			foreach (IBaseSetting baseSetting in this._baseSettings)
			{
				baseSetting.Clear();
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003F38 RID: 16184 RVA: 0x00133FCC File Offset: 0x001321CC
		public FloatSetting Music
		{
			get
			{
				return this._music;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x00133FD4 File Offset: 0x001321D4
		public BoolSetting MusicState
		{
			get
			{
				return this._musicState;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x00133FDC File Offset: 0x001321DC
		public FloatSetting Sfx
		{
			get
			{
				return this._sfx;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003F3B RID: 16187 RVA: 0x00133FE4 File Offset: 0x001321E4
		public BoolSetting SfxState
		{
			get
			{
				return this._sfxState;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003F3C RID: 16188 RVA: 0x00133FEC File Offset: 0x001321EC
		public IntSetting AnimationLevel
		{
			get
			{
				return this._animationLevel;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003F3D RID: 16189 RVA: 0x00133FF4 File Offset: 0x001321F4
		public BoolSetting FullScreen
		{
			get
			{
				return this._fullScreen;
			}
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00133FFC File Offset: 0x001321FC
		public override string ToString()
		{
			string text = "";
			foreach (IBaseSetting arg in this._baseSettings)
			{
				text += string.Format("[{0}] ", arg);
			}
			return text;
		}

		// Token: 0x0400288E RID: 10382
		protected List<IBaseSetting> _baseSettings;

		// Token: 0x0400288F RID: 10383
		[Header("Core Settings")]
		[SerializeField]
		private FloatSetting _music;

		// Token: 0x04002890 RID: 10384
		[SerializeField]
		private BoolSetting _musicState;

		// Token: 0x04002891 RID: 10385
		[SerializeField]
		private FloatSetting _sfx;

		// Token: 0x04002892 RID: 10386
		[SerializeField]
		private BoolSetting _sfxState;

		// Token: 0x04002893 RID: 10387
		[SerializeField]
		private IntSetting _animationLevel;

		// Token: 0x04002894 RID: 10388
		[SerializeField]
		private BoolSetting _fullScreen;
	}
}
