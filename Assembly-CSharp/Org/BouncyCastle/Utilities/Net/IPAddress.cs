using System;
using System.Globalization;

namespace Org.BouncyCastle.Utilities.Net
{
	// Token: 0x02000297 RID: 663
	public class IPAddress
	{
		// Token: 0x06001610 RID: 5648 RVA: 0x0008017F File Offset: 0x0007E37F
		public static bool IsValid(string address)
		{
			return IPAddress.IsValidIPv4(address) || IPAddress.IsValidIPv6(address);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00080191 File Offset: 0x0007E391
		public static bool IsValidWithNetMask(string address)
		{
			return IPAddress.IsValidIPv4WithNetmask(address) || IPAddress.IsValidIPv6WithNetmask(address);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x000801A4 File Offset: 0x0007E3A4
		public static bool IsValidIPv4(string address)
		{
			try
			{
				return IPAddress.unsafeIsValidIPv4(address);
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000801E0 File Offset: 0x0007E3E0
		private static bool unsafeIsValidIPv4(string address)
		{
			if (address.Length == 0)
			{
				return false;
			}
			int num = 0;
			string text = address + ".";
			int num2 = 0;
			int num3;
			while (num2 < text.Length && (num3 = text.IndexOf('.', num2)) > num2)
			{
				if (num == 4)
				{
					return false;
				}
				int num4 = int.Parse(text.Substring(num2, num3 - num2));
				if (num4 < 0 || num4 > 255)
				{
					return false;
				}
				num2 = num3 + 1;
				num++;
			}
			return num == 4;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00080254 File Offset: 0x0007E454
		public static bool IsValidIPv4WithNetmask(string address)
		{
			int num = address.IndexOf('/');
			string text = address.Substring(num + 1);
			return num > 0 && IPAddress.IsValidIPv4(address.Substring(0, num)) && (IPAddress.IsValidIPv4(text) || IPAddress.IsMaskValue(text, 32));
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0008029C File Offset: 0x0007E49C
		public static bool IsValidIPv6WithNetmask(string address)
		{
			int num = address.IndexOf('/');
			string text = address.Substring(num + 1);
			return num > 0 && IPAddress.IsValidIPv6(address.Substring(0, num)) && (IPAddress.IsValidIPv6(text) || IPAddress.IsMaskValue(text, 128));
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x000802E8 File Offset: 0x0007E4E8
		private static bool IsMaskValue(string component, int size)
		{
			int num = int.Parse(component);
			try
			{
				return num >= 0 && num <= size;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00080334 File Offset: 0x0007E534
		public static bool IsValidIPv6(string address)
		{
			try
			{
				return IPAddress.unsafeIsValidIPv6(address);
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00080370 File Offset: 0x0007E570
		private static bool unsafeIsValidIPv6(string address)
		{
			if (address.Length == 0)
			{
				return false;
			}
			int num = 0;
			string text = address + ":";
			bool flag = false;
			int num2 = 0;
			int num3;
			while (num2 < text.Length && (num3 = text.IndexOf(':', num2)) >= num2)
			{
				if (num == 8)
				{
					return false;
				}
				if (num2 != num3)
				{
					string text2 = text.Substring(num2, num3 - num2);
					if (num3 == text.Length - 1 && text2.IndexOf('.') > 0)
					{
						if (!IPAddress.IsValidIPv4(text2))
						{
							return false;
						}
						num++;
					}
					else
					{
						int num4 = int.Parse(text.Substring(num2, num3 - num2), NumberStyles.AllowHexSpecifier);
						if (num4 < 0 || num4 > 65535)
						{
							return false;
						}
					}
				}
				else
				{
					if (num3 != 1 && num3 != text.Length - 1 && flag)
					{
						return false;
					}
					flag = true;
				}
				num2 = num3 + 1;
				num++;
			}
			return num == 8 || flag;
		}
	}
}
