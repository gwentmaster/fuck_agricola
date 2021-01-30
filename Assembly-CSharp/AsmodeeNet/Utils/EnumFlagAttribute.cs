using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000666 RID: 1638
	public class EnumFlagAttribute : PropertyAttribute
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x0006CDEA File Offset: 0x0006AFEA
		public EnumFlagAttribute()
		{
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0012AFA9 File Offset: 0x001291A9
		public EnumFlagAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040026EE RID: 9966
		public string name;
	}
}
