using System;

namespace LitJson
{
	// Token: 0x02000266 RID: 614
	public class JsonException : Exception
	{
		// Token: 0x060013D4 RID: 5076 RVA: 0x000735AD File Offset: 0x000717AD
		public JsonException()
		{
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000735B5 File Offset: 0x000717B5
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000735CD File Offset: 0x000717CD
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000735E6 File Offset: 0x000717E6
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x000735FF File Offset: 0x000717FF
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00073619 File Offset: 0x00071819
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00073622 File Offset: 0x00071822
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
