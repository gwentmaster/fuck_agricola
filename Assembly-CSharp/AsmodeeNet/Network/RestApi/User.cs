using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C5 RID: 1733
	[Serializable]
	public class User
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003E0E RID: 15886 RVA: 0x0012F730 File Offset: 0x0012D930
		public int UserId
		{
			get
			{
				return this._userId;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06003E0F RID: 15887 RVA: 0x0012F738 File Offset: 0x0012D938
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06003E10 RID: 15888 RVA: 0x0012F740 File Offset: 0x0012D940
		public string Country
		{
			get
			{
				return this._country;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x0012F748 File Offset: 0x0012D948
		public bool? EmailValid
		{
			get
			{
				return this._emailValid;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06003E12 RID: 15890 RVA: 0x0012F750 File Offset: 0x0012D950
		public LocalizationManager.Language? Language
		{
			get
			{
				return this._language;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x0012F758 File Offset: 0x0012D958
		public string TimeZone
		{
			get
			{
				return this._timeZone;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06003E14 RID: 15892 RVA: 0x0012F760 File Offset: 0x0012D960
		public int PostedMsgCount
		{
			get
			{
				return this._postedMsgCount;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x0012F768 File Offset: 0x0012D968
		public int LastPostId
		{
			get
			{
				return this._lastPostId;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003E16 RID: 15894 RVA: 0x0012F770 File Offset: 0x0012D970
		public bool? Validated
		{
			get
			{
				return this._validated;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x0012F778 File Offset: 0x0012D978
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06003E18 RID: 15896 RVA: 0x0012F780 File Offset: 0x0012D980
		public DateTime? JoinDate
		{
			get
			{
				return this._joinDate;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x0012F788 File Offset: 0x0012D988
		public DateTime? LastVisit
		{
			get
			{
				return this._lastVisit;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x0012F790 File Offset: 0x0012D990
		public string Zipcode
		{
			get
			{
				return this._zipcode;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003E1B RID: 15899 RVA: 0x0012F798 File Offset: 0x0012D998
		public DateTime? Birthday
		{
			get
			{
				return this._birthday;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x0012F7A0 File Offset: 0x0012D9A0
		public string Email
		{
			get
			{
				return this._email;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x0012F7A8 File Offset: 0x0012D9A8
		public User.UserGender? Gender
		{
			get
			{
				return this._gender;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003E1E RID: 15902 RVA: 0x0012F7B0 File Offset: 0x0012D9B0
		public bool? Coppa
		{
			get
			{
				return this._coppa;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x0012F7B8 File Offset: 0x0012D9B8
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06003E20 RID: 15904 RVA: 0x0012F7C0 File Offset: 0x0012D9C0
		public string[] Features
		{
			get
			{
				return this._features;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06003E21 RID: 15905 RVA: 0x0012F7C8 File Offset: 0x0012D9C8
		public User.BoardGame[] Boardgames
		{
			get
			{
				return this._boardGames;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x0012F7D0 File Offset: 0x0012D9D0
		public User.OnlineGame[] Onlinegames
		{
			get
			{
				return this._onlineGames;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06003E23 RID: 15907 RVA: 0x0012F7D8 File Offset: 0x0012D9D8
		public PartnerAccount[] Partners
		{
			get
			{
				return this._partners;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x0012F7E0 File Offset: 0x0012D9E0
		public bool? Newsletter
		{
			get
			{
				return this._newsletter;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06003E25 RID: 15909 RVA: 0x0012F7E8 File Offset: 0x0012D9E8
		public string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x0012F7F0 File Offset: 0x0012D9F0
		public string Wc
		{
			get
			{
				return this._wc;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x0012F7F8 File Offset: 0x0012D9F8
		public bool? SendSignUpEmail
		{
			get
			{
				return this._sendSignUpEmail;
			}
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0012F800 File Offset: 0x0012DA00
		private User(Builder<User> builder)
		{
			User.Builder builder2 = builder as User.Builder;
			this._email = builder2.GetEmail;
			this._loginName = builder2.GetLoginName;
			this._password = builder2.GetPassword;
			this._name = builder2.GetName;
			this._gender = builder2.GetGender;
			this._birthday = builder2.GetBirthday;
			this._zipcode = builder2.GetZipcode;
			this._country = builder2.GetCountry;
			this._language = builder2.GetLanguage;
			this._timeZone = builder2.GetTimeZone;
			this._coppa = builder2.GetCoppa;
			this._newsletter = builder2.GetNewsletter;
			this._avatar = builder2.GetAvatar;
			this._sendSignUpEmail = builder2.GetSendSignUpEmail;
			this._wc = builder2.GetWC;
			this._postedMsgCount = builder2.GetPostedMsgCount;
			this._emailValid = builder2.GetEmailValid;
			this._userId = builder2.GetUserId;
			this._lastPostId = builder2.GetLastPostId;
			this._validated = builder2.GetValidated;
			this._joinDate = builder2.GetJoinDate;
			this._lastVisit = builder2.GetLastVisit;
			this._features = builder2.GetFeatures;
			this._boardGames = builder2.GetBoardGame;
			this._onlineGames = builder2.GetOnlineGames;
			this._partners = builder2.GetPartners;
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0012F968 File Offset: 0x0012DB68
		public override bool Equals(object obj)
		{
			User user = obj as User;
			if (user == null)
			{
				return false;
			}
			if (this.UserId == user.UserId && this.LoginName == user.LoginName && this.Country == user.Country)
			{
				bool? flag = this.EmailValid;
				bool? flag2 = user.EmailValid;
				if (flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null))
				{
					LocalizationManager.Language? language = this.Language;
					LocalizationManager.Language? language2 = user.Language;
					if ((language.GetValueOrDefault() == language2.GetValueOrDefault() & language != null == (language2 != null)) && this.TimeZone == user.TimeZone && this.PostedMsgCount == user.PostedMsgCount && this.LastPostId == user.LastPostId)
					{
						flag2 = this.Validated;
						flag = user.Validated;
						if ((flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null)) && this.JoinDate == user.JoinDate && this.LastVisit == user.LastVisit && this.Zipcode == user.Zipcode && this.Birthday == user.Birthday && this.Email == user.Email)
						{
							User.UserGender? gender = this.Gender;
							User.UserGender? gender2 = user.Gender;
							if (gender.GetValueOrDefault() == gender2.GetValueOrDefault() & gender != null == (gender2 != null))
							{
								flag = this.Coppa;
								flag2 = user.Coppa;
								if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null)) && this.Name == user.Name)
								{
									flag2 = this.Newsletter;
									flag = user.Newsletter;
									if ((flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null)) && this.Password == user.Password && this.Wc == user.Wc)
									{
										flag = this.SendSignUpEmail;
										flag2 = user.SendSignUpEmail;
										if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null)) && ((this.Features == null && user.Features == null) || this.Features.Length == user.Features.Length) && ((this.Boardgames == null && user.Boardgames == null) || this.Boardgames.Length == user.Boardgames.Length) && ((this.Onlinegames == null && user.Onlinegames == null) || this.Onlinegames.Length == user.Onlinegames.Length))
										{
											return (this.Partners == null && user.Partners == null) || this.Partners.Length == user.Partners.Length;
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0400280F RID: 10255
		private string _email;

		// Token: 0x04002810 RID: 10256
		private string _loginName;

		// Token: 0x04002811 RID: 10257
		private string _password;

		// Token: 0x04002812 RID: 10258
		private string _name;

		// Token: 0x04002813 RID: 10259
		private User.UserGender? _gender;

		// Token: 0x04002814 RID: 10260
		private DateTime? _birthday;

		// Token: 0x04002815 RID: 10261
		private string _zipcode;

		// Token: 0x04002816 RID: 10262
		private string _country;

		// Token: 0x04002817 RID: 10263
		private LocalizationManager.Language? _language;

		// Token: 0x04002818 RID: 10264
		private string _timeZone;

		// Token: 0x04002819 RID: 10265
		private bool? _coppa;

		// Token: 0x0400281A RID: 10266
		private bool? _newsletter;

		// Token: 0x0400281B RID: 10267
		private string _avatar;

		// Token: 0x0400281C RID: 10268
		private bool? _sendSignUpEmail;

		// Token: 0x0400281D RID: 10269
		private string _wc;

		// Token: 0x0400281E RID: 10270
		private int _userId = -1;

		// Token: 0x0400281F RID: 10271
		private bool? _emailValid;

		// Token: 0x04002820 RID: 10272
		private int _postedMsgCount = -1;

		// Token: 0x04002821 RID: 10273
		private int _lastPostId = -1;

		// Token: 0x04002822 RID: 10274
		private bool? _validated;

		// Token: 0x04002823 RID: 10275
		private DateTime? _joinDate;

		// Token: 0x04002824 RID: 10276
		private DateTime? _lastVisit;

		// Token: 0x04002825 RID: 10277
		private string[] _features;

		// Token: 0x04002826 RID: 10278
		private User.BoardGame[] _boardGames;

		// Token: 0x04002827 RID: 10279
		private User.OnlineGame[] _onlineGames;

		// Token: 0x04002828 RID: 10280
		private PartnerAccount[] _partners;

		// Token: 0x020009C4 RID: 2500
		public enum UserGender
		{
			// Token: 0x040032FB RID: 13051
			MALE,
			// Token: 0x040032FC RID: 13052
			FEMALE,
			// Token: 0x040032FD RID: 13053
			UNSPECIFIED
		}

		// Token: 0x020009C5 RID: 2501
		public class BoardGame
		{
			// Token: 0x060048D7 RID: 18647 RVA: 0x0014CC56 File Offset: 0x0014AE56
			public BoardGame(string code, string name, DateTime? registeredDate)
			{
				this.code = code;
				this.name = name;
				this.registeredDate = registeredDate;
			}

			// Token: 0x040032FE RID: 13054
			public string code;

			// Token: 0x040032FF RID: 13055
			public string name;

			// Token: 0x04003300 RID: 13056
			public DateTime? registeredDate;
		}

		// Token: 0x020009C6 RID: 2502
		public class OnlineGame
		{
			// Token: 0x060048D8 RID: 18648 RVA: 0x0014CC73 File Offset: 0x0014AE73
			public OnlineGame(string game, int nbGames, int karma, float rankScore, int rank, DateTime? lastGame, string variant)
			{
				this.game = game;
				this.nbGames = nbGames;
				this.karma = karma;
				this.rankScore = rankScore;
				this.rank = rank;
				this.lastGame = lastGame;
				this.variant = variant;
			}

			// Token: 0x04003301 RID: 13057
			public string game;

			// Token: 0x04003302 RID: 13058
			public int nbGames;

			// Token: 0x04003303 RID: 13059
			public int karma;

			// Token: 0x04003304 RID: 13060
			public float rankScore;

			// Token: 0x04003305 RID: 13061
			public int rank;

			// Token: 0x04003306 RID: 13062
			public DateTime? lastGame;

			// Token: 0x04003307 RID: 13063
			public string variant;
		}

		// Token: 0x020009C7 RID: 2503
		public class Builder : Builder<User>
		{
			// Token: 0x060048D9 RID: 18649 RVA: 0x0014CCB0 File Offset: 0x0014AEB0
			public User.Builder Email(string email)
			{
				this._email = email;
				return this;
			}

			// Token: 0x060048DA RID: 18650 RVA: 0x0014CCBA File Offset: 0x0014AEBA
			public User.Builder LoginName(string loginName)
			{
				this._loginName = loginName;
				return this;
			}

			// Token: 0x060048DB RID: 18651 RVA: 0x0014CCC4 File Offset: 0x0014AEC4
			public User.Builder Password(string password)
			{
				this._password = password;
				return this;
			}

			// Token: 0x060048DC RID: 18652 RVA: 0x0014CCCE File Offset: 0x0014AECE
			public User.Builder Name(string name)
			{
				this._name = name;
				return this;
			}

			// Token: 0x060048DD RID: 18653 RVA: 0x0014CCD8 File Offset: 0x0014AED8
			public User.Builder Gender(User.UserGender? gender)
			{
				this._gender = gender;
				return this;
			}

			// Token: 0x060048DE RID: 18654 RVA: 0x0014CCE2 File Offset: 0x0014AEE2
			public User.Builder Birthday(DateTime? birthday)
			{
				this._birthday = birthday;
				return this;
			}

			// Token: 0x060048DF RID: 18655 RVA: 0x0014CCEC File Offset: 0x0014AEEC
			public User.Builder Zipcode(string zipcode)
			{
				this._zipcode = zipcode;
				return this;
			}

			// Token: 0x060048E0 RID: 18656 RVA: 0x0014CCF6 File Offset: 0x0014AEF6
			public User.Builder Country(string country)
			{
				this._country = country;
				return this;
			}

			// Token: 0x060048E1 RID: 18657 RVA: 0x0014CD00 File Offset: 0x0014AF00
			public User.Builder Language(LocalizationManager.Language? language)
			{
				this._language = language;
				return this;
			}

			// Token: 0x060048E2 RID: 18658 RVA: 0x0014CD0A File Offset: 0x0014AF0A
			public User.Builder TimeZone(string timezone)
			{
				this._timeZone = timezone;
				return this;
			}

			// Token: 0x060048E3 RID: 18659 RVA: 0x0014CD14 File Offset: 0x0014AF14
			public User.Builder Coppa(bool? coppa)
			{
				this._coppa = coppa;
				return this;
			}

			// Token: 0x060048E4 RID: 18660 RVA: 0x0014CD1E File Offset: 0x0014AF1E
			public User.Builder Newsletter(bool? newsletter)
			{
				this._newsletter = newsletter;
				return this;
			}

			// Token: 0x060048E5 RID: 18661 RVA: 0x0014CD28 File Offset: 0x0014AF28
			public User.Builder Avatar(string avatar)
			{
				this._avatar = avatar;
				return this;
			}

			// Token: 0x060048E6 RID: 18662 RVA: 0x0014CD32 File Offset: 0x0014AF32
			public User.Builder SendSignUpEmail(bool? sendSignUpEmail)
			{
				this._sendSignUpEmail = sendSignUpEmail;
				return this;
			}

			// Token: 0x060048E7 RID: 18663 RVA: 0x0014CD3C File Offset: 0x0014AF3C
			public User.Builder WC(string webcode)
			{
				this._wc = webcode;
				return this;
			}

			// Token: 0x060048E8 RID: 18664 RVA: 0x0014CD46 File Offset: 0x0014AF46
			public User.Builder UserId(int userId)
			{
				this._userId = userId;
				return this;
			}

			// Token: 0x060048E9 RID: 18665 RVA: 0x0014CD50 File Offset: 0x0014AF50
			public User.Builder EmailValid(bool? emailValid)
			{
				this._emailValid = emailValid;
				return this;
			}

			// Token: 0x060048EA RID: 18666 RVA: 0x0014CD5A File Offset: 0x0014AF5A
			public User.Builder PostMsgCount(int postMsgCount)
			{
				this._postedMsgCount = postMsgCount;
				return this;
			}

			// Token: 0x060048EB RID: 18667 RVA: 0x0014CD64 File Offset: 0x0014AF64
			public User.Builder LastPostId(int lastPostId)
			{
				this._lastPostId = lastPostId;
				return this;
			}

			// Token: 0x060048EC RID: 18668 RVA: 0x0014CD6E File Offset: 0x0014AF6E
			public User.Builder Validated(bool validated)
			{
				this._validated = new bool?(validated);
				return this;
			}

			// Token: 0x060048ED RID: 18669 RVA: 0x0014CD7D File Offset: 0x0014AF7D
			public User.Builder JoinDate(DateTime joinDate)
			{
				this._joinDate = new DateTime?(joinDate);
				return this;
			}

			// Token: 0x060048EE RID: 18670 RVA: 0x0014CD8C File Offset: 0x0014AF8C
			public User.Builder LastVisit(DateTime lastVisit)
			{
				this._lastVisit = new DateTime?(lastVisit);
				return this;
			}

			// Token: 0x060048EF RID: 18671 RVA: 0x0014CD9B File Offset: 0x0014AF9B
			public User.Builder Features(string[] features)
			{
				this._features = features;
				return this;
			}

			// Token: 0x060048F0 RID: 18672 RVA: 0x0014CDA5 File Offset: 0x0014AFA5
			public User.Builder BoardGames(User.BoardGame[] boardGames)
			{
				this._boardGames = boardGames;
				return this;
			}

			// Token: 0x060048F1 RID: 18673 RVA: 0x0014CDAF File Offset: 0x0014AFAF
			public User.Builder OnlineGames(User.OnlineGame[] onlineGames)
			{
				this._onlineGames = onlineGames;
				return this;
			}

			// Token: 0x060048F2 RID: 18674 RVA: 0x0014CDB9 File Offset: 0x0014AFB9
			public User.Builder Partners(PartnerAccount[] partners)
			{
				this._partners = partners;
				return this;
			}

			// Token: 0x17000A75 RID: 2677
			// (get) Token: 0x060048F3 RID: 18675 RVA: 0x0014CDC3 File Offset: 0x0014AFC3
			public string GetLoginName
			{
				get
				{
					return this._loginName;
				}
			}

			// Token: 0x17000A76 RID: 2678
			// (get) Token: 0x060048F4 RID: 18676 RVA: 0x0014CDCB File Offset: 0x0014AFCB
			public string GetCountry
			{
				get
				{
					return this._country;
				}
			}

			// Token: 0x17000A77 RID: 2679
			// (get) Token: 0x060048F5 RID: 18677 RVA: 0x0014CDD3 File Offset: 0x0014AFD3
			public LocalizationManager.Language? GetLanguage
			{
				get
				{
					return this._language;
				}
			}

			// Token: 0x17000A78 RID: 2680
			// (get) Token: 0x060048F6 RID: 18678 RVA: 0x0014CDDB File Offset: 0x0014AFDB
			public string GetTimeZone
			{
				get
				{
					return this._timeZone;
				}
			}

			// Token: 0x17000A79 RID: 2681
			// (get) Token: 0x060048F7 RID: 18679 RVA: 0x0014CDE3 File Offset: 0x0014AFE3
			public string GetAvatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x17000A7A RID: 2682
			// (get) Token: 0x060048F8 RID: 18680 RVA: 0x0014CDEB File Offset: 0x0014AFEB
			public string GetZipcode
			{
				get
				{
					return this._zipcode;
				}
			}

			// Token: 0x17000A7B RID: 2683
			// (get) Token: 0x060048F9 RID: 18681 RVA: 0x0014CDF3 File Offset: 0x0014AFF3
			public DateTime? GetBirthday
			{
				get
				{
					return this._birthday;
				}
			}

			// Token: 0x17000A7C RID: 2684
			// (get) Token: 0x060048FA RID: 18682 RVA: 0x0014CDFB File Offset: 0x0014AFFB
			public string GetEmail
			{
				get
				{
					return this._email;
				}
			}

			// Token: 0x17000A7D RID: 2685
			// (get) Token: 0x060048FB RID: 18683 RVA: 0x0014CE03 File Offset: 0x0014B003
			public User.UserGender? GetGender
			{
				get
				{
					return this._gender;
				}
			}

			// Token: 0x17000A7E RID: 2686
			// (get) Token: 0x060048FC RID: 18684 RVA: 0x0014CE0B File Offset: 0x0014B00B
			public bool? GetCoppa
			{
				get
				{
					return this._coppa;
				}
			}

			// Token: 0x17000A7F RID: 2687
			// (get) Token: 0x060048FD RID: 18685 RVA: 0x0014CE13 File Offset: 0x0014B013
			public string GetName
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000A80 RID: 2688
			// (get) Token: 0x060048FE RID: 18686 RVA: 0x0014CE1B File Offset: 0x0014B01B
			public bool? GetNewsletter
			{
				get
				{
					return this._newsletter;
				}
			}

			// Token: 0x17000A81 RID: 2689
			// (get) Token: 0x060048FF RID: 18687 RVA: 0x0014CE23 File Offset: 0x0014B023
			public string GetPassword
			{
				get
				{
					return this._password;
				}
			}

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x06004900 RID: 18688 RVA: 0x0014CE2B File Offset: 0x0014B02B
			public string GetWC
			{
				get
				{
					return this._wc;
				}
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x06004901 RID: 18689 RVA: 0x0014CE33 File Offset: 0x0014B033
			public bool? GetSendSignUpEmail
			{
				get
				{
					return this._sendSignUpEmail;
				}
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x06004902 RID: 18690 RVA: 0x0014CE3B File Offset: 0x0014B03B
			public int GetUserId
			{
				get
				{
					return this._userId;
				}
			}

			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x06004903 RID: 18691 RVA: 0x0014CE43 File Offset: 0x0014B043
			public bool? GetEmailValid
			{
				get
				{
					return this._emailValid;
				}
			}

			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x06004904 RID: 18692 RVA: 0x0014CE4B File Offset: 0x0014B04B
			public int GetPostedMsgCount
			{
				get
				{
					return this._postedMsgCount;
				}
			}

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x06004905 RID: 18693 RVA: 0x0014CE53 File Offset: 0x0014B053
			public int GetLastPostId
			{
				get
				{
					return this._lastPostId;
				}
			}

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x06004906 RID: 18694 RVA: 0x0014CE5B File Offset: 0x0014B05B
			public bool? GetValidated
			{
				get
				{
					return this._validated;
				}
			}

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x06004907 RID: 18695 RVA: 0x0014CE63 File Offset: 0x0014B063
			public DateTime? GetJoinDate
			{
				get
				{
					return this._joinDate;
				}
			}

			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x06004908 RID: 18696 RVA: 0x0014CE6B File Offset: 0x0014B06B
			public DateTime? GetLastVisit
			{
				get
				{
					return this._lastVisit;
				}
			}

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x06004909 RID: 18697 RVA: 0x0014CE73 File Offset: 0x0014B073
			public string[] GetFeatures
			{
				get
				{
					return this._features;
				}
			}

			// Token: 0x17000A8C RID: 2700
			// (get) Token: 0x0600490A RID: 18698 RVA: 0x0014CE7B File Offset: 0x0014B07B
			public User.BoardGame[] GetBoardGame
			{
				get
				{
					return this._boardGames;
				}
			}

			// Token: 0x17000A8D RID: 2701
			// (get) Token: 0x0600490B RID: 18699 RVA: 0x0014CE83 File Offset: 0x0014B083
			public User.OnlineGame[] GetOnlineGames
			{
				get
				{
					return this._onlineGames;
				}
			}

			// Token: 0x17000A8E RID: 2702
			// (get) Token: 0x0600490C RID: 18700 RVA: 0x0014CE8B File Offset: 0x0014B08B
			public PartnerAccount[] GetPartners
			{
				get
				{
					return this._partners;
				}
			}

			// Token: 0x0600490D RID: 18701 RVA: 0x0014CE93 File Offset: 0x0014B093
			public Builder()
			{
			}

			// Token: 0x0600490E RID: 18702 RVA: 0x0014CEB0 File Offset: 0x0014B0B0
			public Builder(ApiGetUserDetailsResponse raw)
			{
				this._userId = raw.data.user.user_id;
				this._loginName = ((raw.data.user.login_name == string.Empty) ? null : raw.data.user.login_name);
				this._country = ((raw.data.user.country == string.Empty) ? null : raw.data.user.country);
				this._emailValid = new bool?(raw.data.user.email_valid);
				if (string.IsNullOrEmpty(raw.data.user.language))
				{
					this._language = null;
				}
				else
				{
					this._language = new LocalizationManager.Language?(CoreApplication.Instance.LocalizationManager.GetLanguageFromString(raw.data.user.language));
				}
				this._timeZone = ((raw.data.user.time_zone == string.Empty) ? null : raw.data.user.time_zone);
				this._postedMsgCount = raw.data.user.posted_msg_count;
				this._lastPostId = raw.data.user.last_post_id;
				this._validated = new bool?(raw.data.user.validated);
				this._avatar = ((raw.data.user.avatar == string.Empty) ? null : raw.data.user.avatar);
				this._joinDate = (string.IsNullOrEmpty(raw.data.user.join_date) ? null : new DateTime?(DateTime.Parse(raw.data.user.join_date)));
				this._lastVisit = (string.IsNullOrEmpty(raw.data.user.last_visit) ? null : new DateTime?(DateTime.Parse(raw.data.user.last_visit)));
				this._zipcode = ((raw.data.user.zipcode == string.Empty) ? null : raw.data.user.zipcode);
				this._email = ((raw.data.user.email == string.Empty) ? null : raw.data.user.email);
				this._gender = (string.IsNullOrEmpty(raw.data.user.gender) ? null : new User.UserGender?((User.UserGender)Enum.Parse(typeof(User.UserGender), raw.data.user.gender)));
				this._coppa = new bool?(raw.data.user.coppa);
				this._name = ((raw.data.user.name == string.Empty) ? null : raw.data.user.name);
				if (raw.data.user.features != null)
				{
					this._features = (raw.data.user.features.Clone() as string[]);
				}
				if (raw.data.user.boardgames != null)
				{
					this._boardGames = (from x in raw.data.user.boardgames
					select new User.BoardGame(x.code, x.name, string.IsNullOrEmpty(x.registered_date) ? null : new DateTime?(DateTime.Parse(x.registered_date)))).ToArray<User.BoardGame>();
				}
				if (raw.data.user.onlinegames != null)
				{
					this._onlineGames = (from x in raw.data.user.onlinegames
					select new User.OnlineGame(x.game, x.nbgames, x.karma, x.rankscore, x.rank, string.IsNullOrEmpty(x.lastgame) ? null : new DateTime?(DateTime.Parse(x.lastgame)), x.variant)).ToArray<User.OnlineGame>();
				}
				if (raw.data.user.partners != null)
				{
					this._partners = (from x in raw.data.user.partners
					select new PartnerAccount(x.partner_id, x.partner_user_id, string.IsNullOrEmpty(x.created_at) ? null : new DateTime?(DateTime.Parse(x.created_at)))).ToArray<PartnerAccount>();
				}
				try
				{
					this._birthday = (string.IsNullOrEmpty(raw.data.user.birthday) ? null : new DateTime?(DateTime.Parse(raw.data.user.birthday)));
				}
				catch (FormatException)
				{
					this._birthday = null;
				}
			}

			// Token: 0x0600490F RID: 18703 RVA: 0x0014D38C File Offset: 0x0014B58C
			public override Builder<User>.BuilderErrors[] Validate()
			{
				List<Builder<User>.BuilderErrors> list = new List<Builder<User>.BuilderErrors>();
				if (this._loginName != null && (this._loginName.Length < 5 || this._loginName.Length > 50))
				{
					list.Add(new Builder<User>.BuilderErrors("LoginName", string.Format("User LoginName length must be not be less than {0} characters and more than {1} characters", 5, 50)));
				}
				if (this._loginName != null)
				{
					if (this._loginName.Any((char x) => "()#|@^*%§!?:;.,$~".Contains(x)))
					{
						list.Add(new Builder<User>.BuilderErrors("LoginName", string.Format("User LoginName cannot contain any character in the following set : '{0}'", "()#|@^*%§!?:;.,$~")));
					}
				}
				if (this._name != null && (this._name.Length < 5 || this._name.Length > 80))
				{
					list.Add(new Builder<User>.BuilderErrors("Name", string.Format("User Name length must be not be less than {0} characters and more than {1} characters", 5, 80)));
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
				return null;
			}

			// Token: 0x04003308 RID: 13064
			private string _email;

			// Token: 0x04003309 RID: 13065
			private string _loginName;

			// Token: 0x0400330A RID: 13066
			private string _password;

			// Token: 0x0400330B RID: 13067
			private string _name;

			// Token: 0x0400330C RID: 13068
			private User.UserGender? _gender;

			// Token: 0x0400330D RID: 13069
			private DateTime? _birthday;

			// Token: 0x0400330E RID: 13070
			private string _zipcode;

			// Token: 0x0400330F RID: 13071
			private string _country;

			// Token: 0x04003310 RID: 13072
			private LocalizationManager.Language? _language;

			// Token: 0x04003311 RID: 13073
			private string _timeZone;

			// Token: 0x04003312 RID: 13074
			private bool? _coppa;

			// Token: 0x04003313 RID: 13075
			private bool? _newsletter;

			// Token: 0x04003314 RID: 13076
			private string _avatar;

			// Token: 0x04003315 RID: 13077
			private bool? _sendSignUpEmail;

			// Token: 0x04003316 RID: 13078
			private string _wc;

			// Token: 0x04003317 RID: 13079
			private int _userId = -1;

			// Token: 0x04003318 RID: 13080
			private bool? _emailValid;

			// Token: 0x04003319 RID: 13081
			private int _postedMsgCount = -1;

			// Token: 0x0400331A RID: 13082
			private int _lastPostId = -1;

			// Token: 0x0400331B RID: 13083
			private bool? _validated;

			// Token: 0x0400331C RID: 13084
			private DateTime? _joinDate;

			// Token: 0x0400331D RID: 13085
			private DateTime? _lastVisit;

			// Token: 0x0400331E RID: 13086
			private string[] _features;

			// Token: 0x0400331F RID: 13087
			private User.BoardGame[] _boardGames;

			// Token: 0x04003320 RID: 13088
			private User.OnlineGame[] _onlineGames;

			// Token: 0x04003321 RID: 13089
			private PartnerAccount[] _partners;

			// Token: 0x04003322 RID: 13090
			public const int kLoginNameMinimalLength = 5;

			// Token: 0x04003323 RID: 13091
			public const int kLoginNameMaximalLength = 50;

			// Token: 0x04003324 RID: 13092
			public const int kNameMinimalLength = 5;

			// Token: 0x04003325 RID: 13093
			public const int kNameMaximalLength = 80;

			// Token: 0x04003326 RID: 13094
			public const string kLoginNameForbbidenCharacters = "()#|@^*%§!?:;.,$~";
		}
	}
}
