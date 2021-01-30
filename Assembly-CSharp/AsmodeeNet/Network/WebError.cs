using System;

namespace AsmodeeNet.Network
{
	// Token: 0x02000689 RID: 1673
	public class WebError
	{
		// Token: 0x06003D2F RID: 15663 RVA: 0x0012D397 File Offset: 0x0012B597
		protected WebError()
		{
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003D30 RID: 15664 RVA: 0x0012D3B1 File Offset: 0x0012B5B1
		public OAuthError GetOAuthError
		{
			get
			{
				return this as OAuthError;
			}
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x0012D3B9 File Offset: 0x0012B5B9
		public WebError(string error, int status = -1)
		{
			this.error = error;
			this.status = status;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0012D3E1 File Offset: 0x0012B5E1
		public T ToChildError<T>() where T : WebError
		{
			return this as !!0;
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0012D3EE File Offset: 0x0012B5EE
		public static WebError MakeNoResponseError()
		{
			return new WebError("no_response", -1);
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0012D3FB File Offset: 0x0012B5FB
		public static WebError MakeTimeoutError()
		{
			return new WebError("timeout", -1);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x0012D408 File Offset: 0x0012B608
		public static WebError MakePublicKeyPinningError()
		{
			return new WebError("public_key_pinning_error", -1);
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x0012D415 File Offset: 0x0012B615
		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.status, this.error);
		}

		// Token: 0x04002730 RID: 10032
		public const string kNoResponseError = "no_response";

		// Token: 0x04002731 RID: 10033
		public const string kTimeoutError = "timeout";

		// Token: 0x04002732 RID: 10034
		public const string kPublicKeyPinningError = "public_key_pinning_error";

		// Token: 0x04002733 RID: 10035
		public string error = "";

		// Token: 0x04002734 RID: 10036
		public int status = -1;
	}
}
