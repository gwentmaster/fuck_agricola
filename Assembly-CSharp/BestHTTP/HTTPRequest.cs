using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Forms;
using BestHTTP.Logger;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x02000577 RID: 1399
	public sealed class HTTPRequest : IEnumerator, IEnumerator<HTTPRequest>, IDisposable
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x00101EC1 File Offset: 0x001000C1
		// (set) Token: 0x060032AE RID: 12974 RVA: 0x00101EC9 File Offset: 0x001000C9
		public Uri Uri { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x00101ED2 File Offset: 0x001000D2
		// (set) Token: 0x060032B0 RID: 12976 RVA: 0x00101EDA File Offset: 0x001000DA
		public HTTPMethods MethodType { get; set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x00101EE3 File Offset: 0x001000E3
		// (set) Token: 0x060032B2 RID: 12978 RVA: 0x00101EEB File Offset: 0x001000EB
		public byte[] RawData { get; set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x00101EF4 File Offset: 0x001000F4
		// (set) Token: 0x060032B4 RID: 12980 RVA: 0x00101EFC File Offset: 0x001000FC
		public Stream UploadStream { get; set; }

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x00101F05 File Offset: 0x00100105
		// (set) Token: 0x060032B6 RID: 12982 RVA: 0x00101F0D File Offset: 0x0010010D
		public bool DisposeUploadStream { get; set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x00101F16 File Offset: 0x00100116
		// (set) Token: 0x060032B8 RID: 12984 RVA: 0x00101F1E File Offset: 0x0010011E
		public bool UseUploadStreamLength { get; set; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x00101F27 File Offset: 0x00100127
		// (set) Token: 0x060032BA RID: 12986 RVA: 0x00101F2F File Offset: 0x0010012F
		public bool IsKeepAlive
		{
			get
			{
				return this.isKeepAlive;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the IsKeepAlive property while processing the request is not supported.");
				}
				this.isKeepAlive = value;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x00101F4C File Offset: 0x0010014C
		// (set) Token: 0x060032BC RID: 12988 RVA: 0x00101F54 File Offset: 0x00100154
		public bool DisableCache
		{
			get
			{
				return this.disableCache;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the DisableCache property while processing the request is not supported.");
				}
				this.disableCache = value;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x00101F71 File Offset: 0x00100171
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x00101F79 File Offset: 0x00100179
		public bool CacheOnly
		{
			get
			{
				return this.cacheOnly;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the CacheOnly property while processing the request is not supported.");
				}
				this.cacheOnly = value;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x00101F96 File Offset: 0x00100196
		// (set) Token: 0x060032C0 RID: 12992 RVA: 0x00101F9E File Offset: 0x0010019E
		public bool UseStreaming
		{
			get
			{
				return this.useStreaming;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the UseStreaming property while processing the request is not supported.");
				}
				this.useStreaming = value;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x00101FBB File Offset: 0x001001BB
		// (set) Token: 0x060032C2 RID: 12994 RVA: 0x00101FC3 File Offset: 0x001001C3
		public int StreamFragmentSize
		{
			get
			{
				return this.streamFragmentSize;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the StreamFragmentSize property while processing the request is not supported.");
				}
				if (value < 1)
				{
					throw new ArgumentException("StreamFragmentSize must be at least 1.");
				}
				this.streamFragmentSize = value;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x00101FEF File Offset: 0x001001EF
		// (set) Token: 0x060032C4 RID: 12996 RVA: 0x00101FF7 File Offset: 0x001001F7
		public OnRequestFinishedDelegate Callback { get; set; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x00102000 File Offset: 0x00100200
		// (set) Token: 0x060032C6 RID: 12998 RVA: 0x00102008 File Offset: 0x00100208
		public bool DisableRetry { get; set; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x00102011 File Offset: 0x00100211
		// (set) Token: 0x060032C8 RID: 13000 RVA: 0x00102019 File Offset: 0x00100219
		public bool IsRedirected { get; internal set; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x00102022 File Offset: 0x00100222
		// (set) Token: 0x060032CA RID: 13002 RVA: 0x0010202A File Offset: 0x0010022A
		public Uri RedirectUri { get; internal set; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x00102033 File Offset: 0x00100233
		public Uri CurrentUri
		{
			get
			{
				if (!this.IsRedirected)
				{
					return this.Uri;
				}
				return this.RedirectUri;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x0010204A File Offset: 0x0010024A
		// (set) Token: 0x060032CD RID: 13005 RVA: 0x00102052 File Offset: 0x00100252
		public HTTPResponse Response { get; internal set; }

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x0010205B File Offset: 0x0010025B
		// (set) Token: 0x060032CF RID: 13007 RVA: 0x00102063 File Offset: 0x00100263
		public HTTPResponse ProxyResponse { get; internal set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x0010206C File Offset: 0x0010026C
		// (set) Token: 0x060032D1 RID: 13009 RVA: 0x00102074 File Offset: 0x00100274
		public Exception Exception { get; internal set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x0010207D File Offset: 0x0010027D
		// (set) Token: 0x060032D3 RID: 13011 RVA: 0x00102085 File Offset: 0x00100285
		public object Tag { get; set; }

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x0010208E File Offset: 0x0010028E
		// (set) Token: 0x060032D5 RID: 13013 RVA: 0x00102096 File Offset: 0x00100296
		public Credentials Credentials { get; set; }

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x0010209F File Offset: 0x0010029F
		public bool HasProxy
		{
			get
			{
				return this.Proxy != null;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x001020AA File Offset: 0x001002AA
		// (set) Token: 0x060032D8 RID: 13016 RVA: 0x001020B2 File Offset: 0x001002B2
		public HTTPProxy Proxy { get; set; }

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x001020BB File Offset: 0x001002BB
		// (set) Token: 0x060032DA RID: 13018 RVA: 0x001020C3 File Offset: 0x001002C3
		public int MaxRedirects { get; set; }

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x001020CC File Offset: 0x001002CC
		// (set) Token: 0x060032DC RID: 13020 RVA: 0x001020D4 File Offset: 0x001002D4
		public bool UseAlternateSSL { get; set; }

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x001020DD File Offset: 0x001002DD
		// (set) Token: 0x060032DE RID: 13022 RVA: 0x001020E5 File Offset: 0x001002E5
		public bool IsCookiesEnabled { get; set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x001020EE File Offset: 0x001002EE
		// (set) Token: 0x060032E0 RID: 13024 RVA: 0x00102109 File Offset: 0x00100309
		public List<Cookie> Cookies
		{
			get
			{
				if (this.customCookies == null)
				{
					this.customCookies = new List<Cookie>();
				}
				return this.customCookies;
			}
			set
			{
				this.customCookies = value;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x00102112 File Offset: 0x00100312
		// (set) Token: 0x060032E2 RID: 13026 RVA: 0x0010211A File Offset: 0x0010031A
		public HTTPFormUsage FormUsage { get; set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00102123 File Offset: 0x00100323
		// (set) Token: 0x060032E4 RID: 13028 RVA: 0x0010212B File Offset: 0x0010032B
		public HTTPRequestStates State { get; internal set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x00102134 File Offset: 0x00100334
		// (set) Token: 0x060032E6 RID: 13030 RVA: 0x0010213C File Offset: 0x0010033C
		public int RedirectCount { get; internal set; }

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060032E7 RID: 13031 RVA: 0x00102148 File Offset: 0x00100348
		// (remove) Token: 0x060032E8 RID: 13032 RVA: 0x00102180 File Offset: 0x00100380
		public event Func<HTTPRequest, X509Certificate, X509Chain, bool> CustomCertificationValidator;

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x001021B5 File Offset: 0x001003B5
		// (set) Token: 0x060032EA RID: 13034 RVA: 0x001021BD File Offset: 0x001003BD
		public TimeSpan ConnectTimeout { get; set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x001021C6 File Offset: 0x001003C6
		// (set) Token: 0x060032EC RID: 13036 RVA: 0x001021CE File Offset: 0x001003CE
		public TimeSpan Timeout { get; set; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x001021D7 File Offset: 0x001003D7
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x001021DF File Offset: 0x001003DF
		public bool EnableTimoutForStreaming { get; set; }

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x001021E8 File Offset: 0x001003E8
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x001021F0 File Offset: 0x001003F0
		public bool EnableSafeReadOnUnknownContentLength { get; set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x001021F9 File Offset: 0x001003F9
		// (set) Token: 0x060032F2 RID: 13042 RVA: 0x00102201 File Offset: 0x00100401
		public int Priority { get; set; }

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x0010220A File Offset: 0x0010040A
		// (set) Token: 0x060032F4 RID: 13044 RVA: 0x00102212 File Offset: 0x00100412
		public ICertificateVerifyer CustomCertificateVerifyer { get; set; }

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060032F5 RID: 13045 RVA: 0x0010221B File Offset: 0x0010041B
		// (set) Token: 0x060032F6 RID: 13046 RVA: 0x00102223 File Offset: 0x00100423
		public IClientCredentialsProvider CustomClientCredentialsProvider { get; set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x0010222C File Offset: 0x0010042C
		// (set) Token: 0x060032F8 RID: 13048 RVA: 0x00102234 File Offset: 0x00100434
		public SupportedProtocols ProtocolHandler { get; set; }

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060032F9 RID: 13049 RVA: 0x0010223D File Offset: 0x0010043D
		// (remove) Token: 0x060032FA RID: 13050 RVA: 0x00102256 File Offset: 0x00100456
		public event OnBeforeRedirectionDelegate OnBeforeRedirection
		{
			add
			{
				this.onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Combine(this.onBeforeRedirection, value);
			}
			remove
			{
				this.onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Remove(this.onBeforeRedirection, value);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060032FB RID: 13051 RVA: 0x0010226F File Offset: 0x0010046F
		// (remove) Token: 0x060032FC RID: 13052 RVA: 0x00102288 File Offset: 0x00100488
		public event OnBeforeHeaderSendDelegate OnBeforeHeaderSend
		{
			add
			{
				this._onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Combine(this._onBeforeHeaderSend, value);
			}
			remove
			{
				this._onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Remove(this._onBeforeHeaderSend, value);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x001022A1 File Offset: 0x001004A1
		// (set) Token: 0x060032FE RID: 13054 RVA: 0x001022A9 File Offset: 0x001004A9
		public bool TryToMinimizeTCPLatency { get; set; }

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x001022B2 File Offset: 0x001004B2
		// (set) Token: 0x06003300 RID: 13056 RVA: 0x001022BA File Offset: 0x001004BA
		internal long Downloaded { get; set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x001022C3 File Offset: 0x001004C3
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x001022CB File Offset: 0x001004CB
		internal long DownloadLength { get; set; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x001022D4 File Offset: 0x001004D4
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x001022DC File Offset: 0x001004DC
		internal bool DownloadProgressChanged { get; set; }

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x001022E8 File Offset: 0x001004E8
		internal long UploadStreamLength
		{
			get
			{
				if (this.UploadStream == null || !this.UseUploadStreamLength)
				{
					return -1L;
				}
				long result;
				try
				{
					result = this.UploadStream.Length;
				}
				catch
				{
					result = -1L;
				}
				return result;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06003306 RID: 13062 RVA: 0x00102330 File Offset: 0x00100530
		// (set) Token: 0x06003307 RID: 13063 RVA: 0x00102338 File Offset: 0x00100538
		internal long Uploaded { get; set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x00102341 File Offset: 0x00100541
		// (set) Token: 0x06003309 RID: 13065 RVA: 0x00102349 File Offset: 0x00100549
		internal long UploadLength { get; set; }

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x00102352 File Offset: 0x00100552
		// (set) Token: 0x0600330B RID: 13067 RVA: 0x0010235A File Offset: 0x0010055A
		internal bool UploadProgressChanged { get; set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x00102363 File Offset: 0x00100563
		// (set) Token: 0x0600330D RID: 13069 RVA: 0x0010236B File Offset: 0x0010056B
		private Dictionary<string, List<string>> Headers { get; set; }

		// Token: 0x0600330E RID: 13070 RVA: 0x00102374 File Offset: 0x00100574
		public HTTPRequest(Uri uri) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, null)
		{
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00102389 File Offset: 0x00100589
		public HTTPRequest(Uri uri, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, callback)
		{
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x0010239E File Offset: 0x0010059E
		public HTTPRequest(Uri uri, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, HTTPManager.IsCachingDisabled, callback)
		{
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001023AF File Offset: 0x001005AF
		public HTTPRequest(Uri uri, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, disableCache, callback)
		{
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x001023BD File Offset: 0x001005BD
		public HTTPRequest(Uri uri, HTTPMethods methodType) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, null)
		{
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x001023DB File Offset: 0x001005DB
		public HTTPRequest(Uri uri, HTTPMethods methodType, OnRequestFinishedDelegate callback) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, callback)
		{
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x001023F9 File Offset: 0x001005F9
		public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, methodType, isKeepAlive, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, callback)
		{
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x00102414 File Offset: 0x00100614
		public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
		{
			this.Uri = uri;
			this.MethodType = methodType;
			this.IsKeepAlive = isKeepAlive;
			this.DisableCache = disableCache;
			this.Callback = callback;
			this.StreamFragmentSize = 4096;
			this.DisableRetry = (methodType > HTTPMethods.Get);
			this.MaxRedirects = int.MaxValue;
			this.RedirectCount = 0;
			this.IsCookiesEnabled = HTTPManager.IsCookiesEnabled;
			this.Downloaded = (this.DownloadLength = 0L);
			this.DownloadProgressChanged = false;
			this.State = HTTPRequestStates.Initial;
			this.ConnectTimeout = HTTPManager.ConnectTimeout;
			this.Timeout = HTTPManager.RequestTimeout;
			this.EnableTimoutForStreaming = false;
			this.EnableSafeReadOnUnknownContentLength = true;
			this.Proxy = HTTPManager.Proxy;
			this.UseUploadStreamLength = true;
			this.DisposeUploadStream = true;
			this.CustomCertificateVerifyer = HTTPManager.DefaultCertificateVerifyer;
			this.CustomClientCredentialsProvider = HTTPManager.DefaultClientCredentialsProvider;
			this.UseAlternateSSL = HTTPManager.UseAlternateSSLDefaultValue;
			this.CustomCertificationValidator += HTTPManager.DefaultCertificationValidator;
			this.TryToMinimizeTCPLatency = HTTPManager.TryToMinimizeTCPLatency;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x00102511 File Offset: 0x00100711
		public void AddField(string fieldName, string value)
		{
			this.AddField(fieldName, value, Encoding.UTF8);
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x00102520 File Offset: 0x00100720
		public void AddField(string fieldName, string value, Encoding e)
		{
			if (this.FieldCollector == null)
			{
				this.FieldCollector = new HTTPFormBase();
			}
			this.FieldCollector.AddField(fieldName, value, e);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00102543 File Offset: 0x00100743
		public void AddBinaryData(string fieldName, byte[] content)
		{
			this.AddBinaryData(fieldName, content, null, null);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x0010254F File Offset: 0x0010074F
		public void AddBinaryData(string fieldName, byte[] content, string fileName)
		{
			this.AddBinaryData(fieldName, content, fileName, null);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x0010255B File Offset: 0x0010075B
		public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
		{
			if (this.FieldCollector == null)
			{
				this.FieldCollector = new HTTPFormBase();
			}
			this.FieldCollector.AddBinaryData(fieldName, content, fileName, mimeType);
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00102580 File Offset: 0x00100780
		public void SetFields(WWWForm wwwForm)
		{
			this.FormUsage = HTTPFormUsage.Unity;
			this.FormImpl = new UnityForm(wwwForm);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00102595 File Offset: 0x00100795
		public void SetForm(HTTPFormBase form)
		{
			this.FormImpl = form;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x0010259E File Offset: 0x0010079E
		public void ClearForm()
		{
			this.FormImpl = null;
			this.FieldCollector = null;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x001025B0 File Offset: 0x001007B0
		private HTTPFormBase SelectFormImplementation()
		{
			if (this.FormImpl != null)
			{
				return this.FormImpl;
			}
			if (this.FieldCollector == null)
			{
				return null;
			}
			switch (this.FormUsage)
			{
			case HTTPFormUsage.Automatic:
				if (this.FieldCollector.HasBinary || this.FieldCollector.HasLongValue)
				{
					goto IL_63;
				}
				break;
			case HTTPFormUsage.UrlEncoded:
				break;
			case HTTPFormUsage.Multipart:
				goto IL_63;
			case HTTPFormUsage.RawJSon:
				this.FormImpl = new RawJsonForm();
				goto IL_88;
			case HTTPFormUsage.Unity:
				this.FormImpl = new UnityForm();
				goto IL_88;
			default:
				goto IL_88;
			}
			this.FormImpl = new HTTPUrlEncodedForm();
			goto IL_88;
			IL_63:
			this.FormImpl = new HTTPMultiPartForm();
			IL_88:
			this.FormImpl.CopyFrom(this.FieldCollector);
			return this.FormImpl;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0010265C File Offset: 0x0010085C
		public void AddHeader(string name, string value)
		{
			if (this.Headers == null)
			{
				this.Headers = new Dictionary<string, List<string>>();
			}
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list))
			{
				this.Headers.Add(name, list = new List<string>(1));
			}
			list.Add(value);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x001026A8 File Offset: 0x001008A8
		public void SetHeader(string name, string value)
		{
			if (this.Headers == null)
			{
				this.Headers = new Dictionary<string, List<string>>();
			}
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list))
			{
				this.Headers.Add(name, list = new List<string>(1));
			}
			list.Clear();
			list.Add(value);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x001026F9 File Offset: 0x001008F9
		public bool RemoveHeader(string name)
		{
			return this.Headers != null && this.Headers.Remove(name);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00102711 File Offset: 0x00100911
		public bool HasHeader(string name)
		{
			return this.Headers != null && this.Headers.ContainsKey(name);
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x0010272C File Offset: 0x0010092C
		public string GetFirstHeaderValue(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			List<string> list = null;
			if (this.Headers.TryGetValue(name, out list) && list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x00102768 File Offset: 0x00100968
		public List<string> GetHeaderValues(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			List<string> list = null;
			if (this.Headers.TryGetValue(name, out list) && list.Count > 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x0010279D File Offset: 0x0010099D
		public void RemoveHeaders()
		{
			if (this.Headers == null)
			{
				return;
			}
			this.Headers.Clear();
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x001027B3 File Offset: 0x001009B3
		public void SetRangeHeader(int firstBytePos)
		{
			this.SetHeader("Range", string.Format("bytes={0}-", firstBytePos));
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x001027D0 File Offset: 0x001009D0
		public void SetRangeHeader(int firstBytePos, int lastBytePos)
		{
			this.SetHeader("Range", string.Format("bytes={0}-{1}", firstBytePos, lastBytePos));
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x001027F3 File Offset: 0x001009F3
		public void EnumerateHeaders(OnHeaderEnumerationDelegate callback)
		{
			this.EnumerateHeaders(callback, false);
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00102800 File Offset: 0x00100A00
		public void EnumerateHeaders(OnHeaderEnumerationDelegate callback, bool callBeforeSendCallback)
		{
			if (!this.HasHeader("Host"))
			{
				this.SetHeader("Host", this.CurrentUri.Authority);
			}
			if (this.IsRedirected && !this.HasHeader("Referer"))
			{
				this.AddHeader("Referer", this.Uri.ToString());
			}
			if (!this.HasHeader("Accept-Encoding"))
			{
				this.AddHeader("Accept-Encoding", "gzip, identity");
			}
			if (this.HasProxy && !this.HasHeader("Proxy-Connection"))
			{
				this.AddHeader("Proxy-Connection", this.IsKeepAlive ? "Keep-Alive" : "Close");
			}
			if (!this.HasHeader("Connection"))
			{
				this.AddHeader("Connection", this.IsKeepAlive ? "Keep-Alive, TE" : "Close, TE");
			}
			if (!this.HasHeader("TE"))
			{
				this.AddHeader("TE", "identity");
			}
			if (!this.HasHeader("User-Agent"))
			{
				this.AddHeader("User-Agent", "BestHTTP");
			}
			long num;
			if (this.UploadStream == null)
			{
				byte[] entityBody = this.GetEntityBody();
				num = (long)((entityBody != null) ? entityBody.Length : 0);
				if (this.RawData == null && (this.FormImpl != null || (this.FieldCollector != null && !this.FieldCollector.IsEmpty)))
				{
					this.SelectFormImplementation();
					if (this.FormImpl != null)
					{
						this.FormImpl.PrepareRequest(this);
					}
				}
			}
			else
			{
				num = this.UploadStreamLength;
				if (num == -1L)
				{
					this.SetHeader("Transfer-Encoding", "Chunked");
				}
				if (!this.HasHeader("Content-Type"))
				{
					this.SetHeader("Content-Type", "application/octet-stream");
				}
			}
			if (num > 0L && !this.HasHeader("Content-Length"))
			{
				this.SetHeader("Content-Length", num.ToString());
			}
			if (this.HasProxy && this.Proxy.Credentials != null)
			{
				switch (this.Proxy.Credentials.Type)
				{
				case AuthenticationTypes.Unknown:
				case AuthenticationTypes.Digest:
				{
					Digest digest = DigestStore.Get(this.Proxy.Address);
					if (digest != null)
					{
						string value = digest.GenerateResponseHeader(this, this.Proxy.Credentials);
						if (!string.IsNullOrEmpty(value))
						{
							this.SetHeader("Proxy-Authorization", value);
						}
					}
					break;
				}
				case AuthenticationTypes.Basic:
					this.SetHeader("Proxy-Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Proxy.Credentials.UserName + ":" + this.Proxy.Credentials.Password)));
					break;
				}
			}
			if (this.Credentials != null)
			{
				switch (this.Credentials.Type)
				{
				case AuthenticationTypes.Unknown:
				case AuthenticationTypes.Digest:
				{
					Digest digest2 = DigestStore.Get(this.CurrentUri);
					if (digest2 != null)
					{
						string value2 = digest2.GenerateResponseHeader(this, this.Credentials);
						if (!string.IsNullOrEmpty(value2))
						{
							this.SetHeader("Authorization", value2);
						}
					}
					break;
				}
				case AuthenticationTypes.Basic:
					this.SetHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Credentials.UserName + ":" + this.Credentials.Password)));
					break;
				}
			}
			List<Cookie> list = this.IsCookiesEnabled ? CookieJar.Get(this.CurrentUri) : null;
			if (list == null || list.Count == 0)
			{
				list = this.customCookies;
			}
			else if (this.customCookies != null)
			{
				for (int i = 0; i < this.customCookies.Count; i++)
				{
					Cookie customCookie = this.customCookies[i];
					int num2 = list.FindIndex((Cookie c) => c.Name.Equals(customCookie.Name));
					if (num2 >= 0)
					{
						list[num2] = customCookie;
					}
					else
					{
						list.Add(customCookie);
					}
				}
			}
			if (list != null && list.Count > 0)
			{
				bool flag = true;
				string text = string.Empty;
				bool flag2 = HTTPProtocolFactory.IsSecureProtocol(this.CurrentUri);
				foreach (Cookie cookie in list)
				{
					if (!cookie.IsSecure || (cookie.IsSecure && flag2))
					{
						if (!flag)
						{
							text += "; ";
						}
						else
						{
							flag = false;
						}
						text += cookie.ToString();
						cookie.LastAccess = DateTime.UtcNow;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.SetHeader("Cookie", text);
				}
			}
			if (callBeforeSendCallback && this._onBeforeHeaderSend != null)
			{
				try
				{
					this._onBeforeHeaderSend(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HTTPRequest", "OnBeforeHeaderSend", ex);
				}
			}
			if (callback != null && this.Headers != null)
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair in this.Headers)
				{
					callback(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00102D48 File Offset: 0x00100F48
		private void SendHeaders(Stream stream)
		{
			this.EnumerateHeaders(delegate(string header, List<string> values)
			{
				if (string.IsNullOrEmpty(header) || values == null)
				{
					return;
				}
				byte[] asciibytes = (header + ": ").GetASCIIBytes();
				for (int i = 0; i < values.Count; i++)
				{
					if (string.IsNullOrEmpty(values[i]))
					{
						HTTPManager.Logger.Warning("HTTPRequest", string.Format("Null/empty value for header: {0}", header));
					}
					else
					{
						stream.WriteArray(asciibytes);
						stream.WriteArray(values[i].GetASCIIBytes());
						stream.WriteArray(HTTPRequest.EOL);
					}
				}
			}, true);
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00102D78 File Offset: 0x00100F78
		public string DumpHeaders()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SendHeaders(memoryStream);
				result = memoryStream.ToArray().AsciiToString();
			}
			return result;
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x00102DBC File Offset: 0x00100FBC
		internal byte[] GetEntityBody()
		{
			if (this.RawData != null)
			{
				return this.RawData;
			}
			if (this.FormImpl != null || (this.FieldCollector != null && !this.FieldCollector.IsEmpty))
			{
				this.SelectFormImplementation();
				if (this.FormImpl != null)
				{
					return this.FormImpl.GetData();
				}
			}
			return null;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00102E14 File Offset: 0x00101014
		internal void SendOutTo(Stream stream)
		{
			try
			{
				string arg = (this.HasProxy && this.Proxy.SendWholeUri) ? this.CurrentUri.OriginalString : this.CurrentUri.GetRequestPathAndQueryURL();
				string text = string.Format("{0} {1} HTTP/1.1", HTTPRequest.MethodNames[(int)this.MethodType], arg);
				if (HTTPManager.Logger.Level <= Loglevels.Information)
				{
					HTTPManager.Logger.Information("HTTPRequest", string.Format("Sending request: '{0}'", text));
				}
				stream.WriteArray(text.GetASCIIBytes());
				stream.WriteArray(HTTPRequest.EOL);
				this.SendHeaders(stream);
				stream.WriteArray(HTTPRequest.EOL);
				if (this.UploadStream != null)
				{
					stream.Flush();
				}
				byte[] array = this.RawData;
				if (array == null && this.FormImpl != null)
				{
					array = this.FormImpl.GetData();
				}
				if (array != null || this.UploadStream != null)
				{
					Stream stream2 = this.UploadStream;
					if (stream2 == null)
					{
						stream2 = new MemoryStream(array, 0, array.Length);
						this.UploadLength = (long)array.Length;
					}
					else
					{
						this.UploadLength = (this.UseUploadStreamLength ? this.UploadStreamLength : -1L);
					}
					this.Uploaded = 0L;
					byte[] array2 = new byte[HTTPRequest.UploadChunkSize];
					int num;
					while ((num = stream2.Read(array2, 0, array2.Length)) > 0)
					{
						if (!this.UseUploadStreamLength)
						{
							stream.WriteArray(num.ToString("X").GetASCIIBytes());
							stream.WriteArray(HTTPRequest.EOL);
						}
						stream.Write(array2, 0, num);
						if (!this.UseUploadStreamLength)
						{
							stream.WriteArray(HTTPRequest.EOL);
						}
						stream.Flush();
						this.Uploaded += (long)num;
						this.UploadProgressChanged = true;
					}
					if (!this.UseUploadStreamLength)
					{
						stream.WriteArray("0".GetASCIIBytes());
						stream.WriteArray(HTTPRequest.EOL);
						stream.WriteArray(HTTPRequest.EOL);
					}
					stream.Flush();
					if (this.UploadStream == null && stream2 != null)
					{
						stream2.Dispose();
					}
				}
				else
				{
					stream.Flush();
				}
				HTTPManager.Logger.Information("HTTPRequest", "'" + text + "' sent out");
			}
			finally
			{
				if (this.UploadStream != null && this.DisposeUploadStream)
				{
					this.UploadStream.Dispose();
				}
			}
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00103064 File Offset: 0x00101264
		internal void UpgradeCallback()
		{
			if (this.Response == null || !this.Response.IsUpgraded)
			{
				return;
			}
			try
			{
				if (this.OnUpgraded != null)
				{
					this.OnUpgraded(this, this.Response);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPRequest", "UpgradeCallback", ex);
			}
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x001030CC File Offset: 0x001012CC
		internal void CallCallback()
		{
			try
			{
				if (this.Callback != null)
				{
					this.Callback(this, this.Response);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPRequest", "CallCallback", ex);
			}
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x00103120 File Offset: 0x00101320
		internal bool CallOnBeforeRedirection(Uri redirectUri)
		{
			return this.onBeforeRedirection == null || this.onBeforeRedirection(this, this.Response, redirectUri);
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x0010313F File Offset: 0x0010133F
		internal void FinishStreaming()
		{
			if (this.Response != null && this.UseStreaming)
			{
				this.Response.FinishStreaming();
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x0010315C File Offset: 0x0010135C
		internal void Prepare()
		{
			if (this.FormUsage == HTTPFormUsage.Unity)
			{
				this.SelectFormImplementation();
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x0010316E File Offset: 0x0010136E
		internal bool CallCustomCertificationValidator(X509Certificate cert, X509Chain chain)
		{
			return this.CustomCertificationValidator == null || this.CustomCertificationValidator(this, cert, chain);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x00103188 File Offset: 0x00101388
		public HTTPRequest Send()
		{
			return HTTPManager.SendRequest(this);
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x00103190 File Offset: 0x00101390
		public void Abort()
		{
			if (Monitor.TryEnter(HTTPManager.Locker, TimeSpan.FromMilliseconds(100.0)))
			{
				try
				{
					if (this.State >= HTTPRequestStates.Finished)
					{
						HTTPManager.Logger.Warning("HTTPRequest", string.Format("Abort - Already in a state({0}) where no Abort required!", this.State.ToString()));
						return;
					}
					ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this);
					if (connectionWith == null)
					{
						if (!HTTPManager.RemoveFromQueue(this))
						{
							HTTPManager.Logger.Warning("HTTPRequest", "Abort - No active connection found with this request! (The request may already finished?)");
						}
						this.State = HTTPRequestStates.Aborted;
						this.CallCallback();
						return;
					}
					if (this.Response != null && this.Response.IsStreamed)
					{
						this.Response.Dispose();
					}
					connectionWith.Abort(HTTPConnectionStates.AbortRequested);
					return;
				}
				finally
				{
					Monitor.Exit(HTTPManager.Locker);
				}
			}
			throw new Exception("Wasn't able to acquire a thread lock. Abort failed!");
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x00103278 File Offset: 0x00101478
		public void Clear()
		{
			this.ClearForm();
			this.RemoveHeaders();
			this.IsRedirected = false;
			this.RedirectCount = 0;
			this.Downloaded = (this.DownloadLength = 0L);
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x0000301F File Offset: 0x0000121F
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x001032B0 File Offset: 0x001014B0
		public bool MoveNext()
		{
			return this.State < HTTPRequestStates.Finished;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x00003A58 File Offset: 0x00001C58
		public void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x00035D67 File Offset: 0x00033F67
		HTTPRequest IEnumerator<HTTPRequest>.Current
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x00003022 File Offset: 0x00001222
		public void Dispose()
		{
		}

		// Token: 0x0400216A RID: 8554
		public static readonly byte[] EOL = new byte[]
		{
			13,
			10
		};

		// Token: 0x0400216B RID: 8555
		public static readonly string[] MethodNames = new string[]
		{
			HTTPMethods.Get.ToString().ToUpper(),
			HTTPMethods.Head.ToString().ToUpper(),
			HTTPMethods.Post.ToString().ToUpper(),
			HTTPMethods.Put.ToString().ToUpper(),
			HTTPMethods.Delete.ToString().ToUpper(),
			HTTPMethods.Patch.ToString().ToUpper(),
			HTTPMethods.Merge.ToString().ToUpper(),
			HTTPMethods.Options.ToString().ToUpper()
		};

		// Token: 0x0400216C RID: 8556
		public static int UploadChunkSize = 2048;

		// Token: 0x04002173 RID: 8563
		public OnUploadProgressDelegate OnUploadProgress;

		// Token: 0x04002175 RID: 8565
		public OnDownloadProgressDelegate OnProgress;

		// Token: 0x04002176 RID: 8566
		public OnRequestFinishedDelegate OnUpgraded;

		// Token: 0x04002183 RID: 8579
		private List<Cookie> customCookies;

		// Token: 0x04002190 RID: 8592
		private OnBeforeRedirectionDelegate onBeforeRedirection;

		// Token: 0x04002191 RID: 8593
		private OnBeforeHeaderSendDelegate _onBeforeHeaderSend;

		// Token: 0x04002199 RID: 8601
		private bool isKeepAlive;

		// Token: 0x0400219A RID: 8602
		private bool disableCache;

		// Token: 0x0400219B RID: 8603
		private bool cacheOnly;

		// Token: 0x0400219C RID: 8604
		private int streamFragmentSize;

		// Token: 0x0400219D RID: 8605
		private bool useStreaming;

		// Token: 0x0400219F RID: 8607
		private HTTPFormBase FieldCollector;

		// Token: 0x040021A0 RID: 8608
		private HTTPFormBase FormImpl;
	}
}
