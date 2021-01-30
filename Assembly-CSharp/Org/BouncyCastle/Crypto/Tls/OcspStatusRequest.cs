using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CA RID: 970
	public class OcspStatusRequest
	{
		// Token: 0x060023C0 RID: 9152 RVA: 0x000B7E34 File Offset: 0x000B6034
		public OcspStatusRequest(IList responderIDList, X509Extensions requestExtensions)
		{
			this.mResponderIDList = responderIDList;
			this.mRequestExtensions = requestExtensions;
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x000B7E4A File Offset: 0x000B604A
		public virtual IList ResponderIDList
		{
			get
			{
				return this.mResponderIDList;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x000B7E52 File Offset: 0x000B6052
		public virtual X509Extensions RequestExtensions
		{
			get
			{
				return this.mRequestExtensions;
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000B7E5C File Offset: 0x000B605C
		public virtual void Encode(Stream output)
		{
			if (this.mResponderIDList == null || this.mResponderIDList.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				for (int i = 0; i < this.mResponderIDList.Count; i++)
				{
					TlsUtilities.WriteOpaque16(((ResponderID)this.mResponderIDList[i]).GetEncoded("DER"), memoryStream);
				}
				TlsUtilities.CheckUint16(memoryStream.Length);
				TlsUtilities.WriteUint16((int)memoryStream.Length, output);
				memoryStream.WriteTo(output);
			}
			if (this.mRequestExtensions == null)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			byte[] encoded = this.mRequestExtensions.GetEncoded("DER");
			TlsUtilities.CheckUint16(encoded.Length);
			TlsUtilities.WriteUint16(encoded.Length, output);
			output.Write(encoded, 0, encoded.Length);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000B7F24 File Offset: 0x000B6124
		public static OcspStatusRequest Parse(Stream input)
		{
			IList list = Platform.CreateArrayList();
			int num = TlsUtilities.ReadUint16(input);
			if (num > 0)
			{
				MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
				do
				{
					ResponderID instance = ResponderID.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream)));
					list.Add(instance);
				}
				while (memoryStream.Position < memoryStream.Length);
			}
			X509Extensions requestExtensions = null;
			int num2 = TlsUtilities.ReadUint16(input);
			if (num2 > 0)
			{
				requestExtensions = X509Extensions.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadFully(num2, input)));
			}
			return new OcspStatusRequest(list, requestExtensions);
		}

		// Token: 0x040018D1 RID: 6353
		protected readonly IList mResponderIDList;

		// Token: 0x040018D2 RID: 6354
		protected readonly X509Extensions mRequestExtensions;
	}
}
