using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E0 RID: 1248
	public class BerOctetString : DerOctetString, IEnumerable
	{
		// Token: 0x06002E12 RID: 11794 RVA: 0x000F0DE8 File Offset: 0x000EEFE8
		public static BerOctetString FromSequence(Asn1Sequence seq)
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Encodable value = (Asn1Encodable)obj;
				list.Add(value);
			}
			return new BerOctetString(list);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000F0E4C File Offset: 0x000EF04C
		private static byte[] ToBytes(IEnumerable octs)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in octs)
			{
				byte[] octets = ((DerOctetString)obj).GetOctets();
				memoryStream.Write(octets, 0, octets.Length);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000F0EB8 File Offset: 0x000EF0B8
		public BerOctetString(byte[] str) : base(str)
		{
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000F0EC1 File Offset: 0x000EF0C1
		public BerOctetString(IEnumerable octets) : base(BerOctetString.ToBytes(octets))
		{
			this.octs = octets;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000F0ED6 File Offset: 0x000EF0D6
		public BerOctetString(Asn1Object obj) : base(obj)
		{
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000F0EDF File Offset: 0x000EF0DF
		public BerOctetString(Asn1Encodable obj) : base(obj.ToAsn1Object())
		{
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000F00BA File Offset: 0x000EE2BA
		public override byte[] GetOctets()
		{
			return this.str;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000F0EED File Offset: 0x000EF0ED
		public IEnumerator GetEnumerator()
		{
			if (this.octs == null)
			{
				return this.GenerateOcts().GetEnumerator();
			}
			return this.octs.GetEnumerator();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000F0F0E File Offset: 0x000EF10E
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000F0F18 File Offset: 0x000EF118
		private IList GenerateOcts()
		{
			IList list = Platform.CreateArrayList();
			for (int i = 0; i < this.str.Length; i += 1000)
			{
				byte[] array = new byte[Math.Min(this.str.Length, i + 1000) - i];
				Array.Copy(this.str, i, array, 0, array.Length);
				list.Add(new DerOctetString(array));
			}
			return list;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000F0F80 File Offset: 0x000EF180
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(36);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					DerOctetString obj2 = (DerOctetString)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x04001E13 RID: 7699
		private const int MaxLength = 1000;

		// Token: 0x04001E14 RID: 7700
		private readonly IEnumerable octs;
	}
}
