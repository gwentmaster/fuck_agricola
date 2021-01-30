using System;
using System.Collections.Generic;
using System.Text;

namespace BestHTTP.Forms
{
	// Token: 0x020005E9 RID: 1513
	public class HTTPFormBase
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x00111D3D File Offset: 0x0010FF3D
		// (set) Token: 0x0600379A RID: 14234 RVA: 0x00111D45 File Offset: 0x0010FF45
		public List<HTTPFieldData> Fields { get; set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x00111D4E File Offset: 0x0010FF4E
		public bool IsEmpty
		{
			get
			{
				return this.Fields == null || this.Fields.Count == 0;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x0600379C RID: 14236 RVA: 0x00111D68 File Offset: 0x0010FF68
		// (set) Token: 0x0600379D RID: 14237 RVA: 0x00111D70 File Offset: 0x0010FF70
		public bool IsChanged { get; protected set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x0600379E RID: 14238 RVA: 0x00111D79 File Offset: 0x0010FF79
		// (set) Token: 0x0600379F RID: 14239 RVA: 0x00111D81 File Offset: 0x0010FF81
		public bool HasBinary { get; protected set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x00111D8A File Offset: 0x0010FF8A
		// (set) Token: 0x060037A1 RID: 14241 RVA: 0x00111D92 File Offset: 0x0010FF92
		public bool HasLongValue { get; protected set; }

		// Token: 0x060037A2 RID: 14242 RVA: 0x00111D9B File Offset: 0x0010FF9B
		public void AddBinaryData(string fieldName, byte[] content)
		{
			this.AddBinaryData(fieldName, content, null, null);
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x00111DA7 File Offset: 0x0010FFA7
		public void AddBinaryData(string fieldName, byte[] content, string fileName)
		{
			this.AddBinaryData(fieldName, content, fileName, null);
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x00111DB4 File Offset: 0x0010FFB4
		public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
		{
			if (this.Fields == null)
			{
				this.Fields = new List<HTTPFieldData>();
			}
			HTTPFieldData httpfieldData = new HTTPFieldData();
			httpfieldData.Name = fieldName;
			if (fileName == null)
			{
				httpfieldData.FileName = fieldName + ".dat";
			}
			else
			{
				httpfieldData.FileName = fileName;
			}
			if (mimeType == null)
			{
				httpfieldData.MimeType = "application/octet-stream";
			}
			else
			{
				httpfieldData.MimeType = mimeType;
			}
			httpfieldData.Binary = content;
			this.Fields.Add(httpfieldData);
			this.HasBinary = (this.IsChanged = true);
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x00111E3A File Offset: 0x0011003A
		public void AddField(string fieldName, string value)
		{
			this.AddField(fieldName, value, Encoding.UTF8);
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x00111E4C File Offset: 0x0011004C
		public void AddField(string fieldName, string value, Encoding e)
		{
			if (this.Fields == null)
			{
				this.Fields = new List<HTTPFieldData>();
			}
			HTTPFieldData httpfieldData = new HTTPFieldData();
			httpfieldData.Name = fieldName;
			httpfieldData.FileName = null;
			if (e != null)
			{
				httpfieldData.MimeType = "text/plain; charset=" + e.WebName;
			}
			httpfieldData.Text = value;
			httpfieldData.Encoding = e;
			this.Fields.Add(httpfieldData);
			this.IsChanged = true;
			this.HasLongValue |= (value.Length > 256);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x00111ED4 File Offset: 0x001100D4
		public virtual void CopyFrom(HTTPFormBase fields)
		{
			this.Fields = new List<HTTPFieldData>(fields.Fields);
			this.IsChanged = true;
			this.HasBinary = fields.HasBinary;
			this.HasLongValue = fields.HasLongValue;
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x00003A58 File Offset: 0x00001C58
		public virtual void PrepareRequest(HTTPRequest request)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x00003A58 File Offset: 0x00001C58
		public virtual byte[] GetData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040023B0 RID: 9136
		private const int LongLength = 256;
	}
}
