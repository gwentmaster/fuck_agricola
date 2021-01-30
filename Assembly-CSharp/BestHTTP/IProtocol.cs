using System;

namespace BestHTTP
{
	// Token: 0x02000578 RID: 1400
	public interface IProtocol
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600333D RID: 13117
		bool IsClosed { get; }

		// Token: 0x0600333E RID: 13118
		void HandleEvents();
	}
}
