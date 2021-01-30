using System;

namespace BestHTTP.Authentication
{
	// Token: 0x02000619 RID: 1561
	public sealed class Credentials
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x0011DB94 File Offset: 0x0011BD94
		// (set) Token: 0x0600396B RID: 14699 RVA: 0x0011DB9C File Offset: 0x0011BD9C
		public AuthenticationTypes Type { get; private set; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x0011DBA5 File Offset: 0x0011BDA5
		// (set) Token: 0x0600396D RID: 14701 RVA: 0x0011DBAD File Offset: 0x0011BDAD
		public string UserName { get; private set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x0011DBB6 File Offset: 0x0011BDB6
		// (set) Token: 0x0600396F RID: 14703 RVA: 0x0011DBBE File Offset: 0x0011BDBE
		public string Password { get; private set; }

		// Token: 0x06003970 RID: 14704 RVA: 0x0011DBC7 File Offset: 0x0011BDC7
		public Credentials(string userName, string password) : this(AuthenticationTypes.Unknown, userName, password)
		{
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x0011DBD2 File Offset: 0x0011BDD2
		public Credentials(AuthenticationTypes type, string userName, string password)
		{
			this.Type = type;
			this.UserName = userName;
			this.Password = password;
		}
	}
}
