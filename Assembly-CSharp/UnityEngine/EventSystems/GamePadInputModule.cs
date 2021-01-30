using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000157 RID: 343
	[AddComponentMenu("Event/Extensions/GamePad Input Module")]
	public class GamePadInputModule : BaseInputModule
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x000571B0 File Offset: 0x000553B0
		protected GamePadInputModule()
		{
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00057205 File Offset: 0x00055405
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0005720D File Offset: 0x0005540D
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00057216 File Offset: 0x00055416
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x0005721E File Offset: 0x0005541E
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00057227 File Offset: 0x00055427
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0005722F File Offset: 0x0005542F
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00057238 File Offset: 0x00055438
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x00057240 File Offset: 0x00055440
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00057249 File Offset: 0x00055449
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00057251 File Offset: 0x00055451
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0005725A File Offset: 0x0005545A
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x00057262 File Offset: 0x00055462
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0005726C File Offset: 0x0005546C
		public override bool ShouldActivateModule()
		{
			return base.ShouldActivateModule() && (true | Input.GetButtonDown(this.m_SubmitButton) | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f));
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000572D0 File Offset: 0x000554D0
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component && component.enabled)
			{
				Debug.LogError("StandAloneInputSystem should not be used with the GamePadInputModule, please remove it from the Event System in this scene or disable it when this module is in use");
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00057337 File Offset: 0x00055537
		public override void DeactivateModule()
		{
			base.DeactivateModule();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00057340 File Offset: 0x00055540
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00057378 File Offset: 0x00055578
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000573F0 File Offset: 0x000555F0
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000574A8 File Offset: 0x000556A8
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x000575BC File Offset: 0x000557BC
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x04000D54 RID: 3412
		private float m_PrevActionTime;

		// Token: 0x04000D55 RID: 3413
		private Vector2 m_LastMoveVector;

		// Token: 0x04000D56 RID: 3414
		private int m_ConsecutiveMoveCount;

		// Token: 0x04000D57 RID: 3415
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04000D58 RID: 3416
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04000D5A RID: 3418
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04000D5B RID: 3419
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04000D5C RID: 3420
		[SerializeField]
		private float m_RepeatDelay = 0.1f;
	}
}
