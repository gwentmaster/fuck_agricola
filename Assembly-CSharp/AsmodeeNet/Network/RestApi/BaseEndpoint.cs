using System;
using System.Collections;
using System.Text;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using BestHTTP;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006CC RID: 1740
	public abstract class BaseEndpoint
	{
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003E3A RID: 15930 RVA: 0x0012FF56 File Offset: 0x0012E156
		// (set) Token: 0x06003E3B RID: 15931 RVA: 0x0012FF5E File Offset: 0x0012E15E
		public string DebugModuleName { get; protected set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003E3C RID: 15932 RVA: 0x0012FF67 File Offset: 0x0012E167
		public int GetHTTPResponseStatus
		{
			get
			{
				if (this._HTTPResponse != null)
				{
					return this._HTTPResponse.StatusCode;
				}
				return -1;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0012FF7E File Offset: 0x0012E17E
		public int ApiResponseStatus
		{
			get
			{
				if (this._HTTPResponse != null)
				{
					return this._HTTPResponse.StatusCode;
				}
				return 0;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003E3E RID: 15934 RVA: 0x0012FF95 File Offset: 0x0012E195
		// (set) Token: 0x06003E3F RID: 15935 RVA: 0x0012FF9D File Offset: 0x0012E19D
		public bool SilentFailure { get; set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003E40 RID: 15936 RVA: 0x0012FFA6 File Offset: 0x0012E1A6
		// (set) Token: 0x06003E41 RID: 15937 RVA: 0x0012FFAE File Offset: 0x0012E1AE
		private protected bool _MustUsePrivateScope { protected get; private set; }

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x0012FFB7 File Offset: 0x0012E1B7
		// (set) Token: 0x06003E43 RID: 15939 RVA: 0x0012FFBF File Offset: 0x0012E1BF
		protected OAuthGate _OAuthGate { get; set; }

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x0012FFC8 File Offset: 0x0012E1C8
		// (set) Token: 0x06003E45 RID: 15941 RVA: 0x0012FFD0 File Offset: 0x0012E1D0
		protected NetworkParameters _NetworkParameters { get; set; }

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x0012FFD9 File Offset: 0x0012E1D9
		// (set) Token: 0x06003E47 RID: 15943 RVA: 0x0012FFE1 File Offset: 0x0012E1E1
		protected string _URL { get; set; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0012FFEA File Offset: 0x0012E1EA
		// (set) Token: 0x06003E49 RID: 15945 RVA: 0x0012FFF2 File Offset: 0x0012E1F2
		protected Hashtable _Parameters { get; set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0012FFFB File Offset: 0x0012E1FB
		// (set) Token: 0x06003E4B RID: 15947 RVA: 0x00130003 File Offset: 0x0012E203
		protected HTTPMethods _HttpMethod { get; set; }

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0013000C File Offset: 0x0012E20C
		// (set) Token: 0x06003E4D RID: 15949 RVA: 0x00130014 File Offset: 0x0012E214
		protected HTTPRequest _HTTPRequest { get; set; }

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0013001D File Offset: 0x0012E21D
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x00130025 File Offset: 0x0012E225
		protected HTTPResponse _HTTPResponse { get; set; }

		// Token: 0x06003E50 RID: 15952 RVA: 0x00130030 File Offset: 0x0012E230
		public BaseEndpoint(bool mustUsePrivateScope, OAuthGate oauthGate)
		{
			this.SilentFailure = false;
			this._OAuthGate = ((oauthGate == null && CoreApplication.Instance != null) ? CoreApplication.Instance.OAuthGate : oauthGate);
			if (this._OAuthGate == null)
			{
				throw new ArgumentException("'oauthGate' must not be null");
			}
			if (this._OAuthGate.NetworkParameters == null)
			{
				throw new ArgumentException("'networkParameters' must not be null");
			}
			this.DebugModuleName = "Endpoint";
			this._MustUsePrivateScope = mustUsePrivateScope;
			this._NetworkParameters = this._OAuthGate.NetworkParameters;
			this._URL = null;
			this._Parameters = null;
			this._HttpMethod = HTTPMethods.Get;
			this._HTTPRequest = null;
			this._HTTPResponse = null;
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x001300E4 File Offset: 0x0012E2E4
		public bool Succeeded()
		{
			return this._HTTPResponse != null && this._HTTPResponse.StatusCode >= 200 && this._HTTPResponse.StatusCode < 300;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x00130114 File Offset: 0x0012E314
		protected Hashtable _GetLogDetails()
		{
			string text = "";
			if (!this._URL.StartsWith("http"))
			{
				text += this._NetworkParameters.GetApiBaseUrl();
			}
			text += this._URL;
			return new Hashtable
			{
				{
					"url",
					text
				},
				{
					"method",
					this._HttpMethod.ToString().ToUpper()
				}
			};
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x00130190 File Offset: 0x0012E390
		protected Hashtable _GetCopyOfParameters()
		{
			if (this._Parameters == null)
			{
				return new Hashtable();
			}
			Hashtable hashtable = this._Parameters.Clone() as Hashtable;
			hashtable.Remove("password");
			hashtable.Remove("access_token");
			hashtable.Remove("client_secret");
			return hashtable;
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x001301DC File Offset: 0x0012E3DC
		protected void _ExecuteCore()
		{
			if (!string.IsNullOrEmpty(this._OAuthGate.AccessToken))
			{
				this._HTTPRequest.AddHeader("Authorization", "bearer " + this._OAuthGate.AccessToken);
			}
			this._HTTPRequest.AddHeader("User-Agent", CoreApplication.GetUserAgent());
			if (this._HttpMethod == HTTPMethods.Post || this._HttpMethod == HTTPMethods.Put || this._HttpMethod == HTTPMethods.Delete || this._HttpMethod == HTTPMethods.Patch)
			{
				this._HTTPRequest.AddHeader("Cache-Control", "no-cache");
				if (this._usePutAsPatch && this._HttpMethod == HTTPMethods.Put)
				{
					this._HTTPRequest.AddHeader("Content-Type", "application/prs.dow-online-features-patch+json");
				}
				else
				{
					this._HTTPRequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
				}
				this._SetRequestParameters();
			}
			this._HTTPRequest.CustomCertificateVerifyer = new CertificateVerifier(this._NetworkParameters.RestAPIPinPublicKeys);
			this._HTTPRequest.UseAlternateSSL = true;
			this._LogOnSending();
			this._HTTPRequest.Send();
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x001302EC File Offset: 0x0012E4EC
		protected void _LogOnSending()
		{
			Hashtable hashtable = this._GetLogDetails();
			Hashtable value = this._GetCopyOfParameters();
			hashtable.Add("parameters", value);
			AsmoLogger.Info(this.DebugModuleName + ".sender", "Sending Request", hashtable);
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x00130330 File Offset: 0x0012E530
		public void Abort()
		{
			if (this._getTokenCallbackId != null)
			{
				this._OAuthGate.CancelAccessTokenRequest(this._getTokenCallbackId.Value);
				this._getTokenCallbackId = null;
			}
			if (this._HTTPRequest != null && (this._HTTPRequest.State == HTTPRequestStates.Queued || this._HTTPRequest.State == HTTPRequestStates.Processing))
			{
				this._HTTPRequest.Abort();
			}
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0013039C File Offset: 0x0012E59C
		protected void _LogOnCompletion(WebError error)
		{
			Hashtable hashtable = this._GetLogDetails();
			hashtable.Add("http_status", (this._HTTPResponse == null) ? "null" : this._HTTPResponse.StatusCode.ToString());
			if (this.Succeeded())
			{
				hashtable.Add("result", this._HTTPResponse.DataAsText);
				AsmoLogger.Info(this.DebugModuleName + ".receiver", "Request Success", hashtable);
				return;
			}
			hashtable.Add("error", Reflection.HashtableFromObject(error, null, 30U));
			AsmoLogger.Error(this.DebugModuleName + ".receiver", "Request Failure", hashtable);
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00130446 File Offset: 0x0012E646
		protected virtual ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseError>(this._HTTPResponse.DataAsText);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00130458 File Offset: 0x0012E658
		protected virtual void _SetRequestParameters()
		{
			if (this._Parameters != null)
			{
				string s = Json.Serialize(this._Parameters);
				this._HTTPRequest.RawData = Encoding.UTF8.GetBytes(s);
			}
		}

		// Token: 0x0400282F RID: 10287
		public const string kRequestFailureError = "request_failure";

		// Token: 0x04002833 RID: 10291
		protected bool _usePutAsPatch;

		// Token: 0x0400283B RID: 10299
		protected int? _getTokenCallbackId;
	}
}
