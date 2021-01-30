using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Extensions;

namespace BestHTTP.Caching
{
	// Token: 0x02000613 RID: 1555
	public class HTTPCacheFileInfo : IComparable<HTTPCacheFileInfo>
	{
		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x0011C4EE File Offset: 0x0011A6EE
		// (set) Token: 0x06003918 RID: 14616 RVA: 0x0011C4F6 File Offset: 0x0011A6F6
		internal Uri Uri { get; set; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x0011C4FF File Offset: 0x0011A6FF
		// (set) Token: 0x0600391A RID: 14618 RVA: 0x0011C507 File Offset: 0x0011A707
		internal DateTime LastAccess { get; set; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x0011C510 File Offset: 0x0011A710
		// (set) Token: 0x0600391C RID: 14620 RVA: 0x0011C518 File Offset: 0x0011A718
		public int BodyLength { get; set; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x0011C521 File Offset: 0x0011A721
		// (set) Token: 0x0600391E RID: 14622 RVA: 0x0011C529 File Offset: 0x0011A729
		private string ETag { get; set; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x0011C532 File Offset: 0x0011A732
		// (set) Token: 0x06003920 RID: 14624 RVA: 0x0011C53A File Offset: 0x0011A73A
		private string LastModified { get; set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x0011C543 File Offset: 0x0011A743
		// (set) Token: 0x06003922 RID: 14626 RVA: 0x0011C54B File Offset: 0x0011A74B
		private DateTime Expires { get; set; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x0011C554 File Offset: 0x0011A754
		// (set) Token: 0x06003924 RID: 14628 RVA: 0x0011C55C File Offset: 0x0011A75C
		private long Age { get; set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x0011C565 File Offset: 0x0011A765
		// (set) Token: 0x06003926 RID: 14630 RVA: 0x0011C56D File Offset: 0x0011A76D
		private long MaxAge { get; set; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06003927 RID: 14631 RVA: 0x0011C576 File Offset: 0x0011A776
		// (set) Token: 0x06003928 RID: 14632 RVA: 0x0011C57E File Offset: 0x0011A77E
		private DateTime Date { get; set; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x0011C587 File Offset: 0x0011A787
		// (set) Token: 0x0600392A RID: 14634 RVA: 0x0011C58F File Offset: 0x0011A78F
		private bool MustRevalidate { get; set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600392B RID: 14635 RVA: 0x0011C598 File Offset: 0x0011A798
		// (set) Token: 0x0600392C RID: 14636 RVA: 0x0011C5A0 File Offset: 0x0011A7A0
		private DateTime Received { get; set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x0011C5A9 File Offset: 0x0011A7A9
		// (set) Token: 0x0600392E RID: 14638 RVA: 0x0011C5B1 File Offset: 0x0011A7B1
		private string ConstructedPath { get; set; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x0011C5BA File Offset: 0x0011A7BA
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x0011C5C2 File Offset: 0x0011A7C2
		internal ulong MappedNameIDX { get; set; }

		// Token: 0x06003931 RID: 14641 RVA: 0x0011C5CB File Offset: 0x0011A7CB
		internal HTTPCacheFileInfo(Uri uri) : this(uri, DateTime.UtcNow, -1)
		{
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x0011C5DA File Offset: 0x0011A7DA
		internal HTTPCacheFileInfo(Uri uri, DateTime lastAcces, int bodyLength)
		{
			this.Uri = uri;
			this.LastAccess = lastAcces;
			this.BodyLength = bodyLength;
			this.MaxAge = -1L;
			this.MappedNameIDX = HTTPCacheService.GetNameIdx();
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x0011C60C File Offset: 0x0011A80C
		internal HTTPCacheFileInfo(Uri uri, BinaryReader reader, int version)
		{
			this.Uri = uri;
			this.LastAccess = DateTime.FromBinary(reader.ReadInt64());
			this.BodyLength = reader.ReadInt32();
			if (version != 1)
			{
				if (version != 2)
				{
					return;
				}
				this.MappedNameIDX = reader.ReadUInt64();
			}
			this.ETag = reader.ReadString();
			this.LastModified = reader.ReadString();
			this.Expires = DateTime.FromBinary(reader.ReadInt64());
			this.Age = reader.ReadInt64();
			this.MaxAge = reader.ReadInt64();
			this.Date = DateTime.FromBinary(reader.ReadInt64());
			this.MustRevalidate = reader.ReadBoolean();
			this.Received = DateTime.FromBinary(reader.ReadInt64());
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x0011C6C8 File Offset: 0x0011A8C8
		internal void SaveTo(BinaryWriter writer)
		{
			writer.Write(this.LastAccess.ToBinary());
			writer.Write(this.BodyLength);
			writer.Write(this.MappedNameIDX);
			writer.Write(this.ETag);
			writer.Write(this.LastModified);
			writer.Write(this.Expires.ToBinary());
			writer.Write(this.Age);
			writer.Write(this.MaxAge);
			writer.Write(this.Date.ToBinary());
			writer.Write(this.MustRevalidate);
			writer.Write(this.Received.ToBinary());
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x0011C77C File Offset: 0x0011A97C
		public string GetPath()
		{
			if (this.ConstructedPath != null)
			{
				return this.ConstructedPath;
			}
			return this.ConstructedPath = Path.Combine(HTTPCacheService.CacheFolder, this.MappedNameIDX.ToString("X"));
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x0011C7BE File Offset: 0x0011A9BE
		public bool IsExists()
		{
			return HTTPCacheService.IsSupported && File.Exists(this.GetPath());
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x0011C7D4 File Offset: 0x0011A9D4
		internal void Delete()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			string path = this.GetPath();
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
			finally
			{
				this.Reset();
			}
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x0011C820 File Offset: 0x0011AA20
		private void Reset()
		{
			this.BodyLength = -1;
			this.ETag = string.Empty;
			this.Expires = DateTime.FromBinary(0L);
			this.LastModified = string.Empty;
			this.Age = 0L;
			this.MaxAge = -1L;
			this.Date = DateTime.FromBinary(0L);
			this.MustRevalidate = false;
			this.Received = DateTime.FromBinary(0L);
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0011C888 File Offset: 0x0011AA88
		private void SetUpCachingValues(HTTPResponse response)
		{
			response.CacheFileInfo = this;
			this.ETag = response.GetFirstHeaderValue("ETag").ToStrOrEmpty();
			this.Expires = response.GetFirstHeaderValue("Expires").ToDateTime(DateTime.FromBinary(0L));
			this.LastModified = response.GetFirstHeaderValue("Last-Modified").ToStrOrEmpty();
			this.Age = response.GetFirstHeaderValue("Age").ToInt64(0L);
			this.Date = response.GetFirstHeaderValue("Date").ToDateTime(DateTime.FromBinary(0L));
			string firstHeaderValue = response.GetFirstHeaderValue("cache-control");
			if (!string.IsNullOrEmpty(firstHeaderValue))
			{
				string[] array = firstHeaderValue.FindOption("max-age");
				double num;
				if (array != null && double.TryParse(array[1], out num))
				{
					this.MaxAge = (long)((int)num);
				}
				this.MustRevalidate = firstHeaderValue.ToLower().Contains("must-revalidate");
			}
			this.Received = DateTime.UtcNow;
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x0011C974 File Offset: 0x0011AB74
		internal bool WillExpireInTheFuture()
		{
			if (!this.IsExists())
			{
				return false;
			}
			if (this.MustRevalidate)
			{
				return false;
			}
			if (this.MaxAge != -1L)
			{
				long num = Math.Max(Math.Max(0L, (long)(this.Received - this.Date).TotalSeconds), this.Age);
				long num2 = (long)(DateTime.UtcNow - this.Date).TotalSeconds;
				return num + num2 < this.MaxAge;
			}
			return this.Expires > DateTime.UtcNow;
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x0011CA00 File Offset: 0x0011AC00
		internal void SetUpRevalidationHeaders(HTTPRequest request)
		{
			if (!this.IsExists())
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.ETag))
			{
				request.AddHeader("If-None-Match", this.ETag);
			}
			if (!string.IsNullOrEmpty(this.LastModified))
			{
				request.AddHeader("If-Modified-Since", this.LastModified);
			}
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x0011CA52 File Offset: 0x0011AC52
		public Stream GetBodyStream(out int length)
		{
			if (!this.IsExists())
			{
				length = 0;
				return null;
			}
			length = this.BodyLength;
			this.LastAccess = DateTime.UtcNow;
			FileStream fileStream = new FileStream(this.GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read);
			fileStream.Seek((long)(-(long)length), SeekOrigin.End);
			return fileStream;
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x0011CA90 File Offset: 0x0011AC90
		internal HTTPResponse ReadResponseTo(HTTPRequest request)
		{
			if (!this.IsExists())
			{
				return null;
			}
			this.LastAccess = DateTime.UtcNow;
			HTTPResponse result;
			using (FileStream fileStream = new FileStream(this.GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				HTTPResponse httpresponse = new HTTPResponse(request, fileStream, request.UseStreaming, true);
				httpresponse.CacheFileInfo = this;
				httpresponse.Receive(this.BodyLength, true);
				result = httpresponse;
			}
			return result;
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x0011CB04 File Offset: 0x0011AD04
		internal void Store(HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			string path = this.GetPath();
			if (path.Length > HTTPManager.MaxPathLength)
			{
				return;
			}
			if (File.Exists(path))
			{
				this.Delete();
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.WriteLine("HTTP/1.1 {0} {1}", new object[]
				{
					response.StatusCode,
					response.Message
				});
				foreach (KeyValuePair<string, List<string>> keyValuePair in response.Headers)
				{
					for (int i = 0; i < keyValuePair.Value.Count; i++)
					{
						fileStream.WriteLine("{0}: {1}", new object[]
						{
							keyValuePair.Key,
							keyValuePair.Value[i]
						});
					}
				}
				fileStream.WriteLine();
				fileStream.Write(response.Data, 0, response.Data.Length);
			}
			this.BodyLength = response.Data.Length;
			this.LastAccess = DateTime.UtcNow;
			this.SetUpCachingValues(response);
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x0011CC48 File Offset: 0x0011AE48
		internal Stream GetSaveStream(HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			this.LastAccess = DateTime.UtcNow;
			string path = this.GetPath();
			if (File.Exists(path))
			{
				this.Delete();
			}
			if (path.Length > HTTPManager.MaxPathLength)
			{
				return null;
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.WriteLine("HTTP/1.1 {0} {1}", new object[]
				{
					response.StatusCode,
					response.Message
				});
				foreach (KeyValuePair<string, List<string>> keyValuePair in response.Headers)
				{
					for (int i = 0; i < keyValuePair.Value.Count; i++)
					{
						fileStream.WriteLine("{0}: {1}", new object[]
						{
							keyValuePair.Key,
							keyValuePair.Value[i]
						});
					}
				}
				fileStream.WriteLine();
			}
			if (response.IsFromCache && !response.Headers.ContainsKey("content-length"))
			{
				response.Headers.Add("content-length", new List<string>
				{
					this.BodyLength.ToString()
				});
			}
			this.SetUpCachingValues(response);
			return new FileStream(this.GetPath(), FileMode.Append);
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x0011CDB8 File Offset: 0x0011AFB8
		public int CompareTo(HTTPCacheFileInfo other)
		{
			return this.LastAccess.CompareTo(other.LastAccess);
		}
	}
}
