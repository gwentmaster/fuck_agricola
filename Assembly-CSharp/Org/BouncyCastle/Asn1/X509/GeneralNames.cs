using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052B RID: 1323
	public class GeneralNames : Asn1Encodable
	{
		// Token: 0x06003049 RID: 12361 RVA: 0x000F7658 File Offset: 0x000F5858
		public static GeneralNames GetInstance(object obj)
		{
			if (obj == null || obj is GeneralNames)
			{
				return (GeneralNames)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new GeneralNames((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000F76A5 File Offset: 0x000F58A5
		public static GeneralNames GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GeneralNames.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000F76B3 File Offset: 0x000F58B3
		public GeneralNames(GeneralName name)
		{
			this.names = new GeneralName[]
			{
				name
			};
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000F76CB File Offset: 0x000F58CB
		public GeneralNames(GeneralName[] names)
		{
			this.names = (GeneralName[])names.Clone();
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000F76E4 File Offset: 0x000F58E4
		private GeneralNames(Asn1Sequence seq)
		{
			this.names = new GeneralName[seq.Count];
			for (int num = 0; num != seq.Count; num++)
			{
				this.names[num] = GeneralName.GetInstance(seq[num]);
			}
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000F772D File Offset: 0x000F592D
		public GeneralName[] GetNames()
		{
			return (GeneralName[])this.names.Clone();
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000F7740 File Offset: 0x000F5940
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.names;
			return new DerSequence(v);
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000F775C File Offset: 0x000F595C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("GeneralNames:");
			stringBuilder.Append(newLine);
			foreach (GeneralName value in this.names)
			{
				stringBuilder.Append("    ");
				stringBuilder.Append(value);
				stringBuilder.Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001EDC RID: 7900
		private readonly GeneralName[] names;
	}
}
