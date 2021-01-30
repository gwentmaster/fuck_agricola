using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class InterfaceSelectionHints : MonoBehaviour
{
	// Token: 0x06000530 RID: 1328 RVA: 0x00028360 File Offset: 0x00026560
	private static void BuildDictionary()
	{
		InterfaceSelectionHints.m_DragSelectionHintDictionary = new Dictionary<int, DragSelectionHintDefinition>();
		if (InterfaceSelectionHints.m_DragSelectionHintDictionary != null)
		{
			for (int i = 0; i < InterfaceSelectionHints.m_DragSelectionHintMapping.Length; i++)
			{
				DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.m_DragSelectionHintMapping[i];
				InterfaceSelectionHints.m_DragSelectionHintDictionary.Add(dragSelectionHintDefinition.m_SelectionHint, dragSelectionHintDefinition);
			}
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x000283AC File Offset: 0x000265AC
	public static DragSelectionHintDefinition FindSelectionHintDefinition(int selectionHint)
	{
		if (InterfaceSelectionHints.m_DragSelectionHintDictionary == null)
		{
			InterfaceSelectionHints.BuildDictionary();
		}
		DragSelectionHintDefinition result;
		if (InterfaceSelectionHints.m_DragSelectionHintDictionary.TryGetValue(selectionHint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x040004E5 RID: 1253
	private static DragSelectionHintDefinition[] m_DragSelectionHintMapping = new DragSelectionHintDefinition[]
	{
		new DragSelectionHintDefinition(40962, 2, 0, true, true, false, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), "${Key_MagnifyAssign}"),
		new DragSelectionHintDefinition(40982, 3, 0, true, true, true, new Color32(0, byte.MaxValue, 0, byte.MaxValue), "${Key_MagnifyBuild}"),
		new DragSelectionHintDefinition(40983, 3, 0, true, true, true, new Color32(0, byte.MaxValue, 0, byte.MaxValue), "${Key_MagnifyPlay}"),
		new DragSelectionHintDefinition(40998, 0, 0, false, false, false, new Color32(0, byte.MaxValue, 0, byte.MaxValue), "${Key_MagnifyUse}"),
		new DragSelectionHintDefinition(41056, 4, 0, false, false, false, new Color32(0, byte.MaxValue, 0, byte.MaxValue), "${Key_MagnifyDraft}"),
		new DragSelectionHintDefinition(41057, 5, 0, false, false, false, new Color32(byte.MaxValue, 0, 0, byte.MaxValue), "${Key_MagnifyDiscard}")
	};

	// Token: 0x040004E6 RID: 1254
	private static Dictionary<int, DragSelectionHintDefinition> m_DragSelectionHintDictionary = null;

	// Token: 0x0200077B RID: 1915
	public enum EDragTargetType
	{
		// Token: 0x04002BE1 RID: 11233
		E_DRAGTARGET_GENERIC = 1,
		// Token: 0x04002BE2 RID: 11234
		E_DRAGTARGET_ASSIGNWORKER,
		// Token: 0x04002BE3 RID: 11235
		E_DRAGTARGET_BUILDCARD,
		// Token: 0x04002BE4 RID: 11236
		E_DRAGTARGET_DRAFTCARD,
		// Token: 0x04002BE5 RID: 11237
		E_DRAGTARGET_DISCARD_DRAFT
	}
}
