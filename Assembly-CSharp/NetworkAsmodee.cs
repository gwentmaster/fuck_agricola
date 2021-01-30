using System;
using System.Collections;
using System.Text;
using AsmodeeNet.Analytics;
using UnityEngine;
using UnityEngine.Networking;
using UTNotifications;

// Token: 0x020000BC RID: 188
public class NetworkAsmodee : MonoBehaviour
{
	// Token: 0x060006D1 RID: 1745 RVA: 0x0003384F File Offset: 0x00031A4F
	public int GetVerifyAsmodeeResult()
	{
		return this.m_VerifyAsmodeeResult;
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00033857 File Offset: 0x00031A57
	public int GetVerifyPlaydekResult()
	{
		return this.m_VerifyPlaydekResult;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x0003385F File Offset: 0x00031A5F
	public string GetVerifyError()
	{
		return this.m_VerifyError;
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00033867 File Offset: 0x00031A67
	public int GetCreateAsmodeeResult()
	{
		return this.m_CreateAsmodeeResult;
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0003386F File Offset: 0x00031A6F
	public string GetCreateError()
	{
		return this.m_CreateError;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00033877 File Offset: 0x00031A77
	public int GetLinkAccountResult()
	{
		return this.m_LinkAccountResult;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0003387F File Offset: 0x00031A7F
	public string GetLinkError()
	{
		return this.m_LinkError;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x00033887 File Offset: 0x00031A87
	private void Awake()
	{
		if (NetworkAsmodee.m_NetworkAsmodee == null)
		{
			NetworkAsmodee.m_NetworkAsmodee = this;
			NetworkAsmodee.m_asmodeeAccessToken = string.Empty;
			return;
		}
		if (NetworkAsmodee.m_NetworkAsmodee != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000338BF File Offset: 0x00031ABF
	private void Start()
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000338E4 File Offset: 0x00031AE4
	private void OnDestroy()
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0003390C File Offset: 0x00031B0C
	private void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (eventType == NetworkEvent.EventType.Event_VerifyAccount)
		{
			Debug.Log("NetworkAsmodee: Event_VerifyAccount");
			this.m_VerifyPlaydekResult = eventData;
			if (eventData == 0)
			{
				this.m_VerifyError = "${LINK_INVALID_EMAIL_PASSWORD}";
				return;
			}
			if (eventData == 3)
			{
				this.m_VerifyError = "${LINK_ASMODEE_ACCOUNT_ALREADY}";
			}
			return;
		}
		else
		{
			if (eventType == NetworkEvent.EventType.Event_LinkAccount)
			{
				Debug.Log("NetworkAsmodee: Event_LinkAccount");
				this.m_LinkAccountResult = eventData;
				return;
			}
			return;
		}
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00033965 File Offset: 0x00031B65
	public IEnumerator RequestAsmodeePublicAccessToken()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("grant_type", "client_credentials");
		wwwform.AddField("client_id", AsmodeeNetSettings.GetClientID());
		wwwform.AddField("client_secret", AsmodeeNetSettings.GetClientSecret());
		string access_token = string.Empty;
		string token_type = string.Empty;
		string empty = string.Empty;
		UnityWebRequest post_request = UnityWebRequest.Post("https://api.asmodee.net/main/v2/oauth/token", wwwform);
		post_request.chunkedTransfer = false;
		post_request.useHttpContinue = false;
		yield return post_request.SendWebRequest();
		if (post_request.isNetworkError || post_request.isHttpError)
		{
			Debug.LogError("There was an error sending request: " + post_request.error);
		}
		else
		{
			string text = post_request.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				JSONNode jsonnode = JSON.Parse(text);
				if (jsonnode != null)
				{
					JSONNode jsonnode2 = jsonnode["access_token"];
					if (jsonnode2 != null)
					{
						access_token = jsonnode2.Value;
					}
					JSONNode jsonnode3 = jsonnode["token_type"];
					if (jsonnode3 != null)
					{
						token_type = jsonnode3.Value;
					}
					NetworkAsmodee.m_asmodeeAccessToken = string.Format("{0} {1}", token_type, access_token);
				}
			}
		}
		yield break;
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0003396D File Offset: 0x00031B6D
	public void RequestVerifyPlaydekAcount(string verifyEmail, string verifyPassword)
	{
		this.m_VerifyPlaydekResult = -1;
		this.m_VerifyError = string.Empty;
		AgricolaLib.NetworkVerifyAccount(verifyEmail, verifyPassword);
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00033988 File Offset: 0x00031B88
	private IEnumerator RequestAsmodeeAccessToken(string verifyLoginName, string verifyPassword)
	{
		string access_token = string.Empty;
		string token_type = string.Empty;
		string empty = string.Empty;
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("grant_type", "password");
		wwwform.AddField("client_id", AsmodeeNetSettings.GetClientID());
		wwwform.AddField("client_secret", AsmodeeNetSettings.GetClientSecret());
		wwwform.AddField("username", verifyLoginName);
		wwwform.AddField("password", verifyPassword);
		UnityWebRequest post_request = UnityWebRequest.Post("https://api.asmodee.net/main/v2/oauth/token", wwwform);
		post_request.chunkedTransfer = false;
		post_request.useHttpContinue = false;
		yield return post_request.SendWebRequest();
		if (post_request.isNetworkError || post_request.isHttpError)
		{
			Debug.LogError("There was an error sending request: " + post_request.error);
			string text = post_request.downloadHandler.text;
			if (post_request.responseCode != 0L)
			{
				this.m_VerifyError = "${LINK_REQUEST_ERROR_" + post_request.responseCode.ToString() + "}";
			}
			else if (!string.IsNullOrEmpty(text))
			{
				JSONNode jsonnode = JSON.Parse(text);
				if (jsonnode != null)
				{
					JSONNode jsonnode2 = jsonnode["error_description"];
					if (jsonnode2 != null)
					{
						this.m_VerifyError = jsonnode2.Value;
					}
				}
			}
			if (string.IsNullOrEmpty(this.m_VerifyError))
			{
				this.m_VerifyError = "${LINK_REQUEST_ERROR}";
			}
			this.m_VerifyAsmodeeAccessToken = 0;
		}
		else
		{
			string text2 = post_request.downloadHandler.text;
			if (!string.IsNullOrEmpty(text2))
			{
				JSONNode jsonnode3 = JSON.Parse(text2);
				if (jsonnode3 != null)
				{
					JSONNode jsonnode4 = jsonnode3["status"];
					JSONNode jsonnode5 = jsonnode3["access_token"];
					if (jsonnode5 != null)
					{
						access_token = jsonnode5.Value;
					}
					JSONNode jsonnode6 = jsonnode3["token_type"];
					if (jsonnode6 != null)
					{
						token_type = jsonnode6.Value;
					}
				}
				NetworkAsmodee.m_asmodeeAccessToken = string.Format("{0} {1}", token_type, access_token);
			}
			this.m_VerifyError = string.Empty;
			this.m_VerifyAsmodeeAccessToken = 1;
		}
		yield break;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x000339A5 File Offset: 0x00031BA5
	public IEnumerator RequestAsmodeeAccountInformation()
	{
		UnityWebRequest post_request = UnityWebRequest.Get("https://api.asmodee.net/main/v1/user/me?extras=partners");
		post_request.SetRequestHeader("Authorization", NetworkAsmodee.m_asmodeeAccessToken);
		post_request.chunkedTransfer = false;
		post_request.useHttpContinue = false;
		yield return post_request.SendWebRequest();
		if (post_request.isNetworkError || post_request.isHttpError)
		{
			string text = post_request.downloadHandler.text;
			Debug.LogError("There was an error sending request: " + text);
			if (post_request.responseCode != 0L)
			{
				this.m_VerifyError = "${LINK_REQUEST_ERROR_" + post_request.responseCode.ToString() + "}";
			}
			this.m_VerifyAsmodeeResult = 0;
		}
		else
		{
			string text2 = post_request.downloadHandler.text;
			if (!string.IsNullOrEmpty(text2))
			{
				JSONNode jsonnode = JSON.Parse(text2);
				if (jsonnode != null)
				{
					JSONNode jsonnode2 = jsonnode["data"];
					if (jsonnode2 != null)
					{
						JSONNode jsonnode3 = jsonnode2["user"];
						if (jsonnode3 != null)
						{
							JSONNode jsonnode4 = jsonnode3["login_name"];
							if (jsonnode4 != null)
							{
								this.m_AsmodeeLoginName = jsonnode4.Value;
							}
							JSONNode jsonnode5 = jsonnode3["email"];
							if (jsonnode5 != null)
							{
								this.m_AsmodeeEmail = jsonnode5.Value;
							}
							JSONNode jsonnode6 = jsonnode3["user_id"];
							if (jsonnode6 != null)
							{
								long asmodeeUserID = 0L;
								if (long.TryParse(jsonnode6.Value, out asmodeeUserID))
								{
									this.m_AsmodeeUserID = asmodeeUserID;
								}
							}
							JSONNode jsonnode7 = jsonnode3["partners"];
							if (jsonnode7 != null)
							{
								for (int i = 0; i < jsonnode7.Count; i++)
								{
									JSONNode jsonnode8 = jsonnode7[i];
									if (jsonnode8 != null)
									{
										JSONNode jsonnode9 = jsonnode8["partner_id"];
										if (jsonnode9 != null && jsonnode9.AsInt == AsmodeeNetSettings.GetPartnerType())
										{
											JSONNode jsonnode10 = jsonnode8["partner_user_id"];
											if (jsonnode10 != null)
											{
												this.m_AsmodeePartnerUserID = jsonnode10.AsInt;
												break;
											}
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			if (this.m_AsmodeeUserID == 0L)
			{
				this.m_VerifyError = "${LINK_INVALID_USERNAME_PASSWORD}";
				this.m_VerifyAsmodeeResult = 0;
			}
			else if (this.m_AsmodeePartnerUserID != 0)
			{
				this.m_VerifyError = "${LINK_ASMODEE_ACCOUNT_ALREADY}";
				this.m_VerifyAsmodeeResult = 3;
			}
			else
			{
				this.m_VerifyError = string.Empty;
				this.m_VerifyAsmodeeResult = 1;
			}
		}
		yield break;
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000339B4 File Offset: 0x00031BB4
	private IEnumerator VerifyAsmodeeAccount(string verifyLoginName, string verifyPassword)
	{
		yield return this.RequestAsmodeeAccessToken(verifyLoginName, verifyPassword);
		if (this.m_VerifyAsmodeeAccessToken != 1 || string.IsNullOrEmpty(NetworkAsmodee.m_asmodeeAccessToken))
		{
			this.m_VerifyAsmodeeResult = 0;
			yield break;
		}
		yield return this.RequestAsmodeeAccountInformation();
		yield break;
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000339D4 File Offset: 0x00031BD4
	public void RequestVerifyAsmodeeAccount(string verifyLoginName, string verifyPassword)
	{
		this.m_VerifyAsmodeeAccessToken = -1;
		this.m_VerifyAsmodeeResult = -1;
		this.m_VerifyError = string.Empty;
		this.m_AsmodeeLoginName = verifyLoginName;
		this.m_AsmodeeEmail = string.Empty;
		this.m_AsmodeePassword = verifyPassword;
		this.m_AsmodeeUserID = 0L;
		this.m_AsmodeePartnerUserID = 0;
		base.StartCoroutine(this.VerifyAsmodeeAccount(verifyLoginName, verifyPassword));
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00033A31 File Offset: 0x00031C31
	private IEnumerator CreateAsmodeeAccount(string createEmail, string createPassword, string createLoginName)
	{
		if (string.IsNullOrEmpty(NetworkAsmodee.m_asmodeeAccessToken))
		{
			yield return this.RequestAsmodeePublicAccessToken();
			if (string.IsNullOrEmpty(NetworkAsmodee.m_asmodeeAccessToken))
			{
				this.m_CreateAsmodeeResult = 0;
				this.m_CreateError = "${LINK_FAIL_CONNECT_ASMODEE}";
				yield break;
			}
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string s = string.Format("{{ \"email\":\"{0}\", \"login_name\":\"{1}\", \"password\":\"{2}\" }}", createEmail, createLoginName, createPassword);
		UnityWebRequest post_request = new UnityWebRequest("https://api.asmodee.net/main/v2/user", "POST");
		byte[] bytes = new UTF8Encoding().GetBytes(s);
		post_request.uploadHandler = new UploadHandlerRaw(bytes);
		post_request.downloadHandler = new DownloadHandlerBuffer();
		post_request.SetRequestHeader("Content-Type", "application/json");
		post_request.SetRequestHeader("Authorization", NetworkAsmodee.m_asmodeeAccessToken);
		post_request.chunkedTransfer = false;
		post_request.useHttpContinue = false;
		yield return post_request.SendWebRequest();
		if (post_request.isNetworkError || post_request.isHttpError)
		{
			Debug.LogError("There was an error sending request: " + post_request.error);
			string text = post_request.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				JSONNode jsonnode = JSON.Parse(text);
				if (jsonnode != null)
				{
					JSONNode jsonnode2 = jsonnode["error_code"];
					if (jsonnode2 != null && !string.IsNullOrEmpty(jsonnode2.Value))
					{
						this.m_CreateError = "${LINK_CREATE_ERROR_" + jsonnode2.Value + "}";
					}
					else
					{
						JSONNode jsonnode3 = jsonnode["error_description"];
						if (jsonnode3 != null)
						{
							this.m_CreateError = jsonnode3.Value;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(this.m_CreateError))
			{
				this.m_CreateError = "${LINK_REQUEST_ERROR}";
			}
			this.m_CreateAsmodeeResult = 0;
		}
		else
		{
			string text2 = post_request.downloadHandler.text;
			if (!string.IsNullOrEmpty(text2))
			{
				JSONNode jsonnode4 = JSON.Parse(text2);
				if (jsonnode4 != null)
				{
					string a = string.Empty;
					JSONNode jsonnode5 = jsonnode4["status"];
					if (jsonnode5 != null)
					{
						a = jsonnode5.Value;
					}
					if (a != "200")
					{
						JSONNode jsonnode6 = jsonnode4["error_code"];
						if (jsonnode6 != null && !string.IsNullOrEmpty(jsonnode6.Value))
						{
							this.m_CreateError = "${LINK_CREATE_ERROR_" + jsonnode6.Value + "}";
						}
						else
						{
							JSONNode jsonnode7 = jsonnode4["error_description"];
							if (jsonnode7 != null)
							{
								this.m_CreateError = jsonnode7.Value;
							}
						}
						if (string.IsNullOrEmpty(this.m_CreateError))
						{
							this.m_CreateError = "${LINK_REQUEST_ERROR}";
						}
						this.m_CreateAsmodeeResult = 0;
					}
				}
			}
			this.m_VerifyAsmodeeAccessToken = -1;
			this.m_VerifyAsmodeeResult = -1;
			this.m_VerifyError = string.Empty;
			yield return this.VerifyAsmodeeAccount(createLoginName, createPassword);
			if (this.m_VerifyAsmodeeResult != 1)
			{
				this.m_CreateError = this.m_VerifyError;
				this.m_CreateAsmodeeResult = 0;
			}
			else
			{
				AnalyticsEvents.LogConnectAsmodeeNetStartEvent("create_account", null);
				AnalyticsEvents.LogCreateAsmodeeNetAccountEvent(this.m_bOptInToEmail, null);
				AnalyticsEvents.LogConnectAsmodeeNetStopEvent(true, "null", false, null);
				this.m_CreateAsmodeeResult = 1;
			}
		}
		yield break;
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00033A58 File Offset: 0x00031C58
	public void RequestCreateAsmodeeAccount(string createEmail, string createPassword, string createLoginName)
	{
		this.m_CreateAsmodeeResult = -1;
		this.m_CreateError = string.Empty;
		this.m_AsmodeeLoginName = createLoginName;
		this.m_AsmodeeEmail = createEmail;
		this.m_AsmodeePassword = createPassword;
		this.m_AsmodeeUserID = 0L;
		this.m_AsmodeePartnerUserID = 0;
		base.StartCoroutine(this.CreateAsmodeeAccount(createEmail, createPassword, createLoginName));
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00033AAB File Offset: 0x00031CAB
	public void RequestLinkAccount(string playdekEmail, string playdekPassword)
	{
		this.m_LinkAccountResult = -1;
		this.m_LinkError = string.Empty;
		AgricolaLib.NetworkLinkAccount(playdekEmail, playdekPassword, this.m_AsmodeeUserID, NetworkAsmodee.m_asmodeeAccessToken);
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00033AD1 File Offset: 0x00031CD1
	public void RequestLinkAccountPlaceholder()
	{
		this.m_LinkAccountResult = -1;
		this.m_LinkError = string.Empty;
		AgricolaLib.NetworkLinkAccountPlaceholder(this.m_AsmodeeEmail, this.m_AsmodeePassword, this.m_AsmodeeLoginName, this.m_AsmodeeUserID, NetworkAsmodee.m_asmodeeAccessToken);
	}

	// Token: 0x04000815 RID: 2069
	public static NetworkAsmodee m_NetworkAsmodee;

	// Token: 0x04000816 RID: 2070
	private static string m_asmodeeAccessToken;

	// Token: 0x04000817 RID: 2071
	private int m_VerifyAsmodeeAccessToken = -1;

	// Token: 0x04000818 RID: 2072
	private int m_VerifyAsmodeeResult = -1;

	// Token: 0x04000819 RID: 2073
	private int m_VerifyPlaydekResult = -1;

	// Token: 0x0400081A RID: 2074
	private string m_VerifyError = string.Empty;

	// Token: 0x0400081B RID: 2075
	private int m_CreateAsmodeeResult = -1;

	// Token: 0x0400081C RID: 2076
	private string m_CreateError = string.Empty;

	// Token: 0x0400081D RID: 2077
	private int m_LinkAccountResult = -1;

	// Token: 0x0400081E RID: 2078
	private string m_LinkError = string.Empty;

	// Token: 0x0400081F RID: 2079
	private string m_AsmodeeLoginName = string.Empty;

	// Token: 0x04000820 RID: 2080
	private string m_AsmodeeEmail = string.Empty;

	// Token: 0x04000821 RID: 2081
	private string m_AsmodeePassword = string.Empty;

	// Token: 0x04000822 RID: 2082
	private long m_AsmodeeUserID;

	// Token: 0x04000823 RID: 2083
	private int m_AsmodeePartnerUserID;

	// Token: 0x04000824 RID: 2084
	private bool m_bOptInToEmail;
}
