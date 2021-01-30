using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000529 RID: 1321
	public class DistributionPointName : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600302F RID: 12335 RVA: 0x000F6E4C File Offset: 0x000F504C
		public static DistributionPointName GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DistributionPointName.GetInstance(Asn1TaggedObject.GetInstance(obj, true));
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000F6E5C File Offset: 0x000F505C
		public static DistributionPointName GetInstance(object obj)
		{
			if (obj == null || obj is DistributionPointName)
			{
				return (DistributionPointName)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new DistributionPointName((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000F6EA9 File Offset: 0x000F50A9
		public DistributionPointName(int type, Asn1Encodable name)
		{
			this.type = type;
			this.name = name;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000F6EBF File Offset: 0x000F50BF
		public DistributionPointName(GeneralNames name) : this(0, name)
		{
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x000F6EC9 File Offset: 0x000F50C9
		public int PointType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x000F6ED1 File Offset: 0x000F50D1
		public Asn1Encodable Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000F6ED9 File Offset: 0x000F50D9
		public DistributionPointName(Asn1TaggedObject obj)
		{
			this.type = obj.TagNo;
			if (this.type == 0)
			{
				this.name = GeneralNames.GetInstance(obj, false);
				return;
			}
			this.name = Asn1Set.GetInstance(obj, false);
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000F6F10 File Offset: 0x000F5110
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(false, this.type, this.name);
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000F6F24 File Offset: 0x000F5124
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DistributionPointName: [");
			stringBuilder.Append(newLine);
			if (this.type == 0)
			{
				this.appendObject(stringBuilder, newLine, "fullName", this.name.ToString());
			}
			else
			{
				this.appendObject(stringBuilder, newLine, "nameRelativeToCRLIssuer", this.name.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000F6FA8 File Offset: 0x000F51A8
		private void appendObject(StringBuilder buf, string sep, string name, string val)
		{
			string value = "    ";
			buf.Append(value);
			buf.Append(name);
			buf.Append(":");
			buf.Append(sep);
			buf.Append(value);
			buf.Append(value);
			buf.Append(val);
			buf.Append(sep);
		}

		// Token: 0x04001ECD RID: 7885
		internal readonly Asn1Encodable name;

		// Token: 0x04001ECE RID: 7886
		internal readonly int type;

		// Token: 0x04001ECF RID: 7887
		public const int FullName = 0;

		// Token: 0x04001ED0 RID: 7888
		public const int NameRelativeToCrlIssuer = 1;
	}
}
