using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class BoundingBox2D : IGeometry<BoundingBox2D, Vector2D>, IFormattable {
        public readonly Vector2D Center;
        public readonly Vector2D Scale;
        public readonly Vector2D Min, Max;

        public BoundingBox2D(Vector2D center, Vector2D scale) {
            this.Center = center;
            this.Scale = (ddouble.Abs(scale.X), ddouble.Abs(scale.Y));
            this.Max = Center + Scale;
            this.Min = Center - Scale;
        }

        public BoundingBox2D(params Vector2D[] vs) {
            Vector2D min = vs.Min(), max = vs.Max();

            this.Center = (min + max) / 2d;
            this.Scale = (max - min) / 2d;
            this.Max = max;
            this.Min = min;
        }

        public BoundingBox2D(IEnumerable<Vector2D> vs) {
            Vector2D min = vs.Min(), max = vs.Max();

            this.Center = (min + max) / 2d;
            this.Scale = (max - min) / 2d;
            this.Max = max;
            this.Min = min;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polygon2D polygon = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polygon2D Polygon
            => polygon ??= Center + new Polygon2D(
                (-Scale.X, -Scale.Y), (Scale.X, -Scale.Y), (Scale.X, Scale.Y), (-Scale.X, Scale.Y)
        );

        public ReadOnlyCollection<Vector2D> Vertex => Polygon.Vertex;

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

        public static BoundingBox2D operator +(BoundingBox2D g) {
            return g;
        }

        public static BoundingBox2D operator -(BoundingBox2D g) {
            return new(-g.Center, g.Scale);
        }

        public static BoundingBox2D operator +(BoundingBox2D g, Vector2D v) {
            return new(g.Center + v, g.Scale);
        }

        public static BoundingBox2D operator +(Vector2D v, BoundingBox2D g) {
            return new(g.Center + v, g.Scale);
        }

        public static BoundingBox2D operator -(BoundingBox2D g, Vector2D v) {
            return new(g.Center - v, g.Scale);
        }

        public static BoundingBox2D operator -(Vector2D v, BoundingBox2D g) {
            return new(v - g.Center, g.Scale);
        }

        public static BoundingBox2D operator *(BoundingBox2D g, ddouble r) {
            return new(g.Center * r, g.Scale * r);
        }

        public static BoundingBox2D operator *(BoundingBox2D g, double r) {
            return new(g.Center * r, g.Scale * r);
        }

        public static BoundingBox2D operator *(ddouble r, BoundingBox2D g) {
            return g * r;
        }

        public static BoundingBox2D operator *(double r, BoundingBox2D g) {
            return g * r;
        }

        public static BoundingBox2D operator /(BoundingBox2D g, ddouble r) {
            return new(g.Center / r, g.Scale / r);
        }

        public static BoundingBox2D operator /(BoundingBox2D g, double r) {
            return new(g.Center / r, g.Scale / r);
        }

        public static bool operator ==(BoundingBox2D g1, BoundingBox2D g2) {
            return (g1.Center == g2.Center) && (g1.Scale == g2.Scale);
        }

        public static bool operator !=(BoundingBox2D g1, BoundingBox2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator BoundingBox2D((Vector2D center, Vector2D scale) g) {
            return new(g.center, g.scale);
        }

        public static implicit operator (Vector2D center, Vector2D scale)(BoundingBox2D g) {
            return (g.Center, g.Scale);
        }

        public void Deconstruct(out Vector2D center, out Vector2D scale)
            => (center, scale) = (Center, Scale);

        public static implicit operator (Vector2D v0, Vector2D v1, Vector2D v2, Vector2D v3)(BoundingBox2D g) {
            return (g.Vertex[0], g.Vertex[1], g.Vertex[2], g.Vertex[3]);
        }

        public static implicit operator Polygon2D(BoundingBox2D g) {
            return g.Polygon;
        }

        public void Deconstruct(out Vector2D v0, out Vector2D v1, out Vector2D v2, out Vector2D v3)
            => (v0, v1, v2, v3) = (Vertex[0], Vertex[1], Vertex[2], Vertex[3]);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static BoundingBox2D Invalid { get; } = new(Vector2D.Invalid, (ddouble.NaN, ddouble.NaN));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static BoundingBox2D Zero { get; } = new(Vector2D.Zero, (ddouble.Zero, ddouble.Zero));

        public bool Inside(Vector2D v) {
            Vector2D u = v - Center;
            ddouble sx = Scale.X, sy = Scale.Y;

            bool inside = ddouble.Abs(u.X) <= sx && ddouble.Abs(u.Y) <= sy;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector2D> vs) {
            ddouble sx = Scale.X, sy = Scale.Y;

            foreach (Vector2D v in vs) {
                Vector2D u = v - Center;

                bool inside = ddouble.Abs(u.X) <= sx && ddouble.Abs(u.Y) <= sy;

                yield return inside;
            }
        }

        public static bool IsNaN(BoundingBox2D g) {
            return Vector2D.IsNaN(g.Center) || Vector2D.IsNaN(g.Scale);
        }

        public static bool IsZero(BoundingBox2D g) {
            return Vector2D.IsZero(g.Center) && Vector2D.IsZero(g.Scale);
        }

        public static bool IsFinite(BoundingBox2D g) {
            return Vector2D.IsFinite(g.Center) && Vector2D.IsFinite(g.Scale);
        }

        public static bool IsInfinity(BoundingBox2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(BoundingBox2D g) {
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
            return ReferenceEquals(this, obj) || (obj is not null && obj is BoundingBox2D g && g == this);
        }

        public bool Equals(BoundingBox2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Scale.GetHashCode();
        }
    }
}
