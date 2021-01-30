using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class DontDestroyOnLoad : MonoBehaviour
{
	// Token: 0x06000163 RID: 355 RVA: 0x00007945 File Offset: 0x00005B45
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
