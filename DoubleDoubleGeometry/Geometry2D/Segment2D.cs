using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Segment2D : IGeometry<Segment2D, Vector2D>, IFormattable {
        public readonly Vector2D V0, V1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Vector2D dv;

        public Segment2D(Vector2D v0, Vector2D v1) {
            this.V0 = v0;
            this.V1 = v1;
            this.dv = v1 - v0;
        }

        public Vector2D Point(ddouble t) {
            return V0 + t * dv;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Length => Vector2D.Distance(V0, V1);

        public static Segment2D operator +(Segment2D g) {
            return g;
        }

        public static Segment2D operator -(Segment2D g) {
            return new(-g.V0, -g.V1);
        }

        public static Segment2D operator +(Segment2D g, Vector2D v) {
            return new(g.V0 + v, g.V1 + v);
        }

        public static Segment2D operator +(Vector2D v, Segment2D g) {
            return new(g.V0 + v, g.V1 + v);
        }

        public static Segment2D operator -(Segment2D g, Vector2D v) {
            return new(g.V0 - v, g.V1 - v);
        }

        public static Segment2D operator -(Vector2D v, Segment2D g) {
            return new(v - g.V0, v - g.V1);
        }

        public static Segment2D operator *(Segment2D g, ddouble r) {
            return new(g.V0 * r, g.V1 * r);
        }

        public static Segment2D operator *(Segment2D g, double r) {
            return new(g.V0 * r, g.V1 * r);
        }

        public static Segment2D operator *(ddouble r, Segment2D g) {
            return g * r;
        }

        public static Segment2D operator *(double r, Segment2D g) {
            return g * r;
        }

        public static Segment2D operator /(Segment2D g, ddouble r) {
            return new(g.V0 / r, g.V1 / r);
        }

        public static Segment2D operator /(Segment2D g, double r) {
            return new(g.V0 / r, g.V1 / r);
        }

        public static Segment2D operator *(Matrix2D m, Segment2D g) {
            return new Segment2D(m * g.V0, m * g.V1);
        }

        public static Segment2D operator *(Complex c, Segment2D g) {
            return new Segment2D(c * g.V0, c * g.V1);
        }

        public static bool operator ==(Segment2D g1, Segment2D g2) {
            return (g1.V0 == g2.V0) && (g1.V1 == g2.V1);
        }

        public static bool operator !=(Segment2D g1, Segment2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Segment2D((Vector2D v0, Vector2D v1) g) {
            return new(g.v0, g.v1);
        }

        public static implicit operator (Vector2D v0, Vector2D v1)(Segment2D g) {
            return (g.V0, g.V1);
        }

        public void Deconstruct(out Vector2D v0, out Vector2D v1)
            => (v0, v1) = (V0, V1);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Segment2D Invalid { get; } = new(Vector2D.Invalid, Vector2D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Segment2D Zero { get; } = new(Vector2D.Zero, Vector2D.Zero);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox2D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox2D BoundingBox => bbox ??= new BoundingBox2D(V0, V1);

        public static bool IsNaN(Segment2D g) {
            return Vector2D.IsNaN(g.V0) || Vector2D.IsNaN(g.V1);
        }

        public static bool IsZero(Segment2D g) {
            return Vector2D.IsZero(g.V0) && Vector2D.IsZero(g.V1);
        }

        public static bool IsFinite(Segment2D g) {
            return Vector2D.IsFinite(g.V0) && Vector2D.IsFinite(g.V1);
        }

        public static bool IsInfinity(Segment2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Segment2D g) {
            return IsFinite(g) && g.V0 != g.V1;
        }

        public override string ToString() {
            return $"{V0}, {V1}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"{V0.ToString(format)}, {V1.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Segment2D g && g == this);
        }

        public bool Equals(Segment2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return V0.GetHashCode() ^ V1.GetHashCode();
        }
    }
}
