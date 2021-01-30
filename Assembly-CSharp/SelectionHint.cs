using System;

// Token: 0x020000AE RID: 174
public class SelectionHint
{
	// Token: 0x04000795 RID: 1941
	public const ushort NONE = 40960;

	// Token: 0x04000796 RID: 1942
	public const ushort END_TURN = 40961;

	// Token: 0x04000797 RID: 1943
	public const ushort ASSIGN_WORKER = 40962;

	// Token: 0x04000798 RID: 1944
	public const ushort SELECT_ACTION = 40963;

	// Token: 0x04000799 RID: 1945
	public const ushort ASSIGN_WORKER_FORFEIT = 40964;

	// Token: 0x0400079A RID: 1946
	public const ushort ASSIGN_FOOD = 40965;

	// Token: 0x0400079B RID: 1947
	public const ushort ASSIGN_FENCE = 40966;

	// Token: 0x0400079C RID: 1948
	public const ushort ASSIGN_BREAD = 40967;

	// Token: 0x0400079D RID: 1949
	public const ushort BUILD_ROOM = 40976;

	// Token: 0x0400079E RID: 1950
	public const ushort PLOW_FIELD = 40977;

	// Token: 0x0400079F RID: 1951
	public const ushort SOW_FIELD = 40978;

	// Token: 0x040007A0 RID: 1952
	public const ushort SOW_IMPROVEMENT = 40979;

	// Token: 0x040007A1 RID: 1953
	public const ushort BUILD_STABLE = 40980;

	// Token: 0x040007A2 RID: 1954
	public const ushort FENCE_PASTURE = 40981;

	// Token: 0x040007A3 RID: 1955
	public const ushort BUILD_IMPROVEMENT = 40982;

	// Token: 0x040007A4 RID: 1956
	public const ushort PLAY_OCCUPATION = 40983;

	// Token: 0x040007A5 RID: 1957
	public const ushort FENCE_SINGLE_PASTURE = 40984;

	// Token: 0x040007A6 RID: 1958
	public const ushort DONE_BUILDING_ROOMS = 40986;

	// Token: 0x040007A7 RID: 1959
	public const ushort DONE_BUILDING_STABLES = 40987;

	// Token: 0x040007A8 RID: 1960
	public const ushort DONE_PLOWING_FIELDS = 40988;

	// Token: 0x040007A9 RID: 1961
	public const ushort DONE_SOWING_FIELDS = 40989;

	// Token: 0x040007AA RID: 1962
	public const ushort DONE_FENCING_PASTURES = 40990;

	// Token: 0x040007AB RID: 1963
	public const ushort PLOW_FIELD_FOR_FOOD = 40991;

	// Token: 0x040007AC RID: 1964
	public const ushort USE_CONVERT_ABILITY = 40992;

	// Token: 0x040007AD RID: 1965
	public const ushort END_BAKING_BREAD = 40993;

	// Token: 0x040007AE RID: 1966
	public const ushort END_FEEDING_PHASE = 40994;

	// Token: 0x040007AF RID: 1967
	public const ushort END_BREEDING_PHASE = 40995;

	// Token: 0x040007B0 RID: 1968
	public const ushort USE_ABILITY = 40996;

	// Token: 0x040007B1 RID: 1969
	public const ushort USE_RESOURCE_OPTION = 40997;

	// Token: 0x040007B2 RID: 1970
	public const ushort USE_ANYTIME_ABILITY = 40998;

	// Token: 0x040007B3 RID: 1971
	public const ushort SELECT_RESOURCE_OPTIONS = 40999;

	// Token: 0x040007B4 RID: 1972
	public const ushort USE_ABILITY_INVALID = 41000;

	// Token: 0x040007B5 RID: 1973
	public const ushort USE_ABILITY_CANNOT_UNDO = 41001;

	// Token: 0x040007B6 RID: 1974
	public const ushort CHOOSE_1_BUILDING_RESOURCE = 41008;

	// Token: 0x040007B7 RID: 1975
	public const ushort CHOOSE_2_BUILDING_RESOURCES = 41009;

	// Token: 0x040007B8 RID: 1976
	public const ushort CHOOSE_RESOURCE = 41010;

	// Token: 0x040007B9 RID: 1977
	public const ushort BID_FOR_AUCTION_RESOURCES = 41011;

	// Token: 0x040007BA RID: 1978
	public const ushort MOVE_CROP_TO_EMPTY_FIELD = 41017;

	// Token: 0x040007BB RID: 1979
	public const ushort TAKE_GRAIN_FROM_OPPONENT = 41018;

	// Token: 0x040007BC RID: 1980
	public const ushort ADD_RESOURCE_TO_SOWN_FIELD = 41019;

	// Token: 0x040007BD RID: 1981
	public const ushort TAKE_RESOURCE_FROM_FIELD = 41020;

	// Token: 0x040007BE RID: 1982
	public const ushort SOW_FIELD_FROM_ADJACENT = 41021;

	// Token: 0x040007BF RID: 1983
	public const ushort REMOVE_STABLE = 41022;

	// Token: 0x040007C0 RID: 1984
	public const ushort FENCE_EXISTING_PASTURE = 41023;

	// Token: 0x040007C1 RID: 1985
	public const ushort PAY_BASE_COST = 41024;

	// Token: 0x040007C2 RID: 1986
	public const ushort PAY_ALTERNATE_COST = 41026;

	// Token: 0x040007C3 RID: 1987
	public const ushort PAY_RESOURCE_COST = 41027;

	// Token: 0x040007C4 RID: 1988
	public const ushort BUY_RESOURCES = 41028;

	// Token: 0x040007C5 RID: 1989
	public const ushort ARRANGE_ANIMALS_BEGIN = 41040;

	// Token: 0x040007C6 RID: 1990
	public const ushort ARRANGE_ANIMALS = 41041;

	// Token: 0x040007C7 RID: 1991
	public const ushort ARRANGE_ANIMALS_END = 41042;

	// Token: 0x040007C8 RID: 1992
	public const ushort DRAFT_CARD = 41056;

	// Token: 0x040007C9 RID: 1993
	public const ushort DISCARD_DRAFT_CARD = 41057;

	// Token: 0x040007CA RID: 1994
	public const ushort BUILD_COST_FLAG = 32768;

	// Token: 0x040007CB RID: 1995
	public const ushort BUILD_COST_ROOMS = 4096;

	// Token: 0x040007CC RID: 1996
	public const ushort BUILD_COST_RENOVATE = 8192;

	// Token: 0x040007CD RID: 1997
	public const ushort BUILD_COST_IMPROVEMENT = 16384;
}
