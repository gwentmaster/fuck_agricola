using System;
using System.Collections;
using System.IO;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x020002A4 RID: 676
	public class PemReader
	{
		// Token: 0x0600166B RID: 5739 RVA: 0x00080988 File Offset: 0x0007EB88
		public PemReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.reader = reader;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x000809A5 File Offset: 0x0007EBA5
		public TextReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000809B0 File Offset: 0x0007EBB0
		public PemObject ReadPemObject()
		{
			string text = this.reader.ReadLine();
			if (text != null && Platform.StartsWith(text, "-----BEGIN "))
			{
				text = text.Substring("-----BEGIN ".Length);
				int num = text.IndexOf('-');
				string type = text.Substring(0, num);
				if (num > 0)
				{
					return this.LoadObject(type);
				}
			}
			return null;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00080A0C File Offset: 0x0007EC0C
		private PemObject LoadObject(string type)
		{
			string text = "-----END " + type;
			IList list = Platform.CreateArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			string text2;
			while ((text2 = this.reader.ReadLine()) != null && Platform.IndexOf(text2, text) == -1)
			{
				int num = text2.IndexOf(':');
				if (num == -1)
				{
					stringBuilder.Append(text2.Trim());
				}
				else
				{
					string text3 = text2.Substring(0, num).Trim();
					if (Platform.StartsWith(text3, "X-"))
					{
						text3 = text3.Substring(2);
					}
					string val = text2.Substring(num + 1).Trim();
					list.Add(new PemHeader(text3, val));
				}
			}
			if (text2 == null)
			{
				throw new IOException(text + " not found");
			}
			if (stringBuilder.Length % 4 != 0)
			{
				throw new IOException("base64 data appears to be truncated");
			}
			return new PemObject(type, list, Base64.Decode(stringBuilder.ToString()));
		}

		// Token: 0x0400150C RID: 5388
		private const string BeginString = "-----BEGIN ";

		// Token: 0x0400150D RID: 5389
		private const string EndString = "-----END ";

		// Token: 0x0400150E RID: 5390
		private readonly TextReader reader;
	}
}
