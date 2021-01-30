using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001E3 RID: 483
	[RequireComponent(typeof(InputField))]
	public class HexColorField : MonoBehaviour
	{
		// Token: 0x06001237 RID: 4663 RVA: 0x0007007C File Offset: 0x0006E27C
		private void Awake()
		{
			this.hexInputField = base.GetComponent<InputField>();
			this.hexInputField.onEndEdit.AddListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000700CD File Offset: 0x0006E2CD
		private void OnDestroy()
		{
			this.hexInputField.onValueChanged.RemoveListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00070107 File Offset: 0x0006E307
		private void UpdateHex(Color newColor)
		{
			this.hexInputField.text = this.ColorToHex(newColor);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00070120 File Offset: 0x0006E320
		private void UpdateColor(string newHex)
		{
			Color32 c;
			if (HexColorField.HexToColor(newHex, out c))
			{
				this.ColorPicker.CurrentColor = c;
				return;
			}
			Debug.Log("hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional)");
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00070154 File Offset: 0x0006E354
		private string ColorToHex(Color32 color)
		{
			if (this.displayAlpha)
			{
				return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
				{
					color.r,
					color.g,
					color.b,
					color.a
				});
			}
			return string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000701E0 File Offset: 0x0006E3E0
		public static bool HexToColor(string hex, out Color32 color)
		{
			if (Regex.IsMatch(hex, "^#?(?:[0-9a-fA-F]{3,4}){1,2}$"))
			{
				int num = hex.StartsWith("#") ? 1 : 0;
				if (hex.Length == num + 8)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier));
				}
				else if (hex.Length == num + 6)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				else if (hex.Length == num + 4)
				{
					color = new Color32(byte.Parse(hex[num].ToString() + hex[num].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 3].ToString() + hex[num + 3].ToString(), NumberStyles.AllowHexSpecifier));
				}
				else
				{
					color = new Color32(byte.Parse(hex[num].ToString() + hex[num].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2].ToString(), NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				return true;
			}
			color = default(Color32);
			return false;
		}

		// Token: 0x04001096 RID: 4246
		public ColorPickerControl ColorPicker;

		// Token: 0x04001097 RID: 4247
		public bool displayAlpha;

		// Token: 0x04001098 RID: 4248
		private InputField hexInputField;

		// Token: 0x04001099 RID: 4249
		private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";
	}
}
