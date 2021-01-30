using System;
using System.Runtime.InteropServices;

// Token: 0x020000B9 RID: 185
[Serializable]
public struct VerifyAccountData
{
	// Token: 0x04000800 RID: 2048
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
	public string username;
}
