using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CC RID: 1228
	public class Asn1InputStream : FilterStream
	{
		// Token: 0x06002D97 RID: 11671 RVA: 0x000EF988 File Offset: 0x000EDB88
		internal static int FindLimit(Stream input)
		{
			if (input is LimitedInputStream)
			{
				return ((LimitedInputStream)input).GetRemaining();
			}
			if (input is MemoryStream)
			{
				MemoryStream memoryStream = (MemoryStream)input;
				return (int)(memoryStream.Length - memoryStream.Position);
			}
			return int.MaxValue;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000EF9CC File Offset: 0x000EDBCC
		public Asn1InputStream(Stream inputStream) : this(inputStream, Asn1InputStream.FindLimit(inputStream))
		{
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000EF9DB File Offset: 0x000EDBDB
		public Asn1InputStream(Stream inputStream, int limit) : base(inputStream)
		{
			this.limit = limit;
			this.tmpBuffers = new byte[16][];
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000EF9F8 File Offset: 0x000EDBF8
		public Asn1InputStream(byte[] input) : this(new MemoryStream(input, false), input.Length)
		{
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000EFA0C File Offset: 0x000EDC0C
		private Asn1Object BuildObject(int tag, int tagNo, int length)
		{
			bool flag = (tag & 32) != 0;
			DefiniteLengthInputStream definiteLengthInputStream = new DefiniteLengthInputStream(this.s, length);
			if ((tag & 64) != 0)
			{
				return new DerApplicationSpecific(flag, tagNo, definiteLengthInputStream.ToArray());
			}
			if ((tag & 128) != 0)
			{
				return new Asn1StreamParser(definiteLengthInputStream).ReadTaggedObject(flag, tagNo);
			}
			if (flag)
			{
				if (tagNo <= 8)
				{
					if (tagNo == 4)
					{
						return new BerOctetString(this.BuildDerEncodableVector(definiteLengthInputStream));
					}
					if (tagNo == 8)
					{
						return new DerExternal(this.BuildDerEncodableVector(definiteLengthInputStream));
					}
				}
				else
				{
					if (tagNo == 16)
					{
						return this.CreateDerSequence(definiteLengthInputStream);
					}
					if (tagNo == 17)
					{
						return this.CreateDerSet(definiteLengthInputStream);
					}
				}
				throw new IOException("unknown tag " + tagNo + " encountered");
			}
			return Asn1InputStream.CreatePrimitiveDerObject(tagNo, definiteLengthInputStream, this.tmpBuffers);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000EFAC8 File Offset: 0x000EDCC8
		internal Asn1EncodableVector BuildEncodableVector()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			Asn1Object asn1Object;
			while ((asn1Object = this.ReadObject()) != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					asn1Object
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000EFAFD File Offset: 0x000EDCFD
		internal virtual Asn1EncodableVector BuildDerEncodableVector(DefiniteLengthInputStream dIn)
		{
			return new Asn1InputStream(dIn).BuildEncodableVector();
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000EFB0A File Offset: 0x000EDD0A
		internal virtual DerSequence CreateDerSequence(DefiniteLengthInputStream dIn)
		{
			return DerSequence.FromVector(this.BuildDerEncodableVector(dIn));
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000EFB18 File Offset: 0x000EDD18
		internal virtual DerSet CreateDerSet(DefiniteLengthInputStream dIn)
		{
			return DerSet.FromVector(this.BuildDerEncodableVector(dIn), false);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000EFB28 File Offset: 0x000EDD28
		public Asn1Object ReadObject()
		{
			int num = this.ReadByte();
			if (num <= 0)
			{
				if (num == 0)
				{
					throw new IOException("unexpected end-of-contents marker");
				}
				return null;
			}
			else
			{
				int num2 = Asn1InputStream.ReadTagNumber(this.s, num);
				bool flag = (num & 32) != 0;
				int num3 = Asn1InputStream.ReadLength(this.s, this.limit);
				if (num3 >= 0)
				{
					Asn1Object result;
					try
					{
						result = this.BuildObject(num, num2, num3);
					}
					catch (ArgumentException exception)
					{
						throw new Asn1Exception("corrupted stream detected", exception);
					}
					return result;
				}
				if (!flag)
				{
					throw new IOException("indefinite length primitive encoding encountered");
				}
				Asn1StreamParser parser = new Asn1StreamParser(new IndefiniteLengthInputStream(this.s, this.limit), this.limit);
				if ((num & 64) != 0)
				{
					return new BerApplicationSpecificParser(num2, parser).ToAsn1Object();
				}
				if ((num & 128) != 0)
				{
					return new BerTaggedObjectParser(true, num2, parser).ToAsn1Object();
				}
				if (num2 <= 8)
				{
					if (num2 == 4)
					{
						return new BerOctetStringParser(parser).ToAsn1Object();
					}
					if (num2 == 8)
					{
						return new DerExternalParser(parser).ToAsn1Object();
					}
				}
				else
				{
					if (num2 == 16)
					{
						return new BerSequenceParser(parser).ToAsn1Object();
					}
					if (num2 == 17)
					{
						return new BerSetParser(parser).ToAsn1Object();
					}
				}
				throw new IOException("unknown BER object encountered");
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000EFC60 File Offset: 0x000EDE60
		internal static int ReadTagNumber(Stream s, int tag)
		{
			int num = tag & 31;
			if (num == 31)
			{
				num = 0;
				int num2 = s.ReadByte();
				if ((num2 & 127) == 0)
				{
					throw new IOException("Corrupted stream - invalid high tag number found");
				}
				while (num2 >= 0 && (num2 & 128) != 0)
				{
					num |= (num2 & 127);
					num <<= 7;
					num2 = s.ReadByte();
				}
				if (num2 < 0)
				{
					throw new EndOfStreamException("EOF found inside tag value.");
				}
				num |= (num2 & 127);
			}
			return num;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000EFCC8 File Offset: 0x000EDEC8
		internal static int ReadLength(Stream s, int limit)
		{
			int num = s.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException("EOF found when length expected");
			}
			if (num == 128)
			{
				return -1;
			}
			if (num > 127)
			{
				int num2 = num & 127;
				if (num2 > 4)
				{
					throw new IOException("DER length more than 4 bytes: " + num2);
				}
				num = 0;
				for (int i = 0; i < num2; i++)
				{
					int num3 = s.ReadByte();
					if (num3 < 0)
					{
						throw new EndOfStreamException("EOF found reading length");
					}
					num = (num << 8) + num3;
				}
				if (num < 0)
				{
					throw new IOException("Corrupted stream - negative length found");
				}
				if (num >= limit)
				{
					throw new IOException("Corrupted stream - out of bounds length found");
				}
			}
			return num;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000EFD64 File Offset: 0x000EDF64
		internal static byte[] GetBuffer(DefiniteLengthInputStream defIn, byte[][] tmpBuffers)
		{
			int remaining = defIn.GetRemaining();
			if (remaining >= tmpBuffers.Length)
			{
				return defIn.ToArray();
			}
			byte[] array = tmpBuffers[remaining];
			if (array == null)
			{
				array = (tmpBuffers[remaining] = new byte[remaining]);
			}
			defIn.ReadAllIntoByteArray(array);
			return array;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000EFDA4 File Offset: 0x000EDFA4
		internal static Asn1Object CreatePrimitiveDerObject(int tagNo, DefiniteLengthInputStream defIn, byte[][] tmpBuffers)
		{
			if (tagNo == 1)
			{
				return DerBoolean.FromOctetString(Asn1InputStream.GetBuffer(defIn, tmpBuffers));
			}
			if (tagNo == 6)
			{
				return DerObjectIdentifier.FromOctetString(Asn1InputStream.GetBuffer(defIn, tmpBuffers));
			}
			if (tagNo != 10)
			{
				byte[] array = defIn.ToArray();
				switch (tagNo)
				{
				case 2:
					return new DerInteger(array);
				case 3:
					return DerBitString.FromAsn1Octets(array);
				case 4:
					return new DerOctetString(array);
				case 5:
					return DerNull.Instance;
				case 12:
					return new DerUtf8String(array);
				case 18:
					return new DerNumericString(array);
				case 19:
					return new DerPrintableString(array);
				case 20:
					return new DerT61String(array);
				case 21:
					return new DerVideotexString(array);
				case 22:
					return new DerIA5String(array);
				case 23:
					return new DerUtcTime(array);
				case 24:
					return new DerGeneralizedTime(array);
				case 25:
					return new DerGraphicString(array);
				case 26:
					return new DerVisibleString(array);
				case 27:
					return new DerGeneralString(array);
				case 28:
					return new DerUniversalString(array);
				case 30:
					return new DerBmpString(array);
				}
				throw new IOException("unknown tag " + tagNo + " encountered");
			}
			return DerEnumerated.FromOctetString(Asn1InputStream.GetBuffer(defIn, tmpBuffers));
		}

		// Token: 0x04001DE4 RID: 7652
		private readonly int limit;

		// Token: 0x04001DE5 RID: 7653
		private readonly byte[][] tmpBuffers;
	}
}
