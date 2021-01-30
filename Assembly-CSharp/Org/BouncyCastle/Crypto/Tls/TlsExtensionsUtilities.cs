using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F0 RID: 1008
	public abstract class TlsExtensionsUtilities
	{
		// Token: 0x060024F2 RID: 9458 RVA: 0x000BC25C File Offset: 0x000BA45C
		public static IDictionary EnsureExtensionsInitialised(IDictionary extensions)
		{
			if (extensions != null)
			{
				return extensions;
			}
			return Platform.CreateHashtable();
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000BC268 File Offset: 0x000BA468
		public static void AddEncryptThenMacExtension(IDictionary extensions)
		{
			extensions[22] = TlsExtensionsUtilities.CreateEncryptThenMacExtension();
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000BC27C File Offset: 0x000BA47C
		public static void AddExtendedMasterSecretExtension(IDictionary extensions)
		{
			extensions[23] = TlsExtensionsUtilities.CreateExtendedMasterSecretExtension();
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000BC290 File Offset: 0x000BA490
		public static void AddHeartbeatExtension(IDictionary extensions, HeartbeatExtension heartbeatExtension)
		{
			extensions[15] = TlsExtensionsUtilities.CreateHeartbeatExtension(heartbeatExtension);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000BC2A5 File Offset: 0x000BA4A5
		public static void AddMaxFragmentLengthExtension(IDictionary extensions, byte maxFragmentLength)
		{
			extensions[1] = TlsExtensionsUtilities.CreateMaxFragmentLengthExtension(maxFragmentLength);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000BC2B9 File Offset: 0x000BA4B9
		public static void AddPaddingExtension(IDictionary extensions, int dataLength)
		{
			extensions[21] = TlsExtensionsUtilities.CreatePaddingExtension(dataLength);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000BC2CE File Offset: 0x000BA4CE
		public static void AddServerNameExtension(IDictionary extensions, ServerNameList serverNameList)
		{
			extensions[0] = TlsExtensionsUtilities.CreateServerNameExtension(serverNameList);
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000BC2E2 File Offset: 0x000BA4E2
		public static void AddStatusRequestExtension(IDictionary extensions, CertificateStatusRequest statusRequest)
		{
			extensions[5] = TlsExtensionsUtilities.CreateStatusRequestExtension(statusRequest);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000BC2F6 File Offset: 0x000BA4F6
		public static void AddTruncatedHMacExtension(IDictionary extensions)
		{
			extensions[4] = TlsExtensionsUtilities.CreateTruncatedHMacExtension();
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000BC30C File Offset: 0x000BA50C
		public static HeartbeatExtension GetHeartbeatExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 15);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadHeartbeatExtension(extensionData);
			}
			return null;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000BC330 File Offset: 0x000BA530
		public static short GetMaxFragmentLengthExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 1);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadMaxFragmentLengthExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000BC350 File Offset: 0x000BA550
		public static int GetPaddingExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 21);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadPaddingExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000BC374 File Offset: 0x000BA574
		public static ServerNameList GetServerNameExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadServerNameExtension(extensionData);
			}
			return null;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000BC394 File Offset: 0x000BA594
		public static CertificateStatusRequest GetStatusRequestExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 5);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadStatusRequestExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000BC3B4 File Offset: 0x000BA5B4
		public static bool HasEncryptThenMacExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 22);
			return extensionData != null && TlsExtensionsUtilities.ReadEncryptThenMacExtension(extensionData);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000BC3D8 File Offset: 0x000BA5D8
		public static bool HasExtendedMasterSecretExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 23);
			return extensionData != null && TlsExtensionsUtilities.ReadExtendedMasterSecretExtension(extensionData);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000BC3FC File Offset: 0x000BA5FC
		public static bool HasTruncatedHMacExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 4);
			return extensionData != null && TlsExtensionsUtilities.ReadTruncatedHMacExtension(extensionData);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000BC41C File Offset: 0x000BA61C
		public static byte[] CreateEmptyExtensionData()
		{
			return TlsUtilities.EmptyBytes;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000BC423 File Offset: 0x000BA623
		public static byte[] CreateEncryptThenMacExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000BC423 File Offset: 0x000BA623
		public static byte[] CreateExtendedMasterSecretExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000BC42C File Offset: 0x000BA62C
		public static byte[] CreateHeartbeatExtension(HeartbeatExtension heartbeatExtension)
		{
			if (heartbeatExtension == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			heartbeatExtension.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000BAA83 File Offset: 0x000B8C83
		public static byte[] CreateMaxFragmentLengthExtension(byte maxFragmentLength)
		{
			return new byte[]
			{
				maxFragmentLength
			};
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000BC457 File Offset: 0x000BA657
		public static byte[] CreatePaddingExtension(int dataLength)
		{
			TlsUtilities.CheckUint16(dataLength);
			return new byte[dataLength];
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000BC468 File Offset: 0x000BA668
		public static byte[] CreateServerNameExtension(ServerNameList serverNameList)
		{
			if (serverNameList == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			serverNameList.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000BC494 File Offset: 0x000BA694
		public static byte[] CreateStatusRequestExtension(CertificateStatusRequest statusRequest)
		{
			if (statusRequest == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			statusRequest.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000BC423 File Offset: 0x000BA623
		public static byte[] CreateTruncatedHMacExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000BC4BF File Offset: 0x000BA6BF
		private static bool ReadEmptyExtensionData(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			if (extensionData.Length != 0)
			{
				throw new TlsFatalAlert(47);
			}
			return true;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000BC4DC File Offset: 0x000BA6DC
		public static bool ReadEncryptThenMacExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000BC4DC File Offset: 0x000BA6DC
		public static bool ReadExtendedMasterSecretExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000BC4E4 File Offset: 0x000BA6E4
		public static HeartbeatExtension ReadHeartbeatExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			HeartbeatExtension result = HeartbeatExtension.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000BC513 File Offset: 0x000BA713
		public static short ReadMaxFragmentLengthExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			if (extensionData.Length != 1)
			{
				throw new TlsFatalAlert(50);
			}
			return (short)extensionData[0];
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000BC534 File Offset: 0x000BA734
		public static int ReadPaddingExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			for (int i = 0; i < extensionData.Length; i++)
			{
				if (extensionData[i] != 0)
				{
					throw new TlsFatalAlert(47);
				}
			}
			return extensionData.Length;
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000BC570 File Offset: 0x000BA770
		public static ServerNameList ReadServerNameExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			ServerNameList result = ServerNameList.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000BC5A0 File Offset: 0x000BA7A0
		public static CertificateStatusRequest ReadStatusRequestExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			CertificateStatusRequest result = CertificateStatusRequest.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000BC4DC File Offset: 0x000BA6DC
		public static bool ReadTruncatedHMacExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}
	}
}
