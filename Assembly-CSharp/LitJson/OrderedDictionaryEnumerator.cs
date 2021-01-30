using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000265 RID: 613
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x0007350A File Offset: 0x0007170A
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00073518 File Offset: 0x00071718
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x00073544 File Offset: 0x00071744
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00073564 File Offset: 0x00071764
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00073584 File Offset: 0x00071784
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00073593 File Offset: 0x00071793
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000735A0 File Offset: 0x000717A0
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04001307 RID: 4871
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
