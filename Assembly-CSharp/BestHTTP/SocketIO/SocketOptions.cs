using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SocketIO
{
	// Token: 0x0200059D RID: 1437
	public sealed class SocketOptions
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x00109C46 File Offset: 0x00107E46
		// (set) Token: 0x060034EB RID: 13547 RVA: 0x00109C4E File Offset: 0x00107E4E
		public TransportTypes ConnectWith { get; set; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x00109C57 File Offset: 0x00107E57
		// (set) Token: 0x060034ED RID: 13549 RVA: 0x00109C5F File Offset: 0x00107E5F
		public bool Reconnection { get; set; }

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060034EE RID: 13550 RVA: 0x00109C68 File Offset: 0x00107E68
		// (set) Token: 0x060034EF RID: 13551 RVA: 0x00109C70 File Offset: 0x00107E70
		public int ReconnectionAttempts { get; set; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x00109C79 File Offset: 0x00107E79
		// (set) Token: 0x060034F1 RID: 13553 RVA: 0x00109C81 File Offset: 0x00107E81
		public TimeSpan ReconnectionDelay { get; set; }

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x00109C8A File Offset: 0x00107E8A
		// (set) Token: 0x060034F3 RID: 13555 RVA: 0x00109C92 File Offset: 0x00107E92
		public TimeSpan ReconnectionDelayMax { get; set; }

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060034F4 RID: 13556 RVA: 0x00109C9B File Offset: 0x00107E9B
		// (set) Token: 0x060034F5 RID: 13557 RVA: 0x00109CA3 File Offset: 0x00107EA3
		public float RandomizationFactor
		{
			get
			{
				return this.randomizationFactor;
			}
			set
			{
				this.randomizationFactor = Math.Min(1f, Math.Max(0f, value));
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060034F6 RID: 13558 RVA: 0x00109CC0 File Offset: 0x00107EC0
		// (set) Token: 0x060034F7 RID: 13559 RVA: 0x00109CC8 File Offset: 0x00107EC8
		public TimeSpan Timeout { get; set; }

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x00109CD1 File Offset: 0x00107ED1
		// (set) Token: 0x060034F9 RID: 13561 RVA: 0x00109CD9 File Offset: 0x00107ED9
		public bool AutoConnect { get; set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x00109CE2 File Offset: 0x00107EE2
		// (set) Token: 0x060034FB RID: 13563 RVA: 0x00109CEC File Offset: 0x00107EEC
		public ObservableDictionary<string, string> AdditionalQueryParams
		{
			get
			{
				return this.additionalQueryParams;
			}
			set
			{
				if (this.additionalQueryParams != null)
				{
					this.additionalQueryParams.CollectionChanged -= this.AdditionalQueryParams_CollectionChanged;
				}
				this.additionalQueryParams = value;
				this.BuiltQueryParams = null;
				if (value != null)
				{
					value.CollectionChanged += this.AdditionalQueryParams_CollectionChanged;
				}
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060034FC RID: 13564 RVA: 0x00109D3B File Offset: 0x00107F3B
		// (set) Token: 0x060034FD RID: 13565 RVA: 0x00109D43 File Offset: 0x00107F43
		public bool QueryParamsOnlyForHandshake { get; set; }

		// Token: 0x060034FE RID: 13566 RVA: 0x00109D4C File Offset: 0x00107F4C
		public SocketOptions()
		{
			this.ConnectWith = TransportTypes.Polling;
			this.Reconnection = true;
			this.ReconnectionAttempts = int.MaxValue;
			this.ReconnectionDelay = TimeSpan.FromMilliseconds(1000.0);
			this.ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000.0);
			this.RandomizationFactor = 0.5f;
			this.Timeout = TimeSpan.FromMilliseconds(20000.0);
			this.AutoConnect = true;
			this.QueryParamsOnlyForHandshake = true;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x00109DD0 File Offset: 0x00107FD0
		internal string BuildQueryParams()
		{
			if (this.AdditionalQueryParams == null || this.AdditionalQueryParams.Count == 0)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(this.BuiltQueryParams))
			{
				return this.BuiltQueryParams;
			}
			StringBuilder stringBuilder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
			foreach (KeyValuePair<string, string> keyValuePair in this.AdditionalQueryParams)
			{
				stringBuilder.Append("&");
				stringBuilder.Append(keyValuePair.Key);
				if (!string.IsNullOrEmpty(keyValuePair.Value))
				{
					stringBuilder.Append("=");
					stringBuilder.Append(keyValuePair.Value);
				}
			}
			return this.BuiltQueryParams = stringBuilder.ToString();
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00109EAC File Offset: 0x001080AC
		private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.BuiltQueryParams = null;
		}

		// Token: 0x0400229D RID: 8861
		private float randomizationFactor;

		// Token: 0x040022A0 RID: 8864
		private ObservableDictionary<string, string> additionalQueryParams;

		// Token: 0x040022A2 RID: 8866
		private string BuiltQueryParams;
	}
}
