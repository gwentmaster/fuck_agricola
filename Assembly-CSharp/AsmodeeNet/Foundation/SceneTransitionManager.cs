using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F8 RID: 1784
	public class SceneTransitionManager : MonoBehaviour
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06003F16 RID: 16150 RVA: 0x001336AC File Offset: 0x001318AC
		// (remove) Token: 0x06003F17 RID: 16151 RVA: 0x001336E4 File Offset: 0x001318E4
		public event Action<string> SceneWillLoad;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06003F18 RID: 16152 RVA: 0x0013371C File Offset: 0x0013191C
		// (remove) Token: 0x06003F19 RID: 16153 RVA: 0x00133754 File Offset: 0x00131954
		public event Action<Scene> SceneDidLoad;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06003F1A RID: 16154 RVA: 0x0013378C File Offset: 0x0013198C
		// (remove) Token: 0x06003F1B RID: 16155 RVA: 0x001337C4 File Offset: 0x001319C4
		public event Action<Scene> SceneWillUnload;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06003F1C RID: 16156 RVA: 0x001337FC File Offset: 0x001319FC
		// (remove) Token: 0x06003F1D RID: 16157 RVA: 0x00133834 File Offset: 0x00131A34
		public event Action<Scene> SceneDidUnload;

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003F1E RID: 16158 RVA: 0x00133869 File Offset: 0x00131A69
		// (set) Token: 0x06003F1F RID: 16159 RVA: 0x00133871 File Offset: 0x00131A71
		public bool IsTransitioning
		{
			get
			{
				return this._isTransitioning;
			}
			private set
			{
				this._isTransitioning = value;
				if (this._isTransitioning)
				{
					CoreApplication.Instance.UINavigationManager.BeginIgnoringInteractionEvents("SceneTransitionManager");
					return;
				}
				CoreApplication.Instance.UINavigationManager.EndIgnoringInteractionEvents("SceneTransitionManager");
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003F20 RID: 16160 RVA: 0x001338AB File Offset: 0x00131AAB
		private bool IsInstantTransition
		{
			get
			{
				return (this.IsTransitioning && this._transitionType == SceneTransitionManager.TransitionType.None) || Mathf.Approximately(this._transitionSpeed, 0f);
			}
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x001338D0 File Offset: 0x00131AD0
		public bool IsCurrentScene(string sceneName)
		{
			return SceneManager.GetActiveScene().name == sceneName;
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x001338F0 File Offset: 0x00131AF0
		public void PreLoadScene(string sceneName)
		{
			if (this.IsTransitioning)
			{
				AsmoLogger.Warning("SceneTransitionManager", "Pre loading a scene during a transition is not supported", null);
				return;
			}
			CoreApplication.Instance.StartCoroutine(this.LoadScene(sceneName, false));
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00133920 File Offset: 0x00131B20
		public void DisplayScene(string sceneName, SceneTransitionManager.TransitionType transitionType = SceneTransitionManager.TransitionType.FadeOutIn, float transitionDuration = 1f, bool forceReload = false)
		{
			if (this.IsTransitioning)
			{
				AsmoLogger.Warning("SceneTransitionManager", "Displaying a scene during a transition is not supported", null);
				return;
			}
			this._nextSceneName = sceneName;
			this._transitionType = transitionType;
			transitionDuration = Mathf.Max(0f, transitionDuration);
			float num = (this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn) ? 2f : 1f;
			this._transitionSpeed = num / transitionDuration;
			this.IsTransitioning = !this.IsInstantTransition;
			Hashtable extraInfo = new Hashtable
			{
				{
					"type",
					transitionType
				},
				{
					"duration",
					transitionDuration
				},
				{
					"isInstant",
					this.IsInstantTransition
				}
			};
			Scene sceneByName = SceneManager.GetSceneByName(this._nextSceneName);
			if (sceneByName.IsValid() && !forceReload)
			{
				if (sceneByName.isLoaded)
				{
					AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Already Loaded]", extraInfo);
					if (this.IsInstantTransition)
					{
						this.SetSceneActive(sceneByName);
					}
				}
				else
				{
					AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Loading]", extraInfo);
					if (this.IsInstantTransition)
					{
						this.FinishLoadingScene(this._nextSceneName);
					}
				}
			}
			else
			{
				AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Not Loaded]", extraInfo);
				bool isInstantTransition = this.IsInstantTransition;
				CoreApplication.Instance.StartCoroutine(this.LoadScene(this._nextSceneName, isInstantTransition));
			}
			if (!this.IsInstantTransition)
			{
				this.StartFadeOut();
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00133A88 File Offset: 0x00131C88
		public IEnumerator LoadScene(string sceneName, bool allowSceneActivation = false)
		{
			this.CallSceneWillLoad(sceneName);
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			if (!allowSceneActivation)
			{
				operation.allowSceneActivation = false;
				this._loadingOperations.Add(sceneName, operation);
			}
			float progress = -1f;
			while (!operation.isDone)
			{
				if (operation.progress > progress)
				{
					progress = operation.progress;
					AsmoLogger.Debug("SceneTransitionManager", () => string.Format("Loading scene: {0} [{1}%]", sceneName, progress * 100f), null);
				}
				yield return null;
			}
			Scene? scene = null;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.name == sceneName && sceneAt != SceneManager.GetActiveScene())
				{
					scene = new Scene?(sceneAt);
					break;
				}
			}
			if (scene != null)
			{
				this.CallSceneDidLoad(scene.Value);
				this.SetSceneActive(scene.Value);
			}
			else
			{
				AsmoLogger.Error("SceneTransitionManager", "Scene not found during loading", null);
			}
			yield break;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00133AA5 File Offset: 0x00131CA5
		private void FinishLoadingScene(string sceneName)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Finish loading scene: " + sceneName, null);
			this._loadingOperations[sceneName].allowSceneActivation = true;
			this._loadingOperations.Remove(sceneName);
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00133ADC File Offset: 0x00131CDC
		private void SetSceneActive(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Set scene active: " + scene.name, null);
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.SetActiveScene(scene);
			this.CallSceneWillUnload(activeScene);
			SceneManager.UnloadSceneAsync(activeScene);
			Resources.UnloadUnusedAssets();
			this.CallSceneDidUnload(activeScene);
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00133B2D File Offset: 0x00131D2D
		private void StartFadeOut()
		{
			this._fadingDirection = 1;
			if (this._transitionType == SceneTransitionManager.TransitionType.FadeOut || this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn)
			{
				this._fadingAlpha = 0f;
				return;
			}
			this._fadingAlpha = 1f;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x00133B5F File Offset: 0x00131D5F
		private void StartFadeIn()
		{
			this._fadingDirection = -1;
			if (this._transitionType == SceneTransitionManager.TransitionType.FadeIn || this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn)
			{
				this._fadingAlpha = 1f;
				return;
			}
			this._fadingAlpha = 0f;
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x00133B91 File Offset: 0x00131D91
		private void PauseFade()
		{
			this._fadingDirection = 0;
			this._fadingAlpha = 1f;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00133BA5 File Offset: 0x00131DA5
		private void StopFade()
		{
			this._fadingDirection = 0;
			this._fadingAlpha = 0f;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x00133BBC File Offset: 0x00131DBC
		private Texture2D FadingTexture
		{
			get
			{
				if (this._fadingTexture == null)
				{
					this._fadingTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					this._fadingTexture.SetPixel(0, 0, this.fadingColor);
					this._fadingTexture.Apply();
				}
				return this._fadingTexture;
			}
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00133C0C File Offset: 0x00131E0C
		private void OnGUI()
		{
			if (!this.IsTransitioning)
			{
				return;
			}
			this._fadingAlpha += (float)this._fadingDirection * this._transitionSpeed * Time.deltaTime;
			if (this._fadingDirection > 0 && this._fadingAlpha > 1f)
			{
				this.PauseFade();
				this.FinishLoadingScene(this._nextSceneName);
			}
			else if (this._fadingDirection == 0)
			{
				if (SceneManager.GetActiveScene().name == this._nextSceneName && SceneManager.sceneCount == 1)
				{
					this.StartFadeIn();
				}
			}
			else if (this._fadingDirection < 0 && this._fadingAlpha < 0f)
			{
				this.StopFade();
				this.IsTransitioning = false;
			}
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Clamp01(this._fadingAlpha));
			GUI.depth = -1000;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.FadingTexture);
			if (!this._isTransitioning)
			{
				this._fadingTexture = null;
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00133D34 File Offset: 0x00131F34
		private void CallSceneWillLoad(string sceneName)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Will load scene: " + sceneName, null);
			if (this.SceneWillLoad != null)
			{
				this.SceneWillLoad(sceneName);
			}
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00133D60 File Offset: 0x00131F60
		private void CallSceneDidLoad(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Did load scene: " + scene.name, null);
			if (this.SceneDidLoad != null)
			{
				this.SceneDidLoad(scene);
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00133D92 File Offset: 0x00131F92
		private void CallSceneWillUnload(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Will unload scene: " + scene.name, null);
			if (this.SceneWillUnload != null)
			{
				this.SceneWillUnload(scene);
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00133DC4 File Offset: 0x00131FC4
		private void CallSceneDidUnload(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Did unload scene: " + scene.name, null);
			if (this.SceneDidUnload != null)
			{
				this.SceneDidUnload(scene);
			}
		}

		// Token: 0x0400287F RID: 10367
		private const string _documentation = "<b>SceneTransitionManager</b> loads and displays <b>Scene</b>s with transitions by using <i>Multi Scene Editing</i> system";

		// Token: 0x04002884 RID: 10372
		private Dictionary<string, AsyncOperation> _loadingOperations = new Dictionary<string, AsyncOperation>();

		// Token: 0x04002885 RID: 10373
		private bool _isTransitioning;

		// Token: 0x04002886 RID: 10374
		private SceneTransitionManager.TransitionType _transitionType;

		// Token: 0x04002887 RID: 10375
		private float _transitionSpeed;

		// Token: 0x04002888 RID: 10376
		private string _nextSceneName;

		// Token: 0x04002889 RID: 10377
		private const string _debugModuleName = "SceneTransitionManager";

		// Token: 0x0400288A RID: 10378
		private Texture2D _fadingTexture;

		// Token: 0x0400288B RID: 10379
		private float _fadingAlpha;

		// Token: 0x0400288C RID: 10380
		private int _fadingDirection;

		// Token: 0x0400288D RID: 10381
		public Color fadingColor = Color.black;

		// Token: 0x020009E2 RID: 2530
		public enum TransitionType
		{
			// Token: 0x04003372 RID: 13170
			None,
			// Token: 0x04003373 RID: 13171
			FadeOut,
			// Token: 0x04003374 RID: 13172
			FadeIn,
			// Token: 0x04003375 RID: 13173
			FadeOutIn
		}
	}
}
