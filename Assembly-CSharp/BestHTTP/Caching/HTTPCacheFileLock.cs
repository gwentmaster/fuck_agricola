using System;
using System.Collections.Generic;

namespace BestHTTP.Caching
{
	// Token: 0x02000614 RID: 1556
	internal sealed class HTTPCacheFileLock
	{
		// Token: 0x06003941 RID: 14657 RVA: 0x0011CDDC File Offset: 0x0011AFDC
		internal static object Acquire(Uri uri)
		{
			object syncRoot = HTTPCacheFileLock.SyncRoot;
			object result;
			lock (syncRoot)
			{
				object obj;
				if (!HTTPCacheFileLock.FileLocks.TryGetValue(uri, out obj))
				{
					HTTPCacheFileLock.FileLocks.Add(uri, obj = new object());
				}
				result = obj;
			}
			return result;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x0011CE3C File Offset: 0x0011B03C
		internal static void Remove(Uri uri)
		{
			object syncRoot = HTTPCacheFileLock.SyncRoot;
			lock (syncRoot)
			{
				if (HTTPCacheFileLock.FileLocks.ContainsKey(uri))
				{
					HTTPCacheFileLock.FileLocks.Remove(uri);
				}
			}
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0011CE90 File Offset: 0x0011B090
		internal static void Clear()
		{
			object syncRoot = HTTPCacheFileLock.SyncRoot;
			lock (syncRoot)
			{
				HTTPCacheFileLock.FileLocks.Clear();
			}
		}

		// Token: 0x04002517 RID: 9495
		private static Dictionary<Uri, object> FileLocks = new Dictionary<Uri, object>();

		// Token: 0x04002518 RID: 9496
		private static object SyncRoot = new object();
	}
}
