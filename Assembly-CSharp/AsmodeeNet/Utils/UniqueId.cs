using System;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000676 RID: 1654
	public static class UniqueId
	{
		// Token: 0x06003CC9 RID: 15561 RVA: 0x0012BDA0 File Offset: 0x00129FA0
		public static string GetUniqueId()
		{
			string text;
			if (!KeyValueStore.HasKey("key"))
			{
				text = Guid.NewGuid().ToString("N") + "OAuthGate()#|@^*%§!?:;.,$~안녕하세요";
				text = UniqueId._Shuffle(text, UnityEngine.Random.Range(0, int.MaxValue));
				KeyValueStore.SetString("key", text);
				KeyValueStore.Save();
			}
			else
			{
				text = KeyValueStore.GetString("key", "");
			}
			return UniqueId._Shuffle(text, (from x in CoreApplication.GetUserAgent()
			select (int)x).Sum());
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x0012BE40 File Offset: 0x0012A040
		private static string _Shuffle(string s, int seed)
		{
			UnityEngine.Random.InitState(seed);
			byte[] array = UniqueId._GetBytes(s);
			for (int i = array.Length - 1; i >= 1; i--)
			{
				int num = UnityEngine.Random.Range(0, i);
				byte b = array[num];
				array[num] = array[i];
				array[i] = b;
			}
			return UniqueId._GetString(array);
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x0012BE88 File Offset: 0x0012A088
		private static byte[] _GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0012BEB8 File Offset: 0x0012A0B8
		private static string _GetString(byte[] bytes)
		{
			char[] array = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '\0')
				{
					char[] array2 = array;
					int num = i;
					array2[num] += '\u0001';
				}
			}
			return new string(array);
		}

		// Token: 0x04002709 RID: 9993
		private const string _kKey = "key";
	}
}
