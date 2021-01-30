using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.Networking;
using UTNotifications;

// Token: 0x020000BB RID: 187
public class Network : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060006A0 RID: 1696 RVA: 0x00032CF8 File Offset: 0x00030EF8
	// (remove) Token: 0x060006A1 RID: 1697 RVA: 0x00032D30 File Offset: 0x00030F30
	private event Network.NetworkEventDelegate m_NetworkEventListeners;

	// Token: 0x060006A2 RID: 1698 RVA: 0x00032D65 File Offset: 0x00030F65
	public static bool IsCreateAccount()
	{
		return Network.m_Network != null && Network.m_Network.m_bCreateAccount;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00032D80 File Offset: 0x00030F80
	public void AddNetworkEventHandler(Network.NetworkEventDelegate h)
	{
		this.m_NetworkEventListeners += h;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00032D89 File Offset: 0x00030F89
	public void RemoveNetworkEventHandler(Network.NetworkEventDelegate h)
	{
		this.m_NetworkEventListeners -= h;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00032D92 File Offset: 0x00030F92
	public void ClearNetworkEventHandlerList()
	{
		this.m_NetworkEventListeners = null;
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00032D9B File Offset: 0x00030F9B
	private void RaiseNetworkEvent(NetworkEvent.EventType eventType, int data)
	{
		if (this.m_NetworkEventListeners != null)
		{
			this.m_NetworkEventListeners(eventType, data);
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00032DB4 File Offset: 0x00030FB4
	private void Awake()
	{
		if (Network.m_Network == null)
		{
			Network.m_Network = this;
			this.m_LastNetworkStatus = "Not Connected";
			this.m_bConnectedToServer = false;
			this.m_bLoggedInToServer = false;
			this.m_bServerConnectionLost = false;
			this.m_bServerDisconnected = false;
			this.m_bCreateAccount = false;
			Network.m_bPausedDuringNetworkSession = false;
			Network.m_loginName = string.Empty;
			Network.m_loginPassword = string.Empty;
			Network.m_asmodeeAccessToken = string.Empty;
			Network.Create();
			this.networkEvents = new NetworkEvent[32];
			return;
		}
		if (Network.m_Network != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00032E51 File Offset: 0x00031051
	private void OnDestroy()
	{
		Network.Disconnect();
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00032E58 File Offset: 0x00031058
	public static void Create()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Create(): NO NETWORK OBJECT EXISTS");
			return;
		}
		AgricolaLib.NetworkCreate();
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00032E78 File Offset: 0x00031078
	public static void Login(string user, string password)
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Login(): NO NETWORK OBJECT EXISTS");
			return;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			Debug.LogError("Network.Login() called when already connected!");
		}
		else
		{
			Network.m_Network.m_bServerConnectionLost = false;
			Network.m_Network.m_bServerDisconnected = false;
			Network.m_Network.m_bCreateAccount = false;
			AgricolaLib.NetworkLogin(user, password);
		}
		Network.m_loginName = user;
		Network.m_loginPassword = password;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00032EEC File Offset: 0x000310EC
	public static void SetLogin(string user, string password)
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.SetLogin(): NO NETWORK OBJECT EXISTS");
			return;
		}
		Network.m_Network.m_bServerConnectionLost = false;
		Network.m_Network.m_bServerDisconnected = false;
		Network.m_Network.m_bCreateAccount = false;
		AgricolaLib.NetworkSetLoginPassword(user, password);
		Network.m_loginName = user;
		Network.m_loginPassword = password;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00032F45 File Offset: 0x00031145
	public static void RetryLogin()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.SetLogin(): NO NETWORK OBJECT EXISTS");
			return;
		}
		AgricolaLib.NetworkSetLoginPassword(Network.m_loginName, Network.m_loginPassword);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00032F70 File Offset: 0x00031170
	public static void CreateAccount(string email, string password, string username)
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.CreateAccount(): NO NETWORK OBJECT EXISTS");
			return;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			Debug.LogError("Network.CreateAccount() called when already connected!");
		}
		else
		{
			Network.m_Network.m_bServerConnectionLost = false;
			Network.m_Network.m_bServerDisconnected = false;
			Network.m_Network.m_bCreateAccount = true;
			AgricolaLib.NetworkCreateAccount(email, username, password);
		}
		Network.m_loginName = username;
		Network.m_loginPassword = password;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00032FE2 File Offset: 0x000311E2
	public static void SetCreateAccount(string email, string password, string username)
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.SetCreateAccount(): NO NETWORK OBJECT EXISTS");
			return;
		}
		Network.m_Network.m_bCreateAccount = true;
		AgricolaLib.NetworkSetCreateAccount(email, password, username);
		Network.m_loginName = username;
		Network.m_loginPassword = password;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0003301C File Offset: 0x0003121C
	public static void Connect()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Connect(): NO NETWORK OBJECT EXISTS");
			return;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			Debug.LogError("Network.Connect() called when already connected!");
			return;
		}
		Network.m_Network.m_bServerConnectionLost = false;
		Network.m_Network.m_bServerDisconnected = false;
		AgricolaLib.NetworkConnect();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00033074 File Offset: 0x00031274
	public static void ConnectAsmodee()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Connect(): NO NETWORK OBJECT EXISTS");
			return;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			Debug.LogError("Network.Connect() called when already connected!");
			return;
		}
		if (!string.IsNullOrEmpty(Network.m_asmodeeAccessToken))
		{
			Network.m_Network.m_bServerConnectionLost = false;
			Network.m_Network.m_bServerDisconnected = false;
			Network.m_Network.m_bCreateAccount = false;
			AgricolaLib.NetworkConnectAsmodee(string.Empty, Network.m_asmodeeAccessToken);
		}
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000330EC File Offset: 0x000312EC
	public static void Disconnect()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Disconnect(): NO NETWORK OBJECT EXISTS");
			return;
		}
		AgricolaLib.NetworkDisconnect();
		Network.m_Network.m_bConnectedToServer = false;
		Network.m_Network.m_bLoggedInToServer = false;
		Network.m_Network.m_bServerConnectionLost = false;
		Network.m_Network.m_bServerDisconnected = true;
		Network.m_Network.m_bCreateAccount = false;
		Network.m_loginName = string.Empty;
		Network.m_loginPassword = string.Empty;
		Network.m_asmodeeAccessToken = string.Empty;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0003316B File Offset: 0x0003136B
	public static void PrepareForReconnect()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Disconnect(): NO NETWORK OBJECT EXISTS");
			return;
		}
		AgricolaLib.NetworkDisconnect();
		Network.m_Network.m_bConnectedToServer = false;
		Network.m_Network.m_bCreateAccount = false;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x000331A0 File Offset: 0x000313A0
	public static void Destroy()
	{
		if (Network.m_Network == null)
		{
			Debug.LogError("Error! -- Network.Destroy(): NO NETWORK OBJECT EXISTS");
			return;
		}
		AgricolaLib.NetworkDestroy();
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000331C0 File Offset: 0x000313C0
	private void FixedUpdate()
	{
		GCHandle gchandle = GCHandle.Alloc(this.networkEvents, GCHandleType.Pinned);
		int num = AgricolaLib.NetworkUpdate(gchandle.AddrOfPinnedObject(), 32);
		for (int i = 0; i < num; i++)
		{
			NetworkEvent.EventType type = (NetworkEvent.EventType)this.networkEvents[i].type;
			int data = this.networkEvents[i].data;
			switch (type)
			{
			case NetworkEvent.EventType.Event_LoginInitiated:
				this.m_LastNetworkStatus = "Login Requested";
				break;
			case NetworkEvent.EventType.Event_LoginComplete:
				this.m_LastNetworkStatus = "Login Successful";
				this.m_bConnectedToServer = true;
				AgricolaLib.NetworkRequestFriendsList(Network.m_asmodeeAccessToken);
				break;
			case NetworkEvent.EventType.Event_LoginError:
				this.m_LastNetworkStatus = "Login Error";
				Network.m_loginName = string.Empty;
				Network.m_loginPassword = string.Empty;
				Network.m_asmodeeAccessToken = string.Empty;
				break;
			case NetworkEvent.EventType.Event_AvailableGamesRefreshed:
			{
				this.m_LastNetworkStatus = "Available Games Refreshed";
				bool bRequestRefreshAvailableGames = this.m_bRequestRefreshAvailableGames;
				this.m_bRequestRefreshAvailableGames = false;
				this.m_bNewAvailableGames = true;
				break;
			}
			case NetworkEvent.EventType.Event_ConnectionLost:
				this.m_LastNetworkStatus = "Connection Lost";
				if (!this.m_bServerDisconnected)
				{
					this.m_bServerConnectionLost = this.m_bLoggedInToServer;
				}
				else
				{
					Network.m_loginName = string.Empty;
					Network.m_loginPassword = string.Empty;
				}
				Network.m_asmodeeAccessToken = string.Empty;
				this.m_bConnectedToServer = false;
				this.m_bLoggedInToServer = false;
				break;
			}
			this.RaiseNetworkEvent(type, data);
		}
		gchandle.Free();
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00033320 File Offset: 0x00031520
	public static List<ShortSaveStruct> GetMatchmakingList()
	{
		List<ShortSaveStruct> list = new List<ShortSaveStruct>();
		int num = Marshal.SizeOf(typeof(ShortSaveStruct));
		GCHandle gchandle = GCHandle.Alloc(new byte[32 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetMatchmakingGames(intPtr, 32); i > 0; i--)
		{
			ShortSaveStruct item = (ShortSaveStruct)Marshal.PtrToStructure(intPtr, typeof(ShortSaveStruct));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x000333AC File Offset: 0x000315AC
	public static List<ShortSaveStruct> GetActiveGameList()
	{
		List<ShortSaveStruct> list = new List<ShortSaveStruct>();
		int num = Marshal.SizeOf(typeof(ShortSaveStruct));
		GCHandle gchandle = GCHandle.Alloc(new byte[128 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetActiveGames(intPtr, 128); i > 0; i--)
		{
			ShortSaveStruct item = (ShortSaveStruct)Marshal.PtrToStructure(intPtr, typeof(ShortSaveStruct));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x0003343C File Offset: 0x0003163C
	public static List<ShortSaveStruct> GetCompletedGameList()
	{
		List<ShortSaveStruct> list = new List<ShortSaveStruct>();
		int num = Marshal.SizeOf(typeof(ShortSaveStruct));
		GCHandle gchandle = GCHandle.Alloc(new byte[128 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetCompletedGames(intPtr, 128); i > 0; i--)
		{
			ShortSaveStruct item = (ShortSaveStruct)Marshal.PtrToStructure(intPtr, typeof(ShortSaveStruct));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000334CC File Offset: 0x000316CC
	public static List<GamePlayerTimer> GetGamePlayerTimers(uint gameID)
	{
		List<GamePlayerTimer> list = new List<GamePlayerTimer>();
		int num = Marshal.SizeOf(typeof(GamePlayerTimer));
		GCHandle gchandle = GCHandle.Alloc(new byte[6 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetGamePlayerTimers(gameID, intPtr); i > 0; i--)
		{
			GamePlayerTimer item = (GamePlayerTimer)Marshal.PtrToStructure(intPtr, typeof(GamePlayerTimer));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00033554 File Offset: 0x00031754
	public static bool NewAvailableGames()
	{
		return Network.m_Network.m_bNewAvailableGames;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00033560 File Offset: 0x00031760
	public static void RefreshAvailableGames()
	{
		Network.m_Network.m_bRequestRefreshAvailableGames = true;
		AgricolaLib.NetworkRefreshAvailableGames();
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00033574 File Offset: 0x00031774
	public static List<ShortSaveStruct> GetAvailableGameList(uint playerFilterFlags, uint timerFilterFlags)
	{
		Network.m_Network.m_bNewAvailableGames = false;
		List<ShortSaveStruct> list = new List<ShortSaveStruct>();
		int num = Marshal.SizeOf(typeof(ShortSaveStruct));
		GCHandle gchandle = GCHandle.Alloc(new byte[256 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetAvailableGames(intPtr, 256); i > 0; i--)
		{
			ShortSaveStruct item = (ShortSaveStruct)Marshal.PtrToStructure(intPtr, typeof(ShortSaveStruct));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00033610 File Offset: 0x00031810
	public static void CreateGame(uint maxPlayerCount, uint[] inviteIDs, uint startPlayerTimer, GameParameters gameParams)
	{
		GCHandle gchandle = GCHandle.Alloc(inviteIDs, GCHandleType.Pinned);
		IntPtr invitationIds = gchandle.AddrOfPinnedObject();
		AgricolaLib.NetworkCreateGame(maxPlayerCount, invitationIds, startPlayerTimer, ref gameParams);
		gchandle.Free();
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0003363F File Offset: 0x0003183F
	public static void RequestMatchmaking(uint numPlayers, uint startPlayerTimer, uint maxRatingRange, uint maxWaitTime, GameParameters gameParams)
	{
		AgricolaLib.NetworkRequestMatchmaking(numPlayers, startPlayerTimer, maxRatingRange, maxWaitTime, ref gameParams);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00033650 File Offset: 0x00031850
	public static void SendPlayerParameters(GamePlayerParameters playerParams)
	{
		GCHandle gchandle = GCHandle.Alloc(playerParams, GCHandleType.Pinned);
		AgricolaLib.NetworkSendPlayerParameters(gchandle.AddrOfPinnedObject(), (uint)Marshal.SizeOf(typeof(GamePlayerParameters)));
		gchandle.Free();
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0003368C File Offset: 0x0003188C
	public static List<FriendInfo> GetFriendsList()
	{
		List<FriendInfo> list = new List<FriendInfo>();
		int num = Marshal.SizeOf(typeof(FriendInfo));
		GCHandle gchandle = GCHandle.Alloc(new byte[2048 * num], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = AgricolaLib.NetworkGetFriendsList(intPtr, 2048); i > 0; i--)
		{
			FriendInfo item = (FriendInfo)Marshal.PtrToStructure(intPtr, typeof(FriendInfo));
			list.Add(item);
			intPtr = new IntPtr(intPtr.ToInt64() + (long)num);
		}
		gchandle.Free();
		return list;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0003371C File Offset: 0x0003191C
	public static int GetUserOnlineStatus(uint userID)
	{
		return AgricolaLib.NetworkGetUserOnlineStatus(userID);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00033724 File Offset: 0x00031924
	public static void RequestUsersOnlineStatus(IntPtr usersArray, int numUsers)
	{
		AgricolaLib.NetworkRequestUsersOnlineStatus(usersArray, numUsers);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0003372D File Offset: 0x0003192D
	public static void SetMonitorGame(uint gameID)
	{
		AgricolaLib.NetworkSetMonitorGame(gameID);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00033735 File Offset: 0x00031935
	public static void ClearMonitorGame()
	{
		AgricolaLib.NetworkClearMonitorGame();
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0003373C File Offset: 0x0003193C
	public static int AddFriendFromUsername(string username)
	{
		return AgricolaLib.NetworkAddFriendFromUsername(username, Network.m_asmodeeAccessToken);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00033749 File Offset: 0x00031949
	public static int AddFriendFromEmail(string email)
	{
		AgricolaLib.NetworkAddFriendFromEmail(email, Network.m_asmodeeAccessToken);
		return 0;
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00033757 File Offset: 0x00031957
	public static bool RemoveFriendWithUserId(uint userID)
	{
		return AgricolaLib.NetworkRemoveFriendWithUserId(userID, Network.m_asmodeeAccessToken);
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00033764 File Offset: 0x00031964
	public static void RequestNetworkPlayerProfile(uint userID)
	{
		AgricolaLib.NetworkRequestPlayerProfile(userID);
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0003376C File Offset: 0x0003196C
	public static uint NetworkGetLocalID()
	{
		return AgricolaLib.NetworkGetLocalID();
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00033774 File Offset: 0x00031974
	public static void GetRemotePlayerProfileInfo(int remotePlayerID, out NetworkPlayerProfileInfo profile)
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[Marshal.SizeOf(typeof(NetworkPlayerProfileInfo))], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.NetworkRemotePlayerProfile(remotePlayerID, intPtr);
		profile = (NetworkPlayerProfileInfo)Marshal.PtrToStructure(intPtr, typeof(NetworkPlayerProfileInfo));
		gchandle.Free();
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x000337D0 File Offset: 0x000319D0
	public static void GetLocalPlayerProfileInfo(out NetworkPlayerProfileInfo profile)
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[Marshal.SizeOf(typeof(NetworkPlayerProfileInfo))], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.NetworkLocalPlayerProfile(intPtr);
		profile = (NetworkPlayerProfileInfo)Marshal.PtrToStructure(intPtr, typeof(NetworkPlayerProfileInfo));
		gchandle.Free();
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x00033828 File Offset: 0x00031A28
	public static void SendLocalAvatarIndex(uint avatarIndex)
	{
		AgricolaLib.NetworkSendLocalAvatarIndex(avatarIndex);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00033830 File Offset: 0x00031A30
	public static bool HasAsmodeeAccessToken()
	{
		return !string.IsNullOrEmpty(Network.m_asmodeeAccessToken);
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0003383F File Offset: 0x00031A3F
	public static IEnumerator RequestAsmodeeAccessToken()
	{
		Network.m_asmodeeAccessToken = string.Empty;
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("grant_type", "password");
		wwwform.AddField("client_id", AsmodeeNetSettings.GetClientID());
		wwwform.AddField("client_secret", AsmodeeNetSettings.GetClientSecret());
		wwwform.AddField("username", Network.m_loginName);
		wwwform.AddField("password", Network.m_loginPassword);
		string @string = Encoding.UTF8.GetString(wwwform.data);
		Debug.Log("Post Request: " + @string);
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
			string arg = string.Empty;
			string arg2 = string.Empty;
			string empty = string.Empty;
			string text = post_request.downloadHandler.text;
			Debug.Log("WWW Response: " + text);
			if (!string.IsNullOrEmpty(text))
			{
				JSONNode jsonnode = JSON.Parse(text);
				if (jsonnode != null)
				{
					JSONNode jsonnode2 = jsonnode["access_token"];
					if (jsonnode2 != null)
					{
						arg = jsonnode2.Value;
					}
					JSONNode jsonnode3 = jsonnode["token_type"];
					if (jsonnode3 != null)
					{
						arg2 = jsonnode3.Value;
					}
					Network.m_asmodeeAccessToken = string.Format("{0} {1}", arg2, arg);
				}
			}
		}
		yield break;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00033847 File Offset: 0x00031A47
	public static IEnumerator RequestAsmodeeAccountInformation()
	{
		UnityWebRequest post_request = UnityWebRequest.Get("https://api.asmodee.net/main/v1/user/me");
		post_request.SetRequestHeader("Authorization", Network.m_asmodeeAccessToken);
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
			Debug.Log("WWW Response: " + text);
			if (!string.IsNullOrEmpty(text))
			{
				JSONNode jsonnode = JSON.Parse(text);
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
								string value = jsonnode4.Value;
								Debug.Log("   Asmodee Login Name: " + value);
							}
							JSONNode jsonnode5 = jsonnode3["email"];
							if (jsonnode5 != null)
							{
								string value2 = jsonnode5.Value;
								Debug.Log("   Asmodee Email: " + value2);
							}
							JSONNode jsonnode6 = jsonnode3["user_id"];
							if (jsonnode6 != null)
							{
								long num = 0L;
								if (long.TryParse(jsonnode6.Value, out num))
								{
									long num2 = num;
									Debug.Log("   Asmodee User ID: " + num2);
									CoreApplication.Instance.AnalyticsManager.UserId = num2.ToString();
								}
							}
						}
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x04000801 RID: 2049
	private const int k_maxGamePlayers = 6;

	// Token: 0x04000802 RID: 2050
	private const int k_maxActiveGames = 128;

	// Token: 0x04000803 RID: 2051
	private const int k_maxAvailableGames = 256;

	// Token: 0x04000804 RID: 2052
	private const int k_maxFriendListSize = 2048;

	// Token: 0x04000805 RID: 2053
	private const int k_maxMatchmakingGames = 32;

	// Token: 0x04000806 RID: 2054
	public static Network m_Network;

	// Token: 0x04000807 RID: 2055
	public static bool m_bPausedDuringNetworkSession;

	// Token: 0x04000808 RID: 2056
	public static string m_loginName;

	// Token: 0x04000809 RID: 2057
	private static string m_loginPassword;

	// Token: 0x0400080A RID: 2058
	private static string m_asmodeeAccessToken;

	// Token: 0x0400080C RID: 2060
	[HideInInspector]
	public string m_LastNetworkStatus;

	// Token: 0x0400080D RID: 2061
	[HideInInspector]
	public bool m_bConnectedToServer;

	// Token: 0x0400080E RID: 2062
	[HideInInspector]
	public bool m_bLoggedInToServer;

	// Token: 0x0400080F RID: 2063
	[HideInInspector]
	public bool m_bServerConnectionLost;

	// Token: 0x04000810 RID: 2064
	[HideInInspector]
	public bool m_bServerDisconnected;

	// Token: 0x04000811 RID: 2065
	private bool m_bCreateAccount;

	// Token: 0x04000812 RID: 2066
	public NetworkEvent[] networkEvents;

	// Token: 0x04000813 RID: 2067
	private bool m_bNewAvailableGames;

	// Token: 0x04000814 RID: 2068
	private bool m_bRequestRefreshAvailableGames;

	// Token: 0x0200078C RID: 1932
	// (Invoke) Token: 0x06004255 RID: 16981
	[Serializable]
	public delegate void NetworkEventDelegate(NetworkEvent.EventType eventType, int eventData);
}
