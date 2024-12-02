using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Circle2D : IGeometry<Circle2D, Vector2D>, IFormattable {
        public readonly Vector2D Center;
        public readonly ddouble Radius;

        public Circle2D(Vector2D center, ddouble radius) {
            this.Center = center;
            this.Radius = radius;
        }

        public Vector2D Point(ddouble t) {
            return new Vector2D(Center.X + Radius * ddouble.Cos(t), Center.Y + Radius * ddouble.Sin(t));
        }

        public static Circle2D FromIntersection(Vector2D v1, Vector2D v2, Vector2D v3) {
            return FromCircum((v1, v2, v3));
        }

        public static Circle2D FromCircum(Triangle2D triangle) {
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

        public static Circle2D FromIncircle(Triangle2D triangle) {
            Vector2D a = triangle.V0 - triangle.V1, b = triangle.V1 - triangle.V2, c = triangle.V2 - triangle.V0;

            ddouble a_norm = a.Norm, b_norm = b.Norm, c_norm = c.Norm, s = triangle.Area, sum_norm = a_norm + b_norm + c_norm;

            Vector2D center = (a_norm * triangle.V2 + b_norm * triangle.V0 + c_norm * triangle.V1) / sum_norm;
            ddouble radius = 2d * s / sum_norm;

            return new Circle2D(center, radius);
        }

        public static Circle2D FromImplicit(ddouble a, ddouble b, ddouble c) {
            Vector2D center = (a * -0.5d, b * -0.5d);
            ddouble radius = ddouble.Sqrt(ddouble.Ldexp(a * a + b * b, -2) - c);

            return new Circle2D(center, radius);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Radius * Radius * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 2d * ddouble.Abs(Radius) * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble A => -2d * Center.X;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble B => -2d * Center.Y;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble C => Center.SquareNorm - Radius * Radius;

        public static Circle2D operator +(Circle2D g) {
            return g;
        }

        public static Circle2D operator -(Circle2D g) {
            return new(-g.Center, -g.Radius);
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
            return new(v - g.Center, -g.Radius);
        }

        public static Circle2D operator *(Complex c, Circle2D g) {
            ddouble norm = c.Norm;

            return new(c * g.Center, norm * g.Radius);
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

        public static bool operator ==(Circle2D g1, Circle2D g2) {
            return (g1.Center == g2.Center) && (g1.Radius == g2.Radius);
        }

        public static bool operator !=(Circle2D g1, Circle2D g2) {
            return !(g1 == g2);
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox2D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox2D BoundingBox => bbox ??= new BoundingBox2D(Center, (Radius, Radius));

        public bool Inside(Vector2D v) {
            ddouble radius = ddouble.Abs(Radius);

            bool inside = ((v - Center) / radius).SquareNorm <= 1d;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector2D> vs) {
            ddouble radius_inv = 1d / ddouble.Abs(Radius);

            foreach (Vector2D v in vs) {
                bool inside = ((v - Center) * radius_inv).SquareNorm <= 1d;

                yield return inside;
            }
        }

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
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, radius={Radius}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, radius={Radius.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Circle2D g && g == this);
        }

        public bool Equals(Circle2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }
    }
}
