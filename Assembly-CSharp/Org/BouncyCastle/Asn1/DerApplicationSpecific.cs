using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004EE RID: 1262
	public class DerApplicationSpecific : Asn1Object
	{
		// Token: 0x06002E65 RID: 11877 RVA: 0x000F1CC3 File Offset: 0x000EFEC3
		internal DerApplicationSpecific(bool isConstructed, int tag, byte[] octets)
		{
			this.isConstructed = isConstructed;
			this.tag = tag;
			this.octets = octets;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000F1CE0 File Offset: 0x000EFEE0
		public DerApplicationSpecific(int tag, byte[] octets) : this(false, tag, octets)
		{
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000F1CEB File Offset: 0x000EFEEB
		public DerApplicationSpecific(int tag, Asn1Encodable obj) : this(true, tag, obj)
		{
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000F1CF8 File Offset: 0x000EFEF8
		public DerApplicationSpecific(bool isExplicit, int tag, Asn1Encodable obj)
		{
			Asn1Object asn1Object = obj.ToAsn1Object();
			byte[] derEncoded = asn1Object.GetDerEncoded();
			this.isConstructed = Asn1TaggedObject.IsConstructed(isExplicit, asn1Object);
			this.tag = tag;
			if (isExplicit)
			{
				this.octets = derEncoded;
				return;
			}
			int lengthOfHeader = this.GetLengthOfHeader(derEncoded);
			byte[] array = new byte[derEncoded.Length - lengthOfHeader];
			Array.Copy(derEncoded, lengthOfHeader, array, 0, array.Length);
			this.octets = array;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000F1D60 File Offset: 0x000EFF60
		public DerApplicationSpecific(int tagNo, Asn1EncodableVector vec)
		{
			this.tag = tagNo;
			this.isConstructed = true;
			MemoryStream memoryStream = new MemoryStream();
			for (int num = 0; num != vec.Count; num++)
			{
				try
				{
					byte[] derEncoded = vec[num].GetDerEncoded();
					memoryStream.Write(derEncoded, 0, derEncoded.Length);
				}
				catch (IOException innerException)
				{
					throw new InvalidOperationException("malformed object", innerException);
				}
			}
			this.octets = memoryStream.ToArray();
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000F1DDC File Offset: 0x000EFFDC
		private int GetLengthOfHeader(byte[] data)
		{
			int num = (int)data[1];
			if (num == 128)
			{
				return 2;
			}
			if (num <= 127)
			{
				return 2;
			}
			int num2 = num & 127;
			if (num2 > 4)
			{
				throw new InvalidOperationException("DER length more than 4 bytes: " + num2);
			}
			return num2 + 2;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000F1E20 File Offset: 0x000F0020
		public bool IsConstructed()
		{
			return this.isConstructed;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000F1E28 File Offset: 0x000F0028
		public byte[] GetContents()
		{
			return this.octets;
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x000F1E30 File Offset: 0x000F0030
		public int ApplicationTag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000F1E38 File Offset: 0x000F0038
		public Asn1Object GetObject()
		{
			return Asn1Object.FromByteArray(this.GetContents());
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000F1E48 File Offset: 0x000F0048
		public Asn1Object GetObject(int derTagNo)
		{
			if (derTagNo >= 31)
			{
				throw new IOException("unsupported tag number");
			}
			byte[] encoded = base.GetEncoded();
			byte[] array = this.ReplaceTagNumber(derTagNo, encoded);
			if ((encoded[0] & 32) != 0)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] |= 32;
			}
			return Asn1Object.FromByteArray(array);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000F1E94 File Offset: 0x000F0094
		internal override void Encode(DerOutputStream derOut)
		{
			int num = 64;
			if (this.isConstructed)
			{
				num |= 32;
			}
			derOut.WriteEncoded(num, this.tag, this.octets);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000F1EC4 File Offset: 0x000F00C4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerApplicationSpecific derApplicationSpecific = asn1Object as DerApplicationSpecific;
			return derApplicationSpecific != null && (this.isConstructed == derApplicationSpecific.isConstructed && this.tag == derApplicationSpecific.tag) && Arrays.AreEqual(this.octets, derApplicationSpecific.octets);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000F1F0C File Offset: 0x000F010C
		protected override int Asn1GetHashCode()
		{
			return this.isConstructed.GetHashCode() ^ this.tag.GetHashCode() ^ Arrays.GetHashCode(this.octets);
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000F1F44 File Offset: 0x000F0144
		private byte[] ReplaceTagNumber(int newTag, byte[] input)
		{
			int num = (int)(input[0] & 31);
			int num2 = 1;
			if (num == 31)
			{
				num = 0;
				int num3 = (int)(input[num2++] & byte.MaxValue);
				if ((num3 & 127) == 0)
				{
					throw new InvalidOperationException("corrupted stream - invalid high tag number found");
				}
				while (num3 >= 0 && (num3 & 128) != 0)
				{
					num |= (num3 & 127);
					num <<= 7;
					num3 = (int)(input[num2++] & byte.MaxValue);
				}
				num |= (num3 & 127);
			}
			byte[] array = new byte[input.Length - num2 + 1];
			Array.Copy(input, num2, array, 1, array.Length - 1);
			array[0] = (byte)newTag;
			return array;
		}

		// Token: 0x04001E2A RID: 7722
		private readonly bool isConstructed;

		// Token: 0x04001E2B RID: 7723
		private readonly int tag;

		// Token: 0x04001E2C RID: 7724
		private readonly byte[] octets;
	}
}
