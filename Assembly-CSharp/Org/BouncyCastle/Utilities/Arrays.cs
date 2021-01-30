using System;
using System.Text;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000283 RID: 643
	public abstract class Arrays
	{
		// Token: 0x06001526 RID: 5414 RVA: 0x00078EBC File Offset: 0x000770BC
		public static bool AreEqual(bool[] a, bool[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00078ED3 File Offset: 0x000770D3
		public static bool AreEqual(char[] a, char[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00078EEA File Offset: 0x000770EA
		public static bool AreEqual(byte[] a, byte[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00078F01 File Offset: 0x00077101
		[Obsolete("Use 'AreEqual' method instead")]
		public static bool AreSame(byte[] a, byte[] b)
		{
			return Arrays.AreEqual(a, b);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00078F0C File Offset: 0x0007710C
		public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			int num2 = 0;
			while (num != 0)
			{
				num--;
				num2 |= (int)(a[num] ^ b[num]);
			}
			return num2 == 0;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00078F3E File Offset: 0x0007713E
		public static bool AreEqual(int[] a, int[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00078F55 File Offset: 0x00077155
		public static bool AreEqual(uint[] a, uint[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00078F6C File Offset: 0x0007716C
		private static bool HaveSameContents(bool[] a, bool[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00078F98 File Offset: 0x00077198
		private static bool HaveSameContents(char[] a, char[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00078FC4 File Offset: 0x000771C4
		private static bool HaveSameContents(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00078FF0 File Offset: 0x000771F0
		private static bool HaveSameContents(int[] a, int[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0007901C File Offset: 0x0007721C
		private static bool HaveSameContents(uint[] a, uint[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00079048 File Offset: 0x00077248
		public static string ToString(object[] a)
		{
			StringBuilder stringBuilder = new StringBuilder(91);
			if (a.Length != 0)
			{
				stringBuilder.Append(a[0]);
				for (int i = 1; i < a.Length; i++)
				{
					stringBuilder.Append(", ").Append(a[i]);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0007909C File Offset: 0x0007729C
		public static int GetHashCode(byte[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000790D0 File Offset: 0x000772D0
		public static int GetHashCode(byte[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[off + num];
			}
			return num2;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00079104 File Offset: 0x00077304
		public static int GetHashCode(int[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[num];
			}
			return num2;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00079138 File Offset: 0x00077338
		public static int GetHashCode(int[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[off + num];
			}
			return num2;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0007916C File Offset: 0x0007736C
		public static int GetHashCode(uint[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000791A0 File Offset: 0x000773A0
		public static int GetHashCode(uint[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[off + num];
			}
			return num2;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000791D4 File Offset: 0x000773D4
		public static int GetHashCode(ulong[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				ulong num3 = data[num];
				num2 *= 257;
				num2 ^= (int)num3;
				num2 *= 257;
				num2 ^= (int)(num3 >> 32);
			}
			return num2;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0007921C File Offset: 0x0007741C
		public static int GetHashCode(ulong[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				ulong num3 = data[off + num];
				num2 *= 257;
				num2 ^= (int)num3;
				num2 *= 257;
				num2 ^= (int)(num3 >> 32);
			}
			return num2;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00079262 File Offset: 0x00077462
		public static byte[] Clone(byte[] data)
		{
			if (data != null)
			{
				return (byte[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00079274 File Offset: 0x00077474
		public static byte[] Clone(byte[] data, byte[] existing)
		{
			if (data == null)
			{
				return null;
			}
			if (existing == null || existing.Length != data.Length)
			{
				return Arrays.Clone(data);
			}
			Array.Copy(data, 0, existing, 0, existing.Length);
			return existing;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0007929A File Offset: 0x0007749A
		public static int[] Clone(int[] data)
		{
			if (data != null)
			{
				return (int[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x000792AC File Offset: 0x000774AC
		internal static uint[] Clone(uint[] data)
		{
			if (data != null)
			{
				return (uint[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x000792BE File Offset: 0x000774BE
		public static long[] Clone(long[] data)
		{
			if (data != null)
			{
				return (long[])data.Clone();
			}
			return null;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000792D0 File Offset: 0x000774D0
		public static ulong[] Clone(ulong[] data)
		{
			if (data != null)
			{
				return (ulong[])data.Clone();
			}
			return null;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000792E2 File Offset: 0x000774E2
		public static ulong[] Clone(ulong[] data, ulong[] existing)
		{
			if (data == null)
			{
				return null;
			}
			if (existing == null || existing.Length != data.Length)
			{
				return Arrays.Clone(data);
			}
			Array.Copy(data, 0, existing, 0, existing.Length);
			return existing;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00079308 File Offset: 0x00077508
		public static bool Contains(byte[] a, byte n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0007932C File Offset: 0x0007752C
		public static bool Contains(short[] a, short n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00079350 File Offset: 0x00077550
		public static bool Contains(int[] a, int n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00079374 File Offset: 0x00077574
		public static void Fill(byte[] buf, byte b)
		{
			int i = buf.Length;
			while (i > 0)
			{
				buf[--i] = b;
			}
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00079394 File Offset: 0x00077594
		public static byte[] CopyOf(byte[] data, int newLength)
		{
			byte[] array = new byte[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x000793BC File Offset: 0x000775BC
		public static char[] CopyOf(char[] data, int newLength)
		{
			char[] array = new char[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x000793E4 File Offset: 0x000775E4
		public static int[] CopyOf(int[] data, int newLength)
		{
			int[] array = new int[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0007940C File Offset: 0x0007760C
		public static long[] CopyOf(long[] data, int newLength)
		{
			long[] array = new long[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00079434 File Offset: 0x00077634
		public static BigInteger[] CopyOf(BigInteger[] data, int newLength)
		{
			BigInteger[] array = new BigInteger[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0007945C File Offset: 0x0007765C
		public static byte[] CopyOfRange(byte[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			byte[] array = new byte[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00079490 File Offset: 0x00077690
		public static int[] CopyOfRange(int[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			int[] array = new int[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x000794C4 File Offset: 0x000776C4
		public static long[] CopyOfRange(long[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			long[] array = new long[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x000794F8 File Offset: 0x000776F8
		public static BigInteger[] CopyOfRange(BigInteger[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			BigInteger[] array = new BigInteger[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00079529 File Offset: 0x00077729
		private static int GetLength(int from, int to)
		{
			int num = to - from;
			if (num < 0)
			{
				throw new ArgumentException(from + " > " + to);
			}
			return num;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00079550 File Offset: 0x00077750
		public static byte[] Append(byte[] a, byte b)
		{
			if (a == null)
			{
				return new byte[]
				{
					b
				};
			}
			int num = a.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00079588 File Offset: 0x00077788
		public static short[] Append(short[] a, short b)
		{
			if (a == null)
			{
				return new short[]
				{
					b
				};
			}
			int num = a.Length;
			short[] array = new short[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x000795C0 File Offset: 0x000777C0
		public static int[] Append(int[] a, int b)
		{
			if (a == null)
			{
				return new int[]
				{
					b
				};
			}
			int num = a.Length;
			int[] array = new int[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000795F8 File Offset: 0x000777F8
		public static byte[] Concatenate(byte[] a, byte[] b)
		{
			if (a == null)
			{
				return Arrays.Clone(b);
			}
			if (b == null)
			{
				return Arrays.Clone(a);
			}
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00079644 File Offset: 0x00077844
		public static byte[] ConcatenateAll(params byte[][] vs)
		{
			byte[][] array = new byte[vs.Length][];
			int num = 0;
			int num2 = 0;
			foreach (byte[] array2 in vs)
			{
				if (array2 != null)
				{
					array[num++] = array2;
					num2 += array2.Length;
				}
			}
			byte[] array3 = new byte[num2];
			int num3 = 0;
			for (int j = 0; j < num; j++)
			{
				byte[] array4 = array[j];
				Array.Copy(array4, 0, array3, num3, array4.Length);
				num3 += array4.Length;
			}
			return array3;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x000796C4 File Offset: 0x000778C4
		public static int[] Concatenate(int[] a, int[] b)
		{
			if (a == null)
			{
				return Arrays.Clone(b);
			}
			if (b == null)
			{
				return Arrays.Clone(a);
			}
			int[] array = new int[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00079710 File Offset: 0x00077910
		public static byte[] Prepend(byte[] a, byte b)
		{
			if (a == null)
			{
				return new byte[]
				{
					b
				};
			}
			int num = a.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00079748 File Offset: 0x00077948
		public static short[] Prepend(short[] a, short b)
		{
			if (a == null)
			{
				return new short[]
				{
					b
				};
			}
			int num = a.Length;
			short[] array = new short[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00079780 File Offset: 0x00077980
		public static int[] Prepend(int[] a, int b)
		{
			if (a == null)
			{
				return new int[]
				{
					b
				};
			}
			int num = a.Length;
			int[] array = new int[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x000797B8 File Offset: 0x000779B8
		public static byte[] Reverse(byte[] a)
		{
			if (a == null)
			{
				return null;
			}
			int num = 0;
			int num2 = a.Length;
			byte[] array = new byte[num2];
			while (--num2 >= 0)
			{
				array[num2] = a[num++];
			}
			return array;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000797EC File Offset: 0x000779EC
		public static int[] Reverse(int[] a)
		{
			if (a == null)
			{
				return null;
			}
			int num = 0;
			int num2 = a.Length;
			int[] array = new int[num2];
			while (--num2 >= 0)
			{
				array[num2] = a[num++];
			}
			return array;
		}
	}
}
