using System;
using Org.BouncyCastle.Utilities.Date;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000285 RID: 645
	internal abstract class Enums
	{
		// Token: 0x06001560 RID: 5472 RVA: 0x00079914 File Offset: 0x00077B14
		internal static Enum GetEnumValue(Type enumType, string s)
		{
			if (s.Length > 0 && char.IsLetter(s[0]) && s.IndexOf(',') < 0)
			{
				s = s.Replace('-', '_');
				s = s.Replace('/', '_');
				return (Enum)Enum.Parse(enumType, s, false);
			}
			throw new ArgumentException();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0007996E File Offset: 0x00077B6E
		internal static Array GetEnumValues(Type enumType)
		{
			return Enum.GetValues(enumType);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00079978 File Offset: 0x00077B78
		internal static Enum GetArbitraryValue(Type enumType)
		{
			Array enumValues = Enums.GetEnumValues(enumType);
			int index = (int)(DateTimeUtilities.CurrentUnixMs() & 2147483647L) % enumValues.Length;
			return (Enum)enumValues.GetValue(index);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000799AD File Offset: 0x00077BAD
		internal static bool IsEnumType(Type t)
		{
			return t.IsEnum;
		}
	}
}
