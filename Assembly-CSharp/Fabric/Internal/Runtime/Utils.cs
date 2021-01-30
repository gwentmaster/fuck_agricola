using System;
using UnityEngine;

namespace Fabric.Internal.Runtime
{
	// Token: 0x0200025A RID: 602
	public static class Utils
	{
		// Token: 0x06001306 RID: 4870 RVA: 0x00071FFC File Offset: 0x000701FC
		public static void Log(string kit, string message)
		{
			Debug.Log("[" + kit + "] " + message);
		}
	}
}
