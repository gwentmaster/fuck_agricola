using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CE RID: 1230
	public abstract class Asn1Object : Asn1Encodable
	{
		// Token: 0x06002DA7 RID: 11687 RVA: 0x000EFF0C File Offset: 0x000EE10C
		public static Asn1Object FromByteArray(byte[] data)
		{
			Asn1Object result;
			try
			{
				MemoryStream memoryStream = new MemoryStream(data, false);
				Asn1Object asn1Object = new Asn1InputStream(memoryStream, data.Length).ReadObject();
				if (memoryStream.Position != memoryStream.Length)
				{
					throw new IOException("extra data found after object");
				}
				result = asn1Object;
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in byte array");
			}
			return result;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000EFF68 File Offset: 0x000EE168
		public static Asn1Object FromStream(Stream inStr)
		{
			Asn1Object result;
			try
			{
				result = new Asn1InputStream(inStr).ReadObject();
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in stream");
			}
			return result;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x00035D67 File Offset: 0x00033F67
		public sealed override Asn1Object ToAsn1Object()
		{
			return this;
		}

		// Token: 0x06002DAA RID: 11690
		internal abstract void Encode(DerOutputStream derOut);

		// Token: 0x06002DAB RID: 11691
		protected abstract bool Asn1Equals(Asn1Object asn1Object);

		// Token: 0x06002DAC RID: 11692
		protected abstract int Asn1GetHashCode();

		// Token: 0x06002DAD RID: 11693 RVA: 0x000EFFA0 File Offset: 0x000EE1A0
		internal bool CallAsn1Equals(Asn1Object obj)
		{
			return this.Asn1Equals(obj);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000EFFA9 File Offset: 0x000EE1A9
		internal int CallAsn1GetHashCode()
		{
			return this.Asn1GetHashCode();
		}
	}
}
