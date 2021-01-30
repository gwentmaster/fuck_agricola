using System;
using AsmodeeNet.UserInterface.CrossPromo;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class MoreGamesButton : MonoBehaviour
{
	// Token: 0x06000899 RID: 2201 RVA: 0x0003BD0F File Offset: 0x00039F0F
	public void OnMoreGamesButtonPressed()
	{
		MoreGamesPopup.InstantiateMoreGames();
	}
}
