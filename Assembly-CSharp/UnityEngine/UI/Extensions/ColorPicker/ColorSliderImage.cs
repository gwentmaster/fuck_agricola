using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DF RID: 479
	[RequireComponent(typeof(RawImage))]
	[ExecuteInEditMode]
	public class ColorSliderImage : MonoBehaviour
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0006F795 File Offset: 0x0006D995
		private RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0006F7A2 File Offset: 0x0006D9A2
		private void Awake()
		{
			this.image = base.GetComponent<RawImage>();
			if (this.image)
			{
				this.RegenerateTexture();
				return;
			}
			Debug.LogWarning("Missing RawImage on object [" + base.name + "]");
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0006F7E0 File Offset: 0x0006D9E0
		private void OnEnable()
		{
			if (this.picker != null && Application.isPlaying)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0006F83C File Offset: 0x0006DA3C
		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0006F88F File Offset: 0x0006DA8F
		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0006F8B4 File Offset: 0x0006DAB4
		private void ColorChanged(Color newColor)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			case ColorValues.A:
			case ColorValues.Hue:
				break;
			default:
				return;
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0006F8F4 File Offset: 0x0006DAF4
		private void ColorChanged(float hue, float saturation, float value)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			case ColorValues.A:
			case ColorValues.Hue:
				break;
			default:
				return;
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0006F934 File Offset: 0x0006DB34
		private void RegenerateTexture()
		{
			if (!this.picker)
			{
				Debug.LogWarning("Missing Picker on object [" + base.name + "]");
			}
			Color32 color = (this.picker != null) ? this.picker.CurrentColor : Color.black;
			float num = (this.picker != null) ? this.picker.H : 0f;
			float num2 = (this.picker != null) ? this.picker.S : 0f;
			float num3 = (this.picker != null) ? this.picker.V : 0f;
			bool flag = this.direction == Slider.Direction.BottomToTop || this.direction == Slider.Direction.TopToBottom;
			bool flag2 = this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.RightToLeft;
			int num4;
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.A:
				num4 = 255;
				break;
			case ColorValues.Hue:
				num4 = 360;
				break;
			case ColorValues.Saturation:
			case ColorValues.Value:
				num4 = 100;
				break;
			default:
				throw new NotImplementedException("");
			}
			Texture2D texture2D;
			if (flag)
			{
				texture2D = new Texture2D(1, num4);
			}
			else
			{
				texture2D = new Texture2D(num4, 1);
			}
			texture2D.hideFlags = HideFlags.DontSave;
			Color32[] array = new Color32[num4];
			switch (this.type)
			{
			case ColorValues.R:
			{
				byte b = 0;
				while ((int)b < num4)
				{
					array[flag2 ? (num4 - 1 - (int)b) : ((int)b)] = new Color32(b, color.g, color.b, byte.MaxValue);
					b += 1;
				}
				break;
			}
			case ColorValues.G:
			{
				byte b2 = 0;
				while ((int)b2 < num4)
				{
					array[flag2 ? (num4 - 1 - (int)b2) : ((int)b2)] = new Color32(color.r, b2, color.b, byte.MaxValue);
					b2 += 1;
				}
				break;
			}
			case ColorValues.B:
			{
				byte b3 = 0;
				while ((int)b3 < num4)
				{
					array[flag2 ? (num4 - 1 - (int)b3) : ((int)b3)] = new Color32(color.r, color.g, b3, byte.MaxValue);
					b3 += 1;
				}
				break;
			}
			case ColorValues.A:
			{
				byte b4 = 0;
				while ((int)b4 < num4)
				{
					array[flag2 ? (num4 - 1 - (int)b4) : ((int)b4)] = new Color32(b4, b4, b4, byte.MaxValue);
					b4 += 1;
				}
				break;
			}
			case ColorValues.Hue:
				for (int i = 0; i < num4; i++)
				{
					array[flag2 ? (num4 - 1 - i) : i] = HSVUtil.ConvertHsvToRgb((double)i, 1.0, 1.0, 1f);
				}
				break;
			case ColorValues.Saturation:
				for (int j = 0; j < num4; j++)
				{
					array[flag2 ? (num4 - 1 - j) : j] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)((float)j / (float)num4), (double)num3, 1f);
				}
				break;
			case ColorValues.Value:
				for (int k = 0; k < num4; k++)
				{
					array[flag2 ? (num4 - 1 - k) : k] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)num2, (double)((float)k / (float)num4), 1f);
				}
				break;
			default:
				throw new NotImplementedException("");
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
			this.image.texture = texture2D;
			Slider.Direction direction = this.direction;
			if (direction > Slider.Direction.RightToLeft)
			{
				if (direction - Slider.Direction.BottomToTop <= 1)
				{
					this.image.uvRect = new Rect(0f, 0f, 2f, 1f);
					return;
				}
			}
			else
			{
				this.image.uvRect = new Rect(0f, 0f, 1f, 2f);
			}
		}

		// Token: 0x04001087 RID: 4231
		public ColorPickerControl picker;

		// Token: 0x04001088 RID: 4232
		public ColorValues type;

		// Token: 0x04001089 RID: 4233
		public Slider.Direction direction;

		// Token: 0x0400108A RID: 4234
		private RawImage image;
	}
}
