using System;
using System.Linq;
using System.Text;
using BestHTTP.JSON;

namespace BestHTTP.Forms
{
	// Token: 0x020005ED RID: 1517
	public sealed class RawJsonForm : HTTPFormBase
	{
		// Token: 0x060037B2 RID: 14258 RVA: 0x00112261 File Offset: 0x00110461
		public override void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Content-Type", "application/json");
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x00112274 File Offset: 0x00110474
		public override byte[] GetData()
		{
			if (this.CachedData != null && !base.IsChanged)
			{
				return this.CachedData;
			}
			string s = Json.Encode(base.Fields.ToDictionary((HTTPFieldData x) => x.Name, (HTTPFieldData x) => x.Text));
			base.IsChanged = false;
			return this.CachedData = Encoding.UTF8.GetBytes(s);
		}

		// Token: 0x040023BF RID: 9151
		private byte[] CachedData;
	}
}
