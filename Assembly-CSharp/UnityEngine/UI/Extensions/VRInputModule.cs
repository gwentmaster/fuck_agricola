using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D8 RID: 472
	[AddComponentMenu("Event/VR Input Module")]
	public class VRInputModule : BaseInputModule
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x0006EE47 File Offset: 0x0006D047
		protected override void Awake()
		{
			VRInputModule._singleton = this;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0006EE4F File Offset: 0x0006D04F
		public override void Process()
		{
			if (VRInputModule.targetObject == null)
			{
				VRInputModule.mouseClicked = false;
				return;
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0006EE68 File Offset: 0x0006D068
		public static void PointerSubmit(GameObject obj)
		{
			VRInputModule.targetObject = obj;
			VRInputModule.mouseClicked = true;
			if (VRInputModule.mouseClicked)
			{
				BaseEventData baseEventData = new BaseEventData(VRInputModule._singleton.eventSystem);
				baseEventData.selectedObject = VRInputModule.targetObject;
				ExecuteEvents.Execute<ISubmitHandler>(VRInputModule.targetObject, baseEventData, ExecuteEvents.submitHandler);
				MonoBehaviour.print("clicked " + VRInputModule.targetObject.name);
				VRInputModule.mouseClicked = false;
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0006EED4 File Offset: 0x0006D0D4
		public static void PointerExit(GameObject obj)
		{
			MonoBehaviour.print("PointerExit " + obj.name);
			PointerEventData eventData = new PointerEventData(VRInputModule._singleton.eventSystem);
			ExecuteEvents.Execute<IPointerExitHandler>(obj, eventData, ExecuteEvents.pointerExitHandler);
			ExecuteEvents.Execute<IDeselectHandler>(obj, eventData, ExecuteEvents.deselectHandler);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0006EF20 File Offset: 0x0006D120
		public static void PointerEnter(GameObject obj)
		{
			MonoBehaviour.print("PointerEnter " + obj.name);
			PointerEventData pointerEventData = new PointerEventData(VRInputModule._singleton.eventSystem);
			pointerEventData.pointerEnter = obj;
			RaycastResult pointerCurrentRaycast = new RaycastResult
			{
				worldPosition = VRInputModule.cursorPosition
			};
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			ExecuteEvents.Execute<IPointerEnterHandler>(obj, pointerEventData, ExecuteEvents.pointerEnterHandler);
		}

		// Token: 0x04001067 RID: 4199
		public static GameObject targetObject;

		// Token: 0x04001068 RID: 4200
		private static VRInputModule _singleton;

		// Token: 0x04001069 RID: 4201
		private int counter;

		// Token: 0x0400106A RID: 4202
		private static bool mouseClicked;

		// Token: 0x0400106B RID: 4203
		public static Vector3 cursorPosition;
	}
}
