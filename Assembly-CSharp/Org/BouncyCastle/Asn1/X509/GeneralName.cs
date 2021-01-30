using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Net;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052A RID: 1322
	public class GeneralName : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003039 RID: 12345 RVA: 0x000F7000 File Offset: 0x000F5200
		public GeneralName(X509Name directoryName)
		{
			this.obj = directoryName;
			this.tag = 4;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000F7016 File Offset: 0x000F5216
		public GeneralName(Asn1Object name, int tag)
		{
			this.obj = name;
			this.tag = tag;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000F702C File Offset: 0x000F522C
		public GeneralName(int tag, Asn1Encodable name)
		{
			this.obj = name;
			this.tag = tag;
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000F7044 File Offset: 0x000F5244
		public GeneralName(int tag, string name)
		{
			this.tag = tag;
			if (tag == 1 || tag == 2 || tag == 6)
			{
				this.obj = new DerIA5String(name);
				return;
			}
			if (tag == 8)
			{
				this.obj = new DerObjectIdentifier(name);
				return;
			}
			if (tag == 4)
			{
				this.obj = new X509Name(name);
				return;
			}
			if (tag != 7)
			{
				throw new ArgumentException("can't process string for tag: " + tag, "tag");
			}
			byte[] array = this.toGeneralNameEncoding(name);
			if (array == null)
			{
				throw new ArgumentException("IP Address is invalid", "name");
			}
			this.obj = new DerOctetString(array);
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000F70E0 File Offset: 0x000F52E0
		public static GeneralName GetInstance(object obj)
		{
			if (obj == null || obj is GeneralName)
			{
				return (GeneralName)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				int tagNo = asn1TaggedObject.TagNo;
				switch (tagNo)
				{
				case 0:
					return new GeneralName(tagNo, Asn1Sequence.GetInstance(asn1TaggedObject, false));
				case 1:
					return new GeneralName(tagNo, DerIA5String.GetInstance(asn1TaggedObject, false));
				case 2:
					return new GeneralName(tagNo, DerIA5String.GetInstance(asn1TaggedObject, false));
				case 3:
					throw new ArgumentException("unknown tag: " + tagNo);
				case 4:
					return new GeneralName(tagNo, X509Name.GetInstance(asn1TaggedObject, true));
				case 5:
					return new GeneralName(tagNo, Asn1Sequence.GetInstance(asn1TaggedObject, false));
				case 6:
					return new GeneralName(tagNo, DerIA5String.GetInstance(asn1TaggedObject, false));
				case 7:
					return new GeneralName(tagNo, Asn1OctetString.GetInstance(asn1TaggedObject, false));
				case 8:
					return new GeneralName(tagNo, DerObjectIdentifier.GetInstance(asn1TaggedObject, false));
				}
			}
			if (obj is byte[])
			{
				try
				{
					return GeneralName.GetInstance(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (IOException)
				{
					throw new ArgumentException("unable to parse encoded general name");
				}
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000F7220 File Offset: 0x000F5420
		public static GeneralName GetInstance(Asn1TaggedObject tagObj, bool explicitly)
		{
			return GeneralName.GetInstance(Asn1TaggedObject.GetInstance(tagObj, true));
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000F722E File Offset: 0x000F542E
		public int TagNo
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x000F7236 File Offset: 0x000F5436
		public Asn1Encodable Name
		{
			get
			{
				return this.obj;
			}
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000F7240 File Offset: 0x000F5440
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.tag);
			stringBuilder.Append(": ");
			switch (this.tag)
			{
			case 1:
			case 2:
			case 6:
				stringBuilder.Append(DerIA5String.GetInstance(this.obj).GetString());
				goto IL_8C;
			case 4:
				stringBuilder.Append(X509Name.GetInstance(this.obj).ToString());
				goto IL_8C;
			}
			stringBuilder.Append(this.obj.ToString());
			IL_8C:
			return stringBuilder.ToString();
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000F72E0 File Offset: 0x000F54E0
		private byte[] toGeneralNameEncoding(string ip)
		{
			if (Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6WithNetmask(ip) || Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6(ip))
			{
				int num = ip.IndexOf('/');
				if (num < 0)
				{
					byte[] array = new byte[16];
					int[] parsedIp = this.parseIPv6(ip);
					this.copyInts(parsedIp, array, 0);
					return array;
				}
				byte[] array2 = new byte[32];
				int[] parsedIp2 = this.parseIPv6(ip.Substring(0, num));
				this.copyInts(parsedIp2, array2, 0);
				string text = ip.Substring(num + 1);
				if (text.IndexOf(':') > 0)
				{
					parsedIp2 = this.parseIPv6(text);
				}
				else
				{
					parsedIp2 = this.parseMask(text);
				}
				this.copyInts(parsedIp2, array2, 16);
				return array2;
			}
			else
			{
				if (!Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4WithNetmask(ip) && !Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4(ip))
				{
					return null;
				}
				int num2 = ip.IndexOf('/');
				if (num2 < 0)
				{
					byte[] array3 = new byte[4];
					this.parseIPv4(ip, array3, 0);
					return array3;
				}
				byte[] array4 = new byte[8];
				this.parseIPv4(ip.Substring(0, num2), array4, 0);
				string text2 = ip.Substring(num2 + 1);
				if (text2.IndexOf('.') > 0)
				{
					this.parseIPv4(text2, array4, 4);
				}
				else
				{
					this.parseIPv4Mask(text2, array4, 4);
				}
				return array4;
			}
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000F7408 File Offset: 0x000F5608
		private void parseIPv4Mask(string mask, byte[] addr, int offset)
		{
			int num = int.Parse(mask);
			for (int num2 = 0; num2 != num; num2++)
			{
				int num3 = num2 / 8 + offset;
				addr[num3] |= (byte)(1 << num2 % 8);
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000F7444 File Offset: 0x000F5644
		private void parseIPv4(string ip, byte[] addr, int offset)
		{
			foreach (string s in ip.Split(new char[]
			{
				'.',
				'/'
			}))
			{
				addr[offset++] = (byte)int.Parse(s);
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000F748C File Offset: 0x000F568C
		private int[] parseMask(string mask)
		{
			int[] array = new int[8];
			int num = int.Parse(mask);
			for (int num2 = 0; num2 != num; num2++)
			{
				array[num2 / 16] |= 1 << num2 % 16;
			}
			return array;
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x000F74CC File Offset: 0x000F56CC
		private void copyInts(int[] parsedIp, byte[] addr, int offSet)
		{
			for (int num = 0; num != parsedIp.Length; num++)
			{
				addr[num * 2 + offSet] = (byte)(parsedIp[num] >> 8);
				addr[num * 2 + 1 + offSet] = (byte)parsedIp[num];
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000F7504 File Offset: 0x000F5704
		private int[] parseIPv6(string ip)
		{
			if (Platform.StartsWith(ip, "::"))
			{
				ip = ip.Substring(1);
			}
			else if (Platform.EndsWith(ip, "::"))
			{
				ip = ip.Substring(0, ip.Length - 1);
			}
			IEnumerator enumerator = ip.Split(new char[]
			{
				':'
			}).GetEnumerator();
			int num = 0;
			int[] array = new int[8];
			int num2 = -1;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text = (string)obj;
				if (text.Length == 0)
				{
					num2 = num;
					array[num++] = 0;
				}
				else if (text.IndexOf('.') < 0)
				{
					array[num++] = int.Parse(text, NumberStyles.AllowHexSpecifier);
				}
				else
				{
					string[] array2 = text.Split(new char[]
					{
						'.'
					});
					array[num++] = (int.Parse(array2[0]) << 8 | int.Parse(array2[1]));
					array[num++] = (int.Parse(array2[2]) << 8 | int.Parse(array2[3]));
				}
			}
			if (num != array.Length)
			{
				Array.Copy(array, num2, array, array.Length - (num - num2), num - num2);
				for (int num3 = num2; num3 != array.Length - (num - num2); num3++)
				{
					array[num3] = 0;
				}
			}
			return array;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000F763C File Offset: 0x000F583C
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(this.tag == 4, this.tag, this.obj);
		}

		// Token: 0x04001ED1 RID: 7889
		public const int OtherName = 0;

		// Token: 0x04001ED2 RID: 7890
		public const int Rfc822Name = 1;

		// Token: 0x04001ED3 RID: 7891
		public const int DnsName = 2;

		// Token: 0x04001ED4 RID: 7892
		public const int X400Address = 3;

		// Token: 0x04001ED5 RID: 7893
		public const int DirectoryName = 4;

		// Token: 0x04001ED6 RID: 7894
		public const int EdiPartyName = 5;

		// Token: 0x04001ED7 RID: 7895
		public const int UniformResourceIdentifier = 6;

		// Token: 0x04001ED8 RID: 7896
		public const int IPAddress = 7;

		// Token: 0x04001ED9 RID: 7897
		public const int RegisteredID = 8;

		// Token: 0x04001EDA RID: 7898
		internal readonly Asn1Encodable obj;

		// Token: 0x04001EDB RID: 7899
		internal readonly int tag;
	}
}
