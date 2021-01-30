using System;
using System.Collections;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200068F RID: 1679
	public class RemoveAwardsEndpoint : Endpoint
	{
		// Token: 0x06003D45 RID: 15685 RVA: 0x0012DBB0 File Offset: 0x0012BDB0
		public RemoveAwardsEndpoint(Award[] achievements, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Award";
			if (achievements == null)
			{
				throw new ArgumentException("The \"achievements\" parameter (Achievements[] achievements) must not be null");
			}
			if (achievements.Length == 0)
			{
				throw new ArgumentException("The \"achievements\" parameter (Achievements[] achievements) must at least contain one item");
			}
			Hashtable[] array = new Hashtable[achievements.Length];
			int num = 0;
			foreach (Award award in achievements)
			{
				if (award.Id != -1)
				{
					array[num] = new Hashtable
					{
						{
							"achievement",
							award.Id
						}
					};
				}
				else
				{
					if (!(award.Tag != string.Empty))
					{
						throw new ArgumentException("An instance of Achievement has neither an Id nor a Tag, and thus can not be used.");
					}
					array[num] = new Hashtable
					{
						{
							"achievement",
							award.Tag
						}
					};
				}
				if (award.TableId != -1)
				{
					array[num].Add("table_id", award.TableId);
				}
				if (award.InfoId != -1)
				{
					array[num].Add("info_id", award.InfoId);
				}
				num++;
			}
			base._Parameters = new Hashtable();
			base._Parameters.Add("achievements", array);
			base._URL = "/main/v1/user/me/awards";
			base._HttpMethod = HTTPMethods.Delete;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x0012DCFB File Offset: 0x0012BEFB
		protected override ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseRemoveAwardError>(base._HTTPResponse.DataAsText);
		}
	}
}
