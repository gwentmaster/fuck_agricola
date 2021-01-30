using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Extensions;

namespace BestHTTP.Authentication
{
	// Token: 0x0200061A RID: 1562
	internal sealed class Digest
	{
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x0011DBEF File Offset: 0x0011BDEF
		// (set) Token: 0x06003973 RID: 14707 RVA: 0x0011DBF7 File Offset: 0x0011BDF7
		public Uri Uri { get; private set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003974 RID: 14708 RVA: 0x0011DC00 File Offset: 0x0011BE00
		// (set) Token: 0x06003975 RID: 14709 RVA: 0x0011DC08 File Offset: 0x0011BE08
		public AuthenticationTypes Type { get; private set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06003976 RID: 14710 RVA: 0x0011DC11 File Offset: 0x0011BE11
		// (set) Token: 0x06003977 RID: 14711 RVA: 0x0011DC19 File Offset: 0x0011BE19
		public string Realm { get; private set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003978 RID: 14712 RVA: 0x0011DC22 File Offset: 0x0011BE22
		// (set) Token: 0x06003979 RID: 14713 RVA: 0x0011DC2A File Offset: 0x0011BE2A
		public bool Stale { get; private set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600397A RID: 14714 RVA: 0x0011DC33 File Offset: 0x0011BE33
		// (set) Token: 0x0600397B RID: 14715 RVA: 0x0011DC3B File Offset: 0x0011BE3B
		private string Nonce { get; set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x0011DC44 File Offset: 0x0011BE44
		// (set) Token: 0x0600397D RID: 14717 RVA: 0x0011DC4C File Offset: 0x0011BE4C
		private string Opaque { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600397E RID: 14718 RVA: 0x0011DC55 File Offset: 0x0011BE55
		// (set) Token: 0x0600397F RID: 14719 RVA: 0x0011DC5D File Offset: 0x0011BE5D
		private string Algorithm { get; set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003980 RID: 14720 RVA: 0x0011DC66 File Offset: 0x0011BE66
		// (set) Token: 0x06003981 RID: 14721 RVA: 0x0011DC6E File Offset: 0x0011BE6E
		public List<string> ProtectedUris { get; private set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06003982 RID: 14722 RVA: 0x0011DC77 File Offset: 0x0011BE77
		// (set) Token: 0x06003983 RID: 14723 RVA: 0x0011DC7F File Offset: 0x0011BE7F
		private string QualityOfProtections { get; set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x0011DC88 File Offset: 0x0011BE88
		// (set) Token: 0x06003985 RID: 14725 RVA: 0x0011DC90 File Offset: 0x0011BE90
		private int NonceCount { get; set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x0011DC99 File Offset: 0x0011BE99
		// (set) Token: 0x06003987 RID: 14727 RVA: 0x0011DCA1 File Offset: 0x0011BEA1
		private string HA1Sess { get; set; }

		// Token: 0x06003988 RID: 14728 RVA: 0x0011DCAA File Offset: 0x0011BEAA
		internal Digest(Uri uri)
		{
			this.Uri = uri;
			this.Algorithm = "md5";
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x0011DCC4 File Offset: 0x0011BEC4
		public void ParseChallange(string header)
		{
			this.Type = AuthenticationTypes.Unknown;
			this.Stale = false;
			this.Opaque = null;
			this.HA1Sess = null;
			this.NonceCount = 0;
			this.QualityOfProtections = null;
			if (this.ProtectedUris != null)
			{
				this.ProtectedUris.Clear();
			}
			foreach (HeaderValue headerValue in new WWWAuthenticateHeaderParser(header).Values)
			{
				string key = headerValue.Key;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
				if (num <= 1863671838U)
				{
					if (num <= 474311018U)
					{
						if (num != 87360061U)
						{
							if (num == 474311018U)
							{
								if (key == "algorithm")
								{
									this.Algorithm = headerValue.Value;
								}
							}
						}
						else if (key == "basic")
						{
							this.Type = AuthenticationTypes.Basic;
						}
					}
					else if (num != 1749328254U)
					{
						if (num == 1863671838U)
						{
							if (key == "opaque")
							{
								this.Opaque = headerValue.Value;
							}
						}
					}
					else if (key == "stale")
					{
						this.Stale = bool.Parse(headerValue.Value);
					}
				}
				else if (num <= 3885209585U)
				{
					if (num != 1914854288U)
					{
						if (num == 3885209585U)
						{
							if (key == "domain")
							{
								if (!string.IsNullOrEmpty(headerValue.Value) && headerValue.Value.Length != 0)
								{
									if (this.ProtectedUris == null)
									{
										this.ProtectedUris = new List<string>();
									}
									int num2 = 0;
									string item = headerValue.Value.Read(ref num2, ' ', true);
									do
									{
										this.ProtectedUris.Add(item);
										item = headerValue.Value.Read(ref num2, ' ', true);
									}
									while (num2 < headerValue.Value.Length);
								}
							}
						}
					}
					else if (key == "realm")
					{
						this.Realm = headerValue.Value;
					}
				}
				else if (num != 4143537083U)
				{
					if (num != 4178082296U)
					{
						if (num == 4179908061U)
						{
							if (key == "digest")
							{
								this.Type = AuthenticationTypes.Digest;
							}
						}
					}
					else if (key == "nonce")
					{
						this.Nonce = headerValue.Value;
					}
				}
				else if (key == "qop")
				{
					this.QualityOfProtections = headerValue.Value;
				}
			}
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x0011DF98 File Offset: 0x0011C198
		public string GenerateResponseHeader(HTTPRequest request, Credentials credentials)
		{
			try
			{
				AuthenticationTypes type = this.Type;
				if (type == AuthenticationTypes.Basic)
				{
					return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", credentials.UserName, credentials.Password)));
				}
				if (type == AuthenticationTypes.Digest)
				{
					int nonceCount = this.NonceCount;
					this.NonceCount = nonceCount + 1;
					string text = string.Empty;
					string text2 = new Random(request.GetHashCode()).Next(int.MinValue, int.MaxValue).ToString("X8");
					string text3 = this.NonceCount.ToString("X8");
					string a = this.Algorithm.TrimAndLower();
					if (!(a == "md5"))
					{
						if (!(a == "md5-sess"))
						{
							return string.Empty;
						}
						if (string.IsNullOrEmpty(this.HA1Sess))
						{
							this.HA1Sess = string.Format("{0}:{1}:{2}:{3}:{4}", new object[]
							{
								credentials.UserName,
								this.Realm,
								credentials.Password,
								this.Nonce,
								text3
							}).CalculateMD5Hash();
						}
						text = this.HA1Sess;
					}
					else
					{
						text = string.Format("{0}:{1}:{2}", credentials.UserName, this.Realm, credentials.Password).CalculateMD5Hash();
					}
					string text4 = string.Empty;
					string text5 = (this.QualityOfProtections != null) ? this.QualityOfProtections.TrimAndLower() : null;
					if (text5 == null)
					{
						string arg = (request.MethodType.ToString().ToUpper() + ":" + request.CurrentUri.GetRequestPathAndQueryURL()).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}", text, this.Nonce, arg).CalculateMD5Hash();
					}
					else if (text5.Contains("auth-int"))
					{
						text5 = "auth-int";
						byte[] array = request.GetEntityBody();
						if (array == null)
						{
							array = string.Empty.GetASCIIBytes();
						}
						string text6 = string.Format("{0}:{1}:{2}", request.MethodType.ToString().ToUpper(), request.CurrentUri.GetRequestPathAndQueryURL(), array.CalculateMD5Hash()).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", new object[]
						{
							text,
							this.Nonce,
							text3,
							text2,
							text5,
							text6
						}).CalculateMD5Hash();
					}
					else
					{
						if (!text5.Contains("auth"))
						{
							return string.Empty;
						}
						text5 = "auth";
						string text7 = (request.MethodType.ToString().ToUpper() + ":" + request.CurrentUri.GetRequestPathAndQueryURL()).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", new object[]
						{
							text,
							this.Nonce,
							text3,
							text2,
							text5,
							text7
						}).CalculateMD5Hash();
					}
					string text8 = string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", cnonce=\"{4}\", response=\"{5}\"", new object[]
					{
						credentials.UserName,
						this.Realm,
						this.Nonce,
						request.Uri.GetRequestPathAndQueryURL(),
						text2,
						text4
					});
					if (text5 != null)
					{
						text8 = string.Concat(new string[]
						{
							text8,
							", qop=\"",
							text5,
							"\", nc=",
							text3
						});
					}
					if (!string.IsNullOrEmpty(this.Opaque))
					{
						text8 = text8 + ", opaque=\"" + this.Opaque + "\"";
					}
					return text8;
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x0011E370 File Offset: 0x0011C570
		public bool IsUriProtected(Uri uri)
		{
			if (string.CompareOrdinal(uri.Host, this.Uri.Host) != 0)
			{
				return false;
			}
			string text = uri.ToString();
			if (this.ProtectedUris != null && this.ProtectedUris.Count > 0)
			{
				for (int i = 0; i < this.ProtectedUris.Count; i++)
				{
					if (text.Contains(this.ProtectedUris[i]))
					{
						return true;
					}
				}
			}
			return true;
		}
	}
}
