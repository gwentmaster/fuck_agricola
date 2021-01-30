using System;

// Token: 0x02000045 RID: 69
public class CResourceCache
{
	// Token: 0x060003E6 RID: 998 RVA: 0x0001B91D File Offset: 0x00019B1D
	public CResourceCache()
	{
		this.m_ResourceCount = new int[10];
		this.Clear();
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001B938 File Offset: 0x00019B38
	public CResourceCache(int[] values)
	{
		this.m_ResourceCount = new int[10];
		this.SetArray(values);
	}

	// Token: 0x1700001C RID: 28
	public int this[int key]
	{
		get
		{
			return this.m_ResourceCount[key];
		}
		set
		{
			this.m_ResourceCount[key] = value;
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001B96C File Offset: 0x00019B6C
	public void SetArray(int[] values)
	{
		int num = 0;
		while (num < 10 && num < values.Length)
		{
			this.m_ResourceCount[num] = values[num];
			num++;
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001B998 File Offset: 0x00019B98
	public static bool operator ==(CResourceCache resCache, CResourceCache thisCache)
	{
		for (int i = 0; i < 10; i++)
		{
			if (thisCache[i] != resCache[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001B9C5 File Offset: 0x00019BC5
	public static bool operator !=(CResourceCache resCache, CResourceCache thisCache)
	{
		return !(thisCache == resCache);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001B9D4 File Offset: 0x00019BD4
	public static bool operator >(CResourceCache resCache, CResourceCache thisCache)
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			num |= thisCache[i] - resCache[i];
		}
		return num > 0;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001BA08 File Offset: 0x00019C08
	public static bool operator >=(CResourceCache resCache, CResourceCache thisCache)
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			num |= thisCache[i] - resCache[i];
		}
		return num >= 0;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001BA40 File Offset: 0x00019C40
	public static bool operator <(CResourceCache resCache, CResourceCache thisCache)
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			num |= thisCache[i] - resCache[i];
		}
		return num < 0;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001BA74 File Offset: 0x00019C74
	public static bool operator <=(CResourceCache resCache, CResourceCache thisCache)
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			num |= thisCache[i] - resCache[i];
		}
		return num <= 0;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001BAAC File Offset: 0x00019CAC
	public static CResourceCache operator +(CResourceCache resCache, CResourceCache thisCache)
	{
		CResourceCache cresourceCache = new CResourceCache();
		for (int i = 0; i < 10; i++)
		{
			cresourceCache[i] = thisCache[i] + resCache[i];
		}
		return cresourceCache;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001BAE4 File Offset: 0x00019CE4
	public static CResourceCache operator -(CResourceCache resCache, CResourceCache thisCache)
	{
		CResourceCache cresourceCache = new CResourceCache();
		for (int i = 0; i < 10; i++)
		{
			cresourceCache[i] = thisCache[i] - resCache[i];
		}
		return cresourceCache;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001BB1C File Offset: 0x00019D1C
	public static CResourceCache operator *(CResourceCache resCache, int multiplier)
	{
		CResourceCache cresourceCache = new CResourceCache();
		for (int i = 0; i < 10; i++)
		{
			cresourceCache[i] = resCache[i] * multiplier;
		}
		return cresourceCache;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001BB50 File Offset: 0x00019D50
	public void Clear()
	{
		for (int i = 0; i < 10; i++)
		{
			this.m_ResourceCount[i] = 0;
		}
	}

	// Token: 0x04000331 RID: 817
	private int[] m_ResourceCount;
}
