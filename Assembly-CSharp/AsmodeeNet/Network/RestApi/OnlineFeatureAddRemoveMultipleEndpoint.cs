using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006DD RID: 1757
	public class OnlineFeatureAddRemoveMultipleEndpoint : Endpoint
	{
		// Token: 0x06003E7D RID: 15997 RVA: 0x0013138C File Offset: 0x0012F58C
		public OnlineFeatureAddRemoveMultipleEndpoint(int userId, string[] featuresToAdd, string[] featuresToRemove, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.OnlineFeatures";
			base._URL = string.Format("/main/v1/user/{0}/features", userId);
			this.CtorCore(featuresToAdd, featuresToRemove);
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x001313CB File Offset: 0x0012F5CB
		public OnlineFeatureAddRemoveMultipleEndpoint(string[] featuresToAdd, string[] featuresToRemove, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.OnlineFeatures";
			base._URL = "/main/v1/user/me/features";
			this.CtorCore(featuresToAdd, featuresToRemove);
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00131400 File Offset: 0x0012F600
		private void CtorCore(string[] featuresToAdd, string[] featuresToRemove)
		{
			if ((featuresToAdd == null || featuresToAdd.Length == 0) && (featuresToRemove == null || featuresToRemove.Length == 0))
			{
				throw new ArgumentException("'featuresToAdd' and 'featuresToRemove' arguments are both optional, but both cannot be omitted");
			}
			base._Parameters = new Hashtable();
			if (featuresToAdd != null && featuresToAdd.Length != 0)
			{
				base._Parameters.Add("add", featuresToAdd);
			}
			if (featuresToRemove != null && featuresToRemove.Length != 0)
			{
				base._Parameters.Add("remove", featuresToRemove);
			}
			base._HttpMethod = HTTPMethods.Patch;
		}
	}
}
