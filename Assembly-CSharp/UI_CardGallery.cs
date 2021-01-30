using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000115 RID: 277
public class UI_CardGallery : UI_FrontEndAndInGameScene
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x00044802 File Offset: 0x00042A02
	private void Awake()
	{
		if (this.m_debugHalfHeightToggle != null)
		{
			UnityEngine.Object.Destroy(this.m_debugHalfHeightObject);
			this.m_debugHalfHeightObject = null;
			this.m_debugHalfHeightToggle = null;
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x0004482C File Offset: 0x00042A2C
	public void OnMenuEnter()
	{
		base.SetIsInGame(false);
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.DisableToggleSoundEffects();
		}
		this.m_oldSetFlags = -1;
		this.m_oldTypeFlags = -1;
		KeybindManager instance = KeybindManager.instance;
		instance.AddEvent(new KeybindManager.KeyEvent(this.MoveLeft), "RightArrow", KeybindManager.KeybindEvents.KeyDown);
		instance.AddEvent(new KeybindManager.KeyEvent(this.MoveRight), "LeftArrow", KeybindManager.KeybindEvents.KeyDown);
		instance.AddEvent(new KeybindManager.KeyEvent(this.CardTypeUp), "UpArrow", KeybindManager.KeybindEvents.KeyDownTrigger);
		instance.AddEvent(new KeybindManager.KeyEvent(this.CardTypeDown), "DownArrow", KeybindManager.KeybindEvents.KeyDownTrigger);
		this.ClearCardLists();
		this.RebuildCardLists();
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.EnableToggleSoundEffects();
		}
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00044900 File Offset: 0x00042B00
	public void OnMenuExit(bool bUnderPopup)
	{
		KeybindManager instance = KeybindManager.instance;
		instance.RemoveEvent(new KeybindManager.KeyEvent(this.MoveLeft), "RightArrow", KeybindManager.KeybindEvents.KeyDown);
		instance.RemoveEvent(new KeybindManager.KeyEvent(this.MoveRight), "LeftArrow", KeybindManager.KeybindEvents.KeyDown);
		instance.RemoveEvent(new KeybindManager.KeyEvent(this.CardTypeUp), "UpArrow", KeybindManager.KeybindEvents.KeyDownTrigger);
		instance.RemoveEvent(new KeybindManager.KeyEvent(this.CardTypeDown), "DownArrow", KeybindManager.KeybindEvents.KeyDownTrigger);
		this.ResetNodes();
		for (int i = 0; i < this.m_cards.Length; i++)
		{
			for (int j = 0; j < this.m_cards[i].cards.GetLength(0); j++)
			{
				if (this.m_cards[i].cards[j].flipCard != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_cards[i].cards[j].flipCard.gameObject);
					this.m_cards[i].cards[j].flipCard = null;
				}
				if (this.m_cards[i].cards[j].card != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_cards[i].cards[j].card.gameObject);
					this.m_cards[i].cards[j].card = null;
				}
			}
		}
		foreach (object obj in this.m_limbo.transform)
		{
			UnityEngine.Object.DestroyImmediate(((Transform)obj).gameObject);
		}
		if (this.m_cardManagerFromResouces != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_cardManagerFromResouces);
			this.m_cardManagerFromResouces = null;
			this.m_cardManager = null;
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x00044B08 File Offset: 0x00042D08
	public void SetStartingSet(int setIndex)
	{
		this.m_startingSetIndex = setIndex;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00044B14 File Offset: 0x00042D14
	public void CardTypeUp()
	{
		if (this.m_loadingScreen.activeSelf)
		{
			return;
		}
		this.m_bIgnoreToggles = true;
		for (int i = 0; i < this.m_typeToggles.Length; i++)
		{
			if (this.m_typeToggles[i].isOn && i > 0)
			{
				this.m_typeToggles[i - 1].isOn = true;
				this.m_typeToggles[i].isOn = false;
				this.m_bIgnoreToggles = false;
				this.RebuildCardLists();
				return;
			}
		}
		this.m_bIgnoreToggles = false;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00044B90 File Offset: 0x00042D90
	public void CardTypeDown()
	{
		if (this.m_loadingScreen.activeSelf)
		{
			return;
		}
		this.m_bIgnoreToggles = true;
		for (int i = 0; i < this.m_typeToggles.Length; i++)
		{
			if (this.m_typeToggles[i].isOn && i < this.m_typeToggles.Length - 1)
			{
				this.m_typeToggles[i + 1].isOn = true;
				this.m_typeToggles[i].isOn = false;
				this.m_bIgnoreToggles = false;
				this.RebuildCardLists();
				return;
			}
		}
		this.m_bIgnoreToggles = false;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00044C14 File Offset: 0x00042E14
	public void MoveLeft()
	{
		if (this.m_loadingScreen.activeSelf || this.m_bIgnoreMove || this.GetIndex(this.m_nodes[this.m_currentNodeIndex].m_index, 1) == -1)
		{
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			this.m_nodes[i].MoveLeft();
		}
		int num = this.m_currentNodeIndex + 1;
		this.m_currentNodeIndex = num;
		if (num >= 7)
		{
			this.m_currentNodeIndex = 0;
		}
		this.m_currentIndex = this.m_nodes[this.m_currentNodeIndex].m_index;
		int index = this.GetIndex(this.m_nodes[this.m_currentNodeIndex].m_index, 3);
		int num2 = this.m_currentNodeIndex + 3;
		if (num2 >= 7)
		{
			num2 -= 7;
		}
		if (index != -1)
		{
			this.m_nodes[num2].SetCard(this.m_cards[this.m_currentSetIndex].cards[index].card, this.m_cards[this.m_currentSetIndex].cards[index].flipCard, index, this.m_cards[this.m_currentSetIndex].cards[index].isHorizontal ? this.m_scaleForHorizontal : this.m_scaleForVertical);
		}
		else
		{
			this.m_nodes[num2].ClearCard();
		}
		this.m_bIgnoreMove = true;
		base.StartCoroutine(this.MoveLockout());
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00044D78 File Offset: 0x00042F78
	public void MoveRight()
	{
		if (this.m_loadingScreen.activeSelf || this.m_bIgnoreMove || this.GetIndex(this.m_nodes[this.m_currentNodeIndex].m_index, -1) == -1)
		{
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			this.m_nodes[i].MoveRight();
		}
		int num = this.m_currentNodeIndex - 1;
		this.m_currentNodeIndex = num;
		if (num < 0)
		{
			this.m_currentNodeIndex = 6;
		}
		this.m_currentIndex = this.m_nodes[this.m_currentNodeIndex].m_index;
		int index = this.GetIndex(this.m_nodes[this.m_currentNodeIndex].m_index, -3);
		int num2 = this.m_currentNodeIndex - 3;
		if (num2 < 0)
		{
			num2 += 7;
		}
		if (index != -1)
		{
			this.m_nodes[num2].SetCard(this.m_cards[this.m_currentSetIndex].cards[index].card, this.m_cards[this.m_currentSetIndex].cards[index].flipCard, index, this.m_cards[this.m_currentSetIndex].cards[index].isHorizontal ? this.m_scaleForHorizontal : this.m_scaleForVertical);
		}
		else
		{
			this.m_nodes[num2].ClearCard();
		}
		this.m_bIgnoreMove = true;
		base.StartCoroutine(this.MoveLockout());
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00044EDA File Offset: 0x000430DA
	private IEnumerator MoveLockout()
	{
		int num;
		for (int i = 0; i < 12; i = num)
		{
			yield return new WaitForEndOfFrame();
			num = i + 1;
		}
		this.m_bIgnoreMove = false;
		yield break;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00044EE9 File Offset: 0x000430E9
	public void FlipCard()
	{
		this.m_nodes[this.m_currentNodeIndex].FlipCard(this.m_cards[this.m_currentSetIndex].cards[this.m_currentIndex].isHorizontal);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00003022 File Offset: 0x00001222
	private void SetInfoBox()
	{
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00044F24 File Offset: 0x00043124
	private void ResetNodes()
	{
		for (int i = 0; i < 7; i++)
		{
			this.m_nodes[i].ClearCard();
			this.m_nodes[i].ForceMove(i + 1);
		}
		this.m_currentNodeIndex = 3;
		this.m_currentIndex = 0;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00044F68 File Offset: 0x00043168
	public void IAP_SetStartingCardSet(int index)
	{
		if (index < this.m_cards.Length)
		{
			this.m_currentSetIndex = index;
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00044F7C File Offset: 0x0004317C
	public void OnTypeToggle(int toggleChangedIndex)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.m_typeToggles.Length; i++)
		{
			if (this.m_typeToggles[i].isOn)
			{
				num++;
			}
		}
		if (num > 0)
		{
			this.RebuildCardLists();
			return;
		}
		bool bIgnoreToggles = this.m_bIgnoreToggles;
		this.m_bIgnoreToggles = true;
		this.m_typeToggles[toggleChangedIndex].isOn = true;
		this.m_bIgnoreToggles = bIgnoreToggles;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00044FE8 File Offset: 0x000431E8
	public void RebuildCardLists()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		base.StartCoroutine(this.RebuildCardListsAsync());
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00045000 File Offset: 0x00043200
	private IEnumerator RebuildCardListsAsync()
	{
		int setFlags = 0;
		int num = 0;
		for (int i = 0; i < this.m_setToggles.Length; i++)
		{
			if (this.m_setToggles[i].isOn)
			{
				num++;
				setFlags = i;
			}
		}
		if (num != 1)
		{
			yield break;
		}
		num = 0;
		int typeFlags = 0;
		for (int j = 0; j < this.m_typeToggles.Length; j++)
		{
			if (this.m_typeToggles[j].isOn)
			{
				num++;
				typeFlags |= 1 << j;
			}
		}
		if (num < 1)
		{
			yield break;
		}
		if (this.m_oldSetFlags == setFlags && this.m_oldTypeFlags == typeFlags)
		{
			yield break;
		}
		if (this.m_cardManager == null && this.m_cardManagerPath != string.Empty)
		{
			this.m_loadingScreen.SetActive(true);
			ResourceRequest request = Resources.LoadAsync<GameObject>(this.m_cardManagerPath);
			yield return request;
			if (request.isDone)
			{
				yield return new WaitForSeconds(0.25f);
				this.m_cardManagerFromResouces = (UnityEngine.Object.Instantiate(request.asset) as GameObject);
				this.m_cardManager = this.m_cardManagerFromResouces.GetComponent<AgricolaCardManager>();
			}
			if (this.m_cardManager == null)
			{
				this.m_loadingScreen.SetActive(false);
				Debug.LogError("Card Gallery has no reference to card manager!");
				yield break;
			}
			request = null;
		}
		this.m_loadingScreen.SetActive(true);
		this.m_oldTypeFlags = typeFlags;
		this.m_oldSetFlags = setFlags;
		this.m_currentSetIndex = setFlags;
		this.ClearCardLists();
		this.m_bIgnoreToggles = true;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("Rebuild Card List");
		this.m_currentIndex = -1;
		for (int k = 0; k < this.m_cards[this.m_currentSetIndex].cards.GetLength(0); k++)
		{
			UI_CardGallery.CardGalleryCard cardGalleryCard = this.m_cards[this.m_currentSetIndex].cards[k];
			if (cardGalleryCard.cardType != UI_CardGallery.CardGalleryType.Never && (cardGalleryCard.cardType == UI_CardGallery.CardGalleryType.All || (cardGalleryCard.cardType & (UI_CardGallery.CardGalleryType)typeFlags) != UI_CardGallery.CardGalleryType.All))
			{
				this.m_cards[this.m_currentSetIndex].cards[k].card = this.m_cardManager.CreateCardFromName(this.m_cards[this.m_currentSetIndex].cards[k].cardFileName, false, true);
				if (this.m_cards[this.m_currentSetIndex].cards[k].card == null)
				{
					Debug.LogWarning("CardGallery: Unable to get card: " + this.m_cards[this.m_currentSetIndex].cards[k].cardFileName);
				}
				else
				{
					if (this.m_limbo != null)
					{
						this.m_cards[this.m_currentSetIndex].cards[k].card.transform.SetParent(this.m_limbo.transform);
					}
					else
					{
						Debug.LogWarning("m_limbo is null!");
					}
					AgricolaCard component = this.m_cards[this.m_currentSetIndex].cards[k].card.GetComponent<AgricolaCard>();
					if (component != null)
					{
						component.DisplayCardBack(true, true);
						if (this.m_debugHalfHeightToggle != null && this.m_debugHalfHeightToggle.isOn)
						{
							component.ShowHalfCard(-1f);
						}
						else
						{
							component.ShowFullCard(-1f);
						}
					}
					if (component.GetCardType() == 7)
					{
						this.m_cards[this.m_currentSetIndex].cards[k].card.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
					}
					if (this.m_currentIndex == -1 && this.m_cards[this.m_currentSetIndex].cards[k].card != null)
					{
						this.m_currentIndex = k;
					}
				}
			}
		}
		if (this.m_currentIndex == -1)
		{
			this.m_bIgnoreToggles = false;
			this.m_loadingScreen.SetActive(false);
			yield break;
		}
		for (;;)
		{
			bool flag = true;
			for (int l = 0; l < this.m_cards[this.m_currentSetIndex].cards.GetLength(0); l++)
			{
				UI_CardGallery.CardGalleryCard cardGalleryCard2 = this.m_cards[this.m_currentSetIndex].cards[l];
				if (cardGalleryCard2.card != null)
				{
					AgricolaCard component2 = cardGalleryCard2.card.GetComponent<AgricolaCard>();
					if (component2 != null && !component2.HasLoadCompleted())
					{
						flag = false;
						break;
					}
				}
				if (cardGalleryCard2.flipCard != null)
				{
					AgricolaCard component3 = cardGalleryCard2.flipCard.GetComponent<AgricolaCard>();
					if (component3 != null && !component3.HasLoadCompleted())
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return new WaitForEndOfFrame();
		for (int m = 0; m < this.m_cards[this.m_currentSetIndex].cards.GetLength(0); m++)
		{
			UI_CardGallery.CardGalleryCard cardGalleryCard3 = this.m_cards[this.m_currentSetIndex].cards[m];
			if (cardGalleryCard3.card != null)
			{
				cardGalleryCard3.card.GetComponent<AgricolaCard>().DisplayCardBack(false, true);
			}
			if (cardGalleryCard3.flipCard != null)
			{
				cardGalleryCard3.flipCard.GetComponent<AgricolaCard>().DisplayCardBack(false, true);
			}
		}
		yield return new WaitForEndOfFrame();
		for (int n = -3; n <= 3; n++)
		{
			int index = this.GetIndex(this.m_currentIndex, n);
			if (index != -1)
			{
				this.m_nodes[n + 3].SetCard(this.m_cards[this.m_currentSetIndex].cards[index].card, this.m_cards[this.m_currentSetIndex].cards[index].flipCard, index, this.m_cards[this.m_currentSetIndex].cards[index].isHorizontal ? this.m_scaleForHorizontal : this.m_scaleForVertical);
			}
			else
			{
				this.m_nodes[n + 3].ClearCard();
			}
		}
		if (this.m_debugHalfHeightToggle != null)
		{
			this.m_bOldHHToggle = this.m_debugHalfHeightToggle.isOn;
		}
		yield return new WaitForEndOfFrame();
		this.m_bIgnoreToggles = false;
		this.m_loadingScreen.SetActive(false);
		yield break;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00045010 File Offset: 0x00043210
	private int GetIndex(int startingIndex, int steps)
	{
		int num = startingIndex;
		while (steps != 0)
		{
			num += ((steps > 0) ? 1 : -1);
			if (num < 0)
			{
				num = this.m_cards[this.m_currentSetIndex].cards.Length - 1;
			}
			else if (num > this.m_cards[this.m_currentSetIndex].cards.Length - 1)
			{
				num = 0;
			}
			if (this.m_cards[this.m_currentSetIndex].cards[num].card != null)
			{
				steps += ((steps > 0) ? -1 : 1);
			}
			if (num == startingIndex)
			{
				return 0;
			}
		}
		return num;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000450B4 File Offset: 0x000432B4
	private void ClearCardLists()
	{
		this.ResetNodes();
		for (int i = 0; i < this.m_cards.Length; i++)
		{
			for (int j = 0; j < this.m_cards[i].cards.GetLength(0); j++)
			{
				if (this.m_cards[i].cards[j].flipCard != null)
				{
					UnityEngine.Object.Destroy(this.m_cards[i].cards[j].flipCard.gameObject);
					this.m_cards[i].cards[j].flipCard = null;
				}
				if (this.m_cards[i].cards[j].card != null)
				{
					UnityEngine.Object.Destroy(this.m_cards[i].cards[j].card.gameObject);
					this.m_cards[i].cards[j].card = null;
				}
			}
		}
		foreach (object obj in this.m_limbo.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x04000AE9 RID: 2793
	[Tooltip("Must be size 7")]
	public UIP_CardGalleryNode[] m_nodes;

	// Token: 0x04000AEA RID: 2794
	public UI_CardGallery.CardGalleryCardSet[] m_cards;

	// Token: 0x04000AEB RID: 2795
	[SerializeField]
	private AgricolaCardManager m_cardManager;

	// Token: 0x04000AEC RID: 2796
	public GameObject m_limbo;

	// Token: 0x04000AED RID: 2797
	public float m_scaleForVertical = 1f;

	// Token: 0x04000AEE RID: 2798
	public float m_scaleForHorizontal = 1f;

	// Token: 0x04000AEF RID: 2799
	public Toggle[] m_setToggles;

	// Token: 0x04000AF0 RID: 2800
	public Toggle[] m_typeToggles;

	// Token: 0x04000AF1 RID: 2801
	public GameObject m_debugHalfHeightObject;

	// Token: 0x04000AF2 RID: 2802
	public Toggle m_debugHalfHeightToggle;

	// Token: 0x04000AF3 RID: 2803
	public GameObject m_loadingScreen;

	// Token: 0x04000AF4 RID: 2804
	public float m_MinSwipeDistance;

	// Token: 0x04000AF5 RID: 2805
	public string m_cardManagerPath;

	// Token: 0x04000AF6 RID: 2806
	private bool m_bIgnoreToggles;

	// Token: 0x04000AF7 RID: 2807
	private int m_currentIndex;

	// Token: 0x04000AF8 RID: 2808
	private int m_currentNodeIndex;

	// Token: 0x04000AF9 RID: 2809
	private int m_currentSetIndex;

	// Token: 0x04000AFA RID: 2810
	private int m_oldSetFlags = -1;

	// Token: 0x04000AFB RID: 2811
	private int m_oldTypeFlags = -1;

	// Token: 0x04000AFC RID: 2812
	private bool m_bOldHHToggle = true;

	// Token: 0x04000AFD RID: 2813
	private int m_startingSetIndex = -1;

	// Token: 0x04000AFE RID: 2814
	private Vector2 m_startTouch = Vector2.zero;

	// Token: 0x04000AFF RID: 2815
	private bool m_bIsTouching;

	// Token: 0x04000B00 RID: 2816
	private bool m_bIgnoreMove;

	// Token: 0x04000B01 RID: 2817
	private GameObject m_cardManagerFromResouces;

	// Token: 0x020007E8 RID: 2024
	public enum CardGalleryType
	{
		// Token: 0x04002D51 RID: 11601
		All,
		// Token: 0x04002D52 RID: 11602
		A,
		// Token: 0x04002D53 RID: 11603
		B,
		// Token: 0x04002D54 RID: 11604
		C = 4,
		// Token: 0x04002D55 RID: 11605
		D = 8,
		// Token: 0x04002D56 RID: 11606
		Never = -1
	}

	// Token: 0x020007E9 RID: 2025
	public enum CardGallerySet
	{
		// Token: 0x04002D58 RID: 11608
		Action,
		// Token: 0x04002D59 RID: 11609
		MajorImprovement,
		// Token: 0x04002D5A RID: 11610
		MinorImprovement,
		// Token: 0x04002D5B RID: 11611
		Occupation
	}

	// Token: 0x020007EA RID: 2026
	[Serializable]
	public struct CardGalleryCard
	{
		// Token: 0x04002D5C RID: 11612
		[HideInInspector]
		public GameObject card;

		// Token: 0x04002D5D RID: 11613
		[HideInInspector]
		public GameObject flipCard;

		// Token: 0x04002D5E RID: 11614
		public UI_CardGallery.CardGalleryType cardType;

		// Token: 0x04002D5F RID: 11615
		public bool isHorizontal;

		// Token: 0x04002D60 RID: 11616
		public string cardFileName;
	}

	// Token: 0x020007EB RID: 2027
	[Serializable]
	public struct CardGalleryCardSet
	{
		// Token: 0x04002D61 RID: 11617
		public UI_CardGallery.CardGallerySet cardSet;

		// Token: 0x04002D62 RID: 11618
		public UI_CardGallery.CardGalleryCard[] cards;
	}
}
