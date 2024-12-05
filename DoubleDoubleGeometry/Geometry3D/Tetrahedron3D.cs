using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Tetrahedron3D : IGeometry<Tetrahedron3D, Vector3D>, IFormattable {
        public readonly Vector3D V0, V1, V2, V3;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Matrix3D ortho_matrix;

        public Tetrahedron3D(Vector3D v0, Vector3D v1, Vector3D v2, Vector3D v3) {
            this.V0 = v0;
            this.V1 = v1;
            this.V2 = v2;
            this.V3 = v3;

            Vector3D a = V1 - V0, b = V2 - V0, c = V3 - V0;

            this.ortho_matrix = new Matrix3D(a.X, b.X, c.X, a.Y, b.Y, c.Y, a.Z, b.Z, c.Z).Inverse;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polyhedron3D Polyhedron => new(
            new Connection(4,
                (0, 1), (0, 2), (0, 3), (1, 2), (1, 3), (2, 3)
            ),
            V0, V1, V2, V3
        );

        public Vector3D Point(ddouble u, ddouble v, ddouble w) {
            return V0 + u * (V1 - V0) + (1d - u) * (v * (V2 - V0) + (1d - v) * w * (V3 - V0));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area {
            get {
                Triangle3D t123 = new(V1, V2, V3), t023 = new(V0, V2, V3), t013 = new(V0, V1, V3), t012 = new(V0, V1, V2);

                return t123.Area + t023.Area + t013.Area + t012.Area;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        public ddouble Volume {
            get {
                return ddouble.Abs(Vector3D.Dot(Vector3D.Cross(V1 - V0, V2 - V0), V3 - V0)) / 6d;
            }
        }

        public static Tetrahedron3D operator +(Tetrahedron3D g) {
            return g;
        }

        public static Tetrahedron3D operator -(Tetrahedron3D g) {
            return new(-g.V0, -g.V1, -g.V2, -g.V3);
        }

        public static Tetrahedron3D operator +(Tetrahedron3D g, Vector3D v) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v, g.V3 + v);
        }

        public static Tetrahedron3D operator +(Vector3D v, Tetrahedron3D g) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v, g.V3 + v);
        }

        public static Tetrahedron3D operator -(Tetrahedron3D g, Vector3D v) {
            return new(g.V0 - v, g.V1 - v, g.V2 - v, g.V3 - v);
        }

        public static Tetrahedron3D operator -(Vector3D v, Tetrahedron3D g) {
            return new(v - g.V0, v - g.V1, v - g.V2, v - g.V3);
        }

        public static Tetrahedron3D operator *(Tetrahedron3D g, ddouble r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r, g.V3 * r);
        }

        public static Tetrahedron3D operator *(Tetrahedron3D g, double r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r, g.V3 * r);
        }

        public static Tetrahedron3D operator *(ddouble r, Tetrahedron3D g) {
            return g * r;
        }

        public static Tetrahedron3D operator *(double r, Tetrahedron3D g) {
            return g * r;
        }

        public static Tetrahedron3D operator /(Tetrahedron3D g, ddouble r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r, g.V3 / r);
        }

        public static Tetrahedron3D operator /(Tetrahedron3D g, double r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r, g.V3 / r);
        }

        public static Tetrahedron3D operator *(Matrix3D m, Tetrahedron3D g) {
            return new Tetrahedron3D(m * g.V0, m * g.V1, m * g.V2, m * g.V3);
        }

        public static Tetrahedron3D operator *(Quaternion q, Tetrahedron3D g) {
            return new Tetrahedron3D(q * g.V0, q * g.V1, q * g.V2, q * g.V3);
        }

        public static bool operator ==(Tetrahedron3D g1, Tetrahedron3D g2) {
            return (g1.V0 == g2.V0) && (g1.V1 == g2.V1) && (g1.V2 == g2.V2) && (g1.V3 == g2.V3);
        }

        public static bool operator !=(Tetrahedron3D g1, Tetrahedron3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Tetrahedron3D((Vector3D v0, Vector3D v1, Vector3D v2, Vector3D v3) g) {
            return new(g.v0, g.v1, g.v2, g.v3);
        }

        public static implicit operator (Vector3D v0, Vector3D v1, Vector3D v2, Vector3D v3)(Tetrahedron3D g) {
            return (g.V0, g.V1, g.V2, g.V3);
        }

        public static implicit operator Polyhedron3D(Tetrahedron3D g) {
            return g.Polyhedron;
        }

        public void Deconstruct(out Vector3D v0, out Vector3D v1, out Vector3D v2, out Vector3D v3)
            => (v0, v1, v2, v3) = (V0, V1, V2, V3);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Tetrahedron3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, Vector3D.Invalid, Vector3D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Tetrahedron3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, Vector3D.Zero, Vector3D.Zero);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox3D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox3D BoundingBox => bbox ??= new BoundingBox3D(V0, V1, V2, V3);

        public bool Inside(Vector3D v) {
            Vector3D u = ortho_matrix * (v - V0);

            bool inside = u.X >= 0d && u.Y >= 0d && u.Z >= 0d && u.X + u.Y + u.Z <= 1d;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector3D> vs) {
            foreach (Vector3D v in vs) {
                Vector3D u = ortho_matrix * (v - V0);

                bool inside = u.X >= 0d && u.Y >= 0d && u.Z >= 0d && u.X + u.Y + u.Z <= 1d;

                yield return inside;
            }
        }

        public static bool IsNaN(Tetrahedron3D g) {
            return Vector3D.IsNaN(g.V0) || Vector3D.IsNaN(g.V1) || Vector3D.IsNaN(g.V2) || Vector3D.IsNaN(g.V3);
        }

        public static bool IsZero(Tetrahedron3D g) {
            return Vector3D.IsZero(g.V0) && Vector3D.IsZero(g.V1) && Vector3D.IsZero(g.V2) && Vector3D.IsZero(g.V3);
        }

        public static bool IsFinite(Tetrahedron3D g) {
            return Vector3D.IsFinite(g.V0) && Vector3D.IsFinite(g.V1) && Vector3D.IsFinite(g.V2) && Vector3D.IsFinite(g.V3);
        }

        public static bool IsInfinity(Tetrahedron3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Tetrahedron3D g) {
            return IsFinite(g) && g.V0 != g.V1 && g.V0 != g.V2 && g.V0 != g.V3 && g.V1 != g.V2 && g.V1 != g.V3 && g.V2 != g.V3;
        }

        public static Tetrahedron3D Projection(Plane3D plane, Tetrahedron3D g) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            Tetrahedron3D u = q * g + (0d, 0d, plane.D);

            return u;
        }

        public static IEnumerable<Tetrahedron3D> Projection(Plane3D plane, IEnumerable<Tetrahedron3D> gs) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));
            Vector3D v = (0d, 0d, plane.D);

            foreach (Tetrahedron3D g in gs) {
                Tetrahedron3D u = q * g + v;

                yield return u;
            }
        }

        public override string ToString() {
            return $"{V0}, {V1}, {V2}, {V3}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"{V0.ToString(format)}, {V1.ToString(format)}, {V2.ToString(format)}, {V3.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Tetrahedron3D g && g == this);
        }

        public bool Equals(Tetrahedron3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return V0.GetHashCode() ^ V1.GetHashCode() ^ V2.GetHashCode() ^ V3.GetHashCode();
        }
    }
}
