using System;
using System.Collections;
using Org.BouncyCastle.Math.EC.Endo;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Math.Field;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002DB RID: 731
	public abstract class ECCurve
	{
		// Token: 0x06001925 RID: 6437 RVA: 0x000934B0 File Offset: 0x000916B0
		public static int[] GetAllCoordinateSystems()
		{
			return new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7
			};
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x000934C3 File Offset: 0x000916C3
		protected ECCurve(IFiniteField field)
		{
			this.m_field = field;
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06001927 RID: 6439
		public abstract int FieldSize { get; }

		// Token: 0x06001928 RID: 6440
		public abstract ECFieldElement FromBigInteger(BigInteger x);

		// Token: 0x06001929 RID: 6441
		public abstract bool IsValidFieldElement(BigInteger x);

		// Token: 0x0600192A RID: 6442 RVA: 0x000934D2 File Offset: 0x000916D2
		public virtual ECCurve.Config Configure()
		{
			return new ECCurve.Config(this, this.m_coord, this.m_endomorphism, this.m_multiplier);
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x000934EC File Offset: 0x000916EC
		public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y)
		{
			ECPoint ecpoint = this.CreatePoint(x, y);
			if (!ecpoint.IsValid())
			{
				throw new ArgumentException("Invalid point coordinates");
			}
			return ecpoint;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00093509 File Offset: 0x00091709
		public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			ECPoint ecpoint = this.CreatePoint(x, y, withCompression);
			if (!ecpoint.IsValid())
			{
				throw new ArgumentException("Invalid point coordinates");
			}
			return ecpoint;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x00093527 File Offset: 0x00091727
		public virtual ECPoint CreatePoint(BigInteger x, BigInteger y)
		{
			return this.CreatePoint(x, y, false);
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00093532 File Offset: 0x00091732
		public virtual ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			return this.CreateRawPoint(this.FromBigInteger(x), this.FromBigInteger(y), withCompression);
		}

		// Token: 0x0600192F RID: 6447
		protected abstract ECCurve CloneCurve();

		// Token: 0x06001930 RID: 6448
		protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression);

		// Token: 0x06001931 RID: 6449
		protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression);

		// Token: 0x06001932 RID: 6450 RVA: 0x0009354C File Offset: 0x0009174C
		protected virtual ECMultiplier CreateDefaultMultiplier()
		{
			GlvEndomorphism glvEndomorphism = this.m_endomorphism as GlvEndomorphism;
			if (glvEndomorphism != null)
			{
				return new GlvMultiplier(this, glvEndomorphism);
			}
			return new WNafL2RMultiplier();
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00093575 File Offset: 0x00091775
		public virtual bool SupportsCoordinateSystem(int coord)
		{
			return coord == 0;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0009357C File Offset: 0x0009177C
		public virtual PreCompInfo GetPreCompInfo(ECPoint point, string name)
		{
			this.CheckPoint(point);
			PreCompInfo result;
			lock (point)
			{
				IDictionary preCompTable = point.m_preCompTable;
				result = ((preCompTable == null) ? null : ((PreCompInfo)preCompTable[name]));
			}
			return result;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000935D4 File Offset: 0x000917D4
		public virtual void SetPreCompInfo(ECPoint point, string name, PreCompInfo preCompInfo)
		{
			this.CheckPoint(point);
			lock (point)
			{
				IDictionary dictionary = point.m_preCompTable;
				if (dictionary == null)
				{
					dictionary = (point.m_preCompTable = Platform.CreateHashtable(4));
				}
				dictionary[name] = preCompInfo;
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x00093630 File Offset: 0x00091830
		public virtual ECPoint ImportPoint(ECPoint p)
		{
			if (this == p.Curve)
			{
				return p;
			}
			if (p.IsInfinity)
			{
				return this.Infinity;
			}
			p = p.Normalize();
			return this.ValidatePoint(p.XCoord.ToBigInteger(), p.YCoord.ToBigInteger(), p.IsCompressed);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00093681 File Offset: 0x00091881
		public virtual void NormalizeAll(ECPoint[] points)
		{
			this.NormalizeAll(points, 0, points.Length, null);
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00093690 File Offset: 0x00091890
		public virtual void NormalizeAll(ECPoint[] points, int off, int len, ECFieldElement iso)
		{
			this.CheckPoints(points, off, len);
			int coordinateSystem = this.CoordinateSystem;
			if (coordinateSystem == 0 || coordinateSystem == 5)
			{
				if (iso != null)
				{
					throw new ArgumentException("not valid for affine coordinates", "iso");
				}
				return;
			}
			else
			{
				ECFieldElement[] array = new ECFieldElement[len];
				int[] array2 = new int[len];
				int num = 0;
				for (int i = 0; i < len; i++)
				{
					ECPoint ecpoint = points[off + i];
					if (ecpoint != null && (iso != null || !ecpoint.IsNormalized()))
					{
						array[num] = ecpoint.GetZCoord(0);
						array2[num++] = off + i;
					}
				}
				if (num == 0)
				{
					return;
				}
				ECAlgorithms.MontgomeryTrick(array, 0, num, iso);
				for (int j = 0; j < num; j++)
				{
					int num2 = array2[j];
					points[num2] = points[num2].Normalize(array[j]);
				}
				return;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06001939 RID: 6457
		public abstract ECPoint Infinity { get; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x0009374D File Offset: 0x0009194D
		public virtual IFiniteField Field
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x00093755 File Offset: 0x00091955
		public virtual ECFieldElement A
		{
			get
			{
				return this.m_a;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x0009375D File Offset: 0x0009195D
		public virtual ECFieldElement B
		{
			get
			{
				return this.m_b;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00093765 File Offset: 0x00091965
		public virtual BigInteger Order
		{
			get
			{
				return this.m_order;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x0009376D File Offset: 0x0009196D
		public virtual BigInteger Cofactor
		{
			get
			{
				return this.m_cofactor;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00093775 File Offset: 0x00091975
		public virtual int CoordinateSystem
		{
			get
			{
				return this.m_coord;
			}
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0009377D File Offset: 0x0009197D
		protected virtual void CheckPoint(ECPoint point)
		{
			if (point == null || this != point.Curve)
			{
				throw new ArgumentException("must be non-null and on this curve", "point");
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0009379B File Offset: 0x0009199B
		protected virtual void CheckPoints(ECPoint[] points)
		{
			this.CheckPoints(points, 0, points.Length);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000937A8 File Offset: 0x000919A8
		protected virtual void CheckPoints(ECPoint[] points, int off, int len)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (off < 0 || len < 0 || off > points.Length - len)
			{
				throw new ArgumentException("invalid range specified", "points");
			}
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				if (ecpoint != null && this != ecpoint.Curve)
				{
					throw new ArgumentException("entries must be null or on this curve", "points");
				}
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00093814 File Offset: 0x00091A14
		public virtual bool Equals(ECCurve other)
		{
			return this == other || (other != null && (this.Field.Equals(other.Field) && this.A.ToBigInteger().Equals(other.A.ToBigInteger())) && this.B.ToBigInteger().Equals(other.B.ToBigInteger()));
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00093879 File Offset: 0x00091A79
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECCurve);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00093887 File Offset: 0x00091A87
		public override int GetHashCode()
		{
			return this.Field.GetHashCode() ^ Integers.RotateLeft(this.A.ToBigInteger().GetHashCode(), 8) ^ Integers.RotateLeft(this.B.ToBigInteger().GetHashCode(), 16);
		}

		// Token: 0x06001946 RID: 6470
		protected abstract ECPoint DecompressPoint(int yTilde, BigInteger X1);

		// Token: 0x06001947 RID: 6471 RVA: 0x000938C3 File Offset: 0x00091AC3
		public virtual ECEndomorphism GetEndomorphism()
		{
			return this.m_endomorphism;
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x000938CC File Offset: 0x00091ACC
		public virtual ECMultiplier GetMultiplier()
		{
			ECMultiplier multiplier;
			lock (this)
			{
				if (this.m_multiplier == null)
				{
					this.m_multiplier = this.CreateDefaultMultiplier();
				}
				multiplier = this.m_multiplier;
			}
			return multiplier;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00093920 File Offset: 0x00091B20
		public virtual ECPoint DecodePoint(byte[] encoded)
		{
			int num = (this.FieldSize + 7) / 8;
			byte b = encoded[0];
			ECPoint ecpoint;
			switch (b)
			{
			case 0:
				if (encoded.Length != 1)
				{
					throw new ArgumentException("Incorrect length for infinity encoding", "encoded");
				}
				ecpoint = this.Infinity;
				goto IL_157;
			case 2:
			case 3:
			{
				if (encoded.Length != num + 1)
				{
					throw new ArgumentException("Incorrect length for compressed encoding", "encoded");
				}
				int yTilde = (int)(b & 1);
				BigInteger x = new BigInteger(1, encoded, 1, num);
				ecpoint = this.DecompressPoint(yTilde, x);
				if (!ecpoint.SatisfiesCofactor())
				{
					throw new ArgumentException("Invalid point");
				}
				goto IL_157;
			}
			case 4:
			{
				if (encoded.Length != 2 * num + 1)
				{
					throw new ArgumentException("Incorrect length for uncompressed encoding", "encoded");
				}
				BigInteger x2 = new BigInteger(1, encoded, 1, num);
				BigInteger y = new BigInteger(1, encoded, 1 + num, num);
				ecpoint = this.ValidatePoint(x2, y);
				goto IL_157;
			}
			case 6:
			case 7:
			{
				if (encoded.Length != 2 * num + 1)
				{
					throw new ArgumentException("Incorrect length for hybrid encoding", "encoded");
				}
				BigInteger x3 = new BigInteger(1, encoded, 1, num);
				BigInteger bigInteger = new BigInteger(1, encoded, 1 + num, num);
				if (bigInteger.TestBit(0) != (b == 7))
				{
					throw new ArgumentException("Inconsistent Y coordinate in hybrid encoding", "encoded");
				}
				ecpoint = this.ValidatePoint(x3, bigInteger);
				goto IL_157;
			}
			}
			throw new FormatException("Invalid point encoding " + b);
			IL_157:
			if (b != 0 && ecpoint.IsInfinity)
			{
				throw new ArgumentException("Invalid infinity encoding", "encoded");
			}
			return ecpoint;
		}

		// Token: 0x0400155A RID: 5466
		public const int COORD_AFFINE = 0;

		// Token: 0x0400155B RID: 5467
		public const int COORD_HOMOGENEOUS = 1;

		// Token: 0x0400155C RID: 5468
		public const int COORD_JACOBIAN = 2;

		// Token: 0x0400155D RID: 5469
		public const int COORD_JACOBIAN_CHUDNOVSKY = 3;

		// Token: 0x0400155E RID: 5470
		public const int COORD_JACOBIAN_MODIFIED = 4;

		// Token: 0x0400155F RID: 5471
		public const int COORD_LAMBDA_AFFINE = 5;

		// Token: 0x04001560 RID: 5472
		public const int COORD_LAMBDA_PROJECTIVE = 6;

		// Token: 0x04001561 RID: 5473
		public const int COORD_SKEWED = 7;

		// Token: 0x04001562 RID: 5474
		protected readonly IFiniteField m_field;

		// Token: 0x04001563 RID: 5475
		protected ECFieldElement m_a;

		// Token: 0x04001564 RID: 5476
		protected ECFieldElement m_b;

		// Token: 0x04001565 RID: 5477
		protected BigInteger m_order;

		// Token: 0x04001566 RID: 5478
		protected BigInteger m_cofactor;

		// Token: 0x04001567 RID: 5479
		protected int m_coord;

		// Token: 0x04001568 RID: 5480
		protected ECEndomorphism m_endomorphism;

		// Token: 0x04001569 RID: 5481
		protected ECMultiplier m_multiplier;

		// Token: 0x02000881 RID: 2177
		public class Config
		{
			// Token: 0x06004554 RID: 17748 RVA: 0x001448A5 File Offset: 0x00142AA5
			internal Config(ECCurve outer, int coord, ECEndomorphism endomorphism, ECMultiplier multiplier)
			{
				this.outer = outer;
				this.coord = coord;
				this.endomorphism = endomorphism;
				this.multiplier = multiplier;
			}

			// Token: 0x06004555 RID: 17749 RVA: 0x001448CA File Offset: 0x00142ACA
			public ECCurve.Config SetCoordinateSystem(int coord)
			{
				this.coord = coord;
				return this;
			}

			// Token: 0x06004556 RID: 17750 RVA: 0x001448D4 File Offset: 0x00142AD4
			public ECCurve.Config SetEndomorphism(ECEndomorphism endomorphism)
			{
				this.endomorphism = endomorphism;
				return this;
			}

			// Token: 0x06004557 RID: 17751 RVA: 0x001448DE File Offset: 0x00142ADE
			public ECCurve.Config SetMultiplier(ECMultiplier multiplier)
			{
				this.multiplier = multiplier;
				return this;
			}

			// Token: 0x06004558 RID: 17752 RVA: 0x001448E8 File Offset: 0x00142AE8
			public ECCurve Create()
			{
				if (!this.outer.SupportsCoordinateSystem(this.coord))
				{
					throw new InvalidOperationException("unsupported coordinate system");
				}
				ECCurve eccurve = this.outer.CloneCurve();
				if (eccurve == this.outer)
				{
					throw new InvalidOperationException("implementation returned current curve");
				}
				eccurve.m_coord = this.coord;
				eccurve.m_endomorphism = this.endomorphism;
				eccurve.m_multiplier = this.multiplier;
				return eccurve;
			}

			// Token: 0x04002F5A RID: 12122
			protected ECCurve outer;

			// Token: 0x04002F5B RID: 12123
			protected int coord;

			// Token: 0x04002F5C RID: 12124
			protected ECEndomorphism endomorphism;

			// Token: 0x04002F5D RID: 12125
			protected ECMultiplier multiplier;
		}
	}
}
