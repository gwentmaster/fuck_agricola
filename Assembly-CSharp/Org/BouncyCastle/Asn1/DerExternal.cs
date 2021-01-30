using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E6 RID: 1254
	public class DerExternal : Asn1Object
	{
		// Token: 0x06002E35 RID: 11829 RVA: 0x000F14C4 File Offset: 0x000EF6C4
		public DerExternal(Asn1EncodableVector vector)
		{
			int num = 0;
			Asn1Object objFromVector = DerExternal.GetObjFromVector(vector, num);
			if (objFromVector is DerObjectIdentifier)
			{
				this.directReference = (DerObjectIdentifier)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (objFromVector is DerInteger)
			{
				this.indirectReference = (DerInteger)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				this.dataValueDescriptor = objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (vector.Count != num + 1)
			{
				throw new ArgumentException("input vector too large", "vector");
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				throw new ArgumentException("No tagged object found in vector. Structure doesn't seem to be of type External", "vector");
			}
			Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)objFromVector;
			this.Encoding = asn1TaggedObject.TagNo;
			if (this.encoding < 0 || this.encoding > 2)
			{
				throw new InvalidOperationException("invalid encoding value");
			}
			this.externalContent = asn1TaggedObject.GetObject();
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000F15AB File Offset: 0x000EF7AB
		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, DerTaggedObject externalData) : this(directReference, indirectReference, dataValueDescriptor, externalData.TagNo, externalData.ToAsn1Object())
		{
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000F15C4 File Offset: 0x000EF7C4
		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, int encoding, Asn1Object externalData)
		{
			this.DirectReference = directReference;
			this.IndirectReference = indirectReference;
			this.DataValueDescriptor = dataValueDescriptor;
			this.Encoding = encoding;
			this.ExternalContent = externalData.ToAsn1Object();
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000F15F8 File Offset: 0x000EF7F8
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerExternal.WriteEncodable(memoryStream, this.directReference);
			DerExternal.WriteEncodable(memoryStream, this.indirectReference);
			DerExternal.WriteEncodable(memoryStream, this.dataValueDescriptor);
			DerExternal.WriteEncodable(memoryStream, new DerTaggedObject(8, this.externalContent));
			derOut.WriteEncoded(32, 8, memoryStream.ToArray());
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000F1650 File Offset: 0x000EF850
		protected override int Asn1GetHashCode()
		{
			int num = this.externalContent.GetHashCode();
			if (this.directReference != null)
			{
				num ^= this.directReference.GetHashCode();
			}
			if (this.indirectReference != null)
			{
				num ^= this.indirectReference.GetHashCode();
			}
			if (this.dataValueDescriptor != null)
			{
				num ^= this.dataValueDescriptor.GetHashCode();
			}
			return num;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000F16AC File Offset: 0x000EF8AC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (this == asn1Object)
			{
				return true;
			}
			DerExternal derExternal = asn1Object as DerExternal;
			return derExternal != null && (object.Equals(this.directReference, derExternal.directReference) && object.Equals(this.indirectReference, derExternal.indirectReference) && object.Equals(this.dataValueDescriptor, derExternal.dataValueDescriptor)) && this.externalContent.Equals(derExternal.externalContent);
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000F1717 File Offset: 0x000EF917
		// (set) Token: 0x06002E3C RID: 11836 RVA: 0x000F171F File Offset: 0x000EF91F
		public Asn1Object DataValueDescriptor
		{
			get
			{
				return this.dataValueDescriptor;
			}
			set
			{
				this.dataValueDescriptor = value;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x000F1728 File Offset: 0x000EF928
		// (set) Token: 0x06002E3E RID: 11838 RVA: 0x000F1730 File Offset: 0x000EF930
		public DerObjectIdentifier DirectReference
		{
			get
			{
				return this.directReference;
			}
			set
			{
				this.directReference = value;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x000F1739 File Offset: 0x000EF939
		// (set) Token: 0x06002E40 RID: 11840 RVA: 0x000F1741 File Offset: 0x000EF941
		public int Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (this.encoding < 0 || this.encoding > 2)
				{
					throw new InvalidOperationException("invalid encoding value: " + this.encoding);
				}
				this.encoding = value;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x000F1777 File Offset: 0x000EF977
		// (set) Token: 0x06002E42 RID: 11842 RVA: 0x000F177F File Offset: 0x000EF97F
		public Asn1Object ExternalContent
		{
			get
			{
				return this.externalContent;
			}
			set
			{
				this.externalContent = value;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x000F1788 File Offset: 0x000EF988
		// (set) Token: 0x06002E44 RID: 11844 RVA: 0x000F1790 File Offset: 0x000EF990
		public DerInteger IndirectReference
		{
			get
			{
				return this.indirectReference;
			}
			set
			{
				this.indirectReference = value;
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000F1799 File Offset: 0x000EF999
		private static Asn1Object GetObjFromVector(Asn1EncodableVector v, int index)
		{
			if (v.Count <= index)
			{
				throw new ArgumentException("too few objects in input vector", "v");
			}
			return v[index].ToAsn1Object();
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000F17C0 File Offset: 0x000EF9C0
		private static void WriteEncodable(MemoryStream ms, Asn1Encodable e)
		{
			if (e != null)
			{
				byte[] derEncoded = e.GetDerEncoded();
				ms.Write(derEncoded, 0, derEncoded.Length);
			}
		}

		// Token: 0x04001E1A RID: 7706
		private DerObjectIdentifier directReference;

		// Token: 0x04001E1B RID: 7707
		private DerInteger indirectReference;

		// Token: 0x04001E1C RID: 7708
		private Asn1Object dataValueDescriptor;

		// Token: 0x04001E1D RID: 7709
		private int encoding;

		// Token: 0x04001E1E RID: 7710
		private Asn1Object externalContent;
	}
}
