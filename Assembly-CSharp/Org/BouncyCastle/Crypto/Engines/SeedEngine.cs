﻿using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200049C RID: 1180
	public class SeedEngine : IBlockCipher
	{
		// Token: 0x06002B20 RID: 11040 RVA: 0x000DB2D0 File Offset: 0x000D94D0
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			this.wKey = this.createWorkingKey(((KeyParameter)parameters).GetKey());
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000DB2F0 File Offset: 0x000D94F0
		public virtual string AlgorithmName
		{
			get
			{
				return "SEED";
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002B22 RID: 11042 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000C8990 File Offset: 0x000C6B90
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000DB2F8 File Offset: 0x000D94F8
		public virtual int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff)
		{
			if (this.wKey == null)
			{
				throw new InvalidOperationException("SEED engine not initialised");
			}
			Check.DataLength(inBuf, inOff, 16, "input buffer too short");
			Check.OutputLength(outBuf, outOff, 16, "output buffer too short");
			long num = this.bytesToLong(inBuf, inOff);
			long num2 = this.bytesToLong(inBuf, inOff + 8);
			if (this.forEncryption)
			{
				for (int i = 0; i < 16; i++)
				{
					long num3 = num2;
					num2 = (num ^ this.F(this.wKey[2 * i], this.wKey[2 * i + 1], num2));
					num = num3;
				}
			}
			else
			{
				for (int j = 15; j >= 0; j--)
				{
					long num4 = num2;
					num2 = (num ^ this.F(this.wKey[2 * j], this.wKey[2 * j + 1], num2));
					num = num4;
				}
			}
			this.longToBytes(outBuf, outOff, num2);
			this.longToBytes(outBuf, outOff + 8, num);
			return 16;
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000DB3CC File Offset: 0x000D95CC
		private int[] createWorkingKey(byte[] inKey)
		{
			int[] array = new int[32];
			long num = this.bytesToLong(inKey, 0);
			long num2 = this.bytesToLong(inKey, 8);
			int num3 = this.extractW0(num);
			int num4 = this.extractW1(num);
			int num5 = this.extractW0(num2);
			int num6 = this.extractW1(num2);
			for (int i = 0; i < 16; i++)
			{
				array[2 * i] = this.G(num3 + num5 - (int)SeedEngine.KC[i]);
				array[2 * i + 1] = this.G(num4 - num6 + (int)SeedEngine.KC[i]);
				if (i % 2 == 0)
				{
					num = this.rotateRight8(num);
					num3 = this.extractW0(num);
					num4 = this.extractW1(num);
				}
				else
				{
					num2 = this.rotateLeft8(num2);
					num5 = this.extractW0(num2);
					num6 = this.extractW1(num2);
				}
			}
			return array;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000DB498 File Offset: 0x000D9698
		private int extractW1(long lVal)
		{
			return (int)lVal;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000DB49C File Offset: 0x000D969C
		private int extractW0(long lVal)
		{
			return (int)(lVal >> 32);
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000DB4A3 File Offset: 0x000D96A3
		private long rotateLeft8(long x)
		{
			return x << 8 | (long)((ulong)x >> 56);
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000DB4AD File Offset: 0x000D96AD
		private long rotateRight8(long x)
		{
			return (long)((ulong)x >> 8 | (ulong)((ulong)x << 56));
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000DB4B8 File Offset: 0x000D96B8
		private long bytesToLong(byte[] src, int srcOff)
		{
			long num = 0L;
			for (int i = 0; i <= 7; i++)
			{
				num = (num << 8) + (long)(src[i + srcOff] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000DB4E8 File Offset: 0x000D96E8
		private void longToBytes(byte[] dest, int destOff, long value)
		{
			for (int i = 0; i < 8; i++)
			{
				dest[i + destOff] = (byte)(value >> (7 - i) * 8);
			}
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000DB514 File Offset: 0x000D9714
		private int G(int x)
		{
			return (int)(SeedEngine.SS0[x & 255] ^ SeedEngine.SS1[x >> 8 & 255] ^ SeedEngine.SS2[x >> 16 & 255] ^ SeedEngine.SS3[x >> 24 & 255]);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000DB560 File Offset: 0x000D9760
		private long F(int ki0, int ki1, long r)
		{
			int r2 = (int)(r >> 32);
			int r3 = (int)r;
			int num = this.phaseCalc2(r2, ki0, r3, ki1);
			return (long)(num + this.phaseCalc1(r2, ki0, r3, ki1)) << 32 | ((long)num & (long)((ulong)-1));
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000DB597 File Offset: 0x000D9797
		private int phaseCalc1(int r0, int ki0, int r1, int ki1)
		{
			return this.G(this.G(r0 ^ ki0 ^ (r1 ^ ki1)) + (r0 ^ ki0));
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000DB5B1 File Offset: 0x000D97B1
		private int phaseCalc2(int r0, int ki0, int r1, int ki1)
		{
			return this.G(this.phaseCalc1(r0, ki0, r1, ki1) + this.G(r0 ^ ki0 ^ (r1 ^ ki1)));
		}

		// Token: 0x04001C6B RID: 7275
		private const int BlockSize = 16;

		// Token: 0x04001C6C RID: 7276
		private static readonly uint[] SS0 = new uint[]
		{
			696885672U,
			92635524U,
			382128852U,
			331600848U,
			340021332U,
			487395612U,
			747413676U,
			621093156U,
			491606364U,
			54739776U,
			403181592U,
			504238620U,
			289493328U,
			1020063996U,
			181060296U,
			591618912U,
			671621160U,
			71581764U,
			536879136U,
			495817116U,
			549511392U,
			583197408U,
			147374280U,
			386339604U,
			629514660U,
			261063564U,
			50529024U,
			994800504U,
			999011256U,
			318968592U,
			314757840U,
			785310444U,
			809529456U,
			210534540U,
			1057960764U,
			680042664U,
			839004720U,
			500027868U,
			919007988U,
			876900468U,
			751624428U,
			361075092U,
			185271048U,
			390550356U,
			474763356U,
			457921368U,
			1032696252U,
			16843008U,
			604250148U,
			470552604U,
			860058480U,
			411603096U,
			268439568U,
			214745292U,
			851636976U,
			432656856U,
			738992172U,
			667411428U,
			843215472U,
			58950528U,
			462132120U,
			297914832U,
			109478532U,
			164217288U,
			541089888U,
			272650320U,
			595829664U,
			734782440U,
			218956044U,
			914797236U,
			512660124U,
			256852812U,
			931640244U,
			441078360U,
			113689284U,
			944271480U,
			646357668U,
			302125584U,
			797942700U,
			365285844U,
			557932896U,
			63161280U,
			881111220U,
			21053760U,
			306336336U,
			1028485500U,
			227377548U,
			134742024U,
			521081628U,
			428446104U,
			0U,
			420024600U,
			67371012U,
			323179344U,
			935850996U,
			566354400U,
			1036907004U,
			910586484U,
			789521196U,
			654779172U,
			813740208U,
			193692552U,
			235799052U,
			730571688U,
			578986656U,
			776888940U,
			327390096U,
			223166796U,
			692674920U,
			1011642492U,
			151585032U,
			168428040U,
			1066382268U,
			802153452U,
			868479984U,
			96846276U,
			126321540U,
			335810580U,
			1053750012U,
			608460900U,
			516870876U,
			772678188U,
			189481800U,
			436867608U,
			101057028U,
			553722144U,
			726360936U,
			642146916U,
			33686016U,
			902164980U,
			310547088U,
			176849544U,
			202113036U,
			864269232U,
			1045328508U,
			281071824U,
			977957496U,
			122110788U,
			377918100U,
			633725412U,
			637936164U,
			8421504U,
			764256684U,
			533713884U,
			562143648U,
			805318704U,
			923218740U,
			781099692U,
			906375732U,
			352653588U,
			570565152U,
			940060728U,
			885321972U,
			663200676U,
			88424772U,
			206323788U,
			25264512U,
			701096424U,
			75792516U,
			394761108U,
			889532724U,
			197903304U,
			248431308U,
			1007431740U,
			826372464U,
			285282576U,
			130532292U,
			160006536U,
			893743476U,
			1003222008U,
			449499864U,
			952692984U,
			344232084U,
			424235352U,
			42107520U,
			80003268U,
			1070593020U,
			155795784U,
			956903736U,
			658989924U,
			12632256U,
			265274316U,
			398971860U,
			948482232U,
			252642060U,
			244220556U,
			37896768U,
			587408160U,
			293704080U,
			743202924U,
			466342872U,
			612671652U,
			872689716U,
			834793968U,
			138952776U,
			46318272U,
			793731948U,
			1024274748U,
			755835180U,
			4210752U,
			1049539260U,
			1041117756U,
			1015853244U,
			29475264U,
			713728680U,
			982168248U,
			240009804U,
			356864340U,
			990589752U,
			483184860U,
			675831912U,
			1062171516U,
			478974108U,
			415813848U,
			172638792U,
			373707348U,
			927429492U,
			545300640U,
			768467436U,
			105267780U,
			897954228U,
			722150184U,
			625303908U,
			986379000U,
			600040416U,
			965325240U,
			830583216U,
			529503132U,
			508449372U,
			969535992U,
			650568420U,
			847426224U,
			822161712U,
			717939432U,
			760045932U,
			525292380U,
			616882404U,
			817950960U,
			231588300U,
			143163528U,
			369496596U,
			973746744U,
			407392344U,
			348442836U,
			574775904U,
			688464168U,
			117900036U,
			855847728U,
			684253416U,
			453710616U,
			84214020U,
			961114488U,
			276861072U,
			709517928U,
			705307176U,
			445289112U
		};

		// Token: 0x04001C6D RID: 7277
		private static readonly uint[] SS1 = new uint[]
		{
			943196208U,
			3894986976U,
			741149985U,
			2753988258U,
			3423588291U,
			3693006546U,
			2956166067U,
			3090712752U,
			2888798115U,
			1612726368U,
			1410680145U,
			3288844227U,
			1141130304U,
			1815039843U,
			1747667811U,
			1478183763U,
			3221472195U,
			1612857954U,
			808649523U,
			3023406513U,
			673777953U,
			2686484640U,
			3760374498U,
			2754054051U,
			3490956243U,
			2417066385U,
			269549841U,
			67503618U,
			471600144U,
			3158084784U,
			875955762U,
			1208699715U,
			3962556387U,
			2282260608U,
			1814842464U,
			2821228704U,
			337053459U,
			3288646848U,
			336987666U,
			4097098992U,
			3221406402U,
			1141196097U,
			3760308705U,
			3558262482U,
			1010765619U,
			1010634033U,
			2349764226U,
			2551744656U,
			673712160U,
			1276005954U,
			4097230578U,
			1010699826U,
			2753922465U,
			4164536817U,
			202181889U,
			3693072339U,
			3625502928U,
			673909539U,
			1680229986U,
			2017086066U,
			606537507U,
			741281571U,
			4029792753U,
			1882342002U,
			1073889858U,
			3558130896U,
			1073824065U,
			3221274816U,
			1882407795U,
			1680295779U,
			2888600736U,
			2282457987U,
			4097296371U,
			2888666529U,
			2147516544U,
			471797523U,
			3356150466U,
			741084192U,
			2821360290U,
			875824176U,
			3490890450U,
			134941443U,
			3962490594U,
			3895052769U,
			1545424209U,
			2484372624U,
			404228112U,
			4164471024U,
			1410811731U,
			2888732322U,
			134744064U,
			3288712641U,
			269681427U,
			3423456705U,
			2215020162U,
			3090778545U,
			4232040435U,
			2084392305U,
			3221340609U,
			808517937U,
			4097164785U,
			2282392194U,
			1747602018U,
			2956034481U,
			3490824657U,
			538968096U,
			3558328275U,
			131586U,
			539099682U,
			67372032U,
			1747470432U,
			1882276209U,
			67569411U,
			3625700307U,
			2619182481U,
			2551810449U,
			1612792161U,
			3158216370U,
			3827746530U,
			1478052177U,
			3692940753U,
			1343308113U,
			2417000592U,
			3692874960U,
			2551876242U,
			2686682019U,
			2821426083U,
			3490758864U,
			2147582337U,
			202313475U,
			1141327683U,
			404359698U,
			3760440291U,
			3962359008U,
			2349698433U,
			3158282163U,
			2484504210U,
			2017151859U,
			1545358416U,
			2686616226U,
			2686550433U,
			1612923747U,
			539165475U,
			1275940161U,
			3356018880U,
			2619248274U,
			2619116688U,
			943327794U,
			202116096U,
			741215778U,
			3090844338U,
			1814974050U,
			2619314067U,
			1478117970U,
			4029858546U,
			2417132178U,
			4029924339U,
			1208568129U,
			2016954480U,
			3423390912U,
			336921873U,
			4164668403U,
			1882210416U,
			1949648241U,
			2084523891U,
			875889969U,
			269484048U,
			197379U,
			1680098400U,
			1814908257U,
			3288778434U,
			1949582448U,
			3558196689U,
			3023340720U,
			3895118562U,
			134809857U,
			1949714034U,
			404293905U,
			4231974642U,
			1073758272U,
			269615634U,
			3760242912U,
			3158150577U,
			67437825U,
			4164602610U,
			65793U,
			4029726960U,
			673843746U,
			1545490002U,
			2821294497U,
			1410745938U,
			1073955651U,
			2214954369U,
			336856080U,
			2282326401U,
			2551942035U,
			2955968688U,
			3827680737U,
			1208502336U,
			2017020273U,
			2484570003U,
			4231843056U,
			471731730U,
			2147648130U,
			539033889U,
			2349632640U,
			404425491U,
			1545555795U,
			1949779827U,
			1410614352U,
			2956100274U,
			471665937U,
			606405921U,
			1276071747U,
			0U,
			1141261890U,
			3962424801U,
			1477986384U,
			1343373906U,
			3895184355U,
			2084458098U,
			3625634514U,
			3356084673U,
			4231908849U,
			808452144U,
			2484438417U,
			1680164193U,
			1010568240U,
			3023472306U,
			3827614944U,
			3090910131U,
			2084326512U,
			202247682U,
			1343242320U,
			943262001U,
			606471714U,
			808583730U,
			2214888576U,
			1747536225U,
			2417197971U,
			876021555U,
			3827812323U,
			606340128U,
			2753856672U,
			3356216259U,
			1343439699U,
			134875650U,
			2215085955U,
			3625568721U,
			1275874368U,
			2147713923U,
			2349830019U,
			3423522498U,
			943393587U,
			1208633922U,
			3023538099U
		};

		// Token: 0x04001C6E RID: 7278
		private static readonly uint[] SS2 = new uint[]
		{
			2712152457U,
			2172913029U,
			3537114822U,
			3553629123U,
			1347687492U,
			287055117U,
			2695638156U,
			556016901U,
			1364991309U,
			1128268611U,
			270014472U,
			303832590U,
			1364201793U,
			4043062476U,
			3267889866U,
			1667244867U,
			539502600U,
			1078199364U,
			538976256U,
			2442927501U,
			3772784832U,
			3806339778U,
			3234334920U,
			320083719U,
			2711889285U,
			2206994319U,
			50332419U,
			1937259339U,
			3015195531U,
			319820547U,
			3536851650U,
			3807129294U,
			1886400576U,
			2156661900U,
			859586319U,
			2695374984U,
			842019330U,
			3520863693U,
			4076091078U,
			1886663748U,
			3773574348U,
			2442401157U,
			50858763U,
			1398019911U,
			1348213836U,
			1398283083U,
			2981903757U,
			16777473U,
			539239428U,
			270277644U,
			1936732995U,
			2425886856U,
			269488128U,
			3234598092U,
			4075827906U,
			3520600521U,
			539765772U,
			3823380423U,
			1919955522U,
			2206204803U,
			2476219275U,
			3520074177U,
			2189690502U,
			3251112393U,
			1616912448U,
			1347424320U,
			2745181059U,
			3823643595U,
			17566989U,
			2998154886U,
			2459704974U,
			1129058127U,
			3014932359U,
			1381505610U,
			3267626694U,
			1886926920U,
			2728666758U,
			303043074U,
			2745970575U,
			3520337349U,
			1633689921U,
			3284140995U,
			2964599940U,
			1094713665U,
			1380979266U,
			1903967565U,
			2173439373U,
			526344U,
			320610063U,
			2442664329U,
			0U,
			286791945U,
			263172U,
			1397756739U,
			4092868551U,
			3789562305U,
			4059839949U,
			1920218694U,
			590098191U,
			589571847U,
			2964336768U,
			2206731147U,
			34344462U,
			2745707403U,
			2728403586U,
			1651256910U,
			2475692931U,
			1095503181U,
			1634216265U,
			1887190092U,
			17303817U,
			34081290U,
			3015458703U,
			3823906767U,
			4092605379U,
			3250849221U,
			2206467975U,
			269751300U,
			4076617422U,
			1617175620U,
			3537641166U,
			573320718U,
			1128794955U,
			303569418U,
			33818118U,
			555753729U,
			1667771211U,
			1650730566U,
			33554946U,
			4059313605U,
			2458915458U,
			2189953674U,
			789516U,
			3014669187U,
			1920745038U,
			3503296704U,
			1920481866U,
			1128531783U,
			2459178630U,
			3789825477U,
			572794374U,
			2155872384U,
			2712415629U,
			3554418639U,
			2711626113U,
			808464384U,
			859059975U,
			2729193102U,
			842282502U,
			286528773U,
			572531202U,
			808990728U,
			4042536132U,
			2745444231U,
			1094976837U,
			1078725708U,
			2172649857U,
			3790088649U,
			2156135556U,
			2475956103U,
			825505029U,
			3284667339U,
			3268153038U,
			809253900U,
			1903178049U,
			286265601U,
			3284404167U,
			2173176201U,
			1903441221U,
			4093131723U,
			3537377994U,
			4042799304U,
			2425623684U,
			1364728137U,
			2189427330U,
			3234071748U,
			4093394895U,
			1095240009U,
			825768201U,
			1667508039U,
			3233808576U,
			3284930511U,
			3553892295U,
			2964863112U,
			51121935U,
			2190216846U,
			1111491138U,
			589308675U,
			2442137985U,
			1617701964U,
			3554155467U,
			2695111812U,
			808727556U,
			4059050433U,
			1078462536U,
			3267363522U,
			1668034383U,
			826031373U,
			556543245U,
			1077936192U,
			2998681230U,
			842808846U,
			2965126284U,
			3250586049U,
			2728929930U,
			2998418058U,
			1112280654U,
			1364464965U,
			859323147U,
			3504086220U,
			1617438792U,
			1937522511U,
			2426150028U,
			3503823048U,
			1112017482U,
			1381242438U,
			1936996167U,
			2694848640U,
			3790351821U,
			1111754310U,
			2981377413U,
			589835019U,
			1633953093U,
			4076354250U,
			3823117251U,
			2981640585U,
			2981114241U,
			2476482447U,
			1381768782U,
			4059576777U,
			3806602950U,
			2997891714U,
			825241857U,
			3806866122U,
			1634479437U,
			1398546255U,
			3773048004U,
			4042272960U,
			3251375565U,
			2156398728U,
			303306246U,
			842545674U,
			1347950664U,
			3503559876U,
			1650467394U,
			556280073U,
			50595591U,
			858796803U,
			3773311176U,
			320346891U,
			17040645U,
			1903704393U,
			2425360512U,
			1650993738U,
			573057546U,
			2459441802U
		};

		// Token: 0x04001C6F RID: 7279
		private static readonly uint[] SS3 = new uint[]
		{
			137377848U,
			3370182696U,
			220277805U,
			2258805798U,
			3485715471U,
			3469925406U,
			2209591347U,
			2293282872U,
			2409868335U,
			1080057888U,
			1162957845U,
			3351495687U,
			1145062404U,
			1331915823U,
			1264805931U,
			1263753243U,
			3284385795U,
			1113743394U,
			53686323U,
			2243015733U,
			153167913U,
			2158010400U,
			3269648418U,
			2275648551U,
			3285438483U,
			2173800465U,
			17895441U,
			100795398U,
			202382364U,
			2360392764U,
			103953462U,
			1262700555U,
			3487820847U,
			2290124808U,
			1281387564U,
			2292230184U,
			118690839U,
			3300967428U,
			101848086U,
			3304125492U,
			3267543042U,
			1161905157U,
			3252805665U,
			3335705622U,
			255015999U,
			221330493U,
			2390920206U,
			2291177496U,
			136325160U,
			1312967694U,
			3337810998U,
			238173246U,
			2241963045U,
			3388078137U,
			218172429U,
			3486768159U,
			3369130008U,
			186853419U,
			1180853286U,
			1249015866U,
			119743527U,
			253963311U,
			3253858353U,
			1114796082U,
			1111638018U,
			3302020116U,
			1094795265U,
			3233857536U,
			1131638835U,
			1197696039U,
			2359340076U,
			2340653067U,
			3354653751U,
			2376182829U,
			2155905024U,
			252910623U,
			3401762826U,
			203435052U,
			2325915690U,
			70267956U,
			3268595730U,
			184748043U,
			3470978094U,
			3387025449U,
			1297177629U,
			2224067604U,
			135272472U,
			3371235384U,
			1196643351U,
			2393025582U,
			134219784U,
			3317810181U,
			51580947U,
			3452029965U,
			2256700422U,
			2310125625U,
			3488873535U,
			1299283005U,
			3250700289U,
			20000817U,
			3320968245U,
			2323810314U,
			1247963178U,
			2175905841U,
			3251752977U,
			2105376U,
			3352548375U,
			33685506U,
			35790882U,
			67109892U,
			1214277672U,
			1097953329U,
			117638151U,
			3419658267U,
			2375130141U,
			2308020249U,
			1096900641U,
			2394078270U,
			3336758310U,
			1230067737U,
			3453082653U,
			1095847953U,
			2156957712U,
			3436239900U,
			2324863002U,
			2208538659U,
			2342758443U,
			3234910224U,
			2172747777U,
			251857935U,
			1195590663U,
			168957978U,
			3286491171U,
			3437292588U,
			2374077453U,
			2410921023U,
			2257753110U,
			1265858619U,
			1280334876U,
			2191695906U,
			2174853153U,
			1130586147U,
			52633635U,
			1296124941U,
			3368077320U,
			2391972894U,
			2358287388U,
			171063354U,
			201329676U,
			237120558U,
			2326968378U,
			1315073070U,
			2408815647U,
			1246910490U,
			3270701106U,
			2190643218U,
			3287543859U,
			1229015049U,
			1215330360U,
			3435187212U,
			85005333U,
			3421763643U,
			1081110576U,
			1165063221U,
			1332968511U,
			87110709U,
			1052688U,
			50528259U,
			1147167780U,
			1298230317U,
			3334652934U,
			1148220468U,
			3318862869U,
			2226172980U,
			3403868202U,
			151062537U,
			1181905974U,
			152115225U,
			3472030782U,
			1077952512U,
			34738194U,
			3235962912U,
			2377235517U,
			83952645U,
			3404920890U,
			16842753U,
			3237015600U,
			170010666U,
			1314020382U,
			2309072937U,
			1179800598U,
			1128480771U,
			2239857669U,
			68162580U,
			2306967561U,
			2341705755U,
			2159063088U,
			3319915557U,
			1212172296U,
			1232173113U,
			2274595863U,
			3438345276U,
			236067870U,
			2189590530U,
			18948129U,
			2357234700U,
			185800731U,
			1330863135U,
			1198748727U,
			1146115092U,
			2192748594U,
			219225117U,
			86058021U,
			1329810447U,
			0U,
			1178747910U,
			3454135341U,
			1213224984U,
			1112690706U,
			3420710955U,
			1316125758U,
			3402815514U,
			3384920073U,
			3455188029U,
			3158064U,
			2240910357U,
			1164010533U,
			204487740U,
			2259858486U,
			3303072804U,
			2343811131U,
			1282440252U,
			235015182U,
			1079005200U,
			154220601U,
			102900774U,
			36843570U,
			2223014916U,
			1231120425U,
			2207485971U,
			120796215U,
			3353601063U,
			69215268U,
			2225120292U,
			3418605579U,
			1129533459U,
			167905290U,
			2273543175U,
			3385972761U,
			1279282188U,
			2206433283U,
			2407762959U,
			3468872718U,
			187906107U,
			1245857802U,
			2276701239U
		};

		// Token: 0x04001C70 RID: 7280
		private static readonly uint[] KC = new uint[]
		{
			2654435769U,
			1013904243U,
			2027808486U,
			4055616972U,
			3816266649U,
			3337566003U,
			2380164711U,
			465362127U,
			930724254U,
			1861448508U,
			3722897016U,
			3150826737U,
			2006686179U,
			4013372358U,
			3731777421U,
			3168587547U
		};

		// Token: 0x04001C71 RID: 7281
		private int[] wKey;

		// Token: 0x04001C72 RID: 7282
		private bool forEncryption;
	}
}
