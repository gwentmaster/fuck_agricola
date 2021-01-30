using System;
using System.Reflection;
using Fabric.Runtime.Internal;
using UnityEngine;

namespace Fabric.Runtime
{
	// Token: 0x02000256 RID: 598
	public class Fabric
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x00071EFC File Offset: 0x000700FC
		public static void Initialize()
		{
			string text = Fabric.impl.Initialize();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				Fabric.Initialize(array[i]);
			}
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00071F48 File Offset: 0x00070148
		internal static void Initialize(string kitMethod)
		{
			int num = kitMethod.LastIndexOf('.');
			string typeName = kitMethod.Substring(0, num);
			string name = kitMethod.Substring(num + 1);
			Type type = Type.GetType(typeName);
			if (type == null)
			{
				return;
			}
			MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return;
			}
			object obj = typeof(ScriptableObject).IsAssignableFrom(type) ? ScriptableObject.CreateInstance(type) : Activator.CreateInstance(type);
			if (obj == null)
			{
				return;
			}
			method.Invoke(obj, new object[0]);
		}

		// Token: 0x040012E8 RID: 4840
		private static readonly Impl impl = Impl.Make();
	}
}
