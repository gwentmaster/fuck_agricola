using System;
using BestHTTP.Authentication;

namespace BestHTTP
{
	// Token: 0x0200056E RID: 1390
	public sealed class HTTPProxy
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x00101CFA File Offset: 0x000FFEFA
		// (set) Token: 0x0600327B RID: 12923 RVA: 0x00101D02 File Offset: 0x000FFF02
		public Uri Address { get; set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600327C RID: 12924 RVA: 0x00101D0B File Offset: 0x000FFF0B
		// (set) Token: 0x0600327D RID: 12925 RVA: 0x00101D13 File Offset: 0x000FFF13
		public Credentials Credentials { get; set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x00101D1C File Offset: 0x000FFF1C
		// (set) Token: 0x0600327F RID: 12927 RVA: 0x00101D24 File Offset: 0x000FFF24
		public bool IsTransparent { get; set; }

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06003280 RID: 12928 RVA: 0x00101D2D File Offset: 0x000FFF2D
		// (set) Token: 0x06003281 RID: 12929 RVA: 0x00101D35 File Offset: 0x000FFF35
		public bool SendWholeUri { get; set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x00101D3E File Offset: 0x000FFF3E
		// (set) Token: 0x06003283 RID: 12931 RVA: 0x00101D46 File Offset: 0x000FFF46
		public bool NonTransparentForHTTPS { get; set; }

		// Token: 0x06003284 RID: 12932 RVA: 0x00101D4F File Offset: 0x000FFF4F
		public HTTPProxy(Uri address) : this(address, null, false)
		{
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x00101D5A File Offset: 0x000FFF5A
		public HTTPProxy(Uri address, Credentials credentials) : this(address, credentials, false)
		{
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x00101D65 File Offset: 0x000FFF65
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent) : this(address, credentials, isTransparent, true)
		{
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x00101D71 File Offset: 0x000FFF71
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri) : this(address, credentials, isTransparent, true, true)
		{
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x00101D7E File Offset: 0x000FFF7E
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri, bool nonTransparentForHTTPS)
		{
			this.Address = address;
			this.Credentials = credentials;
			this.IsTransparent = isTransparent;
			this.SendWholeUri = sendWholeUri;
			this.NonTransparentForHTTPS = nonTransparentForHTTPS;
		}
	}
}
