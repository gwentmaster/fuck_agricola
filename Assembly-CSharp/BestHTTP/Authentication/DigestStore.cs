using System;
using System.Collections.Generic;

namespace BestHTTP.Authentication
{
	// Token: 0x0200061B RID: 1563
	internal static class DigestStore
	{
		// Token: 0x0600398C RID: 14732 RVA: 0x0011E3E4 File Offset: 0x0011C5E4
		internal static Digest Get(Uri uri)
		{
			object locker = DigestStore.Locker;
			Digest result;
			lock (locker)
			{
				Digest digest = null;
				if (DigestStore.Digests.TryGetValue(uri.Host, out digest) && !digest.IsUriProtected(uri))
				{
					result = null;
				}
				else
				{
					result = digest;
				}
			}
			return result;
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0011E444 File Offset: 0x0011C644
		public static Digest GetOrCreate(Uri uri)
		{
			object locker = DigestStore.Locker;
			Digest result;
			lock (locker)
			{
				Digest digest = null;
				if (!DigestStore.Digests.TryGetValue(uri.Host, out digest))
				{
					DigestStore.Digests.Add(uri.Host, digest = new Digest(uri));
				}
				result = digest;
			}
			return result;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x0011E4B0 File Offset: 0x0011C6B0
		public static void Remove(Uri uri)
		{
			object locker = DigestStore.Locker;
			lock (locker)
			{
				DigestStore.Digests.Remove(uri.Host);
			}
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0011E4FC File Offset: 0x0011C6FC
		public static string FindBest(List<string> authHeaders)
		{
			if (authHeaders == null || authHeaders.Count == 0)
			{
				return string.Empty;
			}
			List<string> list = new List<string>(authHeaders.Count);
			for (int j = 0; j < authHeaders.Count; j++)
			{
				list.Add(authHeaders[j].ToLower());
			}
			int i;
			int i2;
			for (i = 0; i < DigestStore.SupportedAlgorithms.Length; i = i2)
			{
				int num = list.FindIndex((string header) => header.StartsWith(DigestStore.SupportedAlgorithms[i]));
				if (num != -1)
				{
					return authHeaders[num];
				}
				i2 = i + 1;
			}
			return string.Empty;
		}

		// Token: 0x04002537 RID: 9527
		private static Dictionary<string, Digest> Digests = new Dictionary<string, Digest>();

		// Token: 0x04002538 RID: 9528
		private static object Locker = new object();

		// Token: 0x04002539 RID: 9529
		private static string[] SupportedAlgorithms = new string[]
		{
			"digest",
			"basic"
		};
	}
}
