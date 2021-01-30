using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F2 RID: 1266
	public class DerEnumerated : Asn1Object
	{
		// Token: 0x06002E97 RID: 11927 RVA: 0x000F26F4 File Offset: 0x000F08F4
		public static DerEnumerated GetInstance(object obj)
		{
			if (obj == null || obj is DerEnumerated)
			{
				return (DerEnumerated)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000F2720 File Offset: 0x000F0920
		public static DerEnumerated GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerEnumerated)
			{
				return DerEnumerated.GetInstance(@object);
			}
			return DerEnumerated.FromOctetString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000F2756 File Offset: 0x000F0956
		public DerEnumerated(int val)
		{
			this.bytes = BigInteger.ValueOf((long)val).ToByteArray();
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000F2770 File Offset: 0x000F0970
		public DerEnumerated(BigInteger val)
		{
			this.bytes = val.ToByteArray();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000F2784 File Offset: 0x000F0984
		public DerEnumerated(byte[] bytes)
		{
			this.bytes = bytes;
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x000F2793 File Offset: 0x000F0993
		public BigInteger Value
		{
			get
			{
				return new BigInteger(this.bytes);
			}
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000F27A0 File Offset: 0x000F09A0
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(10, this.bytes);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000F27B0 File Offset: 0x000F09B0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerEnumerated derEnumerated = asn1Object as DerEnumerated;
			return derEnumerated != null && Arrays.AreEqual(this.bytes, derEnumerated.bytes);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000F27DA File Offset: 0x000F09DA
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.bytes);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000F27E8 File Offset: 0x000F09E8
		internal static DerEnumerated FromOctetString(byte[] enc)
		{
			if (enc.Length == 0)
			{
				throw new ArgumentException("ENUMERATED has zero length", "enc");
			}
			if (enc.Length == 1)
			{
				int num = (int)enc[0];
				if (num < DerEnumerated.cache.Length)
				{
					DerEnumerated derEnumerated = DerEnumerated.cache[num];
					if (derEnumerated != null)
					{
						return derEnumerated;
					}
					return DerEnumerated.cache[num] = new DerEnumerated(Arrays.Clone(enc));
				}
			}
			return new DerEnumerated(Arrays.Clone(enc));
		}

		// Token: 0x04001E34 RID: 7732
		private readonly byte[] bytes;

		// Token: 0x04001E35 RID: 7733
		private static readonly DerEnumerated[] cache = new DerEnumerated[12];
	}
}
