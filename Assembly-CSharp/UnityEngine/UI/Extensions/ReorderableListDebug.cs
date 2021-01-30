using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017C RID: 380
	public class ReorderableListDebug : MonoBehaviour
	{
		// Token: 0x06000EAF RID: 3759 RVA: 0x0005CF6C File Offset: 0x0005B16C
		private void Awake()
		{
			ReorderableList[] array = Object.FindObjectsOfType<ReorderableList>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnElementDropped.AddListener(new UnityAction<ReorderableList.ReorderableListEventStruct>(this.ElementDropped));
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0005CFA8 File Offset: 0x0005B1A8
		private void ElementDropped(ReorderableList.ReorderableListEventStruct droppedStruct)
		{
			this.DebugLabel.text = "";
			Text debugLabel = this.DebugLabel;
			debugLabel.text = debugLabel.text + "Dropped Object: " + droppedStruct.DroppedObject.name + "\n";
			Text debugLabel2 = this.DebugLabel;
			debugLabel2.text = debugLabel2.text + "Is Clone ?: " + droppedStruct.IsAClone.ToString() + "\n";
			if (droppedStruct.IsAClone)
			{
				Text debugLabel3 = this.DebugLabel;
				debugLabel3.text = debugLabel3.text + "Source Object: " + droppedStruct.SourceObject.name + "\n";
			}
			Text debugLabel4 = this.DebugLabel;
			debugLabel4.text += string.Format("From {0} at Index {1} \n", droppedStruct.FromList.name, droppedStruct.FromIndex);
			Text debugLabel5 = this.DebugLabel;
			debugLabel5.text += string.Format("To {0} at Index {1} \n", droppedStruct.ToList.name, droppedStruct.ToIndex);
		}

		// Token: 0x04000E4F RID: 3663
		public Text DebugLabel;
	}
}
