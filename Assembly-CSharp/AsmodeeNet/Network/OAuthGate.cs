using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.UserInterface.SSO;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network
{
	// Token: 0x02000688 RID: 1672
	public class OAuthGate
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06003D06 RID: 15622 RVA: 0x0012C7B4 File Offset: 0x0012A9B4
		// (set) Token: 0x06003D07 RID: 15623 RVA: 0x0012C7BC File Offset: 0x0012A9BC
		public NetworkParameters NetworkParameters { get; protected set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x0012C7C5 File Offset: 0x0012A9C5
		// (set) Token: 0x06003D09 RID: 15625 RVA: 0x0012C7CD File Offset: 0x0012A9CD
		protected AuthenticationTokens _PrivateAuthenticationTokens { get; set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0012C7D6 File Offset: 0x0012A9D6
		// (set) Token: 0x06003D0B RID: 15627 RVA: 0x0012C7DE File Offset: 0x0012A9DE
		protected AuthenticationTokens _PublicAuthenticationTokens { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x0012C7E7 File Offset: 0x0012A9E7
		protected AuthenticationTokens _AuthenticationTokens
		{
			get
			{
				if (this._PrivateAuthenticationTokens != null)
				{
					return this._PrivateAuthenticationTokens;
				}
				return this._PublicAuthenticationTokens;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x0012C7FE File Offset: 0x0012A9FE
		// (set) Token: 0x06003D0E RID: 15630 RVA: 0x0012C806 File Offset: 0x0012AA06
		public AsmodeeNet.Foundation.SteamManager SteamManager { get; set; }

		// Token: 0x06003D0F RID: 15631 RVA: 0x0012C810 File Offset: 0x0012AA10
		public OAuthGate(NetworkParameters networkParameters)
		{
			if (networkParameters == null)
			{
				throw new ArgumentNullException("networkParameters");
			}
			this.NetworkParameters = networkParameters;
			this._publicRequestSender = new OAuthGate.RequestSender(this.NetworkParameters);
			this._privateRequestSender = new OAuthGate.RequestSender(this.NetworkParameters);
			this.SetSSOHandler(delegate(SSOAuthenticate authMethod, OnSSOSucceeded onSSOSucceed, AbortSSO onSSOAbort)
			{
				SSOManager.InstantiateSSO(authMethod, onSSOSucceed, onSSOAbort);
			});
			if (this.SteamManager != null)
			{
				this.SteamManager.AutoConnect = true;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x0012C8AF File Offset: 0x0012AAAF
		public string AccessToken
		{
			get
			{
				if (this._AuthenticationTokens != null)
				{
					return this._AuthenticationTokens.access_token;
				}
				return null;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003D11 RID: 15633 RVA: 0x0012C8C8 File Offset: 0x0012AAC8
		public string RefreshToken
		{
			get
			{
				if (this._PrivateAuthenticationTokens != null && !string.IsNullOrEmpty(this._PrivateAuthenticationTokens.refresh_token))
				{
					return this._PrivateAuthenticationTokens.refresh_token;
				}
				if (!KeyValueStore.HasKey("RefreshToken"))
				{
					return null;
				}
				return StringCipher.Decrypt(KeyValueStore.GetString("RefreshToken", ""), UniqueId.GetUniqueId());
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x0012C922 File Offset: 0x0012AB22
		public int TokenLifeSpan
		{
			get
			{
				if (this._AuthenticationTokens != null)
				{
					return this._AuthenticationTokens.expires_in;
				}
				return -1;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003D13 RID: 15635 RVA: 0x0012C939 File Offset: 0x0012AB39
		public string TokenScope
		{
			get
			{
				if (this._AuthenticationTokens != null && !this._AuthenticationTokens.HasExpired)
				{
					return this._AuthenticationTokens.scope;
				}
				return null;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x0012C95D File Offset: 0x0012AB5D
		public bool HasPublicToken
		{
			get
			{
				return this._AuthenticationTokens != null && this._AuthenticationTokens.HasPublicToken;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003D15 RID: 15637 RVA: 0x0012C974 File Offset: 0x0012AB74
		public bool IsRetrievingPublicToken
		{
			get
			{
				return this._publicRequestSender.request != null && (this._publicRequestSender.request.State == HTTPRequestStates.Queued || this._publicRequestSender.request.State == HTTPRequestStates.Processing);
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06003D16 RID: 15638 RVA: 0x0012C9AD File Offset: 0x0012ABAD
		public bool HasPrivateToken
		{
			get
			{
				return this._AuthenticationTokens != null && this._AuthenticationTokens.HasPrivateToken;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x0012C9C4 File Offset: 0x0012ABC4
		public bool IsRetrievingPrivateToken
		{
			get
			{
				return this._privateRequestSender.request != null && (this._privateRequestSender.request.State == HTTPRequestStates.Queued || this._privateRequestSender.request.State == HTTPRequestStates.Processing);
			}
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0012C9FD File Offset: 0x0012ABFD
		public void LogOut()
		{
			this._PrivateAuthenticationTokens = null;
			this.UserDetails = null;
			this._DeleteRefreshTokenFromDisk();
			if (this.SteamManager != null)
			{
				this.SteamManager.AutoConnect = false;
			}
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0012CA28 File Offset: 0x0012AC28
		public bool CancelAccessTokenRequest(int callbackId)
		{
			if (this._callbacksForPrivateToken.ContainsKey(callbackId))
			{
				this._callbacksForPrivateToken.Remove(callbackId);
				if (this._callbacksForPrivateToken.Count == 0)
				{
					this._privateRequestSender.Reset();
				}
				return true;
			}
			if (this._callbacksForPublicToken.ContainsKey(callbackId))
			{
				this._callbacksForPublicToken.Remove(callbackId);
				if (this._callbacksForPublicToken.Count == 0)
				{
					this._publicRequestSender.Reset();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0012CAA0 File Offset: 0x0012ACA0
		private void _WriteRefreshTokenToDisk()
		{
			if (!string.IsNullOrEmpty(this._PrivateAuthenticationTokens.refresh_token))
			{
				KeyValueStore.SetString("RefreshToken", StringCipher.Encrypt(this._PrivateAuthenticationTokens.refresh_token, UniqueId.GetUniqueId()));
				KeyValueStore.Save();
			}
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0012CAD8 File Offset: 0x0012ACD8
		private void _DeleteRefreshTokenFromDisk()
		{
			KeyValueStore.DeleteKey("RefreshToken");
			KeyValueStore.Save();
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x0012CAE9 File Offset: 0x0012ACE9
		private void _RequireNonNullSSOHandler()
		{
			if (this._ssoHandler == null)
			{
				throw new ArgumentException("SetSSOHandler() must be called first with a valid SSO starter function", "SetSSOHandler");
			}
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x0012CB04 File Offset: 0x0012AD04
		private void _ExecuteAndFlushCallbacks(Dictionary<int, OAuthCallback> callbacks, OAuthError authError)
		{
			if (this.isFlushingCallbacks)
			{
				throw new Exception("Recursive call to _ExecuteAndFlushCallbacks !! ABORTING !");
			}
			this.isFlushingCallbacks = true;
			foreach (KeyValuePair<int, OAuthCallback> keyValuePair in callbacks)
			{
				if (keyValuePair.Value != null)
				{
					try
					{
						keyValuePair.Value(authError);
					}
					catch (Exception ex)
					{
						AsmoLogger.Error("OAuthGate", "OAuthGate was executing callbacks (with attached error) when something bad happened" + authError, Reflection.HashtableFromObject(authError, null, 30U));
						AsmoLogger.LogException(ex, "OAuthGate", AsmoLogger.Severity.Error);
					}
				}
			}
			callbacks.Clear();
			this.isFlushingCallbacks = false;
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x0012CBC4 File Offset: 0x0012ADC4
		public int? GetPublicAccessToken(OAuthCallback onComplete)
		{
			if (this.HasPublicToken)
			{
				if (onComplete != null)
				{
					onComplete(null);
				}
				return null;
			}
			bool flag = this._callbacksForPublicToken.Count >= 1;
			int num = OAuthGate.nextCallbackID++;
			this._callbacksForPublicToken[num] = onComplete;
			if (flag)
			{
				return new int?(num);
			}
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{
					"grant_type",
					"client_credentials"
				},
				{
					"client_id",
					this.NetworkParameters.ClientId
				}
			};
			Hashtable logParams = hashtable.Clone() as Hashtable;
			logParams.Add("url", text);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			AsmoLogger.Info("OAuthGate.sender", "Getting public access token", logParams);
			this._publicRequestSender.Reset();
			this._publicRequestSender.SendRequest(text, hashtable, delegate(OAuthError onRequestComplete)
			{
				if (onRequestComplete != null)
				{
					logParams.Add("status", onRequestComplete.status);
					logParams.Add("error", onRequestComplete.error);
					logParams.Add("error_description", onRequestComplete.error_description);
					AsmoLogger.Error("OAuthGate.receiver", "Public access token failure", logParams);
				}
				else
				{
					AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._publicRequestSender.response.DataAsText);
					authenticationTokens.InitExpiration();
					this._PublicAuthenticationTokens = authenticationTokens;
					logParams.Add("expires_in", authenticationTokens.expires_in);
					logParams.Add("scope", authenticationTokens.scope);
					AsmoLogger.Info("OAuthGate.receiver", "Public access token success", logParams);
				}
				this._publicRequestSender.Reset();
				this._ExecuteAndFlushCallbacks(this._callbacksForPublicToken, onRequestComplete);
			});
			return new int?(num);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x0012CCE9 File Offset: 0x0012AEE9
		public void SetSSOHandler(SSOHandler ssoHandler)
		{
			this._ssoHandler = ssoHandler;
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x0012CCF4 File Offset: 0x0012AEF4
		protected void _AbortSSO()
		{
			OAuthError oauthError = new OAuthError();
			oauthError.status = -1;
			oauthError.error = "aborted_by_user";
			oauthError.error_description = "User didn't complete the SSO";
			Hashtable hashtable = new Hashtable();
			hashtable["status"] = oauthError.status;
			hashtable["error"] = oauthError.error;
			hashtable["error_description"] = oauthError.error_description;
			AsmoLogger.Warning("OAuthGate.receiver", "Private access token failure", hashtable);
			this._privateRequestSender.Reset();
			this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, oauthError);
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x0012CD8A File Offset: 0x0012AF8A
		// (set) Token: 0x06003D22 RID: 15650 RVA: 0x0012CD92 File Offset: 0x0012AF92
		public User UserDetails { get; private set; }

		// Token: 0x06003D23 RID: 15651 RVA: 0x0012CD9C File Offset: 0x0012AF9C
		public void UpdateUserDetails(Action onComplete, bool checkEmailValidity = true)
		{
			GetUserDetailsEndpoint endpoint = new GetUserDetailsEndpoint((Extras)15, this);
			Action<User, WebError> <>9__2;
			Action<bool> <>9__1;
			endpoint.Execute(delegate(User result, WebError error)
			{
				if (error == null)
				{
					this.UserDetails = result;
					if (checkEmailValidity && result.EmailValid != null && !result.EmailValid.Value)
					{
						DateTime now = DateTime.Now;
						DateTime d;
						bool flag = !KeyValueStore.HasKey("UpdateEmailPopUpDisplayDate") || !DateTime.TryParse(KeyValueStore.GetString("UpdateEmailPopUpDisplayDate", ""), out d) || (now - d).TotalDays >= 7.0;
						if (flag)
						{
							KeyValueStore.SetString("UpdateEmailPopUpDisplayDate", now.ToString());
							Action<bool> onComplete2;
							if ((onComplete2 = <>9__1) == null)
							{
								onComplete2 = (<>9__1 = delegate(bool success)
								{
									if (success)
									{
										Endpoint<User> endpoint = endpoint;
										Action<User, WebError> onCompletion;
										if ((onCompletion = <>9__2) == null)
										{
											onCompletion = (<>9__2 = delegate(User result2, WebError error2)
											{
												if (error2 == null)
												{
													this.UserDetails = result2;
												}
												if (onComplete != null)
												{
													onComplete();
												}
											});
										}
										endpoint.Execute(onCompletion);
										return;
									}
									if (onComplete != null)
									{
										onComplete();
									}
								});
							}
							SSOManager.InstantiateUpdateEmailPopUp(onComplete2);
							return;
						}
					}
				}
				else
				{
					AsmoLogger.Error("OAuthGate.receiver", "Unable to retreive the UserDetails", null);
				}
				if (onComplete != null)
				{
					onComplete();
				}
			});
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x0012CDEC File Offset: 0x0012AFEC
		protected void _OnSSOSucceeded()
		{
			AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._privateRequestSender.response.DataAsText);
			authenticationTokens.InitExpiration();
			this._PrivateAuthenticationTokens = authenticationTokens;
			this._WriteRefreshTokenToDisk();
			this._logParamsForPrivateToken["expires_in"] = authenticationTokens.expires_in;
			this._logParamsForPrivateToken["scope"] = authenticationTokens.scope;
			AsmoLogger.Info("OAuthGate.receiver", "Private access token success", this._logParamsForPrivateToken);
			this._AuthenticationPostProcess();
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x0012CE70 File Offset: 0x0012B070
		public int? GetPrivateAccessToken(bool silentFailure, OAuthCallback onComplete)
		{
			if (this.HasPrivateToken)
			{
				if (onComplete != null)
				{
					onComplete(null);
				}
				return null;
			}
			AsmoLogger.Info("OAuthGate", "Looking for a private access token...", null);
			bool flag = this._callbacksForPrivateToken.Count >= 1;
			int num = OAuthGate.nextCallbackID++;
			this._callbacksForPrivateToken[num] = onComplete;
			if (flag)
			{
				return new int?(num);
			}
			if (this.RefreshToken != null)
			{
				this._AuthenticateWithRefreshToken(silentFailure);
				return new int?(num);
			}
			if (this.SteamManager != null && this.SteamManager.AutoConnect)
			{
				this._AuthenticateWithSteam(silentFailure);
				return new int?(num);
			}
			if (silentFailure)
			{
				AsmoLogger.Warning("OAuthGate", "Silently failed to find a private access token", null);
				OAuthError authError = OAuthError.MakeSilentAuthError();
				this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, authError);
				return null;
			}
			this._RequireNonNullSSOHandler();
			this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
			return new int?(num);
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x0012CF84 File Offset: 0x0012B184
		protected void _AuthenticateUser(string login, string password, OAuthCallback onComplete)
		{
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{
					"grant_type",
					"password"
				},
				{
					"client_id",
					this.NetworkParameters.ClientId
				},
				{
					"username",
					login
				}
			};
			this._logParamsForPrivateToken = (hashtable.Clone() as Hashtable);
			this._logParamsForPrivateToken.Add("url", text);
			AsmoLogger.Info("OAuthGate.sender", "Getting private access token", this._logParamsForPrivateToken);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			hashtable.Add("password", password);
			this._privateRequestSender.Reset();
			this._privateRequestSender.SendRequest(text, hashtable, onComplete);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x0012D054 File Offset: 0x0012B254
		protected void _AuthenticateWithRefreshToken(bool silentFailure)
		{
			string refreshToken = this.RefreshToken;
			if (string.IsNullOrEmpty(refreshToken))
			{
				throw new Exception("Impossible to authenticate without a refresh token");
			}
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{
					"grant_type",
					"refresh_token"
				},
				{
					"client_id",
					this.NetworkParameters.ClientId
				}
			};
			Hashtable logParams = hashtable.Clone() as Hashtable;
			logParams.Add("url", text);
			AsmoLogger.Info("OAuthGate.sender", "Requesting an access token by refresh token", logParams);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			hashtable.Add("refresh_token", refreshToken);
			this._privateRequestSender.SendRequest(text, hashtable, delegate(OAuthError error)
			{
				if (error == null)
				{
					AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._privateRequestSender.response.DataAsText);
					authenticationTokens.InitExpiration();
					this._PrivateAuthenticationTokens = authenticationTokens;
					this._WriteRefreshTokenToDisk();
					logParams.Add("expires_in", authenticationTokens.expires_in);
					logParams.Add("scope", authenticationTokens.scope);
					AsmoLogger.Info("OAuthGate.receiver", "Authentication successful", logParams);
					this._AuthenticationPostProcess();
					return;
				}
				logParams.Add("status", error.status);
				logParams.Add("error", error.error);
				logParams.Add("error_description", error.error_description);
				AsmoLogger.Error("OAuthGate.receiver", "Private access token failure", logParams);
				if (error.status >= 400)
				{
					this._DeleteRefreshTokenFromDisk();
				}
				if (silentFailure)
				{
					AsmoLogger.Warning("OAuthGate", "Silently failed to find a private access token", null);
					OAuthError authError = OAuthError.MakeSilentAuthError();
					this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, authError);
					return;
				}
				this._RequireNonNullSSOHandler();
				this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
			});
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x0012D144 File Offset: 0x0012B344
		private void _AuthenticateWithSteam(bool silentFailure)
		{
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			if (this.SteamManager != null && this.SteamManager.HasClient)
			{
				Hashtable hashtable = new Hashtable
				{
					{
						"grant_type",
						"steam_partner"
					},
					{
						"client_id",
						this.NetworkParameters.ClientId
					},
					{
						"app_id",
						this.SteamManager.SteamAppId
					},
					{
						"session_ticket",
						this.SteamManager.SessionTicket
					}
				};
				Hashtable logParams = hashtable.Clone() as Hashtable;
				logParams.Add("url", text);
				AsmoLogger.Info("OAuthGate.Steam.sender", "Requesting an access token by Steam partner", logParams);
				hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
				this._privateRequestSender.SendRequest(text, hashtable, delegate(OAuthError error)
				{
					if (error == null)
					{
						AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._privateRequestSender.response.DataAsText);
						authenticationTokens.InitExpiration();
						if (authenticationTokens.HasPrivateToken)
						{
							this._PrivateAuthenticationTokens = authenticationTokens;
							this._WriteRefreshTokenToDisk();
							logParams.Add("expires_in", authenticationTokens.expires_in);
							logParams.Add("scope", authenticationTokens.scope);
							AsmoLogger.Info("OAuthGate.Steam.receiver", "Authentication successful", logParams);
							this._AuthenticationPostProcess();
							return;
						}
						if (authenticationTokens.HasPublicToken)
						{
							AsmoLogger.Info("OAuthGate.Steam.receiver", "Authentication partial: only has public token", logParams);
						}
					}
					if (error != null)
					{
						logParams.Add("error", Reflection.HashtableFromObject(error, null, 30U));
					}
					if (silentFailure)
					{
						AsmoLogger.Warning("OAuthGate.Steam.receiver", "Silently failed to find a private access token", logParams);
						OAuthError authError2 = OAuthError.MakeSilentAuthError();
						this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, authError2);
						return;
					}
					AsmoLogger.Info("OAuthGate.Steam.receiver", "Falling back on traditional SSO flow", logParams);
					this._RequireNonNullSSOHandler();
					this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
				});
				return;
			}
			if (silentFailure)
			{
				AsmoLogger.Warning("OAuthGate.Steam", "Silently failed to find a private access token (no Steam client)", null);
				OAuthError authError = OAuthError.MakeSilentAuthError();
				this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, authError);
				return;
			}
			AsmoLogger.Info("OAuthGate.Steam", "Falling back on traditional SSO flow (no Steam client)", null);
			this._RequireNonNullSSOHandler();
			this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x0012D2C5 File Offset: 0x0012B4C5
		private void _AuthenticationPostProcess()
		{
			this.UpdateUserDetails(delegate
			{
				if (this.SteamManager != null && this.SteamManager.HasClient && this.UserDetails != null && this.UserDetails.Partners.FirstOrDefault((PartnerAccount x) => x.Equals(this.SteamManager.Me)) == null)
				{
					this.SteamManager.LinkSteamAccount(this, delegate
					{
						this.UpdateUserDetails(delegate
						{
							this._privateRequestSender.Reset();
							this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, null);
						}, true);
					});
					return;
				}
				this._privateRequestSender.Reset();
				this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, null);
			}, true);
		}

		// Token: 0x0400271E RID: 10014
		public const string kDebugModuleName = "OAuthGate";

		// Token: 0x0400271F RID: 10015
		public const string kNoPrivateTokenError = "no_private_token";

		// Token: 0x04002720 RID: 10016
		public const string kAbortError = "aborted_by_user";

		// Token: 0x04002721 RID: 10017
		private const string _kRefreshTokenKeyNameInPlayerPref = "RefreshToken";

		// Token: 0x04002722 RID: 10018
		private const string _kUpdateEmailPopUpDisplayDate = "UpdateEmailPopUpDisplayDate";

		// Token: 0x04002724 RID: 10020
		protected OAuthGate.RequestSender _publicRequestSender;

		// Token: 0x04002725 RID: 10021
		protected OAuthGate.RequestSender _privateRequestSender;

		// Token: 0x04002726 RID: 10022
		private static int nextCallbackID;

		// Token: 0x04002727 RID: 10023
		public Dictionary<int, OAuthCallback> _callbacksForPrivateToken = new Dictionary<int, OAuthCallback>();

		// Token: 0x04002728 RID: 10024
		public Dictionary<int, OAuthCallback> _callbacksForPublicToken = new Dictionary<int, OAuthCallback>();

		// Token: 0x04002729 RID: 10025
		private Hashtable _logParamsForPrivateToken;

		// Token: 0x0400272D RID: 10029
		private bool isFlushingCallbacks;

		// Token: 0x0400272E RID: 10030
		private SSOHandler _ssoHandler;

		// Token: 0x02000996 RID: 2454
		protected class RequestSender
		{
			// Token: 0x06004857 RID: 18519 RVA: 0x0014BB2E File Offset: 0x00149D2E
			public RequestSender(NetworkParameters networkParameters)
			{
				this._restAPIPinPublicKeys = networkParameters.RestAPIPinPublicKeys;
			}

			// Token: 0x06004858 RID: 18520 RVA: 0x0014BB42 File Offset: 0x00149D42
			public void Reset()
			{
				if (this.request != null && this.request.State < HTTPRequestStates.Finished)
				{
					this.request.Abort();
				}
				this.request = null;
				this.response = null;
			}

			// Token: 0x06004859 RID: 18521 RVA: 0x0014BB74 File Offset: 0x00149D74
			public virtual void SendRequest(string url, Hashtable parameters, OAuthCallback onComplete)
			{
				if (onComplete == null)
				{
					throw new ArgumentNullException("onComplete");
				}
				HTTPMethods methodType = HTTPMethods.Post;
				Uri uri = new Uri(url);
				this.response = null;
				this.request = new HTTPRequest(uri, methodType, delegate(HTTPRequest req, HTTPResponse resp)
				{
					if (req.State == HTTPRequestStates.Aborted)
					{
						return;
					}
					if (resp == null)
					{
						OAuthError authError;
						if (req.State == HTTPRequestStates.TimedOut)
						{
							authError = OAuthError.MakeTimeoutError();
						}
						else
						{
							CertificateVerifier certificateVerifier = req.CustomCertificateVerifyer as CertificateVerifier;
							if (certificateVerifier != null && !certificateVerifier.isValid)
							{
								authError = OAuthError.MakePublicKeyPinningError();
							}
							else
							{
								authError = OAuthError.MakeNoResponseError();
							}
						}
						onComplete(authError);
						return;
					}
					OAuthError oauthError = JsonUtility.FromJson<OAuthError>(resp.DataAsText);
					if (oauthError != null && !string.IsNullOrEmpty(oauthError.error))
					{
						onComplete(oauthError);
						return;
					}
					this.response = resp;
					onComplete(null);
				});
				this.request.AddHeader("User-Agent", CoreApplication.GetUserAgent());
				this.request.AddHeader("Cache-Control", "no-cache");
				this.request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
				foreach (object obj in parameters.Keys)
				{
					string text = (string)obj;
					this.request.AddField(text, parameters[text].ToString());
				}
				this.request.CustomCertificateVerifyer = new CertificateVerifier(this._restAPIPinPublicKeys);
				this.request.UseAlternateSSL = true;
				this.request.Send();
			}

			// Token: 0x04003286 RID: 12934
			public HTTPRequest request;

			// Token: 0x04003287 RID: 12935
			public HTTPResponse response;

			// Token: 0x04003288 RID: 12936
			private string[] _restAPIPinPublicKeys;
		}
	}
}
