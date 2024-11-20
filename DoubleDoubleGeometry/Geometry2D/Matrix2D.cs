using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Matrix2D : IMatrix<Matrix2D>, IFormattable {
        public readonly ddouble E00, E01, E10, E11;

        public Matrix2D(
            ddouble e00, ddouble e01,
            ddouble e10, ddouble e11) {

            this.E00 = e00;
            this.E01 = e01;
            this.E10 = e10;
            this.E11 = e11;
        }

        public Matrix2D(Complex c) {
            this.E00 = c.R;
            this.E01 = -c.I;
            this.E10 = c.I;
            this.E11 = c.R;
        }

        public Matrix2D(Matrix m) {
            if (m.Shape != (2, 2)) {
                throw new ArgumentException("invalid shape", nameof(m));
            }

            this.E00 = m[0, 0];
            this.E01 = m[0, 1];
            this.E10 = m[1, 0];
            this.E11 = m[1, 1];
        }

        public static Matrix2D Transpose(Matrix2D m) {
            return new Matrix2D(
                m.E00, m.E10,
                m.E01, m.E11
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Matrix2D T => Transpose(this);

        public static Matrix2D Invert(Matrix2D m) {
            int exponent = m.MaxExponent;
            m = ScaleB(m, -exponent);

            return new Matrix2D(
                m.E11, -m.E01,
                -m.E10, m.E00
            ) / ddouble.Ldexp(m.Det, exponent);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Matrix2D Inverse => Invert(this);

        public static Matrix2D operator +(Matrix2D m) {
            return m;
        }

        public static Matrix2D operator -(Matrix2D m) {
            return new Matrix2D(
                -m.E00, -m.E01,
                -m.E10, -m.E11
            );
        }

        public static Matrix2D operator +(Matrix2D m1, Matrix2D m2) {
            return new Matrix2D(
                m1.E00 + m2.E00, m1.E01 + m2.E01,
                m1.E10 + m2.E10, m1.E11 + m2.E11
            );
        }

        public static Matrix2D operator -(Matrix2D m1, Matrix2D m2) {
            return new Matrix2D(
                m1.E00 - m2.E00, m1.E01 - m2.E01,
                m1.E10 - m2.E10, m1.E11 - m2.E11
            );
        }

        public static Matrix2D operator *(Matrix2D m1, Matrix2D m2) {
            return new Matrix2D(
                m1.E00 * m2.E00 + m1.E01 * m2.E10,
                m1.E00 * m2.E01 + m1.E01 * m2.E11,

                m1.E10 * m2.E00 + m1.E11 * m2.E10,
                m1.E10 * m2.E01 + m1.E11 * m2.E11
            );
        }

        public static Matrix2D operator *(ddouble r, Matrix2D m) {
            return new Matrix2D(
                m.E00 * r, m.E01 * r,
                m.E10 * r, m.E11 * r
            );
        }

        public static Matrix2D operator *(double r, Matrix2D m) {
            return new Matrix2D(
                m.E00 * r, m.E01 * r,
                m.E10 * r, m.E11 * r
            );
        }

        public static Matrix2D operator *(Matrix2D m, ddouble r) {
            return r * m;
        }

        public static Matrix2D operator *(Matrix2D m, double r) {
            return r * m;
        }

        public static Matrix2D operator /(Matrix2D m, ddouble r) {
            return 1d / r * m;
        }

        public static Matrix2D operator /(Matrix2D m, double r) {
            return 1d / r * m;
        }

        public static bool operator ==(Matrix2D m1, Matrix2D m2) {
            if (m1.E00 != m2.E00 || m1.E01 != m2.E01)
                return false;

            if (m1.E10 != m2.E10 || m1.E11 != m2.E11)
                return false;

            return true;
        }

        public static bool operator !=(Matrix2D m1, Matrix2D m2) {
            return !(m1 == m2);
        }

        public static Matrix2D Rotate(ddouble theta) {
            ddouble cs = ddouble.Cos(theta), sn = ddouble.Sin(theta);
            return new Matrix2D(cs, -sn, sn, cs);
        }

        public static Matrix2D Scale(ddouble sx, ddouble sy) {
            return new Matrix2D(sx, 0d, 0d, sy);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Det {
            get {
                ddouble det = E00 * E11 - E01 * E10;

                return det;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Matrix2D Zero { get; } = new(0d, 0d, 0d, 0d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Matrix2D Identity { get; } = new(1d, 0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Matrix2D Invalid { get; } = new(
            ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN
        );

        public static implicit operator Matrix(Matrix2D m) {
            return new ddouble[,] {
                { m.E00, m.E01 },
                { m.E10, m.E11 }
            };
        }

        public static implicit operator Matrix2D(Matrix m) {
            return new(m);
        }

        public static explicit operator Matrix2D(Matrix3D m) {
            return new(
                m.E00, m.E01,
                m.E10, m.E11
            );
        }

        public static implicit operator ddouble[,](Matrix2D m) {
            return new ddouble[,] {
                { m.E00, m.E01 },
                { m.E10, m.E11 }
            };
        }

        public static implicit operator Matrix2D(ddouble[,] m) {
            if ((m.GetLength(0), m.GetLength(1)) != (2, 2)) {
                throw new ArgumentException("invalid shape", nameof(m));
            }

            return new(
                m[0, 0], m[0, 1],
                m[1, 0], m[1, 1]
            );
        }

        public static Matrix2D ScaleB(Matrix2D v, int n) {
            return new(
                ddouble.Ldexp(v.E00, n), ddouble.Ldexp(v.E01, n),
                ddouble.Ldexp(v.E10, n), ddouble.Ldexp(v.E11, n)
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
                if (ddouble.IsFinite(E10)) {
                    max_exponent = int.Max(ddouble.ILogB(E10), max_exponent);
                }
                if (ddouble.IsFinite(E11)) {
                    max_exponent = int.Max(ddouble.ILogB(E11), max_exponent);
                }

                return max_exponent;
            }
        }

        public static bool IsZero(Matrix2D m) {
            return
                ddouble.IsZero(m.E00) && ddouble.IsZero(m.E01) &&
                ddouble.IsZero(m.E10) && ddouble.IsZero(m.E11);
        }

        public static bool IsFinite(Matrix2D m) {
            return
                ddouble.IsFinite(m.E00) && ddouble.IsFinite(m.E01) &&
                ddouble.IsFinite(m.E10) && ddouble.IsFinite(m.E11);
        }

        public static bool IsInfinity(Matrix2D m) {
            return !IsNaN(m) && (
                ddouble.IsInfinity(m.E00) || ddouble.IsInfinity(m.E01) ||
                ddouble.IsInfinity(m.E10) || ddouble.IsInfinity(m.E11));
        }

        public static bool IsNaN(Matrix2D m) {
            return
                ddouble.IsNaN(m.E00) || ddouble.IsNaN(m.E01) ||
                ddouble.IsNaN(m.E10) || ddouble.IsNaN(m.E11);
        }

        public static bool IsIdentity(Matrix2D m) {
            return
                m.E00 == 1d && m.E01 == 0d &&
                m.E10 == 0d && m.E11 == 1d;
        }

        public static bool IsValid(Matrix2D v) {
            return IsFinite(v);
        }

        public override string ToString() {
            return
                $"[[{E00}, {E01}], " +
                $"[{E10}, {E11}]]";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return
                $"[[{E00.ToString(format)}, {E01.ToString(format)}], " +
                $"[{E10.ToString(format)}, {E11.ToString(format)}]]";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Matrix2D m && m == this);
        }

        public bool Equals(Matrix2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return E00.GetHashCode() ^ E01.GetHashCode()
                 ^ E10.GetHashCode() ^ E11.GetHashCode();
        }
    }

    public static class Matrix2DIOExpand {

        public static void Write(this BinaryWriter writer, Matrix2D m) {
            DoubleDoubleIOExpand.Write(writer, m.E00);
            DoubleDoubleIOExpand.Write(writer, m.E01);
            DoubleDoubleIOExpand.Write(writer, m.E10);
            DoubleDoubleIOExpand.Write(writer, m.E11);
        }

        public static Matrix2D ReadMatrix2D(this BinaryReader reader) {
            ddouble e00 = reader.ReadDDouble();
            ddouble e01 = reader.ReadDDouble();
            ddouble e10 = reader.ReadDDouble();
            ddouble e11 = reader.ReadDDouble();

            return new(e00, e01, e10, e11);
        }
    }
}
