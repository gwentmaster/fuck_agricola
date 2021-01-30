using System;
using System.Collections;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000411 RID: 1041
	public class IsoTrailers
	{
		// Token: 0x060026AA RID: 9898 RVA: 0x000C2FD4 File Offset: 0x000C11D4
		private static IDictionary CreateTrailerMap()
		{
			IDictionary dictionary = Platform.CreateHashtable();
			dictionary.Add("RIPEMD128", 13004);
			dictionary.Add("RIPEMD160", 12748);
			dictionary.Add("SHA-1", 13260);
			dictionary.Add("SHA-224", 14540);
			dictionary.Add("SHA-256", 13516);
			dictionary.Add("SHA-384", 14028);
			dictionary.Add("SHA-512", 13772);
			dictionary.Add("SHA-512/224", 14796);
			dictionary.Add("SHA-512/256", 16588);
			dictionary.Add("Whirlpool", 14284);
			return CollectionUtilities.ReadOnly(dictionary);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000C30BD File Offset: 0x000C12BD
		public static int GetTrailer(IDigest digest)
		{
			return (int)IsoTrailers.trailerMap[digest.AlgorithmName];
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000C30D4 File Offset: 0x000C12D4
		public static bool NoTrailerAvailable(IDigest digest)
		{
			return !IsoTrailers.trailerMap.Contains(digest.AlgorithmName);
		}

		// Token: 0x040019D8 RID: 6616
		public const int TRAILER_IMPLICIT = 188;

		// Token: 0x040019D9 RID: 6617
		public const int TRAILER_RIPEMD160 = 12748;

		// Token: 0x040019DA RID: 6618
		public const int TRAILER_RIPEMD128 = 13004;

		// Token: 0x040019DB RID: 6619
		public const int TRAILER_SHA1 = 13260;

		// Token: 0x040019DC RID: 6620
		public const int TRAILER_SHA256 = 13516;

		// Token: 0x040019DD RID: 6621
		public const int TRAILER_SHA512 = 13772;

		// Token: 0x040019DE RID: 6622
		public const int TRAILER_SHA384 = 14028;

		// Token: 0x040019DF RID: 6623
		public const int TRAILER_WHIRLPOOL = 14284;

		// Token: 0x040019E0 RID: 6624
		public const int TRAILER_SHA224 = 14540;

		// Token: 0x040019E1 RID: 6625
		public const int TRAILER_SHA512_224 = 14796;

		// Token: 0x040019E2 RID: 6626
		public const int TRAILER_SHA512_256 = 16588;

		// Token: 0x040019E3 RID: 6627
		private static readonly IDictionary trailerMap = IsoTrailers.CreateTrailerMap();
	}
}
