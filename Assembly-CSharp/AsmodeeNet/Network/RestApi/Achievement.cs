using System;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000696 RID: 1686
	[Serializable]
	public class Achievement
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x0012DF58 File Offset: 0x0012C158
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0012DF60 File Offset: 0x0012C160
		public string Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x0012DF68 File Offset: 0x0012C168
		public AchievementStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x0012DF70 File Offset: 0x0012C170
		public AchievementUnicity Unicity
		{
			get
			{
				return this._unicity;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x0012DF78 File Offset: 0x0012C178
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003D50 RID: 15696 RVA: 0x0012DF80 File Offset: 0x0012C180
		public AchievementType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x0012DF88 File Offset: 0x0012C188
		public bool Secret
		{
			get
			{
				return this._secret;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003D52 RID: 15698 RVA: 0x0012DF90 File Offset: 0x0012C190
		public int Treasure
		{
			get
			{
				return this._treasure;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x0012DF98 File Offset: 0x0012C198
		public int Category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x0012DFA0 File Offset: 0x0012C1A0
		public string Picture
		{
			get
			{
				return this._picture;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003D55 RID: 15701 RVA: 0x0012DFA8 File Offset: 0x0012C1A8
		public string Ribbon
		{
			get
			{
				return this._ribbon;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x0012DFB0 File Offset: 0x0012C1B0
		public Achievement.Text[] Texts
		{
			get
			{
				return this._texts;
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x0012DFB8 File Offset: 0x0012C1B8
		public Achievement(JsonAchievement raw)
		{
			this._id = raw.id;
			this._tag = raw.tag;
			this._status = ((raw.status == null) ? AchievementStatus.Null : ((AchievementStatus)Enum.Parse(typeof(AchievementStatus), this._FirstWordLetterToUpper(raw.status))));
			this._unicity = ((raw.unicity == null) ? AchievementUnicity.Null : ((AchievementUnicity)Enum.Parse(typeof(AchievementUnicity), this._FirstWordLetterToUpper(raw.unicity))));
			this._game = raw.game;
			this._type = ((raw.type == null) ? AchievementType.Null : ((AchievementType)Enum.Parse(typeof(AchievementType), this._FirstWordLetterToUpper(raw.type))));
			this._secret = raw.secret;
			this._treasure = raw.treasure;
			this._category = raw.category;
			this._picture = raw.picture;
			this._ribbon = raw.ribbon;
			this._texts = (from x in raw.texts
			select new Achievement.Text(x)).ToArray<Achievement.Text>();
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x0012E0F4 File Offset: 0x0012C2F4
		public override bool Equals(object obj)
		{
			Achievement achievement = obj as Achievement;
			return achievement != null && (this.Id == achievement.Id && this.Tag == achievement.Tag && this.Status == achievement.Status && this.Unicity == achievement.Unicity && this.Game == achievement.Game && this.Type == achievement.Type && this.Secret == achievement.Secret && this.Treasure == achievement.Treasure && this.Category == achievement.Category && this.Picture == achievement.Picture && this.Ribbon == achievement.Ribbon) && this.Texts.Diff(achievement.Texts).Count<Achievement.Text>() == 0;
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0012E1E4 File Offset: 0x0012C3E4
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Tag == null) ? 0 : this.Tag.GetHashCode()) ^ (int)this.Status ^ (int)this.Unicity ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ (int)this.Type ^ this.Secret.GetHashCode() ^ this.Treasure ^ this.Category ^ ((this.Picture == null) ? 0 : this.Picture.GetHashCode()) ^ ((this.Ribbon == null) ? 0 : this.Ribbon.GetHashCode());
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0012E288 File Offset: 0x0012C488
		private string _FirstWordLetterToUpper(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			return char.ToUpper(s[0]).ToString() + s.Substring(1).ToLower();
		}

		// Token: 0x04002745 RID: 10053
		[SerializeField]
		private int _id;

		// Token: 0x04002746 RID: 10054
		[SerializeField]
		private string _tag;

		// Token: 0x04002747 RID: 10055
		[SerializeField]
		private AchievementStatus _status;

		// Token: 0x04002748 RID: 10056
		[SerializeField]
		private AchievementUnicity _unicity;

		// Token: 0x04002749 RID: 10057
		[SerializeField]
		private string _game;

		// Token: 0x0400274A RID: 10058
		[SerializeField]
		private AchievementType _type;

		// Token: 0x0400274B RID: 10059
		[SerializeField]
		private bool _secret;

		// Token: 0x0400274C RID: 10060
		[SerializeField]
		private int _treasure;

		// Token: 0x0400274D RID: 10061
		[SerializeField]
		private int _category;

		// Token: 0x0400274E RID: 10062
		[SerializeField]
		private string _picture;

		// Token: 0x0400274F RID: 10063
		[SerializeField]
		private string _ribbon;

		// Token: 0x04002750 RID: 10064
		[SerializeField]
		private Achievement.Text[] _texts;

		// Token: 0x0200099F RID: 2463
		[Serializable]
		public class Text
		{
			// Token: 0x17000A5A RID: 2650
			// (get) Token: 0x06004870 RID: 18544 RVA: 0x0014C2BE File Offset: 0x0014A4BE
			public string Lang
			{
				get
				{
					return this._lang;
				}
			}

			// Token: 0x17000A5B RID: 2651
			// (get) Token: 0x06004871 RID: 18545 RVA: 0x0014C2C6 File Offset: 0x0014A4C6
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000A5C RID: 2652
			// (get) Token: 0x06004872 RID: 18546 RVA: 0x0014C2CE File Offset: 0x0014A4CE
			public string Description
			{
				get
				{
					return this._description;
				}
			}

			// Token: 0x06004873 RID: 18547 RVA: 0x0014C2D6 File Offset: 0x0014A4D6
			public Text(JsonAchievement.Text raw)
			{
				this._lang = raw.lang;
				this._name = raw.name;
				this._description = raw.description;
			}

			// Token: 0x06004874 RID: 18548 RVA: 0x0014C304 File Offset: 0x0014A504
			public override bool Equals(object obj)
			{
				Achievement.Text text = obj as Achievement.Text;
				return text != null && (this.Lang == text.Lang && this.Name == text.Name) && this.Description == text.Description;
			}

			// Token: 0x06004875 RID: 18549 RVA: 0x0014C358 File Offset: 0x0014A558
			public override int GetHashCode()
			{
				return ((this.Lang == null) ? 0 : this.Lang.GetHashCode()) ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ ((this.Description == null) ? 0 : this.Description.GetHashCode());
			}

			// Token: 0x0400329F RID: 12959
			[SerializeField]
			private string _lang;

			// Token: 0x040032A0 RID: 12960
			[SerializeField]
			private string _name;

			// Token: 0x040032A1 RID: 12961
			[SerializeField]
			private string _description;
		}
	}
}
