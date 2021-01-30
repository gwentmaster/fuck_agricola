using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D1 RID: 977
	public class ServerNameList
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000B89E8 File Offset: 0x000B6BE8
		public ServerNameList(IList serverNameList)
		{
			if (serverNameList == null)
			{
				throw new ArgumentNullException("serverNameList");
			}
			this.mServerNameList = serverNameList;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x000B8A05 File Offset: 0x000B6C05
		public virtual IList ServerNames
		{
			get
			{
				return this.mServerNameList;
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000B8A10 File Offset: 0x000B6C10
		public virtual void Encode(Stream output)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = TlsUtilities.EmptyBytes;
			foreach (object obj in this.ServerNames)
			{
				ServerName serverName = (ServerName)obj;
				array = ServerNameList.CheckNameType(array, serverName.NameType);
				if (array == null)
				{
					throw new TlsFatalAlert(80);
				}
				serverName.Encode(memoryStream);
			}
			TlsUtilities.CheckUint16(memoryStream.Length);
			TlsUtilities.WriteUint16((int)memoryStream.Length, output);
			memoryStream.WriteTo(output);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000B8AB0 File Offset: 0x000B6CB0
		public static ServerNameList Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint16(input);
			if (num < 1)
			{
				throw new TlsFatalAlert(50);
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			byte[] array = TlsUtilities.EmptyBytes;
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				ServerName serverName = ServerName.Parse(memoryStream);
				array = ServerNameList.CheckNameType(array, serverName.NameType);
				if (array == null)
				{
					throw new TlsFatalAlert(47);
				}
				list.Add(serverName);
			}
			return new ServerNameList(list);
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000B8B25 File Offset: 0x000B6D25
		private static byte[] CheckNameType(byte[] nameTypesSeen, byte nameType)
		{
			if (!NameType.IsValid(nameType) || Arrays.Contains(nameTypesSeen, nameType))
			{
				return null;
			}
			return Arrays.Append(nameTypesSeen, nameType);
		}

		// Token: 0x04001908 RID: 6408
		protected readonly IList mServerNameList;
	}
}
