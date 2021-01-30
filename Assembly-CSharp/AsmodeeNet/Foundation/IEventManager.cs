using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F2 RID: 1778
	public interface IEventManager
	{
		// Token: 0x06003ED4 RID: 16084
		void QueueEvent(Action action);

		// Token: 0x06003ED5 RID: 16085
		void QueueEvent<T>(Action<T> action, T parameter);

		// Token: 0x06003ED6 RID: 16086
		void QueueEvent<T1, T2>(Action<T1, T2> action, T1 parameter1, T2 parameter2);
	}
}
