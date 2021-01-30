using System;
using System.IO;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CF RID: 1231
	public abstract class Asn1OctetString : Asn1Object, Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06002DB0 RID: 11696 RVA: 0x000EFFBC File Offset: 0x000EE1BC
		public static Asn1OctetString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is Asn1OctetString)
			{
				return Asn1OctetString.GetInstance(@object);
			}
			return BerOctetString.FromSequence(Asn1Sequence.GetInstance(@object));
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000EFFF0 File Offset: 0x000EE1F0
		public static Asn1OctetString GetInstance(object obj)
		{
			if (obj == null || obj is Asn1OctetString)
			{
				return (Asn1OctetString)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return Asn1OctetString.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000F003D File Offset: 0x000EE23D
		internal Asn1OctetString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000F005C File Offset: 0x000EE25C
		internal Asn1OctetString(Asn1Encodable obj)
		{
			try
			{
				this.str = obj.GetEncoded("DER");
			}
			catch (IOException ex)
			{
				throw new ArgumentException("Error processing object : " + ex.ToString());
			}
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000F00AC File Offset: 0x000EE2AC
		public Stream GetOctetStream()
		{
			return new MemoryStream(this.str, false);
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x00035D67 File Offset: 0x00033F67
		public Asn1OctetStringParser Parser
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000F00BA File Offset: 0x000EE2BA
		public virtual byte[] GetOctets()
		{
			return this.str;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000F00C2 File Offset: 0x000EE2C2
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.GetOctets());
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000F00D0 File Offset: 0x000EE2D0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerOctetString derOctetString = asn1Object as DerOctetString;
			return derOctetString != null && Arrays.AreEqual(this.GetOctets(), derOctetString.GetOctets());
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000F00FA File Offset: 0x000EE2FA
		public override string ToString()
		{
			return "#" + Hex.ToHexString(this.str);
		}

		// Token: 0x04001DE6 RID: 7654
		internal byte[] str;
	}
}
