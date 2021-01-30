using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class ProfileManager : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060008BF RID: 2239 RVA: 0x0003C7F0 File Offset: 0x0003A9F0
	public static ProfileManager instance
	{
		get
		{
			if (ProfileManager._instance == null)
			{
				ProfileManager._instance = UnityEngine.Object.FindObjectOfType<ProfileManager>();
				if (ProfileManager._instance == null)
				{
					ProfileManager._instance = new GameObject
					{
						name = "ScreenManager"
					}.AddComponent<ProfileManager>();
				}
			}
			return ProfileManager._instance;
		}
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00007945 File Offset: 0x00005B45
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0003C840 File Offset: 0x0003AA40
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0003C849 File Offset: 0x0003AA49
	private static int HashString(string s)
	{
		return s.GetHashCode();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0003C854 File Offset: 0x0003AA54
	private int GetProfileListHash()
	{
		int num = 0;
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			num = (33 * num ^ this.m_profileEntries[i].data.GetHash());
		}
		return num;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0003C898 File Offset: 0x0003AA98
	public bool Initialize()
	{
		if (!this.Load())
		{
			this.CreateDefaultProfile();
		}
		this.Verify();
		string @string = PlayerPrefs.GetString("OfflineProfile_LastUsed", "");
		if (!string.IsNullOrEmpty(@string))
		{
			this.m_CurrentProfile = this.CheckIfProfileExistWithDisplayName(@string);
		}
		if (this.m_CurrentProfile == null)
		{
			this.m_CurrentProfile = this.m_profileEntries[0].data;
			PlayerPrefs.SetString("OfflineProfile_LastUsed", this.m_profileEntries[0].data.name);
		}
		return true;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0003C920 File Offset: 0x0003AB20
	private void CreateDefaultProfile()
	{
		ProfileManager.OfflineProfileEntry offlineProfileEntry = new ProfileManager.OfflineProfileEntry();
		this.InitializeNewEntry(ref offlineProfileEntry);
		offlineProfileEntry.name = "Player";
		this.Add(offlineProfileEntry, true);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0003C950 File Offset: 0x0003AB50
	private void InitializeHeader(ref ProfileManager.OfflineProfileHeader header)
	{
		header.id = 21975632U;
		header.sizePerEntry = this.GetProfileListHash();
		header.numEntries = this.m_profileEntries.Count;
		header.lastUpdated = DateTime.Now.Second;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0003C99C File Offset: 0x0003AB9C
	private bool CheckFileVersion(ProfileManager.OfflineProfileHeader header)
	{
		return header.id == 21975632U;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0003C9AC File Offset: 0x0003ABAC
	private bool CheckValidProfileHeader(ProfileManager.OfflineProfileHeader fileHeader)
	{
		int profileListHash = this.GetProfileListHash();
		bool flag = this.CheckFileVersion(fileHeader);
		bool flag2 = fileHeader.sizePerEntry == profileListHash;
		bool flag3 = fileHeader.numEntries != 0;
		bool flag4 = flag && flag2 && flag3;
		Debug.Log(string.Format("Version OK?{0} checksum OK?{1} Count OK?{2} All OK:{3}", new object[]
		{
			flag,
			flag2,
			flag3,
			flag4
		}));
		if (!flag4)
		{
			if (!flag)
			{
				Debug.LogError("CheckValidProfileHeader - ERROR: Mismatched profile version");
			}
			if (!flag2)
			{
				Debug.LogError("CheckValidProfileHeader - ERROR: Mismatched profile checksum");
			}
			if (!flag3)
			{
				Debug.LogError("CheckValidProfileHeader - ERROR: File has zero entires");
			}
			string arg = new string((char)(fileHeader.id & 255U), 1);
			string arg2 = new string((char)(fileHeader.id >> 8 & 255U), 1);
			string arg3 = new string((char)(fileHeader.id >> 16 & 255U), 1);
			uint num = fileHeader.id >> 24 & 255U;
			Debug.Log(string.Format("Profile VERSION: Expected:{0} Observed{1}", 1, num));
			Debug.Log(string.Format("Profile TAG: Expected:PRO Observed{0}{1}{2}", arg, arg2, arg3));
			Debug.Log(string.Format("Profile CHECKSUM: Expected:{0} Observed{1}", profileListHash, fileHeader.sizePerEntry));
			Debug.Log(string.Format("Profiles ENTRIES: Expected:{0} Observed{1}", 1, fileHeader.numEntries));
		}
		return flag4;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0003CB14 File Offset: 0x0003AD14
	public void InitializeNewEntry(ref ProfileManager.OfflineProfileEntry profile)
	{
		profile.gameAvatar1 = 1;
		profile.gameAvatar2 = 2;
		profile.gameAvatar3 = 3;
		profile.gameAvatar4 = 4;
		profile.gameAvatar5 = 5;
		profile.factionIndex = 0;
		profile.name = "";
		this.ResetStats(ref profile);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0003CB64 File Offset: 0x0003AD64
	public void ResetStats(ref ProfileManager.OfflineProfileEntry profile)
	{
		profile.flags = 0U;
		profile.idNumber = 0L;
		profile.wins_2p = 0U;
		profile.wins_3p = 0U;
		profile.wins_4p = 0U;
		profile.wins_5p = 0U;
		profile.wins_6p = 0U;
		profile.losses_2p = 0U;
		profile.losses_3p = 0U;
		profile.losses_4p = 0U;
		profile.losses_5p = 0U;
		profile.losses_6p = 0U;
		profile.tutorialDone = 0U;
		for (int i = 0; i < profile.soloGameScores.Length; i++)
		{
			profile.soloGameScores[i] = 0;
		}
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0003CBF8 File Offset: 0x0003ADF8
	private void Verify()
	{
		bool flag = false;
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (this.m_profileEntries[i].data.soloGameScores == null || this.m_profileEntries[i].data.soloGameScores.Length == 0)
			{
				this.m_profileEntries[i].data.soloGameScores = new int[64];
				flag = true;
			}
		}
		if (flag)
		{
			this.Save();
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003CC78 File Offset: 0x0003AE78
	public bool Save()
	{
		ProfileManager.OfflineProfileHeader graph = new ProfileManager.OfflineProfileHeader();
		this.InitializeHeader(ref graph);
		string path = Path.Combine(Application.persistentDataPath, "OfflineProfiles.dat");
		try
		{
			using (FileStream fileStream = File.Open(path, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, graph);
				binaryFormatter.Serialize(fileStream, this.m_profileEntries);
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003CCF8 File Offset: 0x0003AEF8
	private bool Load()
	{
		this.m_CurrentProfile = null;
		this.m_profileEntries.Clear();
		string path = Path.Combine(Application.persistentDataPath, "OfflineProfiles.dat");
		if (!File.Exists(path))
		{
			Debug.Log("OfflineProfile save file does not exist!");
			return false;
		}
		try
		{
			bool flag = false;
			using (FileStream fileStream = File.Open(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				ProfileManager.OfflineProfileHeader fileHeader = (ProfileManager.OfflineProfileHeader)binaryFormatter.Deserialize(fileStream);
				this.m_profileEntries = (List<ProfileManager.MyProfile>)binaryFormatter.Deserialize(fileStream);
				if (!this.CheckValidProfileHeader(fileHeader))
				{
					flag = true;
				}
			}
			if (flag)
			{
				Debug.Log("Deleting corrupt profile file...");
				File.Delete(path);
				return false;
			}
		}
		catch (Exception)
		{
			Debug.Log("OfflineProfile exception!");
			return false;
		}
		if (this.m_profileEntries.Count == 0)
		{
			this.CreateDefaultProfile();
		}
		return true;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0003CDE4 File Offset: 0x0003AFE4
	public int Count()
	{
		if (this.m_profileEntries == null)
		{
			return 0;
		}
		return this.m_profileEntries.Count;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0003CDFC File Offset: 0x0003AFFC
	public int GetIndex(ProfileManager.OfflineProfileEntry profile)
	{
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (profile == this.m_profileEntries[i].data)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0003CE36 File Offset: 0x0003B036
	public ProfileManager.OfflineProfileEntry Get(int index)
	{
		if (index < 0 || index >= this.Count())
		{
			return null;
		}
		return this.m_profileEntries[index].data;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003CE58 File Offset: 0x0003B058
	public ProfileManager.OfflineProfileEntry GetProfile(string name)
	{
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (string.Equals(this.m_profileEntries[i].data.name, name, StringComparison.OrdinalIgnoreCase))
			{
				return this.m_profileEntries[i].data;
			}
		}
		return null;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003CEAD File Offset: 0x0003B0AD
	public string StringifyID(uint myID)
	{
		return "MBER:" + myID;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
	public ProfileManager.OfflineProfileEntry CheckIfProfileExistWithDisplayName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		List<ProfileManager.OfflineProfileEntry> list = new List<ProfileManager.OfflineProfileEntry>();
		int gamePlayerNameHash = this.GetGamePlayerNameHash(name);
		if (this.GetProfilesWithDisplayNameH(gamePlayerNameHash, ref list) > 0)
		{
			return list[0];
		}
		return null;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003CEFC File Offset: 0x0003B0FC
	public int GetProfilesWithDisplayNameH(int hash, ref List<ProfileManager.OfflineProfileEntry> names)
	{
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (this.m_profileEntries[i].nameHash == hash)
			{
				names.Add(this.m_profileEntries[i].data);
			}
		}
		return names.Count;
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003CF52 File Offset: 0x0003B152
	public string GetName(int index)
	{
		if (index < 0 || index >= this.m_profileEntries.Count)
		{
			return "INVALID- ERROR: 0x93";
		}
		return this.m_profileEntries[index].data.name;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003CF82 File Offset: 0x0003B182
	public string GetDisplayName(ProfileManager.OfflineProfileEntry profile)
	{
		return profile.name;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003CF8C File Offset: 0x0003B18C
	public void UpdateProfileName(ref ProfileManager.OfflineProfileEntry profile)
	{
		int index = this.GetIndex(profile);
		if (index < 0 || index >= this.Count())
		{
			return;
		}
		ProfileManager.MyProfile myProfile = this.m_profileEntries[index];
		this.GenerateNameHash(ref myProfile);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003CFC8 File Offset: 0x0003B1C8
	public bool Delete(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (string.Equals(this.m_profileEntries[i].data.name, name, StringComparison.OrdinalIgnoreCase))
			{
				return this.Delete(i);
			}
		}
		return false;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003D020 File Offset: 0x0003B220
	public bool Delete(int index)
	{
		if (index < 0 || index >= this.m_profileEntries.Count)
		{
			return false;
		}
		if (this.CheckDeleteProfile(this.m_profileEntries[index].data))
		{
			this.m_profileEntries.RemoveAt(index);
			if (this.m_profileEntries.Count == 0)
			{
				this.CreateDefaultProfile();
			}
			this.Save();
			return true;
		}
		return false;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003D084 File Offset: 0x0003B284
	public bool Delete(ProfileManager.OfflineProfileEntry profile)
	{
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			if (this.m_profileEntries[i].data == profile)
			{
				return this.Delete(i);
			}
		}
		return false;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003D0C4 File Offset: 0x0003B2C4
	public bool Create(string name)
	{
		ProfileManager.OfflineProfileEntry offlineProfileEntry = new ProfileManager.OfflineProfileEntry();
		this.InitializeNewEntry(ref offlineProfileEntry);
		offlineProfileEntry.name = name;
		return this.Add(offlineProfileEntry, true) != -1;
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003D0F4 File Offset: 0x0003B2F4
	public int Add(ProfileManager.OfflineProfileEntry newProfile, bool bSave = true)
	{
		ProfileManager.OfflineProfileEntry offlineProfileEntry = new ProfileManager.OfflineProfileEntry();
		ProfileManager.MyProfile myProfile = new ProfileManager.MyProfile();
		myProfile.data = newProfile;
		if (this.GetProfile(myProfile.data.name) != null)
		{
			return -1;
		}
		this.GenerateNameHash(ref myProfile);
		long num = Convert.ToInt64(PlayerPrefs.GetString("UsersSettings.OfflineProfileID", "1"));
		PlayerPrefs.SetString("UsersSettings.OfflineProfileID", string.Concat(myProfile.data.idNumber = num + 1L));
		this.m_profileEntries.Add(myProfile);
		if (bSave)
		{
			this.Save();
		}
		return this.m_profileEntries.Count - 1;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003D191 File Offset: 0x0003B391
	private void GenerateNameHash(ref ProfileManager.MyProfile profile)
	{
		profile.nameHash = this.GetGamePlayerNameHash(profile.data.name);
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003D1AC File Offset: 0x0003B3AC
	public ProfileManager.OfflineProfileEntry GetCurrentProfile()
	{
		return this.m_CurrentProfile;
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0003D1AC File Offset: 0x0003B3AC
	public ProfileManager.OfflineProfileEntry GetCurrentProfileRef()
	{
		return this.m_CurrentProfile;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003D1B4 File Offset: 0x0003B3B4
	public void SetCurrentProfile(ProfileManager.OfflineProfileEntry current)
	{
		if (this.GetIndex(current) != -1)
		{
			this.m_CurrentProfile = current;
			PlayerPrefs.SetString("OfflineProfile_LastUsed", this.m_CurrentProfile.name);
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0003D1DC File Offset: 0x0003B3DC
	public bool CheckDeleteProfile(ProfileManager.OfflineProfileEntry profile)
	{
		return this.Count() != 1;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0003D1EA File Offset: 0x0003B3EA
	private int GetGamePlayerNameHash(string s)
	{
		return s.GetHashCode();
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003D1F2 File Offset: 0x0003B3F2
	public string GetProfileSaveDirectory(ProfileManager.OfflineProfileEntry profile)
	{
		return string.Format("Profile_{0,0:D10}", profile.idNumber);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00003022 File Offset: 0x00001222
	private void PrintProfileEntry(ProfileManager.OfflineProfileEntry profileEntry)
	{
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003D20C File Offset: 0x0003B40C
	private void PrintProfile(ProfileManager.MyProfile profile)
	{
		Debug.Log(string.Format("PrintProfile() - Name:{0} NameHash:{1} ID:{2}", profile.data.name, profile.nameHash, profile.data.idNumber));
		this.PrintProfileEntry(profile.data);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003D25C File Offset: 0x0003B45C
	private void PrintAllProfiles()
	{
		Debug.Log(string.Format("PrintAllProfiles() - Number of Profiles:{0}", this.m_profileEntries.Count));
		for (int i = 0; i < this.m_profileEntries.Count; i++)
		{
			this.PrintProfile(this.m_profileEntries[i]);
		}
	}

	// Token: 0x04000987 RID: 2439
	private const byte OFFLINEPROFILE_VERSION = 1;

	// Token: 0x04000988 RID: 2440
	private const string OfflineProfileFilename = "OfflineProfiles.dat";

	// Token: 0x04000989 RID: 2441
	private const uint OFFLINEPROFILE_ID = 21975632U;

	// Token: 0x0400098A RID: 2442
	private List<ProfileManager.MyProfile> m_profileEntries = new List<ProfileManager.MyProfile>();

	// Token: 0x0400098B RID: 2443
	private ProfileManager.OfflineProfileEntry m_CurrentProfile;

	// Token: 0x0400098C RID: 2444
	private static ProfileManager _instance;

	// Token: 0x020007AF RID: 1967
	[Serializable]
	private class OfflineProfileHeader
	{
		// Token: 0x04002C99 RID: 11417
		public uint id;

		// Token: 0x04002C9A RID: 11418
		public int numEntries;

		// Token: 0x04002C9B RID: 11419
		public int sizePerEntry;

		// Token: 0x04002C9C RID: 11420
		public int lastUpdated;
	}

	// Token: 0x020007B0 RID: 1968
	[Serializable]
	public class OfflineProfileEntry
	{
		// Token: 0x060042C1 RID: 17089 RVA: 0x0013ED77 File Offset: 0x0013CF77
		public OfflineProfileEntry()
		{
			this.soloGameScores = new int[64];
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x0013ED9C File Offset: 0x0013CF9C
		public int GetHash()
		{
			int num = 0;
			num = (33 * num ^ ProfileManager.HashString(this.name));
			num = (33 * num ^ (int)this.flags);
			num = (33 * num ^ (int)this.factionIndex);
			num = (33 * num ^ (int)this.gameAvatar1);
			num = (33 * num ^ (int)this.gameAvatar2);
			num = (33 * num ^ (int)this.gameAvatar3);
			num = (33 * num ^ (int)this.gameAvatar4);
			num = (33 * num ^ (int)this.gameAvatar5);
			num = (33 * num ^ (int)this.completed);
			num = (33 * num ^ (int)this.forfeits);
			num = (33 * num ^ (int)this.soloCurrentStreak);
			num = (33 * num ^ (int)this.soloTopStreak);
			num = (33 * num ^ (int)this.soloTopScore);
			num = (33 * num ^ (int)this.wins_2p);
			num = (33 * num ^ (int)this.wins_3p);
			num = (33 * num ^ (int)this.wins_4p);
			num = (33 * num ^ (int)this.wins_5p);
			num = (33 * num ^ (int)this.wins_6p);
			num = (33 * num ^ (int)this.losses_2p);
			num = (33 * num ^ (int)this.losses_3p);
			num = (33 * num ^ (int)this.losses_4p);
			num = (33 * num ^ (int)this.losses_5p);
			num = (33 * num ^ (int)this.losses_6p);
			num = (33 * num ^ (int)this.tutorialDone);
			return 33 * num ^ (int)this.rating;
		}

		// Token: 0x04002C9D RID: 11421
		public string name;

		// Token: 0x04002C9E RID: 11422
		public uint flags;

		// Token: 0x04002C9F RID: 11423
		public byte factionIndex;

		// Token: 0x04002CA0 RID: 11424
		public byte gameAvatar1;

		// Token: 0x04002CA1 RID: 11425
		public byte gameAvatar2;

		// Token: 0x04002CA2 RID: 11426
		public byte gameAvatar3;

		// Token: 0x04002CA3 RID: 11427
		public byte gameAvatar4;

		// Token: 0x04002CA4 RID: 11428
		public byte gameAvatar5;

		// Token: 0x04002CA5 RID: 11429
		public uint completed;

		// Token: 0x04002CA6 RID: 11430
		public uint forfeits;

		// Token: 0x04002CA7 RID: 11431
		public uint soloCurrentStreak;

		// Token: 0x04002CA8 RID: 11432
		public uint soloTopStreak;

		// Token: 0x04002CA9 RID: 11433
		public uint soloTopScore;

		// Token: 0x04002CAA RID: 11434
		public uint wins_2p;

		// Token: 0x04002CAB RID: 11435
		public uint wins_3p;

		// Token: 0x04002CAC RID: 11436
		public uint wins_4p;

		// Token: 0x04002CAD RID: 11437
		public uint wins_5p;

		// Token: 0x04002CAE RID: 11438
		public uint wins_6p;

		// Token: 0x04002CAF RID: 11439
		public uint losses_2p;

		// Token: 0x04002CB0 RID: 11440
		public uint losses_3p;

		// Token: 0x04002CB1 RID: 11441
		public uint losses_4p;

		// Token: 0x04002CB2 RID: 11442
		public uint losses_5p;

		// Token: 0x04002CB3 RID: 11443
		public uint losses_6p;

		// Token: 0x04002CB4 RID: 11444
		public uint tutorialDone;

		// Token: 0x04002CB5 RID: 11445
		public uint rating;

		// Token: 0x04002CB6 RID: 11446
		public long idNumber;

		// Token: 0x04002CB7 RID: 11447
		public int[] soloGameScores = new int[64];
	}

	// Token: 0x020007B1 RID: 1969
	[Serializable]
	public class MyProfile
	{
		// Token: 0x04002CB8 RID: 11448
		public ProfileManager.OfflineProfileEntry data;

		// Token: 0x04002CB9 RID: 11449
		public int nameHash;
	}
}
