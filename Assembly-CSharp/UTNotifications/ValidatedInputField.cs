using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x0200013F RID: 319
	[RequireComponent(typeof(InputField))]
	public class ValidatedInputField : MonoBehaviour
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x00054072 File Offset: 0x00052272
		public bool IsValid()
		{
			return this.regex.IsMatch(this.inputField.text);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0005408A File Offset: 0x0005228A
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00054097 File Offset: 0x00052297
		public string text
		{
			get
			{
				return this.inputField.text;
			}
			set
			{
				this.inputField.text = value;
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000540A8 File Offset: 0x000522A8
		private void Awake()
		{
			this.regex = new Regex(this.RequiredPattern);
			this.inputField = base.GetComponent<InputField>();
			this.incorrect = base.transform.Find("Incorrect").gameObject;
			this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00054109 File Offset: 0x00052309
		private void Start()
		{
			this.OnValueChanged(this.inputField.text);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0005411C File Offset: 0x0005231C
		private void OnValueChanged(string value)
		{
			this.incorrect.SetActive(!this.IsValid());
		}

		// Token: 0x04000CF7 RID: 3319
		public string RequiredPattern;

		// Token: 0x04000CF8 RID: 3320
		private Regex regex;

		// Token: 0x04000CF9 RID: 3321
		private InputField inputField;

		// Token: 0x04000CFA RID: 3322
		private GameObject incorrect;
	}
}
