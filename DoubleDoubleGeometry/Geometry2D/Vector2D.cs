using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

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

        public static explicit operator Complex(Vector2D v) {
            return (v.X, v.Y);
        }

        public static implicit operator Vector(Vector2D v) {
            return new(v.X, v.Y);
        }

        public static implicit operator Vector2D(Vector v) {
            return new(v);
        }

        public static explicit operator Vector2D(Vector3D v) {
            return new(v.X, v.Y);
        }

        public static implicit operator ddouble[](Vector2D v) {
            return [v.X, v.Y];
        }

        public static implicit operator Vector2D(ddouble[] v) {
            if (v.Length != 2) {
                throw new ArgumentException("invalid dim", nameof(v));
            }

            return new Vector2D(v[0], v[1]);
        }

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

        public static Complex Rot(Vector2D v1, Vector2D v2) {
            ddouble v1_norm = v1.Norm, v2_norm = v2.Norm;

            if (v2_norm == 0d) {
                return Complex.Zero;
            }

            ddouble n = v2_norm / v1_norm;
            ddouble c = v1_norm * v2_norm;

            ddouble r = (v1.X * v2.X + v1.Y * v2.Y) / c;
            ddouble i = (v1.X * v2.Y - v1.Y * v2.X) / c;

            return (n * r, n * i);
        }

        public static Vector2D ScaleB(Vector2D v, int n) {
            return new(ddouble.Ldexp(v.X, n), ddouble.Ldexp(v.Y, n));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MaxExponent {
            get {
                int max_exponent = int.MinValue + 1; // abs(int.minvalue) throw arithmetic exception

                if (ddouble.IsFinite(X)) {
                    max_exponent = int.Max(ddouble.ILogB(X), max_exponent);
                }
                if (ddouble.IsFinite(Y)) {
                    max_exponent = int.Max(ddouble.ILogB(Y), max_exponent);
                }

                return max_exponent;
            }
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

        public static Vector2D NormalizeSign(Vector2D v) {
            if (IsNaN(v)) {
                return v;
            }

            if (v.X > 0d) {
                return (v.X, v.Y == 0d ? 0d : v.Y);
            }
            else if (ddouble.IsNegative(v.X)) {
                v = (-v.X, v.Y == 0d ? 0d : -v.Y);
            }

            if (v.X > 0d || v.Y > 0d) {
                return v;
            }
            else if (ddouble.IsNegative(v.Y)) {
                v = (0d, -v.Y);
            }

            return v;
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
            return ReferenceEquals(this, obj) || (obj is not null && obj is Vector2D v && v == this);
        }

        public bool Equals(Vector2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    public static class Vector2DIOExpand {

        public static void Write(this BinaryWriter writer, Vector2D v) {
            DoubleDoubleIOExpand.Write(writer, v.X);
            DoubleDoubleIOExpand.Write(writer, v.Y);
        }

        public static Vector2D ReadVector2D(this BinaryReader reader) {
            ddouble x = reader.ReadDDouble();
            ddouble y = reader.ReadDDouble();

            return (x, y);
        }
    }

    public static class Vector2DEnumerableExpand {
        public static Vector2D Sum(this IEnumerable<Vector2D> source) {
            Vector2D acc = Vector2D.Zero, carry = Vector2D.Zero;

            foreach (var v in source) {
                Vector2D d = v - carry;
                Vector2D acc_next = acc + d;

                carry = (acc_next - acc) - d;
                acc = acc_next;
            }

            return acc;
        }

        public static Vector2D Average(this IEnumerable<Vector2D> source) {
            return source.Sum() / source.Count();
        }

        public static Vector2D Max(this IEnumerable<Vector2D> source) {
            ddouble xmax = ddouble.NaN, ymax = ddouble.NaN;

            foreach (var v in source) {
                xmax = !(xmax >= v.X) ? v.X : xmax;
                ymax = !(ymax >= v.Y) ? v.Y : ymax;
            }

            return (xmax, ymax);
        }

        public static Vector2D Min(this IEnumerable<Vector2D> source) {
            ddouble xmin = ddouble.NaN, ymin = ddouble.NaN;

            foreach (var v in source) {
                xmin = !(xmin <= v.X) ? v.X : xmin;
                ymin = !(ymin <= v.Y) ? v.Y : ymin;
            }

            return (xmin, ymin);
        }

        public static IEnumerable<ddouble> X(this IEnumerable<Vector2D> source) {
            foreach (var v in source) {
                yield return v.X;
            }
        }

        public static IEnumerable<ddouble> Y(this IEnumerable<Vector2D> source) {
            foreach (var v in source) {
                yield return v.Y;
            }
        }

        public static IEnumerable<ddouble> SquareNorm(this IEnumerable<Vector2D> source) {
            foreach (var v in source) {
                yield return v.SquareNorm;
            }
        }

        public static IEnumerable<ddouble> Norm(this IEnumerable<Vector2D> source) {
            foreach (var v in source) {
                yield return v.Norm;
            }
        }

        public static IEnumerable<Vector2D> Normal(this IEnumerable<Vector2D> source) {
            foreach (var v in source) {
                yield return v.Normal;
            }
        }
    }
}
