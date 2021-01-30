using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000188 RID: 392
	[AddComponentMenu("UI/Extensions/TextPic")]
	[ExecuteInEditMode]
	public class TextPic : Text, IPointerClickHandler, IEventSystemHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0005F527 File Offset: 0x0005D727
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x0005F52F File Offset: 0x0005D72F
		public bool AllowClickParents
		{
			get
			{
				return this.m_ClickParents;
			}
			set
			{
				this.m_ClickParents = value;
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0005F538 File Offset: 0x0005D738
		public override void SetVerticesDirty()
		{
			base.SetVerticesDirty();
			this.UpdateQuadImage();
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0005F548 File Offset: 0x0005D748
		private new void Start()
		{
			this.button = base.GetComponentInParent<Button>();
			if (this.button != null)
			{
				CanvasGroup canvasGroup = base.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
				canvasGroup.blocksRaycasts = false;
				this.highlightselectable = canvasGroup.GetComponent<Selectable>();
			}
			else
			{
				this.highlightselectable = base.GetComponent<Selectable>();
			}
			this.Reset_m_HrefInfos();
			base.Start();
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0005F5B8 File Offset: 0x0005D7B8
		protected void UpdateQuadImage()
		{
			this.m_OutputText = this.GetOutputText();
			this.m_ImagesVertexIndex.Clear();
			foreach (object obj in TextPic.s_Regex.Matches(this.m_OutputText))
			{
				Match match = (Match)obj;
				int item = match.Index * 4 + 3;
				this.m_ImagesVertexIndex.Add(item);
				this.m_ImagesPool.RemoveAll((Image image) => image == null);
				if (this.m_ImagesPool.Count == 0)
				{
					base.GetComponentsInChildren<Image>(this.m_ImagesPool);
				}
				if (this.m_ImagesVertexIndex.Count > this.m_ImagesPool.Count)
				{
					GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
					gameObject.layer = base.gameObject.layer;
					RectTransform rectTransform = gameObject.transform as RectTransform;
					if (rectTransform)
					{
						rectTransform.SetParent(base.rectTransform);
						rectTransform.localPosition = Vector3.zero;
						rectTransform.localRotation = Quaternion.identity;
						rectTransform.localScale = Vector3.one;
					}
					this.m_ImagesPool.Add(gameObject.GetComponent<Image>());
				}
				string value = match.Groups[1].Value;
				Image image2 = this.m_ImagesPool[this.m_ImagesVertexIndex.Count - 1];
				Vector2 b = Vector2.zero;
				if ((image2.sprite == null || image2.sprite.name != value) && this.inspectorIconList != null && this.inspectorIconList.Length != 0)
				{
					foreach (TextPic.IconName iconName in this.inspectorIconList)
					{
						if (iconName.name == value)
						{
							image2.sprite = iconName.sprite;
							image2.rectTransform.sizeDelta = new Vector2((float)base.fontSize * this.ImageScalingFactor * iconName.scale.x, (float)base.fontSize * this.ImageScalingFactor * iconName.scale.y);
							b = iconName.offset;
							break;
						}
					}
				}
				image2.enabled = true;
				if (this.positions.Count == this.m_ImagesPool.Count)
				{
					RectTransform rectTransform2 = image2.rectTransform;
					List<Vector2> list = this.positions;
					int i = this.m_ImagesVertexIndex.Count - 1;
					rectTransform2.anchoredPosition = (list[i] += b);
				}
			}
			for (int j = this.m_ImagesVertexIndex.Count; j < this.m_ImagesPool.Count; j++)
			{
				if (this.m_ImagesPool[j])
				{
					this.m_ImagesPool[j].gameObject.SetActive(false);
					this.m_ImagesPool[j].gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.culled_ImagesPool.Add(this.m_ImagesPool[j].gameObject);
					this.m_ImagesPool.Remove(this.m_ImagesPool[j]);
				}
			}
			if (this.culled_ImagesPool.Count > 1)
			{
				this.clearImages = true;
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0005F94C File Offset: 0x0005DB4C
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			string text = this.m_Text;
			this.m_Text = this.GetOutputText();
			base.OnPopulateMesh(toFill);
			this.m_Text = text;
			this.positions.Clear();
			UIVertex uivertex = default(UIVertex);
			for (int i = 0; i < this.m_ImagesVertexIndex.Count; i++)
			{
				int num = this.m_ImagesVertexIndex[i];
				Vector2 sizeDelta = this.m_ImagesPool[i].rectTransform.sizeDelta;
				if (num < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref uivertex, num);
					this.positions.Add(new Vector2(uivertex.position.x + sizeDelta.x / 2f, uivertex.position.y + sizeDelta.y / 2f) + this.imageOffset);
					toFill.PopulateUIVertex(ref uivertex, num - 3);
					Vector3 position = uivertex.position;
					int j = num;
					int num2 = num - 3;
					while (j > num2)
					{
						toFill.PopulateUIVertex(ref uivertex, num);
						uivertex.position = position;
						toFill.SetUIVertex(uivertex, j);
						j--;
					}
				}
			}
			if (this.m_ImagesVertexIndex.Count != 0)
			{
				this.m_ImagesVertexIndex.Clear();
			}
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				hrefInfo.boxes.Clear();
				if (hrefInfo.startIndex < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref uivertex, hrefInfo.startIndex);
					Vector3 position2 = uivertex.position;
					Bounds bounds = new Bounds(position2, Vector3.zero);
					int num3 = hrefInfo.startIndex;
					int endIndex = hrefInfo.endIndex;
					while (num3 < endIndex && num3 < toFill.currentVertCount)
					{
						toFill.PopulateUIVertex(ref uivertex, num3);
						position2 = uivertex.position;
						if (position2.x < bounds.min.x)
						{
							hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
							bounds = new Bounds(position2, Vector3.zero);
						}
						else
						{
							bounds.Encapsulate(position2);
						}
						num3++;
					}
					hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
				}
			}
			this.UpdateQuadImage();
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0005FBE4 File Offset: 0x0005DDE4
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0005FBEC File Offset: 0x0005DDEC
		public TextPic.HrefClickEvent onHrefClick
		{
			get
			{
				return this.m_OnHrefClick;
			}
			set
			{
				this.m_OnHrefClick = value;
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0005FBF8 File Offset: 0x0005DDF8
		protected string GetOutputText()
		{
			TextPic.s_TextBuilder.Length = 0;
			int num = 0;
			this.fixedString = this.text;
			if (this.inspectorIconList != null && this.inspectorIconList.Length != 0)
			{
				foreach (TextPic.IconName iconName in this.inspectorIconList)
				{
					if (iconName.name != null && iconName.name != "")
					{
						this.fixedString = this.fixedString.Replace(iconName.name, string.Concat(new object[]
						{
							"<quad name=",
							iconName.name,
							" size=",
							base.fontSize,
							" width=1 />"
						}));
					}
				}
			}
			int num2 = 0;
			foreach (object obj in TextPic.s_HrefRegex.Matches(this.fixedString))
			{
				Match match = (Match)obj;
				TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, match.Index - num));
				TextPic.s_TextBuilder.Append("<color=" + this.hyperlinkColor + ">");
				Group group = match.Groups[1];
				if (this.isCreating_m_HrefInfos)
				{
					TextPic.HrefInfo item = new TextPic.HrefInfo
					{
						startIndex = TextPic.s_TextBuilder.Length * 4,
						endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
						name = group.Value
					};
					this.m_HrefInfos.Add(item);
				}
				else if (this.m_HrefInfos.Count > 0)
				{
					this.m_HrefInfos[num2].startIndex = TextPic.s_TextBuilder.Length * 4;
					this.m_HrefInfos[num2].endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3;
					num2++;
				}
				TextPic.s_TextBuilder.Append(match.Groups[2].Value);
				TextPic.s_TextBuilder.Append("</color>");
				num = match.Index + match.Length;
			}
			if (this.isCreating_m_HrefInfos)
			{
				this.isCreating_m_HrefInfos = false;
			}
			TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, this.fixedString.Length - num));
			return TextPic.s_TextBuilder.ToString();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0005FEC4 File Offset: 0x0005E0C4
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out point);
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				List<Rect> boxes = hrefInfo.boxes;
				for (int i = 0; i < boxes.Count; i++)
				{
					if (boxes[i].Contains(point))
					{
						this.m_OnHrefClick.Invoke(hrefInfo.name);
						return;
					}
				}
			}
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0005FF70 File Offset: 0x0005E170
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00060000 File Offset: 0x0005E200
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.normalColor;
					}
					else
					{
						image.color = this.color;
					}
				}
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0006009C File Offset: 0x0005E29C
		public void OnSelect(BaseEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0006012C File Offset: 0x0005E32C
		private void Update()
		{
			Object obj = this.thisLock;
			lock (obj)
			{
				if (this.clearImages)
				{
					for (int i = 0; i < this.culled_ImagesPool.Count; i++)
					{
						Object.DestroyImmediate(this.culled_ImagesPool[i]);
					}
					this.culled_ImagesPool.Clear();
					this.clearImages = false;
				}
			}
			if (this.previousText != this.text)
			{
				this.Reset_m_HrefInfos();
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000601C0 File Offset: 0x0005E3C0
		private void Reset_m_HrefInfos()
		{
			this.previousText = this.text;
			this.m_HrefInfos.Clear();
			this.isCreating_m_HrefInfos = true;
		}

		// Token: 0x04000E93 RID: 3731
		private readonly List<Image> m_ImagesPool = new List<Image>();

		// Token: 0x04000E94 RID: 3732
		private readonly List<GameObject> culled_ImagesPool = new List<GameObject>();

		// Token: 0x04000E95 RID: 3733
		private bool clearImages;

		// Token: 0x04000E96 RID: 3734
		private Object thisLock = new Object();

		// Token: 0x04000E97 RID: 3735
		private readonly List<int> m_ImagesVertexIndex = new List<int>();

		// Token: 0x04000E98 RID: 3736
		private static readonly Regex s_Regex = new Regex("<quad name=(.+?) size=(\\d*\\.?\\d+%?) width=(\\d*\\.?\\d+%?) />", RegexOptions.Singleline);

		// Token: 0x04000E99 RID: 3737
		private string fixedString;

		// Token: 0x04000E9A RID: 3738
		[SerializeField]
		[Tooltip("Allow click events to be received by parents, (default) blocks")]
		private bool m_ClickParents;

		// Token: 0x04000E9B RID: 3739
		private string m_OutputText;

		// Token: 0x04000E9C RID: 3740
		public TextPic.IconName[] inspectorIconList;

		// Token: 0x04000E9D RID: 3741
		[Tooltip("Global scaling factor for all images")]
		public float ImageScalingFactor = 1f;

		// Token: 0x04000E9E RID: 3742
		public string hyperlinkColor = "blue";

		// Token: 0x04000E9F RID: 3743
		[SerializeField]
		public Vector2 imageOffset = Vector2.zero;

		// Token: 0x04000EA0 RID: 3744
		private Button button;

		// Token: 0x04000EA1 RID: 3745
		private Selectable highlightselectable;

		// Token: 0x04000EA2 RID: 3746
		private List<Vector2> positions = new List<Vector2>();

		// Token: 0x04000EA3 RID: 3747
		private string previousText = "";

		// Token: 0x04000EA4 RID: 3748
		public bool isCreating_m_HrefInfos = true;

		// Token: 0x04000EA5 RID: 3749
		private readonly List<TextPic.HrefInfo> m_HrefInfos = new List<TextPic.HrefInfo>();

		// Token: 0x04000EA6 RID: 3750
		private static readonly StringBuilder s_TextBuilder = new StringBuilder();

		// Token: 0x04000EA7 RID: 3751
		private static readonly Regex s_HrefRegex = new Regex("<a href=([^>\\n\\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

		// Token: 0x04000EA8 RID: 3752
		[SerializeField]
		private TextPic.HrefClickEvent m_OnHrefClick = new TextPic.HrefClickEvent();

		// Token: 0x0200084B RID: 2123
		[Serializable]
		public struct IconName
		{
			// Token: 0x04002EAD RID: 11949
			public string name;

			// Token: 0x04002EAE RID: 11950
			public Sprite sprite;

			// Token: 0x04002EAF RID: 11951
			public Vector2 offset;

			// Token: 0x04002EB0 RID: 11952
			public Vector2 scale;
		}

		// Token: 0x0200084C RID: 2124
		[Serializable]
		public class HrefClickEvent : UnityEvent<string>
		{
		}

		// Token: 0x0200084D RID: 2125
		private class HrefInfo
		{
			// Token: 0x04002EB1 RID: 11953
			public int startIndex;

			// Token: 0x04002EB2 RID: 11954
			public int endIndex;

			// Token: 0x04002EB3 RID: 11955
			public string name;

			// Token: 0x04002EB4 RID: 11956
			public readonly List<Rect> boxes = new List<Rect>();
		}
	}
}
