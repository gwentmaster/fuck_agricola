using System;
using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F0 RID: 1264
	public class DerBitString : DerStringBase
	{
		// Token: 0x06002E7B RID: 11899 RVA: 0x000F2134 File Offset: 0x000F0334
		public static DerBitString GetInstance(object obj)
		{
			if (obj == null || obj is DerBitString)
			{
				return (DerBitString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerBitString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString());
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000F21AC File Offset: 0x000F03AC
		public static DerBitString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBitString)
			{
				return DerBitString.GetInstance(@object);
			}
			return DerBitString.FromAsn1Octets(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000F21E4 File Offset: 0x000F03E4
		public DerBitString(byte[] data, int padBits)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (padBits < 0 || padBits > 7)
			{
				throw new ArgumentException("must be in the range 0 to 7", "padBits");
			}
			if (data.Length == 0 && padBits != 0)
			{
				throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");
			}
			this.mData = Arrays.Clone(data);
			this.mPadBits = padBits;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000F2242 File Offset: 0x000F0442
		public DerBitString(byte[] data) : this(data, 0)
		{
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000F224C File Offset: 0x000F044C
		public DerBitString(int namedBits)
		{
			if (namedBits == 0)
			{
				this.mData = new byte[0];
				this.mPadBits = 0;
				return;
			}
			int num = (BigInteger.BitLen(namedBits) + 7) / 8;
			byte[] array = new byte[num];
			num--;
			for (int i = 0; i < num; i++)
			{
				array[i] = (byte)namedBits;
				namedBits >>= 8;
			}
			array[num] = (byte)namedBits;
			int num2 = 0;
			while ((namedBits & 1 << num2) == 0)
			{
				num2++;
			}
			this.mData = array;
			this.mPadBits = num2;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000F22C7 File Offset: 0x000F04C7
		public DerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded())
		{
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000F22D5 File Offset: 0x000F04D5
		public virtual byte[] GetOctets()
		{
			if (this.mPadBits != 0)
			{
				throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
			}
			return Arrays.Clone(this.mData);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000F22F8 File Offset: 0x000F04F8
		public virtual byte[] GetBytes()
		{
			byte[] array = Arrays.Clone(this.mData);
			if (this.mPadBits > 0)
			{
				byte[] array2 = array;
				int num = array.Length - 1;
				array2[num] &= (byte)(255 << this.mPadBits);
			}
			return array;
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x000F233B File Offset: 0x000F053B
		public virtual int PadBits
		{
			get
			{
				return this.mPadBits;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x000F2344 File Offset: 0x000F0544
		public virtual int IntValue
		{
			get
			{
				int num = 0;
				int num2 = Math.Min(4, this.mData.Length);
				for (int i = 0; i < num2; i++)
				{
					num |= (int)this.mData[i] << 8 * i;
				}
				if (this.mPadBits > 0 && num2 == this.mData.Length)
				{
					int num3 = (1 << this.mPadBits) - 1;
					num &= ~(num3 << 8 * (num2 - 1));
				}
				return num;
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000F23B4 File Offset: 0x000F05B4
		internal override void Encode(DerOutputStream derOut)
		{
			if (this.mPadBits > 0)
			{
				int num = (int)this.mData[this.mData.Length - 1];
				int num2 = (1 << this.mPadBits) - 1;
				int num3 = num & num2;
				if (num3 != 0)
				{
					byte[] array = Arrays.Prepend(this.mData, (byte)this.mPadBits);
					array[array.Length - 1] = (byte)(num ^ num3);
					derOut.WriteEncoded(3, array);
					return;
				}
			}
			derOut.WriteEncoded(3, (byte)this.mPadBits, this.mData);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000F242C File Offset: 0x000F062C
		protected override int Asn1GetHashCode()
		{
			return this.mPadBits.GetHashCode() ^ Arrays.GetHashCode(this.mData);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000F2454 File Offset: 0x000F0654
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBitString derBitString = asn1Object as DerBitString;
			return derBitString != null && this.mPadBits == derBitString.mPadBits && Arrays.AreEqual(this.mData, derBitString.mData);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000F2490 File Offset: 0x000F0690
		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder("#");
			byte[] derEncoded = base.GetDerEncoded();
			for (int num = 0; num != derEncoded.Length; num++)
			{
				uint num2 = (uint)derEncoded[num];
				stringBuilder.Append(DerBitString.table[(int)(num2 >> 4 & 15U)]);
				stringBuilder.Append(DerBitString.table[(int)(derEncoded[num] & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000F24F0 File Offset: 0x000F06F0
		internal static DerBitString FromAsn1Octets(byte[] octets)
		{
			if (octets.Length < 1)
			{
				throw new ArgumentException("truncated BIT STRING detected", "octets");
			}
			int num = (int)octets[0];
			byte[] array = Arrays.CopyOfRange(octets, 1, octets.Length);
			if (num > 0 && num < 8 && array.Length != 0)
			{
				bool flag = array[array.Length - 1] != 0;
				int num2 = (1 << num) - 1;
				if (((flag ? 1 : 0) & num2) != 0)
				{
					return new BerBitString(array, num);
				}
			}
			return new DerBitString(array, num);
		}

		// Token: 0x04001E2E RID: 7726
		private static readonly char[] table = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		// Token: 0x04001E2F RID: 7727
		protected readonly byte[] mData;

		// Token: 0x04001E30 RID: 7728
		protected readonly int mPadBits;
	}
}
