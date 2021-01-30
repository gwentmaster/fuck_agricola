using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
public class CustomDictionnaryComparer : IEqualityComparer<KeyValuePair<string, string>>
{
	// Token: 0x0600006A RID: 106 RVA: 0x00003402 File Offset: 0x00001602
	public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
	{
		return x.Key.Equals(y.Key);
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003417 File Offset: 0x00001617
	public int GetHashCode(KeyValuePair<string, string> obj)
	{
		return obj.Key.GetHashCode();
	}
}
