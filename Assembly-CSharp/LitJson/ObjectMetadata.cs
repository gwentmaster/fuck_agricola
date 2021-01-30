using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000269 RID: 617
	internal struct ObjectMetadata
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00073678 File Offset: 0x00071878
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x00073699 File Offset: 0x00071899
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x000736A2 File Offset: 0x000718A2
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x000736AA File Offset: 0x000718AA
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x000736B3 File Offset: 0x000718B3
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x000736BB File Offset: 0x000718BB
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x0400130E RID: 4878
		private Type element_type;

		// Token: 0x0400130F RID: 4879
		private bool is_dictionary;

		// Token: 0x04001310 RID: 4880
		private IDictionary<string, PropertyMetadata> properties;
	}
}
