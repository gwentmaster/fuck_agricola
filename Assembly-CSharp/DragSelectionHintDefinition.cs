using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class DragSelectionHintDefinition
{
	// Token: 0x0600052F RID: 1327 RVA: 0x00028310 File Offset: 0x00026510
	public DragSelectionHintDefinition(int selectionHint, int dragTargetZoneIndex, int dragTargetZoneShortcut, bool bUseTargetZoneInstanceID, bool bUseWorkerSpaceGlow, bool bDragReturnOnSelection, Color highlightColor, string optionTextDisplay)
	{
		this.m_SelectionHint = selectionHint;
		this.m_DragTargetZoneIndex = dragTargetZoneIndex;
		this.m_DragTargetZoneShortcut = dragTargetZoneShortcut;
		this.m_bUseTargetZoneInstanceID = bUseTargetZoneInstanceID;
		this.m_bUseWorkerSpaceGlow = bUseWorkerSpaceGlow;
		this.m_bDragReturnOnSelection = bDragReturnOnSelection;
		this.m_HighlightColor = highlightColor;
		this.m_OptionTextDisplay = optionTextDisplay;
	}

	// Token: 0x040004DD RID: 1245
	public int m_SelectionHint;

	// Token: 0x040004DE RID: 1246
	public int m_DragTargetZoneIndex;

	// Token: 0x040004DF RID: 1247
	public int m_DragTargetZoneShortcut;

	// Token: 0x040004E0 RID: 1248
	public bool m_bUseTargetZoneInstanceID;

	// Token: 0x040004E1 RID: 1249
	public bool m_bUseWorkerSpaceGlow;

	// Token: 0x040004E2 RID: 1250
	public bool m_bDragReturnOnSelection;

	// Token: 0x040004E3 RID: 1251
	public Color m_HighlightColor;

	// Token: 0x040004E4 RID: 1252
	public string m_OptionTextDisplay;
}
