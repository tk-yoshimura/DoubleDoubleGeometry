using DoubleDouble;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Line2D : IGeometry<Line2D, Vector2D>, IFormattable {
        public readonly Vector2D Origin, Direction;

        private Line2D(Vector2D origin, Vector2D direction) {
            this.Origin = origin;
            this.Direction = direction;
        }

        public Vector2D Point(ddouble t) {
            return Origin + t * Direction;
        }

        public static Line2D FromImplicit(ddouble a, ddouble b, ddouble c) {
            Vector2D dir = new Vector2D(b, -a).Normal;

            if (ddouble.Abs(a) >= ddouble.Abs(b)) {
                return new Line2D((-c / a, 0d), dir);
            }
            else {
                return new Line2D((0d, -c / b), dir);
            }
        }

        public static Line2D FromDirection(Vector2D origin, Vector2D direction) {
            return new Line2D(origin, direction.Normal);
        }

        public static Line2D FromIntersection(Vector2D v0, Vector2D v1) {
            return new Line2D(v0, (v1 - v0).Normal);
        }

        public static Line2D operator +(Line2D g) {
            return g;
        }

        public static Line2D operator -(Line2D g) {
            return new(-g.Origin, -g.Direction);
        }

        public static Line2D operator +(Line2D g, Vector2D v) {
            return new(g.Origin + v, g.Direction);
        }

        public static Line2D operator +(Vector2D v, Line2D g) {
            return new(g.Origin + v, g.Direction);
        }

        public static Line2D operator -(Line2D g, Vector2D v) {
            return new(g.Origin - v, g.Direction);
        }

        public static Line2D operator -(Vector2D v, Line2D g) {
            return new(v - g.Origin, -g.Direction);
        }

        public static Line2D operator *(Line2D g, ddouble r) {
            return new(g.Origin * r, g.Direction * ddouble.Sign(r));
        }

        public static Line2D operator *(Line2D g, double r) {
            return new(g.Origin * r, g.Direction * double.Sign(r));
        }

        public static Line2D operator *(ddouble r, Line2D g) {
            return g * r;
        }

        public static Line2D operator *(double r, Line2D g) {
            return g * r;
        }

        public static Line2D operator /(Line2D g, ddouble r) {
            return new(g.Origin / r, g.Direction * ddouble.Sign(r));
        }

        public static Line2D operator /(Line2D g, double r) {
            return new(g.Origin / r, g.Direction * double.Sign(r));
        }

        public static Line2D operator *(Matrix2D m, Line2D g) {
            Vector2D v0 = m * g.Origin, v1 = m * (g.Origin + g.Direction);

            return FromIntersection(v0, v1);
        }

        public static bool operator ==(Line2D g1, Line2D g2) {
            return (g1.Origin == g2.Origin) && (g1.Direction == g2.Direction);
        }

        public static bool operator !=(Line2D g1, Line2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Line2D((Vector2D v0, Vector2D v1) g) {
            return FromIntersection(g.v0, g.v1);
        }

        public static implicit operator (Vector2D origin, Vector2D direction)(Line2D g) {
            return (g.Origin, g.Direction);
        }

        public void Deconstruct(out Vector2D origin, out Vector2D direction)
            => (origin, direction) = (Origin, Direction);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Line2D Invalid { get; } = new(Vector2D.Invalid, Vector2D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Line2D Zero { get; } = new(Vector2D.Zero, Vector2D.Zero);

        public static bool IsNaN(Line2D g) {
            return Vector2D.IsNaN(g.Origin) || Vector2D.IsNaN(g.Direction);
        }

        public static bool IsZero(Line2D g) {
            return Vector2D.IsZero(g.Origin) && Vector2D.IsZero(g.Direction);
        }

        public static bool IsFinite(Line2D g) {
            return Vector2D.IsFinite(g.Origin) && Vector2D.IsFinite(g.Direction);
        }

        public static bool IsInfinity(Line2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Line2D g) {
            return IsFinite(g) && Vector2D.IsFinite(g.Direction) && !Vector2D.IsZero(g.Direction);
        }

        public override string ToString() {
            return $"origin={Origin}, direction={Direction}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"origin={Origin.ToString(format)}, direction={Direction.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Line2D g && g == this);
        }

        public bool Equals(Line2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Origin.GetHashCode() ^ Direction.GetHashCode();
        }
    }
}
