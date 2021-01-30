using System;
using System.Text;

namespace BestHTTP.Forms
{
	// Token: 0x020005EC RID: 1516
	public sealed class HTTPUrlEncodedForm : HTTPFormBase
	{
		// Token: 0x060037AE RID: 14254 RVA: 0x001120D8 File Offset: 0x001102D8
		public override void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Content-Type", "application/x-www-form-urlencoded");
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x001120EC File Offset: 0x001102EC
		public override byte[] GetData()
		{
			if (this.CachedData != null && !base.IsChanged)
			{
				return this.CachedData;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < base.Fields.Count; i++)
			{
				HTTPFieldData httpfieldData = base.Fields[i];
				if (i > 0)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(HTTPUrlEncodedForm.EscapeString(httpfieldData.Name));
				stringBuilder.Append("=");
				if (!string.IsNullOrEmpty(httpfieldData.Text) || httpfieldData.Binary == null)
				{
					stringBuilder.Append(HTTPUrlEncodedForm.EscapeString(httpfieldData.Text));
				}
				else
				{
					stringBuilder.Append(HTTPUrlEncodedForm.EscapeString(Encoding.UTF8.GetString(httpfieldData.Binary, 0, httpfieldData.Binary.Length)));
				}
			}
			base.IsChanged = false;
			return this.CachedData = Encoding.UTF8.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001121DC File Offset: 0x001103DC
		public static string EscapeString(string originalString)
		{
			if (originalString.Length < 256)
			{
				return Uri.EscapeDataString(originalString);
			}
			int num = originalString.Length / 256;
			StringBuilder stringBuilder = new StringBuilder(num);
			for (int i = 0; i <= num; i++)
			{
				stringBuilder.Append((i < num) ? Uri.EscapeDataString(originalString.Substring(256 * i, 256)) : Uri.EscapeDataString(originalString.Substring(256 * i)));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040023BD RID: 9149
		private const int EscapeTreshold = 256;

		// Token: 0x040023BE RID: 9150
		private byte[] CachedData;
	}
}
