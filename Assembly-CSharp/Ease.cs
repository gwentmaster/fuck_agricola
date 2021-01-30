using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public static class Ease
{
	// Token: 0x06000075 RID: 117 RVA: 0x00003430 File Offset: 0x00001630
	public static Easer FromType(EaseType type)
	{
		switch (type)
		{
		case EaseType.Linear:
			return Ease.Linear;
		case EaseType.QuadIn:
			return Ease.QuadIn;
		case EaseType.QuadOut:
			return Ease.QuadOut;
		case EaseType.QuadInOut:
			return Ease.QuadInOut;
		case EaseType.CubeIn:
			return Ease.CubeIn;
		case EaseType.CubeOut:
			return Ease.CubeOut;
		case EaseType.CubeInOut:
			return Ease.CubeInOut;
		case EaseType.BackIn:
			return Ease.BackIn;
		case EaseType.BackOut:
			return Ease.BackOut;
		case EaseType.BackInOut:
			return Ease.BackInOut;
		case EaseType.ExpoIn:
			return Ease.ExpoIn;
		case EaseType.ExpoOut:
			return Ease.ExpoOut;
		case EaseType.ExpoInOut:
			return Ease.ExpoInOut;
		case EaseType.SineIn:
			return Ease.SineIn;
		case EaseType.SineOut:
			return Ease.SineOut;
		case EaseType.SineInOut:
			return Ease.SineInOut;
		case EaseType.ElasticIn:
			return Ease.ElasticIn;
		case EaseType.ElasticOut:
			return Ease.ElasticOut;
		case EaseType.ElasticInOut:
			return Ease.ElasticInOut;
		default:
			return Ease.Linear;
		}
	}

	// Token: 0x04000026 RID: 38
	public static readonly Easer Linear = (float t) => t;

	// Token: 0x04000027 RID: 39
	public static readonly Easer QuadIn = (float t) => t * t;

	// Token: 0x04000028 RID: 40
	public static readonly Easer QuadOut = (float t) => 1f - Ease.QuadIn(1f - t);

	// Token: 0x04000029 RID: 41
	public static readonly Easer QuadInOut = delegate(float t)
	{
		if (t > 0.5f)
		{
			return Ease.QuadOut(t * 2f - 1f) / 2f + 0.5f;
		}
		return Ease.QuadIn(t * 2f) / 2f;
	};

	// Token: 0x0400002A RID: 42
	public static readonly Easer CubeIn = (float t) => t * t * t;

	// Token: 0x0400002B RID: 43
	public static readonly Easer CubeOut = (float t) => 1f - Ease.CubeIn(1f - t);

	// Token: 0x0400002C RID: 44
	public static readonly Easer CubeInOut = delegate(float t)
	{
		if (t > 0.5f)
		{
			return Ease.CubeOut(t * 2f - 1f) / 2f + 0.5f;
		}
		return Ease.CubeIn(t * 2f) / 2f;
	};

	// Token: 0x0400002D RID: 45
	public static readonly Easer BackIn = (float t) => t * t * (2.70158f * t - 1.70158f);

	// Token: 0x0400002E RID: 46
	public static readonly Easer BackOut = (float t) => 1f - Ease.BackIn(1f - t);

	// Token: 0x0400002F RID: 47
	public static readonly Easer BackInOut = delegate(float t)
	{
		if (t > 0.5f)
		{
			return Ease.BackOut(t * 2f - 1f) / 2f + 0.5f;
		}
		return Ease.BackIn(t * 2f) / 2f;
	};

	// Token: 0x04000030 RID: 48
	public static readonly Easer ExpoIn = (float t) => Mathf.Pow(2f, 10f * (t - 1f));

	// Token: 0x04000031 RID: 49
	public static readonly Easer ExpoOut = (float t) => 1f - Ease.ExpoIn(t);

	// Token: 0x04000032 RID: 50
	public static readonly Easer ExpoInOut = delegate(float t)
	{
		if (t >= 0.5f)
		{
			return Ease.ExpoOut(t * 2f) / 2f;
		}
		return Ease.ExpoIn(t * 2f) / 2f;
	};

	// Token: 0x04000033 RID: 51
	public static readonly Easer SineIn = (float t) => -Mathf.Cos(1.5707964f * t) + 1f;

	// Token: 0x04000034 RID: 52
	public static readonly Easer SineOut = (float t) => Mathf.Sin(1.5707964f * t);

	// Token: 0x04000035 RID: 53
	public static readonly Easer SineInOut = (float t) => -Mathf.Cos(3.1415927f * t) / 2f + 0.5f;

	// Token: 0x04000036 RID: 54
	public static readonly Easer ElasticIn = (float t) => 1f - Ease.ElasticOut(1f - t);

	// Token: 0x04000037 RID: 55
	public static readonly Easer ElasticOut = (float t) => Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - 0.075f) * 6.2831855f / 0.3f) + 1f;

	// Token: 0x04000038 RID: 56
	public static readonly Easer ElasticInOut = delegate(float t)
	{
		if (t > 0.5f)
		{
			return Ease.ElasticOut(t * 2f - 1f) / 2f + 0.5f;
		}
		return Ease.ElasticIn(t * 2f) / 2f;
	};
}
