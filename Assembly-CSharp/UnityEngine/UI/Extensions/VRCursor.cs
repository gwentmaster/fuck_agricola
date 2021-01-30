using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D7 RID: 471
	[AddComponentMenu("UI/Extensions/VR Cursor")]
	public class VRCursor : MonoBehaviour
	{
		// Token: 0x060011E7 RID: 4583 RVA: 0x0006ED84 File Offset: 0x0006CF84
		private void Update()
		{
			Vector3 position;
			position.x = Input.mousePosition.x * this.xSens;
			position.y = Input.mousePosition.y * this.ySens - 1f;
			position.z = base.transform.position.z;
			base.transform.position = position;
			VRInputModule.cursorPosition = base.transform.position;
			if (Input.GetMouseButtonDown(0) && this.currentCollider)
			{
				VRInputModule.PointerSubmit(this.currentCollider.gameObject);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0006EE1F File Offset: 0x0006D01F
		private void OnTriggerEnter(Collider other)
		{
			VRInputModule.PointerEnter(other.gameObject);
			this.currentCollider = other;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0006EE33 File Offset: 0x0006D033
		private void OnTriggerExit(Collider other)
		{
			VRInputModule.PointerExit(other.gameObject);
			this.currentCollider = null;
		}

		// Token: 0x04001064 RID: 4196
		public float xSens;

		// Token: 0x04001065 RID: 4197
		public float ySens;

		// Token: 0x04001066 RID: 4198
		private Collider currentCollider;
	}
}
