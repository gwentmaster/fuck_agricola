using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016C RID: 364
	[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(ToggleGroup))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Group")]
	public class Accordion : MonoBehaviour
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0005A01C File Offset: 0x0005821C
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0005A024 File Offset: 0x00058224
		public Accordion.Transition transition
		{
			get
			{
				return this.m_Transition;
			}
			set
			{
				this.m_Transition = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0005A02D File Offset: 0x0005822D
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0005A035 File Offset: 0x00058235
		public float transitionDuration
		{
			get
			{
				return this.m_TransitionDuration;
			}
			set
			{
				this.m_TransitionDuration = value;
			}
		}

		// Token: 0x04000DAB RID: 3499
		[SerializeField]
		private Accordion.Transition m_Transition;

		// Token: 0x04000DAC RID: 3500
		[SerializeField]
		private float m_TransitionDuration = 0.3f;

		// Token: 0x02000835 RID: 2101
		public enum Transition
		{
			// Token: 0x04002E91 RID: 11921
			Instant,
			// Token: 0x04002E92 RID: 11922
			Tween
		}
	}
}
