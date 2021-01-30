using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A7 RID: 935
	public abstract class CipherSuite
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x000B706C File Offset: 0x000B526C
		public static bool IsScsv(int cipherSuite)
		{
			return cipherSuite == 255 || cipherSuite == 22016;
		}

		// Token: 0x040016F1 RID: 5873
		public const int TLS_NULL_WITH_NULL_NULL = 0;

		// Token: 0x040016F2 RID: 5874
		public const int TLS_RSA_WITH_NULL_MD5 = 1;

		// Token: 0x040016F3 RID: 5875
		public const int TLS_RSA_WITH_NULL_SHA = 2;

		// Token: 0x040016F4 RID: 5876
		public const int TLS_RSA_EXPORT_WITH_RC4_40_MD5 = 3;

		// Token: 0x040016F5 RID: 5877
		public const int TLS_RSA_WITH_RC4_128_MD5 = 4;

		// Token: 0x040016F6 RID: 5878
		public const int TLS_RSA_WITH_RC4_128_SHA = 5;

		// Token: 0x040016F7 RID: 5879
		public const int TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5 = 6;

		// Token: 0x040016F8 RID: 5880
		public const int TLS_RSA_WITH_IDEA_CBC_SHA = 7;

		// Token: 0x040016F9 RID: 5881
		public const int TLS_RSA_EXPORT_WITH_DES40_CBC_SHA = 8;

		// Token: 0x040016FA RID: 5882
		public const int TLS_RSA_WITH_DES_CBC_SHA = 9;

		// Token: 0x040016FB RID: 5883
		public const int TLS_RSA_WITH_3DES_EDE_CBC_SHA = 10;

		// Token: 0x040016FC RID: 5884
		public const int TLS_DH_DSS_EXPORT_WITH_DES40_CBC_SHA = 11;

		// Token: 0x040016FD RID: 5885
		public const int TLS_DH_DSS_WITH_DES_CBC_SHA = 12;

		// Token: 0x040016FE RID: 5886
		public const int TLS_DH_DSS_WITH_3DES_EDE_CBC_SHA = 13;

		// Token: 0x040016FF RID: 5887
		public const int TLS_DH_RSA_EXPORT_WITH_DES40_CBC_SHA = 14;

		// Token: 0x04001700 RID: 5888
		public const int TLS_DH_RSA_WITH_DES_CBC_SHA = 15;

		// Token: 0x04001701 RID: 5889
		public const int TLS_DH_RSA_WITH_3DES_EDE_CBC_SHA = 16;

		// Token: 0x04001702 RID: 5890
		public const int TLS_DHE_DSS_EXPORT_WITH_DES40_CBC_SHA = 17;

		// Token: 0x04001703 RID: 5891
		public const int TLS_DHE_DSS_WITH_DES_CBC_SHA = 18;

		// Token: 0x04001704 RID: 5892
		public const int TLS_DHE_DSS_WITH_3DES_EDE_CBC_SHA = 19;

		// Token: 0x04001705 RID: 5893
		public const int TLS_DHE_RSA_EXPORT_WITH_DES40_CBC_SHA = 20;

		// Token: 0x04001706 RID: 5894
		public const int TLS_DHE_RSA_WITH_DES_CBC_SHA = 21;

		// Token: 0x04001707 RID: 5895
		public const int TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 22;

		// Token: 0x04001708 RID: 5896
		public const int TLS_DH_anon_EXPORT_WITH_RC4_40_MD5 = 23;

		// Token: 0x04001709 RID: 5897
		public const int TLS_DH_anon_WITH_RC4_128_MD5 = 24;

		// Token: 0x0400170A RID: 5898
		public const int TLS_DH_anon_EXPORT_WITH_DES40_CBC_SHA = 25;

		// Token: 0x0400170B RID: 5899
		public const int TLS_DH_anon_WITH_DES_CBC_SHA = 26;

		// Token: 0x0400170C RID: 5900
		public const int TLS_DH_anon_WITH_3DES_EDE_CBC_SHA = 27;

		// Token: 0x0400170D RID: 5901
		public const int TLS_RSA_WITH_AES_128_CBC_SHA = 47;

		// Token: 0x0400170E RID: 5902
		public const int TLS_DH_DSS_WITH_AES_128_CBC_SHA = 48;

		// Token: 0x0400170F RID: 5903
		public const int TLS_DH_RSA_WITH_AES_128_CBC_SHA = 49;

		// Token: 0x04001710 RID: 5904
		public const int TLS_DHE_DSS_WITH_AES_128_CBC_SHA = 50;

		// Token: 0x04001711 RID: 5905
		public const int TLS_DHE_RSA_WITH_AES_128_CBC_SHA = 51;

		// Token: 0x04001712 RID: 5906
		public const int TLS_DH_anon_WITH_AES_128_CBC_SHA = 52;

		// Token: 0x04001713 RID: 5907
		public const int TLS_RSA_WITH_AES_256_CBC_SHA = 53;

		// Token: 0x04001714 RID: 5908
		public const int TLS_DH_DSS_WITH_AES_256_CBC_SHA = 54;

		// Token: 0x04001715 RID: 5909
		public const int TLS_DH_RSA_WITH_AES_256_CBC_SHA = 55;

		// Token: 0x04001716 RID: 5910
		public const int TLS_DHE_DSS_WITH_AES_256_CBC_SHA = 56;

		// Token: 0x04001717 RID: 5911
		public const int TLS_DHE_RSA_WITH_AES_256_CBC_SHA = 57;

		// Token: 0x04001718 RID: 5912
		public const int TLS_DH_anon_WITH_AES_256_CBC_SHA = 58;

		// Token: 0x04001719 RID: 5913
		public const int TLS_RSA_WITH_CAMELLIA_128_CBC_SHA = 65;

		// Token: 0x0400171A RID: 5914
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA = 66;

		// Token: 0x0400171B RID: 5915
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA = 67;

		// Token: 0x0400171C RID: 5916
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA = 68;

		// Token: 0x0400171D RID: 5917
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA = 69;

		// Token: 0x0400171E RID: 5918
		public const int TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA = 70;

		// Token: 0x0400171F RID: 5919
		public const int TLS_RSA_WITH_CAMELLIA_256_CBC_SHA = 132;

		// Token: 0x04001720 RID: 5920
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA = 133;

		// Token: 0x04001721 RID: 5921
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA = 134;

		// Token: 0x04001722 RID: 5922
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA = 135;

		// Token: 0x04001723 RID: 5923
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA = 136;

		// Token: 0x04001724 RID: 5924
		public const int TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA = 137;

		// Token: 0x04001725 RID: 5925
		public const int TLS_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 186;

		// Token: 0x04001726 RID: 5926
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA256 = 187;

		// Token: 0x04001727 RID: 5927
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 188;

		// Token: 0x04001728 RID: 5928
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA256 = 189;

		// Token: 0x04001729 RID: 5929
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 190;

		// Token: 0x0400172A RID: 5930
		public const int TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA256 = 191;

		// Token: 0x0400172B RID: 5931
		public const int TLS_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 192;

		// Token: 0x0400172C RID: 5932
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA256 = 193;

		// Token: 0x0400172D RID: 5933
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 194;

		// Token: 0x0400172E RID: 5934
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA256 = 195;

		// Token: 0x0400172F RID: 5935
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 196;

		// Token: 0x04001730 RID: 5936
		public const int TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA256 = 197;

		// Token: 0x04001731 RID: 5937
		public const int TLS_RSA_WITH_SEED_CBC_SHA = 150;

		// Token: 0x04001732 RID: 5938
		public const int TLS_DH_DSS_WITH_SEED_CBC_SHA = 151;

		// Token: 0x04001733 RID: 5939
		public const int TLS_DH_RSA_WITH_SEED_CBC_SHA = 152;

		// Token: 0x04001734 RID: 5940
		public const int TLS_DHE_DSS_WITH_SEED_CBC_SHA = 153;

		// Token: 0x04001735 RID: 5941
		public const int TLS_DHE_RSA_WITH_SEED_CBC_SHA = 154;

		// Token: 0x04001736 RID: 5942
		public const int TLS_DH_anon_WITH_SEED_CBC_SHA = 155;

		// Token: 0x04001737 RID: 5943
		public const int TLS_PSK_WITH_RC4_128_SHA = 138;

		// Token: 0x04001738 RID: 5944
		public const int TLS_PSK_WITH_3DES_EDE_CBC_SHA = 139;

		// Token: 0x04001739 RID: 5945
		public const int TLS_PSK_WITH_AES_128_CBC_SHA = 140;

		// Token: 0x0400173A RID: 5946
		public const int TLS_PSK_WITH_AES_256_CBC_SHA = 141;

		// Token: 0x0400173B RID: 5947
		public const int TLS_DHE_PSK_WITH_RC4_128_SHA = 142;

		// Token: 0x0400173C RID: 5948
		public const int TLS_DHE_PSK_WITH_3DES_EDE_CBC_SHA = 143;

		// Token: 0x0400173D RID: 5949
		public const int TLS_DHE_PSK_WITH_AES_128_CBC_SHA = 144;

		// Token: 0x0400173E RID: 5950
		public const int TLS_DHE_PSK_WITH_AES_256_CBC_SHA = 145;

		// Token: 0x0400173F RID: 5951
		public const int TLS_RSA_PSK_WITH_RC4_128_SHA = 146;

		// Token: 0x04001740 RID: 5952
		public const int TLS_RSA_PSK_WITH_3DES_EDE_CBC_SHA = 147;

		// Token: 0x04001741 RID: 5953
		public const int TLS_RSA_PSK_WITH_AES_128_CBC_SHA = 148;

		// Token: 0x04001742 RID: 5954
		public const int TLS_RSA_PSK_WITH_AES_256_CBC_SHA = 149;

		// Token: 0x04001743 RID: 5955
		public const int TLS_ECDH_ECDSA_WITH_NULL_SHA = 49153;

		// Token: 0x04001744 RID: 5956
		public const int TLS_ECDH_ECDSA_WITH_RC4_128_SHA = 49154;

		// Token: 0x04001745 RID: 5957
		public const int TLS_ECDH_ECDSA_WITH_3DES_EDE_CBC_SHA = 49155;

		// Token: 0x04001746 RID: 5958
		public const int TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA = 49156;

		// Token: 0x04001747 RID: 5959
		public const int TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA = 49157;

		// Token: 0x04001748 RID: 5960
		public const int TLS_ECDHE_ECDSA_WITH_NULL_SHA = 49158;

		// Token: 0x04001749 RID: 5961
		public const int TLS_ECDHE_ECDSA_WITH_RC4_128_SHA = 49159;

		// Token: 0x0400174A RID: 5962
		public const int TLS_ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA = 49160;

		// Token: 0x0400174B RID: 5963
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA = 49161;

		// Token: 0x0400174C RID: 5964
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA = 49162;

		// Token: 0x0400174D RID: 5965
		public const int TLS_ECDH_RSA_WITH_NULL_SHA = 49163;

		// Token: 0x0400174E RID: 5966
		public const int TLS_ECDH_RSA_WITH_RC4_128_SHA = 49164;

		// Token: 0x0400174F RID: 5967
		public const int TLS_ECDH_RSA_WITH_3DES_EDE_CBC_SHA = 49165;

		// Token: 0x04001750 RID: 5968
		public const int TLS_ECDH_RSA_WITH_AES_128_CBC_SHA = 49166;

		// Token: 0x04001751 RID: 5969
		public const int TLS_ECDH_RSA_WITH_AES_256_CBC_SHA = 49167;

		// Token: 0x04001752 RID: 5970
		public const int TLS_ECDHE_RSA_WITH_NULL_SHA = 49168;

		// Token: 0x04001753 RID: 5971
		public const int TLS_ECDHE_RSA_WITH_RC4_128_SHA = 49169;

		// Token: 0x04001754 RID: 5972
		public const int TLS_ECDHE_RSA_WITH_3DES_EDE_CBC_SHA = 49170;

		// Token: 0x04001755 RID: 5973
		public const int TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA = 49171;

		// Token: 0x04001756 RID: 5974
		public const int TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA = 49172;

		// Token: 0x04001757 RID: 5975
		public const int TLS_ECDH_anon_WITH_NULL_SHA = 49173;

		// Token: 0x04001758 RID: 5976
		public const int TLS_ECDH_anon_WITH_RC4_128_SHA = 49174;

		// Token: 0x04001759 RID: 5977
		public const int TLS_ECDH_anon_WITH_3DES_EDE_CBC_SHA = 49175;

		// Token: 0x0400175A RID: 5978
		public const int TLS_ECDH_anon_WITH_AES_128_CBC_SHA = 49176;

		// Token: 0x0400175B RID: 5979
		public const int TLS_ECDH_anon_WITH_AES_256_CBC_SHA = 49177;

		// Token: 0x0400175C RID: 5980
		public const int TLS_PSK_WITH_NULL_SHA = 44;

		// Token: 0x0400175D RID: 5981
		public const int TLS_DHE_PSK_WITH_NULL_SHA = 45;

		// Token: 0x0400175E RID: 5982
		public const int TLS_RSA_PSK_WITH_NULL_SHA = 46;

		// Token: 0x0400175F RID: 5983
		public const int TLS_SRP_SHA_WITH_3DES_EDE_CBC_SHA = 49178;

		// Token: 0x04001760 RID: 5984
		public const int TLS_SRP_SHA_RSA_WITH_3DES_EDE_CBC_SHA = 49179;

		// Token: 0x04001761 RID: 5985
		public const int TLS_SRP_SHA_DSS_WITH_3DES_EDE_CBC_SHA = 49180;

		// Token: 0x04001762 RID: 5986
		public const int TLS_SRP_SHA_WITH_AES_128_CBC_SHA = 49181;

		// Token: 0x04001763 RID: 5987
		public const int TLS_SRP_SHA_RSA_WITH_AES_128_CBC_SHA = 49182;

		// Token: 0x04001764 RID: 5988
		public const int TLS_SRP_SHA_DSS_WITH_AES_128_CBC_SHA = 49183;

		// Token: 0x04001765 RID: 5989
		public const int TLS_SRP_SHA_WITH_AES_256_CBC_SHA = 49184;

		// Token: 0x04001766 RID: 5990
		public const int TLS_SRP_SHA_RSA_WITH_AES_256_CBC_SHA = 49185;

		// Token: 0x04001767 RID: 5991
		public const int TLS_SRP_SHA_DSS_WITH_AES_256_CBC_SHA = 49186;

		// Token: 0x04001768 RID: 5992
		public const int TLS_RSA_WITH_NULL_SHA256 = 59;

		// Token: 0x04001769 RID: 5993
		public const int TLS_RSA_WITH_AES_128_CBC_SHA256 = 60;

		// Token: 0x0400176A RID: 5994
		public const int TLS_RSA_WITH_AES_256_CBC_SHA256 = 61;

		// Token: 0x0400176B RID: 5995
		public const int TLS_DH_DSS_WITH_AES_128_CBC_SHA256 = 62;

		// Token: 0x0400176C RID: 5996
		public const int TLS_DH_RSA_WITH_AES_128_CBC_SHA256 = 63;

		// Token: 0x0400176D RID: 5997
		public const int TLS_DHE_DSS_WITH_AES_128_CBC_SHA256 = 64;

		// Token: 0x0400176E RID: 5998
		public const int TLS_DHE_RSA_WITH_AES_128_CBC_SHA256 = 103;

		// Token: 0x0400176F RID: 5999
		public const int TLS_DH_DSS_WITH_AES_256_CBC_SHA256 = 104;

		// Token: 0x04001770 RID: 6000
		public const int TLS_DH_RSA_WITH_AES_256_CBC_SHA256 = 105;

		// Token: 0x04001771 RID: 6001
		public const int TLS_DHE_DSS_WITH_AES_256_CBC_SHA256 = 106;

		// Token: 0x04001772 RID: 6002
		public const int TLS_DHE_RSA_WITH_AES_256_CBC_SHA256 = 107;

		// Token: 0x04001773 RID: 6003
		public const int TLS_DH_anon_WITH_AES_128_CBC_SHA256 = 108;

		// Token: 0x04001774 RID: 6004
		public const int TLS_DH_anon_WITH_AES_256_CBC_SHA256 = 109;

		// Token: 0x04001775 RID: 6005
		public const int TLS_RSA_WITH_AES_128_GCM_SHA256 = 156;

		// Token: 0x04001776 RID: 6006
		public const int TLS_RSA_WITH_AES_256_GCM_SHA384 = 157;

		// Token: 0x04001777 RID: 6007
		public const int TLS_DHE_RSA_WITH_AES_128_GCM_SHA256 = 158;

		// Token: 0x04001778 RID: 6008
		public const int TLS_DHE_RSA_WITH_AES_256_GCM_SHA384 = 159;

		// Token: 0x04001779 RID: 6009
		public const int TLS_DH_RSA_WITH_AES_128_GCM_SHA256 = 160;

		// Token: 0x0400177A RID: 6010
		public const int TLS_DH_RSA_WITH_AES_256_GCM_SHA384 = 161;

		// Token: 0x0400177B RID: 6011
		public const int TLS_DHE_DSS_WITH_AES_128_GCM_SHA256 = 162;

		// Token: 0x0400177C RID: 6012
		public const int TLS_DHE_DSS_WITH_AES_256_GCM_SHA384 = 163;

		// Token: 0x0400177D RID: 6013
		public const int TLS_DH_DSS_WITH_AES_128_GCM_SHA256 = 164;

		// Token: 0x0400177E RID: 6014
		public const int TLS_DH_DSS_WITH_AES_256_GCM_SHA384 = 165;

		// Token: 0x0400177F RID: 6015
		public const int TLS_DH_anon_WITH_AES_128_GCM_SHA256 = 166;

		// Token: 0x04001780 RID: 6016
		public const int TLS_DH_anon_WITH_AES_256_GCM_SHA384 = 167;

		// Token: 0x04001781 RID: 6017
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 = 49187;

		// Token: 0x04001782 RID: 6018
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 = 49188;

		// Token: 0x04001783 RID: 6019
		public const int TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256 = 49189;

		// Token: 0x04001784 RID: 6020
		public const int TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384 = 49190;

		// Token: 0x04001785 RID: 6021
		public const int TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256 = 49191;

		// Token: 0x04001786 RID: 6022
		public const int TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384 = 49192;

		// Token: 0x04001787 RID: 6023
		public const int TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256 = 49193;

		// Token: 0x04001788 RID: 6024
		public const int TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384 = 49194;

		// Token: 0x04001789 RID: 6025
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 = 49195;

		// Token: 0x0400178A RID: 6026
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 = 49196;

		// Token: 0x0400178B RID: 6027
		public const int TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256 = 49197;

		// Token: 0x0400178C RID: 6028
		public const int TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384 = 49198;

		// Token: 0x0400178D RID: 6029
		public const int TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256 = 49199;

		// Token: 0x0400178E RID: 6030
		public const int TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384 = 49200;

		// Token: 0x0400178F RID: 6031
		public const int TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256 = 49201;

		// Token: 0x04001790 RID: 6032
		public const int TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384 = 49202;

		// Token: 0x04001791 RID: 6033
		public const int TLS_PSK_WITH_AES_128_GCM_SHA256 = 168;

		// Token: 0x04001792 RID: 6034
		public const int TLS_PSK_WITH_AES_256_GCM_SHA384 = 169;

		// Token: 0x04001793 RID: 6035
		public const int TLS_DHE_PSK_WITH_AES_128_GCM_SHA256 = 170;

		// Token: 0x04001794 RID: 6036
		public const int TLS_DHE_PSK_WITH_AES_256_GCM_SHA384 = 171;

		// Token: 0x04001795 RID: 6037
		public const int TLS_RSA_PSK_WITH_AES_128_GCM_SHA256 = 172;

		// Token: 0x04001796 RID: 6038
		public const int TLS_RSA_PSK_WITH_AES_256_GCM_SHA384 = 173;

		// Token: 0x04001797 RID: 6039
		public const int TLS_PSK_WITH_AES_128_CBC_SHA256 = 174;

		// Token: 0x04001798 RID: 6040
		public const int TLS_PSK_WITH_AES_256_CBC_SHA384 = 175;

		// Token: 0x04001799 RID: 6041
		public const int TLS_PSK_WITH_NULL_SHA256 = 176;

		// Token: 0x0400179A RID: 6042
		public const int TLS_PSK_WITH_NULL_SHA384 = 177;

		// Token: 0x0400179B RID: 6043
		public const int TLS_DHE_PSK_WITH_AES_128_CBC_SHA256 = 178;

		// Token: 0x0400179C RID: 6044
		public const int TLS_DHE_PSK_WITH_AES_256_CBC_SHA384 = 179;

		// Token: 0x0400179D RID: 6045
		public const int TLS_DHE_PSK_WITH_NULL_SHA256 = 180;

		// Token: 0x0400179E RID: 6046
		public const int TLS_DHE_PSK_WITH_NULL_SHA384 = 181;

		// Token: 0x0400179F RID: 6047
		public const int TLS_RSA_PSK_WITH_AES_128_CBC_SHA256 = 182;

		// Token: 0x040017A0 RID: 6048
		public const int TLS_RSA_PSK_WITH_AES_256_CBC_SHA384 = 183;

		// Token: 0x040017A1 RID: 6049
		public const int TLS_RSA_PSK_WITH_NULL_SHA256 = 184;

		// Token: 0x040017A2 RID: 6050
		public const int TLS_RSA_PSK_WITH_NULL_SHA384 = 185;

		// Token: 0x040017A3 RID: 6051
		public const int TLS_ECDHE_PSK_WITH_RC4_128_SHA = 49203;

		// Token: 0x040017A4 RID: 6052
		public const int TLS_ECDHE_PSK_WITH_3DES_EDE_CBC_SHA = 49204;

		// Token: 0x040017A5 RID: 6053
		public const int TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA = 49205;

		// Token: 0x040017A6 RID: 6054
		public const int TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA = 49206;

		// Token: 0x040017A7 RID: 6055
		public const int TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA256 = 49207;

		// Token: 0x040017A8 RID: 6056
		public const int TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA384 = 49208;

		// Token: 0x040017A9 RID: 6057
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA = 49209;

		// Token: 0x040017AA RID: 6058
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA256 = 49210;

		// Token: 0x040017AB RID: 6059
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA384 = 49211;

		// Token: 0x040017AC RID: 6060
		public const int TLS_EMPTY_RENEGOTIATION_INFO_SCSV = 255;

		// Token: 0x040017AD RID: 6061
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = 49266;

		// Token: 0x040017AE RID: 6062
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = 49267;

		// Token: 0x040017AF RID: 6063
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = 49268;

		// Token: 0x040017B0 RID: 6064
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = 49269;

		// Token: 0x040017B1 RID: 6065
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 49270;

		// Token: 0x040017B2 RID: 6066
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_256_CBC_SHA384 = 49271;

		// Token: 0x040017B3 RID: 6067
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 49272;

		// Token: 0x040017B4 RID: 6068
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_256_CBC_SHA384 = 49273;

		// Token: 0x040017B5 RID: 6069
		public const int TLS_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49274;

		// Token: 0x040017B6 RID: 6070
		public const int TLS_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49275;

		// Token: 0x040017B7 RID: 6071
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49276;

		// Token: 0x040017B8 RID: 6072
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49277;

		// Token: 0x040017B9 RID: 6073
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49278;

		// Token: 0x040017BA RID: 6074
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49279;

		// Token: 0x040017BB RID: 6075
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_GCM_SHA256 = 49280;

		// Token: 0x040017BC RID: 6076
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_GCM_SHA384 = 49281;

		// Token: 0x040017BD RID: 6077
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_GCM_SHA256 = 49282;

		// Token: 0x040017BE RID: 6078
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_GCM_SHA384 = 49283;

		// Token: 0x040017BF RID: 6079
		public const int TLS_DH_anon_WITH_CAMELLIA_128_GCM_SHA256 = 49284;

		// Token: 0x040017C0 RID: 6080
		public const int TLS_DH_anon_WITH_CAMELLIA_256_GCM_SHA384 = 49285;

		// Token: 0x040017C1 RID: 6081
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = 49286;

		// Token: 0x040017C2 RID: 6082
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = 49287;

		// Token: 0x040017C3 RID: 6083
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = 49288;

		// Token: 0x040017C4 RID: 6084
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = 49289;

		// Token: 0x040017C5 RID: 6085
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49290;

		// Token: 0x040017C6 RID: 6086
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49291;

		// Token: 0x040017C7 RID: 6087
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49292;

		// Token: 0x040017C8 RID: 6088
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49293;

		// Token: 0x040017C9 RID: 6089
		public const int TLS_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49294;

		// Token: 0x040017CA RID: 6090
		public const int TLS_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49295;

		// Token: 0x040017CB RID: 6091
		public const int TLS_DHE_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49296;

		// Token: 0x040017CC RID: 6092
		public const int TLS_DHE_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49297;

		// Token: 0x040017CD RID: 6093
		public const int TLS_RSA_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49298;

		// Token: 0x040017CE RID: 6094
		public const int TLS_RSA_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49299;

		// Token: 0x040017CF RID: 6095
		public const int TLS_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49300;

		// Token: 0x040017D0 RID: 6096
		public const int TLS_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49301;

		// Token: 0x040017D1 RID: 6097
		public const int TLS_DHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49302;

		// Token: 0x040017D2 RID: 6098
		public const int TLS_DHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49303;

		// Token: 0x040017D3 RID: 6099
		public const int TLS_RSA_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49304;

		// Token: 0x040017D4 RID: 6100
		public const int TLS_RSA_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49305;

		// Token: 0x040017D5 RID: 6101
		public const int TLS_ECDHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49306;

		// Token: 0x040017D6 RID: 6102
		public const int TLS_ECDHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49307;

		// Token: 0x040017D7 RID: 6103
		public const int TLS_RSA_WITH_AES_128_CCM = 49308;

		// Token: 0x040017D8 RID: 6104
		public const int TLS_RSA_WITH_AES_256_CCM = 49309;

		// Token: 0x040017D9 RID: 6105
		public const int TLS_DHE_RSA_WITH_AES_128_CCM = 49310;

		// Token: 0x040017DA RID: 6106
		public const int TLS_DHE_RSA_WITH_AES_256_CCM = 49311;

		// Token: 0x040017DB RID: 6107
		public const int TLS_RSA_WITH_AES_128_CCM_8 = 49312;

		// Token: 0x040017DC RID: 6108
		public const int TLS_RSA_WITH_AES_256_CCM_8 = 49313;

		// Token: 0x040017DD RID: 6109
		public const int TLS_DHE_RSA_WITH_AES_128_CCM_8 = 49314;

		// Token: 0x040017DE RID: 6110
		public const int TLS_DHE_RSA_WITH_AES_256_CCM_8 = 49315;

		// Token: 0x040017DF RID: 6111
		public const int TLS_PSK_WITH_AES_128_CCM = 49316;

		// Token: 0x040017E0 RID: 6112
		public const int TLS_PSK_WITH_AES_256_CCM = 49317;

		// Token: 0x040017E1 RID: 6113
		public const int TLS_DHE_PSK_WITH_AES_128_CCM = 49318;

		// Token: 0x040017E2 RID: 6114
		public const int TLS_DHE_PSK_WITH_AES_256_CCM = 49319;

		// Token: 0x040017E3 RID: 6115
		public const int TLS_PSK_WITH_AES_128_CCM_8 = 49320;

		// Token: 0x040017E4 RID: 6116
		public const int TLS_PSK_WITH_AES_256_CCM_8 = 49321;

		// Token: 0x040017E5 RID: 6117
		public const int TLS_PSK_DHE_WITH_AES_128_CCM_8 = 49322;

		// Token: 0x040017E6 RID: 6118
		public const int TLS_PSK_DHE_WITH_AES_256_CCM_8 = 49323;

		// Token: 0x040017E7 RID: 6119
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CCM = 49324;

		// Token: 0x040017E8 RID: 6120
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CCM = 49325;

		// Token: 0x040017E9 RID: 6121
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CCM_8 = 49326;

		// Token: 0x040017EA RID: 6122
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CCM_8 = 49327;

		// Token: 0x040017EB RID: 6123
		public const int TLS_FALLBACK_SCSV = 22016;

		// Token: 0x040017EC RID: 6124
		public const int DRAFT_TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 52392;

		// Token: 0x040017ED RID: 6125
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 = 52393;

		// Token: 0x040017EE RID: 6126
		public const int DRAFT_TLS_DHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 52394;

		// Token: 0x040017EF RID: 6127
		public const int DRAFT_TLS_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52395;

		// Token: 0x040017F0 RID: 6128
		public const int DRAFT_TLS_ECDHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52396;

		// Token: 0x040017F1 RID: 6129
		public const int DRAFT_TLS_DHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52397;

		// Token: 0x040017F2 RID: 6130
		public const int DRAFT_TLS_RSA_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52398;

		// Token: 0x040017F3 RID: 6131
		public const int DRAFT_TLS_DHE_RSA_WITH_AES_128_OCB = 65280;

		// Token: 0x040017F4 RID: 6132
		public const int DRAFT_TLS_DHE_RSA_WITH_AES_256_OCB = 65281;

		// Token: 0x040017F5 RID: 6133
		public const int DRAFT_TLS_ECDHE_RSA_WITH_AES_128_OCB = 65282;

		// Token: 0x040017F6 RID: 6134
		public const int DRAFT_TLS_ECDHE_RSA_WITH_AES_256_OCB = 65283;

		// Token: 0x040017F7 RID: 6135
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_AES_128_OCB = 65284;

		// Token: 0x040017F8 RID: 6136
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_AES_256_OCB = 65285;

		// Token: 0x040017F9 RID: 6137
		public const int DRAFT_TLS_PSK_WITH_AES_128_OCB = 65296;

		// Token: 0x040017FA RID: 6138
		public const int DRAFT_TLS_PSK_WITH_AES_256_OCB = 65297;

		// Token: 0x040017FB RID: 6139
		public const int DRAFT_TLS_DHE_PSK_WITH_AES_128_OCB = 65298;

		// Token: 0x040017FC RID: 6140
		public const int DRAFT_TLS_DHE_PSK_WITH_AES_256_OCB = 65299;

		// Token: 0x040017FD RID: 6141
		public const int DRAFT_TLS_ECDHE_PSK_WITH_AES_128_OCB = 65300;

		// Token: 0x040017FE RID: 6142
		public const int DRAFT_TLS_ECDHE_PSK_WITH_AES_256_OCB = 65301;
	}
}
