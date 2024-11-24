using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Rectangle2D : IGeometry<Rectangle2D, Vector2D>, IFormattable {
        public readonly Vector2D Center;
        public readonly Vector2D Scale;
        public readonly Complex Rotation;

        private Rectangle2D(Vector2D center, Vector2D scale, Complex rotation, int _) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation;
        }

        public Rectangle2D(Vector2D center, Vector2D scale, ddouble angle)
            : this(center, scale, Complex.FromPhase(angle), 0) { }

        public Rectangle2D(Vector2D center, Vector2D scale, Complex rotation) {
            this.Center = center;
            this.Scale = scale;
            this.Rotation = rotation.Normal;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polygon2D polygon = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polygon2D Polygon
            => polygon ??= Center + Rotation * new Polygon2D(
                (-Scale.X, -Scale.Y), (Scale.X, -Scale.Y), (Scale.X, Scale.Y), (-Scale.X, Scale.Y)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<Vector2D> Vertex => Polygon.Vertex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Width => ddouble.Abs(Scale.X) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Height => ddouble.Abs(Scale.Y) * 2d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble LongSide => ddouble.Max(Width, Height);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble ShortSide => ddouble.Min(Width, Height);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Aspect => ddouble.Abs(Scale.Y / Scale.X);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Angle => Rotation.Phase;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Width * Height;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 2d * (Width + Height);

        public static Rectangle2D operator +(Rectangle2D g) {
            return g;
        }

        public static Rectangle2D operator -(Rectangle2D g) {
            return new(-g.Center, -g.Scale, g.Rotation, 0);
        }

        public static Rectangle2D operator +(Rectangle2D g, Vector2D v) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle2D operator +(Vector2D v, Rectangle2D g) {
            return new(g.Center + v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle2D operator -(Rectangle2D g, Vector2D v) {
            return new(g.Center - v, g.Scale, g.Rotation, 0);
        }

        public static Rectangle2D operator -(Vector2D v, Rectangle2D g) {
            return new(v - g.Center, -g.Scale, g.Rotation, 0);
        }

        public static Rectangle2D operator *(Complex c, Rectangle2D g) {
            ddouble norm = c.Norm;

            return new(c * g.Center, norm * g.Scale, (c / norm) * g.Rotation, 0);
        }

        public static Rectangle2D operator *(Rectangle2D g, ddouble r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Rectangle2D operator *(Rectangle2D g, double r) {
            return new(g.Center * r, g.Scale * r, g.Rotation, 0);
        }

        public static Rectangle2D operator *(ddouble r, Rectangle2D g) {
            return g * r;
        }

        public static Rectangle2D operator *(double r, Rectangle2D g) {
            return g * r;
        }

        public static Rectangle2D operator /(Rectangle2D g, ddouble r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static Rectangle2D operator /(Rectangle2D g, double r) {
            return new(g.Center / r, g.Scale / r, g.Rotation, 0);
        }

        public static bool operator ==(Rectangle2D g1, Rectangle2D g2) {
            return (g1.Center == g2.Center) && (g1.Scale == g2.Scale) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Rectangle2D g1, Rectangle2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Rectangle2D((Vector2D center, Vector2D scale, Complex rotation) g) {
            return new(g.center, g.scale, g.rotation);
        }

        public static implicit operator (Vector2D center, Vector2D scale, Complex rotation)(Rectangle2D g) {
            return (g.Center, g.Scale, g.Rotation);
        }

        public void Deconstruct(out Vector2D center, out Vector2D scale, out Complex rotation)
            => (center, scale, rotation) = (Center, Scale, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rectangle2D Invalid { get; } = new(Vector2D.Invalid, (ddouble.NaN, ddouble.NaN), ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rectangle2D Zero { get; } = new(Vector2D.Zero, (ddouble.Zero, ddouble.Zero), ddouble.Zero);

        public static bool IsNaN(Rectangle2D g) {
            return Vector2D.IsNaN(g.Center) || Vector2D.IsNaN(g.Scale) || Complex.IsNaN(g.Rotation);
        }

        public static bool IsZero(Rectangle2D g) {
            return Vector2D.IsZero(g.Center) && Vector2D.IsZero(g.Scale) && Complex.IsFinite(g.Rotation);
        }

        public static bool IsFinite(Rectangle2D g) {
            return Vector2D.IsFinite(g.Center) && Vector2D.IsFinite(g.Scale) && Complex.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Rectangle2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Rectangle2D g) {
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, scale={Scale}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, scale=({Scale.ToString(format)}), rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Rectangle2D g && g == this);
        }

        public bool Equals(Rectangle2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Scale.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
