using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000F3 RID: 243
public class RegisterMenu : MonoBehaviour
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x0003D2C4 File Offset: 0x0003B4C4
	protected virtual void Awake()
	{
		ScreenManager instance = ScreenManager.instance;
		if (instance == null)
		{
			Debug.LogError("RegisterMenu::Awake() - Could not find scene manager!");
			return;
		}
		this.animator = base.gameObject.GetComponent<Animator>();
		this.canvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		if (!this.isBuiltInPopup)
		{
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0f, 0f);
			component.offsetMin = vector;
			component.offsetMax = vector;
		}
		else
		{
			this.isPopup = true;
			if (this.canvasGroup != null)
			{
				this.canvasGroup.ignoreParentGroups = true;
			}
		}
		this.sceneName = this.menuName.ToString();
		this.animator == null;
		if (this.canvasGroup == null)
		{
			Debug.LogError("RegisterMenu::Awake() - Menu canvas group not found for scene " + this.sceneName);
		}
		instance.Register(this);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003D3AC File Offset: 0x0003B5AC
	private void OnDestroy()
	{
		if (!ScreenManager.ValidInstance())
		{
			return;
		}
		ScreenManager instance = ScreenManager.instance;
		if (instance == null)
		{
			Debug.LogWarning("RegisterMenu::OnDestroy() - Could not find scene manager!");
			return;
		}
		instance.UnRegister(this);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00003022 File Offset: 0x00001222
	private void Update()
	{
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0003D3E4 File Offset: 0x0003B5E4
	public void SetInteractable(bool bActive)
	{
		if (this.canvasGroup != null)
		{
			CanvasGroup canvasGroup = this.canvasGroup;
			this.canvasGroup.interactable = bActive;
			canvasGroup.blocksRaycasts = bActive;
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00020E80 File Offset: 0x0001F080
	public void SetEnabled(bool bOn)
	{
		base.gameObject.SetActive(bOn);
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0003D41B File Offset: 0x0003B61B
	public void SetAnimation(int triggerId, bool bOn)
	{
		if (this.animator != null)
		{
			this.animator.SetBool(triggerId, bOn);
		}
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0003D438 File Offset: 0x0003B638
	public bool HasAnimation()
	{
		return this.animator != null;
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0003D446 File Offset: 0x0003B646
	public Animator GetAnimation()
	{
		return this.animator;
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0003D44E File Offset: 0x0003B64E
	public string GetName()
	{
		return this.sceneName;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0003D456 File Offset: 0x0003B656
	public virtual void OnMenuStart()
	{
		this.m_OnEnterMenu.Invoke();
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0003D463 File Offset: 0x0003B663
	public virtual void OnMenuEnd(bool bUnderPopup)
	{
		this.m_OnExitMenu.Invoke(bUnderPopup);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00032E51 File Offset: 0x00031051
	private void DisconnectFromNetwork()
	{
		Network.Disconnect();
	}

	// Token: 0x0400098D RID: 2445
	[Header("Name must be unique for each screen")]
	public string menuName;

	// Token: 0x0400098E RID: 2446
	[Tooltip("Forced on for mobile")]
	public bool isWaitForAnimation;

	// Token: 0x0400098F RID: 2447
	public bool isPopup;

	// Token: 0x04000990 RID: 2448
	public bool isBuiltInPopup;

	// Token: 0x04000991 RID: 2449
	[SerializeField]
	protected RegisterMenu.EnterMenuEvent m_OnEnterMenu = new RegisterMenu.EnterMenuEvent();

	// Token: 0x04000992 RID: 2450
	[SerializeField]
	protected RegisterMenu.ExitMenuEvent m_OnExitMenu = new RegisterMenu.ExitMenuEvent();

	// Token: 0x04000993 RID: 2451
	protected Animator animator;

	// Token: 0x04000994 RID: 2452
	protected CanvasGroup canvasGroup;

	// Token: 0x04000995 RID: 2453
	protected string sceneName;

	// Token: 0x020007B2 RID: 1970
	[Serializable]
	public class EnterMenuEvent : UnityEvent
	{
	}

	// Token: 0x020007B3 RID: 1971
	[Serializable]
	public class ExitMenuEvent : UnityEvent<bool>
	{
	}
}
