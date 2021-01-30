using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000643 RID: 1603
	[RequireComponent(typeof(Selectable))]
	public class Focusable : MonoBehaviour
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x00125767 File Offset: 0x00123967
		// (set) Token: 0x06003B0C RID: 15116 RVA: 0x0012576F File Offset: 0x0012396F
		public Selectable Selectable { get; private set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x00125778 File Offset: 0x00123978
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x00125780 File Offset: 0x00123980
		public InputField InputField { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x00125789 File Offset: 0x00123989
		// (set) Token: 0x06003B10 RID: 15120 RVA: 0x00125791 File Offset: 0x00123991
		public TMP_InputField TMP_InputField { get; private set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x0012579A File Offset: 0x0012399A
		public bool IsInputField
		{
			get
			{
				return this.InputField != null || this.TMP_InputField != null;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x001257B8 File Offset: 0x001239B8
		public FocusableLayer FocusableLayer
		{
			get
			{
				Transform transform = base.gameObject.transform;
				while (transform.parent != null)
				{
					FocusableLayer component = transform.parent.GetComponent<FocusableLayer>();
					if (component != null)
					{
						return component;
					}
					transform = transform.parent;
				}
				return CoreApplication.Instance.UINavigationManager.RootFocusableLayer;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x00125810 File Offset: 0x00123A10
		public Vector2 ViewportPosition
		{
			get
			{
				if (this._canvas == null)
				{
					return Camera.main.WorldToViewportPoint(base.transform.position);
				}
				if (this._canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					RectTransform rectTransform = this._canvas.transform as RectTransform;
					Vector2 size = rectTransform.rect.size;
					Vector2 vector = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, base.transform as RectTransform).center;
					return new Vector2(vector.x / size.x + 0.5f, vector.y / size.y + 0.5f);
				}
				Canvas canvas = this._canvas;
				return (((canvas != null) ? canvas.worldCamera : null) ?? Camera.main).WorldToViewportPoint(base.transform.position);
			}
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x001258EC File Offset: 0x00123AEC
		private void Start()
		{
			this.Selectable = base.GetComponent<Selectable>();
			if (this.Selectable == null)
			{
				AsmoLogger.Error("Focusable", "Missing Selectable component", null);
			}
			this._canvas = base.GetComponentInParent<Canvas>();
			if (this.next != null)
			{
				this.next.previous = this;
			}
			this.InputField = base.GetComponent<InputField>();
			this.TMP_InputField = base.GetComponent<TMP_InputField>();
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x00125961 File Offset: 0x00123B61
		private void OnEnable()
		{
			this.FocusableLayer.RegisterFocusable(this);
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0012596F File Offset: 0x00123B6F
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			this.FocusableLayer.UnRegisterFocusable(this);
		}

		// Token: 0x0400263D RID: 9789
		private const string _documentation = "<b>Focusable</b> registers a <b>UI.Selectable</b> control with the <b>FocusManager</b>. It will be part of the UI cross-platform navigation system.";

		// Token: 0x0400263E RID: 9790
		private const string _kModuleName = "Focusable";

		// Token: 0x04002640 RID: 9792
		[Tooltip("Flag this Focusable as the first element to take focus")]
		public bool firstFocusable;

		// Token: 0x04002641 RID: 9793
		public Focusable next;

		// Token: 0x04002642 RID: 9794
		[HideInInspector]
		public Focusable previous;

		// Token: 0x04002645 RID: 9797
		private Canvas _canvas;
	}
}
