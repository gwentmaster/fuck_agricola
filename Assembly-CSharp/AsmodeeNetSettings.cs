using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class AsmodeeNetSettings : MonoBehaviour
{
	// Token: 0x0600015F RID: 351 RVA: 0x00007933 File Offset: 0x00005B33
	public static string GetClientID()
	{
		return "agricola";
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000793A File Offset: 0x00005B3A
	public static string GetClientSecret()
	{
		return "k6Fs6nSDQSlOXU8keyfXihkmiM87tkSToYLrkUY35O4=";
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00007941 File Offset: 0x00005B41
	public static int GetPartnerType()
	{
		return 24;
	}

	// Token: 0x040000A5 RID: 165
	private const string s_AsmodeeClientID = "agricola";

	// Token: 0x040000A6 RID: 166
	private const string s_AsmodeeClientSecret = "k6Fs6nSDQSlOXU8keyfXihkmiM87tkSToYLrkUY35O4=";

	// Token: 0x040000A7 RID: 167
	private const int s_AsmodeePartnerType = 24;
}
