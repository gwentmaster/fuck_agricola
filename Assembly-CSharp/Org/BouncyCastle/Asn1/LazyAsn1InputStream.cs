using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x0200050D RID: 1293
	public class LazyAsn1InputStream : Asn1InputStream
	{
		// Token: 0x06002F70 RID: 12144 RVA: 0x000F4927 File Offset: 0x000F2B27
		public LazyAsn1InputStream(byte[] input) : base(input)
		{
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000F4930 File Offset: 0x000F2B30
		public LazyAsn1InputStream(Stream inputStream) : base(inputStream)
		{
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000F4939 File Offset: 0x000F2B39
		internal override DerSequence CreateDerSequence(DefiniteLengthInputStream dIn)
		{
			return new LazyDerSequence(dIn.ToArray());
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000F4946 File Offset: 0x000F2B46
		internal override DerSet CreateDerSet(DefiniteLengthInputStream dIn)
		{
			return new LazyDerSet(dIn.ToArray());
		}
	}
}
