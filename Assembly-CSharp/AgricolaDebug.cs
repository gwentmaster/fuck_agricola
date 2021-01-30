using System;
using System.Runtime.InteropServices;

// Token: 0x02000082 RID: 130
public class AgricolaDebug
{
	// Token: 0x060005FA RID: 1530
	[DllImport("AgricolaLib")]
	public static extern void SetDebugFunction(AgricolaDebug.DebugDelegate pDebugDelegate);

	// Token: 0x060005FB RID: 1531 RVA: 0x00032BB4 File Offset: 0x00030DB4
	public static void Initialize()
	{
		AgricolaDebug.SetDebugFunction(new AgricolaDebug.DebugDelegate(AgricolaDebug.DebugCallBackFunction));
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x00003022 File Offset: 0x00001222
	private static void DebugCallBackFunction(string str)
	{
	}

	// Token: 0x0400061C RID: 1564
	private const string DLL_NAME = "AgricolaLib";

	// Token: 0x02000786 RID: 1926
	// (Invoke) Token: 0x06004249 RID: 16969
	public delegate void DebugDelegate(string str);
}
