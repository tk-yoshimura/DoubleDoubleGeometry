using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Cuboid3D : IGeometry<Cuboid3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly Vector3D Scale;
        public readonly Quaternion Rotation;

        public Cuboid3D(Vector3D center, Vector3D scale, Quaternion rotation, int _) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation;
        }

        public Cuboid3D(Vector3D center, Vector3D scale, Quaternion rotation) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation.Normal;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polyhedron3D polyhedron = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polyhedron3D Polyhedron
            => polyhedron ??= Center + Rotation * new Polyhedron3D(
                new Connection(8,
                    (0, 1), (0, 3), (0, 4), (1, 2), (1, 5), (2, 3), 
                    (2, 6), (3, 7), (4, 5), (4, 7), (5, 6), (6, 7)
                ),
                (-Scale.X, -Scale.Y, -Scale.Z), (Scale.X, -Scale.Y, -Scale.Z), 
                (Scale.X, Scale.Y, -Scale.Z), (-Scale.X, Scale.Y, -Scale.Z),
                (-Scale.X, -Scale.Y, Scale.Z), (Scale.X, -Scale.Y, Scale.Z), 
                (Scale.X, Scale.Y, Scale.Z), (-Scale.X, Scale.Y, Scale.Z)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<Vector3D> Vertex => Polyhedron.Vertex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Width => ddouble.Abs(Scale.X) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Height => ddouble.Abs(Scale.Y) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Depth => ddouble.Abs(Scale.Z) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => (Width * Height + Height * Depth + Width * Depth) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Volume => Width * Height * Depth;

        public static Cuboid3D operator +(Cuboid3D g) {
            return g;
        }

        public static Cuboid3D operator -(Cuboid3D g) {
            return new(-g.Center, g.Scale, g.Rotation, 0);
        }

        public static Cuboid3D operator +(Cuboid3D g, Vector3D v) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Cuboid3D operator +(Vector3D v, Cuboid3D g) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Cuboid3D operator -(Cuboid3D g, Vector3D v) {
            return new(g.Center - v, g.Scale, g.Rotation, 0);
        }

        public static Cuboid3D operator -(Vector3D v, Cuboid3D g) {
            return new(v - g.Center, g.Scale, g.Rotation, 0);
        }

        public static Cuboid3D operator *(Quaternion q, Cuboid3D g) {
            return new(q * g.Center, q.SquareNorm * g.Scale, q.Normal * g.Rotation, 0);
        }

        public static Cuboid3D operator *(Cuboid3D g, ddouble r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Cuboid3D operator *(Cuboid3D g, double r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Cuboid3D operator *(ddouble r, Cuboid3D g) {
            return g * r;
        }

        public static Cuboid3D operator *(double r, Cuboid3D g) {
            return g * r;
        }

        public static Cuboid3D operator /(Cuboid3D g, ddouble r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static Cuboid3D operator /(Cuboid3D g, double r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static bool operator ==(Cuboid3D g1, Cuboid3D g2) {
            return (g1.Center == g2.Center) && (g1.Scale == g2.Scale) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Cuboid3D g1, Cuboid3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Cuboid3D((Vector3D center, Vector3D scale, Quaternion rotation) g) {
            return new(g.center, g.scale, g.rotation);
        }

        public static implicit operator (Vector3D center, Vector3D scale, Quaternion rotation)(Cuboid3D g) {
            return (g.Center, g.Scale, g.Rotation);
        }

        public void Deconstruct(out Vector3D center, out Vector3D scale, out Quaternion rotation)
            => (center, scale, rotation) = (Center, Scale, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Cuboid3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, Quaternion.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Cuboid3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, Quaternion.Zero, 0);

        public static bool IsNaN(Cuboid3D g) {
            return Vector3D.IsNaN(g.Center) || Vector3D.IsNaN(g.Scale) || Quaternion.IsNaN(g.Rotation);
        }

        public static bool IsZero(Cuboid3D g) {
            return Vector3D.IsZero(g.Center) && Vector3D.IsZero(g.Scale) && Quaternion.IsZero(g.Rotation);
        }

        public static bool IsFinite(Cuboid3D g) {
            return Vector3D.IsFinite(g.Center) && Vector3D.IsFinite(g.Scale) && Quaternion.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Cuboid3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Cuboid3D g) {
            return IsFinite(g) && !Quaternion.IsZero(g.Rotation);
        }

        public override string ToString() {
            return $"center={Center}, scale={Scale}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, scale={Scale.ToString(format)}, rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Cuboid3D g && g == this);
        }

        public bool Equals(Cuboid3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Scale.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
