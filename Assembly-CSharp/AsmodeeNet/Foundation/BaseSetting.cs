using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006FB RID: 1787
	[Serializable]
	public abstract class BaseSetting<T> : IBaseSetting
	{
		// Token: 0x06003F41 RID: 16193 RVA: 0x00134064 File Offset: 0x00132264
		protected BaseSetting(string name)
		{
			this.Name = name;
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x00134073 File Offset: 0x00132273
		// (set) Token: 0x06003F43 RID: 16195 RVA: 0x0013407B File Offset: 0x0013227B
		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00134084 File Offset: 0x00132284
		public virtual void Clear()
		{
			T value = this.Value;
			KeyValueStore.DeleteKey(this._FullPath);
			KeyValueStore.Save();
			if (!EqualityComparer<!0>.Default.Equals(this.DefaultValue, value))
			{
				Action<!0, !0> onValueChanged = this.OnValueChanged;
				if (onValueChanged == null)
				{
					return;
				}
				onValueChanged(value, this.DefaultValue);
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06003F45 RID: 16197 RVA: 0x001340D4 File Offset: 0x001322D4
		// (remove) Token: 0x06003F46 RID: 16198 RVA: 0x0013410C File Offset: 0x0013230C
		public event Action<!0, !0> OnValueChanged
		{
			[CompilerGenerated]
			add
			{
				Action<T, T> action = this.OnValueChanged;
				Action<T, T> action2;
				do
				{
					action2 = action;
					Action<T, T> value2 = (Action<!0, !0>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange<Action<T, T>>(ref this.OnValueChanged, value2, action2);
				}
				while (action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<T, T> action = this.OnValueChanged;
				Action<T, T> action2;
				do
				{
					action2 = action;
					Action<T, T> value2 = (Action<!0, !0>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange<Action<T, T>>(ref this.OnValueChanged, value2, action2);
				}
				while (action != action2);
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003F47 RID: 16199 RVA: 0x00134141 File Offset: 0x00132341
		protected string _FullPath
		{
			get
			{
				return "Settings." + this.Name;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x00134153 File Offset: 0x00132353
		// (set) Token: 0x06003F49 RID: 16201 RVA: 0x00134170 File Offset: 0x00132370
		public T Value
		{
			get
			{
				if (KeyValueStore.HasKey(this._FullPath))
				{
					return this._ReadValue();
				}
				return this.DefaultValue;
			}
			set
			{
				T value2 = this.Value;
				if (!EqualityComparer<!0>.Default.Equals(value, value2))
				{
					this._WriteValue(value);
					KeyValueStore.Save();
					Action<!0, !0> onValueChanged = this.OnValueChanged;
					if (onValueChanged == null)
					{
						return;
					}
					onValueChanged(value2, value);
				}
			}
		}

		// Token: 0x06003F4A RID: 16202
		protected abstract T _ReadValue();

		// Token: 0x06003F4B RID: 16203
		protected abstract void _WriteValue(T value);

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x001341B0 File Offset: 0x001323B0
		public T DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x001341B8 File Offset: 0x001323B8
		public override string ToString()
		{
			return string.Format("{0}={1} (default:{2})", this.Name, this.Value, this.DefaultValue);
		}

		// Token: 0x04002895 RID: 10389
		[HideInInspector]
		[SerializeField]
		private string _name;

		// Token: 0x04002897 RID: 10391
		[SerializeField]
		private T _defaultValue;
	}
}
