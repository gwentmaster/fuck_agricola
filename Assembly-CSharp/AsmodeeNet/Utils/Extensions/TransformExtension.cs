using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x0200067E RID: 1662
	public static class TransformExtension
	{
		// Token: 0x06003CDD RID: 15581 RVA: 0x0012C404 File Offset: 0x0012A604
		public static void RemoveAllChildren(this Transform transform)
		{
			foreach (object obj in transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			transform.DetachChildren();
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x0012C460 File Offset: 0x0012A660
		public static void Show(this Transform transform, bool show)
		{
			Renderer component = transform.gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.enabled = show;
			}
			CanvasRenderer component2 = transform.gameObject.GetComponent<CanvasRenderer>();
			if (component2 != null)
			{
				component2.cull = !show;
			}
			Canvas component3 = transform.gameObject.GetComponent<Canvas>();
			if (component3 != null)
			{
				component3.enabled = show;
				return;
			}
			foreach (object obj in transform)
			{
				((Transform)obj).Show(show);
			}
		}
	}
}
