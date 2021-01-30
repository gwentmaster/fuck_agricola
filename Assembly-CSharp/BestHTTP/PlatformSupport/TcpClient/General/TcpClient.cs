using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BestHTTP.PlatformSupport.TcpClient.General
{
	// Token: 0x020005E3 RID: 1507
	public class TcpClient : IDisposable
	{
		// Token: 0x0600372A RID: 14122 RVA: 0x0011099C File Offset: 0x0010EB9C
		private void Init(AddressFamily family)
		{
			this.active = false;
			if (this.client != null)
			{
				this.client.Close();
				this.client = null;
			}
			this.client = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x001109CD File Offset: 0x0010EBCD
		public TcpClient()
		{
			this.Init(AddressFamily.InterNetwork);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001109F0 File Offset: 0x0010EBF0
		public TcpClient(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");
			}
			this.Init(family);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00110A2C File Offset: 0x0010EC2C
		public TcpClient(IPEndPoint localEP)
		{
			this.Init(localEP.AddressFamily);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x00110A54 File Offset: 0x0010EC54
		public TcpClient(string hostname, int port)
		{
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
			this.Connect(hostname, port);
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x00110A78 File Offset: 0x0010EC78
		// (set) Token: 0x06003730 RID: 14128 RVA: 0x00110A80 File Offset: 0x0010EC80
		protected bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x00110A89 File Offset: 0x0010EC89
		// (set) Token: 0x06003732 RID: 14130 RVA: 0x00110A91 File Offset: 0x0010EC91
		public Socket Client
		{
			get
			{
				return this.client;
			}
			set
			{
				this.client = value;
				this.stream = null;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x00110AA1 File Offset: 0x0010ECA1
		public int Available
		{
			get
			{
				return this.client.Available;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x00110AAE File Offset: 0x0010ECAE
		public bool Connected
		{
			get
			{
				return this.client.Connected;
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00110ABC File Offset: 0x0010ECBC
		public bool IsConnected()
		{
			bool result;
			try
			{
				result = (!this.Client.Poll(1, SelectMode.SelectRead) || this.Client.Available != 0);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x00110B04 File Offset: 0x0010ED04
		// (set) Token: 0x06003737 RID: 14135 RVA: 0x00110B11 File Offset: 0x0010ED11
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.client.ExclusiveAddressUse;
			}
			set
			{
				this.client.ExclusiveAddressUse = value;
			}
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x00110B1F File Offset: 0x0010ED1F
		internal void SetTcpClient(Socket s)
		{
			this.Client = s;
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x00110B28 File Offset: 0x0010ED28
		// (set) Token: 0x0600373A RID: 14138 RVA: 0x00110B55 File Offset: 0x0010ED55
		public LingerOption LingerState
		{
			get
			{
				if ((this.values & TcpClient.Properties.LingerState) != (TcpClient.Properties)0U)
				{
					return this.linger_state;
				}
				return (LingerOption)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.linger_state = value;
					this.values |= TcpClient.Properties.LingerState;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x00110B90 File Offset: 0x0010ED90
		// (set) Token: 0x0600373C RID: 14140 RVA: 0x00110BB5 File Offset: 0x0010EDB5
		public bool NoDelay
		{
			get
			{
				if ((this.values & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0U)
				{
					return this.no_delay;
				}
				return (bool)this.client.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.no_delay = value;
					this.values |= TcpClient.Properties.NoDelay;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x00110BEE File Offset: 0x0010EDEE
		// (set) Token: 0x0600373E RID: 14142 RVA: 0x00110C1B File Offset: 0x0010EE1B
		public int ReceiveBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0U)
				{
					return this.recv_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_buffer_size = value;
					this.values |= TcpClient.Properties.ReceiveBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x00110C56 File Offset: 0x0010EE56
		// (set) Token: 0x06003740 RID: 14144 RVA: 0x00110C83 File Offset: 0x0010EE83
		public int ReceiveTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0U)
				{
					return this.recv_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_timeout = value;
					this.values |= TcpClient.Properties.ReceiveTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x00110CBE File Offset: 0x0010EEBE
		// (set) Token: 0x06003742 RID: 14146 RVA: 0x00110CEC File Offset: 0x0010EEEC
		public int SendBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0U)
				{
					return this.send_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_buffer_size = value;
					this.values |= TcpClient.Properties.SendBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x00110D28 File Offset: 0x0010EF28
		// (set) Token: 0x06003744 RID: 14148 RVA: 0x00110D56 File Offset: 0x0010EF56
		public int SendTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0U)
				{
					return this.send_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_timeout = value;
					this.values |= TcpClient.Properties.SendTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x00110D92 File Offset: 0x0010EF92
		// (set) Token: 0x06003746 RID: 14150 RVA: 0x00110D9A File Offset: 0x0010EF9A
		public TimeSpan ConnectTimeout { get; set; }

		// Token: 0x06003747 RID: 14151 RVA: 0x00110DA3 File Offset: 0x0010EFA3
		public void Close()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x00110DAC File Offset: 0x0010EFAC
		public void Connect(IPEndPoint remoteEP)
		{
			try
			{
				if (this.ConnectTimeout > TimeSpan.Zero)
				{
					ManualResetEvent mre = new ManualResetEvent(false);
					IAsyncResult asyncResult = this.client.BeginConnect(remoteEP, delegate(IAsyncResult res)
					{
						mre.Set();
					}, null);
					this.active = mre.WaitOne(this.ConnectTimeout);
					if (!this.active)
					{
						try
						{
							this.client.Close();
						}
						catch
						{
						}
						throw new TimeoutException("Connection timed out!");
					}
					this.client.EndConnect(asyncResult);
				}
				else
				{
					this.client.Connect(remoteEP);
					this.active = true;
				}
			}
			finally
			{
				this.CheckDisposed();
			}
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x00110E78 File Offset: 0x0010F078
		public void Connect(IPAddress address, int port)
		{
			this.Connect(new IPEndPoint(address, port));
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00110E88 File Offset: 0x0010F088
		private void SetOptions()
		{
			TcpClient.Properties properties = this.values;
			this.values = (TcpClient.Properties)0U;
			if ((properties & TcpClient.Properties.LingerState) != (TcpClient.Properties)0U)
			{
				this.LingerState = this.linger_state;
			}
			if ((properties & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0U)
			{
				this.NoDelay = this.no_delay;
			}
			if ((properties & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0U)
			{
				this.ReceiveBufferSize = this.recv_buffer_size;
			}
			if ((properties & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0U)
			{
				this.ReceiveTimeout = this.recv_timeout;
			}
			if ((properties & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0U)
			{
				this.SendBufferSize = this.send_buffer_size;
			}
			if ((properties & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0U)
			{
				this.SendTimeout = this.send_timeout;
			}
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x00110F0C File Offset: 0x0010F10C
		public void Connect(string hostname, int port)
		{
			if (!(this.ConnectTimeout > TimeSpan.Zero))
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
				this.Connect(hostAddresses, port);
				return;
			}
			ManualResetEvent mre = new ManualResetEvent(false);
			IAsyncResult asyncResult = Dns.BeginGetHostAddresses(hostname, delegate(IAsyncResult res)
			{
				mre.Set();
			}, null);
			if (mre.WaitOne(this.ConnectTimeout))
			{
				IPAddress[] ipAddresses = Dns.EndGetHostAddresses(asyncResult);
				this.Connect(ipAddresses, port);
				return;
			}
			throw new TimeoutException("DNS resolve timed out!");
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x00110F90 File Offset: 0x0010F190
		public void Connect(IPAddress[] ipAddresses, int port)
		{
			this.CheckDisposed();
			if (ipAddresses == null)
			{
				throw new ArgumentNullException("ipAddresses");
			}
			for (int i = 0; i < ipAddresses.Length; i++)
			{
				try
				{
					IPAddress ipaddress = ipAddresses[i];
					if (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any))
					{
						throw new SocketException(10049);
					}
					this.Init(ipaddress.AddressFamily);
					if (ipaddress.AddressFamily != AddressFamily.InterNetwork && ipaddress.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new NotSupportedException("This method is only valid for sockets in the InterNetwork and InterNetworkV6 families");
					}
					this.Connect(new IPEndPoint(ipaddress, port));
					if (this.values != (TcpClient.Properties)0U)
					{
						this.SetOptions();
					}
					this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
					break;
				}
				catch (Exception ex)
				{
					this.Init(AddressFamily.InterNetwork);
					if (i == ipAddresses.Length - 1)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x0011106C File Offset: 0x0010F26C
		public void EndConnect(IAsyncResult asyncResult)
		{
			this.client.EndConnect(asyncResult);
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x0011107A File Offset: 0x0010F27A
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(address, port, requestCallback, state);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x0011108C File Offset: 0x0010F28C
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(addresses, port, requestCallback, state);
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x0011109E File Offset: 0x0010F29E
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(host, port, requestCallback, state);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x001110B0 File Offset: 0x0010F2B0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x001110C0 File Offset: 0x0010F2C0
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (disposing)
			{
				NetworkStream networkStream = this.stream;
				this.stream = null;
				if (networkStream != null)
				{
					networkStream.Close();
					this.active = false;
					return;
				}
				if (this.client != null)
				{
					this.client.Close();
					this.client = null;
				}
			}
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x0011111C File Offset: 0x0010F31C
		~TcpClient()
		{
			this.Dispose(false);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x0011114C File Offset: 0x0010F34C
		public Stream GetStream()
		{
			Stream result;
			try
			{
				if (this.stream == null)
				{
					this.stream = new NetworkStream(this.client, true);
				}
				result = this.stream;
			}
			finally
			{
				this.CheckDisposed();
			}
			return result;
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x00111194 File Offset: 0x0010F394
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04002384 RID: 9092
		private NetworkStream stream;

		// Token: 0x04002385 RID: 9093
		private bool active;

		// Token: 0x04002386 RID: 9094
		private Socket client;

		// Token: 0x04002387 RID: 9095
		private bool disposed;

		// Token: 0x04002388 RID: 9096
		private TcpClient.Properties values;

		// Token: 0x04002389 RID: 9097
		private int recv_timeout;

		// Token: 0x0400238A RID: 9098
		private int send_timeout;

		// Token: 0x0400238B RID: 9099
		private int recv_buffer_size;

		// Token: 0x0400238C RID: 9100
		private int send_buffer_size;

		// Token: 0x0400238D RID: 9101
		private LingerOption linger_state;

		// Token: 0x0400238E RID: 9102
		private bool no_delay;

		// Token: 0x020008FF RID: 2303
		private enum Properties : uint
		{
			// Token: 0x04003037 RID: 12343
			LingerState = 1U,
			// Token: 0x04003038 RID: 12344
			NoDelay,
			// Token: 0x04003039 RID: 12345
			ReceiveBufferSize = 4U,
			// Token: 0x0400303A RID: 12346
			ReceiveTimeout = 8U,
			// Token: 0x0400303B RID: 12347
			SendBufferSize = 16U,
			// Token: 0x0400303C RID: 12348
			SendTimeout = 32U
		}
	}
}
