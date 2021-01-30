using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F5 RID: 245
public class ScreenManager : MonoBehaviour
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x0003D57C File Offset: 0x0003B77C
	public static ScreenManager instance
	{
		get
		{
			if (ScreenManager._instance == null)
			{
				ScreenManager._instance = UnityEngine.Object.FindObjectOfType<ScreenManager>();
				if (ScreenManager._instance == null)
				{
					ScreenManager._instance = new GameObject
					{
						name = "ScreenManager"
					}.AddComponent<ScreenManager>();
				}
			}
			return ScreenManager._instance;
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0003D5CC File Offset: 0x0003B7CC
	public static bool ValidInstance()
	{
		return ScreenManager._instance != null;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0003D5DC File Offset: 0x0003B7DC
	private void Awake()
	{
		if (ScreenManager._instance != null && this != ScreenManager._instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ScreenManager._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ScreenManager.s_onStartScreen = this.m_firstSceneToShow;
		this.m_OpenParameterId = Animator.StringToHash(this.m_animatorBooleanVariableName);
		this.m_screenNames = new Dictionary<string, RegisterMenu>();
		this.m_screenStack = new Stack<RegisterMenu>();
		if (this.m_inputBlockerImage != null)
		{
			this.m_inputBlockerImage.enabled = false;
		}
		if (this.m_inputBlockerRaycaster != null)
		{
			this.m_inputBlockerRaycaster.enabled = false;
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0003D686 File Offset: 0x0003B886
	private void Start()
	{
		if (this.m_loadingScreenRootNode != null)
		{
			this.m_loadLevelSplashScreen = this.m_loadingScreenRootNode.GetComponent<LoadLevelSplashScreen>();
		}
		this.GoToFrontEndScreens(true, 0f, false);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0003D6B4 File Offset: 0x0003B8B4
	public bool CanTransition()
	{
		return this.m_bCanTransition;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0003D6BC File Offset: 0x0003B8BC
	public void Register(RegisterMenu rm)
	{
		if (this.m_screenNames.ContainsKey(rm.menuName))
		{
			UnityEngine.Object.Destroy(rm.gameObject);
			Debug.LogWarning("ScreenManager::Register() - Attempted to register a duplicate menu " + rm.GetName());
			return;
		}
		rm.SetEnabled(false);
		this.m_screenNames.Add(rm.menuName, rm);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0003D718 File Offset: 0x0003B918
	public void UnRegister(RegisterMenu rm)
	{
		if (!this.m_screenNames.ContainsValue(rm))
		{
			Debug.LogWarning("ScreenManager::UnRegister() - Attempted to unregister menu not in menu manager " + rm.GetName());
			return;
		}
		if (!this.m_screenNames.Remove(rm.menuName))
		{
			Debug.LogWarning("ScreenManager::UnRegister() - Attempted to unregister menu not in menu manager " + rm.GetName());
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0003D771 File Offset: 0x0003B971
	public GameObject GetScene(string sceneName)
	{
		if (!this.m_screenNames.ContainsKey(sceneName))
		{
			Debug.LogWarning("ScreenManager::GetScene() - Attempted to get menu not in menu manager " + sceneName);
			return null;
		}
		return this.m_screenNames[sceneName].gameObject;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0003D7A4 File Offset: 0x0003B9A4
	public bool GetIsSceneInStack(string newScene)
	{
		GameObject scene = this.GetScene(newScene);
		return !(scene == null) && this.m_screenStack.Contains(scene.GetComponent<RegisterMenu>());
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0003D7D5 File Offset: 0x0003B9D5
	private void DisableStaticButtons()
	{
		if (this.m_inputBlockerImage != null)
		{
			this.m_inputBlockerImage.enabled = true;
		}
		if (this.m_inputBlockerRaycaster != null)
		{
			this.m_inputBlockerRaycaster.enabled = true;
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0003D80B File Offset: 0x0003BA0B
	private void EnableStaticButtons()
	{
		if (this.m_inputBlockerImage != null)
		{
			this.m_inputBlockerImage.enabled = false;
		}
		if (this.m_inputBlockerRaycaster != null)
		{
			this.m_inputBlockerRaycaster.enabled = false;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00003022 File Offset: 0x00001222
	private void Update()
	{
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0003D844 File Offset: 0x0003BA44
	private static GameObject FindFirstEnabledSelectable(GameObject gameObject)
	{
		GameObject result = null;
		foreach (Selectable selectable in gameObject.GetComponentsInChildren<Selectable>(true))
		{
			if (selectable.IsActive() && selectable.IsInteractable())
			{
				result = selectable.gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0003D887 File Offset: 0x0003BA87
	private void CheckLastScene(string newScene)
	{
		if (newScene == this.m_lastSceneName)
		{
			base.StopAllCoroutines();
		}
		this.m_lastSceneName = newScene;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0003D8A4 File Offset: 0x0003BAA4
	public string GetCurrentScreenName()
	{
		if (this.m_screenStack.Peek() != null)
		{
			return this.m_screenStack.Peek().GetName();
		}
		return string.Empty;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003D8CF File Offset: 0x0003BACF
	public int GetSceneStackCount()
	{
		return this.m_screenStack.Count;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003D8DC File Offset: 0x0003BADC
	public void PushScene(string scnName)
	{
		if (!this.m_screenNames.ContainsKey(scnName))
		{
			Debug.LogError("ScreenManager::PushScene() - Attempted to show menu not in menu manager: " + scnName);
			return;
		}
		this.CheckLastScene(scnName);
		RegisterMenu registerMenu = (this.m_screenStack.Count > 0) ? this.m_screenStack.Peek() : null;
		bool flag = false;
		if (registerMenu != null && this.ExitScene(registerMenu, this.m_screenNames[scnName].isPopup))
		{
			flag = true;
			AnalyticsEvents.LogScreenDisplayEvent(scnName, string.Empty, string.Empty, null);
			this.DisableStaticButtons();
			this.m_bCanTransition = false;
			if (registerMenu.isWaitForAnimation && !this.m_screenNames[scnName].isPopup)
			{
				base.StartCoroutine(this.WaitForAnimationBeforeSwitchingScreen(ScreenManager.ScreenSwitchDelayType.WaitForAnimationBeforePush, this.m_screenNames[scnName]));
				return;
			}
			base.StartCoroutine(this.DisablePanelDelayed(registerMenu));
		}
		if (!flag)
		{
			AnalyticsEvents.LogScreenDisplayEvent(scnName, string.Empty, string.Empty, null);
		}
		this.UI_PushScene(this.m_screenNames[scnName]);
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003D9DB File Offset: 0x0003BBDB
	public void PopScene()
	{
		this.PopScene2(false, false);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
	private void PopScene2(bool bClearingStack, bool bSkipAnims = false)
	{
		int count = this.m_screenStack.Count;
		if (this.m_screenStack.Count > 0)
		{
			RegisterMenu registerMenu = this.m_screenStack.Peek();
			if (this.ExitScene2(registerMenu, false, bSkipAnims))
			{
				if (this.m_screenStack.Count > 1)
				{
					this.m_screenStack.Pop();
					AnalyticsEvents.LogScreenDisplayEvent(this.m_screenStack.Peek().GetName(), string.Empty, string.Empty, null);
					this.m_screenStack.Push(registerMenu);
				}
				else
				{
					AnalyticsEvents.LogScreenDisplayEvent(ScreenManager.s_onStartScreen, string.Empty, string.Empty, null);
				}
				this.DisableStaticButtons();
				this.m_bCanTransition = false;
				if (registerMenu.isWaitForAnimation)
				{
					base.StartCoroutine(this.WaitForAnimationBeforeSwitchingScreen(ScreenManager.ScreenSwitchDelayType.WaitForAnimationBeforePop, null));
					return;
				}
				base.StartCoroutine(this.DisablePanelDelayed(registerMenu));
			}
			this.UI_PopScene(bClearingStack);
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0003DAC5 File Offset: 0x0003BCC5
	private IEnumerator WaitForAnimationBeforeSwitchingScreen(ScreenManager.ScreenSwitchDelayType waitType, RegisterMenu nextScn)
	{
		yield return base.StartCoroutine(this.DisablePanelDelayed(this.m_screenStack.Peek()));
		if (waitType == ScreenManager.ScreenSwitchDelayType.WaitForAnimationBeforePop)
		{
			this.UI_PopScene(false);
		}
		else if (waitType == ScreenManager.ScreenSwitchDelayType.WaitForAnimationBeforePush)
		{
			this.UI_PushScene(nextScn);
		}
		yield break;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0003DAE4 File Offset: 0x0003BCE4
	private void CheckForAudioHandler()
	{
		if (this.m_audioHandler == null)
		{
			GameObject gameObject = GameObject.Find(this.m_AudioHandlerPath);
			if (gameObject != null)
			{
				this.m_audioHandler = gameObject.GetComponent<AudioSettingsHandler>();
			}
		}
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0003DB20 File Offset: 0x0003BD20
	private void UI_PushScene(RegisterMenu newScene)
	{
		this.m_screenStack.Push(newScene);
		this.EnterScene(this.m_screenStack.Peek());
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0003DB3F File Offset: 0x0003BD3F
	private void UI_PopScene(bool bClearingStack)
	{
		this.m_screenStack.Pop();
		if (this.m_screenStack.Count > 0)
		{
			this.EnterScene2(this.m_screenStack.Peek(), bClearingStack);
			return;
		}
		if (!bClearingStack)
		{
			this.PushScene(ScreenManager.s_onStartScreen);
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0003DB7C File Offset: 0x0003BD7C
	private void ClearStack(bool bSkipAnims)
	{
		while (this.m_screenStack.Count > 0)
		{
			this.PopScene2(true, bSkipAnims);
		}
		this.m_screenStack.Clear();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0003DBA4 File Offset: 0x0003BDA4
	public void GoToScene(string scnName, bool bSkipAnims = true)
	{
		if (!string.IsNullOrEmpty(scnName) && !this.m_screenNames.ContainsKey(scnName))
		{
			Debug.LogError("ScreenManager::GoToScene() - Attempted to show menu not in menu manager: " + scnName);
			return;
		}
		this.DisableStaticButtons();
		this.m_bCanTransition = false;
		this.ClearStack(bSkipAnims);
		this.PushScene(scnName);
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0003DBF3 File Offset: 0x0003BDF3
	private void EnterScene(RegisterMenu newScene)
	{
		this.EnterScene2(newScene, false);
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0003DC00 File Offset: 0x0003BE00
	private void EnterScene2(RegisterMenu newScene, bool bClearingStack)
	{
		if (!bClearingStack)
		{
			newScene.SetEnabled(true);
			newScene.OnMenuStart();
			if (newScene.HasAnimation() && !bClearingStack)
			{
				newScene.SetAnimation(this.m_OpenParameterId, false);
				base.StartCoroutine(this.EnablePanelDelayed(newScene));
				return;
			}
			this.CheckForAudioHandler();
			if (this.m_audioHandler != null)
			{
				this.m_audioHandler.DisableToggleSoundEffects();
			}
			base.StartCoroutine(this.DelayedInteractable(newScene));
			newScene.SetInteractable(true);
			if (this.m_audioHandler != null)
			{
				this.m_audioHandler.EnableToggleSoundEffects();
			}
		}
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0003DC94 File Offset: 0x0003BE94
	private IEnumerator EnablePanelDelayed(RegisterMenu anim)
	{
		bool openStateReached = false;
		bool wantToOpen = true;
		while (!openStateReached && wantToOpen)
		{
			if (!anim.GetAnimation().IsInTransition(0))
			{
				openStateReached = anim.GetAnimation().GetCurrentAnimatorStateInfo(0).IsTag(this.m_OpenStateName);
			}
			wantToOpen = !anim.GetAnimation().GetBool(this.m_OpenParameterId);
			yield return new WaitForEndOfFrame();
		}
		if (wantToOpen)
		{
			this.CheckForAudioHandler();
			if (this.m_audioHandler != null)
			{
				this.m_audioHandler.DisableToggleSoundEffects();
			}
			base.StartCoroutine(this.DelayedInteractable(anim));
			anim.SetInteractable(true);
			if (this.m_audioHandler != null)
			{
				this.m_audioHandler.EnableToggleSoundEffects();
			}
		}
		yield break;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0003DCAA File Offset: 0x0003BEAA
	private bool ExitScene(RegisterMenu oldScene, bool isNewSceneAPopup)
	{
		return this.ExitScene2(oldScene, isNewSceneAPopup, false);
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0003DCB8 File Offset: 0x0003BEB8
	private bool ExitScene2(RegisterMenu oldScene, bool isNewSceneAPopup, bool bClearingStack)
	{
		if (oldScene != null)
		{
			if (!isNewSceneAPopup)
			{
				oldScene.SetInteractable(false);
				if (!bClearingStack && oldScene.HasAnimation())
				{
					oldScene.SetAnimation(this.m_OpenParameterId, true);
					return true;
				}
				oldScene.OnMenuEnd(false);
				oldScene.SetEnabled(false);
			}
			else
			{
				oldScene.SetInteractable(false);
				oldScene.OnMenuEnd(true);
			}
		}
		return false;
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0003DD11 File Offset: 0x0003BF11
	private IEnumerator DelayedClose(RegisterMenu closeScene)
	{
		yield return new WaitForSeconds(this.m_delayBeforeSetInactive);
		closeScene.SetEnabled(false);
		yield break;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0003DD27 File Offset: 0x0003BF27
	private IEnumerator DelayedInteractable(RegisterMenu newMenu)
	{
		yield return new WaitForSeconds(0.4f);
		this.EnableStaticButtons();
		this.m_bCanTransition = true;
		yield break;
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0003DD36 File Offset: 0x0003BF36
	private IEnumerator DisablePanelDelayed(RegisterMenu anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.GetAnimation().IsInTransition(0))
			{
				closedStateReached = anim.GetAnimation().GetCurrentAnimatorStateInfo(0).IsTag(this.m_ClosedStateName);
			}
			wantToClose = anim.GetAnimation().GetBool(this.m_OpenParameterId);
			yield return new WaitForEndOfFrame();
		}
		if (wantToClose)
		{
			anim.OnMenuEnd(false);
			base.StartCoroutine(this.DelayedClose(anim));
		}
		yield break;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0003DD4C File Offset: 0x0003BF4C
	private void SetSelected(GameObject go)
	{
		EventSystem.current.SetSelectedGameObject(go);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0003DD5C File Offset: 0x0003BF5C
	public void LoadIntoGameScreen(int roundNumber = 1)
	{
		this.ClearStack(true);
		this.CheckForAudioHandler();
		if (this.m_audioHandler != null)
		{
			this.m_audioHandler.StopAllMusic();
			GameObject gameObject = GameObject.Find(this.m_AudioStartGameObjectPath);
			if (gameObject != null)
			{
				this.m_startGameAudio = gameObject.GetComponent<AudioSource>();
			}
		}
		if (this.m_startGameAudio != null)
		{
			this.m_startGameAudio.Play();
		}
		this.EnableStaticButtons();
		if (this.m_InGameBackgroundSceneName != string.Empty)
		{
			this.m_loadLevelSplashScreen.BeginLoadingSequence(3, false, 0.5f, false);
		}
		else
		{
			this.m_loadLevelSplashScreen.BeginLoadingSequence(2, false, 0.5f, false);
		}
		base.StartCoroutine(this.LoadIntoGameScreenAsync());
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0003DE18 File Offset: 0x0003C018
	public void GoToFrontEndScreens(bool bSupressLoadingScreen = false, float minLoadTime = 0f, bool bManualSceneActivation = false)
	{
		this.DisableStaticButtons();
		if (this.m_FrontEndBackgroundSceneName != string.Empty)
		{
			this.m_loadLevelSplashScreen.BeginLoadingSequence(2, bSupressLoadingScreen, minLoadTime, bManualSceneActivation);
		}
		else
		{
			this.m_loadLevelSplashScreen.BeginLoadingSequence(1, bSupressLoadingScreen, minLoadTime, bManualSceneActivation);
		}
		base.StartCoroutine(this.GoToFrontEndScreensAsync(bSupressLoadingScreen, minLoadTime, bManualSceneActivation));
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0003DE6F File Offset: 0x0003C06F
	public void HandleFrontEndSequenceComplete(int sequenceIndex)
	{
		this.PushScene(ScreenManager.s_onStartScreen);
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0003DE7C File Offset: 0x0003C07C
	private IEnumerator LoadIntoGameScreenAsync()
	{
		yield return new WaitForSeconds(this.m_DelayBeforeLoadingInGame);
		if (this.m_InGameBackgroundSceneName != string.Empty)
		{
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_InGameSceneName, 0, false, null);
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_InGameBackgroundSceneName, 1, true, null);
		}
		else
		{
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_InGameSceneName, 0, false, null);
		}
		yield break;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0003DE8B File Offset: 0x0003C08B
	private IEnumerator GoToFrontEndScreensAsync(bool bSupressLoadingScreen, float minLoadTime, bool bManualSceneActivation)
	{
		yield return new WaitForSeconds(this.m_DelayBeforeLoadingFrontEnd);
		if (this.m_FrontEndBackgroundSceneName != string.Empty)
		{
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_FrontEndBackgroundSceneName, 0, false, null);
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_FrontEndSceneName, 1, true, new LoadLevelSplashScreen.SequenceHandlerDelegate(this.HandleFrontEndSequenceComplete));
		}
		else
		{
			this.m_loadLevelSplashScreen.LoadLevelAsync(this.m_FrontEndSceneName, 0, false, new LoadLevelSplashScreen.SequenceHandlerDelegate(this.HandleFrontEndSequenceComplete));
		}
		yield break;
	}

	// Token: 0x04000999 RID: 2457
	private static ScreenManager _instance = null;

	// Token: 0x0400099A RID: 2458
	public static string s_shortFilename = string.Empty;

	// Token: 0x0400099B RID: 2459
	public static string s_fullFilename = string.Empty;

	// Token: 0x0400099C RID: 2460
	public static string s_onStartScreen = "Main Menu";

	// Token: 0x0400099D RID: 2461
	public GameObject m_loadingScreenRootNode;

	// Token: 0x0400099E RID: 2462
	public AudioSource m_startGameAudio;

	// Token: 0x0400099F RID: 2463
	public AudioSettingsHandler m_audioHandler;

	// Token: 0x040009A0 RID: 2464
	public float m_delayBeforeSetInactive;

	// Token: 0x040009A1 RID: 2465
	[SerializeField]
	private string m_firstSceneToShow = "TitleMenu";

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private string m_animatorBooleanVariableName = "isHidden";

	// Token: 0x040009A3 RID: 2467
	[SerializeField]
	private string m_ClosedStateName = "Close";

	// Token: 0x040009A4 RID: 2468
	[SerializeField]
	private string m_OpenStateName = "Open";

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private string m_FrontEndSceneName = "FrontEnd";

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private string m_FrontEndBackgroundSceneName = "";

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private string m_InGameSceneName = "InGameAlt";

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private string m_InGameBackgroundSceneName = "";

	// Token: 0x040009A9 RID: 2473
	[SerializeField]
	private string m_AudioHandlerPath = "/Audio";

	// Token: 0x040009AA RID: 2474
	[SerializeField]
	private string m_AudioStartGameObjectPath = "/Audio/AudioStartGame";

	// Token: 0x040009AB RID: 2475
	[SerializeField]
	private float m_DelayBeforeLoadingFrontEnd = 1f;

	// Token: 0x040009AC RID: 2476
	[SerializeField]
	private float m_DelayBeforeLoadingInGame = 2f;

	// Token: 0x040009AD RID: 2477
	public Image m_inputBlockerImage;

	// Token: 0x040009AE RID: 2478
	public GraphicRaycaster m_inputBlockerRaycaster;

	// Token: 0x040009AF RID: 2479
	private Dictionary<string, RegisterMenu> m_screenNames;

	// Token: 0x040009B0 RID: 2480
	private Stack<RegisterMenu> m_screenStack;

	// Token: 0x040009B1 RID: 2481
	private LoadLevelSplashScreen m_loadLevelSplashScreen;

	// Token: 0x040009B2 RID: 2482
	private bool m_bCanTransition = true;

	// Token: 0x040009B3 RID: 2483
	private int m_OpenParameterId;

	// Token: 0x040009B4 RID: 2484
	private string m_lastSceneName = string.Empty;

	// Token: 0x020007B4 RID: 1972
	private enum ScreenSwitchDelayType
	{
		// Token: 0x04002CBB RID: 11451
		WaitForAnimationBeforePush,
		// Token: 0x04002CBC RID: 11452
		WaitForAnimationBeforePop
	}
}
