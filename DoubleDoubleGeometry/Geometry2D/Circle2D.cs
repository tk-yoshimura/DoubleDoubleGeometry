using DoubleDouble;
using System.Diagnostics;

namespace DoubleDoubleGeometry.Geometry2D {
    public class Circle2D : IGeometry<Circle2D, Vector2D> {
        public readonly Vector2D Center;
        public readonly ddouble Radius;

        public Circle2D(Vector2D center, ddouble radius) {
            this.Center = center;
            this.Radius = radius;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Radius * Radius * ddouble.Pi;

        public static Circle2D Circum(Triangle2D triangle) {
            Vector2D a = triangle.V0 - triangle.V1, b = triangle.V1 - triangle.V2, c = triangle.V2 - triangle.V0;

            ddouble a_sqnorm = a.SquareNorm, b_sqnorm = b.SquareNorm, c_sqnorm = c.SquareNorm;
            ddouble a_norm = ddouble.Sqrt(a_sqnorm), b_norm = ddouble.Sqrt(b_sqnorm), c_norm = ddouble.Sqrt(c_sqnorm);

            ddouble ra = a_sqnorm * (b_sqnorm + c_sqnorm - a_sqnorm);
            ddouble rb = b_sqnorm * (c_sqnorm + a_sqnorm - b_sqnorm);
            ddouble rc = c_sqnorm * (a_sqnorm + b_sqnorm - c_sqnorm);

            Vector2D center = (ra * triangle.V2 + rb * triangle.V0 + rc * triangle.V1) / (ra + rb + rc);
            ddouble radius = a_norm * b_norm * c_norm / ddouble.Sqrt((a_norm + b_norm + c_norm) * (-a_norm + b_norm + c_norm) * (a_norm - b_norm + c_norm) * (a_norm + b_norm - c_norm));

            return new Circle2D(center, radius);
        }

        public static Circle2D Incircle(Triangle2D triangle) {
            Vector2D a = triangle.V0 - triangle.V1, b = triangle.V1 - triangle.V2, c = triangle.V2 - triangle.V0;

            ddouble a_norm = a.Norm, b_norm = b.Norm, c_norm = c.Norm, s = triangle.Area, sum_norm = a_norm + b_norm + c_norm;

            Vector2D center = (a_norm * triangle.V2 + b_norm * triangle.V0 + c_norm * triangle.V1) / sum_norm;
            ddouble radius = 2d * s / sum_norm;

            return new Circle2D(center, radius);
        }

        public static Circle2D operator +(Circle2D g) {
            return g;
        }

        public static Circle2D operator -(Circle2D g) {
            return new(-g.Center, g.Radius);
        }

        public static Circle2D operator +(Circle2D g, Vector2D v) {
            return new(g.Center + v, g.Radius);
        }

        public static Circle2D operator +(Vector2D v, Circle2D g) {
            return new(g.Center + v, g.Radius);
        }

        public static Circle2D operator -(Circle2D g, Vector2D v) {
            return new(g.Center - v, g.Radius);
        }

        public static Circle2D operator -(Vector2D v, Circle2D g) {
            return new(v - g.Center, g.Radius);
        }

        public static Circle2D operator *(Circle2D g, ddouble r) {
            return new(g.Center * r, g.Radius * r);
        }

        public static Circle2D operator *(Circle2D g, double r) {
            return new(g.Center * r, g.Radius * r);
        }

        public static Circle2D operator *(ddouble r, Circle2D g) {
            return g * r;
        }

        public static Circle2D operator *(double r, Circle2D g) {
            return g * r;
        }

        public static Circle2D operator /(Circle2D g, ddouble r) {
            return new(g.Center / r, g.Radius / r);
        }

        public static Circle2D operator /(Circle2D g, double r) {
            return new(g.Center / r, g.Radius / r);
        }

        public static bool operator ==(Circle2D t1, Circle2D t2) {
            return (t1.Center == t2.Center) && (t1.Radius == t2.Radius);
        }

        public static bool operator !=(Circle2D t1, Circle2D t2) {
            return !(t1 == t2);
        }

        public static implicit operator Circle2D((Vector2D center, ddouble radius) g) {
            return new(g.center, g.radius);
        }

        public static implicit operator (Vector2D center, ddouble radius)(Circle2D g) {
            return (g.Center, g.Radius);
        }

        public void Deconstruct(out Vector2D center, out ddouble radius)
            => (center, radius) = (Center, Radius);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Circle2D Invalid { get; } = new(Vector2D.Invalid, ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Circle2D Zero { get; } = new(Vector2D.Zero, ddouble.Zero);

        public static bool IsNaN(Circle2D g) {
            return Vector2D.IsNaN(g.Center) || ddouble.IsNaN(g.Radius);
        }

        public static bool IsZero(Circle2D g) {
            return Vector2D.IsZero(g.Center) && ddouble.IsZero(g.Radius);
        }

        public static bool IsFinite(Circle2D g) {
            return Vector2D.IsFinite(g.Center) && ddouble.IsFinite(g.Radius);
        }

        public static bool IsInfinity(Circle2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Circle2D g) {
            return IsFinite(g) && g.Radius >= 0d;
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Circle2D geo && geo == this;
        }

        public bool Equals(Circle2D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }
    }
}
