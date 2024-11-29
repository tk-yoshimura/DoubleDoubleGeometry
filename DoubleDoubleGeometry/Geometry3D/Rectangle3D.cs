using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Rectangle3D : IGeometry<Rectangle3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly Vector2D Scale;
        public readonly Quaternion Rotation;

        private Rectangle3D(Vector3D center, Vector2D scale, Quaternion rotation, int _) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation;
        }

        public Rectangle3D(Vector3D center, Vector2D scale, Vector3D normal)
            : this(center, scale, Vector3D.Rot((0d, 0d, 1d), normal.Normal), 0) { }

        public Rectangle3D(Vector3D center, Vector2D scale, Quaternion rotation) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation.Normal;
        }

        public Rectangle3D(Rectangle2D rectangle) {
            this.Center = (Vector3D)rectangle.Center;
            this.Scale = rectangle.Scale;
            this.Rotation = Quaternion.One;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polygon3D polygon = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polygon3D Polygon
            => polygon ??= new Polygon3D(
                new Polygon2D((-Scale.X, -Scale.Y), (Scale.X, -Scale.Y), (Scale.X, Scale.Y), (-Scale.X, Scale.Y)),
                Center, Rotation
            );

        public ReadOnlyCollection<Vector3D> Vertex => Polygon.Vertex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D normal = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal => normal ??= Rotation * new Vector3D(0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Plane3D plane = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Plane3D Plane => plane ??= Plane3D.FromNormal(Center, Normal);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Width => ddouble.Abs(Scale.X) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Height => ddouble.Abs(Scale.Y) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Size => (Width, Height);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble LongSide => ddouble.Max(Width, Height);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble ShortSide => ddouble.Min(Width, Height);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Aspect => ddouble.Abs(Scale.Y / Scale.X);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Width * Height;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 2d * (Width + Height);

        public static Rectangle3D operator +(Rectangle3D g) {
            return g;
        }

        public static Rectangle3D operator -(Rectangle3D g) {
            return new(-g.Center, -g.Scale, g.Rotation, 0);
        }

        public static Rectangle3D operator +(Rectangle3D g, Vector3D v) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle3D operator +(Vector3D v, Rectangle3D g) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle3D operator -(Rectangle3D g, Vector3D v) {
            return new(g.Center - v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle3D operator -(Vector3D v, Rectangle3D g) {
            return new(v - g.Center, -g.Scale, g.Rotation, 0);
        }

        public static Rectangle3D operator *(Quaternion q, Rectangle3D g) {
            return new(q * g.Center, q.SquareNorm * g.Scale, q.Normal * g.Rotation, 0);
        }

        public static Rectangle3D operator *(Rectangle3D g, ddouble r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Rectangle3D operator *(Rectangle3D g, double r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Rectangle3D operator *(ddouble r, Rectangle3D g) {
            return g * r;
        }

        public static Rectangle3D operator *(double r, Rectangle3D g) {
            return g * r;
        }

        public static Rectangle3D operator /(Rectangle3D g, ddouble r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static Rectangle3D operator /(Rectangle3D g, double r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static bool operator ==(Rectangle3D g1, Rectangle3D g2) {
            return (g1.Center == g2.Center) && (g1.Scale == g2.Scale) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Rectangle3D g1, Rectangle3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Rectangle3D((Vector3D center, Vector2D scale, Quaternion rotation) g) {
            return new(g.center, g.scale, g.rotation);
        }

        public static implicit operator (Vector3D center, Vector2D scale, Quaternion rotation)(Rectangle3D g) {
            return (g.Center, g.Scale, g.Rotation);
        }

        public void Deconstruct(out Vector3D center, out Vector2D scale, out Quaternion rotation)
            => (center, scale, rotation) = (Center, Scale, Rotation);

        public static implicit operator (Vector3D v0, Vector3D v1, Vector3D v2, Vector3D v3)(Rectangle3D g) {
            return (g.Vertex[0], g.Vertex[1], g.Vertex[2], g.Vertex[3]);
        }

        public static implicit operator Polygon3D(Rectangle3D g) {
            return g.Polygon;
        }

        public void Deconstruct(out Vector3D v0, out Vector3D v1, out Vector3D v2, out Vector3D v3)
            => (v0, v1, v2, v3) = (Vertex[0], Vertex[1], Vertex[2], Vertex[3]);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rectangle3D Invalid { get; } = new(Vector3D.Invalid, Vector2D.Invalid, Quaternion.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rectangle3D Zero { get; } = new(Vector3D.Zero, Vector2D.Zero, Quaternion.Zero, 0);

        public static bool IsNaN(Rectangle3D g) {
            return Vector3D.IsNaN(g.Center) || Vector2D.IsNaN(g.Scale) || Quaternion.IsNaN(g.Rotation);
        }

        public static bool IsZero(Rectangle3D g) {
            return Vector3D.IsZero(g.Center) && Vector2D.IsZero(g.Scale) && Quaternion.IsZero(g.Rotation);
        }

        public static bool IsFinite(Rectangle3D g) {
            return Vector3D.IsFinite(g.Center) && Vector2D.IsFinite(g.Scale) && Quaternion.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Rectangle3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Rectangle3D g) {
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, scale={Scale}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, scale=({Scale.ToString(format)}, rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Rectangle3D g && g == this);
        }

        public bool Equals(Rectangle3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Scale.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
