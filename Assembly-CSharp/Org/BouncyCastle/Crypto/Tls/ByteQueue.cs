using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039D RID: 925
	public class ByteQueue
	{
		// Token: 0x060022F2 RID: 8946 RVA: 0x000B63E2 File Offset: 0x000B45E2
		public static int NextTwoPow(int i)
		{
			i |= i >> 1;
			i |= i >> 2;
			i |= i >> 4;
			i |= i >> 8;
			i |= i >> 16;
			return i + 1;
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000B640B File Offset: 0x000B460B
		public ByteQueue() : this(1024)
		{
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000B6418 File Offset: 0x000B4618
		public ByteQueue(int capacity)
		{
			this.databuf = new byte[capacity];
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000B642C File Offset: 0x000B462C
		public void Read(byte[] buf, int offset, int len, int skip)
		{
			if (buf.Length - offset < len)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Buffer size of ",
					buf.Length,
					" is too small for a read of ",
					len,
					" bytes"
				}));
			}
			if (this.available - skip < len)
			{
				throw new InvalidOperationException("Not enough data to read");
			}
			Array.Copy(this.databuf, this.skipped + skip, buf, offset, len);
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000B64AC File Offset: 0x000B46AC
		public void AddData(byte[] data, int offset, int len)
		{
			if (this.skipped + this.available + len > this.databuf.Length)
			{
				int num = ByteQueue.NextTwoPow(this.available + len);
				if (num > this.databuf.Length)
				{
					byte[] destinationArray = new byte[num];
					Array.Copy(this.databuf, this.skipped, destinationArray, 0, this.available);
					this.databuf = destinationArray;
				}
				else
				{
					Array.Copy(this.databuf, this.skipped, this.databuf, 0, this.available);
				}
				this.skipped = 0;
			}
			Array.Copy(data, offset, this.databuf, this.skipped + this.available, len);
			this.available += len;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000B6564 File Offset: 0x000B4764
		public void RemoveData(int i)
		{
			if (i > this.available)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot remove ",
					i,
					" bytes, only got ",
					this.available
				}));
			}
			this.available -= i;
			this.skipped += i;
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000B65CE File Offset: 0x000B47CE
		public void RemoveData(byte[] buf, int off, int len, int skip)
		{
			this.Read(buf, off, len, skip);
			this.RemoveData(skip + len);
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000B65E8 File Offset: 0x000B47E8
		public byte[] RemoveData(int len, int skip)
		{
			byte[] array = new byte[len];
			this.RemoveData(array, 0, len, skip);
			return array;
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000B6607 File Offset: 0x000B4807
		public int Available
		{
			get
			{
				return this.available;
			}
		}

		// Token: 0x040016D9 RID: 5849
		private const int DefaultCapacity = 1024;

		// Token: 0x040016DA RID: 5850
		private byte[] databuf;

		// Token: 0x040016DB RID: 5851
		private int skipped;

		// Token: 0x040016DC RID: 5852
		private int available;
	}
}
