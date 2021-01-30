using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004C9 RID: 1225
	public abstract class Asn1Encodable : IAsn1Convertible
	{
		// Token: 0x06002D84 RID: 11652 RVA: 0x000EF79E File Offset: 0x000ED99E
		public byte[] GetEncoded()
		{
			MemoryStream memoryStream = new MemoryStream();
			new Asn1OutputStream(memoryStream).WriteObject(this);
			return memoryStream.ToArray();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000EF7B6 File Offset: 0x000ED9B6
		public byte[] GetEncoded(string encoding)
		{
			if (encoding.Equals("DER"))
			{
				MemoryStream memoryStream = new MemoryStream();
				new DerOutputStream(memoryStream).WriteObject(this);
				return memoryStream.ToArray();
			}
			return this.GetEncoded();
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000EF7E4 File Offset: 0x000ED9E4
		public byte[] GetDerEncoded()
		{
			byte[] result;
			try
			{
				result = this.GetEncoded("DER");
			}
			catch (IOException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000EF818 File Offset: 0x000EDA18
		public sealed override int GetHashCode()
		{
			return this.ToAsn1Object().CallAsn1GetHashCode();
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000EF828 File Offset: 0x000EDA28
		public sealed override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			IAsn1Convertible asn1Convertible = obj as IAsn1Convertible;
			if (asn1Convertible == null)
			{
				return false;
			}
			Asn1Object asn1Object = this.ToAsn1Object();
			Asn1Object asn1Object2 = asn1Convertible.ToAsn1Object();
			return asn1Object == asn1Object2 || asn1Object.CallAsn1Equals(asn1Object2);
		}

		// Token: 0x06002D89 RID: 11657
		public abstract Asn1Object ToAsn1Object();

		// Token: 0x04001DE1 RID: 7649
		public const string Der = "DER";

		// Token: 0x04001DE2 RID: 7650
		public const string Ber = "BER";
	}
}
