using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AF RID: 1711
	public abstract class EndpointWithPaginatedResponse<T> : Endpoint<PaginatedResult<!0>> where T : class
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x0012E703 File Offset: 0x0012C903
		public EndpointWithPaginatedResponse(OAuthGate oauthGate, bool mustUsePrivateScope) : base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0012E710 File Offset: 0x0012C910
		protected Action<Action<PaginatedResult<T>, WebError>> _LinkSetter(string url)
		{
			if (url == null)
			{
				return null;
			}
			return delegate(Action<PaginatedResult<T>, WebError> callback)
			{
				this._HTTPRequest = null;
				this._HTTPResponse = null;
				this._Execute(url, callback);
			};
		}
	}
}
