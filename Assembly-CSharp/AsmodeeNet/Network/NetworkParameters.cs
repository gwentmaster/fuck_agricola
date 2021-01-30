using System;
using UnityEngine;

namespace AsmodeeNet.Network
{
	// Token: 0x02000681 RID: 1665
	[CreateAssetMenu]
	public class NetworkParameters : ScriptableObject
	{
		// Token: 0x06003CEA RID: 15594 RVA: 0x0012C6A8 File Offset: 0x0012A8A8
		public string GetApiBaseUrl()
		{
			if (this.RestAPIHostName.StartsWith("http"))
			{
				return this.RestAPIHostName;
			}
			return "https://" + this.RestAPIHostName;
		}

		// Token: 0x04002712 RID: 10002
		[Header("Scalable Server")]
		public string HostName = string.Empty;

		// Token: 0x04002713 RID: 10003
		public int HostPort;

		// Token: 0x04002714 RID: 10004
		[HideInInspector]
		public bool UseSSL = true;

		// Token: 0x04002715 RID: 10005
		public string[] PinPublicKeys = new string[1];

		// Token: 0x04002716 RID: 10006
		public float PingDelay = 10f;

		// Token: 0x04002717 RID: 10007
		public float AutoReconnectDelay = 6f;

		// Token: 0x04002718 RID: 10008
		public string GameType = string.Empty;

		// Token: 0x04002719 RID: 10009
		[Header("REST API")]
		public string ClientId = string.Empty;

		// Token: 0x0400271A RID: 10010
		public string ClientSecret = string.Empty;

		// Token: 0x0400271B RID: 10011
		public string RestAPIHostName = string.Empty;

		// Token: 0x0400271C RID: 10012
		public string[] RestAPIPinPublicKeys = new string[1];
	}
}
