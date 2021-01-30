using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200065D RID: 1629
	public class ActivityIndicatorButton : MonoBehaviour
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x0012A049 File Offset: 0x00128249
		// (set) Token: 0x06003C1A RID: 15386 RVA: 0x0012A051 File Offset: 0x00128251
		public bool Waiting
		{
			get
			{
				return this._waiting;
			}
			set
			{
				this._waiting = value;
				this._text.SetActive(!this._waiting);
				this._spinner.SetActive(this._waiting);
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x0012A07F File Offset: 0x0012827F
		// (set) Token: 0x06003C1C RID: 15388 RVA: 0x0012A08C File Offset: 0x0012828C
		public bool Interactable
		{
			get
			{
				return this._button.interactable;
			}
			set
			{
				this._button.interactable = value;
			}
		}

		// Token: 0x040026DC RID: 9948
		[SerializeField]
		private GameObject _text;

		// Token: 0x040026DD RID: 9949
		[SerializeField]
		private GameObject _spinner;

		// Token: 0x040026DE RID: 9950
		[SerializeField]
		private Button _button;

		// Token: 0x040026DF RID: 9951
		private bool _waiting;
	}
}
