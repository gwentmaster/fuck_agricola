using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D2 RID: 1234
	public abstract class Asn1Sequence : Asn1Object, IEnumerable
	{
		// Token: 0x06002DBF RID: 11711 RVA: 0x000F0188 File Offset: 0x000EE388
		public static Asn1Sequence GetInstance(object obj)
		{
			if (obj == null || obj is Asn1Sequence)
			{
				return (Asn1Sequence)obj;
			}
			if (obj is Asn1SequenceParser)
			{
				return Asn1Sequence.GetInstance(((Asn1SequenceParser)obj).ToAsn1Object());
			}
			if (obj is byte[])
			{
				try
				{
					return Asn1Sequence.GetInstance(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (IOException ex)
				{
					throw new ArgumentException("failed to construct sequence from byte[]: " + ex.Message);
				}
			}
			if (obj is Asn1Encodable)
			{
				Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
				if (asn1Object is Asn1Sequence)
				{
					return (Asn1Sequence)asn1Object;
				}
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000F0244 File Offset: 0x000EE444
		public static Asn1Sequence GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			Asn1Object @object = obj.GetObject();
			if (explicitly)
			{
				if (!obj.IsExplicit())
				{
					throw new ArgumentException("object implicit - explicit expected.");
				}
				return (Asn1Sequence)@object;
			}
			else if (obj.IsExplicit())
			{
				if (obj is BerTaggedObject)
				{
					return new BerSequence(@object);
				}
				return new DerSequence(@object);
			}
			else
			{
				if (@object is Asn1Sequence)
				{
					return (Asn1Sequence)@object;
				}
				throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000F02BC File Offset: 0x000EE4BC
		protected internal Asn1Sequence(int capacity)
		{
			this.seq = Platform.CreateArrayList(capacity);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000F02D0 File Offset: 0x000EE4D0
		public virtual IEnumerator GetEnumerator()
		{
			return this.seq.GetEnumerator();
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000F02DD File Offset: 0x000EE4DD
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002DC4 RID: 11716 RVA: 0x000F02E5 File Offset: 0x000EE4E5
		public virtual Asn1SequenceParser Parser
		{
			get
			{
				return new Asn1Sequence.Asn1SequenceParserImpl(this);
			}
		}

		// Token: 0x17000535 RID: 1333
		public virtual Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this.seq[index];
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000F0300 File Offset: 0x000EE500
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetObjectAt(int index)
		{
			return this[index];
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000F0309 File Offset: 0x000EE509
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002DC8 RID: 11720 RVA: 0x000F0311 File Offset: 0x000EE511
		public virtual int Count
		{
			get
			{
				return this.seq.Count;
			}
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000F0320 File Offset: 0x000EE520
		protected override int Asn1GetHashCode()
		{
			int num = this.Count;
			foreach (object obj in this)
			{
				num *= 17;
				if (obj == null)
				{
					num ^= DerNull.Instance.GetHashCode();
				}
				else
				{
					num ^= obj.GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000F0390 File Offset: 0x000EE590
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1Sequence asn1Sequence = asn1Object as Asn1Sequence;
			if (asn1Sequence == null)
			{
				return false;
			}
			if (this.Count != asn1Sequence.Count)
			{
				return false;
			}
			IEnumerator enumerator = this.GetEnumerator();
			IEnumerator enumerator2 = asn1Sequence.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object obj = this.GetCurrent(enumerator).ToAsn1Object();
				Asn1Object obj2 = this.GetCurrent(enumerator2).ToAsn1Object();
				if (!obj.Equals(obj2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000F0400 File Offset: 0x000EE600
		private Asn1Encodable GetCurrent(IEnumerator e)
		{
			Asn1Encodable asn1Encodable = (Asn1Encodable)e.Current;
			if (asn1Encodable == null)
			{
				return DerNull.Instance;
			}
			return asn1Encodable;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000F0423 File Offset: 0x000EE623
		protected internal void AddObject(Asn1Encodable obj)
		{
			this.seq.Add(obj);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000F0432 File Offset: 0x000EE632
		public override string ToString()
		{
			return CollectionUtilities.ToString(this.seq);
		}

		// Token: 0x04001DE7 RID: 7655
		private readonly IList seq;

		// Token: 0x020008A7 RID: 2215
		private class Asn1SequenceParserImpl : Asn1SequenceParser, IAsn1Convertible
		{
			// Token: 0x060045CC RID: 17868 RVA: 0x00145748 File Offset: 0x00143948
			public Asn1SequenceParserImpl(Asn1Sequence outer)
			{
				this.outer = outer;
				this.max = outer.Count;
			}

			// Token: 0x060045CD RID: 17869 RVA: 0x00145764 File Offset: 0x00143964
			public IAsn1Convertible ReadObject()
			{
				if (this.index == this.max)
				{
					return null;
				}
				Asn1Sequence asn1Sequence = this.outer;
				int num = this.index;
				this.index = num + 1;
				Asn1Encodable asn1Encodable = asn1Sequence[num];
				if (asn1Encodable is Asn1Sequence)
				{
					return ((Asn1Sequence)asn1Encodable).Parser;
				}
				if (asn1Encodable is Asn1Set)
				{
					return ((Asn1Set)asn1Encodable).Parser;
				}
				return asn1Encodable;
			}

			// Token: 0x060045CE RID: 17870 RVA: 0x001457C7 File Offset: 0x001439C7
			public Asn1Object ToAsn1Object()
			{
				return this.outer;
			}

			// Token: 0x04002F8A RID: 12170
			private readonly Asn1Sequence outer;

			// Token: 0x04002F8B RID: 12171
			private readonly int max;

			// Token: 0x04002F8C RID: 12172
			private int index;
		}
	}
}
