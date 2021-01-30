using System;
using System.Reflection;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000662 RID: 1634
	public abstract class Builder<T> where T : class
	{
		// Token: 0x06003C6E RID: 15470
		public abstract Builder<T>.BuilderErrors[] Validate();

		// Token: 0x06003C6F RID: 15471 RVA: 0x0012ABE8 File Offset: 0x00128DE8
		public Either<T, Builder<T>.BuilderErrors[]> Build(bool mustValidate = true)
		{
			if (mustValidate)
			{
				Builder<T>.BuilderErrors[] array = this.Validate();
				if (array != null)
				{
					return Either<!0, Builder<!0>.BuilderErrors[]>.newWithError(array);
				}
			}
			return Either<!0, Builder<!0>.BuilderErrors[]>.newWithValue(Activator.CreateInstance(typeof(!0), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[]
			{
				this
			}, null) as !0);
		}

		// Token: 0x0200098F RID: 2447
		public class BuilderErrors
		{
			// Token: 0x0600483D RID: 18493 RVA: 0x0014B5D4 File Offset: 0x001497D4
			public BuilderErrors(string badField, string reason)
			{
				this.badField = badField;
				this.reason = reason;
			}

			// Token: 0x0600483E RID: 18494 RVA: 0x0014B5EA File Offset: 0x001497EA
			public override string ToString()
			{
				return string.Format("'{0}' field is badly formatted. The reason is '{1}'", this.badField, this.reason);
			}

			// Token: 0x0400326C RID: 12908
			public string badField;

			// Token: 0x0400326D RID: 12909
			public string reason;
		}
	}
}
