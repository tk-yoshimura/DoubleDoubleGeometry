using DoubleDouble;
using System.Diagnostics;

namespace DoubleDoubleGeometry.Geometry3D {

    public class Plane3D : IGeometry<Plane3D, Vector3D> {
        public readonly Vector3D Normal;
        public readonly ddouble D;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble A => Normal.X;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble B => Normal.Y;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble C => Normal.Z;

        public Plane3D(Vector3D normal, ddouble d, int _) {
            this.Normal = normal;
            this.D = d;
        }

        public Plane3D(Vector3D normal, ddouble d) {
            this.Normal = normal.Normal;
            this.D = d;
        }

        public Plane3D(Vector3D normal, Vector3D v) {
            this.Normal = normal.Normal;
            this.D = -(A * v.X + B * v.Y + C * v.Z);
        }

        public static Plane3D FromIntersection(Vector3D v0, Vector3D v1, Vector3D v2) {
            Vector3D normal = Vector3D.Cross(v1 - v0, v2 - v0).Normal;

            return new Plane3D(
                normal,
                -(normal.X * v0.X + normal.Y * v0.Y + normal.Z * v0.Z)
            );
        }

        public static Plane3D operator +(Plane3D g) {
            return g;
        }

        public static Plane3D operator -(Plane3D g) {
            return new(-g.Normal, -g.D, 0);
        }

        public static Plane3D operator +(Plane3D g, Vector3D v) {
            return new(-g.Normal, g.D - (g.A * v.X + g.B * v.Y + g.C * v.Z), 0);
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

        public static Plane3D operator *(Plane3D g, ddouble r) {
            return new(g.Normal, g.D * r, 0);
        }

        public static Plane3D operator *(Plane3D g, double r) {
            return new(g.Normal, g.D * r, 0);
        }

        public static Plane3D operator *(ddouble r, Plane3D g) {
            return g * r;
        }

        public static Plane3D operator *(double r, Plane3D g) {
            return g * r;
        }

        public static Plane3D operator /(Plane3D g, ddouble r) {
            return new(g.Normal, g.D / r, 0);
        }

        public static Plane3D operator /(Plane3D g, double r) {
            return new(g.Normal, g.D / r, 0);
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

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Plane3D geo && geo == this;
        }

        public bool Equals(Plane3D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }
    }
}
