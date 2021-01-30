using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C9 RID: 969
	public class NewSessionTicket
	{
		// Token: 0x060023BB RID: 9147 RVA: 0x000B7DD1 File Offset: 0x000B5FD1
		public NewSessionTicket(long ticketLifetimeHint, byte[] ticket)
		{
			this.mTicketLifetimeHint = ticketLifetimeHint;
			this.mTicket = ticket;
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000B7DE7 File Offset: 0x000B5FE7
		public virtual long TicketLifetimeHint
		{
			get
			{
				return this.mTicketLifetimeHint;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x000B7DEF File Offset: 0x000B5FEF
		public virtual byte[] Ticket
		{
			get
			{
				return this.mTicket;
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000B7DF7 File Offset: 0x000B5FF7
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint32(this.mTicketLifetimeHint, output);
			TlsUtilities.WriteOpaque16(this.mTicket, output);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000B7E14 File Offset: 0x000B6014
		public static NewSessionTicket Parse(Stream input)
		{
			long ticketLifetimeHint = TlsUtilities.ReadUint32(input);
			byte[] ticket = TlsUtilities.ReadOpaque16(input);
			return new NewSessionTicket(ticketLifetimeHint, ticket);
		}

		// Token: 0x040018CF RID: 6351
		protected readonly long mTicketLifetimeHint;

		// Token: 0x040018D0 RID: 6352
		protected readonly byte[] mTicket;
	}
}
