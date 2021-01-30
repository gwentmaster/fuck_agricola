using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001E8 RID: 488
	public class ComboBoxChanged : MonoBehaviour
	{
		// Token: 0x0600125F RID: 4703 RVA: 0x00070849 File Offset: 0x0006EA49
		public void ComboBoxChangedEvent(string text)
		{
			Debug.Log("ComboBox changed [" + text + "]");
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00070860 File Offset: 0x0006EA60
		public void AutoCompleteComboBoxChangedEvent(string text)
		{
			Debug.Log("AutoCompleteComboBox changed [" + text + "]");
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00070877 File Offset: 0x0006EA77
		public void AutoCompleteComboBoxSelectionChangedEvent(string text, bool valid)
		{
			Debug.Log(string.Concat(new string[]
			{
				"AutoCompleteComboBox selection changed [",
				text,
				"] and its validity was [",
				valid.ToString(),
				"]"
			}));
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000708AF File Offset: 0x0006EAAF
		public void DropDownChangedEvent(int newValue)
		{
			Debug.Log("DropDown changed [" + newValue + "]");
		}
	}
}
