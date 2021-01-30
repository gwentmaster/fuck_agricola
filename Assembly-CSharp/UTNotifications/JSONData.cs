using System;
using System.IO;

namespace UTNotifications
{
	// Token: 0x02000153 RID: 339
	public class JSONData : JSONNode
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00056D57 File Offset: 0x00054F57
		// (set) Token: 0x06000D3D RID: 3389 RVA: 0x00056D5F File Offset: 0x00054F5F
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

		// Token: 0x06000D3E RID: 3390 RVA: 0x00056D68 File Offset: 0x00054F68
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00056D77 File Offset: 0x00054F77
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00056D86 File Offset: 0x00054F86
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00056D95 File Offset: 0x00054F95
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00056DA4 File Offset: 0x00054FA4
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00056DB3 File Offset: 0x00054FB3
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00056DB3 File Offset: 0x00054FB3
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00056DD0 File Offset: 0x00054FD0
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

		// Token: 0x04000D50 RID: 3408
		private string m_Data;
	}
}
