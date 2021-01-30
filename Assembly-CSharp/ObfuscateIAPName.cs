using System;

// Token: 0x020000D6 RID: 214
public class ObfuscateIAPName
{
	// Token: 0x060007EC RID: 2028 RVA: 0x000385C8 File Offset: 0x000367C8
	public static string CreateHashString(string text)
	{
		ulong num = 5381UL;
		int i = 0;
		while (i < text.Length)
		{
			ulong num2 = (ulong)text[i++];
			num = (num << 5) + num + num2;
		}
		return num.ToString("X16");
	}
}
