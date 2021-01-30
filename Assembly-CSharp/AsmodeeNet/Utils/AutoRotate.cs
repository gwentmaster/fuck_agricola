using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200065F RID: 1631
	public class AutoRotate : MonoBehaviour
	{
		// Token: 0x06003C38 RID: 15416 RVA: 0x0012A329 File Offset: 0x00128529
		private void FixedUpdate()
		{
			base.transform.Rotate(0f, 0f, this.Speed);
		}

		// Token: 0x040026E3 RID: 9955
		public float Speed = 1f;
	}
}
