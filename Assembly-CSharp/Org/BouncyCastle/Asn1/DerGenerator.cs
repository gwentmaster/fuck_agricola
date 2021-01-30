using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E8 RID: 1256
	public abstract class DerGenerator : Asn1Generator
	{
		// Token: 0x06002E4A RID: 11850 RVA: 0x000F0AAC File Offset: 0x000EECAC
		protected DerGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000F1810 File Offset: 0x000EFA10
		protected DerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
		{
			this._tagged = true;
			this._isExplicit = isExplicit;
			this._tagNo = tagNo;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000F1830 File Offset: 0x000EFA30
		private static void WriteLength(Stream outStr, int length)
		{
			if (length > 127)
			{
				int num = 1;
				int num2 = length;
				while ((num2 >>= 8) != 0)
				{
					num++;
				}
				outStr.WriteByte((byte)(num | 128));
				for (int i = (num - 1) * 8; i >= 0; i -= 8)
				{
					outStr.WriteByte((byte)(length >> i));
				}
				return;
			}
			outStr.WriteByte((byte)length);
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000F1887 File Offset: 0x000EFA87
		internal static void WriteDerEncoded(Stream outStream, int tag, byte[] bytes)
		{
			outStream.WriteByte((byte)tag);
			DerGenerator.WriteLength(outStream, bytes.Length);
			outStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000F18A8 File Offset: 0x000EFAA8
		internal void WriteDerEncoded(int tag, byte[] bytes)
		{
			if (!this._tagged)
			{
				DerGenerator.WriteDerEncoded(base.Out, tag, bytes);
				return;
			}
			int num = this._tagNo | 128;
			if (this._isExplicit)
			{
				int tag2 = this._tagNo | 32 | 128;
				MemoryStream memoryStream = new MemoryStream();
				DerGenerator.WriteDerEncoded(memoryStream, tag, bytes);
				DerGenerator.WriteDerEncoded(base.Out, tag2, memoryStream.ToArray());
				return;
			}
			if ((tag & 32) != 0)
			{
				num |= 32;
			}
			DerGenerator.WriteDerEncoded(base.Out, num, bytes);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000F1929 File Offset: 0x000EFB29
		internal static void WriteDerEncoded(Stream outStr, int tag, Stream inStr)
		{
			DerGenerator.WriteDerEncoded(outStr, tag, Streams.ReadAll(inStr));
		}

		// Token: 0x04001E20 RID: 7712
		private bool _tagged;

		// Token: 0x04001E21 RID: 7713
		private bool _isExplicit;

		// Token: 0x04001E22 RID: 7714
		private int _tagNo;
	}
}
