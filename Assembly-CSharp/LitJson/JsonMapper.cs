using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LitJson
{
	// Token: 0x0200026F RID: 623
	public class JsonMapper
	{
		// Token: 0x060013FB RID: 5115 RVA: 0x000736C4 File Offset: 0x000718C4
		static JsonMapper()
		{
			JsonMapper.max_nesting_depth = 100;
			JsonMapper.array_metadata = new Dictionary<Type, ArrayMetadata>();
			JsonMapper.conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
			JsonMapper.object_metadata = new Dictionary<Type, ObjectMetadata>();
			JsonMapper.type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
			JsonMapper.static_writer = new JsonWriter();
			JsonMapper.datetime_format = DateTimeFormatInfo.InvariantInfo;
			JsonMapper.base_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.custom_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.RegisterBaseExporters();
			JsonMapper.RegisterBaseImporters();
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00073778 File Offset: 0x00071978
		private static bool HasInterface(Type type, string name)
		{
			return type.GetInterface(name, true) != null;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00073788 File Offset: 0x00071988
		public static PropertyInfo[] GetPublicInstanceProperties(Type type)
		{
			return type.GetProperties();
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00073790 File Offset: 0x00071990
		private static void AddArrayMetadata(Type type)
		{
			if (JsonMapper.array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata value = default(ArrayMetadata);
			value.IsArray = type.IsArray;
			if (JsonMapper.HasInterface(type, "System.Collections.IList"))
			{
				value.IsList = true;
			}
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(int))
					{
						value.ElementType = propertyInfo.PropertyType;
					}
				}
			}
			object obj = JsonMapper.array_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.array_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00073884 File Offset: 0x00071A84
		private static void AddObjectMetadata(Type type)
		{
			if (JsonMapper.object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata value = default(ObjectMetadata);
			if (JsonMapper.HasInterface(type, "System.Collections.IDictionary"))
			{
				value.IsDictionary = true;
			}
			value.Properties = new Dictionary<string, PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(string))
					{
						value.ElementType = propertyInfo.PropertyType;
					}
				}
				else
				{
					PropertyMetadata value2 = default(PropertyMetadata);
					value2.Info = propertyInfo;
					value2.Type = propertyInfo.PropertyType;
					value.Properties.Add(propertyInfo.Name, value2);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				PropertyMetadata value3 = default(PropertyMetadata);
				value3.Info = fieldInfo;
				value3.IsField = true;
				value3.Type = fieldInfo.FieldType;
				value.Properties.Add(fieldInfo.Name, value3);
			}
			object obj = JsonMapper.object_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.object_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00073A08 File Offset: 0x00071C08
		private static void AddTypeProperties(Type type)
		{
			if (JsonMapper.type_properties.ContainsKey(type))
			{
				return;
			}
			IList<PropertyMetadata> list = new List<PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (!(propertyInfo.Name == "Item"))
				{
					list.Add(new PropertyMetadata
					{
						Info = propertyInfo,
						IsField = false
					});
				}
			}
			foreach (FieldInfo info in type.GetFields())
			{
				list.Add(new PropertyMetadata
				{
					Info = info,
					IsField = true
				});
			}
			object obj = JsonMapper.type_properties_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.type_properties.Add(type, list);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00073B00 File Offset: 0x00071D00
		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			object obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				if (!JsonMapper.conv_ops.ContainsKey(t1))
				{
					JsonMapper.conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			if (JsonMapper.conv_ops[t1].ContainsKey(t2))
			{
				return JsonMapper.conv_ops[t1][t2];
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new Type[]
			{
				t2
			});
			obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.conv_ops[t1].Add(t2, method);
				}
				catch (ArgumentException)
				{
					return JsonMapper.conv_ops[t1][t2];
				}
			}
			return method;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00073BF0 File Offset: 0x00071DF0
		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			if (reader.Token == JsonToken.Null)
			{
				if (!inst_type.IsClass)
				{
					throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
				}
				return null;
			}
			else
			{
				if (reader.Token != JsonToken.Double && reader.Token != JsonToken.Int && reader.Token != JsonToken.Long && reader.Token != JsonToken.String && reader.Token != JsonToken.Boolean)
				{
					object obj = null;
					if (reader.Token == JsonToken.ArrayStart)
					{
						if (inst_type.FullName == "System.Object")
						{
							inst_type = typeof(object[]);
						}
						JsonMapper.AddArrayMetadata(inst_type);
						ArrayMetadata arrayMetadata = JsonMapper.array_metadata[inst_type];
						if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
						{
							throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
						}
						IList list;
						Type elementType;
						if (!arrayMetadata.IsArray)
						{
							list = (IList)Activator.CreateInstance(inst_type);
							elementType = arrayMetadata.ElementType;
						}
						else
						{
							list = new ArrayList();
							elementType = inst_type.GetElementType();
						}
						for (;;)
						{
							object obj2 = JsonMapper.ReadValue(elementType, reader);
							if (obj2 == null && reader.Token == JsonToken.ArrayEnd)
							{
								break;
							}
							list.Add(obj2);
						}
						if (arrayMetadata.IsArray)
						{
							int count = list.Count;
							obj = Array.CreateInstance(elementType, count);
							for (int i = 0; i < count; i++)
							{
								((Array)obj).SetValue(list[i], i);
							}
						}
						else
						{
							obj = list;
						}
					}
					else if (reader.Token == JsonToken.ObjectStart)
					{
						if (inst_type == typeof(object))
						{
							inst_type = typeof(Dictionary<string, object>);
						}
						JsonMapper.AddObjectMetadata(inst_type);
						ObjectMetadata objectMetadata = JsonMapper.object_metadata[inst_type];
						obj = Activator.CreateInstance(inst_type);
						string text;
						for (;;)
						{
							reader.Read();
							if (reader.Token == JsonToken.ObjectEnd)
							{
								return obj;
							}
							text = (string)reader.Value;
							if (objectMetadata.Properties.ContainsKey(text))
							{
								PropertyMetadata propertyMetadata = objectMetadata.Properties[text];
								if (propertyMetadata.IsField)
								{
									((FieldInfo)propertyMetadata.Info).SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader));
								}
								else
								{
									PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
									if (propertyInfo.CanWrite)
									{
										propertyInfo.SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader), null);
									}
									else
									{
										JsonMapper.ReadValue(propertyMetadata.Type, reader);
									}
								}
							}
							else if (!objectMetadata.IsDictionary)
							{
								if (!reader.SkipNonMembers)
								{
									break;
								}
								JsonMapper.ReadSkip(reader);
							}
							else
							{
								((IDictionary)obj).Add(text, JsonMapper.ReadValue(objectMetadata.ElementType, reader));
							}
						}
						throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, text));
					}
					return obj;
				}
				Type type = reader.Value.GetType();
				if (inst_type.IsAssignableFrom(type))
				{
					return reader.Value;
				}
				if (JsonMapper.custom_importers_table.ContainsKey(type) && JsonMapper.custom_importers_table[type].ContainsKey(inst_type))
				{
					return JsonMapper.custom_importers_table[type][inst_type](reader.Value);
				}
				if (JsonMapper.base_importers_table.ContainsKey(type) && JsonMapper.base_importers_table[type].ContainsKey(inst_type))
				{
					return JsonMapper.base_importers_table[type][inst_type](reader.Value);
				}
				if (inst_type.IsEnum)
				{
					return Enum.ToObject(inst_type, reader.Value);
				}
				MethodInfo convOp = JsonMapper.GetConvOp(inst_type, type);
				if (convOp != null)
				{
					return convOp.Invoke(null, new object[]
					{
						reader.Value
					});
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, type, inst_type));
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00073F98 File Offset: 0x00072198
		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper jsonWrapper = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrapper.SetString((string)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrapper.SetDouble((double)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrapper.SetInt((int)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrapper.SetLong((long)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrapper.SetBoolean((bool)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrapper.SetJsonType(JsonType.Array);
				for (;;)
				{
					IJsonWrapper jsonWrapper2 = JsonMapper.ReadValue(factory, reader);
					if (jsonWrapper2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					jsonWrapper.Add(jsonWrapper2);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrapper.SetJsonType(JsonType.Object);
				for (;;)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string key = (string)reader.Value;
					jsonWrapper[key] = JsonMapper.ReadValue(factory, reader);
				}
			}
			return jsonWrapper;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000740C1 File Offset: 0x000722C1
		private static void ReadSkip(JsonReader reader)
		{
			JsonMapper.ToWrapper(() => new JsonMockWrapper(), reader);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000740EC File Offset: 0x000722EC
		private static void RegisterBaseExporters()
		{
			JsonMapper.base_exporters_table[typeof(byte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((byte)obj));
			};
			JsonMapper.base_exporters_table[typeof(char)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((char)obj));
			};
			JsonMapper.base_exporters_table[typeof(DateTime)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((DateTime)obj, JsonMapper.datetime_format));
			};
			JsonMapper.base_exporters_table[typeof(decimal)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((decimal)obj);
			};
			JsonMapper.base_exporters_table[typeof(sbyte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((sbyte)obj));
			};
			JsonMapper.base_exporters_table[typeof(short)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((short)obj));
			};
			JsonMapper.base_exporters_table[typeof(ushort)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((ushort)obj));
			};
			JsonMapper.base_exporters_table[typeof(uint)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToUInt64((uint)obj));
			};
			JsonMapper.base_exporters_table[typeof(ulong)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((ulong)obj);
			};
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000742C4 File Offset: 0x000724C4
		private static void RegisterBaseImporters()
		{
			ImporterFunc importer = (object input) => Convert.ToByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(byte), importer);
			importer = ((object input) => Convert.ToUInt64((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ulong), importer);
			importer = ((object input) => Convert.ToSByte((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(sbyte), importer);
			importer = ((object input) => Convert.ToInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(short), importer);
			importer = ((object input) => Convert.ToUInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ushort), importer);
			importer = ((object input) => Convert.ToUInt32((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(uint), importer);
			importer = ((object input) => Convert.ToSingle((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(float), importer);
			importer = ((object input) => Convert.ToDouble((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(double), importer);
			importer = ((object input) => Convert.ToDecimal((double)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(decimal), importer);
			importer = ((object input) => Convert.ToUInt32((long)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(long), typeof(uint), importer);
			importer = ((object input) => Convert.ToChar((string)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(char), importer);
			importer = ((object input) => Convert.ToDateTime((string)input, JsonMapper.datetime_format));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTime), importer);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000745C5 File Offset: 0x000727C5
		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000745EC File Offset: 0x000727EC
		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > JsonMapper.max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
					return;
				}
				((IJsonWrapper)obj).ToJson(writer);
				return;
			}
			else
			{
				if (obj is string)
				{
					writer.Write((string)obj);
					return;
				}
				if (obj is double)
				{
					writer.Write((double)obj);
					return;
				}
				if (obj is int)
				{
					writer.Write((int)obj);
					return;
				}
				if (obj is bool)
				{
					writer.Write((bool)obj);
					return;
				}
				if (obj is long)
				{
					writer.Write((long)obj);
					return;
				}
				if (obj is Array)
				{
					writer.WriteArrayStart();
					foreach (object obj2 in ((Array)obj))
					{
						JsonMapper.WriteValue(obj2, writer, writer_is_private, depth + 1);
					}
					writer.WriteArrayEnd();
					return;
				}
				if (obj is IList)
				{
					writer.WriteArrayStart();
					foreach (object obj3 in ((IList)obj))
					{
						JsonMapper.WriteValue(obj3, writer, writer_is_private, depth + 1);
					}
					writer.WriteArrayEnd();
					return;
				}
				if (obj is IDictionary)
				{
					writer.WriteObjectStart();
					foreach (object obj4 in ((IDictionary)obj))
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj4;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonMapper.WriteValue(dictionaryEntry.Value, writer, writer_is_private, depth + 1);
					}
					writer.WriteObjectEnd();
					return;
				}
				Type type = obj.GetType();
				if (JsonMapper.custom_exporters_table.ContainsKey(type))
				{
					JsonMapper.custom_exporters_table[type](obj, writer);
					return;
				}
				if (JsonMapper.base_exporters_table.ContainsKey(type))
				{
					JsonMapper.base_exporters_table[type](obj, writer);
					return;
				}
				if (!(obj is Enum))
				{
					JsonMapper.AddTypeProperties(type);
					IEnumerable<PropertyMetadata> enumerable = JsonMapper.type_properties[type];
					writer.WriteObjectStart();
					foreach (PropertyMetadata propertyMetadata in enumerable)
					{
						if (propertyMetadata.IsField)
						{
							writer.WritePropertyName(propertyMetadata.Info.Name);
							JsonMapper.WriteValue(((FieldInfo)propertyMetadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
						}
						else
						{
							PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
							if (propertyInfo.CanRead)
							{
								writer.WritePropertyName(propertyMetadata.Info.Name);
								JsonMapper.WriteValue(propertyInfo.GetValue(obj, null), writer, writer_is_private, depth + 1);
							}
						}
					}
					writer.WriteObjectEnd();
					return;
				}
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (underlyingType == typeof(long) || underlyingType == typeof(uint) || underlyingType == typeof(ulong))
				{
					writer.Write((ulong)obj);
					return;
				}
				writer.Write((int)obj);
				return;
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00074960 File Offset: 0x00072B60
		public static string ToJson(object obj)
		{
			object obj2 = JsonMapper.static_writer_lock;
			string result;
			lock (obj2)
			{
				JsonMapper.static_writer.Reset();
				JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
				result = JsonMapper.static_writer.ToString();
			}
			return result;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x000749BC File Offset: 0x00072BBC
		public static void ToJson(object obj, JsonWriter writer)
		{
			JsonMapper.WriteValue(obj, writer, false, 0);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x000749C7 File Offset: 0x00072BC7
		public static JsonData ToObject(JsonReader reader)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x000749F4 File Offset: 0x00072BF4
		public static JsonData ToObject(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader2);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00074A32 File Offset: 0x00072C32
		public static JsonData ToObject(string json)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00074A5E File Offset: 0x00072C5E
		public static T ToObject<T>(JsonReader reader)
		{
			return (!!0)((object)JsonMapper.ReadValue(typeof(!!0), reader));
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00074A78 File Offset: 0x00072C78
		public static T ToObject<T>(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (!!0)((object)JsonMapper.ReadValue(typeof(!!0), reader2));
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00074AA4 File Offset: 0x00072CA4
		public static T ToObject<T>(string json)
		{
			JsonReader reader = new JsonReader(json);
			return (!!0)((object)JsonMapper.ReadValue(typeof(!!0), reader));
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00074ACD File Offset: 0x00072CCD
		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00074AD8 File Offset: 0x00072CD8
		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader reader = new JsonReader(json);
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00074AF4 File Offset: 0x00072CF4
		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			ExporterFunc value = delegate(object obj, JsonWriter writer)
			{
				exporter((!0)((object)obj), writer);
			};
			JsonMapper.custom_exporters_table[typeof(!!0)] = value;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00074B30 File Offset: 0x00072D30
		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			ImporterFunc importer2 = (object input) => importer((!0)((object)input));
			JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof(!!0), typeof(!!1), importer2);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x00074B74 File Offset: 0x00072D74
		public static void UnregisterExporters()
		{
			JsonMapper.custom_exporters_table.Clear();
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00074B80 File Offset: 0x00072D80
		public static void UnregisterImporters()
		{
			JsonMapper.custom_importers_table.Clear();
		}

		// Token: 0x04001311 RID: 4881
		private static int max_nesting_depth;

		// Token: 0x04001312 RID: 4882
		private static IFormatProvider datetime_format;

		// Token: 0x04001313 RID: 4883
		private static IDictionary<Type, ExporterFunc> base_exporters_table;

		// Token: 0x04001314 RID: 4884
		private static IDictionary<Type, ExporterFunc> custom_exporters_table;

		// Token: 0x04001315 RID: 4885
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

		// Token: 0x04001316 RID: 4886
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;

		// Token: 0x04001317 RID: 4887
		private static IDictionary<Type, ArrayMetadata> array_metadata;

		// Token: 0x04001318 RID: 4888
		private static readonly object array_metadata_lock = new object();

		// Token: 0x04001319 RID: 4889
		private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;

		// Token: 0x0400131A RID: 4890
		private static readonly object conv_ops_lock = new object();

		// Token: 0x0400131B RID: 4891
		private static IDictionary<Type, ObjectMetadata> object_metadata;

		// Token: 0x0400131C RID: 4892
		private static readonly object object_metadata_lock = new object();

		// Token: 0x0400131D RID: 4893
		private static IDictionary<Type, IList<PropertyMetadata>> type_properties;

		// Token: 0x0400131E RID: 4894
		private static readonly object type_properties_lock = new object();

		// Token: 0x0400131F RID: 4895
		private static JsonWriter static_writer;

		// Token: 0x04001320 RID: 4896
		private static readonly object static_writer_lock = new object();
	}
}
