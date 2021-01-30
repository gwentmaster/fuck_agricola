using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000177 RID: 375
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/InputFocus")]
	public class InputFocus : MonoBehaviour
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x0005C7F1 File Offset: 0x0005A9F1
		private void Start()
		{
			this._inputField = base.GetComponent<InputField>();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005C7FF File Offset: 0x0005A9FF
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Return) && !this._inputField.isFocused)
			{
				if (this._ignoreNextActivation)
				{
					this._ignoreNextActivation = false;
					return;
				}
				this._inputField.Select();
				this._inputField.ActivateInputField();
			}
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005C83D File Offset: 0x0005AA3D
		public void buttonPressed()
		{
			bool flag = this._inputField.text == "";
			this._inputField.text = "";
			if (!flag)
			{
				this._inputField.Select();
				this._inputField.ActivateInputField();
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005C87C File Offset: 0x0005AA7C
		public void OnEndEdit(string textString)
		{
			if (!Input.GetKeyDown(KeyCode.Return))
			{
				return;
			}
			bool flag = this._inputField.text == "";
			this._inputField.text = "";
			if (flag)
			{
				this._ignoreNextActivation = true;
			}
		}

		// Token: 0x04000E2B RID: 3627
		protected InputField _inputField;

		// Token: 0x04000E2C RID: 3628
		public bool _ignoreNextActivation;
	}
}
