using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Circle3D : IGeometry<Circle3D, Vector3D>, IFormattable {
        public readonly Vector3D Center, Normal;
        public readonly ddouble Radius;

        private Circle3D(Vector3D center, Vector3D normal, ddouble radius, int _) {
            this.Center = center;
            this.Normal = normal;
            this.Radius = radius;
        }

        public Circle3D(Vector3D center, Vector3D normal, ddouble radius) {
            this.Center = center;
            this.Normal = normal.Normal;
            this.Radius = radius;
        }

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Quaternion? rot = null;
#pragma warning restore CS8632
        public Vector3D Point(ddouble t) {
            rot ??= Vector3D.Rot((0d, 0d, 1d), Normal);

            return Center + rot * new Vector3D(Radius * ddouble.Cos(t), Radius * ddouble.Sin(t), 0d);
        }

        public static Circle3D FromIntersection(Vector3D v1, Vector3D v2, Vector3D v3) {
            return FromCircum((v1, v2, v3));
        }

        public static Circle3D FromCircum(Triangle3D triangle) {
            Vector3D a = triangle.V0 - triangle.V1, b = triangle.V1 - triangle.V2, c = triangle.V2 - triangle.V0;

            ddouble a_sqnorm = a.SquareNorm, b_sqnorm = b.SquareNorm, c_sqnorm = c.SquareNorm;
            ddouble a_norm = ddouble.Sqrt(a_sqnorm), b_norm = ddouble.Sqrt(b_sqnorm), c_norm = ddouble.Sqrt(c_sqnorm);

            ddouble ra = a_sqnorm * (b_sqnorm + c_sqnorm - a_sqnorm);
            ddouble rb = b_sqnorm * (c_sqnorm + a_sqnorm - b_sqnorm);
            ddouble rc = c_sqnorm * (a_sqnorm + b_sqnorm - c_sqnorm);

            Vector3D center = (ra * triangle.V2 + rb * triangle.V0 + rc * triangle.V1) / (ra + rb + rc);
            Vector3D normal = Vector3D.NormalizeSign(Vector3D.Cross(c, a));
            ddouble radius = a_norm * b_norm * c_norm / ddouble.Sqrt((a_norm + b_norm + c_norm) * (-a_norm + b_norm + c_norm) * (a_norm - b_norm + c_norm) * (a_norm + b_norm - c_norm));

            return new Circle3D(center, normal, radius);
        }

        public static Circle3D FromIncircle(Triangle3D triangle) {
            Vector3D a = triangle.V0 - triangle.V1, b = triangle.V1 - triangle.V2, c = triangle.V2 - triangle.V0;

            ddouble a_norm = a.Norm, b_norm = b.Norm, c_norm = c.Norm, s = triangle.Area, sum_norm = a_norm + b_norm + c_norm;

            Vector3D center = (a_norm * triangle.V2 + b_norm * triangle.V0 + c_norm * triangle.V1) / sum_norm;
            Vector3D normal = Vector3D.NormalizeSign(Vector3D.Cross(c, a));
            ddouble radius = 2d * s / sum_norm;

            return new Circle3D(center, normal, radius);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Radius * Radius * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 2d * Radius * ddouble.Pi;

        public static Circle3D operator +(Circle3D g) {
            return g;
        }

        public static Circle3D operator -(Circle3D g) {
            return new(-g.Center, -g.Normal, g.Radius, 0);
        }

        public static Circle3D operator +(Circle3D g, Vector3D v) {
            return new(g.Center + v, g.Normal, g.Radius, 0);
        }

        public static Circle3D operator +(Vector3D v, Circle3D g) {
            return new(g.Center + v, g.Normal, g.Radius, 0);
        }

        public static Circle3D operator -(Circle3D g, Vector3D v) {
            return new(g.Center - v, g.Normal, g.Radius, 0);
        }

        public static Circle3D operator -(Vector3D v, Circle3D g) {
            return new(v - g.Center, -g.Normal, g.Radius, 0);
        }

        public static Circle3D operator *(Quaternion q, Circle3D g) {
            Vector3D normal = q * g.Normal;
            ddouble norm = normal.Norm;

            return new(g.Center * norm, normal / norm, g.Radius * norm, 0);
        }

        public static Circle3D operator *(Circle3D g, ddouble r) {
            return new(g.Center * r, g.Normal * ddouble.Sign(r), g.Radius * ddouble.Abs(r), 0);
        }

        public static Circle3D operator *(Circle3D g, double r) {
            return new(g.Center * r, g.Normal * double.Sign(r), g.Radius * double.Abs(r), 0);
        }

        public static Circle3D operator *(ddouble r, Circle3D g) {
            return g * r;
        }

        public static Circle3D operator *(double r, Circle3D g) {
            return g * r;
        }

        public static Circle3D operator /(Circle3D g, ddouble r) {
            return new(g.Center / r, g.Normal * ddouble.Sign(r), g.Radius / ddouble.Abs(r), 0);
        }

        public static Circle3D operator /(Circle3D g, double r) {
            return new(g.Center / r, g.Normal * ddouble.Sign(r), g.Radius / double.Abs(r), 0);
        }

        public static bool operator ==(Circle3D g1, Circle3D g2) {
            return (g1.Center == g2.Center) && (g1.Normal == g2.Normal) && (g1.Radius == g2.Radius);
        }

        public static bool operator !=(Circle3D g1, Circle3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Circle3D((Vector3D center, Vector3D normal, ddouble radius) g) {
            return new(g.center, g.normal, g.radius);
        }

        public static implicit operator (Vector3D center, Vector3D normal, ddouble radius)(Circle3D g) {
            return (g.Center, g.Normal, g.Radius);
        }

        public void Deconstruct(out Vector3D center, out Vector3D normal, out ddouble radius)
            => (center, normal, radius) = (Center, Normal, Radius);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Circle3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, ddouble.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Circle3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, ddouble.Zero, 0);

        public static bool IsNaN(Circle3D g) {
            return Vector3D.IsNaN(g.Center) || Vector3D.IsNaN(g.Normal) || ddouble.IsNaN(g.Radius);
        }

        public static bool IsZero(Circle3D g) {
            return Vector3D.IsZero(g.Center) && Vector3D.IsZero(g.Normal) && ddouble.IsZero(g.Radius);
        }

        public static bool IsFinite(Circle3D g) {
            return Vector3D.IsFinite(g.Center) && Vector3D.IsFinite(g.Normal) && ddouble.IsFinite(g.Radius);
        }

        public static bool IsInfinity(Circle3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Circle3D g) {
            return IsFinite(g) && !Vector3D.IsZero(g.Normal) && g.Radius >= 0d;
        }

        public override string ToString() {
            return $"center={Center}, normal={Normal}, radius={Radius}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, normal={Normal.ToString(format)}, radius={Radius.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Circle3D geo && geo == this;
        }

        public bool Equals(Circle3D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Normal.GetHashCode() ^ Radius.GetHashCode();
        }
    }
}
