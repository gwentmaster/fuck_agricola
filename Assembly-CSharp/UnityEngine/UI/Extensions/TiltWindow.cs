using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016F RID: 367
	public class TiltWindow : MonoBehaviour
	{
		// Token: 0x06000E2B RID: 3627 RVA: 0x0005A8CD File Offset: 0x00058ACD
		private void Start()
		{
			this.mTrans = base.transform;
			this.mStart = this.mTrans.localRotation;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0005A8EC File Offset: 0x00058AEC
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			float num = (float)Screen.width * 0.5f;
			float num2 = (float)Screen.height * 0.5f;
			float x = Mathf.Clamp((mousePosition.x - num) / num, -1f, 1f);
			float y = Mathf.Clamp((mousePosition.y - num2) / num2, -1f, 1f);
			this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), Time.deltaTime * 5f);
			this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.range.y, this.mRot.x * this.range.x, 0f);
		}

		// Token: 0x04000DBD RID: 3517
		public Vector2 range = new Vector2(5f, 3f);

		// Token: 0x04000DBE RID: 3518
		private Transform mTrans;

		// Token: 0x04000DBF RID: 3519
		private Quaternion mStart;

		// Token: 0x04000DC0 RID: 3520
		private Vector2 mRot = Vector2.zero;
	}
}
