using System;

namespace LitJson
{
	// Token: 0x02000268 RID: 616
	internal struct ArrayMetadata
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x0007362C File Offset: 0x0007182C
		// (set) Token: 0x060013DC RID: 5084 RVA: 0x0007364D File Offset: 0x0007184D
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x00073656 File Offset: 0x00071856
		// (set) Token: 0x060013DE RID: 5086 RVA: 0x0007365E File Offset: 0x0007185E
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x00073667 File Offset: 0x00071867
		// (set) Token: 0x060013E0 RID: 5088 RVA: 0x0007366F File Offset: 0x0007186F
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x0400130B RID: 4875
		private Type element_type;

		// Token: 0x0400130C RID: 4876
		private bool is_array;

		// Token: 0x0400130D RID: 4877
		private bool is_list;
	}
}
