using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class BoundingBox3D : IGeometry<BoundingBox3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly Vector3D Scale;
        public readonly Vector3D Min, Max;

        public BoundingBox3D(Vector3D center, Vector3D scale) {
            this.Center = center;
            this.Scale = (ddouble.Abs(scale.X), ddouble.Abs(scale.Y), ddouble.Abs(scale.Z));
            this.Max = Center + Scale;
            this.Min = Center - Scale;
        }

        public BoundingBox3D(params Vector3D[] vs) {
            Vector3D min = vs.Min(), max = vs.Max();

            this.Center = (min + max) / 2d;
            this.Scale = (max - min) / 2d;
            this.Max = max;
            this.Min = min;
        }

        public BoundingBox3D(IEnumerable<Vector3D> vs) {
            Vector3D min = vs.Min(), max = vs.Max();

            this.Center = (min + max) / 2d;
            this.Scale = (max - min) / 2d;
            this.Max = max;
            this.Min = min;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polyhedron3D polyhedron = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polyhedron3D Polyhedron
            => polyhedron ??= Center + new Polyhedron3D(
                new Connection(8,
                    (0, 1), (0, 3), (0, 4), (1, 2), (1, 5), (2, 3),
                    (2, 6), (3, 7), (4, 5), (4, 7), (5, 6), (6, 7)
                ),
                (-Scale.X, -Scale.Y, -Scale.Z), (Scale.X, -Scale.Y, -Scale.Z),
                (Scale.X, Scale.Y, -Scale.Z), (-Scale.X, Scale.Y, -Scale.Z),
                (-Scale.X, -Scale.Y, Scale.Z), (Scale.X, -Scale.Y, Scale.Z),
                (Scale.X, Scale.Y, Scale.Z), (-Scale.X, Scale.Y, Scale.Z)
        );

        public ReadOnlyCollection<Vector3D> Vertex => Polyhedron.Vertex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Width => ddouble.Abs(Scale.X) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Height => ddouble.Abs(Scale.Y) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Depth => ddouble.Abs(Scale.Z) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Size => (Width, Height, Depth);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => (Width * (Height + Depth) + Height * Depth) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Volume => Width * Height * Depth;

        public static BoundingBox3D operator +(BoundingBox3D g) {
            return g;
        }

        public static BoundingBox3D operator -(BoundingBox3D g) {
            return new(-g.Center, g.Scale);
        }

        public static BoundingBox3D operator +(BoundingBox3D g, Vector3D v) {
            return new(g.Center + v, g.Scale);
        }

        public static BoundingBox3D operator +(Vector3D v, BoundingBox3D g) {
            return new(g.Center + v, g.Scale);
        }

        public static BoundingBox3D operator -(BoundingBox3D g, Vector3D v) {
            return new(g.Center - v, g.Scale);
        }

        public static BoundingBox3D operator -(Vector3D v, BoundingBox3D g) {
            return new(v - g.Center, g.Scale);
        }

        public static BoundingBox3D operator *(BoundingBox3D g, ddouble r) {
            return new(g.Center * r, g.Scale * r);
        }

        public static BoundingBox3D operator *(BoundingBox3D g, double r) {
            return new(g.Center * r, g.Scale * r);
        }

        public static BoundingBox3D operator *(ddouble r, BoundingBox3D g) {
            return g * r;
        }

        public static BoundingBox3D operator *(double r, BoundingBox3D g) {
            return g * r;
        }

        public static BoundingBox3D operator /(BoundingBox3D g, ddouble r) {
            return new(g.Center / r, g.Scale / r);
        }

        public static BoundingBox3D operator /(BoundingBox3D g, double r) {
            return new(g.Center / r, g.Scale / r);
        }

        public static bool operator ==(BoundingBox3D g1, BoundingBox3D g2) {
            return (g1.Center == g2.Center) && (g1.Scale == g2.Scale);
        }

        public static bool operator !=(BoundingBox3D g1, BoundingBox3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator BoundingBox3D((Vector3D center, Vector3D scale) g) {
            return new(g.center, g.scale);
        }

        public static implicit operator (Vector3D center, Vector3D scale)(BoundingBox3D g) {
            return (g.Center, g.Scale);
        }

        public void Deconstruct(out Vector3D center, out Vector3D scale)
            => (center, scale) = (Center, Scale);

        public static implicit operator (Vector3D v0, Vector3D v1, Vector3D v2, Vector3D v3)(BoundingBox3D g) {
            return (g.Vertex[0], g.Vertex[1], g.Vertex[2], g.Vertex[3]);
        }

        public static implicit operator Polyhedron3D(BoundingBox3D g) {
            return g.Polyhedron;
        }

        public void Deconstruct(out Vector3D v0, out Vector3D v1, out Vector3D v2, out Vector3D v3)
            => (v0, v1, v2, v3) = (Vertex[0], Vertex[1], Vertex[2], Vertex[3]);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static BoundingBox3D Invalid { get; } = new(Vector3D.Invalid, (ddouble.NaN, ddouble.NaN, ddouble.NaN));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static BoundingBox3D Zero { get; } = new(Vector3D.Zero, (ddouble.Zero, ddouble.Zero, ddouble.Zero));

        public bool Inside(Vector3D v) {
            Vector3D u = v - Center;
            ddouble sx = Scale.X, sy = Scale.Y, sz = Scale.Y;

            bool inside = ddouble.Abs(u.X) <= sx && ddouble.Abs(u.Y) <= sy && ddouble.Abs(u.Z) <= sz;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector3D> vs) {
            ddouble sx = Scale.X, sy = Scale.Y, sz = Scale.Y;

            foreach (Vector3D v in vs) {
                Vector3D u = v - Center;

                bool inside = ddouble.Abs(u.X) <= sx && ddouble.Abs(u.Y) <= sy && ddouble.Abs(u.Z) <= sz;

                yield return inside;
            }
        }

        public static bool IsNaN(BoundingBox3D g) {
            return Vector3D.IsNaN(g.Center) || Vector3D.IsNaN(g.Scale);
        }

        public static bool IsZero(BoundingBox3D g) {
            return Vector3D.IsZero(g.Center) && Vector3D.IsZero(g.Scale);
        }

        public static bool IsFinite(BoundingBox3D g) {
            return Vector3D.IsFinite(g.Center) && Vector3D.IsFinite(g.Scale);
        }

        public static bool IsInfinity(BoundingBox3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(BoundingBox3D g) {
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, scale={Scale}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, scale={Scale.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is BoundingBox3D g && g == this);
        }

        public bool Equals(BoundingBox3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Scale.GetHashCode();
        }
    }
}
