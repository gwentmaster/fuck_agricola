using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001E4 RID: 484
	[RequireComponent(typeof(BoxSlider), typeof(RawImage))]
	[ExecuteInEditMode]
	public class SVBoxSlider : MonoBehaviour
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0006F795 File Offset: 0x0006D995
		public RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00070459 File Offset: 0x0006E659
		private void Awake()
		{
			this.slider = base.GetComponent<BoxSlider>();
			this.image = base.GetComponent<RawImage>();
			this.RegenerateSVTexture();
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0007047C File Offset: 0x0006E67C
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.slider.OnValueChanged.AddListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000704D8 File Offset: 0x0006E6D8
		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.slider.OnValueChanged.RemoveListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0007052B File Offset: 0x0006E72B
		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00070550 File Offset: 0x0006E750
		private void SliderChanged(float saturation, float value)
		{
			if (this.listen)
			{
				this.picker.AssignColor(ColorValues.Saturation, saturation);
				this.picker.AssignColor(ColorValues.Value, value);
			}
			this.listen = true;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0007057C File Offset: 0x0006E77C
		private void HSVChanged(float h, float s, float v)
		{
			if (this.lastH != h)
			{
				this.lastH = h;
				this.RegenerateSVTexture();
			}
			if (s != this.slider.NormalizedValueX)
			{
				this.listen = false;
				this.slider.NormalizedValueX = s;
			}
			if (v != this.slider.NormalizedValueY)
			{
				this.listen = false;
				this.slider.NormalizedValueY = v;
			}
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000705E4 File Offset: 0x0006E7E4
		private void RegenerateSVTexture()
		{
			double h = (double)((this.picker != null) ? (this.picker.H * 360f) : 0f);
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
			Texture2D texture2D = new Texture2D(100, 100)
			{
				hideFlags = HideFlags.DontSave
			};
			for (int i = 0; i < 100; i++)
			{
				Color32[] array = new Color32[100];
				for (int j = 0; j < 100; j++)
				{
					array[j] = HSVUtil.ConvertHsvToRgb(h, (double)((float)i / 100f), (double)((float)j / 100f), 1f);
				}
				texture2D.SetPixels32(i, 0, 1, 100, array);
			}
			texture2D.Apply();
			this.image.texture = texture2D;
		}

		// Token: 0x0400109A RID: 4250
		public ColorPickerControl picker;

		// Token: 0x0400109B RID: 4251
		private BoxSlider slider;

		// Token: 0x0400109C RID: 4252
		private RawImage image;

		// Token: 0x0400109D RID: 4253
		private float lastH = -1f;

		// Token: 0x0400109E RID: 4254
		private bool listen = true;
	}
}
