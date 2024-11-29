using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Plane3D : IGeometry<Plane3D, Vector3D>, IFormattable {
        public readonly Vector3D Normal;
        public readonly ddouble D;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble A => Normal.X;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble B => Normal.Y;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble C => Normal.Z;

        private Plane3D(Vector3D normal, ddouble d) {
            this.Normal = normal;
            this.D = d;
        }

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Quaternion? rot = null;
#pragma warning restore CS8632
        public Vector3D Point(ddouble u, ddouble v) {
            rot ??= Vector3D.Rot((0d, 0d, 1d), Normal);

            return rot * new Vector3D(u, v, -D);
        }

        public static Plane3D FromImplicit(ddouble a, ddouble b, ddouble c, ddouble d) {
            Vector3D normal = (a, b, c);
            ddouble norm = normal.Norm;

            return new Plane3D(normal / norm, d / norm);
        }

        public static Plane3D FromIntercept(Vector3D normal, ddouble d) {
            normal = normal.Normal;

            return new Plane3D(
                normal,
                d
            );
        }

        public static Plane3D FromNormal(Vector3D v, Vector3D normal) {
            normal = normal.Normal;

            return new Plane3D(
                normal,
                -(normal.X * v.X + normal.Y * v.Y + normal.Z * v.Z)
            );
        }

        public static Plane3D FromIntersection(Vector3D v0, Vector3D v1, Vector3D v2) {
            Vector3D normal = Vector3D.NormalizeSign(Vector3D.Cross(v1 - v0, v2 - v0).Normal);

            return new Plane3D(
                normal,
                -(normal.X * v0.X + normal.Y * v0.Y + normal.Z * v0.Z)
            );
        }

        public static Plane3D operator +(Plane3D g) {
            return g;
        }

        public static Plane3D operator -(Plane3D g) {
            return new(g.Normal, -g.D);
        }

        public static Plane3D operator +(Plane3D g, Vector3D v) {
            return new(g.Normal, g.D - (g.A * v.X + g.B * v.Y + g.C * v.Z));
        }

        public static Plane3D operator +(Vector3D v, Plane3D g) {
            return g + v;
        }

        public static Plane3D operator -(Plane3D g, Vector3D v) {
            return g + (-v);
        }

        public static Plane3D operator -(Vector3D v, Plane3D g) {
            return v + (-g);
        }

        public static Plane3D operator *(Matrix3D m, Plane3D g) {
            (Vector3D v0, Vector3D v1, Vector3D v2) = Points(g);

            return FromIntersection(m * v0, m * v1, m * v2);
        }

        public static Plane3D operator *(Quaternion q, Plane3D g) {
            (Vector3D v0, Vector3D v1, Vector3D v2) = Points(g);

            return FromIntersection(q * v0, q * v1, q * v2);
        }

        public static Plane3D operator *(Plane3D g, ddouble r) {
            return new(g.Normal, g.D * r);
        }

        public static Plane3D operator *(Plane3D g, double r) {
            return new(g.Normal, g.D * r);
        }

        public static Plane3D operator *(ddouble r, Plane3D g) {
            return g * r;
        }

        public static Plane3D operator *(double r, Plane3D g) {
            return g * r;
        }

        public static Plane3D operator /(Plane3D g, ddouble r) {
            return new(g.Normal, g.D / r);
        }

        public static Plane3D operator /(Plane3D g, double r) {
            return new(g.Normal, g.D / r);
        }

        public static bool operator ==(Plane3D g1, Plane3D g2) {
            return (g1.Normal == g2.Normal) && (g1.D == g2.D);
        }

        public static bool operator !=(Plane3D g1, Plane3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Plane3D((Vector3D normal, ddouble d) g) {
            return new(g.normal, g.d);
        }

        public static implicit operator (Vector3D normal, ddouble d)(Plane3D g) {
            return (g.Normal, g.D);
        }

        public void Deconstruct(out Vector3D normal, out ddouble d)
            => (normal, d) = (Normal, D);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Plane3D Invalid { get; } = new(Vector3D.Invalid, ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Plane3D Zero { get; } = new(Vector3D.Zero, ddouble.Zero);

        public static bool IsNaN(Plane3D g) {
            return Vector3D.IsNaN(g.Normal) || ddouble.IsNaN(g.D);
        }

        public static bool IsZero(Plane3D g) {
            return Vector3D.IsZero(g.Normal) && ddouble.IsZero(g.D);
        }

        public static bool IsFinite(Plane3D g) {
            return Vector3D.IsFinite(g.Normal) && ddouble.IsFinite(g.D);
        }

        public static bool IsInfinity(Plane3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Plane3D g) {
            return IsFinite(g) && !Vector3D.IsZero(g.Normal);
        }

        public override string ToString() {
            return $"normal={Normal}, d={D}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"normal={Normal.ToString(format)}, d={D.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Plane3D g && g == this);
        }

        public bool Equals(Plane3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        private static (Vector3D v0, Vector3D v1, Vector3D v2) Points(Plane3D g) {
            int index = Vector3D.MaxAbsIndex(g.Normal);

            if (index == 0) {
                return ((-g.D / g.A, 0d, 0d), (-(g.D + g.B) / g.A, 1d, 0d), (-(g.D + g.C) / g.A, 0d, 1d));
            }
            else if (index == 1) {
                return ((0d, -g.D / g.B, 0d), (0d, -(g.D + g.C) / g.B, 1d), (1d, -(g.D + g.A) / g.B, 0d));
            }
            else {
                return ((0d, 0d, -g.D / g.C), (0d, 1d, -(g.D + g.B) / g.C), (1d, 0d, -(g.D + g.A) / g.C));
            }
        }

        public Vector3D Projection(Vector3D v) {
            Quaternion q = Vector3D.Rot(Normal, (0d, 0d, 1d));

            Vector3D u = q * v;

            return (u.X, u.Y, u.Z + D);
        }

        public IEnumerable<Vector3D> Projection(IEnumerable<Vector3D> vs) {
            Quaternion q = Vector3D.Rot(Normal, (0d, 0d, 1d));

            foreach (Vector3D v in vs) {
                Vector3D u = q * v;

                yield return (u.X, u.Y, u.Z + D);
            }
        }
    }
}
