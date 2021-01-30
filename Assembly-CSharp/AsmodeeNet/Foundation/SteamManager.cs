using System;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000700 RID: 1792
	public class SteamManager : IDisposable
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x001342A8 File Offset: 0x001324A8
		public PartnerAccount Me
		{
			get
			{
				throw new Exception("Steam is not available or active on this platform !");
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x0002A062 File Offset: 0x00028262
		// (set) Token: 0x06003F5E RID: 16222 RVA: 0x00003022 File Offset: 0x00001222
		public bool AutoConnect
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x0002A062 File Offset: 0x00028262
		public bool HasClient
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x001342A8 File Offset: 0x001324A8
		public uint SteamAppId
		{
			get
			{
				throw new Exception("Steam is not available or active on this platform !");
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x001342A8 File Offset: 0x001324A8
		public string SessionTicket
		{
			get
			{
				throw new Exception("Steam is not available or active on this platform !");
			}
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00003425 File Offset: 0x00001625
		public SteamManager(uint steamGameId)
		{
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x00003022 File Offset: 0x00001222
		public void Dispose()
		{
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x001342A8 File Offset: 0x001324A8
		public void LinkSteamAccount(OAuthGate gate, Action onComplete)
		{
			throw new Exception("Steam is not available or active on this platform !");
		}

		// Token: 0x04002898 RID: 10392
		private const string _kSteamNotAvailable = "Steam is not available or active on this platform !";

		// Token: 0x04002899 RID: 10393
		private const string _kConsoleModuleName = "SteamManager";
	}
}
