using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x020002A2 RID: 674
	public class PemObject : PemObjectGenerator
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x0008093F File Offset: 0x0007EB3F
		public PemObject(string type, byte[] content) : this(type, Platform.CreateArrayList(), content)
		{
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0008094E File Offset: 0x0007EB4E
		public PemObject(string type, IList headers, byte[] content)
		{
			this.type = type;
			this.headers = Platform.CreateArrayList(headers);
			this.content = content;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x00080970 File Offset: 0x0007EB70
		public string Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x00080978 File Offset: 0x0007EB78
		public IList Headers
		{
			get
			{
				return this.headers;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x00080980 File Offset: 0x0007EB80
		public byte[] Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00035D67 File Offset: 0x00033F67
		public PemObject Generate()
		{
			return this;
		}

		// Token: 0x04001509 RID: 5385
		private string type;

		// Token: 0x0400150A RID: 5386
		private IList headers;

		// Token: 0x0400150B RID: 5387
		private byte[] content;
	}
}
