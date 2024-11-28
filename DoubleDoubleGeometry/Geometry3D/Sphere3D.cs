using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Sphere3D : IGeometry<Sphere3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly ddouble Radius;

        public Sphere3D(Vector3D center, ddouble radius) {
            this.Center = center;
            this.Radius = radius;
        }

        public Vector3D Point(ddouble theta, ddouble phi) {
            ddouble ct = ddouble.Cos(theta), st = ddouble.Sin(theta);
            ddouble cp = ddouble.Cos(phi), sp = ddouble.Sin(phi);

            return Center + Radius * new Vector3D(st * cp, st * sp, ct);
        }

        public static Sphere3D FromIntersection(Vector3D v1, Vector3D v2, Vector3D v3, Vector3D v4) {
            ddouble x1 = v1.X, y1 = v1.Y, z1 = v1.Z, t1 = v1.SquareNorm;
            ddouble x2 = v2.X, y2 = v2.Y, z2 = v2.Z, t2 = v2.SquareNorm;
            ddouble x3 = v3.X, y3 = v3.Y, z3 = v3.Z, t3 = v3.SquareNorm;
            ddouble x4 = v4.X, y4 = v4.Y, z4 = v4.Z, t4 = v4.SquareNorm;

            ddouble a_det = Determinant(t1, y1, z1, t2, y2, z2, t3, y3, z3, t4, y4, z4);
            ddouble b_det = Determinant(x1, t1, z1, x2, t2, z2, x3, t3, z3, x4, t4, z4);
            ddouble c_det = Determinant(x1, y1, t1, x2, y2, t2, x3, y3, t3, x4, y4, t4);
            ddouble d_det = Determinant(x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4);

            Vector3D center = new Vector3D(a_det, b_det, c_det) / (2d * d_det);

            return new Sphere3D(
                center,
                Vector3D.Distance(v1, center)
            );
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => 4d * Radius * Radius * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Volume => ddouble.Abs(Radius * Radius * Radius) * ddouble.Pi / 0.75d;

        public static Sphere3D operator +(Sphere3D g) {
            return g;
        }

        public static Sphere3D operator -(Sphere3D g) {
            return new(-g.Center, g.Radius);
        }

        public static Sphere3D operator +(Sphere3D g, Vector3D v) {
            return new(g.Center + v, g.Radius);
        }

        public static Sphere3D operator +(Vector3D v, Sphere3D g) {
            return new(g.Center + v, g.Radius);
        }

        public static Sphere3D operator -(Sphere3D g, Vector3D v) {
            return new(g.Center - v, g.Radius);
        }

        public static Sphere3D operator -(Vector3D v, Sphere3D g) {
            return new(v - g.Center, g.Radius);
        }

        public static Sphere3D operator *(Quaternion q, Sphere3D g) {
            return new(q * g.Center, q.SquareNorm * g.Radius);
        }

        public static Sphere3D operator *(Sphere3D g, ddouble r) {
            return new(g.Center * r, g.Radius * r);
        }

        public static Sphere3D operator *(Sphere3D g, double r) {
            return new(g.Center * r, g.Radius * r);
        }

        public static Sphere3D operator *(ddouble r, Sphere3D g) {
            return g * r;
        }

        public static Sphere3D operator *(double r, Sphere3D g) {
            return g * r;
        }

        public static Sphere3D operator /(Sphere3D g, ddouble r) {
            return new(g.Center / r, g.Radius / r);
        }

        public static Sphere3D operator /(Sphere3D g, double r) {
            return new(g.Center / r, g.Radius / r);
        }

        public static bool operator ==(Sphere3D g1, Sphere3D g2) {
            return (g1.Center == g2.Center) && (g1.Radius == g2.Radius);
        }

        public static bool operator !=(Sphere3D g1, Sphere3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Sphere3D((Vector3D center, ddouble radius) g) {
            return new(g.center, g.radius);
        }

        public static implicit operator (Vector3D center, ddouble radius)(Sphere3D g) {
            return (g.Center, g.Radius);
        }

        public void Deconstruct(out Vector3D center, out ddouble radius)
            => (center, radius) = (Center, Radius);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Sphere3D Invalid { get; } = new(Vector3D.Invalid, ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Sphere3D Zero { get; } = new(Vector3D.Zero, ddouble.Zero);
               
        public bool Inside(Vector3D v) {
            ddouble radius = ddouble.Abs(Radius);

            bool inside = ((v - Center) / radius).SquareNorm <= 1d;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector3D> vs) {
            ddouble radius_inv = 1d / ddouble.Abs(Radius);

            foreach (Vector3D v in vs) {
                bool inside = ((v - Center) * radius_inv).SquareNorm <= 1d;

                yield return inside;
            }
        }

        public static bool IsNaN(Sphere3D g) {
            return Vector3D.IsNaN(g.Center) || ddouble.IsNaN(g.Radius);
        }

        public static bool IsZero(Sphere3D g) {
            return Vector3D.IsZero(g.Center) && ddouble.IsZero(g.Radius);
        }

        public static bool IsFinite(Sphere3D g) {
            return Vector3D.IsFinite(g.Center) && ddouble.IsFinite(g.Radius);
        }

        public static bool IsInfinity(Sphere3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Sphere3D g) {
            return IsFinite(g) && g.Radius >= 0d;
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
            return ReferenceEquals(this, obj) || (obj is not null && obj is Sphere3D g && g == this);
        }

        public bool Equals(Sphere3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }

        private static ddouble Determinant(
            ddouble e11, ddouble e12, ddouble e13,
            ddouble e21, ddouble e22, ddouble e23,
            ddouble e31, ddouble e32, ddouble e33,
            ddouble e41, ddouble e42, ddouble e43) {

            return e11 * (e32 * e43 + e22 * (e33 - e43) - e33 * e42 - e23 * (e32 - e42)) - e21 * (e32 * e43 - e33 * e42)
                 - e12 * (e31 * e43 + e21 * (e33 - e43) - e33 * e41 - e23 * (e31 - e41)) + e22 * (e31 * e43 - e33 * e41)
                 + e13 * (e31 * e42 + e21 * (e32 - e42) - e32 * e41 - e22 * (e31 - e41)) - e23 * (e31 * e42 - e32 * e41);
        }
    }
}
