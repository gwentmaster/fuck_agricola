using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D0 RID: 976
	public class ServerName
	{
		// Token: 0x06002400 RID: 9216 RVA: 0x000B88D3 File Offset: 0x000B6AD3
		public ServerName(byte nameType, object name)
		{
			if (!ServerName.IsCorrectType(nameType, name))
			{
				throw new ArgumentException("not an instance of the correct type", "name");
			}
			this.mNameType = nameType;
			this.mName = name;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x000B8902 File Offset: 0x000B6B02
		public virtual byte NameType
		{
			get
			{
				return this.mNameType;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x000B890A File Offset: 0x000B6B0A
		public virtual object Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000B8912 File Offset: 0x000B6B12
		public virtual string GetHostName()
		{
			if (!ServerName.IsCorrectType(0, this.mName))
			{
				throw new InvalidOperationException("'name' is not a HostName string");
			}
			return (string)this.mName;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000B8938 File Offset: 0x000B6B38
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mNameType, output);
			if (this.mNameType != 0)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] array = Strings.ToAsciiByteArray((string)this.mName);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(80);
			}
			TlsUtilities.WriteOpaque16(array, output);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000B8988 File Offset: 0x000B6B88
		public static ServerName Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b != 0)
			{
				throw new TlsFatalAlert(50);
			}
			byte[] array = TlsUtilities.ReadOpaque16(input);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(50);
			}
			object name = Strings.FromAsciiByteArray(array);
			return new ServerName(b, name);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000B89CA File Offset: 0x000B6BCA
		protected static bool IsCorrectType(byte nameType, object name)
		{
			if (nameType == 0)
			{
				return name is string;
			}
			throw new ArgumentException("unsupported value", "name");
		}

		// Token: 0x04001906 RID: 6406
		protected readonly byte mNameType;

		// Token: 0x04001907 RID: 6407
		protected readonly object mName;
	}
}
