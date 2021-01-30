using System;
using System.Collections;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006DC RID: 1756
	public class LinkUnlinkMultipleEndpoint : Endpoint
	{
		// Token: 0x06003E7B RID: 15995 RVA: 0x00131284 File Offset: 0x0012F484
		public LinkUnlinkMultipleEndpoint(PartnerAccount[] add, PartnerAccount[] remove, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			if ((add == null || add.Length == 0) && (remove == null || remove.Length == 0))
			{
				throw new ArgumentException("'add' and 'remove' arrays cannot both be null or empty");
			}
			base.DebugModuleName += ".User.LinkUnlinkMultiple";
			base._HttpMethod = HTTPMethods.Put;
			this._usePutAsPatch = true;
			base._URL = "/main/v1/user/me/link";
			base._Parameters = new Hashtable();
			Hashtable[] array;
			if (add != null)
			{
				array = (from x in add
				select new Hashtable
				{
					{
						"partner",
						x.PartnerId
					},
					{
						"partner_user",
						x.PartnerUser
					}
				}).ToArray<Hashtable>();
			}
			else
			{
				array = new Hashtable[0];
			}
			Hashtable[] value = array;
			Hashtable[] array2;
			if (remove != null)
			{
				array2 = (from x in remove
				select new Hashtable
				{
					{
						"partner",
						x.PartnerId
					},
					{
						"partner_user",
						x.PartnerUser
					}
				}).ToArray<Hashtable>();
			}
			else
			{
				array2 = new Hashtable[0];
			}
			Hashtable[] value2 = array2;
			base._Parameters.Add("add", value);
			base._Parameters.Add("remove", value2);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0013137A File Offset: 0x0012F57A
		protected override ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseLinkUnlinkMultipleError>(base._HTTPResponse.DataAsText);
		}
	}
}
