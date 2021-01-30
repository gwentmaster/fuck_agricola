using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x02000263 RID: 611
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600135A RID: 4954
		bool IsArray { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600135B RID: 4955
		bool IsBoolean { get; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600135C RID: 4956
		bool IsDouble { get; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600135D RID: 4957
		bool IsInt { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600135E RID: 4958
		bool IsLong { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600135F RID: 4959
		bool IsObject { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06001360 RID: 4960
		bool IsString { get; }

		// Token: 0x06001361 RID: 4961
		bool GetBoolean();

		// Token: 0x06001362 RID: 4962
		double GetDouble();

		// Token: 0x06001363 RID: 4963
		int GetInt();

		// Token: 0x06001364 RID: 4964
		JsonType GetJsonType();

		// Token: 0x06001365 RID: 4965
		long GetLong();

		// Token: 0x06001366 RID: 4966
		string GetString();

		// Token: 0x06001367 RID: 4967
		void SetBoolean(bool val);

		// Token: 0x06001368 RID: 4968
		void SetDouble(double val);

		// Token: 0x06001369 RID: 4969
		void SetInt(int val);

		// Token: 0x0600136A RID: 4970
		void SetJsonType(JsonType type);

		// Token: 0x0600136B RID: 4971
		void SetLong(long val);

		// Token: 0x0600136C RID: 4972
		void SetString(string val);

		// Token: 0x0600136D RID: 4973
		string ToJson();

		// Token: 0x0600136E RID: 4974
		void ToJson(JsonWriter writer);
	}
}
