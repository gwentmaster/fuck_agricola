using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FC RID: 1276
	public class DerOutputStream : FilterStream
	{
		// Token: 0x06002F02 RID: 12034 RVA: 0x000F38CE File Offset: 0x000F1ACE
		public DerOutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000F38D8 File Offset: 0x000F1AD8
		private void WriteLength(int length)
		{
			if (length > 127)
			{
				int num = 1;
				uint num2 = (uint)length;
				while ((num2 >>= 8) != 0U)
				{
					num++;
				}
				this.WriteByte((byte)(num | 128));
				for (int i = (num - 1) * 8; i >= 0; i -= 8)
				{
					this.WriteByte((byte)(length >> i));
				}
				return;
			}
			this.WriteByte((byte)length);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000F392F File Offset: 0x000F1B2F
		internal void WriteEncoded(int tag, byte[] bytes)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(bytes.Length);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000F394D File Offset: 0x000F1B4D
		internal void WriteEncoded(int tag, byte first, byte[] bytes)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(bytes.Length + 1);
			this.WriteByte(first);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000F3974 File Offset: 0x000F1B74
		internal void WriteEncoded(int tag, byte[] bytes, int offset, int length)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(length);
			this.Write(bytes, offset, length);
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000F3990 File Offset: 0x000F1B90
		internal void WriteTag(int flags, int tagNo)
		{
			if (tagNo < 31)
			{
				this.WriteByte((byte)(flags | tagNo));
				return;
			}
			this.WriteByte((byte)(flags | 31));
			if (tagNo < 128)
			{
				this.WriteByte((byte)tagNo);
				return;
			}
			byte[] array = new byte[5];
			int num = array.Length;
			array[--num] = (byte)(tagNo & 127);
			do
			{
				tagNo >>= 7;
				array[--num] = (byte)((tagNo & 127) | 128);
			}
			while (tagNo > 127);
			this.Write(array, num, array.Length - num);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000F3A09 File Offset: 0x000F1C09
		internal void WriteEncoded(int flags, int tagNo, byte[] bytes)
		{
			this.WriteTag(flags, tagNo);
			this.WriteLength(bytes.Length);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000F3A27 File Offset: 0x000F1C27
		protected void WriteNull()
		{
			this.WriteByte(5);
			this.WriteByte(0);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000F3A38 File Offset: 0x000F1C38
		[Obsolete("Use version taking an Asn1Encodable arg instead")]
		public virtual void WriteObject(object obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			if (obj is Asn1Object)
			{
				((Asn1Object)obj).Encode(this);
				return;
			}
			if (obj is Asn1Encodable)
			{
				((Asn1Encodable)obj).ToAsn1Object().Encode(this);
				return;
			}
			throw new IOException("object not Asn1Object");
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000F3A88 File Offset: 0x000F1C88
		public virtual void WriteObject(Asn1Encodable obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			obj.ToAsn1Object().Encode(this);
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000F3AA0 File Offset: 0x000F1CA0
		public virtual void WriteObject(Asn1Object obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			obj.Encode(this);
		}
	}
}
