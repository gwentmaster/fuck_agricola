using System;
using System.Collections;
using AsmodeeNet.Utils;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006CE RID: 1742
	public abstract class Endpoint<ResultType> : BaseEndpoint where ResultType : class
	{
		// Token: 0x06003E5D RID: 15965 RVA: 0x0013048F File Offset: 0x0012E68F
		public Endpoint(bool mustUsePrivateScope, OAuthGate oauthGate) : base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00130548 File Offset: 0x0012E748
		public void Execute(Action<ResultType, WebError> onCompletion)
		{
			OAuthCallback onComplete = delegate(OAuthError authError)
			{
				this._getTokenCallbackId = null;
				if (authError != null)
				{
					if (onCompletion != null)
					{
						onCompletion(default(!0), authError);
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

		// Token: 0x06003E5F RID: 15967 RVA: 0x001305A8 File Offset: 0x0012E7A8
		protected void _Execute(string fullUrl, Action<ResultType, WebError> onCompletion)
		{
			base._HTTPRequest = new HTTPRequest(new Uri(fullUrl), base._HttpMethod, delegate(HTTPRequest req, HTTPResponse response)
			{
				if (response != null)
				{
					this._HTTPResponse = response;
					WebError webError = null;
					if (!this.Succeeded())
					{
						if (string.IsNullOrEmpty(response.DataAsText))
						{
							webError = new WebError("request_failure", response.StatusCode);
						}
						else
						{
							ApiResponseError apiResponseError = this._ParseError();
							if (string.IsNullOrEmpty(apiResponseError.error))
							{
								webError = new WebError("request_failure", response.StatusCode);
							}
							else
							{
								webError = apiResponseError;
							}
						}
						this._LogOnCompletion(webError);
						if (onCompletion != null)
						{
							onCompletion(default(!0), webError);
							return;
						}
					}
					else
					{
						this._LogOnCompletion(webError);
						this.ProcessResponse(onCompletion);
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
					onCompletion(default(!0), WebError.MakeNoResponseError());
				}
			});
			base._ExecuteCore();
		}

		// Token: 0x06003E60 RID: 15968
		protected abstract void ProcessResponse(Action<ResultType, WebError> onCompletion);
	}
}
