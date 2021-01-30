using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000729 RID: 1833
	public static class AnalyticsEvents
	{
		// Token: 0x06003FA6 RID: 16294 RVA: 0x00135C30 File Offset: 0x00133E30
		public static void LogAppBootEvent(string lanchMethod, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			analyticsManager.AddContext(new ApplicationAnalyticsContext());
			analyticsManager.SetUserProperties(new Dictionary<string, object>());
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["launch_method"] = lanchMethod;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("APP_BOOT", dictionary2);
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00135CF8 File Offset: 0x00133EF8
		public static void LogCrossPromoDisplayedEvent(ShowcaseProduct[] products, CROSSPROMO_DISPLAYED.crosspromo_type crossPromoType, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = new CrossPromoAnalyticsContext(crossPromoType.ToString());
			analyticsManager.AddContext(crossPromoAnalyticsContext);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("crosspromo_session_id", crossPromoAnalyticsContext.CrossPromoSessionId);
			dictionary.Add("api_version", crossPromoAnalyticsContext.ApiVersion);
			dictionary.Add("crosspromo_type", crossPromoAnalyticsContext.CrossPromoType);
			dictionary.Add("product_id", string.Join<int>(",", (from p in products
			select p.Id).ToArray<int>()));
			dictionary.Add("product_name", string.Join(",", (from p in products
			select p.Name).ToArray<string>()));
			if (customEventProperties != null)
			{
				dictionary = customEventProperties.Concat(from p in dictionary
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CROSSPROMO_DISPLAYED", dictionary);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00135E6C File Offset: 0x0013406C
		public static void LogCrossPromoOpenedEvent(CROSSPROMO_OPENED.crosspromo_type? crossPromoType = null, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			CrossPromoAnalyticsContext crossPromoAnalyticsContext;
			if (!analyticsManager.HasContext(typeof(CrossPromoAnalyticsContext)))
			{
				crossPromoAnalyticsContext = new CrossPromoAnalyticsContext(crossPromoType.Value.ToString());
				analyticsManager.AddContext(crossPromoAnalyticsContext);
			}
			else
			{
				crossPromoAnalyticsContext = (analyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("crosspromo_session_id", crossPromoAnalyticsContext.CrossPromoSessionId);
			dictionary.Add("api_version", crossPromoAnalyticsContext.ApiVersion);
			dictionary.Add("crosspromo_type", crossPromoAnalyticsContext.CrossPromoType);
			dictionary.Add("is_automatic", crossPromoAnalyticsContext.CrossPromoType.Equals("interstitial"));
			if (customEventProperties != null)
			{
				dictionary = customEventProperties.Concat(from p in dictionary
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CROSSPROMO_OPENED", dictionary);
			crossPromoAnalyticsContext.CrossPromoOpenedEventLogged = true;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x00135FC0 File Offset: 0x001341C0
		public static void LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current screen, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action action, ShowcaseProduct product = null, Vector2? tileCoordinate = null, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = CoreApplication.Instance.AnalyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext;
			if (!crossPromoAnalyticsContext.CrossPromoOpenedEventLogged)
			{
				AnalyticsEvents.LogCrossPromoOpenedEvent(null, null);
			}
			crossPromoAnalyticsContext.ScreenCurrent = screen.ToString();
			switch (action)
			{
			case CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_featured:
				crossPromoAnalyticsContext.MoreGameCategory = CROSSPROMO_SCREEN_DISPLAY.more_game_category.featured.ToString();
				break;
			case CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_gamer:
				crossPromoAnalyticsContext.MoreGameCategory = CROSSPROMO_SCREEN_DISPLAY.more_game_category.advanced.ToString();
				break;
			case CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_family:
				crossPromoAnalyticsContext.MoreGameCategory = CROSSPROMO_SCREEN_DISPLAY.more_game_category.family.ToString();
				break;
			case CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_boardgame:
				crossPromoAnalyticsContext.MoreGameCategory = CROSSPROMO_SCREEN_DISPLAY.more_game_category.tabletop.ToString();
				break;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("crosspromo_session_id", crossPromoAnalyticsContext.CrossPromoSessionId);
			dictionary.Add("crosspromo_type", crossPromoAnalyticsContext.CrossPromoType);
			dictionary.Add("screen_count", crossPromoAnalyticsContext.ScreenCount);
			dictionary.Add("screen_current", crossPromoAnalyticsContext.ScreenCurrent);
			dictionary.Add("screen_previous", crossPromoAnalyticsContext.ScreenPrevious);
			dictionary.Add("screen_previous_time_sec", crossPromoAnalyticsContext.ScreenPreviousTime);
			dictionary.Add("screen_previous_nav_action", action);
			dictionary.Add("more_game_category", crossPromoAnalyticsContext.MoreGameCategory);
			dictionary.Add("game_detail_product_id", (product == null) ? null : string.Format("{0}", product.Id));
			dictionary.Add("game_detail_product_name", (product == null) ? null : product.Name);
			dictionary.Add("game_detail_product_type", (product == null) ? null : ((product.ShopDigitalUrl == null) ? "boardgame" : "digital"));
			dictionary.Add("clicked_crosspromo_tile_size", (product == null) ? null : string.Format("{0}x{1}", product.Tile.Width, product.Tile.Height));
			dictionary.Add("clicked_crosspromo_tile_position_xy", (tileCoordinate == null) ? null : string.Format("{0}-{1}", tileCoordinate.Value.x, tileCoordinate.Value.y));
			if (customEventProperties != null)
			{
				dictionary = customEventProperties.Concat(from p in dictionary
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00136288 File Offset: 0x00134488
		public static void LogCrossPromoRedirectedEvent(ShowcaseProduct product, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = analyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("crosspromo_session_id", crossPromoAnalyticsContext.CrossPromoSessionId);
			dictionary.Add("crosspromo_type", crossPromoAnalyticsContext.CrossPromoType);
			dictionary.Add("more_game_category", crossPromoAnalyticsContext.MoreGameCategory);
			dictionary.Add("product_id", string.Format("{0}", product.Id));
			dictionary.Add("product_name", product.Name);
			dictionary.Add("product_type", (product.ShopDigitalUrl == null) ? "boardgame" : "digital");
			if (customEventProperties != null)
			{
				dictionary = customEventProperties.Concat(from p in dictionary
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CROSSPROMO_REDIRECTED", dictionary);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x001363C8 File Offset: 0x001345C8
		public static void LogCrossPromoClosedEvent(IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = analyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext;
			if (!crossPromoAnalyticsContext.CrossPromoOpenedEventLogged)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("crosspromo_session_id", crossPromoAnalyticsContext.CrossPromoSessionId);
			dictionary.Add("crosspromo_type", crossPromoAnalyticsContext.CrossPromoType);
			dictionary.Add("time_active_sec", (int)crossPromoAnalyticsContext.LifeTime);
			if (customEventProperties != null)
			{
				dictionary = customEventProperties.Concat(from p in dictionary
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.RemoveContext(typeof(CrossPromoAnalyticsContext));
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x001364CC File Offset: 0x001346CC
		public static void LogContentDlStartEvent(string dlContentId, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ContentDlAnalyticsContext contentDlAnalyticsContext = new ContentDlAnalyticsContext(dlContentId);
			analyticsManager.AddContext(contentDlAnalyticsContext);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["dl_session_id"] = contentDlAnalyticsContext.DlSessionId;
			dictionary["dl_content_id"] = contentDlAnalyticsContext.DlContentId;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CONTENT_DL_START", dictionary2);
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x001365A4 File Offset: 0x001347A4
		public static void LogContentDlStopEvent(bool dlIsComplete, CONTENT_DL_STOP.dl_end_reason dlEndReason, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ContentDlAnalyticsContext contentDlAnalyticsContext = analyticsManager.GetContext(typeof(ContentDlAnalyticsContext)) as ContentDlAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["dl_session_id"] = contentDlAnalyticsContext.DlSessionId;
			dictionary["dl_content_id"] = contentDlAnalyticsContext.DlContentId;
			dictionary["dl_is_complete"] = dlIsComplete;
			dictionary["dl_end_reason"] = dlEndReason;
			dictionary["dl_time"] = contentDlAnalyticsContext.DlTime;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CONTENT_DL_STOP", dictionary2);
			analyticsManager.RemoveContext(typeof(ContentDlAnalyticsContext));
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x001366CC File Offset: 0x001348CC
		public static void LogMatchStartEvent(string matchSessionId, string lobbySessionId, string mode, string mapId, string[] activatedDlc, int playerCountHuman, int playerCountAi, int? playerPlayOrder, string launchMethod, int? playerClockSec, string difficulty, bool isOnline, bool isTutorial, bool? isAsynchronous, bool? isPrivate, bool? isRanked, bool? isObservable, MATCH_START.obs_access? obsAccess, bool? obsShowHiddenInfo, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			MatchAnalyticsContext matchAnalyticsContext = new MatchAnalyticsContext(matchSessionId, mode, mapId, (activatedDlc != null) ? string.Join(",", activatedDlc) : null, playerPlayOrder, playerClockSec, difficulty, isOnline, isTutorial, isAsynchronous, isPrivate, isRanked, isObservable, obsAccess, obsShowHiddenInfo);
			analyticsManager.AddContext(matchAnalyticsContext);
			(analyticsManager.GetContext(typeof(ApplicationAnalyticsContext)) as ApplicationAnalyticsContext).StartGameplay();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["match_session_id"] = matchAnalyticsContext.MatchSessionId;
			dictionary["lobby_session_id"] = lobbySessionId;
			dictionary["mode"] = matchAnalyticsContext.Mode;
			dictionary["map_id"] = matchAnalyticsContext.MapId;
			dictionary["activated_dlc"] = matchAnalyticsContext.ActivatedDlc;
			dictionary["player_count_human"] = playerCountHuman;
			dictionary["player_count_ai"] = playerCountAi;
			dictionary["player_playorder"] = matchAnalyticsContext.PlayerPlayOrder;
			dictionary["launch_method"] = launchMethod;
			dictionary["player_clock_sec"] = matchAnalyticsContext.PlayerClockSec;
			dictionary["difficulty"] = matchAnalyticsContext.Difficulty;
			dictionary["is_online"] = matchAnalyticsContext.IsOnline;
			dictionary["is_tutorial"] = matchAnalyticsContext.IsTutorial;
			dictionary["is_asynchronous"] = matchAnalyticsContext.IsAsynchronous;
			dictionary["is_private"] = matchAnalyticsContext.IsPrivate;
			dictionary["is_ranked"] = matchAnalyticsContext.IsRanked;
			dictionary["is_observable"] = matchAnalyticsContext.IsObservable;
			dictionary["obs_access"] = matchAnalyticsContext.ObsAccess;
			dictionary["obs_show_hidden_info"] = matchAnalyticsContext.ObsShowHiddenInfo;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("MATCH_START", dictionary2);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00136934 File Offset: 0x00134B34
		public static void LogMatchStopEvent(int playerCountHuman, int playerCountAi, string endReason, MATCH_STOP.player_result? playerResult, int? turnCount, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			MatchAnalyticsContext matchAnalyticsContext = analyticsManager.GetContext(typeof(MatchAnalyticsContext)) as MatchAnalyticsContext;
			(analyticsManager.GetContext(typeof(ApplicationAnalyticsContext)) as ApplicationAnalyticsContext).StopGameplay();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["match_session_id"] = matchAnalyticsContext.MatchSessionId;
			dictionary["mode"] = matchAnalyticsContext.Mode;
			dictionary["map_id"] = matchAnalyticsContext.MapId;
			dictionary["activated_dlc"] = matchAnalyticsContext.ActivatedDlc;
			dictionary["player_count_human"] = playerCountHuman;
			dictionary["player_count_ai"] = playerCountAi;
			dictionary["player_playorder"] = matchAnalyticsContext.PlayerPlayOrder;
			dictionary["time_active_sec"] = matchAnalyticsContext.TimeActiveSec;
			dictionary["end_reason"] = endReason;
			dictionary["player_result"] = playerResult;
			dictionary["player_clock_sec"] = matchAnalyticsContext.PlayerClockSec;
			dictionary["difficulty"] = matchAnalyticsContext.Difficulty;
			dictionary["is_online"] = matchAnalyticsContext.IsOnline;
			dictionary["is_tutorial"] = matchAnalyticsContext.IsTutorial;
			dictionary["is_asynchronous"] = matchAnalyticsContext.IsAsynchronous;
			dictionary["is_private"] = matchAnalyticsContext.IsPrivate;
			dictionary["is_ranked"] = matchAnalyticsContext.IsRanked;
			dictionary["is_observable"] = matchAnalyticsContext.IsObservable;
			dictionary["obs_access"] = matchAnalyticsContext.ObsAccess;
			dictionary["obs_show_hidden_info"] = matchAnalyticsContext.ObsShowHiddenInfo;
			dictionary["turn_count"] = turnCount;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("MATCH_STOP", dictionary2);
			analyticsManager.RemoveContext(typeof(MatchAnalyticsContext));
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00136BB4 File Offset: 0x00134DB4
		public static void LogLobbyStartEvent(int onlinePlayerCountConnected, int onlinePlayerCountLobbyOrTable, int onlinePlayerCountTable, int onlinePlayerCountMatch, int onlineOpenTableCount, int onlineOngoingMatchCount, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			LobbyAnalyticsContext lobbyAnalyticsContext = new LobbyAnalyticsContext();
			analyticsManager.AddContext(lobbyAnalyticsContext);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["lobby_session_id"] = lobbyAnalyticsContext.LobbySessionId;
			dictionary["online_player_count_connected"] = onlinePlayerCountConnected;
			dictionary["online_player_count_lobbyortable"] = onlinePlayerCountLobbyOrTable;
			dictionary["online_player_count_table"] = onlinePlayerCountTable;
			dictionary["online_player_count_match"] = onlinePlayerCountMatch;
			dictionary["online_open_table_count"] = onlineOpenTableCount;
			dictionary["online_ongoing_match_count"] = onlineOngoingMatchCount;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("LOBBY_START", dictionary2);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00136CE4 File Offset: 0x00134EE4
		public static void LogLobbyStopEvent(int onlinePlayerCountConnected, int onlinePlayerCountLobbyOrTable, int onlinePlayerCountTable, int onlinePlayerCountMatch, int onlineOpenTableCount, int onlineOngoingMatchCount, string endReason, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			LobbyAnalyticsContext lobbyAnalyticsContext = analyticsManager.GetContext(typeof(LobbyAnalyticsContext)) as LobbyAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["lobby_session_id"] = lobbyAnalyticsContext.LobbySessionId;
			dictionary["online_player_count_connected"] = onlinePlayerCountConnected;
			dictionary["online_player_count_lobbyortable"] = onlinePlayerCountLobbyOrTable;
			dictionary["online_player_count_table"] = onlinePlayerCountTable;
			dictionary["online_player_count_match"] = onlinePlayerCountMatch;
			dictionary["online_open_table_count"] = onlineOpenTableCount;
			dictionary["online_ongoing_match_count"] = onlineOngoingMatchCount;
			dictionary["time_active_sec"] = lobbyAnalyticsContext.TimeActiveSec;
			dictionary["end_reason"] = endReason;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("LOBBY_STOP", dictionary2);
			analyticsManager.RemoveContext(typeof(LobbyAnalyticsContext));
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00136E50 File Offset: 0x00135050
		public static void LogTableStartEvent(string lobbySessionId, string matchSessionId, TABLE_START.launch_method launchMethod, int playerCountSlots, int playerCountHuman, int playerCountAi, int playerClockSec, bool isAsynchronous, bool isPrivate, bool isRanked, bool isObservable, TABLE_START.obs_access obsAccess, bool obsShowHiddenInfo, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			TableAnalyticsContext tableAnalyticsContext = new TableAnalyticsContext(matchSessionId, playerClockSec, isAsynchronous, isPrivate, isRanked, isObservable, obsAccess, obsShowHiddenInfo);
			analyticsManager.AddContext(tableAnalyticsContext);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["lobby_session_id"] = lobbySessionId;
			dictionary["match_session_id"] = tableAnalyticsContext.MatchSessionId;
			dictionary["launch_method"] = launchMethod;
			dictionary["player_count_slots"] = playerCountSlots;
			dictionary["player_count_human"] = playerCountHuman;
			dictionary["player_count_ai"] = playerCountAi;
			dictionary["player_clock_sec"] = tableAnalyticsContext.PlayerClockSec;
			dictionary["is_asynchronous"] = tableAnalyticsContext.IsAsynchronous;
			dictionary["is_private"] = tableAnalyticsContext.IsPrivate;
			dictionary["is_ranked"] = tableAnalyticsContext.IsRanked;
			dictionary["is_observable"] = tableAnalyticsContext.IsObservable;
			dictionary["obs_access"] = tableAnalyticsContext.ObsAccess;
			dictionary["obs_show_hidden_info"] = tableAnalyticsContext.ObsShowHiddenInfo;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("TABLE_START", dictionary2);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00137010 File Offset: 0x00135210
		public static void LogTableStopEvent(string endReason, int playerCountSlots, int playerCountHuman, int playerCountAi, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			TableAnalyticsContext tableAnalyticsContext = analyticsManager.GetContext(typeof(TableAnalyticsContext)) as TableAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["match_session_id"] = tableAnalyticsContext.MatchSessionId;
			dictionary["end_reason"] = endReason;
			dictionary["player_count_slots"] = playerCountSlots;
			dictionary["player_count_human"] = playerCountHuman;
			dictionary["player_count_ai"] = playerCountAi;
			dictionary["player_clock_sec"] = tableAnalyticsContext.PlayerClockSec;
			dictionary["is_asynchronous"] = tableAnalyticsContext.IsAsynchronous;
			dictionary["is_private"] = tableAnalyticsContext.IsPrivate;
			dictionary["is_ranked"] = tableAnalyticsContext.IsRanked;
			dictionary["is_observable"] = tableAnalyticsContext.IsObservable;
			dictionary["obs_access"] = tableAnalyticsContext.ObsAccess;
			dictionary["obs_show_hidden_info"] = tableAnalyticsContext.ObsShowHiddenInfo;
			dictionary["time_active_sec"] = tableAnalyticsContext.TimeActiveSec;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("TABLE_STOP", dictionary2);
			analyticsManager.RemoveContext(typeof(TableAnalyticsContext));
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x001371E0 File Offset: 0x001353E0
		public static void LogScreenDisplayEvent(string screenCurrent, string context, string screenPreviousNavAction, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ScreenDisplayAnalyticsContext screenDisplayAnalyticsContext;
			if (!analyticsManager.HasContext(typeof(ScreenDisplayAnalyticsContext)))
			{
				screenDisplayAnalyticsContext = new ScreenDisplayAnalyticsContext(screenCurrent);
				analyticsManager.AddContext(screenDisplayAnalyticsContext);
			}
			else
			{
				screenDisplayAnalyticsContext = (analyticsManager.GetContext(typeof(ScreenDisplayAnalyticsContext)) as ScreenDisplayAnalyticsContext);
				screenDisplayAnalyticsContext.ScreenCurrent = screenCurrent;
			}
			screenDisplayAnalyticsContext.Context = context;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["screen_previous"] = screenDisplayAnalyticsContext.ScreenPrevious;
			dictionary["screen_current"] = screenDisplayAnalyticsContext.ScreenCurrent;
			dictionary["screen_count"] = screenDisplayAnalyticsContext.ScreenCount;
			dictionary["context"] = context;
			dictionary["screen_previous_nav_action"] = screenPreviousNavAction;
			dictionary["time_screen_previous_sec"] = screenDisplayAnalyticsContext.ScreenPreviousTime;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SCREEN_DISPLAY", dictionary2);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x00137338 File Offset: 0x00135538
		public static void LogShopStartEvent(string entryPoint, string itemId, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ShopAnalyticsContext shopAnalyticsContext = new ShopAnalyticsContext(entryPoint, itemId);
			analyticsManager.AddContext(shopAnalyticsContext);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["shop_session_id"] = shopAnalyticsContext.ShopSessionId;
			dictionary["entry_point"] = shopAnalyticsContext.EntryPoint;
			dictionary["item_id"] = itemId;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SHOP_START", dictionary2);
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0013741C File Offset: 0x0013561C
		public static void LogShopStopEvent(IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ShopAnalyticsContext shopAnalyticsContext = analyticsManager.GetContext(typeof(ShopAnalyticsContext)) as ShopAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["shop_session_id"] = shopAnalyticsContext.ShopSessionId;
			dictionary["did_purchase"] = shopAnalyticsContext.DidPurchase;
			dictionary["time_active_sec"] = shopAnalyticsContext.TimeActiveSec;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SHOP_STOP", dictionary2);
			analyticsManager.RemoveContext(typeof(ShopAnalyticsContext));
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x00137528 File Offset: 0x00135728
		public static void LogShopItemFocusEvent(string itemId, float itemPrice, string itemCurrency, int itemQuantity, bool itemView, bool itemPurchase, bool itemPurchaseConfirmedUser, bool itemPurchaseConfirmedFirstParty, string transactionBackendId, string transactionFirstPartyId, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ShopAnalyticsContext shopAnalyticsContext = analyticsManager.GetContext(typeof(ShopAnalyticsContext)) as ShopAnalyticsContext;
			bool? flag = null;
			if (shopAnalyticsContext != null)
			{
				if (itemPurchaseConfirmedFirstParty)
				{
					shopAnalyticsContext.DidPurchase = true;
				}
				flag = new bool?(shopAnalyticsContext.DefaultItemId == itemId);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["shop_session_id"] = ((shopAnalyticsContext != null) ? shopAnalyticsContext.ShopSessionId : null);
			dictionary["entry_point"] = ((shopAnalyticsContext != null) ? shopAnalyticsContext.EntryPoint : null);
			dictionary["item_id"] = itemId;
			dictionary["item_price"] = itemPrice;
			dictionary["item_currency"] = itemCurrency;
			dictionary["item_quantity"] = itemQuantity;
			dictionary["item_view"] = itemView;
			dictionary["item_purchase"] = itemPurchase;
			dictionary["item_purchase_confirmed_user"] = itemPurchaseConfirmedUser;
			dictionary["item_purchase_confirmed_first_party"] = itemPurchaseConfirmedFirstParty;
			dictionary["transaction_backend_id"] = transactionBackendId;
			dictionary["transaction_first_party_id"] = transactionFirstPartyId;
			dictionary["transaction_price"] = itemPrice * (float)itemQuantity;
			dictionary["transaction_currency"] = itemCurrency;
			dictionary["purchases_outside_shop"] = (shopAnalyticsContext == null);
			dictionary["is_default_item"] = flag;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SHOP_ITEMFOCUS", dictionary2);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x00137720 File Offset: 0x00135920
		public static void LogSocialNetworkConnectEvent(string socialNetworkName, int? age, string gender, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["social_network_name"] = socialNetworkName;
			dictionary["age"] = age;
			dictionary["gender"] = gender;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SOCIAL_NETWORK_CONNECT", dictionary2);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x001377F0 File Offset: 0x001359F0
		public static void LogSocialNetworkShareEvent(string shareType, string socialNetworkName, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["share_type"] = shareType;
			dictionary["social_network_name"] = socialNetworkName;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SOCIAL_NETWORK_SHARE", dictionary2);
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x001378B0 File Offset: 0x00135AB0
		public static void LogSocialNetworkAsmodeeRedirectedEvent(string socialNetworkName, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["social_network_name"] = socialNetworkName;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SOCIAL_NETWORK_ASMODEE_REDIRECTED", dictionary2);
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00137964 File Offset: 0x00135B64
		public static void LogMilestoneEvent(string milestoneType, string milestoneValue, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["milestone_type"] = milestoneType;
			dictionary["milestone_value"] = milestoneValue;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("MILESTONE", dictionary2);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00137A24 File Offset: 0x00135C24
		public static void LogAdDisplayEvent(string adType, bool isSkippable, bool isSkipped, int timeSec, string adTrigger, string contextId, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["ad_type"] = adType;
			dictionary["is_skippable"] = isSkippable;
			dictionary["is_skipped"] = isSkipped;
			dictionary["time_sec"] = timeSec;
			dictionary["ad_trigger"] = adTrigger;
			dictionary["context_id"] = contextId;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("AD_DISPLAY", dictionary2);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x00137B24 File Offset: 0x00135D24
		public static void LogContentUnlockEvent(string contentId, string contentType, string unlockReason, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["content_id"] = contentId;
			dictionary["content_type"] = contentType;
			dictionary["unlock_reason"] = unlockReason;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CONTENT_UNLOCK", dictionary2);
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00137BF0 File Offset: 0x00135DF0
		public static void LogAchievementUnlockEvent(string achievementId, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["achievement_id"] = achievementId;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("ACHIEVEMENT_UNLOCK", dictionary2);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00137CA4 File Offset: 0x00135EA4
		public static void LogFriendManagementEvent(string action, string friendUserId, int friendCount, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = action;
			dictionary["friend_user_id"] = friendUserId;
			dictionary["friend_count"] = friendCount;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("FRIEND_MANAGEMENT", dictionary2);
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00137D74 File Offset: 0x00135F74
		public static void LogTutorialStepEvent(string stepId, float stepSequenceNumber, TUTORIAL_STEP.step_status stepStatus, int timeOnStep, bool isTutoComplete, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["step_id"] = stepId;
			dictionary["step_sequence_number"] = stepSequenceNumber;
			dictionary["step_status"] = stepStatus;
			dictionary["time_on_step"] = timeOnStep;
			dictionary["is_tuto_complete"] = isTutoComplete;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("TUTORIAL_STEP", dictionary2);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00137E6C File Offset: 0x0013606C
		public static void LogSettingChangeEvent(string setting, string valueOld, string valueNew, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["setting"] = setting;
			dictionary["value_old"] = valueOld;
			dictionary["value_new"] = valueNew;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("SETTING_CHANGE", dictionary2);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00137F38 File Offset: 0x00136138
		public static void LogCreateAsmodeeNetAccountEvent(bool isEmailOptIn, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ConnectAsmodeeNetAnalyticsContext connectAsmodeeNetAnalyticsContext = analyticsManager.GetContext(typeof(ConnectAsmodeeNetAnalyticsContext)) as ConnectAsmodeeNetAnalyticsContext;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["signin_session_id"] = connectAsmodeeNetAnalyticsContext.SigninSessionId;
			dictionary["is_email_opt_in"] = isEmailOptIn;
			dictionary["signup_path"] = connectAsmodeeNetAnalyticsContext.ConnectPath;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CREATE_ASMODEENET_ACCOUNT", dictionary2);
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00138028 File Offset: 0x00136228
		public static void LogConnectAsmodeeNetStartEvent(string connectPath, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ConnectAsmodeeNetAnalyticsContext context = new ConnectAsmodeeNetAnalyticsContext(connectPath);
			analyticsManager.AddContext(context);
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00138064 File Offset: 0x00136264
		public static void LogConnectAsmodeeNetStopEvent(bool isSigninComplete, string lastError, bool resetPassword, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			ConnectAsmodeeNetAnalyticsContext connectAsmodeeNetAnalyticsContext = analyticsManager.GetContext(typeof(ConnectAsmodeeNetAnalyticsContext)) as ConnectAsmodeeNetAnalyticsContext;
			analyticsManager.RemoveContext(typeof(ConnectAsmodeeNetAnalyticsContext));
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x001380B8 File Offset: 0x001362B8
		public static void LogInstallSourceEvent(string source, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["source"] = source;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("INSTALL_SOURCE", dictionary2);
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0013816C File Offset: 0x0013636C
		public static void LogCustomerSupportDisplayedEvent(string openPath, string errorMessage, IDictionary<string, object> customEventProperties = null)
		{
			if (!CoreApplication.HasInstance)
			{
				return;
			}
			AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["open_path"] = openPath;
			dictionary["error_message"] = errorMessage;
			Dictionary<string, object> dictionary2 = dictionary;
			if (customEventProperties != null)
			{
				dictionary2 = customEventProperties.Concat(from p in dictionary2
				where !customEventProperties.Keys.Contains(p.Key)
				select p).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			}
			analyticsManager.LogEvent("CUSTOMER_SUPPORT_DISPLAYED", dictionary2);
		}
	}
}
