using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Extensions;

namespace BestHTTP.Cookies
{
	// Token: 0x02000611 RID: 1553
	public sealed class Cookie : IComparable<Cookie>, IEquatable<Cookie>
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060038DB RID: 14555 RVA: 0x0011B3C6 File Offset: 0x001195C6
		// (set) Token: 0x060038DC RID: 14556 RVA: 0x0011B3CE File Offset: 0x001195CE
		public string Name { get; private set; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060038DD RID: 14557 RVA: 0x0011B3D7 File Offset: 0x001195D7
		// (set) Token: 0x060038DE RID: 14558 RVA: 0x0011B3DF File Offset: 0x001195DF
		public string Value { get; private set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060038DF RID: 14559 RVA: 0x0011B3E8 File Offset: 0x001195E8
		// (set) Token: 0x060038E0 RID: 14560 RVA: 0x0011B3F0 File Offset: 0x001195F0
		public DateTime Date { get; internal set; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060038E1 RID: 14561 RVA: 0x0011B3F9 File Offset: 0x001195F9
		// (set) Token: 0x060038E2 RID: 14562 RVA: 0x0011B401 File Offset: 0x00119601
		public DateTime LastAccess { get; set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060038E3 RID: 14563 RVA: 0x0011B40A File Offset: 0x0011960A
		// (set) Token: 0x060038E4 RID: 14564 RVA: 0x0011B412 File Offset: 0x00119612
		public DateTime Expires { get; private set; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x0011B41B File Offset: 0x0011961B
		// (set) Token: 0x060038E6 RID: 14566 RVA: 0x0011B423 File Offset: 0x00119623
		public long MaxAge { get; private set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x0011B42C File Offset: 0x0011962C
		// (set) Token: 0x060038E8 RID: 14568 RVA: 0x0011B434 File Offset: 0x00119634
		public bool IsSession { get; private set; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060038E9 RID: 14569 RVA: 0x0011B43D File Offset: 0x0011963D
		// (set) Token: 0x060038EA RID: 14570 RVA: 0x0011B445 File Offset: 0x00119645
		public string Domain { get; private set; }

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060038EB RID: 14571 RVA: 0x0011B44E File Offset: 0x0011964E
		// (set) Token: 0x060038EC RID: 14572 RVA: 0x0011B456 File Offset: 0x00119656
		public string Path { get; private set; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060038ED RID: 14573 RVA: 0x0011B45F File Offset: 0x0011965F
		// (set) Token: 0x060038EE RID: 14574 RVA: 0x0011B467 File Offset: 0x00119667
		public bool IsSecure { get; private set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060038EF RID: 14575 RVA: 0x0011B470 File Offset: 0x00119670
		// (set) Token: 0x060038F0 RID: 14576 RVA: 0x0011B478 File Offset: 0x00119678
		public bool IsHttpOnly { get; private set; }

		// Token: 0x060038F1 RID: 14577 RVA: 0x0011B481 File Offset: 0x00119681
		public Cookie(string name, string value) : this(name, value, "/", string.Empty)
		{
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0011B495 File Offset: 0x00119695
		public Cookie(string name, string value, string path) : this(name, value, path, string.Empty)
		{
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0011B4A5 File Offset: 0x001196A5
		public Cookie(string name, string value, string path, string domain) : this()
		{
			this.Name = name;
			this.Value = value;
			this.Path = path;
			this.Domain = domain;
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0011B4CA File Offset: 0x001196CA
		public Cookie(Uri uri, string name, string value, DateTime expires, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
		{
			this.Expires = expires;
			this.IsSession = isSession;
			this.Date = DateTime.UtcNow;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0011B4FB File Offset: 0x001196FB
		public Cookie(Uri uri, string name, string value, long maxAge = -1L, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
		{
			this.MaxAge = maxAge;
			this.IsSession = isSession;
			this.Date = DateTime.UtcNow;
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x0011B52C File Offset: 0x0011972C
		internal Cookie()
		{
			this.IsSession = true;
			this.MaxAge = -1L;
			this.LastAccess = DateTime.UtcNow;
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x0011B550 File Offset: 0x00119750
		public bool WillExpireInTheFuture()
		{
			if (this.IsSession)
			{
				return true;
			}
			if (this.MaxAge == -1L)
			{
				return this.Expires > DateTime.UtcNow;
			}
			return Math.Max(0L, (long)(DateTime.UtcNow - this.Date).TotalSeconds) < this.MaxAge;
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0011B5AC File Offset: 0x001197AC
		public uint GuessSize()
		{
			return (uint)(((this.Name != null) ? (this.Name.Length * 2) : 0) + ((this.Value != null) ? (this.Value.Length * 2) : 0) + ((this.Domain != null) ? (this.Domain.Length * 2) : 0) + ((this.Path != null) ? (this.Path.Length * 2) : 0) + 32 + 3);
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0011B624 File Offset: 0x00119824
		public static Cookie Parse(string header, Uri defaultDomain)
		{
			Cookie cookie = new Cookie();
			try
			{
				foreach (HeaderValue headerValue in Cookie.ParseCookieHeader(header))
				{
					string a = headerValue.Key.ToLowerInvariant();
					if (!(a == "path"))
					{
						if (!(a == "domain"))
						{
							if (!(a == "expires"))
							{
								if (!(a == "max-age"))
								{
									if (!(a == "secure"))
									{
										if (!(a == "httponly"))
										{
											cookie.Name = headerValue.Key;
											cookie.Value = headerValue.Value;
										}
										else
										{
											cookie.IsHttpOnly = true;
										}
									}
									else
									{
										cookie.IsSecure = true;
									}
								}
								else
								{
									cookie.MaxAge = headerValue.Value.ToInt64(-1L);
									cookie.IsSession = false;
								}
							}
							else
							{
								cookie.Expires = headerValue.Value.ToDateTime(DateTime.FromBinary(0L));
								cookie.IsSession = false;
							}
						}
						else
						{
							if (string.IsNullOrEmpty(headerValue.Value))
							{
								return null;
							}
							cookie.Domain = (headerValue.Value.StartsWith(".") ? headerValue.Value.Substring(1) : headerValue.Value);
						}
					}
					else
					{
						cookie.Path = ((string.IsNullOrEmpty(headerValue.Value) || !headerValue.Value.StartsWith("/")) ? "/" : (cookie.Path = headerValue.Value));
					}
				}
				if (HTTPManager.EnablePrivateBrowsing)
				{
					cookie.IsSession = true;
				}
				if (string.IsNullOrEmpty(cookie.Domain))
				{
					cookie.Domain = defaultDomain.Host;
				}
				if (string.IsNullOrEmpty(cookie.Path))
				{
					cookie.Path = defaultDomain.AbsolutePath;
				}
				cookie.Date = (cookie.LastAccess = DateTime.UtcNow);
			}
			catch
			{
			}
			return cookie;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0011B858 File Offset: 0x00119A58
		internal void SaveTo(BinaryWriter stream)
		{
			stream.Write(1);
			stream.Write(this.Name ?? string.Empty);
			stream.Write(this.Value ?? string.Empty);
			stream.Write(this.Date.ToBinary());
			stream.Write(this.LastAccess.ToBinary());
			stream.Write(this.Expires.ToBinary());
			stream.Write(this.MaxAge);
			stream.Write(this.IsSession);
			stream.Write(this.Domain ?? string.Empty);
			stream.Write(this.Path ?? string.Empty);
			stream.Write(this.IsSecure);
			stream.Write(this.IsHttpOnly);
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0011B92C File Offset: 0x00119B2C
		internal void LoadFrom(BinaryReader stream)
		{
			stream.ReadInt32();
			this.Name = stream.ReadString();
			this.Value = stream.ReadString();
			this.Date = DateTime.FromBinary(stream.ReadInt64());
			this.LastAccess = DateTime.FromBinary(stream.ReadInt64());
			this.Expires = DateTime.FromBinary(stream.ReadInt64());
			this.MaxAge = stream.ReadInt64();
			this.IsSession = stream.ReadBoolean();
			this.Domain = stream.ReadString();
			this.Path = stream.ReadString();
			this.IsSecure = stream.ReadBoolean();
			this.IsHttpOnly = stream.ReadBoolean();
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0011B9D3 File Offset: 0x00119BD3
		public override string ToString()
		{
			return this.Name + "=" + this.Value;
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0011B9EB File Offset: 0x00119BEB
		public override bool Equals(object obj)
		{
			return obj != null && this.Equals(obj as Cookie);
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0011BA00 File Offset: 0x00119C00
		public bool Equals(Cookie cookie)
		{
			return cookie != null && (this == cookie || (this.Name.Equals(cookie.Name, StringComparison.Ordinal) && ((this.Domain == null && cookie.Domain == null) || this.Domain.Equals(cookie.Domain, StringComparison.Ordinal)) && ((this.Path == null && cookie.Path == null) || this.Path.Equals(cookie.Path, StringComparison.Ordinal))));
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x0011BA76 File Offset: 0x00119C76
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0011BA84 File Offset: 0x00119C84
		private static string ReadValue(string str, ref int pos)
		{
			string empty = string.Empty;
			if (str == null)
			{
				return empty;
			}
			return str.Read(ref pos, ';', true);
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0011BAA8 File Offset: 0x00119CA8
		private static List<HeaderValue> ParseCookieHeader(string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != '=' && ch != ';', true).Trim());
				if (i < str.Length && str[i - 1] == '=')
				{
					headerValue.Value = Cookie.ReadValue(str, ref i);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0011BB2C File Offset: 0x00119D2C
		public int CompareTo(Cookie other)
		{
			return this.LastAccess.CompareTo(other.LastAccess);
		}

		// Token: 0x040024F6 RID: 9462
		private const int Version = 1;
	}
}
