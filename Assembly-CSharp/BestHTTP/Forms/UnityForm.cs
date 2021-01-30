using System;
using UnityEngine;

namespace BestHTTP.Forms
{
	// Token: 0x020005EE RID: 1518
	public sealed class UnityForm : HTTPFormBase
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x00112302 File Offset: 0x00110502
		// (set) Token: 0x060037B6 RID: 14262 RVA: 0x0011230A File Offset: 0x0011050A
		public WWWForm Form { get; set; }

		// Token: 0x060037B7 RID: 14263 RVA: 0x00112259 File Offset: 0x00110459
		public UnityForm()
		{
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x00112313 File Offset: 0x00110513
		public UnityForm(WWWForm form)
		{
			this.Form = form;
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x00112324 File Offset: 0x00110524
		public override void CopyFrom(HTTPFormBase fields)
		{
			base.Fields = fields.Fields;
			base.IsChanged = true;
			if (this.Form == null)
			{
				this.Form = new WWWForm();
				if (base.Fields != null)
				{
					for (int i = 0; i < base.Fields.Count; i++)
					{
						HTTPFieldData httpfieldData = base.Fields[i];
						if (string.IsNullOrEmpty(httpfieldData.Text) && httpfieldData.Binary != null)
						{
							this.Form.AddBinaryData(httpfieldData.Name, httpfieldData.Binary, httpfieldData.FileName, httpfieldData.MimeType);
						}
						else
						{
							this.Form.AddField(httpfieldData.Name, httpfieldData.Text, httpfieldData.Encoding);
						}
					}
				}
			}
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x001123DC File Offset: 0x001105DC
		public override void PrepareRequest(HTTPRequest request)
		{
			if (this.Form.headers.ContainsKey("Content-Type"))
			{
				request.SetHeader("Content-Type", this.Form.headers["Content-Type"]);
				return;
			}
			request.SetHeader("Content-Type", "application/x-www-form-urlencoded");
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x00112431 File Offset: 0x00110631
		public override byte[] GetData()
		{
			return this.Form.data;
		}
	}
}
