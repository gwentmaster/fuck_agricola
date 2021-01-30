using System;
using System.Linq;
using System.Text;

// Token: 0x0200001C RID: 28
public class CSVReader
{
	// Token: 0x0600013F RID: 319 RVA: 0x000070B2 File Offset: 0x000052B2
	public CSVReader(byte[] csv_bytes)
	{
		this.m_CSVBytes = csv_bytes;
		this.m_ParseIndex = 0;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000070C8 File Offset: 0x000052C8
	public string[] ParseNextEntry()
	{
		string[] array = null;
		while (this.m_ParseIndex < this.m_CSVBytes.Length)
		{
			array = CSVReader.ParseCsvLine(this.m_CSVBytes, ref this.m_ParseIndex);
			if (array != null && array.Length != 0)
			{
				return array;
			}
		}
		return array;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00007108 File Offset: 0x00005308
	private static string[] ParseCsvLine(byte[] csv_bytes, ref int parseIndex)
	{
		string[] array = new string[16];
		int num = 0;
		string newLine = Environment.NewLine;
		char c = newLine.ElementAt(0);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		while (parseIndex < csv_bytes.Length)
		{
			char c2 = (char)csv_bytes[parseIndex];
			if (c2 == c && !flag)
			{
				bool flag2 = true;
				for (int i = 1; i < newLine.Length; i++)
				{
					if ((char)csv_bytes[parseIndex + i] != newLine.ElementAt(i))
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					parseIndex += newLine.Length;
					break;
				}
			}
			parseIndex++;
			if (c2 == ',' && !flag)
			{
				if (num < 16)
				{
					array[num++] = stringBuilder.ToString();
				}
				stringBuilder.Remove(0, stringBuilder.Length);
			}
			else
			{
				if (c2 == '"')
				{
					if (csv_bytes[parseIndex] != 34)
					{
						flag = !flag;
						continue;
					}
					parseIndex++;
				}
				stringBuilder.Append(c2);
			}
		}
		if (num < 16)
		{
			array[num++] = stringBuilder.ToString();
		}
		return array;
	}

	// Token: 0x04000098 RID: 152
	private byte[] m_CSVBytes;

	// Token: 0x04000099 RID: 153
	private int m_ParseIndex;
}
