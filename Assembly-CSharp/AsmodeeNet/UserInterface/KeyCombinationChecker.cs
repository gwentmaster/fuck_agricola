using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063A RID: 1594
	public static class KeyCombinationChecker
	{
		// Token: 0x06003A9F RID: 15007 RVA: 0x001234D2 File Offset: 0x001216D2
		public static bool IsDebugKeyCombination()
		{
			return (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
		}
	}
}
