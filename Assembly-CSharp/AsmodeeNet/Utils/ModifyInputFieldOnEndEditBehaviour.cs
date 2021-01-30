using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066C RID: 1644
	public static class ModifyInputFieldOnEndEditBehaviour
	{
		// Token: 0x06003C8A RID: 15498 RVA: 0x0012B3C8 File Offset: 0x001295C8
		public static void ModifyBehaviour(TMP_InputField inputField)
		{
			for (int i = 0; i < inputField.onEndEdit.GetPersistentEventCount(); i++)
			{
				int index = i;
				inputField.onEndEdit.SetPersistentListenerState(i, UnityEventCallState.Off);
				inputField.onEndEdit.AddListener(delegate(string value)
				{
					if (Input.GetButtonDown(EventSystem.current.GetComponent<StandaloneInputModule>().submitButton))
					{
						((Component)inputField.onEndEdit.GetPersistentTarget(index)).SendMessage(inputField.onEndEdit.GetPersistentMethodName(index), value);
					}
				});
			}
		}
	}
}
