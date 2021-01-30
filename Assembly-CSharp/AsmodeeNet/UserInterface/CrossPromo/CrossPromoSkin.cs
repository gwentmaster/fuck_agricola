using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000655 RID: 1621
	[CreateAssetMenu]
	public class CrossPromoSkin : ScriptableObject
	{
		// Token: 0x0400268E RID: 9870
		[Header("Popup")]
		public Sprite PopupShadow;

		// Token: 0x0400268F RID: 9871
		public int PopupShadowSize;

		// Token: 0x04002690 RID: 9872
		public Sprite PopupWindow;

		// Token: 0x04002691 RID: 9873
		public Sprite PopupHeaderBackground;

		// Token: 0x04002692 RID: 9874
		public TextSkin PopupHeaderText;

		// Token: 0x04002693 RID: 9875
		public Sprite PopupCloseButtonBackground;

		// Token: 0x04002694 RID: 9876
		public Sprite PopupCloseButtonForeground;

		// Token: 0x04002695 RID: 9877
		public Color PopupCloseButtonNormalColor;

		// Token: 0x04002696 RID: 9878
		public Color PopupCloseButtonHighlightedColor;

		// Token: 0x04002697 RID: 9879
		public Color PopupCloseButtonPressedColor;

		// Token: 0x04002698 RID: 9880
		public Color PopupCloseButtonDisabledColor;

		// Token: 0x04002699 RID: 9881
		[Header("Scroll bar")]
		public Sprite ScrollbarLine;

		// Token: 0x0400269A RID: 9882
		public Sprite ScrollbarButton;

		// Token: 0x0400269B RID: 9883
		public Sprite ScrollbarButtonGrip;

		// Token: 0x0400269C RID: 9884
		public Color ScrollbarNormalColor;

		// Token: 0x0400269D RID: 9885
		public Color ScrollbarHighlightedColor;

		// Token: 0x0400269E RID: 9886
		public Color ScrollbarPressedColor;

		// Token: 0x0400269F RID: 9887
		public Color ScrollbarDisabledColor;

		// Token: 0x040026A0 RID: 9888
		[Header("More games")]
		public Sprite FilterHighlighted;

		// Token: 0x040026A1 RID: 9889
		public Sprite FilterPressed;

		// Token: 0x040026A2 RID: 9890
		public Sprite FilterDisabled;

		// Token: 0x040026A3 RID: 9891
		public Color FilterHighlightedTextColor;

		// Token: 0x040026A4 RID: 9892
		public Color FilterNormalTextColor;

		// Token: 0x040026A5 RID: 9893
		[Header("More games - Tile")]
		public int TileBorderSize = 2;

		// Token: 0x040026A6 RID: 9894
		public Color TileBorderColor = Color.white;

		// Token: 0x040026A7 RID: 9895
		public Sprite TileDetailButton;

		// Token: 0x040026A8 RID: 9896
		public Color TileDetailButtonNormalColor;

		// Token: 0x040026A9 RID: 9897
		public Color TileDetailButtonHighlightedColor;

		// Token: 0x040026AA RID: 9898
		public Color TileDetailButtonPressedColor;

		// Token: 0x040026AB RID: 9899
		public Color TileDetailButtonDisabledColor;

		// Token: 0x040026AC RID: 9900
		public Sprite TileLoading;

		// Token: 0x040026AD RID: 9901
		public Color TileLoadingColor;

		// Token: 0x040026AE RID: 9902
		public float TileLoadingSpeed;

		// Token: 0x040026AF RID: 9903
		[Header("More games - Tile - Button")]
		public Color TileButtonColor;

		// Token: 0x040026B0 RID: 9904
		public Sprite TileButton;

		// Token: 0x040026B1 RID: 9905
		[Range(0f, 1f)]
		public float TileButtonAlpha;

		// Token: 0x040026B2 RID: 9906
		public TextSkin TileButtonText;
	}
}
