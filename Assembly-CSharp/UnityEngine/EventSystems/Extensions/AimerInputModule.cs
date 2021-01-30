using System;

namespace UnityEngine.EventSystems.Extensions
{
	// Token: 0x02000158 RID: 344
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Aimer Input Module")]
	public class AimerInputModule : PointerInputModule
	{
		// Token: 0x06000D7E RID: 3454 RVA: 0x00057602 File Offset: 0x00055802
		protected AimerInputModule()
		{
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0005762C File Offset: 0x0005582C
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component != null && component.enabled)
			{
				Debug.LogError("Aimer Input Module is incompatible with the StandAloneInputSystem, please remove it from the Event System in this scene or disable it when this module is in use");
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0005765C File Offset: 0x0005585C
		public override void Process()
		{
			bool buttonDown = Input.GetButtonDown(this.activateAxis);
			bool buttonUp = Input.GetButtonUp(this.activateAxis);
			PointerEventData aimerPointerEventData = this.GetAimerPointerEventData();
			this.ProcessInteraction(aimerPointerEventData, buttonDown, buttonUp);
			if (!buttonUp)
			{
				this.ProcessMove(aimerPointerEventData);
				return;
			}
			base.RemovePointerData(aimerPointerEventData);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000576A4 File Offset: 0x000558A4
		protected virtual PointerEventData GetAimerPointerEventData()
		{
			PointerEventData pointerEventData;
			base.GetPointerData(-2, out pointerEventData, true);
			pointerEventData.Reset();
			pointerEventData.position = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f) + this.aimerOffset;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return pointerEventData;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00057724 File Offset: 0x00055924
		private void ProcessInteraction(PointerEventData pointer, bool pressed, bool released)
		{
			GameObject gameObject = pointer.pointerCurrentRaycast.gameObject;
			AimerInputModule.objectUnderAimer = ExecuteEvents.GetEventHandler<ISubmitHandler>(gameObject);
			if (pressed)
			{
				pointer.eligibleForClick = true;
				pointer.delta = Vector2.zero;
				pointer.pressPosition = pointer.position;
				pointer.pointerPressRaycast = pointer.pointerCurrentRaycast;
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(gameObject, pointer, ExecuteEvents.submitHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointer, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
				}
				else
				{
					pointer.eligibleForClick = false;
				}
				if (gameObject2 != pointer.pointerPress)
				{
					pointer.pointerPress = gameObject2;
					pointer.rawPointerPress = gameObject;
					pointer.clickCount = 0;
				}
				pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IBeginDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.beginDragHandler);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointer.pointerPress == eventHandler && pointer.eligibleForClick)
				{
					float unscaledTime = Time.unscaledTime;
					if (unscaledTime - pointer.clickTime < 0.3f)
					{
						int clickCount = pointer.clickCount + 1;
						pointer.clickCount = clickCount;
					}
					else
					{
						pointer.clickCount = 1;
					}
					pointer.clickTime = unscaledTime;
					ExecuteEvents.Execute<IPointerClickHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerClickHandler);
				}
				else if (pointer.pointerDrag != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointer, ExecuteEvents.dropHandler);
				}
				pointer.eligibleForClick = false;
				pointer.pointerPress = null;
				pointer.rawPointerPress = null;
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.endDragHandler);
				}
				pointer.pointerDrag = null;
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000578E0 File Offset: 0x00055AE0
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x04000D5D RID: 3421
		public string activateAxis = "Submit";

		// Token: 0x04000D5E RID: 3422
		public Vector2 aimerOffset = new Vector2(0f, 0f);

		// Token: 0x04000D5F RID: 3423
		public static GameObject objectUnderAimer;
	}
}
