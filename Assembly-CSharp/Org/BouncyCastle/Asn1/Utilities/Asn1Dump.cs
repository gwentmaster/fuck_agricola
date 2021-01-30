using System;
using System.Collections;
using System.IO;
using System.Text;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.Utilities
{
	// Token: 0x0200053D RID: 1341
	public sealed class Asn1Dump
	{
		// Token: 0x060030F5 RID: 12533 RVA: 0x00003425 File Offset: 0x00001625
		private Asn1Dump()
		{
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000FA9AC File Offset: 0x000F8BAC
		private static void AsString(string indent, bool verbose, Asn1Object obj, StringBuilder buf)
		{
			if (obj is Asn1Sequence)
			{
				string text = indent + "    ";
				buf.Append(indent);
				if (obj is BerSequence)
				{
					buf.Append("BER Sequence");
				}
				else if (obj is DerSequence)
				{
					buf.Append("DER Sequence");
				}
				else
				{
					buf.Append("Sequence");
				}
				buf.Append(Asn1Dump.NewLine);
				using (IEnumerator enumerator = ((Asn1Sequence)obj).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Asn1Encodable asn1Encodable = (Asn1Encodable)obj2;
						if (asn1Encodable == null || asn1Encodable is Asn1Null)
						{
							buf.Append(text);
							buf.Append("NULL");
							buf.Append(Asn1Dump.NewLine);
						}
						else
						{
							Asn1Dump.AsString(text, verbose, asn1Encodable.ToAsn1Object(), buf);
						}
					}
					return;
				}
			}
			if (obj is DerTaggedObject)
			{
				string text2 = indent + "    ";
				buf.Append(indent);
				if (obj is BerTaggedObject)
				{
					buf.Append("BER Tagged [");
				}
				else
				{
					buf.Append("Tagged [");
				}
				DerTaggedObject derTaggedObject = (DerTaggedObject)obj;
				buf.Append(derTaggedObject.TagNo.ToString());
				buf.Append(']');
				if (!derTaggedObject.IsExplicit())
				{
					buf.Append(" IMPLICIT ");
				}
				buf.Append(Asn1Dump.NewLine);
				if (derTaggedObject.IsEmpty())
				{
					buf.Append(text2);
					buf.Append("EMPTY");
					buf.Append(Asn1Dump.NewLine);
					return;
				}
				Asn1Dump.AsString(text2, verbose, derTaggedObject.GetObject(), buf);
				return;
			}
			else
			{
				if (obj is BerSet)
				{
					string text3 = indent + "    ";
					buf.Append(indent);
					buf.Append("BER Set");
					buf.Append(Asn1Dump.NewLine);
					using (IEnumerator enumerator = ((Asn1Set)obj).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							Asn1Encodable asn1Encodable2 = (Asn1Encodable)obj3;
							if (asn1Encodable2 == null)
							{
								buf.Append(text3);
								buf.Append("NULL");
								buf.Append(Asn1Dump.NewLine);
							}
							else
							{
								Asn1Dump.AsString(text3, verbose, asn1Encodable2.ToAsn1Object(), buf);
							}
						}
						return;
					}
				}
				if (obj is DerSet)
				{
					string text4 = indent + "    ";
					buf.Append(indent);
					buf.Append("DER Set");
					buf.Append(Asn1Dump.NewLine);
					using (IEnumerator enumerator = ((Asn1Set)obj).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj4 = enumerator.Current;
							Asn1Encodable asn1Encodable3 = (Asn1Encodable)obj4;
							if (asn1Encodable3 == null)
							{
								buf.Append(text4);
								buf.Append("NULL");
								buf.Append(Asn1Dump.NewLine);
							}
							else
							{
								Asn1Dump.AsString(text4, verbose, asn1Encodable3.ToAsn1Object(), buf);
							}
						}
						return;
					}
				}
				if (obj is DerObjectIdentifier)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"ObjectIdentifier(",
						((DerObjectIdentifier)obj).Id,
						")",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerBoolean)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"Boolean(",
						((DerBoolean)obj).IsTrue.ToString(),
						")",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerInteger)
				{
					buf.Append(string.Concat(new object[]
					{
						indent,
						"Integer(",
						((DerInteger)obj).Value,
						")",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is BerOctetString)
				{
					byte[] octets = ((Asn1OctetString)obj).GetOctets();
					string text5 = verbose ? Asn1Dump.dumpBinaryDataAsString(indent, octets) : "";
					buf.Append(string.Concat(new object[]
					{
						indent,
						"BER Octet String[",
						octets.Length,
						"] ",
						text5,
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerOctetString)
				{
					byte[] octets2 = ((Asn1OctetString)obj).GetOctets();
					string text6 = verbose ? Asn1Dump.dumpBinaryDataAsString(indent, octets2) : "";
					buf.Append(string.Concat(new object[]
					{
						indent,
						"DER Octet String[",
						octets2.Length,
						"] ",
						text6,
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerBitString)
				{
					DerBitString derBitString = (DerBitString)obj;
					byte[] bytes = derBitString.GetBytes();
					string text7 = verbose ? Asn1Dump.dumpBinaryDataAsString(indent, bytes) : "";
					buf.Append(string.Concat(new object[]
					{
						indent,
						"DER Bit String[",
						bytes.Length,
						", ",
						derBitString.PadBits,
						"] ",
						text7,
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerIA5String)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"IA5String(",
						((DerIA5String)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerUtf8String)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"UTF8String(",
						((DerUtf8String)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerPrintableString)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"PrintableString(",
						((DerPrintableString)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerVisibleString)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"VisibleString(",
						((DerVisibleString)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerBmpString)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"BMPString(",
						((DerBmpString)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerT61String)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"T61String(",
						((DerT61String)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerGraphicString)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"GraphicString(",
						((DerGraphicString)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerVideotexString)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"VideotexString(",
						((DerVideotexString)obj).GetString(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerUtcTime)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"UTCTime(",
						((DerUtcTime)obj).TimeString,
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerGeneralizedTime)
				{
					buf.Append(string.Concat(new string[]
					{
						indent,
						"GeneralizedTime(",
						((DerGeneralizedTime)obj).GetTime(),
						") ",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is BerApplicationSpecific)
				{
					buf.Append(Asn1Dump.outputApplicationSpecific("BER", indent, verbose, (BerApplicationSpecific)obj));
					return;
				}
				if (obj is DerApplicationSpecific)
				{
					buf.Append(Asn1Dump.outputApplicationSpecific("DER", indent, verbose, (DerApplicationSpecific)obj));
					return;
				}
				if (obj is DerEnumerated)
				{
					DerEnumerated derEnumerated = (DerEnumerated)obj;
					buf.Append(string.Concat(new object[]
					{
						indent,
						"DER Enumerated(",
						derEnumerated.Value,
						")",
						Asn1Dump.NewLine
					}));
					return;
				}
				if (obj is DerExternal)
				{
					DerExternal derExternal = (DerExternal)obj;
					buf.Append(indent + "External " + Asn1Dump.NewLine);
					string text8 = indent + "    ";
					if (derExternal.DirectReference != null)
					{
						buf.Append(text8 + "Direct Reference: " + derExternal.DirectReference.Id + Asn1Dump.NewLine);
					}
					if (derExternal.IndirectReference != null)
					{
						buf.Append(text8 + "Indirect Reference: " + derExternal.IndirectReference.ToString() + Asn1Dump.NewLine);
					}
					if (derExternal.DataValueDescriptor != null)
					{
						Asn1Dump.AsString(text8, verbose, derExternal.DataValueDescriptor, buf);
					}
					buf.Append(string.Concat(new object[]
					{
						text8,
						"Encoding: ",
						derExternal.Encoding,
						Asn1Dump.NewLine
					}));
					Asn1Dump.AsString(text8, verbose, derExternal.ExternalContent, buf);
					return;
				}
				buf.Append(indent + obj.ToString() + Asn1Dump.NewLine);
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000FB344 File Offset: 0x000F9544
		private static string outputApplicationSpecific(string type, string indent, bool verbose, DerApplicationSpecific app)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (app.IsConstructed())
			{
				try
				{
					Asn1Sequence instance = Asn1Sequence.GetInstance(app.GetObject(16));
					stringBuilder.Append(string.Concat(new object[]
					{
						indent,
						type,
						" ApplicationSpecific[",
						app.ApplicationTag,
						"]",
						Asn1Dump.NewLine
					}));
					foreach (object obj in instance)
					{
						Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
						Asn1Dump.AsString(indent + "    ", verbose, asn1Encodable.ToAsn1Object(), stringBuilder);
					}
				}
				catch (IOException value)
				{
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
			return string.Concat(new object[]
			{
				indent,
				type,
				" ApplicationSpecific[",
				app.ApplicationTag,
				"] (",
				Hex.ToHexString(app.GetContents()),
				")",
				Asn1Dump.NewLine
			});
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000FB47C File Offset: 0x000F967C
		[Obsolete("Use version accepting Asn1Encodable")]
		public static string DumpAsString(object obj)
		{
			if (obj is Asn1Encodable)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Asn1Dump.AsString("", false, ((Asn1Encodable)obj).ToAsn1Object(), stringBuilder);
				return stringBuilder.ToString();
			}
			return "unknown object type " + obj.ToString();
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000FB4C5 File Offset: 0x000F96C5
		public static string DumpAsString(Asn1Encodable obj)
		{
			return Asn1Dump.DumpAsString(obj, false);
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000FB4D0 File Offset: 0x000F96D0
		public static string DumpAsString(Asn1Encodable obj, bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Asn1Dump.AsString("", verbose, obj.ToAsn1Object(), stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000FB4FC File Offset: 0x000F96FC
		private static string dumpBinaryDataAsString(string indent, byte[] bytes)
		{
			indent += "    ";
			StringBuilder stringBuilder = new StringBuilder(Asn1Dump.NewLine);
			for (int i = 0; i < bytes.Length; i += 32)
			{
				if (bytes.Length - i > 32)
				{
					stringBuilder.Append(indent);
					stringBuilder.Append(Hex.ToHexString(bytes, i, 32));
					stringBuilder.Append("    ");
					stringBuilder.Append(Asn1Dump.calculateAscString(bytes, i, 32));
					stringBuilder.Append(Asn1Dump.NewLine);
				}
				else
				{
					stringBuilder.Append(indent);
					stringBuilder.Append(Hex.ToHexString(bytes, i, bytes.Length - i));
					for (int num = bytes.Length - i; num != 32; num++)
					{
						stringBuilder.Append("  ");
					}
					stringBuilder.Append("    ");
					stringBuilder.Append(Asn1Dump.calculateAscString(bytes, i, bytes.Length - i));
					stringBuilder.Append(Asn1Dump.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000FB5EC File Offset: 0x000F97EC
		private static string calculateAscString(byte[] bytes, int off, int len)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int num = off; num != off + len; num++)
			{
				char c = (char)bytes[num];
				if (c >= ' ' && c <= '~')
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001F7D RID: 8061
		private static readonly string NewLine = Platform.NewLine;

		// Token: 0x04001F7E RID: 8062
		private const string Tab = "    ";

		// Token: 0x04001F7F RID: 8063
		private const int SampleSize = 32;
	}
}
