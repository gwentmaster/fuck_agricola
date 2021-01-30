using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.X509
{
	// Token: 0x0200027A RID: 634
	internal class PemParser
	{
		// Token: 0x060014B8 RID: 5304 RVA: 0x00076FAC File Offset: 0x000751AC
		internal PemParser(string type)
		{
			this._header1 = "-----BEGIN " + type + "-----";
			this._header2 = "-----BEGIN X509 " + type + "-----";
			this._footer1 = "-----END " + type + "-----";
			this._footer2 = "-----END X509 " + type + "-----";
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00077018 File Offset: 0x00075218
		private string ReadLine(Stream inStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (;;)
			{
				if ((num = inStream.ReadByte()) == 13 || num == 10 || num < 0)
				{
					if (num < 0 || stringBuilder.Length != 0)
					{
						break;
					}
				}
				else if (num != 13)
				{
					stringBuilder.Append((char)num);
				}
			}
			if (num < 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00077068 File Offset: 0x00075268
		internal Asn1Sequence ReadPemObject(Stream inStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text;
			while ((text = this.ReadLine(inStream)) != null)
			{
				if (Platform.StartsWith(text, this._header1) || Platform.StartsWith(text, this._header2))
				{
					IL_55:
					while ((text = this.ReadLine(inStream)) != null && !Platform.StartsWith(text, this._footer1) && !Platform.StartsWith(text, this._footer2))
					{
						stringBuilder.Append(text);
					}
					if (stringBuilder.Length == 0)
					{
						return null;
					}
					Asn1Object asn1Object = Asn1Object.FromByteArray(Base64.Decode(stringBuilder.ToString()));
					if (!(asn1Object is Asn1Sequence))
					{
						throw new IOException("malformed PEM data encountered");
					}
					return (Asn1Sequence)asn1Object;
				}
			}
			goto IL_55;
		}

		// Token: 0x04001379 RID: 4985
		private readonly string _header1;

		// Token: 0x0400137A RID: 4986
		private readonly string _header2;

		// Token: 0x0400137B RID: 4987
		private readonly string _footer1;

		// Token: 0x0400137C RID: 4988
		private readonly string _footer2;
	}
}
