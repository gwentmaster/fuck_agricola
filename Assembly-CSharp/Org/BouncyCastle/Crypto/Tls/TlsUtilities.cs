using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000405 RID: 1029
	public abstract class TlsUtilities
	{
		// Token: 0x060025D5 RID: 9685 RVA: 0x000BE5BE File Offset: 0x000BC7BE
		public static void CheckUint8(int i)
		{
			if (!TlsUtilities.IsValidUint8(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000BE5D0 File Offset: 0x000BC7D0
		public static void CheckUint8(long i)
		{
			if (!TlsUtilities.IsValidUint8(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000BE5E2 File Offset: 0x000BC7E2
		public static void CheckUint16(int i)
		{
			if (!TlsUtilities.IsValidUint16(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000BE5F4 File Offset: 0x000BC7F4
		public static void CheckUint16(long i)
		{
			if (!TlsUtilities.IsValidUint16(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000BE606 File Offset: 0x000BC806
		public static void CheckUint24(int i)
		{
			if (!TlsUtilities.IsValidUint24(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000BE618 File Offset: 0x000BC818
		public static void CheckUint24(long i)
		{
			if (!TlsUtilities.IsValidUint24(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000BE62A File Offset: 0x000BC82A
		public static void CheckUint32(long i)
		{
			if (!TlsUtilities.IsValidUint32(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x000BE63C File Offset: 0x000BC83C
		public static void CheckUint48(long i)
		{
			if (!TlsUtilities.IsValidUint48(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000BE64E File Offset: 0x000BC84E
		public static void CheckUint64(long i)
		{
			if (!TlsUtilities.IsValidUint64(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000BE660 File Offset: 0x000BC860
		public static bool IsValidUint8(int i)
		{
			return (i & 255) == i;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000BE66C File Offset: 0x000BC86C
		public static bool IsValidUint8(long i)
		{
			return (i & 255L) == i;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000BE679 File Offset: 0x000BC879
		public static bool IsValidUint16(int i)
		{
			return (i & 65535) == i;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000BE685 File Offset: 0x000BC885
		public static bool IsValidUint16(long i)
		{
			return (i & 65535L) == i;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000BE692 File Offset: 0x000BC892
		public static bool IsValidUint24(int i)
		{
			return (i & 16777215) == i;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000BE69E File Offset: 0x000BC89E
		public static bool IsValidUint24(long i)
		{
			return (i & 16777215L) == i;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000BE6AB File Offset: 0x000BC8AB
		public static bool IsValidUint32(long i)
		{
			return (i & (long)((ulong)-1)) == i;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000BE6B4 File Offset: 0x000BC8B4
		public static bool IsValidUint48(long i)
		{
			return (i & 281474976710655L) == i;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x0000900B File Offset: 0x0000720B
		public static bool IsValidUint64(long i)
		{
			return true;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000BE6C4 File Offset: 0x000BC8C4
		public static bool IsSsl(TlsContext context)
		{
			return context.ServerVersion.IsSsl;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000BE6D1 File Offset: 0x000BC8D1
		public static bool IsTlsV11(ProtocolVersion version)
		{
			return ProtocolVersion.TLSv11.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000BE6E3 File Offset: 0x000BC8E3
		public static bool IsTlsV11(TlsContext context)
		{
			return TlsUtilities.IsTlsV11(context.ServerVersion);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000BE6F0 File Offset: 0x000BC8F0
		public static bool IsTlsV12(ProtocolVersion version)
		{
			return ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000BE702 File Offset: 0x000BC902
		public static bool IsTlsV12(TlsContext context)
		{
			return TlsUtilities.IsTlsV12(context.ServerVersion);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000BE70F File Offset: 0x000BC90F
		public static void WriteUint8(byte i, Stream output)
		{
			output.WriteByte(i);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000BE718 File Offset: 0x000BC918
		public static void WriteUint8(byte i, byte[] buf, int offset)
		{
			buf[offset] = i;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000BE71E File Offset: 0x000BC91E
		public static void WriteUint16(int i, Stream output)
		{
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000B531D File Offset: 0x000B351D
		public static void WriteUint16(int i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 8);
			buf[offset + 1] = (byte)i;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000BE732 File Offset: 0x000BC932
		public static void WriteUint24(int i, Stream output)
		{
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000BE751 File Offset: 0x000BC951
		public static void WriteUint24(int i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 16);
			buf[offset + 1] = (byte)(i >> 8);
			buf[offset + 2] = (byte)i;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000BE76B File Offset: 0x000BC96B
		public static void WriteUint32(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000BE795 File Offset: 0x000BC995
		public static void WriteUint32(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 24);
			buf[offset + 1] = (byte)(i >> 16);
			buf[offset + 2] = (byte)(i >> 8);
			buf[offset + 3] = (byte)i;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000BE7B9 File Offset: 0x000BC9B9
		public static void WriteUint48(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 40));
			output.WriteByte((byte)(i >> 32));
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000BE7F9 File Offset: 0x000BC9F9
		public static void WriteUint48(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 40);
			buf[offset + 1] = (byte)(i >> 32);
			buf[offset + 2] = (byte)(i >> 24);
			buf[offset + 3] = (byte)(i >> 16);
			buf[offset + 4] = (byte)(i >> 8);
			buf[offset + 5] = (byte)i;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000BE834 File Offset: 0x000BCA34
		public static void WriteUint64(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 56));
			output.WriteByte((byte)(i >> 48));
			output.WriteByte((byte)(i >> 40));
			output.WriteByte((byte)(i >> 32));
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000BE898 File Offset: 0x000BCA98
		public static void WriteUint64(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 56);
			buf[offset + 1] = (byte)(i >> 48);
			buf[offset + 2] = (byte)(i >> 40);
			buf[offset + 3] = (byte)(i >> 32);
			buf[offset + 4] = (byte)(i >> 24);
			buf[offset + 5] = (byte)(i >> 16);
			buf[offset + 6] = (byte)(i >> 8);
			buf[offset + 7] = (byte)i;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000BE8EF File Offset: 0x000BCAEF
		public static void WriteOpaque8(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint8((byte)buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000BE906 File Offset: 0x000BCB06
		public static void WriteOpaque16(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint16(buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000BE91C File Offset: 0x000BCB1C
		public static void WriteOpaque24(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint24(buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000BE932 File Offset: 0x000BCB32
		public static void WriteUint8Array(byte[] uints, Stream output)
		{
			output.Write(uints, 0, uints.Length);
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000BE940 File Offset: 0x000BCB40
		public static void WriteUint8Array(byte[] uints, byte[] buf, int offset)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint8(uints[i], buf, offset);
				offset++;
			}
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000BE96A File Offset: 0x000BCB6A
		public static void WriteUint8ArrayWithUint8Length(byte[] uints, Stream output)
		{
			TlsUtilities.CheckUint8(uints.Length);
			TlsUtilities.WriteUint8((byte)uints.Length, output);
			TlsUtilities.WriteUint8Array(uints, output);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000BE985 File Offset: 0x000BCB85
		public static void WriteUint8ArrayWithUint8Length(byte[] uints, byte[] buf, int offset)
		{
			TlsUtilities.CheckUint8(uints.Length);
			TlsUtilities.WriteUint8((byte)uints.Length, buf, offset);
			TlsUtilities.WriteUint8Array(uints, buf, offset + 1);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000BE9A4 File Offset: 0x000BCBA4
		public static void WriteUint16Array(int[] uints, Stream output)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint16(uints[i], output);
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000BE9C8 File Offset: 0x000BCBC8
		public static void WriteUint16Array(int[] uints, byte[] buf, int offset)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint16(uints[i], buf, offset);
				offset += 2;
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000BE9F2 File Offset: 0x000BCBF2
		public static void WriteUint16ArrayWithUint16Length(int[] uints, Stream output)
		{
			int i = 2 * uints.Length;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, output);
			TlsUtilities.WriteUint16Array(uints, output);
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000BEA0C File Offset: 0x000BCC0C
		public static void WriteUint16ArrayWithUint16Length(int[] uints, byte[] buf, int offset)
		{
			int i = 2 * uints.Length;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, buf, offset);
			TlsUtilities.WriteUint16Array(uints, buf, offset + 2);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000BEA2A File Offset: 0x000BCC2A
		public static byte[] EncodeOpaque8(byte[] buf)
		{
			TlsUtilities.CheckUint8(buf.Length);
			return Arrays.Prepend(buf, (byte)buf.Length);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000BEA40 File Offset: 0x000BCC40
		public static byte[] EncodeUint8ArrayWithUint8Length(byte[] uints)
		{
			byte[] array = new byte[1 + uints.Length];
			TlsUtilities.WriteUint8ArrayWithUint8Length(uints, array, 0);
			return array;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000BEA64 File Offset: 0x000BCC64
		public static byte[] EncodeUint16ArrayWithUint16Length(int[] uints)
		{
			int num = 2 * uints.Length;
			byte[] array = new byte[2 + num];
			TlsUtilities.WriteUint16ArrayWithUint16Length(uints, array, 0);
			return array;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000BEA89 File Offset: 0x000BCC89
		public static byte ReadUint8(Stream input)
		{
			int num = input.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return (byte)num;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000BEA9C File Offset: 0x000BCC9C
		public static byte ReadUint8(byte[] buf, int offset)
		{
			return buf[offset];
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000BEAA4 File Offset: 0x000BCCA4
		public static int ReadUint16(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 8 | num2;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000BEACC File Offset: 0x000BCCCC
		public static int ReadUint16(byte[] buf, int offset)
		{
			return (int)buf[offset] << 8 | (int)buf[++offset];
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000BEADC File Offset: 0x000BCCDC
		public static int ReadUint24(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			int num3 = input.ReadByte();
			if (num3 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 16 | num2 << 8 | num3;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000BEB10 File Offset: 0x000BCD10
		public static int ReadUint24(byte[] buf, int offset)
		{
			return (int)buf[offset] << 16 | (int)buf[++offset] << 8 | (int)buf[++offset];
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000BEB2C File Offset: 0x000BCD2C
		public static long ReadUint32(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			int num3 = input.ReadByte();
			int num4 = input.ReadByte();
			if (num4 < 0)
			{
				throw new EndOfStreamException();
			}
			return (long)((ulong)(num << 24 | num2 << 16 | num3 << 8 | num4));
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000BEB6D File Offset: 0x000BCD6D
		public static long ReadUint32(byte[] buf, int offset)
		{
			return (long)((ulong)((int)buf[offset] << 24 | (int)buf[++offset] << 16 | (int)buf[++offset] << 8 | (int)buf[++offset]));
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000BEB98 File Offset: 0x000BCD98
		public static long ReadUint48(Stream input)
		{
			long num = (long)TlsUtilities.ReadUint24(input);
			int num2 = TlsUtilities.ReadUint24(input);
			return (num & (long)((ulong)-1)) << 24 | ((long)num2 & (long)((ulong)-1));
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000BEBC0 File Offset: 0x000BCDC0
		public static long ReadUint48(byte[] buf, int offset)
		{
			long num = (long)TlsUtilities.ReadUint24(buf, offset);
			int num2 = TlsUtilities.ReadUint24(buf, offset + 3);
			return (num & (long)((ulong)-1)) << 24 | ((long)num2 & (long)((ulong)-1));
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000BEBEC File Offset: 0x000BCDEC
		public static byte[] ReadAllOrNothing(int length, Stream input)
		{
			if (length < 1)
			{
				return TlsUtilities.EmptyBytes;
			}
			byte[] array = new byte[length];
			int num = Streams.ReadFully(input, array);
			if (num == 0)
			{
				return null;
			}
			if (num != length)
			{
				throw new EndOfStreamException();
			}
			return array;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000BEC24 File Offset: 0x000BCE24
		public static byte[] ReadFully(int length, Stream input)
		{
			if (length < 1)
			{
				return TlsUtilities.EmptyBytes;
			}
			byte[] array = new byte[length];
			if (length != Streams.ReadFully(input, array))
			{
				throw new EndOfStreamException();
			}
			return array;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000BEC53 File Offset: 0x000BCE53
		public static void ReadFully(byte[] buf, Stream input)
		{
			if (Streams.ReadFully(input, buf, 0, buf.Length) < buf.Length)
			{
				throw new EndOfStreamException();
			}
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000BEC6B File Offset: 0x000BCE6B
		public static byte[] ReadOpaque8(Stream input)
		{
			byte[] array = new byte[(int)TlsUtilities.ReadUint8(input)];
			TlsUtilities.ReadFully(array, input);
			return array;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000BEC7F File Offset: 0x000BCE7F
		public static byte[] ReadOpaque16(Stream input)
		{
			byte[] array = new byte[TlsUtilities.ReadUint16(input)];
			TlsUtilities.ReadFully(array, input);
			return array;
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000BEC93 File Offset: 0x000BCE93
		public static byte[] ReadOpaque24(Stream input)
		{
			return TlsUtilities.ReadFully(TlsUtilities.ReadUint24(input), input);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000BECA4 File Offset: 0x000BCEA4
		public static byte[] ReadUint8Array(int count, Stream input)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = TlsUtilities.ReadUint8(input);
			}
			return array;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000BECD0 File Offset: 0x000BCED0
		public static int[] ReadUint16Array(int count, Stream input)
		{
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = TlsUtilities.ReadUint16(input);
			}
			return array;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000BECFA File Offset: 0x000BCEFA
		public static ProtocolVersion ReadVersion(byte[] buf, int offset)
		{
			return ProtocolVersion.Get((int)buf[offset], (int)buf[offset + 1]);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000BED0C File Offset: 0x000BCF0C
		public static ProtocolVersion ReadVersion(Stream input)
		{
			int major = input.ReadByte();
			int num = input.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return ProtocolVersion.Get(major, num);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000BED36 File Offset: 0x000BCF36
		public static int ReadVersionRaw(byte[] buf, int offset)
		{
			return (int)buf[offset] << 8 | (int)buf[offset + 1];
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000BED44 File Offset: 0x000BCF44
		public static int ReadVersionRaw(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 8 | num2;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000BED6C File Offset: 0x000BCF6C
		public static Asn1Object ReadAsn1Object(byte[] encoding)
		{
			MemoryStream memoryStream = new MemoryStream(encoding, false);
			Asn1Object asn1Object = new Asn1InputStream(memoryStream, encoding.Length).ReadObject();
			if (asn1Object == null)
			{
				throw new TlsFatalAlert(50);
			}
			if (memoryStream.Position != memoryStream.Length)
			{
				throw new TlsFatalAlert(50);
			}
			return asn1Object;
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000BEDB2 File Offset: 0x000BCFB2
		public static Asn1Object ReadDerObject(byte[] encoding)
		{
			Asn1Object asn1Object = TlsUtilities.ReadAsn1Object(encoding);
			if (!Arrays.AreEqual(asn1Object.GetEncoded("DER"), encoding))
			{
				throw new TlsFatalAlert(50);
			}
			return asn1Object;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000BEDD8 File Offset: 0x000BCFD8
		public static void WriteGmtUnixTime(byte[] buf, int offset)
		{
			int num = (int)(DateTimeUtilities.CurrentUnixMs() / 1000L);
			buf[offset] = (byte)(num >> 24);
			buf[offset + 1] = (byte)(num >> 16);
			buf[offset + 2] = (byte)(num >> 8);
			buf[offset + 3] = (byte)num;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000BEE15 File Offset: 0x000BD015
		public static void WriteVersion(ProtocolVersion version, Stream output)
		{
			output.WriteByte((byte)version.MajorVersion);
			output.WriteByte((byte)version.MinorVersion);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000BEE31 File Offset: 0x000BD031
		public static void WriteVersion(ProtocolVersion version, byte[] buf, int offset)
		{
			buf[offset] = (byte)version.MajorVersion;
			buf[offset + 1] = (byte)version.MinorVersion;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000BEE49 File Offset: 0x000BD049
		public static IList GetDefaultDssSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 2));
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000BEE57 File Offset: 0x000BD057
		public static IList GetDefaultECDsaSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 3));
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000BEE65 File Offset: 0x000BD065
		public static IList GetDefaultRsaSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 1));
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000BEE73 File Offset: 0x000BD073
		public static byte[] GetExtensionData(IDictionary extensions, int extensionType)
		{
			if (extensions != null)
			{
				return (byte[])extensions[extensionType];
			}
			return null;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000BEE8C File Offset: 0x000BD08C
		public static IList GetDefaultSupportedSignatureAlgorithms()
		{
			byte[] array = new byte[]
			{
				2,
				3,
				4,
				5,
				6
			};
			byte[] array2 = new byte[]
			{
				1,
				2,
				3
			};
			IList list = Platform.CreateArrayList();
			for (int i = 0; i < array2.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(new SignatureAndHashAlgorithm(array[j], array2[i]));
				}
			}
			return list;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000BEEF8 File Offset: 0x000BD0F8
		public static SignatureAndHashAlgorithm GetSignatureAndHashAlgorithm(TlsContext context, TlsSignerCredentials signerCredentials)
		{
			SignatureAndHashAlgorithm signatureAndHashAlgorithm = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				signatureAndHashAlgorithm = signerCredentials.SignatureAndHashAlgorithm;
				if (signatureAndHashAlgorithm == null)
				{
					throw new TlsFatalAlert(80);
				}
			}
			return signatureAndHashAlgorithm;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000BEF24 File Offset: 0x000BD124
		public static bool HasExpectedEmptyExtensionData(IDictionary extensions, int extensionType, byte alertDescription)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, extensionType);
			if (extensionData == null)
			{
				return false;
			}
			if (extensionData.Length != 0)
			{
				throw new TlsFatalAlert(alertDescription);
			}
			return true;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000BEF4A File Offset: 0x000BD14A
		public static TlsSession ImportSession(byte[] sessionID, SessionParameters sessionParameters)
		{
			return new TlsSessionImpl(sessionID, sessionParameters);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000BE6F0 File Offset: 0x000BC8F0
		public static bool IsSignatureAlgorithmsExtensionAllowed(ProtocolVersion clientVersion)
		{
			return ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(clientVersion.GetEquivalentTLSVersion());
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000BEF53 File Offset: 0x000BD153
		public static void AddSignatureAlgorithmsExtension(IDictionary extensions, IList supportedSignatureAlgorithms)
		{
			extensions[13] = TlsUtilities.CreateSignatureAlgorithmsExtension(supportedSignatureAlgorithms);
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000BEF68 File Offset: 0x000BD168
		public static IList GetSignatureAlgorithmsExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 13);
			if (extensionData != null)
			{
				return TlsUtilities.ReadSignatureAlgorithmsExtension(extensionData);
			}
			return null;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000BEF8C File Offset: 0x000BD18C
		public static byte[] CreateSignatureAlgorithmsExtension(IList supportedSignatureAlgorithms)
		{
			MemoryStream memoryStream = new MemoryStream();
			TlsUtilities.EncodeSupportedSignatureAlgorithms(supportedSignatureAlgorithms, false, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000BEFB0 File Offset: 0x000BD1B0
		public static IList ReadSignatureAlgorithmsExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			IList result = TlsUtilities.ParseSupportedSignatureAlgorithms(false, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000BEFE0 File Offset: 0x000BD1E0
		public static void EncodeSupportedSignatureAlgorithms(IList supportedSignatureAlgorithms, bool allowAnonymous, Stream output)
		{
			if (supportedSignatureAlgorithms == null)
			{
				throw new ArgumentNullException("supportedSignatureAlgorithms");
			}
			if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
			{
				throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
			}
			int i = 2 * supportedSignatureAlgorithms.Count;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, output);
			foreach (object obj in supportedSignatureAlgorithms)
			{
				SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
				if (!allowAnonymous && signatureAndHashAlgorithm.Signature == 0)
				{
					throw new ArgumentException("SignatureAlgorithm.anonymous MUST NOT appear in the signature_algorithms extension");
				}
				signatureAndHashAlgorithm.Encode(output);
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000BF094 File Offset: 0x000BD294
		public static IList ParseSupportedSignatureAlgorithms(bool allowAnonymous, Stream input)
		{
			int num = TlsUtilities.ReadUint16(input);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			int num2 = num / 2;
			IList list = Platform.CreateArrayList(num2);
			for (int i = 0; i < num2; i++)
			{
				SignatureAndHashAlgorithm signatureAndHashAlgorithm = SignatureAndHashAlgorithm.Parse(input);
				if (!allowAnonymous && signatureAndHashAlgorithm.Signature == 0)
				{
					throw new TlsFatalAlert(47);
				}
				list.Add(signatureAndHashAlgorithm);
			}
			return list;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000BF0F8 File Offset: 0x000BD2F8
		public static void VerifySupportedSignatureAlgorithm(IList supportedSignatureAlgorithms, SignatureAndHashAlgorithm signatureAlgorithm)
		{
			if (supportedSignatureAlgorithms == null)
			{
				throw new ArgumentNullException("supportedSignatureAlgorithms");
			}
			if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
			{
				throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
			}
			if (signatureAlgorithm == null)
			{
				throw new ArgumentNullException("signatureAlgorithm");
			}
			if (signatureAlgorithm.Signature != 0)
			{
				foreach (object obj in supportedSignatureAlgorithms)
				{
					SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
					if (signatureAndHashAlgorithm.Hash == signatureAlgorithm.Hash && signatureAndHashAlgorithm.Signature == signatureAlgorithm.Signature)
					{
						return;
					}
				}
			}
			throw new TlsFatalAlert(47);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000BF1B8 File Offset: 0x000BD3B8
		public static byte[] PRF(TlsContext context, byte[] secret, string asciiLabel, byte[] seed, int size)
		{
			if (context.ServerVersion.IsSsl)
			{
				throw new InvalidOperationException("No PRF available for SSLv3 session");
			}
			byte[] array = Strings.ToByteArray(asciiLabel);
			byte[] array2 = TlsUtilities.Concat(array, seed);
			int prfAlgorithm = context.SecurityParameters.PrfAlgorithm;
			if (prfAlgorithm == 0)
			{
				return TlsUtilities.PRF_legacy(secret, array, array2, size);
			}
			IDigest digest = TlsUtilities.CreatePrfHash(prfAlgorithm);
			byte[] array3 = new byte[size];
			TlsUtilities.HMacHash(digest, secret, array2, array3);
			return array3;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000BF220 File Offset: 0x000BD420
		public static byte[] PRF_legacy(byte[] secret, string asciiLabel, byte[] seed, int size)
		{
			byte[] array = Strings.ToByteArray(asciiLabel);
			byte[] labelSeed = TlsUtilities.Concat(array, seed);
			return TlsUtilities.PRF_legacy(secret, array, labelSeed, size);
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000BF248 File Offset: 0x000BD448
		internal static byte[] PRF_legacy(byte[] secret, byte[] label, byte[] labelSeed, int size)
		{
			int num = (secret.Length + 1) / 2;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num];
			Array.Copy(secret, 0, array, 0, num);
			Array.Copy(secret, secret.Length - num, array2, 0, num);
			byte[] array3 = new byte[size];
			byte[] array4 = new byte[size];
			TlsUtilities.HMacHash(TlsUtilities.CreateHash(1), array, labelSeed, array3);
			TlsUtilities.HMacHash(TlsUtilities.CreateHash(2), array2, labelSeed, array4);
			for (int i = 0; i < size; i++)
			{
				byte[] array5 = array3;
				int num2 = i;
				array5[num2] ^= array4[i];
			}
			return array3;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000BF2D4 File Offset: 0x000BD4D4
		internal static byte[] Concat(byte[] a, byte[] b)
		{
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000BF30C File Offset: 0x000BD50C
		internal static void HMacHash(IDigest digest, byte[] secret, byte[] seed, byte[] output)
		{
			HMac hmac = new HMac(digest);
			hmac.Init(new KeyParameter(secret));
			byte[] array = seed;
			int digestSize = digest.GetDigestSize();
			int num = (output.Length + digestSize - 1) / digestSize;
			byte[] array2 = new byte[hmac.GetMacSize()];
			byte[] array3 = new byte[hmac.GetMacSize()];
			for (int i = 0; i < num; i++)
			{
				hmac.BlockUpdate(array, 0, array.Length);
				hmac.DoFinal(array2, 0);
				array = array2;
				hmac.BlockUpdate(array, 0, array.Length);
				hmac.BlockUpdate(seed, 0, seed.Length);
				hmac.DoFinal(array3, 0);
				Array.Copy(array3, 0, output, digestSize * i, Math.Min(digestSize, output.Length - digestSize * i));
			}
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000BF3BC File Offset: 0x000BD5BC
		internal static void ValidateKeyUsage(X509CertificateStructure c, int keyUsageBits)
		{
			X509Extensions extensions = c.TbsCertificate.Extensions;
			if (extensions != null)
			{
				X509Extension extension = extensions.GetExtension(X509Extensions.KeyUsage);
				if (extension != null && ((int)KeyUsage.GetInstance(extension).GetBytes()[0] & keyUsageBits) != keyUsageBits)
				{
					throw new TlsFatalAlert(46);
				}
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000BF404 File Offset: 0x000BD604
		internal static byte[] CalculateKeyBlock(TlsContext context, int size)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] masterSecret = securityParameters.MasterSecret;
			byte[] array = TlsUtilities.Concat(securityParameters.ServerRandom, securityParameters.ClientRandom);
			if (TlsUtilities.IsSsl(context))
			{
				return TlsUtilities.CalculateKeyBlock_Ssl(masterSecret, array, size);
			}
			return TlsUtilities.PRF(context, masterSecret, "key expansion", array, size);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000BF450 File Offset: 0x000BD650
		internal static byte[] CalculateKeyBlock_Ssl(byte[] master_secret, byte[] random, int size)
		{
			IDigest digest = TlsUtilities.CreateHash(1);
			IDigest digest2 = TlsUtilities.CreateHash(2);
			int digestSize = digest.GetDigestSize();
			byte[] array = new byte[digest2.GetDigestSize()];
			byte[] array2 = new byte[size + digestSize];
			int num = 0;
			int i = 0;
			while (i < size)
			{
				byte[] array3 = TlsUtilities.SSL3_CONST[num];
				digest2.BlockUpdate(array3, 0, array3.Length);
				digest2.BlockUpdate(master_secret, 0, master_secret.Length);
				digest2.BlockUpdate(random, 0, random.Length);
				digest2.DoFinal(array, 0);
				digest.BlockUpdate(master_secret, 0, master_secret.Length);
				digest.BlockUpdate(array, 0, array.Length);
				digest.DoFinal(array2, i);
				i += digestSize;
				num++;
			}
			return Arrays.CopyOfRange(array2, 0, size);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000BF504 File Offset: 0x000BD704
		internal static byte[] CalculateMasterSecret(TlsContext context, byte[] pre_master_secret)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] array = securityParameters.extendedMasterSecret ? securityParameters.SessionHash : TlsUtilities.Concat(securityParameters.ClientRandom, securityParameters.ServerRandom);
			if (TlsUtilities.IsSsl(context))
			{
				return TlsUtilities.CalculateMasterSecret_Ssl(pre_master_secret, array);
			}
			string asciiLabel = securityParameters.extendedMasterSecret ? ExporterLabel.extended_master_secret : "master secret";
			return TlsUtilities.PRF(context, pre_master_secret, asciiLabel, array, 48);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000BF56C File Offset: 0x000BD76C
		internal static byte[] CalculateMasterSecret_Ssl(byte[] pre_master_secret, byte[] random)
		{
			IDigest digest = TlsUtilities.CreateHash(1);
			IDigest digest2 = TlsUtilities.CreateHash(2);
			int digestSize = digest.GetDigestSize();
			byte[] array = new byte[digest2.GetDigestSize()];
			byte[] array2 = new byte[digestSize * 3];
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				byte[] array3 = TlsUtilities.SSL3_CONST[i];
				digest2.BlockUpdate(array3, 0, array3.Length);
				digest2.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
				digest2.BlockUpdate(random, 0, random.Length);
				digest2.DoFinal(array, 0);
				digest.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
				digest.BlockUpdate(array, 0, array.Length);
				digest.DoFinal(array2, num);
				num += digestSize;
			}
			return array2;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000BF618 File Offset: 0x000BD818
		internal static byte[] CalculateVerifyData(TlsContext context, string asciiLabel, byte[] handshakeHash)
		{
			if (TlsUtilities.IsSsl(context))
			{
				return handshakeHash;
			}
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] masterSecret = securityParameters.MasterSecret;
			int verifyDataLength = securityParameters.VerifyDataLength;
			return TlsUtilities.PRF(context, masterSecret, asciiLabel, handshakeHash, verifyDataLength);
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000BF64C File Offset: 0x000BD84C
		public static IDigest CreateHash(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return new MD5Digest();
			case 2:
				return new Sha1Digest();
			case 3:
				return new Sha224Digest();
			case 4:
				return new Sha256Digest();
			case 5:
				return new Sha384Digest();
			case 6:
				return new Sha512Digest();
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000BF6B0 File Offset: 0x000BD8B0
		public static IDigest CreateHash(SignatureAndHashAlgorithm signatureAndHashAlgorithm)
		{
			if (signatureAndHashAlgorithm != null)
			{
				return TlsUtilities.CreateHash(signatureAndHashAlgorithm.Hash);
			}
			return new CombinedHash();
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000BF6D4 File Offset: 0x000BD8D4
		public static IDigest CloneHash(byte hashAlgorithm, IDigest hash)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return new MD5Digest((MD5Digest)hash);
			case 2:
				return new Sha1Digest((Sha1Digest)hash);
			case 3:
				return new Sha224Digest((Sha224Digest)hash);
			case 4:
				return new Sha256Digest((Sha256Digest)hash);
			case 5:
				return new Sha384Digest((Sha384Digest)hash);
			case 6:
				return new Sha512Digest((Sha512Digest)hash);
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000BF75A File Offset: 0x000BD95A
		public static IDigest CreatePrfHash(int prfAlgorithm)
		{
			if (prfAlgorithm == 0)
			{
				return new CombinedHash();
			}
			return TlsUtilities.CreateHash(TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm));
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000BF770 File Offset: 0x000BD970
		public static IDigest ClonePrfHash(int prfAlgorithm, IDigest hash)
		{
			if (prfAlgorithm == 0)
			{
				return new CombinedHash((CombinedHash)hash);
			}
			return TlsUtilities.CloneHash(TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm), hash);
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000BF78D File Offset: 0x000BD98D
		public static byte GetHashAlgorithmForPrfAlgorithm(int prfAlgorithm)
		{
			switch (prfAlgorithm)
			{
			case 0:
				throw new ArgumentException("legacy PRF not a valid algorithm", "prfAlgorithm");
			case 1:
				return 4;
			case 2:
				return 5;
			default:
				throw new ArgumentException("unknown PrfAlgorithm", "prfAlgorithm");
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000BF7C8 File Offset: 0x000BD9C8
		public static DerObjectIdentifier GetOidForHashAlgorithm(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return PkcsObjectIdentifiers.MD5;
			case 2:
				return X509ObjectIdentifiers.IdSha1;
			case 3:
				return NistObjectIdentifiers.IdSha224;
			case 4:
				return NistObjectIdentifiers.IdSha256;
			case 5:
				return NistObjectIdentifiers.IdSha384;
			case 6:
				return NistObjectIdentifiers.IdSha512;
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000BF82C File Offset: 0x000BDA2C
		internal static short GetClientCertificateType(Certificate clientCertificate, Certificate serverCertificate)
		{
			if (clientCertificate.IsEmpty)
			{
				return -1;
			}
			X509CertificateStructure certificateAt = clientCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			short result;
			try
			{
				AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
				if (asymmetricKeyParameter.IsPrivate)
				{
					throw new TlsFatalAlert(80);
				}
				if (asymmetricKeyParameter is RsaKeyParameters)
				{
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 1;
				}
				else if (asymmetricKeyParameter is DsaPublicKeyParameters)
				{
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 2;
				}
				else
				{
					if (!(asymmetricKeyParameter is ECPublicKeyParameters))
					{
						throw new TlsFatalAlert(43);
					}
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 64;
				}
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			return result;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000BF8D4 File Offset: 0x000BDAD4
		internal static void TrackHashAlgorithms(TlsHandshakeHash handshakeHash, IList supportedSignatureAlgorithms)
		{
			if (supportedSignatureAlgorithms != null)
			{
				foreach (object obj in supportedSignatureAlgorithms)
				{
					byte hash = ((SignatureAndHashAlgorithm)obj).Hash;
					if (!HashAlgorithm.IsPrivate(hash))
					{
						handshakeHash.TrackHashAlgorithm(hash);
					}
				}
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000BF938 File Offset: 0x000BDB38
		public static bool HasSigningCapability(byte clientCertificateType)
		{
			return clientCertificateType - 1 <= 1 || clientCertificateType == 64;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000BF948 File Offset: 0x000BDB48
		public static TlsSigner CreateTlsSigner(byte clientCertificateType)
		{
			if (clientCertificateType == 1)
			{
				return new TlsRsaSigner();
			}
			if (clientCertificateType == 2)
			{
				return new TlsDssSigner();
			}
			if (clientCertificateType != 64)
			{
				throw new ArgumentException("not a type with signing capability", "clientCertificateType");
			}
			return new TlsECDsaSigner();
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000BF97C File Offset: 0x000BDB7C
		private static byte[][] GenSsl3Const()
		{
			int num = 10;
			byte[][] array = new byte[num][];
			for (int i = 0; i < num; i++)
			{
				byte[] array2 = new byte[i + 1];
				Arrays.Fill(array2, (byte)(65 + i));
				array[i] = array2;
			}
			return array;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000BF9B8 File Offset: 0x000BDBB8
		private static IList VectorOfOne(object obj)
		{
			IList list = Platform.CreateArrayList(1);
			list.Add(obj);
			return list;
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000BF9C8 File Offset: 0x000BDBC8
		public static int GetCipherType(int ciphersuite)
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(ciphersuite);
			switch (encryptionAlgorithm)
			{
			case 1:
			case 2:
				return 0;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 12:
			case 13:
			case 14:
				return 1;
			case 10:
			case 11:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
				break;
			default:
				if (encryptionAlgorithm - 102 > 2)
				{
					throw new TlsFatalAlert(80);
				}
				break;
			}
			return 2;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000BFA48 File Offset: 0x000BDC48
		public static int GetEncryptionAlgorithm(int ciphersuite)
		{
			if (ciphersuite <= 49327)
			{
				switch (ciphersuite)
				{
				case 1:
					return 0;
				case 2:
				case 44:
				case 45:
				case 46:
					return 0;
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 17:
				case 18:
				case 20:
				case 21:
				case 23:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
				case 43:
				case 52:
				case 58:
				case 70:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
				case 87:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
				case 108:
				case 109:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				case 122:
				case 123:
				case 124:
				case 125:
				case 126:
				case 127:
				case 128:
				case 129:
				case 130:
				case 131:
				case 137:
				case 155:
				case 166:
				case 167:
				case 191:
					goto IL_6A9;
				case 4:
				case 24:
					return 2;
				case 5:
				case 138:
				case 142:
				case 146:
					return 2;
				case 10:
				case 13:
				case 16:
				case 19:
				case 22:
				case 139:
				case 143:
				case 147:
					break;
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 60:
				case 62:
				case 63:
				case 64:
				case 103:
				case 140:
				case 144:
				case 148:
				case 174:
				case 178:
				case 182:
					return 8;
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 61:
				case 104:
				case 105:
				case 106:
				case 107:
				case 141:
				case 145:
				case 149:
				case 175:
				case 179:
				case 183:
					return 9;
				case 59:
				case 176:
				case 180:
				case 184:
					return 0;
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
					return 12;
				case 132:
				case 133:
				case 134:
				case 135:
				case 136:
					return 13;
				case 150:
				case 151:
				case 152:
				case 153:
				case 154:
					return 14;
				case 156:
				case 158:
				case 160:
				case 162:
				case 164:
				case 168:
				case 170:
				case 172:
					return 10;
				case 157:
				case 159:
				case 161:
				case 163:
				case 165:
				case 169:
				case 171:
				case 173:
					return 11;
				case 177:
				case 181:
				case 185:
					return 0;
				case 186:
				case 187:
				case 188:
				case 189:
				case 190:
					return 12;
				case 192:
				case 193:
				case 194:
				case 195:
				case 196:
					return 13;
				default:
					switch (ciphersuite)
					{
					case 49153:
					case 49158:
					case 49163:
					case 49168:
					case 49173:
					case 49209:
						return 0;
					case 49154:
					case 49159:
					case 49164:
					case 49169:
					case 49174:
					case 49203:
						return 2;
					case 49155:
					case 49160:
					case 49165:
					case 49170:
					case 49175:
					case 49178:
					case 49179:
					case 49180:
					case 49204:
						break;
					case 49156:
					case 49161:
					case 49166:
					case 49171:
					case 49176:
					case 49181:
					case 49182:
					case 49183:
					case 49187:
					case 49189:
					case 49191:
					case 49193:
					case 49205:
					case 49207:
						return 8;
					case 49157:
					case 49162:
					case 49167:
					case 49172:
					case 49177:
					case 49184:
					case 49185:
					case 49186:
					case 49188:
					case 49190:
					case 49192:
					case 49194:
					case 49206:
					case 49208:
						return 9;
					case 49195:
					case 49197:
					case 49199:
					case 49201:
						return 10;
					case 49196:
					case 49198:
					case 49200:
					case 49202:
						return 11;
					case 49210:
						return 0;
					case 49211:
						return 0;
					case 49212:
					case 49213:
					case 49214:
					case 49215:
					case 49216:
					case 49217:
					case 49218:
					case 49219:
					case 49220:
					case 49221:
					case 49222:
					case 49223:
					case 49224:
					case 49225:
					case 49226:
					case 49227:
					case 49228:
					case 49229:
					case 49230:
					case 49231:
					case 49232:
					case 49233:
					case 49234:
					case 49235:
					case 49236:
					case 49237:
					case 49238:
					case 49239:
					case 49240:
					case 49241:
					case 49242:
					case 49243:
					case 49244:
					case 49245:
					case 49246:
					case 49247:
					case 49248:
					case 49249:
					case 49250:
					case 49251:
					case 49252:
					case 49253:
					case 49254:
					case 49255:
					case 49256:
					case 49257:
					case 49258:
					case 49259:
					case 49260:
					case 49261:
					case 49262:
					case 49263:
					case 49264:
					case 49265:
					case 49284:
					case 49285:
						goto IL_6A9;
					case 49266:
					case 49268:
					case 49270:
					case 49272:
					case 49300:
					case 49302:
					case 49304:
					case 49306:
						return 12;
					case 49267:
					case 49269:
					case 49271:
					case 49273:
					case 49301:
					case 49303:
					case 49305:
					case 49307:
						return 13;
					case 49274:
					case 49276:
					case 49278:
					case 49280:
					case 49282:
					case 49286:
					case 49288:
					case 49290:
					case 49292:
					case 49294:
					case 49296:
					case 49298:
						return 19;
					case 49275:
					case 49277:
					case 49279:
					case 49281:
					case 49283:
					case 49287:
					case 49289:
					case 49291:
					case 49293:
					case 49295:
					case 49297:
					case 49299:
						return 20;
					case 49308:
					case 49310:
					case 49316:
					case 49318:
					case 49324:
						return 15;
					case 49309:
					case 49311:
					case 49317:
					case 49319:
					case 49325:
						return 17;
					case 49312:
					case 49314:
					case 49320:
					case 49322:
					case 49326:
						return 16;
					case 49313:
					case 49315:
					case 49321:
					case 49323:
					case 49327:
						return 18;
					default:
						goto IL_6A9;
					}
					break;
				}
				return 7;
			}
			if (ciphersuite - 52392 <= 6)
			{
				return 102;
			}
			switch (ciphersuite)
			{
			case 65280:
			case 65282:
			case 65284:
			case 65296:
			case 65298:
			case 65300:
				return 103;
			case 65281:
			case 65283:
			case 65285:
			case 65297:
			case 65299:
			case 65301:
				return 104;
			}
			IL_6A9:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000C0108 File Offset: 0x000BE308
		public static int GetKeyExchangeAlgorithm(int ciphersuite)
		{
			if (ciphersuite > 196)
			{
				switch (ciphersuite)
				{
				case 49153:
				case 49154:
				case 49155:
				case 49156:
				case 49157:
				case 49189:
				case 49190:
				case 49197:
				case 49198:
				case 49268:
				case 49269:
				case 49288:
				case 49289:
					break;
				case 49158:
				case 49159:
				case 49160:
				case 49161:
				case 49162:
				case 49187:
				case 49188:
				case 49195:
				case 49196:
				case 49266:
				case 49267:
				case 49286:
				case 49287:
				case 49324:
				case 49325:
				case 49326:
				case 49327:
					return 17;
				case 49163:
				case 49164:
				case 49165:
				case 49166:
				case 49167:
				case 49193:
				case 49194:
				case 49201:
				case 49202:
				case 49272:
				case 49273:
				case 49292:
				case 49293:
					return 18;
				case 49168:
				case 49169:
				case 49170:
				case 49171:
				case 49172:
				case 49191:
				case 49192:
				case 49199:
				case 49200:
				case 49270:
				case 49271:
				case 49290:
				case 49291:
					return 19;
				case 49173:
				case 49174:
				case 49175:
				case 49176:
				case 49177:
					return 20;
				case 49178:
				case 49181:
				case 49184:
					return 21;
				case 49179:
				case 49182:
				case 49185:
					return 23;
				case 49180:
				case 49183:
				case 49186:
					return 22;
				case 49203:
				case 49204:
				case 49205:
				case 49206:
				case 49207:
				case 49208:
				case 49209:
				case 49210:
				case 49211:
				case 49306:
				case 49307:
					return 24;
				case 49212:
				case 49213:
				case 49214:
				case 49215:
				case 49216:
				case 49217:
				case 49218:
				case 49219:
				case 49220:
				case 49221:
				case 49222:
				case 49223:
				case 49224:
				case 49225:
				case 49226:
				case 49227:
				case 49228:
				case 49229:
				case 49230:
				case 49231:
				case 49232:
				case 49233:
				case 49234:
				case 49235:
				case 49236:
				case 49237:
				case 49238:
				case 49239:
				case 49240:
				case 49241:
				case 49242:
				case 49243:
				case 49244:
				case 49245:
				case 49246:
				case 49247:
				case 49248:
				case 49249:
				case 49250:
				case 49251:
				case 49252:
				case 49253:
				case 49254:
				case 49255:
				case 49256:
				case 49257:
				case 49258:
				case 49259:
				case 49260:
				case 49261:
				case 49262:
				case 49263:
				case 49264:
				case 49265:
				case 49284:
				case 49285:
					goto IL_69C;
				case 49274:
				case 49275:
				case 49308:
				case 49309:
				case 49312:
				case 49313:
					return 1;
				case 49276:
				case 49277:
				case 49310:
				case 49311:
				case 49314:
				case 49315:
					return 5;
				case 49278:
				case 49279:
					return 9;
				case 49280:
				case 49281:
					return 3;
				case 49282:
				case 49283:
					return 7;
				case 49294:
				case 49295:
				case 49300:
				case 49301:
				case 49316:
				case 49317:
				case 49320:
				case 49321:
					return 13;
				case 49296:
				case 49297:
				case 49302:
				case 49303:
				case 49318:
				case 49319:
				case 49322:
				case 49323:
					return 14;
				case 49298:
				case 49299:
				case 49304:
				case 49305:
					return 15;
				default:
					switch (ciphersuite)
					{
					case 52392:
						return 19;
					case 52393:
						return 17;
					case 52394:
						return 5;
					case 52395:
						return 13;
					case 52396:
						return 24;
					case 52397:
						return 14;
					case 52398:
						return 1;
					default:
						switch (ciphersuite)
						{
						case 65280:
						case 65281:
							return 5;
						case 65282:
						case 65283:
							return 19;
						case 65284:
						case 65285:
							break;
						case 65286:
						case 65287:
						case 65288:
						case 65289:
						case 65290:
						case 65291:
						case 65292:
						case 65293:
						case 65294:
						case 65295:
							goto IL_69C;
						case 65296:
						case 65297:
							return 13;
						case 65298:
						case 65299:
							return 14;
						case 65300:
						case 65301:
							return 24;
						default:
							goto IL_69C;
						}
						break;
					}
					break;
				}
				return 16;
			}
			if (ciphersuite - 1 <= 1 || ciphersuite - 4 <= 1)
			{
				return 1;
			}
			switch (ciphersuite)
			{
			case 10:
			case 47:
			case 53:
			case 59:
			case 60:
			case 61:
			case 65:
			case 132:
			case 150:
			case 156:
			case 157:
			case 186:
			case 192:
				return 1;
			case 11:
			case 12:
			case 14:
			case 15:
			case 17:
			case 18:
			case 20:
			case 21:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
			case 43:
			case 52:
			case 58:
			case 70:
			case 71:
			case 72:
			case 73:
			case 74:
			case 75:
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 81:
			case 82:
			case 83:
			case 84:
			case 85:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 96:
			case 97:
			case 98:
			case 99:
			case 100:
			case 101:
			case 102:
			case 108:
			case 109:
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 137:
			case 155:
			case 166:
			case 167:
			case 191:
				goto IL_69C;
			case 13:
			case 48:
			case 54:
			case 62:
			case 66:
			case 104:
			case 133:
			case 151:
			case 164:
			case 165:
			case 187:
			case 193:
				break;
			case 16:
			case 49:
			case 55:
			case 63:
			case 67:
			case 105:
			case 134:
			case 152:
			case 160:
			case 161:
			case 188:
			case 194:
				return 9;
			case 19:
			case 50:
			case 56:
			case 64:
			case 68:
			case 106:
			case 135:
			case 153:
			case 162:
			case 163:
			case 189:
			case 195:
				return 3;
			case 22:
			case 51:
			case 57:
			case 69:
			case 103:
			case 107:
			case 136:
			case 154:
			case 158:
			case 159:
			case 190:
			case 196:
				return 5;
			case 44:
			case 138:
			case 139:
			case 140:
			case 141:
			case 168:
			case 169:
			case 174:
			case 175:
			case 176:
			case 177:
				return 13;
			case 45:
			case 142:
			case 143:
			case 144:
			case 145:
			case 170:
			case 171:
			case 178:
			case 179:
			case 180:
			case 181:
				return 14;
			case 46:
			case 146:
			case 147:
			case 148:
			case 149:
			case 172:
			case 173:
			case 182:
			case 183:
			case 184:
			case 185:
				return 15;
			default:
				goto IL_69C;
			}
			return 7;
			IL_69C:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000C07B8 File Offset: 0x000BE9B8
		public static int GetMacAlgorithm(int ciphersuite)
		{
			if (ciphersuite <= 49327)
			{
				switch (ciphersuite)
				{
				case 1:
				case 4:
					return 1;
				case 2:
				case 5:
					break;
				case 3:
					goto IL_60E;
				default:
					switch (ciphersuite)
					{
					case 10:
					case 13:
					case 16:
					case 19:
					case 22:
					case 44:
					case 45:
					case 46:
					case 47:
					case 48:
					case 49:
					case 50:
					case 51:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
					case 132:
					case 133:
					case 134:
					case 135:
					case 136:
					case 138:
					case 139:
					case 140:
					case 141:
					case 142:
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 151:
					case 152:
					case 153:
					case 154:
						return 2;
					case 11:
					case 12:
					case 14:
					case 15:
					case 17:
					case 18:
					case 20:
					case 21:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
					case 30:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
					case 40:
					case 41:
					case 42:
					case 43:
					case 52:
					case 58:
					case 70:
					case 71:
					case 72:
					case 73:
					case 74:
					case 75:
					case 76:
					case 77:
					case 78:
					case 79:
					case 80:
					case 81:
					case 82:
					case 83:
					case 84:
					case 85:
					case 86:
					case 87:
					case 88:
					case 89:
					case 90:
					case 91:
					case 92:
					case 93:
					case 94:
					case 95:
					case 96:
					case 97:
					case 98:
					case 99:
					case 100:
					case 101:
					case 102:
					case 108:
					case 109:
					case 110:
					case 111:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
					case 122:
					case 123:
					case 124:
					case 125:
					case 126:
					case 127:
					case 128:
					case 129:
					case 130:
					case 131:
					case 137:
					case 155:
					case 166:
					case 167:
					case 191:
						goto IL_60E;
					case 59:
					case 60:
					case 61:
					case 62:
					case 63:
					case 64:
					case 103:
					case 104:
					case 105:
					case 106:
					case 107:
					case 174:
					case 176:
					case 178:
					case 180:
					case 182:
					case 184:
					case 186:
					case 187:
					case 188:
					case 189:
					case 190:
					case 192:
					case 193:
					case 194:
					case 195:
					case 196:
						break;
					case 156:
					case 157:
					case 158:
					case 159:
					case 160:
					case 161:
					case 162:
					case 163:
					case 164:
					case 165:
					case 168:
					case 169:
					case 170:
					case 171:
					case 172:
					case 173:
						return 0;
					case 175:
					case 177:
					case 179:
					case 181:
					case 183:
					case 185:
						return 4;
					default:
						switch (ciphersuite)
						{
						case 49153:
						case 49154:
						case 49155:
						case 49156:
						case 49157:
						case 49158:
						case 49159:
						case 49160:
						case 49161:
						case 49162:
						case 49163:
						case 49164:
						case 49165:
						case 49166:
						case 49167:
						case 49168:
						case 49169:
						case 49170:
						case 49171:
						case 49172:
						case 49173:
						case 49174:
						case 49175:
						case 49176:
						case 49177:
						case 49178:
						case 49179:
						case 49180:
						case 49181:
						case 49182:
						case 49183:
						case 49184:
						case 49185:
						case 49186:
						case 49203:
						case 49204:
						case 49205:
						case 49206:
						case 49209:
							return 2;
						case 49187:
						case 49189:
						case 49191:
						case 49193:
						case 49207:
						case 49210:
						case 49266:
						case 49268:
						case 49270:
						case 49272:
						case 49300:
						case 49302:
						case 49304:
						case 49306:
							break;
						case 49188:
						case 49190:
						case 49192:
						case 49194:
						case 49208:
						case 49211:
						case 49267:
						case 49269:
						case 49271:
						case 49273:
						case 49301:
						case 49303:
						case 49305:
						case 49307:
							return 4;
						case 49195:
						case 49196:
						case 49197:
						case 49198:
						case 49199:
						case 49200:
						case 49201:
						case 49202:
						case 49274:
						case 49275:
						case 49276:
						case 49277:
						case 49278:
						case 49279:
						case 49280:
						case 49281:
						case 49282:
						case 49283:
						case 49286:
						case 49287:
						case 49288:
						case 49289:
						case 49290:
						case 49291:
						case 49292:
						case 49293:
						case 49294:
						case 49295:
						case 49296:
						case 49297:
						case 49298:
						case 49299:
						case 49308:
						case 49309:
						case 49310:
						case 49311:
						case 49312:
						case 49313:
						case 49314:
						case 49315:
						case 49316:
						case 49317:
						case 49318:
						case 49319:
						case 49320:
						case 49321:
						case 49322:
						case 49323:
						case 49324:
						case 49325:
						case 49326:
						case 49327:
							return 0;
						case 49212:
						case 49213:
						case 49214:
						case 49215:
						case 49216:
						case 49217:
						case 49218:
						case 49219:
						case 49220:
						case 49221:
						case 49222:
						case 49223:
						case 49224:
						case 49225:
						case 49226:
						case 49227:
						case 49228:
						case 49229:
						case 49230:
						case 49231:
						case 49232:
						case 49233:
						case 49234:
						case 49235:
						case 49236:
						case 49237:
						case 49238:
						case 49239:
						case 49240:
						case 49241:
						case 49242:
						case 49243:
						case 49244:
						case 49245:
						case 49246:
						case 49247:
						case 49248:
						case 49249:
						case 49250:
						case 49251:
						case 49252:
						case 49253:
						case 49254:
						case 49255:
						case 49256:
						case 49257:
						case 49258:
						case 49259:
						case 49260:
						case 49261:
						case 49262:
						case 49263:
						case 49264:
						case 49265:
						case 49284:
						case 49285:
							goto IL_60E;
						default:
							goto IL_60E;
						}
						break;
					}
					return 3;
				}
				return 2;
			}
			if (ciphersuite - 52392 > 6 && ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_60E;
			}
			return 0;
			IL_60E:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000C0DDC File Offset: 0x000BEFDC
		public static ProtocolVersion GetMinimumVersion(int ciphersuite)
		{
			if (ciphersuite <= 49202)
			{
				if (ciphersuite <= 107)
				{
					if (ciphersuite - 59 > 5 && ciphersuite - 103 > 4)
					{
						goto IL_12E;
					}
				}
				else
				{
					switch (ciphersuite)
					{
					case 156:
					case 157:
					case 158:
					case 159:
					case 160:
					case 161:
					case 162:
					case 163:
					case 164:
					case 165:
					case 168:
					case 169:
					case 170:
					case 171:
					case 172:
					case 173:
					case 186:
					case 187:
					case 188:
					case 189:
					case 190:
					case 191:
					case 192:
					case 193:
					case 194:
					case 195:
					case 196:
					case 197:
						break;
					case 166:
					case 167:
					case 174:
					case 175:
					case 176:
					case 177:
					case 178:
					case 179:
					case 180:
					case 181:
					case 182:
					case 183:
					case 184:
					case 185:
						goto IL_12E;
					default:
						if (ciphersuite - 49187 > 15)
						{
							goto IL_12E;
						}
						break;
					}
				}
			}
			else if (ciphersuite <= 49327)
			{
				if (ciphersuite - 49266 > 33 && ciphersuite - 49308 > 19)
				{
					goto IL_12E;
				}
			}
			else if (ciphersuite - 52392 > 6 && ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_12E;
			}
			return ProtocolVersion.TLSv12;
			IL_12E:
			return ProtocolVersion.SSLv3;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000C0F1C File Offset: 0x000BF11C
		public static bool IsAeadCipherSuite(int ciphersuite)
		{
			return 2 == TlsUtilities.GetCipherType(ciphersuite);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000C0F27 File Offset: 0x000BF127
		public static bool IsBlockCipherSuite(int ciphersuite)
		{
			return 1 == TlsUtilities.GetCipherType(ciphersuite);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000C0F32 File Offset: 0x000BF132
		public static bool IsStreamCipherSuite(int ciphersuite)
		{
			return TlsUtilities.GetCipherType(ciphersuite) == 0;
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000C0F3D File Offset: 0x000BF13D
		public static bool IsValidCipherSuiteForVersion(int cipherSuite, ProtocolVersion serverVersion)
		{
			return TlsUtilities.GetMinimumVersion(cipherSuite).IsEqualOrEarlierVersionOf(serverVersion.GetEquivalentTLSVersion());
		}

		// Token: 0x040019A3 RID: 6563
		public static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x040019A4 RID: 6564
		public static readonly short[] EmptyShorts = new short[0];

		// Token: 0x040019A5 RID: 6565
		public static readonly int[] EmptyInts = new int[0];

		// Token: 0x040019A6 RID: 6566
		public static readonly long[] EmptyLongs = new long[0];

		// Token: 0x040019A7 RID: 6567
		internal static readonly byte[] SSL_CLIENT = new byte[]
		{
			67,
			76,
			78,
			84
		};

		// Token: 0x040019A8 RID: 6568
		internal static readonly byte[] SSL_SERVER = new byte[]
		{
			83,
			82,
			86,
			82
		};

		// Token: 0x040019A9 RID: 6569
		internal static readonly byte[][] SSL3_CONST = TlsUtilities.GenSsl3Const();
	}
}
