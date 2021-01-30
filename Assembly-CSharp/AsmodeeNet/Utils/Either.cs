using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000663 RID: 1635
	public class Either<T, U>
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003C71 RID: 15473 RVA: 0x0012AC35 File Offset: 0x00128E35
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x0012AC3D File Offset: 0x00128E3D
		public U Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0012AC45 File Offset: 0x00128E45
		protected Either(T value, U error)
		{
			this._value = value;
			this._error = error;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0012AC5C File Offset: 0x00128E5C
		public static Either<T, U> newWithValue(T value)
		{
			return new Either<!0, !1>(value, default(!1));
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0012AC78 File Offset: 0x00128E78
		public static Either<T, U> newWithError(U error)
		{
			return new Either<!0, !1>(default(!0), error);
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x0012AC94 File Offset: 0x00128E94
		public bool HasError
		{
			get
			{
				return this.Error != null;
			}
		}

		// Token: 0x040026EA RID: 9962
		private T _value;

		// Token: 0x040026EB RID: 9963
		private U _error;
	}
}
