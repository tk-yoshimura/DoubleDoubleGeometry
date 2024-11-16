using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Vector2D : IVector<Vector2D>, IFormattable {
        public readonly ddouble X, Y;

        public Vector2D(ddouble x, ddouble y) {
            this.X = x;
            this.Y = y;
        }

        public Vector2D(Vector v) {
            if (v.Dim != 2) {
                throw new ArgumentException("invalid dim", nameof(v));
            }

            this.X = v[0];
            this.Y = v[1];
        }

        public ddouble Norm => ddouble.Hypot(X, Y);

        public ddouble SquareNorm => X * X + Y * Y;

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2D? normal = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Normal => normal ??= this / Norm;

        public static Vector2D operator +(Vector2D v) {
            return v;
        }

        public static Vector2D operator -(Vector2D v) {
            return new Vector2D(-v.X, -v.Y);
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2) {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2) {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator *(Vector2D v1, Vector2D v2) {
            return new Vector2D(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2D operator *(ddouble r, Vector2D v) {
            return new Vector2D(v.X * r, v.Y * r);
        }

        public static Vector2D operator *(double r, Vector2D v) {
            return new Vector2D(v.X * r, v.Y * r);
        }

        public static Vector2D operator *(Vector2D v, ddouble r) {
            return r * v;
        }

        public static Vector2D operator *(Vector2D v, double r) {
            return r * v;
        }

        public static Vector2D operator *(Matrix2D m, Vector2D v) {
            Vector2D ret = new(
                v.X * m.E00 + v.Y * m.E01,
                v.X * m.E10 + v.Y * m.E11
            );

            return ret;
        }

        public static Vector2D operator *(HomogeneousMatrix2D m, Vector2D v) {
            Vector2D ret = new(
                v.X * m.E00 + v.Y * m.E01 + m.E02,
                v.X * m.E10 + v.Y * m.E11 + m.E12
            );

            ddouble w = v.X * m.E20 + v.Y * m.E21 + m.E22;
            Vector2D y = (w == 1d) ? ret : (ret / w);

            return y;
        }

        public static Vector2D operator *(Complex c, Vector2D v) {
            Vector2D ret = new(v.X * c.R - v.Y * c.I,
                               v.X * c.I + v.Y * c.R);

            return ret;
        }

        public static Vector2D operator /(Vector2D v1, Vector2D v2) {
            return new Vector2D(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Vector2D operator /(Vector2D v, ddouble r) {
            return new Vector2D(v.X / r, v.Y / r);
        }

        public static Vector2D operator /(Vector2D v, double r) {
            return new Vector2D(v.X / r, v.Y / r);
        }

        public static bool operator ==(Vector2D v1, Vector2D v2) {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator !=(Vector2D v1, Vector2D v2) {
            return !(v1 == v2);
        }

        public static implicit operator Vector2D((ddouble x, ddouble y) v) {
            return new(v.x, v.y);
        }

        public static implicit operator (ddouble x, ddouble y)(Vector2D v) {
            return (v.X, v.Y);
        }

        public static implicit operator Vector(Vector2D v) {
            return new(v.X, v.Y);
        }

        public static implicit operator Vector2D(Vector v) {
            return new(v);
        }

        public static implicit operator ddouble[](Vector2D v) {
            return [v.X, v.Y];
        }

        public static Complex ToComplex(Vector3D v) => (v.X, v.Y);

        public void Deconstruct(out ddouble x, out ddouble y)
            => (x, y) = (X, Y);

        public static ddouble Distance(Vector2D v1, Vector2D v2) {
            return (v1 - v2).Norm;
        }

        public static ddouble SquareDistance(Vector2D v1, Vector2D v2) {
            return (v1 - v2).SquareNorm;
        }

        public static ddouble Dot(Vector2D v1, Vector2D v2) {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static int MaxAbsIndex(Vector2D v) => ddouble.Abs(v.X) >= ddouble.Abs(v.Y) ? 0 : 1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Vector2D Zero { get; } = new(0d, 0d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Vector2D Invalid { get; } = new(ddouble.NaN, ddouble.NaN);

        public static bool IsZero(Vector2D v) {
            return ddouble.IsZero(v.X) && ddouble.IsZero(v.Y);
        }

        public static bool IsFinite(Vector2D v) {
            return ddouble.IsFinite(v.X) && ddouble.IsFinite(v.Y);
        }

        public static bool IsInfinity(Vector2D v) {
            return !IsNaN(v) && (ddouble.IsInfinity(v.X) || ddouble.IsInfinity(v.Y));
        }

        public static bool IsNaN(Vector2D v) {
            return ddouble.IsNaN(v.X) || ddouble.IsNaN(v.Y);
        }

        public static bool IsValid(Vector2D v) {
            return IsFinite(v);
        }

        public override string ToString() {
            return $"[{X}, {Y}]";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"[{X.ToString(format)}, {Y.ToString(format)}]";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Vector2D v && v == this;
        }

        public bool Equals(Vector2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
