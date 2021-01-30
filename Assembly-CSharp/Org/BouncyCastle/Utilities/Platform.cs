using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000289 RID: 649
	internal abstract class Platform
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x000799DE File Offset: 0x00077BDE
		private static string GetNewLine()
		{
			return Environment.NewLine;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000799E5 File Offset: 0x00077BE5
		internal static bool EqualsIgnoreCase(string a, string b)
		{
			return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000799F4 File Offset: 0x00077BF4
		internal static string GetEnvironmentVariable(string variable)
		{
			string result;
			try
			{
				result = Environment.GetEnvironmentVariable(variable);
			}
			catch (SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00079A20 File Offset: 0x00077C20
		internal static Exception CreateNotImplementedException(string message)
		{
			return new NotImplementedException(message);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00079A28 File Offset: 0x00077C28
		internal static IList CreateArrayList()
		{
			return new ArrayList();
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00079A2F File Offset: 0x00077C2F
		internal static IList CreateArrayList(int capacity)
		{
			return new ArrayList(capacity);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00079A37 File Offset: 0x00077C37
		internal static IList CreateArrayList(ICollection collection)
		{
			return new ArrayList(collection);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00079A40 File Offset: 0x00077C40
		internal static IList CreateArrayList(IEnumerable collection)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object value in collection)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00079A98 File Offset: 0x00077C98
		internal static IDictionary CreateHashtable()
		{
			return new Hashtable();
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00079A9F File Offset: 0x00077C9F
		internal static IDictionary CreateHashtable(int capacity)
		{
			return new Hashtable(capacity);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00079AA7 File Offset: 0x00077CA7
		internal static IDictionary CreateHashtable(IDictionary dictionary)
		{
			return new Hashtable(dictionary);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x00079AAF File Offset: 0x00077CAF
		internal static string ToLowerInvariant(string s)
		{
			return s.ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00079ABC File Offset: 0x00077CBC
		internal static string ToUpperInvariant(string s)
		{
			return s.ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00079AC9 File Offset: 0x00077CC9
		internal static void Dispose(Stream s)
		{
			s.Close();
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00079AD1 File Offset: 0x00077CD1
		internal static void Dispose(TextWriter t)
		{
			t.Close();
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00079AD9 File Offset: 0x00077CD9
		internal static int IndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.IndexOf(source, value, CompareOptions.Ordinal);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00079AEC File Offset: 0x00077CEC
		internal static int LastIndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.LastIndexOf(source, value, CompareOptions.Ordinal);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00079AFF File Offset: 0x00077CFF
		internal static bool StartsWith(string source, string prefix)
		{
			return Platform.InvariantCompareInfo.IsPrefix(source, prefix, CompareOptions.Ordinal);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00079B12 File Offset: 0x00077D12
		internal static bool EndsWith(string source, string suffix)
		{
			return Platform.InvariantCompareInfo.IsSuffix(source, suffix, CompareOptions.Ordinal);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00079B25 File Offset: 0x00077D25
		internal static string GetTypeName(object obj)
		{
			return obj.GetType().FullName;
		}

		// Token: 0x04001395 RID: 5013
		private static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

		// Token: 0x04001396 RID: 5014
		internal static readonly string NewLine = Platform.GetNewLine();
	}
}
