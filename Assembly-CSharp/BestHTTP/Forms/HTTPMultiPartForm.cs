using System;
using System.IO;
using BestHTTP.Extensions;

namespace BestHTTP.Forms
{
	// Token: 0x020005EB RID: 1515
	public sealed class HTTPMultiPartForm : HTTPFormBase
	{
		// Token: 0x060037AB RID: 14251 RVA: 0x00111F08 File Offset: 0x00110108
		public HTTPMultiPartForm()
		{
			this.Boundary = "BestHTTP_HTTPMultiPartForm_" + this.GetHashCode().ToString("X");
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x00111F3E File Offset: 0x0011013E
		public override void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Content-Type", "multipart/form-data; boundary=\"" + this.Boundary + "\"");
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x00111F60 File Offset: 0x00110160
		public override byte[] GetData()
		{
			if (this.CachedData != null)
			{
				return this.CachedData;
			}
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				for (int i = 0; i < base.Fields.Count; i++)
				{
					HTTPFieldData httpfieldData = base.Fields[i];
					memoryStream.WriteLine("--" + this.Boundary);
					memoryStream.WriteLine("Content-Disposition: form-data; name=\"" + httpfieldData.Name + "\"" + ((!string.IsNullOrEmpty(httpfieldData.FileName)) ? ("; filename=\"" + httpfieldData.FileName + "\"") : string.Empty));
					if (!string.IsNullOrEmpty(httpfieldData.MimeType))
					{
						memoryStream.WriteLine("Content-Type: " + httpfieldData.MimeType);
					}
					memoryStream.WriteLine("Content-Length: " + httpfieldData.Payload.Length.ToString());
					memoryStream.WriteLine();
					memoryStream.Write(httpfieldData.Payload, 0, httpfieldData.Payload.Length);
					memoryStream.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
				}
				memoryStream.WriteLine("--" + this.Boundary + "--");
				base.IsChanged = false;
				array = (this.CachedData = memoryStream.ToArray());
				array = array;
			}
			return array;
		}

		// Token: 0x040023BB RID: 9147
		private string Boundary;

		// Token: 0x040023BC RID: 9148
		private byte[] CachedData;
	}
}
