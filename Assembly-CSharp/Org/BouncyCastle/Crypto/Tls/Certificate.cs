using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A0 RID: 928
	public class Certificate
	{
		// Token: 0x06002310 RID: 8976 RVA: 0x000B673E File Offset: 0x000B493E
		public Certificate(X509CertificateStructure[] certificateList)
		{
			if (certificateList == null)
			{
				throw new ArgumentNullException("certificateList");
			}
			this.mCertificateList = certificateList;
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000B675B File Offset: 0x000B495B
		public virtual X509CertificateStructure[] GetCertificateList()
		{
			return this.CloneCertificateList();
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000B6763 File Offset: 0x000B4963
		public virtual X509CertificateStructure GetCertificateAt(int index)
		{
			return this.mCertificateList[index];
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x000B676D File Offset: 0x000B496D
		public virtual int Length
		{
			get
			{
				return this.mCertificateList.Length;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000B6777 File Offset: 0x000B4977
		public virtual bool IsEmpty
		{
			get
			{
				return this.mCertificateList.Length == 0;
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000B6784 File Offset: 0x000B4984
		public virtual void Encode(Stream output)
		{
			IList list = Platform.CreateArrayList(this.mCertificateList.Length);
			int num = 0;
			X509CertificateStructure[] array = this.mCertificateList;
			for (int i = 0; i < array.Length; i++)
			{
				byte[] encoded = array[i].GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 3;
			}
			TlsUtilities.CheckUint24(num);
			TlsUtilities.WriteUint24(num, output);
			foreach (object obj in list)
			{
				TlsUtilities.WriteOpaque24((byte[])obj, output);
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000B6830 File Offset: 0x000B4A30
		public static Certificate Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint24(input);
			if (num == 0)
			{
				return Certificate.EmptyChain;
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				Asn1Object obj = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(memoryStream));
				list.Add(X509CertificateStructure.GetInstance(obj));
			}
			X509CertificateStructure[] array = new X509CertificateStructure[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = (X509CertificateStructure)list[i];
			}
			return new Certificate(array);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000B68C4 File Offset: 0x000B4AC4
		protected virtual X509CertificateStructure[] CloneCertificateList()
		{
			return (X509CertificateStructure[])this.mCertificateList.Clone();
		}

		// Token: 0x040016E0 RID: 5856
		public static readonly Certificate EmptyChain = new Certificate(new X509CertificateStructure[0]);

		// Token: 0x040016E1 RID: 5857
		protected readonly X509CertificateStructure[] mCertificateList;
	}
}
