using Algebra;
using DoubleDouble;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class HomogeneousMatrix2D : IMatrix<HomogeneousMatrix2D>, IFormattable {
        public readonly ddouble E00, E01, E02, E10, E11, E12, E20, E21, E22;

        public HomogeneousMatrix2D(
            ddouble e00, ddouble e01,
            ddouble e10, ddouble e11) {

            this.E00 = e00;
            this.E01 = e01;
            this.E10 = e10;
            this.E11 = e11;

            this.E02 = this.E12 = this.E20 = this.E21 = 0d;
            this.E22 = 1d;
        }

        public HomogeneousMatrix2D(
            ddouble e00, ddouble e01, ddouble e02,
            ddouble e10, ddouble e11, ddouble e12,
            ddouble e20, ddouble e21, ddouble e22) {

            this.E00 = e00;
            this.E01 = e01;
            this.E02 = e02;
            this.E10 = e10;
            this.E11 = e11;
            this.E12 = e12;
            this.E20 = e20;
            this.E21 = e21;
            this.E22 = e22;
        }

        public HomogeneousMatrix2D(Matrix m) {
            if (m.Shape != (3, 3)) {
                throw new ArgumentException("invalid shape", nameof(m));
            }

            this.E00 = m[0, 0];
            this.E01 = m[0, 1];
            this.E02 = m[0, 2];
            this.E10 = m[1, 0];
            this.E11 = m[1, 1];
            this.E12 = m[1, 2];
            this.E20 = m[2, 0];
            this.E21 = m[2, 1];
            this.E22 = m[2, 2];
        }

        public static HomogeneousMatrix2D Transpose(HomogeneousMatrix2D m) {
            return new HomogeneousMatrix2D(
                m.E00, m.E10, m.E21,
                m.E01, m.E11, m.E21,
                m.E12, m.E12, m.E22
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public HomogeneousMatrix2D T => Transpose(this);

        public static HomogeneousMatrix2D Invert(HomogeneousMatrix2D m) {
            int exponent = m.MaxExponent;
            m = ScaleB(m, -exponent);

            return new HomogeneousMatrix2D(
                m.E11 * m.E22 - m.E12 * m.E21,
                m.E02 * m.E21 - m.E01 * m.E22,
                m.E01 * m.E12 - m.E02 * m.E11,
                m.E12 * m.E20 - m.E10 * m.E22,
                m.E00 * m.E22 - m.E02 * m.E20,
                m.E02 * m.E10 - m.E00 * m.E12,
                m.E10 * m.E21 - m.E11 * m.E20,
                m.E01 * m.E20 - m.E00 * m.E21,
                m.E00 * m.E11 - m.E01 * m.E10
            ) / ddouble.Ldexp(m.Det, exponent);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public HomogeneousMatrix2D Inverse => Invert(this);

        public static HomogeneousMatrix2D operator +(HomogeneousMatrix2D m) {
            return m;
        }

        public static HomogeneousMatrix2D operator -(HomogeneousMatrix2D m) {
            return new HomogeneousMatrix2D(
                -m.E00, -m.E01, -m.E02,
                -m.E10, -m.E11, -m.E12,
                -m.E20, -m.E21, -m.E22
            );
        }

        public static HomogeneousMatrix2D operator +(HomogeneousMatrix2D m1, HomogeneousMatrix2D m2) {
            return new HomogeneousMatrix2D(
                m1.E00 + m2.E00, m1.E01 + m2.E01, m1.E02 + m2.E02,
                m1.E10 + m2.E10, m1.E11 + m2.E11, m1.E12 + m2.E12,
                m1.E20 + m2.E20, m1.E21 + m2.E21, m1.E22 + m2.E22
            );
        }

        public static HomogeneousMatrix2D operator -(HomogeneousMatrix2D m1, HomogeneousMatrix2D m2) {
            return new HomogeneousMatrix2D(
                m1.E00 - m2.E00, m1.E01 - m2.E01, m1.E02 - m2.E02,
                m1.E10 - m2.E10, m1.E11 - m2.E11, m1.E12 - m2.E12,
                m1.E20 - m2.E20, m1.E21 - m2.E21, m1.E22 - m2.E22
            );
        }

        public static HomogeneousMatrix2D operator *(HomogeneousMatrix2D m1, HomogeneousMatrix2D m2) {
            return new HomogeneousMatrix2D(
                m1.E00 * m2.E00 + m1.E01 * m2.E10 + m1.E02 * m2.E21,
                m1.E00 * m2.E01 + m1.E01 * m2.E11 + m1.E02 * m2.E21,
                m1.E00 * m2.E12 + m1.E01 * m2.E12 + m1.E02 * m2.E22,

                m1.E10 * m2.E00 + m1.E11 * m2.E10 + m1.E12 * m2.E21,
                m1.E10 * m2.E01 + m1.E11 * m2.E11 + m1.E12 * m2.E21,
                m1.E10 * m2.E12 + m1.E11 * m2.E12 + m1.E12 * m2.E22,

                m1.E20 * m2.E00 + m1.E21 * m2.E10 + m1.E22 * m2.E21,
                m1.E20 * m2.E01 + m1.E21 * m2.E11 + m1.E22 * m2.E21,
                m1.E20 * m2.E12 + m1.E21 * m2.E12 + m1.E22 * m2.E22
            );
        }

        public static HomogeneousMatrix2D operator *(ddouble r, HomogeneousMatrix2D m) {
            return new HomogeneousMatrix2D(
                m.E00 * r, m.E01 * r, m.E02 * r,
                m.E10 * r, m.E11 * r, m.E12 * r,
                m.E20 * r, m.E21 * r, m.E22 * r
            );
        }

        public static HomogeneousMatrix2D operator *(double r, HomogeneousMatrix2D m) {
            return new HomogeneousMatrix2D(
                m.E00 * r, m.E01 * r, m.E02 * r,
                m.E10 * r, m.E11 * r, m.E12 * r,
                m.E20 * r, m.E21 * r, m.E22 * r
            );
        }

        public static HomogeneousMatrix2D operator *(HomogeneousMatrix2D m, ddouble r) {
            return r * m;
        }

        public static HomogeneousMatrix2D operator *(HomogeneousMatrix2D m, double r) {
            return r * m;
        }

        public static HomogeneousMatrix2D operator /(HomogeneousMatrix2D m, ddouble r) {
            return 1d / r * m;
        }

        public static HomogeneousMatrix2D operator /(HomogeneousMatrix2D m, double r) {
            return 1d / r * m;
        }

        public static bool operator ==(HomogeneousMatrix2D m1, HomogeneousMatrix2D m2) {
            if (m1.E00 != m2.E00 || m1.E01 != m2.E01 || m1.E02 != m2.E02)
                return false;

            if (m1.E10 != m2.E10 || m1.E11 != m2.E11 || m1.E12 != m2.E12)
                return false;

            if (m1.E20 != m2.E20 || m1.E21 != m2.E21 || m1.E22 != m2.E22)
                return false;

            return true;
        }

        public static bool operator !=(HomogeneousMatrix2D m1, HomogeneousMatrix2D m2) {
            return !(m1 == m2);
        }

        public static implicit operator HomogeneousMatrix2D(Matrix2D m) {
            return new(m.E00, m.E01, m.E10, m.E11);
        }

        public static HomogeneousMatrix2D Move(ddouble mx, ddouble my) {
            return new HomogeneousMatrix2D(1d, 0d, mx, 0d, 1d, my, 0d, 0d, 1d);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Det {
            get {
                ddouble det =
                    E00 * (E11 * E22 - E21 * E12) +
                    E10 * (E21 * E02 - E01 * E22) +
                    E20 * (E01 * E12 - E11 * E02);

                return det;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static HomogeneousMatrix2D Zero { get; } = new(0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static HomogeneousMatrix2D Identity { get; } = new(1d, 0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static HomogeneousMatrix2D Invalid { get; } = new(
            ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN
        );

        public static implicit operator Matrix(HomogeneousMatrix2D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02 },
                { m.E10, m.E11, m.E12 },
                { m.E20, m.E21, m.E22 }
            };
        }

        public static implicit operator HomogeneousMatrix2D(Matrix m) {
            return new(m);
        }

        public static implicit operator ddouble[,](HomogeneousMatrix2D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02 },
                { m.E10, m.E11, m.E12 },
                { m.E20, m.E21, m.E22 }
            };
        }

        public static HomogeneousMatrix2D ScaleB(HomogeneousMatrix2D v, int n) {
            return new(
                ddouble.Ldexp(v.E00, n), ddouble.Ldexp(v.E01, n), ddouble.Ldexp(v.E02, n),
                ddouble.Ldexp(v.E10, n), ddouble.Ldexp(v.E11, n), ddouble.Ldexp(v.E12, n),
                ddouble.Ldexp(v.E20, n), ddouble.Ldexp(v.E21, n), ddouble.Ldexp(v.E22, n)
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MaxExponent {
            get {
                int max_exponent = int.MinValue + 1; // abs(int.minvalue) throw arithmetic exception

                if (ddouble.IsFinite(E00)) {
                    max_exponent = int.Max(ddouble.ILogB(E00), max_exponent);
                }
                if (ddouble.IsFinite(E01)) {
                    max_exponent = int.Max(ddouble.ILogB(E01), max_exponent);
                }
                if (ddouble.IsFinite(E02)) {
                    max_exponent = int.Max(ddouble.ILogB(E02), max_exponent);
                }
                if (ddouble.IsFinite(E10)) {
                    max_exponent = int.Max(ddouble.ILogB(E10), max_exponent);
                }
                if (ddouble.IsFinite(E11)) {
                    max_exponent = int.Max(ddouble.ILogB(E11), max_exponent);
                }
                if (ddouble.IsFinite(E12)) {
                    max_exponent = int.Max(ddouble.ILogB(E12), max_exponent);
                }
                if (ddouble.IsFinite(E20)) {
                    max_exponent = int.Max(ddouble.ILogB(E20), max_exponent);
                }
                if (ddouble.IsFinite(E21)) {
                    max_exponent = int.Max(ddouble.ILogB(E21), max_exponent);
                }
                if (ddouble.IsFinite(E22)) {
                    max_exponent = int.Max(ddouble.ILogB(E22), max_exponent);
                }

                return max_exponent;
            }
        }

        public static bool IsZero(HomogeneousMatrix2D m) {
            return
                ddouble.IsZero(m.E00) && ddouble.IsZero(m.E01) && ddouble.IsZero(m.E02) &&
                ddouble.IsZero(m.E10) && ddouble.IsZero(m.E11) && ddouble.IsZero(m.E12) &&
                ddouble.IsZero(m.E20) && ddouble.IsZero(m.E21) && ddouble.IsZero(m.E22);
        }

        public static bool IsFinite(HomogeneousMatrix2D m) {
            return
                ddouble.IsFinite(m.E00) && ddouble.IsFinite(m.E01) && ddouble.IsFinite(m.E02) &&
                ddouble.IsFinite(m.E10) && ddouble.IsFinite(m.E11) && ddouble.IsFinite(m.E12) &&
                ddouble.IsFinite(m.E20) && ddouble.IsFinite(m.E21) && ddouble.IsFinite(m.E22);
        }

        public static bool IsInfinity(HomogeneousMatrix2D m) {
            return !IsNaN(m) && (
                ddouble.IsInfinity(m.E00) || ddouble.IsInfinity(m.E01) || ddouble.IsInfinity(m.E02) ||
                ddouble.IsInfinity(m.E10) || ddouble.IsInfinity(m.E11) || ddouble.IsInfinity(m.E12) ||
                ddouble.IsInfinity(m.E20) || ddouble.IsInfinity(m.E21) || ddouble.IsInfinity(m.E22));
        }

        public static bool IsNaN(HomogeneousMatrix2D m) {
            return
                ddouble.IsNaN(m.E00) || ddouble.IsNaN(m.E01) || ddouble.IsFinite(m.E02) ||
                ddouble.IsNaN(m.E10) || ddouble.IsNaN(m.E11) || ddouble.IsFinite(m.E12) ||
                ddouble.IsNaN(m.E20) || ddouble.IsNaN(m.E21) || ddouble.IsFinite(m.E22);
        }

        public static bool IsIdentity(HomogeneousMatrix2D m) {
            return
                m.E00 == 1d && m.E01 == 0d && m.E02 == 0d &&
                m.E10 == 0d && m.E11 == 1d && m.E12 == 0d &&
                m.E20 == 0d && m.E21 == 0d && m.E22 == 1d;
        }

        public static bool IsValid(HomogeneousMatrix2D v) {
            return IsFinite(v);
        }

        public override string ToString() {
            return
                $"[[{E00}, {E01}, {E02}], " +
                $"[{E10}, {E11}, {E12}], " +
                $"[{E20}, {E21}, {E22}]]";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return
                $"[[{E00.ToString(format)}, {E01.ToString(format)}, {E02.ToString(format)}], " +
                $"[{E10.ToString(format)}, {E11.ToString(format)}, {E12.ToString(format)}], " +
                $"[{E20.ToString(format)}, {E21.ToString(format)}, {E22.ToString(format)}]]";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is HomogeneousMatrix2D matrix && matrix == this;
        }

        public bool Equals(HomogeneousMatrix2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return E00.GetHashCode() ^ E01.GetHashCode() ^ E02.GetHashCode()
                 ^ E10.GetHashCode() ^ E11.GetHashCode() ^ E12.GetHashCode()
                 ^ E20.GetHashCode() ^ E21.GetHashCode() ^ E22.GetHashCode();
        }
    }
}
