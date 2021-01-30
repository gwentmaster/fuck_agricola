using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000537 RID: 1335
	public class X509Extension
	{
		// Token: 0x060030B0 RID: 12464 RVA: 0x000F8838 File Offset: 0x000F6A38
		public X509Extension(DerBoolean critical, Asn1OctetString value)
		{
			if (critical == null)
			{
				throw new ArgumentNullException("critical");
			}
			this.critical = critical.IsTrue;
			this.value = value;
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000F8861 File Offset: 0x000F6A61
		public X509Extension(bool critical, Asn1OctetString value)
		{
			this.critical = critical;
			this.value = value;
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000F8877 File Offset: 0x000F6A77
		public bool IsCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000F887F File Offset: 0x000F6A7F
		public Asn1OctetString Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x000F8887 File Offset: 0x000F6A87
		public Asn1Encodable GetParsedValue()
		{
			return X509Extension.ConvertValueToObject(this);
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x000F8890 File Offset: 0x000F6A90
		public override int GetHashCode()
		{
			int hashCode = this.Value.GetHashCode();
			if (!this.IsCritical)
			{
				return ~hashCode;
			}
			return hashCode;
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000F88B8 File Offset: 0x000F6AB8
		public override bool Equals(object obj)
		{
			X509Extension x509Extension = obj as X509Extension;
			return x509Extension != null && this.Value.Equals(x509Extension.Value) && this.IsCritical == x509Extension.IsCritical;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000F88F4 File Offset: 0x000F6AF4
		public static Asn1Object ConvertValueToObject(X509Extension ext)
		{
			Asn1Object result;
			try
			{
				result = Asn1Object.FromByteArray(ext.Value.GetOctets());
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("can't convert extension", innerException);
			}
			return result;
		}

		// Token: 0x04001F16 RID: 7958
		internal bool critical;

		// Token: 0x04001F17 RID: 7959
		internal Asn1OctetString value;
	}
}
