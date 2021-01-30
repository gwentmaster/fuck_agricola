using System;
using System.Collections;
using AsmodeeNet.Utils;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006CD RID: 1741
	public abstract class Endpoint : BaseEndpoint
	{
		// Token: 0x06003E5A RID: 15962 RVA: 0x0013048F File Offset: 0x0012E68F
		public Endpoint(bool mustUsePrivateScope, OAuthGate oauthGate) : base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0013049C File Offset: 0x0012E69C
		public void Execute(EndpointCallback onCompletion)
		{
			OAuthCallback onComplete = delegate(OAuthError authError)
			{
				this._getTokenCallbackId = null;
				if (authError != null)
				{
					if (onCompletion != null)
					{
						onCompletion(authError);
					}
					return;
				}
				this._Execute(this._NetworkParameters.GetApiBaseUrl() + this._URL, onCompletion);
			};
			if (base._MustUsePrivateScope)
			{
				this._getTokenCallbackId = base._OAuthGate.GetPrivateAccessToken(base.SilentFailure, onComplete);
				return;
			}
			this._getTokenCallbackId = base._OAuthGate.GetPublicAccessToken(onComplete);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x001304FC File Offset: 0x0012E6FC
		private void _Execute(string fullUrl, EndpointCallback onCompletion)
		{
			base._HTTPRequest = new HTTPRequest(new Uri(fullUrl), base._HttpMethod, delegate(HTTPRequest req, HTTPResponse response)
			{
				if (response != null)
				{
					this._HTTPResponse = response;
					WebError error = null;
					if (!this.Succeeded())
					{
						if (string.IsNullOrEmpty(response.DataAsText))
						{
							error = new WebError("request_failure", response.StatusCode);
						}
						else
						{
							ApiResponseError apiResponseError = this._ParseError();
							if (string.IsNullOrEmpty(apiResponseError.error))
							{
								error = new WebError("request_failure", response.StatusCode);
							}
							else
							{
								error = apiResponseError;
							}
						}
					}
					this._LogOnCompletion(error);
					if (onCompletion != null)
					{
						onCompletion(error);
					}
					return;
				}
				Hashtable hashtable = this._GetLogDetails();
				Hashtable value = this._GetCopyOfParameters();
				hashtable.Add("parameters", value);
				if (req.State == HTTPRequestStates.Aborted)
				{
					AsmoLogger.Error(this.DebugModuleName + ".receiver", "Aborting", hashtable);
					return;
				}
				AsmoLogger.Error(this.DebugModuleName + ".receiver", "No response", hashtable);
				if (onCompletion != null)
				{
					onCompletion(WebError.MakeNoResponseError());
				}
			});
			base._ExecuteCore();
		}
	}
}
