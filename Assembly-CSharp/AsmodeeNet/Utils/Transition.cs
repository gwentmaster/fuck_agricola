using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000671 RID: 1649
	[Serializable]
	public class Transition
	{
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x0012BB95 File Offset: 0x00129D95
		// (set) Token: 0x06003CB3 RID: 15539 RVA: 0x0012BB9D File Offset: 0x00129D9D
		public ActionState ActionStateStart { get; set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x0012BBA6 File Offset: 0x00129DA6
		// (set) Token: 0x06003CB5 RID: 15541 RVA: 0x0012BBAE File Offset: 0x00129DAE
		public ActionState ActionStateEnd { get; set; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x0012BBB7 File Offset: 0x00129DB7
		// (set) Token: 0x06003CB7 RID: 15543 RVA: 0x0012BBBF File Offset: 0x00129DBF
		public Func<bool> Condition { get; set; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x0012BBC8 File Offset: 0x00129DC8
		// (set) Token: 0x06003CB9 RID: 15545 RVA: 0x0012BBD0 File Offset: 0x00129DD0
		public TransitionType TransitionType { get; set; }

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x0012BBD9 File Offset: 0x00129DD9
		// (set) Token: 0x06003CBB RID: 15547 RVA: 0x0012BBE1 File Offset: 0x00129DE1
		public float TransitionDuration { get; set; }

		// Token: 0x06003CBC RID: 15548 RVA: 0x0012BBEA File Offset: 0x00129DEA
		public Transition(ActionState actionStateStart, ActionState actionStateEnd, Func<bool> condition, TransitionType transitionType)
		{
			this.ActionStateStart = actionStateStart;
			this.ActionStateEnd = actionStateEnd;
			this.Condition = condition;
			this.TransitionType = transitionType;
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0012BC0F File Offset: 0x00129E0F
		public Transition(ActionState actionStateStart, ActionState actionStateEnd, float transitionDuration, TransitionType transitionType)
		{
			this.ActionStateStart = actionStateStart;
			this.ActionStateEnd = actionStateEnd;
			this.TransitionDuration = transitionDuration;
			this.TransitionType = transitionType;
		}
	}
}
