using System;
using System.Collections;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000539 RID: 1337
	public class X509Name : Asn1Encodable
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000F917E File Offset: 0x000F737E
		// (set) Token: 0x060030CD RID: 12493 RVA: 0x000F9187 File Offset: 0x000F7387
		public static bool DefaultReverse
		{
			get
			{
				return X509Name.defaultReverse[0];
			}
			set
			{
				X509Name.defaultReverse[0] = value;
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000F9194 File Offset: 0x000F7394
		static X509Name()
		{
			X509Name.DefaultSymbols.Add(X509Name.C, "C");
			X509Name.DefaultSymbols.Add(X509Name.O, "O");
			X509Name.DefaultSymbols.Add(X509Name.T, "T");
			X509Name.DefaultSymbols.Add(X509Name.OU, "OU");
			X509Name.DefaultSymbols.Add(X509Name.CN, "CN");
			X509Name.DefaultSymbols.Add(X509Name.L, "L");
			X509Name.DefaultSymbols.Add(X509Name.ST, "ST");
			X509Name.DefaultSymbols.Add(X509Name.SerialNumber, "SERIALNUMBER");
			X509Name.DefaultSymbols.Add(X509Name.EmailAddress, "E");
			X509Name.DefaultSymbols.Add(X509Name.DC, "DC");
			X509Name.DefaultSymbols.Add(X509Name.UID, "UID");
			X509Name.DefaultSymbols.Add(X509Name.Street, "STREET");
			X509Name.DefaultSymbols.Add(X509Name.Surname, "SURNAME");
			X509Name.DefaultSymbols.Add(X509Name.GivenName, "GIVENNAME");
			X509Name.DefaultSymbols.Add(X509Name.Initials, "INITIALS");
			X509Name.DefaultSymbols.Add(X509Name.Generation, "GENERATION");
			X509Name.DefaultSymbols.Add(X509Name.UnstructuredAddress, "unstructuredAddress");
			X509Name.DefaultSymbols.Add(X509Name.UnstructuredName, "unstructuredName");
			X509Name.DefaultSymbols.Add(X509Name.UniqueIdentifier, "UniqueIdentifier");
			X509Name.DefaultSymbols.Add(X509Name.DnQualifier, "DN");
			X509Name.DefaultSymbols.Add(X509Name.Pseudonym, "Pseudonym");
			X509Name.DefaultSymbols.Add(X509Name.PostalAddress, "PostalAddress");
			X509Name.DefaultSymbols.Add(X509Name.NameAtBirth, "NameAtBirth");
			X509Name.DefaultSymbols.Add(X509Name.CountryOfCitizenship, "CountryOfCitizenship");
			X509Name.DefaultSymbols.Add(X509Name.CountryOfResidence, "CountryOfResidence");
			X509Name.DefaultSymbols.Add(X509Name.Gender, "Gender");
			X509Name.DefaultSymbols.Add(X509Name.PlaceOfBirth, "PlaceOfBirth");
			X509Name.DefaultSymbols.Add(X509Name.DateOfBirth, "DateOfBirth");
			X509Name.DefaultSymbols.Add(X509Name.PostalCode, "PostalCode");
			X509Name.DefaultSymbols.Add(X509Name.BusinessCategory, "BusinessCategory");
			X509Name.DefaultSymbols.Add(X509Name.TelephoneNumber, "TelephoneNumber");
			X509Name.RFC2253Symbols.Add(X509Name.C, "C");
			X509Name.RFC2253Symbols.Add(X509Name.O, "O");
			X509Name.RFC2253Symbols.Add(X509Name.OU, "OU");
			X509Name.RFC2253Symbols.Add(X509Name.CN, "CN");
			X509Name.RFC2253Symbols.Add(X509Name.L, "L");
			X509Name.RFC2253Symbols.Add(X509Name.ST, "ST");
			X509Name.RFC2253Symbols.Add(X509Name.Street, "STREET");
			X509Name.RFC2253Symbols.Add(X509Name.DC, "DC");
			X509Name.RFC2253Symbols.Add(X509Name.UID, "UID");
			X509Name.RFC1779Symbols.Add(X509Name.C, "C");
			X509Name.RFC1779Symbols.Add(X509Name.O, "O");
			X509Name.RFC1779Symbols.Add(X509Name.OU, "OU");
			X509Name.RFC1779Symbols.Add(X509Name.CN, "CN");
			X509Name.RFC1779Symbols.Add(X509Name.L, "L");
			X509Name.RFC1779Symbols.Add(X509Name.ST, "ST");
			X509Name.RFC1779Symbols.Add(X509Name.Street, "STREET");
			X509Name.DefaultLookup.Add("c", X509Name.C);
			X509Name.DefaultLookup.Add("o", X509Name.O);
			X509Name.DefaultLookup.Add("t", X509Name.T);
			X509Name.DefaultLookup.Add("ou", X509Name.OU);
			X509Name.DefaultLookup.Add("cn", X509Name.CN);
			X509Name.DefaultLookup.Add("l", X509Name.L);
			X509Name.DefaultLookup.Add("st", X509Name.ST);
			X509Name.DefaultLookup.Add("serialnumber", X509Name.SerialNumber);
			X509Name.DefaultLookup.Add("street", X509Name.Street);
			X509Name.DefaultLookup.Add("emailaddress", X509Name.E);
			X509Name.DefaultLookup.Add("dc", X509Name.DC);
			X509Name.DefaultLookup.Add("e", X509Name.E);
			X509Name.DefaultLookup.Add("uid", X509Name.UID);
			X509Name.DefaultLookup.Add("surname", X509Name.Surname);
			X509Name.DefaultLookup.Add("givenname", X509Name.GivenName);
			X509Name.DefaultLookup.Add("initials", X509Name.Initials);
			X509Name.DefaultLookup.Add("generation", X509Name.Generation);
			X509Name.DefaultLookup.Add("unstructuredaddress", X509Name.UnstructuredAddress);
			X509Name.DefaultLookup.Add("unstructuredname", X509Name.UnstructuredName);
			X509Name.DefaultLookup.Add("uniqueidentifier", X509Name.UniqueIdentifier);
			X509Name.DefaultLookup.Add("dn", X509Name.DnQualifier);
			X509Name.DefaultLookup.Add("pseudonym", X509Name.Pseudonym);
			X509Name.DefaultLookup.Add("postaladdress", X509Name.PostalAddress);
			X509Name.DefaultLookup.Add("nameofbirth", X509Name.NameAtBirth);
			X509Name.DefaultLookup.Add("countryofcitizenship", X509Name.CountryOfCitizenship);
			X509Name.DefaultLookup.Add("countryofresidence", X509Name.CountryOfResidence);
			X509Name.DefaultLookup.Add("gender", X509Name.Gender);
			X509Name.DefaultLookup.Add("placeofbirth", X509Name.PlaceOfBirth);
			X509Name.DefaultLookup.Add("dateofbirth", X509Name.DateOfBirth);
			X509Name.DefaultLookup.Add("postalcode", X509Name.PostalCode);
			X509Name.DefaultLookup.Add("businesscategory", X509Name.BusinessCategory);
			X509Name.DefaultLookup.Add("telephonenumber", X509Name.TelephoneNumber);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000F99E0 File Offset: 0x000F7BE0
		public static X509Name GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509Name.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000F99EE File Offset: 0x000F7BEE
		public static X509Name GetInstance(object obj)
		{
			if (obj == null || obj is X509Name)
			{
				return (X509Name)obj;
			}
			if (obj != null)
			{
				return new X509Name(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("null object in factory", "obj");
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000F9A20 File Offset: 0x000F7C20
		protected X509Name()
		{
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000F9A4C File Offset: 0x000F7C4C
		protected X509Name(Asn1Sequence seq)
		{
			this.seq = seq;
			foreach (object obj in seq)
			{
				Asn1Set instance = Asn1Set.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
				for (int i = 0; i < instance.Count; i++)
				{
					Asn1Sequence instance2 = Asn1Sequence.GetInstance(instance[i].ToAsn1Object());
					if (instance2.Count != 2)
					{
						throw new ArgumentException("badly sized pair");
					}
					this.ordering.Add(DerObjectIdentifier.GetInstance(instance2[0].ToAsn1Object()));
					Asn1Object asn1Object = instance2[1].ToAsn1Object();
					if (asn1Object is IAsn1String && !(asn1Object is DerUniversalString))
					{
						string text = ((IAsn1String)asn1Object).GetString();
						if (Platform.StartsWith(text, "#"))
						{
							text = "\\" + text;
						}
						this.values.Add(text);
					}
					else
					{
						this.values.Add("#" + Hex.ToHexString(asn1Object.GetEncoded()));
					}
					this.added.Add(i != 0);
				}
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000F9BD4 File Offset: 0x000F7DD4
		public X509Name(IList ordering, IDictionary attributes) : this(ordering, attributes, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000F9BE4 File Offset: 0x000F7DE4
		public X509Name(IList ordering, IDictionary attributes, X509NameEntryConverter converter)
		{
			this.converter = converter;
			foreach (object obj in ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				object obj2 = attributes[derObjectIdentifier];
				if (obj2 == null)
				{
					throw new ArgumentException("No attribute for object id - " + derObjectIdentifier + " - passed to distinguished name");
				}
				this.ordering.Add(derObjectIdentifier);
				this.added.Add(false);
				this.values.Add(obj2);
			}
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000F9CAC File Offset: 0x000F7EAC
		public X509Name(IList oids, IList values) : this(oids, values, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000F9CBC File Offset: 0x000F7EBC
		public X509Name(IList oids, IList values, X509NameEntryConverter converter)
		{
			this.converter = converter;
			if (oids.Count != values.Count)
			{
				throw new ArgumentException("'oids' must be same length as 'values'.");
			}
			for (int i = 0; i < oids.Count; i++)
			{
				this.ordering.Add(oids[i]);
				this.values.Add(values[i]);
				this.added.Add(false);
			}
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000F9D59 File Offset: 0x000F7F59
		public X509Name(string dirName) : this(X509Name.DefaultReverse, X509Name.DefaultLookup, dirName)
		{
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000F9D6C File Offset: 0x000F7F6C
		public X509Name(string dirName, X509NameEntryConverter converter) : this(X509Name.DefaultReverse, X509Name.DefaultLookup, dirName, converter)
		{
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000F9D80 File Offset: 0x000F7F80
		public X509Name(bool reverse, string dirName) : this(reverse, X509Name.DefaultLookup, dirName)
		{
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000F9D8F File Offset: 0x000F7F8F
		public X509Name(bool reverse, string dirName, X509NameEntryConverter converter) : this(reverse, X509Name.DefaultLookup, dirName, converter)
		{
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000F9D9F File Offset: 0x000F7F9F
		public X509Name(bool reverse, IDictionary lookUp, string dirName) : this(reverse, lookUp, dirName, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000F9DB0 File Offset: 0x000F7FB0
		private DerObjectIdentifier DecodeOid(string name, IDictionary lookUp)
		{
			if (Platform.StartsWith(Platform.ToUpperInvariant(name), "OID."))
			{
				return new DerObjectIdentifier(name.Substring(4));
			}
			if (name[0] >= '0' && name[0] <= '9')
			{
				return new DerObjectIdentifier(name);
			}
			DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)lookUp[Platform.ToLowerInvariant(name)];
			if (derObjectIdentifier == null)
			{
				throw new ArgumentException("Unknown object id - " + name + " - passed to distinguished name");
			}
			return derObjectIdentifier;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000F9E24 File Offset: 0x000F8024
		public X509Name(bool reverse, IDictionary lookUp, string dirName, X509NameEntryConverter converter)
		{
			this.converter = converter;
			X509NameTokenizer x509NameTokenizer = new X509NameTokenizer(dirName);
			while (x509NameTokenizer.HasMoreTokens())
			{
				string text = x509NameTokenizer.NextToken();
				int num = text.IndexOf('=');
				if (num == -1)
				{
					throw new ArgumentException("badly formated directory string");
				}
				string name = text.Substring(0, num);
				string text2 = text.Substring(num + 1);
				DerObjectIdentifier value = this.DecodeOid(name, lookUp);
				if (text2.IndexOf('+') > 0)
				{
					X509NameTokenizer x509NameTokenizer2 = new X509NameTokenizer(text2, '+');
					string value2 = x509NameTokenizer2.NextToken();
					this.ordering.Add(value);
					this.values.Add(value2);
					this.added.Add(false);
					while (x509NameTokenizer2.HasMoreTokens())
					{
						string text3 = x509NameTokenizer2.NextToken();
						int num2 = text3.IndexOf('=');
						string name2 = text3.Substring(0, num2);
						string value3 = text3.Substring(num2 + 1);
						this.ordering.Add(this.DecodeOid(name2, lookUp));
						this.values.Add(value3);
						this.added.Add(true);
					}
				}
				else
				{
					this.ordering.Add(value);
					this.values.Add(text2);
					this.added.Add(false);
				}
			}
			if (reverse)
			{
				IList list = Platform.CreateArrayList();
				IList list2 = Platform.CreateArrayList();
				IList list3 = Platform.CreateArrayList();
				int num3 = 1;
				for (int i = 0; i < this.ordering.Count; i++)
				{
					if (!(bool)this.added[i])
					{
						num3 = 0;
					}
					int index = num3++;
					list.Insert(index, this.ordering[i]);
					list2.Insert(index, this.values[i]);
					list3.Insert(index, this.added[i]);
				}
				this.ordering = list;
				this.values = list2;
				this.added = list3;
			}
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000FA048 File Offset: 0x000F8248
		public IList GetOidList()
		{
			return Platform.CreateArrayList(this.ordering);
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000FA055 File Offset: 0x000F8255
		public IList GetValueList()
		{
			return this.GetValueList(null);
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000FA060 File Offset: 0x000F8260
		public IList GetValueList(DerObjectIdentifier oid)
		{
			IList list = Platform.CreateArrayList();
			for (int num = 0; num != this.values.Count; num++)
			{
				if (oid == null || oid.Equals(this.ordering[num]))
				{
					string text = (string)this.values[num];
					if (Platform.StartsWith(text, "\\#"))
					{
						text = text.Substring(1);
					}
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000FA0D0 File Offset: 0x000F82D0
		public override Asn1Object ToAsn1Object()
		{
			if (this.seq == null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				DerObjectIdentifier derObjectIdentifier = null;
				for (int num = 0; num != this.ordering.Count; num++)
				{
					DerObjectIdentifier derObjectIdentifier2 = (DerObjectIdentifier)this.ordering[num];
					string value = (string)this.values[num];
					if (derObjectIdentifier != null && !(bool)this.added[num])
					{
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							new DerSet(asn1EncodableVector2)
						});
						asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					}
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						new DerSequence(new Asn1Encodable[]
						{
							derObjectIdentifier2,
							this.converter.GetConvertedValue(derObjectIdentifier2, value)
						})
					});
					derObjectIdentifier = derObjectIdentifier2;
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerSet(asn1EncodableVector2)
				});
				this.seq = new DerSequence(asn1EncodableVector);
			}
			return this.seq;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x000FA1D4 File Offset: 0x000F83D4
		public bool Equivalent(X509Name other, bool inOrder)
		{
			if (!inOrder)
			{
				return this.Equivalent(other);
			}
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = this.ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				object obj = (DerObjectIdentifier)this.ordering[i];
				DerObjectIdentifier obj2 = (DerObjectIdentifier)other.ordering[i];
				if (!obj.Equals(obj2))
				{
					return false;
				}
				string s = (string)this.values[i];
				string s2 = (string)other.values[i];
				if (!X509Name.equivalentStrings(s, s2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000FA27C File Offset: 0x000F847C
		public bool Equivalent(X509Name other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = this.ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			bool[] array = new bool[count];
			int num;
			int num2;
			int num3;
			if (this.ordering[0].Equals(other.ordering[0]))
			{
				num = 0;
				num2 = count;
				num3 = 1;
			}
			else
			{
				num = count - 1;
				num2 = -1;
				num3 = -1;
			}
			for (int num4 = num; num4 != num2; num4 += num3)
			{
				bool flag = false;
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)this.ordering[num4];
				string s = (string)this.values[num4];
				for (int i = 0; i < count; i++)
				{
					if (!array[i])
					{
						DerObjectIdentifier obj = (DerObjectIdentifier)other.ordering[i];
						if (derObjectIdentifier.Equals(obj))
						{
							string s2 = (string)other.values[i];
							if (X509Name.equivalentStrings(s, s2))
							{
								array[i] = true;
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000FA390 File Offset: 0x000F8590
		private static bool equivalentStrings(string s1, string s2)
		{
			string text = X509Name.canonicalize(s1);
			string text2 = X509Name.canonicalize(s2);
			if (!text.Equals(text2))
			{
				text = X509Name.stripInternalSpaces(text);
				text2 = X509Name.stripInternalSpaces(text2);
				if (!text.Equals(text2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000FA3D0 File Offset: 0x000F85D0
		private static string canonicalize(string s)
		{
			string text = Platform.ToLowerInvariant(s).Trim();
			if (Platform.StartsWith(text, "#"))
			{
				Asn1Object asn1Object = X509Name.decodeObject(text);
				if (asn1Object is IAsn1String)
				{
					text = Platform.ToLowerInvariant(((IAsn1String)asn1Object).GetString()).Trim();
				}
			}
			return text;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000FA41C File Offset: 0x000F861C
		private static Asn1Object decodeObject(string v)
		{
			Asn1Object result;
			try
			{
				result = Asn1Object.FromByteArray(Hex.Decode(v.Substring(1)));
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("unknown encoding in name: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000FA468 File Offset: 0x000F8668
		private static string stripInternalSpaces(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (str.Length != 0)
			{
				char c = str[0];
				stringBuilder.Append(c);
				for (int i = 1; i < str.Length; i++)
				{
					char c2 = str[i];
					if (c != ' ' || c2 != ' ')
					{
						stringBuilder.Append(c2);
					}
					c = c2;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000FA4C8 File Offset: 0x000F86C8
		private void AppendValue(StringBuilder buf, IDictionary oidSymbols, DerObjectIdentifier oid, string val)
		{
			string text = (string)oidSymbols[oid];
			if (text != null)
			{
				buf.Append(text);
			}
			else
			{
				buf.Append(oid.Id);
			}
			buf.Append('=');
			int num = buf.Length;
			buf.Append(val);
			int num2 = buf.Length;
			if (Platform.StartsWith(val, "\\#"))
			{
				num += 2;
			}
			while (num != num2)
			{
				if (buf[num] == ',' || buf[num] == '"' || buf[num] == '\\' || buf[num] == '+' || buf[num] == '=' || buf[num] == '<' || buf[num] == '>' || buf[num] == ';')
				{
					buf.Insert(num++, "\\");
					num2++;
				}
				num++;
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000FA5A8 File Offset: 0x000F87A8
		public string ToString(bool reverse, IDictionary oidSymbols)
		{
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = null;
			for (int i = 0; i < this.ordering.Count; i++)
			{
				if ((bool)this.added[i])
				{
					stringBuilder.Append('+');
					this.AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)this.ordering[i], (string)this.values[i]);
				}
				else
				{
					stringBuilder = new StringBuilder();
					this.AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)this.ordering[i], (string)this.values[i]);
					arrayList.Add(stringBuilder);
				}
			}
			if (reverse)
			{
				arrayList.Reverse();
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (arrayList.Count > 0)
			{
				stringBuilder2.Append(arrayList[0].ToString());
				for (int j = 1; j < arrayList.Count; j++)
				{
					stringBuilder2.Append(',');
					stringBuilder2.Append(arrayList[j].ToString());
				}
			}
			return stringBuilder2.ToString();
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000FA6BC File Offset: 0x000F88BC
		public override string ToString()
		{
			return this.ToString(X509Name.DefaultReverse, X509Name.DefaultSymbols);
		}

		// Token: 0x04001F39 RID: 7993
		public static readonly DerObjectIdentifier C = new DerObjectIdentifier("2.5.4.6");

		// Token: 0x04001F3A RID: 7994
		public static readonly DerObjectIdentifier O = new DerObjectIdentifier("2.5.4.10");

		// Token: 0x04001F3B RID: 7995
		public static readonly DerObjectIdentifier OU = new DerObjectIdentifier("2.5.4.11");

		// Token: 0x04001F3C RID: 7996
		public static readonly DerObjectIdentifier T = new DerObjectIdentifier("2.5.4.12");

		// Token: 0x04001F3D RID: 7997
		public static readonly DerObjectIdentifier CN = new DerObjectIdentifier("2.5.4.3");

		// Token: 0x04001F3E RID: 7998
		public static readonly DerObjectIdentifier Street = new DerObjectIdentifier("2.5.4.9");

		// Token: 0x04001F3F RID: 7999
		public static readonly DerObjectIdentifier SerialNumber = new DerObjectIdentifier("2.5.4.5");

		// Token: 0x04001F40 RID: 8000
		public static readonly DerObjectIdentifier L = new DerObjectIdentifier("2.5.4.7");

		// Token: 0x04001F41 RID: 8001
		public static readonly DerObjectIdentifier ST = new DerObjectIdentifier("2.5.4.8");

		// Token: 0x04001F42 RID: 8002
		public static readonly DerObjectIdentifier Surname = new DerObjectIdentifier("2.5.4.4");

		// Token: 0x04001F43 RID: 8003
		public static readonly DerObjectIdentifier GivenName = new DerObjectIdentifier("2.5.4.42");

		// Token: 0x04001F44 RID: 8004
		public static readonly DerObjectIdentifier Initials = new DerObjectIdentifier("2.5.4.43");

		// Token: 0x04001F45 RID: 8005
		public static readonly DerObjectIdentifier Generation = new DerObjectIdentifier("2.5.4.44");

		// Token: 0x04001F46 RID: 8006
		public static readonly DerObjectIdentifier UniqueIdentifier = new DerObjectIdentifier("2.5.4.45");

		// Token: 0x04001F47 RID: 8007
		public static readonly DerObjectIdentifier BusinessCategory = new DerObjectIdentifier("2.5.4.15");

		// Token: 0x04001F48 RID: 8008
		public static readonly DerObjectIdentifier PostalCode = new DerObjectIdentifier("2.5.4.17");

		// Token: 0x04001F49 RID: 8009
		public static readonly DerObjectIdentifier DnQualifier = new DerObjectIdentifier("2.5.4.46");

		// Token: 0x04001F4A RID: 8010
		public static readonly DerObjectIdentifier Pseudonym = new DerObjectIdentifier("2.5.4.65");

		// Token: 0x04001F4B RID: 8011
		public static readonly DerObjectIdentifier DateOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.1");

		// Token: 0x04001F4C RID: 8012
		public static readonly DerObjectIdentifier PlaceOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.2");

		// Token: 0x04001F4D RID: 8013
		public static readonly DerObjectIdentifier Gender = new DerObjectIdentifier("1.3.6.1.5.5.7.9.3");

		// Token: 0x04001F4E RID: 8014
		public static readonly DerObjectIdentifier CountryOfCitizenship = new DerObjectIdentifier("1.3.6.1.5.5.7.9.4");

		// Token: 0x04001F4F RID: 8015
		public static readonly DerObjectIdentifier CountryOfResidence = new DerObjectIdentifier("1.3.6.1.5.5.7.9.5");

		// Token: 0x04001F50 RID: 8016
		public static readonly DerObjectIdentifier NameAtBirth = new DerObjectIdentifier("1.3.36.8.3.14");

		// Token: 0x04001F51 RID: 8017
		public static readonly DerObjectIdentifier PostalAddress = new DerObjectIdentifier("2.5.4.16");

		// Token: 0x04001F52 RID: 8018
		public static readonly DerObjectIdentifier DmdName = new DerObjectIdentifier("2.5.4.54");

		// Token: 0x04001F53 RID: 8019
		public static readonly DerObjectIdentifier TelephoneNumber = X509ObjectIdentifiers.id_at_telephoneNumber;

		// Token: 0x04001F54 RID: 8020
		public static readonly DerObjectIdentifier Name = X509ObjectIdentifiers.id_at_name;

		// Token: 0x04001F55 RID: 8021
		public static readonly DerObjectIdentifier EmailAddress = PkcsObjectIdentifiers.Pkcs9AtEmailAddress;

		// Token: 0x04001F56 RID: 8022
		public static readonly DerObjectIdentifier UnstructuredName = PkcsObjectIdentifiers.Pkcs9AtUnstructuredName;

		// Token: 0x04001F57 RID: 8023
		public static readonly DerObjectIdentifier UnstructuredAddress = PkcsObjectIdentifiers.Pkcs9AtUnstructuredAddress;

		// Token: 0x04001F58 RID: 8024
		public static readonly DerObjectIdentifier E = X509Name.EmailAddress;

		// Token: 0x04001F59 RID: 8025
		public static readonly DerObjectIdentifier DC = new DerObjectIdentifier("0.9.2342.19200300.100.1.25");

		// Token: 0x04001F5A RID: 8026
		public static readonly DerObjectIdentifier UID = new DerObjectIdentifier("0.9.2342.19200300.100.1.1");

		// Token: 0x04001F5B RID: 8027
		private static readonly bool[] defaultReverse = new bool[1];

		// Token: 0x04001F5C RID: 8028
		public static readonly Hashtable DefaultSymbols = new Hashtable();

		// Token: 0x04001F5D RID: 8029
		public static readonly Hashtable RFC2253Symbols = new Hashtable();

		// Token: 0x04001F5E RID: 8030
		public static readonly Hashtable RFC1779Symbols = new Hashtable();

		// Token: 0x04001F5F RID: 8031
		public static readonly Hashtable DefaultLookup = new Hashtable();

		// Token: 0x04001F60 RID: 8032
		private readonly IList ordering = Platform.CreateArrayList();

		// Token: 0x04001F61 RID: 8033
		private readonly X509NameEntryConverter converter;

		// Token: 0x04001F62 RID: 8034
		private IList values = Platform.CreateArrayList();

		// Token: 0x04001F63 RID: 8035
		private IList added = Platform.CreateArrayList();

		// Token: 0x04001F64 RID: 8036
		private Asn1Sequence seq;
	}
}
