using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Triangle2D : IGeometry<Triangle2D, Vector2D>, IFormattable {
        public readonly Vector2D V0, V1, V2;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Matrix2D ortho_matrix;

        public Triangle2D(Vector2D v0, Vector2D v1, Vector2D v2) {
            this.V0 = v0;
            this.V1 = v1;
            this.V2 = v2;

            Vector2D a = V1 - V0, b = V2 - V0;

            this.ortho_matrix = new Matrix2D(a.X, b.X, a.Y, b.Y).Inverse;
        }

        public Vector2D Point(ddouble u, ddouble v) {
            return V0 + u * (V1 - V0) + (1d - u) * v * (V2 - V0);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area {
            get {
                Vector2D a = V1 - V0, b = V2 - V0;
                ddouble inner_product_ab = Vector2D.Dot(a, b);

                return ddouble.Ldexp(ddouble.Sqrt(a.SquareNorm * b.SquareNorm - inner_product_ab * inner_product_ab), -1);
            }
        }

        public static Triangle2D operator +(Triangle2D g) {
            return g;
        }

        public static Triangle2D operator -(Triangle2D g) {
            return new(-g.V0, -g.V1, -g.V2);
        }

        public static Triangle2D operator +(Triangle2D g, Vector2D v) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v);
        }

        public static Triangle2D operator +(Vector2D v, Triangle2D g) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v);
        }

        public static Triangle2D operator -(Triangle2D g, Vector2D v) {
            return new(g.V0 - v, g.V1 - v, g.V2 - v);
        }

        public static Triangle2D operator -(Vector2D v, Triangle2D g) {
            return new(v - g.V0, v - g.V1, v - g.V2);
        }

        public static Triangle2D operator *(Triangle2D g, ddouble r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r);
        }

        public static Triangle2D operator *(Triangle2D g, double r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r);
        }

        public static Triangle2D operator *(ddouble r, Triangle2D g) {
            return g * r;
        }

        public static Triangle2D operator *(double r, Triangle2D g) {
            return g * r;
        }

        public static Triangle2D operator /(Triangle2D g, ddouble r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r);
        }

        public static Triangle2D operator /(Triangle2D g, double r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r);
        }

        public static Triangle2D operator *(Matrix2D m, Triangle2D g) {
            return new Triangle2D(m * g.V0, m * g.V1, m * g.V2);
        }

        public static Triangle2D operator *(Complex c, Triangle2D g) {
            return new Triangle2D(c * g.V0, c * g.V1, c * g.V2);
        }

        public static bool operator ==(Triangle2D g1, Triangle2D g2) {
            return (g1.V0 == g2.V0) && (g1.V1 == g2.V1) && (g1.V2 == g2.V2);
        }

        public static bool operator !=(Triangle2D g1, Triangle2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Triangle2D((Vector2D v0, Vector2D v1, Vector2D v2) g) {
            return new(g.v0, g.v1, g.v2);
        }

        public static implicit operator (Vector2D v0, Vector2D v1, Vector2D v2)(Triangle2D g) {
            return (g.V0, g.V1, g.V2);
        }

        public void Deconstruct(out Vector2D v0, out Vector2D v1, out Vector2D v2)
            => (v0, v1, v2) = (V0, V1, V2);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Triangle2D Invalid { get; } = new(Vector2D.Invalid, Vector2D.Invalid, Vector2D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Triangle2D Zero { get; } = new(Vector2D.Zero, Vector2D.Zero, Vector2D.Zero);

        public bool Inside(Vector2D v) {
            Vector2D u = ortho_matrix * (v - V0);

            bool inside = u.X >= 0d && u.Y >= 0d && u.X + u.Y <= 1d;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector2D> vs) {
            foreach (Vector2D v in vs) {
                Vector2D u = ortho_matrix * (v - V0);

                bool inside = u.X >= 0d && u.Y >= 0d && u.X + u.Y <= 1d;

                yield return inside;
            }
        }

        public static bool IsNaN(Triangle2D g) {
            return Vector2D.IsNaN(g.V0) || Vector2D.IsNaN(g.V1) || Vector2D.IsNaN(g.V2);
        }

        public static bool IsZero(Triangle2D g) {
            return Vector2D.IsZero(g.V0) && Vector2D.IsZero(g.V1) && Vector2D.IsZero(g.V2);
        }

        public static bool IsFinite(Triangle2D g) {
            return Vector2D.IsFinite(g.V0) && Vector2D.IsFinite(g.V1) && Vector2D.IsFinite(g.V2);
        }

        public static bool IsInfinity(Triangle2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Triangle2D g) {
            return IsFinite(g) && g.V0 != g.V1 && g.V1 != g.V2 && g.V2 != g.V0;
        }

        public override string ToString() {
            return $"{V0}, {V1}, {V2}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"{V0.ToString(format)}, {V1.ToString(format)}, {V2.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Triangle2D g && g == this);
        }

        public bool Equals(Triangle2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return V0.GetHashCode() ^ V1.GetHashCode() ^ V2.GetHashCode();
        }
    }
}
