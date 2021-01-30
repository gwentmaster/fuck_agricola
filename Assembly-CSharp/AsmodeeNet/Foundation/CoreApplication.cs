using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.UserInterface;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006EF RID: 1775
	[RequireComponent(typeof(CoreApplicationDelegate))]
	[RequireComponent(typeof(SceneTransitionManager))]
	[RequireComponent(typeof(EventManager))]
	[RequireComponent(typeof(UINavigationManager))]
	[RequireComponent(typeof(AnalyticsManager))]
	[RequireComponent(typeof(AsmodeeNet.Utils.AvatarManager))]
	[RequireComponent(typeof(CommunityHubLauncher))]
	[RequireComponent(typeof(FocusableLayer))]
	public class CoreApplication : MonoBehaviour
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x00132989 File Offset: 0x00130B89
		public static CoreApplication Instance
		{
			get
			{
				if (CoreApplication._instance == null)
				{
					AsmoLogger.Log(Application.isPlaying ? AsmoLogger.Severity.Error : AsmoLogger.Severity.Warning, "CoreApplication", "Missing CoreApplication Instance", null);
				}
				return CoreApplication._instance;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x001329B8 File Offset: 0x00130BB8
		public static bool HasInstance
		{
			get
			{
				return CoreApplication._instance != null;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x001329C5 File Offset: 0x00130BC5
		public Channel Channel
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return Channel.steam;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x001329CE File Offset: 0x00130BCE
		public CoreApplicationDelegate CoreApplicationDelegate
		{
			get
			{
				return this._coreApplicationDelegate;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x001329D6 File Offset: 0x00130BD6
		public SceneTransitionManager SceneTransitionManager
		{
			get
			{
				return this._sceneTransitionManager;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x001329DE File Offset: 0x00130BDE
		public EventManager EventManager
		{
			get
			{
				return this._eventManager;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x001329E6 File Offset: 0x00130BE6
		public UINavigationManager UINavigationManager
		{
			get
			{
				return this._uiNavigationManager;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x001329EE File Offset: 0x00130BEE
		public AnalyticsManager AnalyticsManager
		{
			get
			{
				return this._analyticsManager;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x001329F6 File Offset: 0x00130BF6
		public CommunityHubLauncher CommunityHubLauncher
		{
			get
			{
				return this._communityHubLauncher;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x001329FE File Offset: 0x00130BFE
		public CommunityHub CommunityHub
		{
			get
			{
				return this._communityHubLauncher.CommunityHub;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x00132A0B File Offset: 0x00130C0B
		public OAuthGate OAuthGate
		{
			get
			{
				return this._oauthGate;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x00132A13 File Offset: 0x00130C13
		public AsmodeeNet.Utils.AvatarManager AvatarManager
		{
			get
			{
				return this._avatarManager;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003EBB RID: 16059 RVA: 0x00132A1B File Offset: 0x00130C1B
		public Preferences Preferences
		{
			get
			{
				return this._preferences;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003EBC RID: 16060 RVA: 0x00132A23 File Offset: 0x00130C23
		public LocalizationManager LocalizationManager
		{
			get
			{
				return this._localizationManager;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x00132A2B File Offset: 0x00130C2B
		// (set) Token: 0x06003EBE RID: 16062 RVA: 0x00132A33 File Offset: 0x00130C33
		public SteamManager SteamManager { get; private set; }

		// Token: 0x06003EBF RID: 16063 RVA: 0x00132A3C File Offset: 0x00130C3C
		protected virtual void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (CoreApplication._instance == null)
			{
				this._Initialize();
				return;
			}
			if (CoreApplication._instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x00132A78 File Offset: 0x00130C78
		private void _Initialize()
		{
			Hashtable extraInfo = new Hashtable
			{
				{
					"SDK Version",
					SDKVersionManager.Version()
				}
			};
			AsmoLogger.Info("CoreApplication", "Initialization", extraInfo);
			CoreApplication._instance = this;
			this._coreApplicationDelegate = base.GetComponent<CoreApplicationDelegate>();
			if (this._coreApplicationDelegate == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing CoreApplicationDelegate component", null);
			}
			this._sceneTransitionManager = base.GetComponent<SceneTransitionManager>();
			if (this._sceneTransitionManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing SceneTransitionManager component", null);
			}
			this._eventManager = base.GetComponent<EventManager>();
			if (this._eventManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing EventManager component", null);
			}
			this._uiNavigationManager = base.GetComponent<UINavigationManager>();
			if (this._uiNavigationManager == null)
			{
				this._uiNavigationManager = base.gameObject.AddComponent<UINavigationManager>();
			}
			if (base.GetComponent<FocusableLayer>() == null)
			{
				base.gameObject.AddComponent<FocusableLayer>();
			}
			this._analyticsManager = base.GetComponent<AnalyticsManager>();
			if (this._analyticsManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing AnalyticsManager component", null);
			}
			this._avatarManager = base.GetComponent<AsmodeeNet.Utils.AvatarManager>();
			if (this._avatarManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing AvatarManager component", null);
			}
			this._communityHubLauncher = base.GetComponent<CommunityHubLauncher>();
			if (this._communityHubLauncher == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing CommunityHubLauncher component", null);
			}
			KeyValueStore.Instance = new PlayerPrefsKeyValueStore();
			try
			{
				this.SteamManager = new SteamManager(this.SteamGameId);
			}
			catch (Exception ex)
			{
				AsmoLogger.Error("CoreApplication.Steam", string.Format("Couldn't initialize Steam because: \"{0}\" (exception type = {1})", ex.Message, ex.GetType()), null);
			}
			if (this.NetworkParameters == null)
			{
				string text = "NetworkParameters must be set for CoreApplication to work";
				AsmoLogger.Error("CoreApplication", text, null);
				throw new ArgumentNullException(text);
			}
			this._oauthGate = new OAuthGate(this.NetworkParameters)
			{
				SteamManager = this.SteamManager
			};
			if (this.InterfaceSkin == null)
			{
				AsmoLogger.Warning("CoreApplication", "Interface Skin not provided -> Fall back to default", null);
				this.InterfaceSkin = (UnityEngine.Object.Instantiate(Resources.Load("InterfaceDefaultSkin", typeof(InterfaceSkin))) as InterfaceSkin);
			}
			this._localizationManager = new LocalizationManager(this.supportedLanguages);
			this._preferences = new Preferences();
			AsmoLogger.Debug("CoreApplication", "Initialized (Lite SDK)", extraInfo);
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x00132CF8 File Offset: 0x00130EF8
		private void Start()
		{
			this._localizationManager.Init();
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00132D05 File Offset: 0x00130F05
		private void Update()
		{
			this._preferences.Update();
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x0002A062 File Offset: 0x00028262
		public static bool IsFullSDK
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003EC4 RID: 16068 RVA: 0x00132D12 File Offset: 0x00130F12
		// (set) Token: 0x06003EC5 RID: 16069 RVA: 0x00132D19 File Offset: 0x00130F19
		public static bool IsQuitting { get; private set; }

		// Token: 0x06003EC6 RID: 16070 RVA: 0x00132D21 File Offset: 0x00130F21
		private void OnApplicationQuit()
		{
			CoreApplication.IsQuitting = true;
			KeyValueStore.ResetInstance();
			if (this.SteamManager != null)
			{
				this.SteamManager.Dispose();
				this.SteamManager = null;
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00132D48 File Offset: 0x00130F48
		private void OnApplicationPause(bool paused)
		{
			if (!paused && this._avatarManager != null)
			{
				this._avatarManager.ClearCache();
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x00132D66 File Offset: 0x00130F66
		public static string GetUserAgent()
		{
			return string.Format("{0}/{1} {2}/ Unity/{3}", new object[]
			{
				Application.productName,
				Application.version,
				Application.platform,
				Application.unityVersion
			});
		}

		// Token: 0x0400285A RID: 10330
		private const string _documentation = "<b>CoreApplication</b> is the root singleton of your game (<i>DontDestroyOnLoad</i>). You should add it to all your scenes.\nYou may overide <b>CoreApplicationDelegate</b> to add your main services.";

		// Token: 0x0400285B RID: 10331
		private const string _consoleModuleName = "CoreApplication";

		// Token: 0x0400285C RID: 10332
		public uint SteamGameId = 480U;

		// Token: 0x0400285D RID: 10333
		private static CoreApplication _instance;

		// Token: 0x0400285E RID: 10334
		private CoreApplicationDelegate _coreApplicationDelegate;

		// Token: 0x0400285F RID: 10335
		private SceneTransitionManager _sceneTransitionManager;

		// Token: 0x04002860 RID: 10336
		private EventManager _eventManager;

		// Token: 0x04002861 RID: 10337
		private UINavigationManager _uiNavigationManager;

		// Token: 0x04002862 RID: 10338
		private AnalyticsManager _analyticsManager;

		// Token: 0x04002863 RID: 10339
		private CommunityHubLauncher _communityHubLauncher;

		// Token: 0x04002864 RID: 10340
		public NetworkParameters NetworkParameters;

		// Token: 0x04002865 RID: 10341
		public InterfaceSkin InterfaceSkin;

		// Token: 0x04002866 RID: 10342
		private OAuthGate _oauthGate;

		// Token: 0x04002867 RID: 10343
		private AsmodeeNet.Utils.AvatarManager _avatarManager;

		// Token: 0x04002868 RID: 10344
		private Preferences _preferences;

		// Token: 0x04002869 RID: 10345
		private LocalizationManager _localizationManager;

		// Token: 0x0400286A RID: 10346
		[Tooltip("You can manage localization in Asmodee.net/Localization")]
		public List<LocalizationManager.Language> supportedLanguages = new List<LocalizationManager.Language>
		{
			LocalizationManager.Language.en_US
		};
	}
}
