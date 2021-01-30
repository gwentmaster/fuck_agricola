using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066F RID: 1647
	[Serializable]
	public class StateMachine
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003C9C RID: 15516 RVA: 0x0012B82C File Offset: 0x00129A2C
		// (set) Token: 0x06003C9D RID: 15517 RVA: 0x0012B834 File Offset: 0x00129A34
		public bool FirstUpdate { get; set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003C9E RID: 15518 RVA: 0x0012B83D File Offset: 0x00129A3D
		// (set) Token: 0x06003C9F RID: 15519 RVA: 0x0012B845 File Offset: 0x00129A45
		public string FSMName { get; set; }

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0012B84E File Offset: 0x00129A4E
		public StateMachine(string fsmName)
		{
			this.FSMName = fsmName;
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x0012B87C File Offset: 0x00129A7C
		public void Update()
		{
			if (!this.Enabled)
			{
				return;
			}
			if (this.CurrentState.ActionUpdate != null)
			{
				this.CurrentState.ActionUpdate();
			}
			this.FirstUpdate = false;
			foreach (Transition transition in this._listTransition)
			{
				if ((transition.ActionStateStart == this.CurrentState || (transition.ActionStateEnd != this.CurrentState && transition.ActionStateStart == null)) && transition.Condition())
				{
					if (this.IsDebug)
					{
						Debug.Log(string.Concat(new string[]
						{
							"FSM ",
							this.FSMName,
							" : ",
							this.CurrentState.Name,
							" -> ",
							transition.ActionStateEnd.Name
						}));
					}
					if (transition.TransitionType == TransitionType.WithDuration)
					{
						if (this.CurrentState.ActionExit != null)
						{
							this.CurrentState.ActionExit();
						}
						this.PreviousState = transition.ActionStateStart;
						this.CurrentState = transition.ActionStateEnd;
						this.FirstUpdate = true;
						if (this.OnStateChanged != null)
						{
							this.OnStateChanged();
						}
						if (this.CurrentState.ActionEnter != null)
						{
							this.CurrentState.ActionEnter();
							break;
						}
						break;
					}
					else
					{
						if (this.CurrentState.ActionExit != null)
						{
							this.CurrentState.ActionExit();
						}
						this.PreviousState = transition.ActionStateStart;
						this.CurrentState = transition.ActionStateEnd;
						this.FirstUpdate = true;
						if (this.OnStateChanged != null)
						{
							this.OnStateChanged();
						}
						if (this.CurrentState.ActionEnter != null)
						{
							this.CurrentState.ActionEnter();
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x0012BA84 File Offset: 0x00129C84
		public ActionState AddActionState(string name)
		{
			return this.AddActionState(name, null, null, null);
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0012BA90 File Offset: 0x00129C90
		public ActionState AddActionState(string name, Action actionEnter)
		{
			return this.AddActionState(name, actionEnter, null, null);
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x0012BA9C File Offset: 0x00129C9C
		public ActionState AddActionState(string name, Action actionEnter, Action actionUpdate, Action actionExit)
		{
			ActionState actionState = new ActionState(name, actionEnter, actionUpdate, actionExit);
			this._listState.Add(actionState);
			return actionState;
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x0012BAC1 File Offset: 0x00129CC1
		public Transition AddTransition(ActionState actionStateEnd, Func<bool> condition)
		{
			return this.AddTransition(null, actionStateEnd, condition);
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0012BACC File Offset: 0x00129CCC
		public Transition AddTransition(ActionState actionStateStart, ActionState actionStateEnd, Func<bool> condition)
		{
			Transition transition = new Transition(actionStateStart, actionStateEnd, condition, TransitionType.Normal);
			this._listTransition.Add(transition);
			return transition;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0012BAF0 File Offset: 0x00129CF0
		public Transition AddTransition(ActionState actionStateStart, ActionState actionStateEnd, float transitionDuration)
		{
			Transition transition = new Transition(actionStateStart, actionStateEnd, transitionDuration, TransitionType.WithDuration);
			this._listTransition.Add(transition);
			return transition;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x0012BB14 File Offset: 0x00129D14
		public void Reset()
		{
			this._listState = new List<ActionState>();
			this._listTransition = new List<Transition>();
		}

		// Token: 0x040026F0 RID: 9968
		private List<ActionState> _listState = new List<ActionState>();

		// Token: 0x040026F1 RID: 9969
		private List<Transition> _listTransition = new List<Transition>();

		// Token: 0x040026F2 RID: 9970
		public ActionState CurrentState;

		// Token: 0x040026F3 RID: 9971
		public ActionState PreviousState;

		// Token: 0x040026F4 RID: 9972
		public bool IsDebug;

		// Token: 0x040026F7 RID: 9975
		public Action OnStateChanged;

		// Token: 0x040026F8 RID: 9976
		public bool Enabled = true;
	}
}
