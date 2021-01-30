using System;
using System.IO;

namespace com.adjust.sdk
{
	// Token: 0x0200073C RID: 1852
	public class JSONData : JSONNode
	{
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0013A653 File Offset: 0x00138853
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x0013A65B File Offset: 0x0013885B
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x0013A664 File Offset: 0x00138864
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x0013A673 File Offset: 0x00138873
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x0013A682 File Offset: 0x00138882
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0013A691 File Offset: 0x00138891
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x0013A6A0 File Offset: 0x001388A0
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x0013A6AF File Offset: 0x001388AF
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x0013A6AF File Offset: 0x001388AF
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0013A6CC File Offset: 0x001388CC
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData("");
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x040029D6 RID: 10710
		private string m_Data;
	}
}
