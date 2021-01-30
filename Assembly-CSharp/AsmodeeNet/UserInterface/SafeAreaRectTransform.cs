using System;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000630 RID: 1584
	[RequireComponent(typeof(RectTransform))]
	public class SafeAreaRectTransform : MonoBehaviour
	{
		// Token: 0x06003A4A RID: 14922 RVA: 0x00121921 File Offset: 0x0011FB21
		private void Awake()
		{
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00121930 File Offset: 0x0011FB30
		private void Update()
		{
			Rect safeArea = Screen.safeArea;
			if (!MathUtils.Approximately(this._safeArea, safeArea, 0.01f))
			{
				this._safeArea = safeArea;
				float num = 1f / (float)Screen.width;
				float num2 = 1f / (float)Screen.height;
				this._rectTransform.anchorMin = new Vector2(safeArea.x * num, safeArea.y * num2);
				this._rectTransform.anchorMax = new Vector2((safeArea.x + safeArea.width) * num, (safeArea.y + safeArea.height) * num2);
			}
		}

		// Token: 0x040025AC RID: 9644
		private const string _documentation = "<b>SafeAreaRectTransform</b> automatically modifies its <b>Anchors</b> according to <b>Screen.safeArea</b>.";

		// Token: 0x040025AD RID: 9645
		private RectTransform _rectTransform;

		// Token: 0x040025AE RID: 9646
		private Rect _safeArea = Rect.zero;
	}
}
