using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004ED RID: 1261
	internal class DefiniteLengthInputStream : LimitedInputStream
	{
		// Token: 0x06002E5E RID: 11870 RVA: 0x000F1A53 File Offset: 0x000EFC53
		internal DefiniteLengthInputStream(Stream inStream, int length) : base(inStream, length)
		{
			if (length < 0)
			{
				throw new ArgumentException("negative lengths not allowed", "length");
			}
			this._originalLength = length;
			this._remaining = length;
			if (length == 0)
			{
				this.SetParentEofDetect(true);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002E5F RID: 11871 RVA: 0x000F1A89 File Offset: 0x000EFC89
		internal int Remaining
		{
			get
			{
				return this._remaining;
			}
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000F1A94 File Offset: 0x000EFC94
		public override int ReadByte()
		{
			if (this._remaining == 0)
			{
				return -1;
			}
			int num = this._in.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			int num2 = this._remaining - 1;
			this._remaining = num2;
			if (num2 == 0)
			{
				this.SetParentEofDetect(true);
			}
			return num;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000F1B14 File Offset: 0x000EFD14
		public override int Read(byte[] buf, int off, int len)
		{
			if (this._remaining == 0)
			{
				return 0;
			}
			int count = Math.Min(len, this._remaining);
			int num = this._in.Read(buf, off, count);
			if (num < 1)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			if ((this._remaining -= num) == 0)
			{
				this.SetParentEofDetect(true);
			}
			return num;
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000F1BA4 File Offset: 0x000EFDA4
		internal void ReadAllIntoByteArray(byte[] buf)
		{
			if (this._remaining != buf.Length)
			{
				throw new ArgumentException("buffer length not right for data");
			}
			if ((this._remaining -= Streams.ReadFully(this._in, buf)) != 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			this.SetParentEofDetect(true);
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000F1C2C File Offset: 0x000EFE2C
		internal byte[] ToArray()
		{
			if (this._remaining == 0)
			{
				return DefiniteLengthInputStream.EmptyBytes;
			}
			byte[] array = new byte[this._remaining];
			if ((this._remaining -= Streams.ReadFully(this._in, array)) != 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			this.SetParentEofDetect(true);
			return array;
		}

		// Token: 0x04001E27 RID: 7719
		private static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x04001E28 RID: 7720
		private readonly int _originalLength;

		// Token: 0x04001E29 RID: 7721
		private int _remaining;
	}
}
