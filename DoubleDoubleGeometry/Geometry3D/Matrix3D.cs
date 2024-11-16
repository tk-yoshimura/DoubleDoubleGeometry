using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Matrix3D : IMatrix<Matrix3D>, IFormattable {
        public readonly ddouble E00, E01, E02, E10, E11, E12, E20, E21, E22;

        public Matrix3D(
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

        public Matrix3D(Quaternion q) {
            ddouble r2 = q.R * q.R, i2 = q.I * q.I, j2 = q.J * q.J, k2 = q.K * q.K;
            ddouble ri = q.R * q.I, rj = q.R * q.J, rk = q.R * q.K;
            ddouble ij = q.I * q.J, ik = q.I * q.K, jk = q.J * q.K;

            this.E00 = r2 + i2 - j2 - k2;
            this.E11 = r2 - i2 + j2 - k2;
            this.E22 = r2 - i2 - j2 + k2;

            this.E01 = 2d * (ij - rk);
            this.E02 = 2d * (rj + ik);
            this.E10 = 2d * (rk + ij);
            this.E12 = 2d * (jk - ri);
            this.E20 = 2d * (ik - rj);
            this.E21 = 2d * (ri + jk);
        }

        public Matrix3D(Matrix m) {
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

        public static Matrix3D Transpose(Matrix3D m) {
            return new Matrix3D(
                m.E00, m.E10, m.E20,
                m.E01, m.E11, m.E21,
                m.E02, m.E12, m.E22
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Matrix3D T => Transpose(this);

        public static Matrix3D Invert(Matrix3D m) {
            int exponent = m.MaxExponent;
            m = ScaleB(m, -exponent);

            return new Matrix3D(
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
        public Matrix3D Inverse => Invert(this);

        public static Matrix3D operator +(Matrix3D m) {
            return m;
        }

        public static Matrix3D operator -(Matrix3D m) {
            return new Matrix3D(
                -m.E00, -m.E01, -m.E02,
                -m.E10, -m.E11, -m.E12,
                -m.E20, -m.E21, -m.E22
            );
        }

        public static Matrix3D operator +(Matrix3D m1, Matrix3D m2) {
            return new Matrix3D(
                m1.E00 + m2.E00, m1.E01 + m2.E01, m1.E02 + m2.E02,
                m1.E10 + m2.E10, m1.E11 + m2.E11, m1.E12 + m2.E12,
                m1.E20 + m2.E20, m1.E21 + m2.E21, m1.E22 + m2.E22
            );
        }

        public static Matrix3D operator -(Matrix3D m1, Matrix3D m2) {
            return new Matrix3D(
                m1.E00 - m2.E00, m1.E01 - m2.E01, m1.E02 - m2.E02,
                m1.E10 - m2.E10, m1.E11 - m2.E11, m1.E12 - m2.E12,
                m1.E20 - m2.E20, m1.E21 - m2.E21, m1.E22 - m2.E22
            );
        }

        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2) {
            return new Matrix3D(
                m1.E00 * m2.E00 + m1.E01 * m2.E10 + m1.E02 * m2.E20,
                m1.E00 * m2.E01 + m1.E01 * m2.E11 + m1.E02 * m2.E21,
                m1.E00 * m2.E02 + m1.E01 * m2.E12 + m1.E02 * m2.E22,

                m1.E10 * m2.E00 + m1.E11 * m2.E10 + m1.E12 * m2.E20,
                m1.E10 * m2.E01 + m1.E11 * m2.E11 + m1.E12 * m2.E21,
                m1.E10 * m2.E02 + m1.E11 * m2.E12 + m1.E12 * m2.E22,

                m1.E20 * m2.E00 + m1.E21 * m2.E10 + m1.E22 * m2.E20,
                m1.E20 * m2.E01 + m1.E21 * m2.E11 + m1.E22 * m2.E21,
                m1.E20 * m2.E02 + m1.E21 * m2.E12 + m1.E22 * m2.E22
            );
        }

        public static Matrix3D operator *(ddouble r, Matrix3D m) {
            return new Matrix3D(
                m.E00 * r, m.E01 * r, m.E02 * r,
                m.E10 * r, m.E11 * r, m.E12 * r,
                m.E20 * r, m.E21 * r, m.E22 * r
            );
        }

        public static Matrix3D operator *(double r, Matrix3D m) {
            return new Matrix3D(
                m.E00 * r, m.E01 * r, m.E02 * r,
                m.E10 * r, m.E11 * r, m.E12 * r,
                m.E20 * r, m.E21 * r, m.E22 * r
            );
        }

        public static Matrix3D operator *(Matrix3D m, ddouble r) {
            return r * m;
        }

        public static Matrix3D operator *(Matrix3D m, double r) {
            return r * m;
        }

        public static Matrix3D operator /(Matrix3D m, ddouble r) {
            return 1d / r * m;
        }

        public static Matrix3D operator /(Matrix3D m, double r) {
            return 1d / r * m;
        }

        public static bool operator ==(Matrix3D m1, Matrix3D m2) {
            if (m1.E00 != m2.E00 || m1.E01 != m2.E01 || m1.E02 != m2.E02)
                return false;

            if (m1.E10 != m2.E10 || m1.E11 != m2.E11 || m1.E12 != m2.E12)
                return false;

            if (m1.E20 != m2.E20 || m1.E21 != m2.E21 || m1.E22 != m2.E22)
                return false;

            return true;
        }

        public static bool operator !=(Matrix3D m1, Matrix3D m2) {
            return !(m1 == m2);
        }

        public static Matrix3D RotateX(ddouble theta) {
            ddouble cs = ddouble.Cos(theta), sn = ddouble.Sin(theta);
            return new Matrix3D(1, 0, 0, 0, +cs, -sn, 0, +sn, +cs);
        }

        public static Matrix3D RotateY(ddouble theta) {
            ddouble cs = ddouble.Cos(theta), sn = ddouble.Sin(theta);
            return new Matrix3D(+cs, 0, +sn, 0, 1, 0, -sn, 0, +cs);
        }

        public static Matrix3D RotateZ(ddouble theta) {
            ddouble cs = ddouble.Cos(theta), sn = ddouble.Sin(theta);
            return new Matrix3D(+cs, -sn, 0, +sn, +cs, 0, 0, 0, 1);
        }

        public static Matrix3D Rotate(ddouble roll, ddouble pitch, ddouble yaw) {
            return RotateX(roll) * RotateY(pitch) * RotateZ(yaw);
        }

        public static Matrix3D RotateAxis(Vector3D axis, ddouble theta) {
            axis = axis.Normal;

            ddouble cs = ddouble.Cos(theta), dcs = 1d - cs, sn = ddouble.Sin(theta);
            ddouble xx, xy, xz, yx, yy, yz, zx, zy, zz;
            ddouble nx = axis.X, ny = axis.Y, nz = axis.Z;

            xx = nx * nx * dcs + cs;
            xy = nx * ny * dcs - nz * sn;
            xz = nx * nz * dcs + ny * sn;

            yx = ny * nx * dcs + nz * sn;
            yy = ny * ny * dcs + cs;
            yz = ny * nz * dcs - nx * sn;

            zx = nz * nx * dcs - ny * sn;
            zy = nz * ny * dcs + nx * sn;
            zz = nz * nz * dcs + cs;

            return new Matrix3D(xx, xy, xz, yx, yy, yz, zx, zy, zz);
        }

        public static Matrix3D Scale(double sx, double sy, double sz) {
            return new Matrix3D(sx, 0d, 0d, 0d, sy, 0d, 0d, 0d, sz);
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
        public static Matrix3D Zero { get; } = new(0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Matrix3D Identity { get; } = new(1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Matrix3D Invalid { get; } = new(
            ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN,
            ddouble.NaN, ddouble.NaN, ddouble.NaN
        );

        public static implicit operator Matrix(Matrix3D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02 },
                { m.E10, m.E11, m.E12 },
                { m.E20, m.E21, m.E22 }
            };
        }

        public static implicit operator Matrix3D(Matrix m) {
            return new(m);
        }

        public static implicit operator ddouble[,](Matrix3D m) {
            return new ddouble[,] {
                { m.E00, m.E01, m.E02 },
                { m.E10, m.E11, m.E12 },
                { m.E20, m.E21, m.E22 }
            };
        }

        public static Matrix3D ScaleB(Matrix3D v, int n) {
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

        public static bool IsZero(Matrix3D m) {
            return
                ddouble.IsZero(m.E00) && ddouble.IsZero(m.E01) && ddouble.IsZero(m.E02) &&
                ddouble.IsZero(m.E10) && ddouble.IsZero(m.E11) && ddouble.IsZero(m.E12) &&
                ddouble.IsZero(m.E20) && ddouble.IsZero(m.E21) && ddouble.IsZero(m.E22);
        }

        public static bool IsFinite(Matrix3D m) {
            return
                ddouble.IsFinite(m.E00) && ddouble.IsFinite(m.E01) && ddouble.IsFinite(m.E02) &&
                ddouble.IsFinite(m.E10) && ddouble.IsFinite(m.E11) && ddouble.IsFinite(m.E12) &&
                ddouble.IsFinite(m.E20) && ddouble.IsFinite(m.E21) && ddouble.IsFinite(m.E22);
        }

        public static bool IsInfinity(Matrix3D m) {
            return !IsNaN(m) && (
                ddouble.IsInfinity(m.E00) || ddouble.IsInfinity(m.E01) || ddouble.IsInfinity(m.E02) ||
                ddouble.IsInfinity(m.E10) || ddouble.IsInfinity(m.E11) || ddouble.IsInfinity(m.E12) ||
                ddouble.IsInfinity(m.E20) || ddouble.IsInfinity(m.E21) || ddouble.IsInfinity(m.E22));
        }

        public static bool IsNaN(Matrix3D m) {
            return
                ddouble.IsNaN(m.E00) || ddouble.IsNaN(m.E01) || ddouble.IsFinite(m.E02) ||
                ddouble.IsNaN(m.E10) || ddouble.IsNaN(m.E11) || ddouble.IsFinite(m.E12) ||
                ddouble.IsNaN(m.E20) || ddouble.IsNaN(m.E21) || ddouble.IsFinite(m.E22);
        }

        public static bool IsIdentity(Matrix3D m) {
            return
                m.E00 == 1d && m.E01 == 0d && m.E02 == 0d &&
                m.E10 == 0d && m.E11 == 1d && m.E12 == 0d &&
                m.E20 == 0d && m.E21 == 0d && m.E22 == 1d;
        }

        public static bool IsValid(Matrix3D v) {
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
            return (obj is not null) && obj is Matrix3D matrix && matrix == this;
        }

        public bool Equals(Matrix3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return E00.GetHashCode() ^ E01.GetHashCode() ^ E02.GetHashCode()
                 ^ E10.GetHashCode() ^ E11.GetHashCode() ^ E12.GetHashCode()
                 ^ E20.GetHashCode() ^ E21.GetHashCode() ^ E22.GetHashCode();
        }
    }
}
