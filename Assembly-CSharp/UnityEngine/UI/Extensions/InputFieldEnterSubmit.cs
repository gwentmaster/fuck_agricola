using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C1 RID: 449
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Input Field Submit")]
	public class InputFieldEnterSubmit : MonoBehaviour
	{
		// Token: 0x0600115F RID: 4447 RVA: 0x0006CB5C File Offset: 0x0006AD5C
		private void Awake()
		{
			this._input = base.GetComponent<InputField>();
			this._input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0006CB86 File Offset: 0x0006AD86
		public void OnEndEdit(string txt)
		{
			if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				return;
			}
			this.EnterSubmit.Invoke(txt);
		}

		// Token: 0x04001000 RID: 4096
		public InputFieldEnterSubmit.EnterSubmitEvent EnterSubmit;

		// Token: 0x04001001 RID: 4097
		private InputField _input;

		// Token: 0x02000861 RID: 2145
		[Serializable]
		public class EnterSubmitEvent : UnityEvent<string>
		{
		}
	}
}
