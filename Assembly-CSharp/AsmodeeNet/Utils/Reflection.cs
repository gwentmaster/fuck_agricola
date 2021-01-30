using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066E RID: 1646
	public static class Reflection
	{
		// Token: 0x06003C94 RID: 15508 RVA: 0x0012B518 File Offset: 0x00129718
		public static Hashtable HashtableFromObject(object obj, HashSet<string> excludedFields = null, uint maxDepth = 30U)
		{
			object obj2 = Reflection.ParseObject(obj, "root", excludedFields, 1U, maxDepth);
			Hashtable hashtable = obj2 as Hashtable;
			if (obj2 != null && hashtable == null)
			{
				hashtable = new Hashtable
				{
					{
						"array",
						obj2
					}
				};
			}
			return hashtable;
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0012B554 File Offset: 0x00129754
		private static object ParseObject(object obj, string varPath, HashSet<string> excludedFields, uint depth, uint maxDepth)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (Reflection.CanBeLogged(obj))
			{
				return obj;
			}
			if (Reflection.IsCollection(obj))
			{
				return Reflection.ParseCollection(obj as ICollection, depth, maxDepth, varPath, excludedFields);
			}
			return Reflection.ParseUnknownObject(obj, depth, maxDepth, varPath, excludedFields);
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x0012B58C File Offset: 0x0012978C
		private static object ParseCollection(ICollection collec, uint depth, uint maxDepth, string varPath, HashSet<string> excludedFields)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (collec == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			int num = 0;
			foreach (object obj in collec)
			{
				object value = Reflection.ParseObject(obj, varPath + "." + num.ToString(), excludedFields, depth + 1U, maxDepth);
				hashtable.Add(num, value);
				num++;
			}
			return hashtable;
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x0012B61C File Offset: 0x0012981C
		private static object ParseUnknownObject(object obj, uint depth, uint maxDepth, string varPath, HashSet<string> excludedFields)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (obj == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			Type type = obj.GetType();
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				object value = type.GetField(fieldInfo.Name).GetValue(obj);
				string text = varPath + "." + fieldInfo.Name;
				if (!Reflection.PathMatchesExclusion(text, excludedFields))
				{
					object value2 = Reflection.ParseObject(value, text, excludedFields, depth + 1U, maxDepth);
					hashtable.Add(fieldInfo.Name, value2);
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				object value3 = type.GetProperty(propertyInfo.Name).GetValue(obj, null);
				string text2 = varPath + "." + propertyInfo.Name;
				if (!Reflection.PathMatchesExclusion(text2, excludedFields))
				{
					object value4 = Reflection.ParseObject(value3, text2, excludedFields, depth + 1U, maxDepth);
					hashtable.Add(propertyInfo.Name, value4);
				}
			}
			return hashtable;
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x0012B722 File Offset: 0x00129922
		public static bool CanBeLogged(object obj)
		{
			return obj != null && Reflection.CanLogType(obj.GetType());
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x0012B734 File Offset: 0x00129934
		public static bool CanLogType(Type type)
		{
			if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
			{
				return true;
			}
			if (type.IsArray)
			{
				return Reflection.CanLogType(type.GetElementType());
			}
			if (typeof(ICollection).IsAssignableFrom(type))
			{
				Type[] genericArguments = type.GetGenericArguments();
				bool result = true;
				Type[] array = genericArguments;
				for (int i = 0; i < array.Length; i++)
				{
					if (!Reflection.CanLogType(array[i]))
					{
						result = false;
					}
				}
				return result;
			}
			return false;
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x0012B7B1 File Offset: 0x001299B1
		public static bool IsCollection(object obj)
		{
			return obj != null && typeof(ICollection).IsAssignableFrom(obj.GetType());
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x0012B7D0 File Offset: 0x001299D0
		private static bool PathMatchesExclusion(string path, HashSet<string> excludedFields)
		{
			if (excludedFields == null)
			{
				return false;
			}
			foreach (string value in excludedFields)
			{
				if (path.Contains(value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
