using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200018E RID: 398
	[Serializable]
	public struct Vector3_Array2D
	{
		// Token: 0x170000E3 RID: 227
		public Vector3 this[int _idx]
		{
			get
			{
				return this.array[_idx];
			}
			set
			{
				this.array[_idx] = value;
			}
		}

		// Token: 0x04000ECA RID: 3786
		[SerializeField]
		public Vector3[] array;
	}
}
