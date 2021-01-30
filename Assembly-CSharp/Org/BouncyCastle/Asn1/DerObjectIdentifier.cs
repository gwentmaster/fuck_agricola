using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FA RID: 1274
	public class DerObjectIdentifier : Asn1Object
	{
		// Token: 0x06002EE9 RID: 12009 RVA: 0x000F3314 File Offset: 0x000F1514
		public static DerObjectIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is DerObjectIdentifier)
			{
				return (DerObjectIdentifier)obj;
			}
			if (obj is byte[])
			{
				return DerObjectIdentifier.FromOctetString((byte[])obj);
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000F3361 File Offset: 0x000F1561
		public static DerObjectIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DerObjectIdentifier.GetInstance(obj.GetObject());
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000F336E File Offset: 0x000F156E
		public DerObjectIdentifier(string identifier)
		{
			if (identifier == null)
			{
				throw new ArgumentNullException("identifier");
			}
			if (!DerObjectIdentifier.IsValidIdentifier(identifier))
			{
				throw new FormatException("string " + identifier + " not an OID");
			}
			this.identifier = identifier;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000F33AC File Offset: 0x000F15AC
		internal DerObjectIdentifier(DerObjectIdentifier oid, string branchID)
		{
			if (!DerObjectIdentifier.IsValidBranchID(branchID, 0))
			{
				throw new ArgumentException("string " + branchID + " not a valid OID branch", "branchID");
			}
			this.identifier = oid.Id + "." + branchID;
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x000F33FA File Offset: 0x000F15FA
		public string Id
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000F3402 File Offset: 0x000F1602
		public virtual DerObjectIdentifier Branch(string branchID)
		{
			return new DerObjectIdentifier(this, branchID);
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000F340C File Offset: 0x000F160C
		public virtual bool On(DerObjectIdentifier stem)
		{
			string id = this.Id;
			string id2 = stem.Id;
			return id.Length > id2.Length && id[id2.Length] == '.' && Platform.StartsWith(id, id2);
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000F344E File Offset: 0x000F164E
		internal DerObjectIdentifier(byte[] bytes)
		{
			this.identifier = DerObjectIdentifier.MakeOidStringFromBytes(bytes);
			this.body = Arrays.Clone(bytes);
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000F3470 File Offset: 0x000F1670
		private void WriteField(Stream outputStream, long fieldValue)
		{
			byte[] array = new byte[9];
			int num = 8;
			array[num] = (byte)(fieldValue & 127L);
			while (fieldValue >= 128L)
			{
				fieldValue >>= 7;
				array[--num] = (byte)((fieldValue & 127L) | 128L);
			}
			outputStream.Write(array, num, 9 - num);
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000F34C0 File Offset: 0x000F16C0
		private void WriteField(Stream outputStream, BigInteger fieldValue)
		{
			int num = (fieldValue.BitLength + 6) / 7;
			if (num == 0)
			{
				outputStream.WriteByte(0);
				return;
			}
			BigInteger bigInteger = fieldValue;
			byte[] array = new byte[num];
			for (int i = num - 1; i >= 0; i--)
			{
				array[i] = (byte)((bigInteger.IntValue & 127) | 128);
				bigInteger = bigInteger.ShiftRight(7);
			}
			byte[] array2 = array;
			int num2 = num - 1;
			array2[num2] &= 127;
			outputStream.Write(array, 0, array.Length);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000F3530 File Offset: 0x000F1730
		private void DoOutput(MemoryStream bOut)
		{
			OidTokenizer oidTokenizer = new OidTokenizer(this.identifier);
			string text = oidTokenizer.NextToken();
			int num = int.Parse(text) * 40;
			text = oidTokenizer.NextToken();
			if (text.Length <= 18)
			{
				this.WriteField(bOut, (long)num + long.Parse(text));
			}
			else
			{
				this.WriteField(bOut, new BigInteger(text).Add(BigInteger.ValueOf((long)num)));
			}
			while (oidTokenizer.HasMoreTokens)
			{
				text = oidTokenizer.NextToken();
				if (text.Length <= 18)
				{
					this.WriteField(bOut, long.Parse(text));
				}
				else
				{
					this.WriteField(bOut, new BigInteger(text));
				}
			}
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000F35D0 File Offset: 0x000F17D0
		internal byte[] GetBody()
		{
			lock (this)
			{
				if (this.body == null)
				{
					MemoryStream memoryStream = new MemoryStream();
					this.DoOutput(memoryStream);
					this.body = memoryStream.ToArray();
				}
			}
			return this.body;
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000F362C File Offset: 0x000F182C
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(6, this.GetBody());
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000F363B File Offset: 0x000F183B
		protected override int Asn1GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000F3648 File Offset: 0x000F1848
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerObjectIdentifier derObjectIdentifier = asn1Object as DerObjectIdentifier;
			return derObjectIdentifier != null && this.identifier.Equals(derObjectIdentifier.identifier);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000F33FA File Offset: 0x000F15FA
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000F3674 File Offset: 0x000F1874
		private static bool IsValidBranchID(string branchID, int start)
		{
			bool flag = false;
			int num = branchID.Length;
			while (--num >= start)
			{
				char c = branchID[num];
				if ('0' <= c && c <= '9')
				{
					flag = true;
				}
				else
				{
					if (c != '.')
					{
						return false;
					}
					if (!flag)
					{
						return false;
					}
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000F36BC File Offset: 0x000F18BC
		private static bool IsValidIdentifier(string identifier)
		{
			if (identifier.Length < 3 || identifier[1] != '.')
			{
				return false;
			}
			char c = identifier[0];
			return c >= '0' && c <= '2' && DerObjectIdentifier.IsValidBranchID(identifier, 2);
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000F36FC File Offset: 0x000F18FC
		private static string MakeOidStringFromBytes(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			long num = 0L;
			BigInteger bigInteger = null;
			bool flag = true;
			for (int num2 = 0; num2 != bytes.Length; num2++)
			{
				int num3 = (int)bytes[num2];
				if (num <= 72057594037927808L)
				{
					num += (long)(num3 & 127);
					if ((num3 & 128) == 0)
					{
						if (flag)
						{
							if (num < 40L)
							{
								stringBuilder.Append('0');
							}
							else if (num < 80L)
							{
								stringBuilder.Append('1');
								num -= 40L;
							}
							else
							{
								stringBuilder.Append('2');
								num -= 80L;
							}
							flag = false;
						}
						stringBuilder.Append('.');
						stringBuilder.Append(num);
						num = 0L;
					}
					else
					{
						num <<= 7;
					}
				}
				else
				{
					if (bigInteger == null)
					{
						bigInteger = BigInteger.ValueOf(num);
					}
					bigInteger = bigInteger.Or(BigInteger.ValueOf((long)(num3 & 127)));
					if ((num3 & 128) == 0)
					{
						if (flag)
						{
							stringBuilder.Append('2');
							bigInteger = bigInteger.Subtract(BigInteger.ValueOf(80L));
							flag = false;
						}
						stringBuilder.Append('.');
						stringBuilder.Append(bigInteger);
						bigInteger = null;
						num = 0L;
					}
					else
					{
						bigInteger = bigInteger.ShiftLeft(7);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000F3814 File Offset: 0x000F1A14
		internal static DerObjectIdentifier FromOctetString(byte[] enc)
		{
			int num = Arrays.GetHashCode(enc) & 1023;
			DerObjectIdentifier[] obj = DerObjectIdentifier.cache;
			DerObjectIdentifier result;
			lock (obj)
			{
				DerObjectIdentifier derObjectIdentifier = DerObjectIdentifier.cache[num];
				if (derObjectIdentifier != null && Arrays.AreEqual(enc, derObjectIdentifier.GetBody()))
				{
					result = derObjectIdentifier;
				}
				else
				{
					result = (DerObjectIdentifier.cache[num] = new DerObjectIdentifier(enc));
				}
			}
			return result;
		}

		// Token: 0x04001E3E RID: 7742
		private readonly string identifier;

		// Token: 0x04001E3F RID: 7743
		private byte[] body;

		// Token: 0x04001E40 RID: 7744
		private const long LONG_LIMIT = 72057594037927808L;

		// Token: 0x04001E41 RID: 7745
		private static readonly DerObjectIdentifier[] cache = new DerObjectIdentifier[1024];
	}
}
