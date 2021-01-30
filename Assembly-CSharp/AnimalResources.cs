using System;

// Token: 0x02000026 RID: 38
public struct AnimalResources
{
	// Token: 0x06000194 RID: 404 RVA: 0x0000898F File Offset: 0x00006B8F
	public static bool operator ==(AnimalResources resCache, AnimalResources thisCache)
	{
		return resCache.sheep == thisCache.sheep && resCache.boar == thisCache.boar && resCache.cattle == thisCache.cattle;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x000089BD File Offset: 0x00006BBD
	public static bool operator !=(AnimalResources resCache, AnimalResources thisCache)
	{
		return !(resCache == thisCache);
	}

	// Token: 0x040000CC RID: 204
	public byte sheep;

	// Token: 0x040000CD RID: 205
	public byte boar;

	// Token: 0x040000CE RID: 206
	public byte cattle;
}
