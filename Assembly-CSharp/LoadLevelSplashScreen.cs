using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000111 RID: 273
public class LoadLevelSplashScreen : MonoBehaviour
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00043C94 File Offset: 0x00041E94
	public static LoadLevelSplashScreen instance
	{
		get
		{
			return LoadLevelSplashScreen._instance;
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00043C9B File Offset: 0x00041E9B
	private void Awake()
	{
		if (LoadLevelSplashScreen._instance != null && this != LoadLevelSplashScreen._instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		LoadLevelSplashScreen._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00003022 File Offset: 0x00001222
	private void OnDestroy()
	{
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00043CD4 File Offset: 0x00041ED4
	private void ResetProgressDisplay()
	{
		if (this.m_loadProgressBar != null)
		{
			this.m_loadProgressBar.fillAmount = 0f;
		}
		if (this.m_loadPercentageText != null)
		{
			string format = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Loading}");
			this.m_loadPercentageText.text = string.Format(format, 0);
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00043D34 File Offset: 0x00041F34
	public bool BeginLoadingSequence(int numberOfLoads, bool bSupressLoadingScreen = false, float minimumLoadTime = 0f, bool bManualSceneActivation = false)
	{
		this.m_inGameNetworkDisconnect.SetActive(false);
		if (!this.m_bLoadSequenceComplete)
		{
			return false;
		}
		this.m_loadSequenceTotal = numberOfLoads;
		this.m_loadSequenceMultiplier = 1f / (float)numberOfLoads;
		this.m_loadingSequenceInfo = new LoadLevelSplashScreen.LoadInfo[numberOfLoads];
		for (int i = 0; i < numberOfLoads; i++)
		{
			this.m_loadingSequenceInfo[i].loadAmount = 0f;
			this.m_loadingSequenceInfo[i].bLoadComplete = false;
			this.m_loadingSequenceInfo[i].m_callback = null;
		}
		this.m_bLoadSequenceComplete = false;
		this.ResetProgressDisplay();
		if (!bSupressLoadingScreen)
		{
			this.m_loadingScreenCanvasNode.SetActive(true);
		}
		if (minimumLoadTime > 0f)
		{
			base.StartCoroutine(this.ProcessMinimumLoadTime(minimumLoadTime));
		}
		else
		{
			this.m_bMinimumLoadTimeReached = true;
		}
		this.m_bManualSceneActivation = bManualSceneActivation;
		return true;
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00043E04 File Offset: 0x00042004
	public bool SetProgressDisplay(float loadAmount, int sequenceIndex)
	{
		if (this.m_bLoadSequenceComplete)
		{
			return false;
		}
		loadAmount *= this.m_loadSequenceMultiplier;
		this.m_loadingSequenceInfo[sequenceIndex].loadAmount = loadAmount;
		float num = this.m_loadingSequenceInfo[0].loadAmount;
		bool flag = this.m_loadingSequenceInfo[0].bLoadComplete;
		for (int i = 1; i < this.m_loadSequenceTotal; i++)
		{
			num += this.m_loadingSequenceInfo[i].loadAmount;
			if (!this.m_loadingSequenceInfo[i].bLoadComplete)
			{
				flag = false;
			}
		}
		if (!flag)
		{
			if (this.m_loadProgressBar != null)
			{
				this.m_loadProgressBar.fillAmount = num;
			}
			if (this.m_loadPercentageText != null)
			{
				int num2 = (int)(num * 100f);
				string format = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Loading}");
				this.m_loadPercentageText.text = string.Format(format, num2);
			}
		}
		else
		{
			if (this.m_loadingScreenCanvasNode != null)
			{
				this.m_inGameNetworkDisconnect.SetActive(false);
				this.m_loadingScreenCanvasNode.SetActive(false);
				this.m_bIgnoreNextDisconnect = false;
			}
			this.m_bLoadSequenceComplete = true;
			this.m_numLoadSequencesComplete += 1U;
		}
		return true;
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00043F38 File Offset: 0x00042138
	public bool MarkLoadComplete(int sequenceIndex)
	{
		if (this.m_bLoadSequenceComplete || this.m_loadingSequenceInfo[sequenceIndex].bLoadComplete)
		{
			return false;
		}
		this.m_loadingSequenceInfo[sequenceIndex].bLoadComplete = true;
		if (this.m_loadingSequenceInfo[sequenceIndex].m_callback != null)
		{
			this.m_loadingSequenceInfo[sequenceIndex].m_callback(sequenceIndex);
		}
		if (this.m_bShowNetworkDisconnectOnLoadComplete)
		{
			this.m_bShowNetworkDisconnectOnLoadComplete = false;
		}
		return this.SetProgressDisplay(1f, sequenceIndex);
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00043FB9 File Offset: 0x000421B9
	public void TriggerSceneActivation()
	{
		this.m_bManualSceneActivation = false;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00043FC2 File Offset: 0x000421C2
	private IEnumerator ProcessMinimumLoadTime(float minimumLoadTime)
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		this.m_bMinimumLoadTimeReached = false;
		while (accumulatedTime < minimumLoadTime)
		{
			accumulatedTime += Time.time - previousTime;
			previousTime = Time.time;
			yield return new WaitForEndOfFrame();
		}
		this.m_bMinimumLoadTimeReached = true;
		yield break;
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00043FD8 File Offset: 0x000421D8
	private IEnumerator ProcessLoadLevelAsync(AsyncOperation asyncOp, int sequenceIndex)
	{
		while (!asyncOp.isDone)
		{
			this.SetProgressDisplay(asyncOp.progress, sequenceIndex);
			if (this.m_bMinimumLoadTimeReached && !this.m_bManualSceneActivation)
			{
				asyncOp.allowSceneActivation = true;
			}
			yield return null;
		}
		this.MarkLoadComplete(sequenceIndex);
		yield break;
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00043FF8 File Offset: 0x000421F8
	public bool LoadLevelAsync(string levelName, int sequenceIndex, bool bAdditive = false, LoadLevelSplashScreen.SequenceHandlerDelegate callBack = null)
	{
		if (this.m_bLoadSequenceComplete || this.m_loadingSequenceInfo[sequenceIndex].bLoadComplete)
		{
			return false;
		}
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, bAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
		asyncOperation.allowSceneActivation = false;
		this.m_loadingSequenceInfo[sequenceIndex].m_callback = callBack;
		base.StartCoroutine(this.ProcessLoadLevelAsync(asyncOperation, sequenceIndex));
		return true;
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0004405A File Offset: 0x0004225A
	public uint GetNumLoadSequencesCompleted()
	{
		return this.m_numLoadSequencesComplete;
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00044064 File Offset: 0x00042264
	public void SetRoundDisplay(bool bVisible, int roundNumber)
	{
		if (roundNumber == 0)
		{
			roundNumber++;
		}
		if (bVisible && roundNumber > 0 && roundNumber < 9)
		{
			if (this.m_roundBase != null)
			{
				this.m_roundBase.SetActive(true);
			}
			roundNumber--;
			if (this.m_roundObjs != null)
			{
				for (int i = 0; i < this.m_roundObjs.Length; i++)
				{
					this.m_roundObjs[i].SetActive(roundNumber == i);
				}
			}
			if (this.m_roundTitleText != null && this.m_roundTitleTexts != null)
			{
				this.m_roundTitleText.text = this.m_roundTitleTexts[roundNumber];
			}
			if (this.m_roundDescText != null && this.m_roundDescTexts != null)
			{
				this.m_roundDescText.text = this.m_roundDescTexts[roundNumber];
				return;
			}
		}
		else if (this.m_roundBase != null)
		{
			this.m_roundBase.SetActive(false);
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00044146 File Offset: 0x00042346
	public bool IsLoadSequenceComplete()
	{
		return this.m_bLoadSequenceComplete;
	}

	// Token: 0x04000AC1 RID: 2753
	private static LoadLevelSplashScreen _instance;

	// Token: 0x04000AC2 RID: 2754
	public GameObject m_loadingScreenCanvasNode;

	// Token: 0x04000AC3 RID: 2755
	public Image m_loadProgressBar;

	// Token: 0x04000AC4 RID: 2756
	public TextMeshProUGUI m_loadPercentageText;

	// Token: 0x04000AC5 RID: 2757
	public GameObject m_inGameNetworkDisconnect;

	// Token: 0x04000AC6 RID: 2758
	[Header("Waterdeep Only Nodes")]
	public GameObject m_roundBase;

	// Token: 0x04000AC7 RID: 2759
	public GameObject[] m_roundObjs;

	// Token: 0x04000AC8 RID: 2760
	public string[] m_roundTitleTexts;

	// Token: 0x04000AC9 RID: 2761
	public string[] m_roundDescTexts;

	// Token: 0x04000ACA RID: 2762
	public Text m_roundTitleText;

	// Token: 0x04000ACB RID: 2763
	public Text m_roundDescText;

	// Token: 0x04000ACC RID: 2764
	private int m_loadSequenceTotal;

	// Token: 0x04000ACD RID: 2765
	private float m_loadSequenceMultiplier;

	// Token: 0x04000ACE RID: 2766
	private bool m_bShowNetworkDisconnectOnLoadComplete;

	// Token: 0x04000ACF RID: 2767
	private bool m_bIgnoreNextDisconnect;

	// Token: 0x04000AD0 RID: 2768
	private LoadLevelSplashScreen.LoadInfo[] m_loadingSequenceInfo;

	// Token: 0x04000AD1 RID: 2769
	private uint m_numLoadSequencesComplete;

	// Token: 0x04000AD2 RID: 2770
	private bool m_bMinimumLoadTimeReached;

	// Token: 0x04000AD3 RID: 2771
	private bool m_bManualSceneActivation;

	// Token: 0x04000AD4 RID: 2772
	private bool m_bLoadSequenceComplete = true;

	// Token: 0x020007E3 RID: 2019
	// (Invoke) Token: 0x0600435F RID: 17247
	public delegate void SequenceHandlerDelegate(int sequenceIndex);

	// Token: 0x020007E4 RID: 2020
	private struct LoadInfo
	{
		// Token: 0x04002D42 RID: 11586
		public float loadAmount;

		// Token: 0x04002D43 RID: 11587
		public bool bLoadComplete;

		// Token: 0x04002D44 RID: 11588
		public LoadLevelSplashScreen.SequenceHandlerDelegate m_callback;
	}
}
