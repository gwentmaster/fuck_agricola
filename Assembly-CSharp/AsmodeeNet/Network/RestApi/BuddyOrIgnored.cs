using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AE RID: 1710
	[Serializable]
	public class BuddyOrIgnored
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0012E644 File Offset: 0x0012C844
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0012E64C File Offset: 0x0012C84C
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x0012E654 File Offset: 0x0012C854
		public BuddyOrIgnored()
		{
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x0012E663 File Offset: 0x0012C863
		public BuddyOrIgnored(int id, string name)
		{
			this._id = id;
			this._name = name;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0012E680 File Offset: 0x0012C880
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			BuddyOrIgnored buddyOrIgnored = o as BuddyOrIgnored;
			return buddyOrIgnored != null && this.Id == buddyOrIgnored.Id && this.Name == buddyOrIgnored.Name;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0012E6BF File Offset: 0x0012C8BF
		public override int GetHashCode()
		{
			return this.Id;
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0012E6C7 File Offset: 0x0012C8C7
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"this : Id : \"",
				this.Id,
				"\",  Name : \"",
				this.Name,
				"\""
			});
		}

		// Token: 0x040027A5 RID: 10149
		[SerializeField]
		private int _id = -1;

		// Token: 0x040027A6 RID: 10150
		[SerializeField]
		private string _name;
	}
}
