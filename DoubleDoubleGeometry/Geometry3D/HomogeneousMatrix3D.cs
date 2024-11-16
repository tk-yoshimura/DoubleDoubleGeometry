using Algebra;
using DoubleDouble;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class HomogeneousMatrix3D : IMatrix<HomogeneousMatrix3D>, IFormattable {
        public readonly ddouble E00, E01, E02, E03, E10, E11, E12, E13, E20, E21, E22, E23, E30, E31, E32, E33;

        public HomogeneousMatrix3D(
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

            this.E03 = this.E13 = this.E23 = this.E30 = this.E31 = this.E32 = 0d;
            this.E33 = 1d;
        }

        public HomogeneousMatrix3D(
            ddouble e00, ddouble e01, ddouble e02, ddouble e03,
            ddouble e10, ddouble e11, ddouble e12, ddouble e13,
            ddouble e20, ddouble e21, ddouble e22, ddouble e23,
            ddouble e30, ddouble e31, ddouble e32, ddouble e33) {

            this.E00 = e00;
            this.E01 = e01;
            this.E02 = e02;
            this.E03 = e03;
            this.E10 = e10;
            this.E11 = e11;
            this.E12 = e12;
            this.E13 = e13;
            this.E20 = e20;
            this.E21 = e21;
            this.E22 = e22;
            this.E23 = e23;
            this.E30 = e30;
            this.E31 = e31;
            this.E32 = e32;
            this.E33 = e33;
        }

        public HomogeneousMatrix3D(Matrix m) {
            if (m.Shape != (4, 4)) {
                throw new ArgumentException("invalid shape", nameof(m));
            }

            this.E00 = m[0, 0];
            this.E01 = m[0, 1];
            this.E02 = m[0, 2];
            this.E03 = m[0, 3];
            this.E10 = m[1, 0];
            this.E11 = m[1, 1];
            this.E12 = m[1, 2];
            this.E13 = m[1, 3];
            this.E20 = m[2, 0];
            this.E21 = m[2, 1];
            this.E22 = m[2, 2];
            this.E23 = m[2, 3];
            this.E30 = m[3, 0];
            this.E31 = m[3, 1];
            this.E32 = m[3, 2];
            this.E33 = m[3, 3];
        }

        public static HomogeneousMatrix3D Transpose(HomogeneousMatrix3D m) {
            return new HomogeneousMatrix3D(
                m.E00, m.E10, m.E20, m.E30,
                m.E01, m.E11, m.E21, m.E31,
                m.E02, m.E12, m.E22, m.E32,
                m.E03, m.E13, m.E23, m.E33
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public HomogeneousMatrix3D T => Transpose(this);

        public static HomogeneousMatrix3D Invert(HomogeneousMatrix3D m) {
            return ((Matrix)m).Inverse;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public HomogeneousMatrix3D Inverse => Invert(this);

        public static HomogeneousMatrix3D operator +(HomogeneousMatrix3D m) {
            return m;
        }

        public static HomogeneousMatrix3D operator -(HomogeneousMatrix3D m) {
            return new HomogeneousMatrix3D(
                -m.E00, -m.E01, -m.E02, -m.E03,
                -m.E10, -m.E11, -m.E12, -m.E13,
                -m.E20, -m.E21, -m.E22, -m.E23,
                -m.E30, -m.E31, -m.E32, -m.E33
            );
        }

        public static HomogeneousMatrix3D operator +(HomogeneousMatrix3D m1, HomogeneousMatrix3D m2) {
            return new HomogeneousMatrix3D(
                m1.E00 + m2.E00, m1.E01 + m2.E01, m1.E02 + m2.E02, m1.E03 + m2.E03,
                m1.E10 + m2.E10, m1.E11 + m2.E11, m1.E12 + m2.E12, m1.E13 + m2.E13,
                m1.E20 + m2.E20, m1.E21 + m2.E21, m1.E22 + m2.E22, m1.E23 + m2.E23,
                m1.E30 + m2.E30, m1.E31 + m2.E31, m1.E32 + m2.E32, m1.E33 + m2.E33
            );
        }

        public static HomogeneousMatrix3D operator -(HomogeneousMatrix3D m1, HomogeneousMatrix3D m2) {
            return new HomogeneousMatrix3D(
                m1.E00 - m2.E00, m1.E01 - m2.E01, m1.E02 - m2.E02, m1.E03 - m2.E03,
                m1.E10 - m2.E10, m1.E11 - m2.E11, m1.E12 - m2.E12, m1.E13 - m2.E13,
                m1.E20 - m2.E20, m1.E21 - m2.E21, m1.E22 - m2.E22, m1.E23 - m2.E23,
                m1.E30 - m2.E30, m1.E31 - m2.E31, m1.E32 - m2.E32, m1.E33 - m2.E33
            );
        }

        public static HomogeneousMatrix3D operator *(HomogeneousMatrix3D m1, HomogeneousMatrix3D m2) {
            return new HomogeneousMatrix3D(
                m1.E00 * m2.E00 + m1.E01 * m2.E10 + m1.E02 * m2.E20 + m1.E03 * m2.E30,
                m1.E00 * m2.E01 + m1.E01 * m2.E11 + m1.E02 * m2.E21 + m1.E03 * m2.E31,
                m1.E00 * m2.E02 + m1.E01 * m2.E12 + m1.E02 * m2.E22 + m1.E03 * m2.E32,
                m1.E00 * m2.E03 + m1.E01 * m2.E13 + m1.E02 * m2.E23 + m1.E03 * m2.E33,

                m1.E10 * m2.E00 + m1.E11 * m2.E10 + m1.E12 * m2.E20 + m1.E13 * m2.E30,
                m1.E10 * m2.E01 + m1.E11 * m2.E11 + m1.E12 * m2.E21 + m1.E13 * m2.E31,
                m1.E10 * m2.E02 + m1.E11 * m2.E12 + m1.E12 * m2.E22 + m1.E13 * m2.E32,
                m1.E10 * m2.E03 + m1.E11 * m2.E13 + m1.E12 * m2.E23 + m1.E13 * m2.E33,

                m1.E20 * m2.E00 + m1.E21 * m2.E10 + m1.E22 * m2.E20 + m1.E23 * m2.E30,
                m1.E20 * m2.E01 + m1.E21 * m2.E11 + m1.E22 * m2.E21 + m1.E23 * m2.E31,
                m1.E20 * m2.E02 + m1.E21 * m2.E12 + m1.E22 * m2.E22 + m1.E23 * m2.E32,
                m1.E20 * m2.E03 + m1.E21 * m2.E13 + m1.E22 * m2.E23 + m1.E23 * m2.E33,

                m1.E30 * m2.E00 + m1.E31 * m2.E10 + m1.E32 * m2.E20 + m1.E33 * m2.E30,
                m1.E30 * m2.E01 + m1.E31 * m2.E11 + m1.E32 * m2.E21 + m1.E33 * m2.E31,
                m1.E30 * m2.E02 + m1.E31 * m2.E12 + m1.E32 * m2.E22 + m1.E33 * m2.E32,
                m1.E30 * m2.E03 + m1.E31 * m2.E13 + m1.E32 * m2.E23 + m1.E33 * m2.E33
            );
        }

        public static HomogeneousMatrix3D operator *(double r, HomogeneousMatrix3D m) {
            return new HomogeneousMatrix3D(
                m.E00 * r, m.E01 * r, m.E02 * r, m.E03 * r,
                m.E10 * r, m.E11 * r, m.E12 * r, m.E13 * r,
                m.E20 * r, m.E21 * r, m.E22 * r, m.E23 * r,
                m.E30 * r, m.E31 * r, m.E32 * r, m.E33 * r
            );
        }

        public static HomogeneousMatrix3D operator *(HomogeneousMatrix3D m, double r) {
            return r * m;
        }

        public static HomogeneousMatrix3D operator /(HomogeneousMatrix3D m, double r) {
            return 1d / r * m;
        }

        public static bool operator ==(HomogeneousMatrix3D m1, HomogeneousMatrix3D m2) {
            if (m1.E00 != m2.E00 || m1.E01 != m2.E01 || m1.E02 != m2.E02 || m1.E03 != m2.E03)
                return false;

            if (m1.E10 != m2.E10 || m1.E11 != m2.E11 || m1.E12 != m2.E12 || m1.E13 != m2.E13)
                return false;

            if (m1.E20 != m2.E20 || m1.E21 != m2.E21 || m1.E22 != m2.E22 || m1.E13 != m2.E13)
                return false;

            if (m1.E30 != m2.E30 || m1.E31 != m2.E31 || m1.E32 != m2.E32 || m1.E33 != m2.E33)
                return false;

            return true;
        }

        public static bool operator !=(HomogeneousMatrix3D m1, HomogeneousMatrix3D m2) {
            return !(m1 == m2);
        }

        public static implicit operator HomogeneousMatrix3D(Matrix3D m) {
            return new(m.E00, m.E01, m.E02, m.E10, m.E11, m.E12, m.E20, m.E21, m.E22);
        }

        public static HomogeneousMatrix3D Move(double mx, double my, double mz) {
            return new HomogeneousMatrix3D(1, 0, 0, mx, 0, 1, 0, my, 0, 0, 1, mz, 0, 0, 0, 1);
        }

        public static ddouble Det(HomogeneousMatrix3D m) {
            return ((Matrix)m).Det;
        }

        public static HomogeneousMatrix3D Zero { get; } = new(0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d);

        public static HomogeneousMatrix3D Identity { get; } = new(1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d);

        public static HomogeneousMatrix3D Invalid { get; } = new(
            ddouble.NaN, ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN, ddouble.NaN
        );

        public static implicit operator Matrix(HomogeneousMatrix3D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02, m.E03 },
                { m.E10, m.E11, m.E12, m.E13 },
                { m.E20, m.E21, m.E22, m.E23 },
                { m.E30, m.E31, m.E32, m.E33 },
            };
        }

        public static implicit operator HomogeneousMatrix3D(Matrix m) {
            return new(m);
        }

        public static implicit operator ddouble[,](HomogeneousMatrix3D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02, m.E03 },
                { m.E10, m.E11, m.E12, m.E13 },
                { m.E20, m.E21, m.E22, m.E23 },
                { m.E30, m.E31, m.E32, m.E33 },
            };
        }

        public static HomogeneousMatrix3D ScaleB(HomogeneousMatrix3D v, int n) {
            return new(
                ddouble.Ldexp(v.E00, n), ddouble.Ldexp(v.E01, n), ddouble.Ldexp(v.E02, n), ddouble.Ldexp(v.E03, n),
                ddouble.Ldexp(v.E10, n), ddouble.Ldexp(v.E11, n), ddouble.Ldexp(v.E12, n), ddouble.Ldexp(v.E13, n),
                ddouble.Ldexp(v.E20, n), ddouble.Ldexp(v.E21, n), ddouble.Ldexp(v.E22, n), ddouble.Ldexp(v.E23, n),
                ddouble.Ldexp(v.E30, n), ddouble.Ldexp(v.E31, n), ddouble.Ldexp(v.E32, n), ddouble.Ldexp(v.E33, n)
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
                if (ddouble.IsFinite(E03)) {
                    max_exponent = int.Max(ddouble.ILogB(E03), max_exponent);
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
                if (ddouble.IsFinite(E13)) {
                    max_exponent = int.Max(ddouble.ILogB(E13), max_exponent);
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
                if (ddouble.IsFinite(E23)) {
                    max_exponent = int.Max(ddouble.ILogB(E23), max_exponent);
                }
                if (ddouble.IsFinite(E30)) {
                    max_exponent = int.Max(ddouble.ILogB(E30), max_exponent);
                }
                if (ddouble.IsFinite(E31)) {
                    max_exponent = int.Max(ddouble.ILogB(E31), max_exponent);
                }
                if (ddouble.IsFinite(E32)) {
                    max_exponent = int.Max(ddouble.ILogB(E32), max_exponent);
                }
                if (ddouble.IsFinite(E33)) {
                    max_exponent = int.Max(ddouble.ILogB(E33), max_exponent);
                }

                return max_exponent;
            }
        }

        public static bool IsZero(HomogeneousMatrix3D m) {
            return
                ddouble.IsZero(m.E00) && ddouble.IsZero(m.E01) && ddouble.IsZero(m.E02) && ddouble.IsZero(m.E03) &&
                ddouble.IsZero(m.E10) && ddouble.IsZero(m.E11) && ddouble.IsZero(m.E12) && ddouble.IsZero(m.E13) &&
                ddouble.IsZero(m.E20) && ddouble.IsZero(m.E21) && ddouble.IsZero(m.E22) && ddouble.IsZero(m.E23) &&
                ddouble.IsZero(m.E30) && ddouble.IsZero(m.E31) && ddouble.IsZero(m.E32) && ddouble.IsZero(m.E33);
        }

        public static bool IsFinite(HomogeneousMatrix3D m) {
            return
                ddouble.IsFinite(m.E00) && ddouble.IsFinite(m.E01) && ddouble.IsFinite(m.E02) && ddouble.IsFinite(m.E03) &&
                ddouble.IsFinite(m.E10) && ddouble.IsFinite(m.E11) && ddouble.IsFinite(m.E12) && ddouble.IsFinite(m.E13) &&
                ddouble.IsFinite(m.E20) && ddouble.IsFinite(m.E21) && ddouble.IsFinite(m.E22) && ddouble.IsFinite(m.E23) &&
                ddouble.IsFinite(m.E30) && ddouble.IsFinite(m.E31) && ddouble.IsFinite(m.E32) && ddouble.IsFinite(m.E33);
        }

        public static bool IsInfinity(HomogeneousMatrix3D m) {
            return !IsNaN(m) && (
                ddouble.IsInfinity(m.E00) || ddouble.IsInfinity(m.E01) || ddouble.IsInfinity(m.E02) || ddouble.IsInfinity(m.E03) ||
                ddouble.IsInfinity(m.E10) || ddouble.IsInfinity(m.E11) || ddouble.IsInfinity(m.E12) || ddouble.IsInfinity(m.E13) ||
                ddouble.IsInfinity(m.E20) || ddouble.IsInfinity(m.E21) || ddouble.IsInfinity(m.E22) || ddouble.IsInfinity(m.E23) ||
                ddouble.IsInfinity(m.E30) || ddouble.IsInfinity(m.E31) || ddouble.IsInfinity(m.E32) || ddouble.IsInfinity(m.E33));
        }

        public static bool IsNaN(HomogeneousMatrix3D m) {
            return
                ddouble.IsNaN(m.E00) || ddouble.IsNaN(m.E01) || ddouble.IsNaN(m.E02) || ddouble.IsNaN(m.E03) ||
                ddouble.IsNaN(m.E10) || ddouble.IsNaN(m.E11) || ddouble.IsNaN(m.E12) || ddouble.IsNaN(m.E13) ||
                ddouble.IsNaN(m.E20) || ddouble.IsNaN(m.E21) || ddouble.IsNaN(m.E22) || ddouble.IsNaN(m.E23) ||
                ddouble.IsNaN(m.E30) || ddouble.IsNaN(m.E31) || ddouble.IsNaN(m.E32) || ddouble.IsNaN(m.E33);
        }

        public static bool IsIdentity(HomogeneousMatrix3D m) {
            return
                m.E00 == 1d && m.E01 == 0d && m.E02 == 0d && m.E03 == 0d &&
                m.E10 == 0d && m.E11 == 1d && m.E12 == 0d && m.E13 == 0d &&
                m.E20 == 0d && m.E21 == 0d && m.E22 == 1d && m.E23 == 0d &&
                m.E30 == 0d && m.E31 == 0d && m.E32 == 0d && m.E33 == 1d;
        }

        public static bool IsValid(HomogeneousMatrix3D v) {
            return IsFinite(v);
        }

        public override string ToString() {
            return
                $"[[{E00}, {E01}, {E02}, {E03}], " +
                $"[{E10}, {E11}, {E12}, {E13}], " +
                $"[{E20}, {E21}, {E22}, {E23}], " +
                $"[{E30}, {E31}, {E32}, {E33}]]";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return
                $"[[{E00.ToString(format)}, {E01.ToString(format)}, {E02.ToString(format)}, {E03.ToString(format)}], " +
                $"[{E10.ToString(format)}, {E11.ToString(format)}, {E12.ToString(format)}, {E03.ToString(format)}], " +
                $"[{E20.ToString(format)}, {E21.ToString(format)}, {E22.ToString(format)}, {E23.ToString(format)}], " +
                $"[{E30.ToString(format)}, {E31.ToString(format)}, {E32.ToString(format)}, {E33.ToString(format)}]]";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is HomogeneousMatrix3D matrix && matrix == this;
        }

        public bool Equals(HomogeneousMatrix3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return E00.GetHashCode() ^ E01.GetHashCode() ^ E02.GetHashCode() ^ E03.GetHashCode()
                 ^ E10.GetHashCode() ^ E11.GetHashCode() ^ E12.GetHashCode() ^ E13.GetHashCode()
                 ^ E20.GetHashCode() ^ E21.GetHashCode() ^ E22.GetHashCode() ^ E23.GetHashCode()
                 ^ E30.GetHashCode() ^ E31.GetHashCode() ^ E32.GetHashCode() ^ E33.GetHashCode();
        }
    }
}
