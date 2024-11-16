using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Vector3D : IVector<Vector3D>, IFormattable {
        public readonly ddouble X, Y, Z;

        public Vector3D(ddouble x, ddouble y, ddouble z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public ddouble Norm => ddouble.Hypot(X, Y, Z);

        public ddouble SquareNorm => X * X + Y * Y + Z * Z;

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D? normal = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal => normal ??= this / Norm;

        public static Vector3D operator +(Vector3D v) {
            return v;
        }

        public static Vector3D operator -(Vector3D v) {
            return new Vector3D(-v.X, -v.Y, -v.Z);
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3D operator *(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3D operator *(ddouble r, Vector3D v) {
            return new Vector3D(v.X * r, v.Y * r, v.Z * r);
        }

        public static Vector3D operator *(double r, Vector3D v) {
            return new Vector3D(v.X * r, v.Y * r, v.Z * r);
        }

        public static Vector3D operator *(Vector3D v, ddouble r) {
            return r * v;
        }

        public static Vector3D operator *(Vector3D v, double r) {
            return r * v;
        }

        public static Vector3D operator *(Matrix3D m, Vector3D v) {
            Vector3D ret = new(
                v.X * m.E00 + v.Y * m.E01 + v.Z * m.E02,
                v.X * m.E10 + v.Y * m.E11 + v.Z * m.E12,
                v.X * m.E20 + v.Y * m.E21 + v.Z * m.E22
            );

            return ret;
        }

        public static Vector3D operator *(HomogeneousMatrix3D m, Vector3D v) {
            Vector3D ret = new(
                v.X * m.E00 + v.Y * m.E01 + v.Z * m.E02 + m.E03,
                v.X * m.E10 + v.Y * m.E11 + v.Z * m.E12 + m.E13,
                v.X * m.E20 + v.Y * m.E21 + v.Z * m.E22 + m.E23
            );

            ddouble w = v.X * m.E30 + v.Y * m.E31 + v.Z * m.E32 + m.E33;
            Vector3D y = (w == 1d) ? ret : (ret / w);

            return y;
        }

        public static Vector3D operator *(Quaternion q, Vector3D v) {
            ddouble r = -q.I * v.X - q.J * v.Y - q.K * v.Z;
            ddouble i = +q.R * v.X + q.J * v.Z - q.K * v.Y;
            ddouble j = +q.R * v.Y - q.I * v.Z + q.K * v.X;
            ddouble k = +q.R * v.Z + q.I * v.Y - q.J * v.X;

            ddouble x = i * q.R - r * q.I + k * q.J - j * q.K;
            ddouble y = j * q.R - k * q.I - r * q.J + i * q.K;
            ddouble z = k * q.R + j * q.I - i * q.J - r * q.K;

            return new Vector3D(x, y, z);
        }

        public static Vector3D operator /(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3D operator /(Vector3D v, ddouble r) {
            return new Vector3D(v.X / r, v.Y / r, v.Z / r);
        }

        public static Vector3D operator /(Vector3D v, double r) {
            return new Vector3D(v.X / r, v.Y / r, v.Z / r);
        }

        public static bool operator ==(Vector3D v1, Vector3D v2) {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        public static bool operator !=(Vector3D v1, Vector3D v2) {
            return !(v1 == v2);
        }

        public static implicit operator Vector3D((ddouble x, ddouble y, ddouble z) v) {
            return new(v.x, v.y, v.z);
        }

        public static implicit operator (ddouble x, ddouble y, ddouble z)(Vector3D v) {
            return (v.X, v.Y, v.Z);
        }

        public static implicit operator Algebra.Vector(Vector3D v) {
            return new(v.X, v.Y, v.Z);
        }

        public static implicit operator ddouble[](Vector3D v) {
            return [v.X, v.Y, v.Z];
        }

        public static Quaternion ToQuaternion(Vector3D v) => (0d, v.X, v.Y, v.Z);

        public void Deconstruct(out ddouble x, out ddouble y, out ddouble z)
            => (x, y, z) = (X, Y, Z);

        public static ddouble Distance(Vector3D v1, Vector3D v2) {
            return (v1 - v2).Norm;
        }

        public static ddouble SquareDistance(Vector3D v1, Vector3D v2) {
            return (v1 - v2).SquareNorm;
        }

        public static ddouble Dot(Vector3D v1, Vector3D v2) {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3D Cross(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static Quaternion Rot(Vector3D v1, Vector3D v2) {
            Vector3D axis = Cross(v1, v2);

            if (v1 == v2 || IsZero(axis)) {
                return Quaternion.One;
            }
            else {
                ddouble v1_norm = v1.Norm, v2_norm = v2.Norm;

                return Quaternion.FromAxisAngle(
                    axis, ddouble.Acos(Dot(v1, v2) / (v1_norm * v2_norm))
                ) * ddouble.Sqrt(v2_norm / v1_norm);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Vector3D Zero { get; } = new(0d, 0d, 0d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Vector3D Invalid { get; } = new(ddouble.NaN, ddouble.NaN, ddouble.NaN);

        public static bool IsZero(Vector3D v) {
            return ddouble.IsZero(v.X) && ddouble.IsZero(v.Y) && ddouble.IsZero(v.Z);
        }

        public static bool IsFinite(Vector3D v) {
            return ddouble.IsFinite(v.X) && ddouble.IsFinite(v.Y) && ddouble.IsFinite(v.Z);
        }

        public static bool IsInfinity(Vector3D v) {
            return !IsNaN(v) && (ddouble.IsInfinity(v.X) || ddouble.IsInfinity(v.Y) || ddouble.IsInfinity(v.Z));
        }

        public static bool IsNaN(Vector3D v) {
            return ddouble.IsNaN(v.X) || ddouble.IsNaN(v.Y) || ddouble.IsNaN(v.Z);
        }

        public static bool IsValid(Vector3D v) {
            return IsFinite(v);
        }

        public override string ToString() {
            return $"[{X}, {Y}, {Z}]";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"[{X.ToString(format)}, {Y.ToString(format)}, {Z.ToString(format)}]";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Vector3D v && v == this;
        }

        public bool Equals(Vector3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
