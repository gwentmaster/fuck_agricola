using System;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x0200055D RID: 1373
	public interface INotifyCollectionChanged
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060031AC RID: 12716
		// (remove) Token: 0x060031AD RID: 12717
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
