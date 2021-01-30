using System;
using Fabric.Internal.Runtime;

namespace Fabric.Runtime.Internal
{
	// Token: 0x02000257 RID: 599
	internal class Impl
	{
		// Token: 0x06001300 RID: 4864 RVA: 0x00071FCA File Offset: 0x000701CA
		public static Impl Make()
		{
			return new Impl();
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00071FD1 File Offset: 0x000701D1
		public virtual string Initialize()
		{
			Utils.Log("Fabric", "Method Initialize () is unimplemented on this platform");
			return "";
		}

		// Token: 0x040012E9 RID: 4841
		protected const string Name = "Fabric";
	}
}
