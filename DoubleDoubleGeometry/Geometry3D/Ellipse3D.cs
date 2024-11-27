using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Ellipse3D : IGeometry<Ellipse3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly Vector2D Axis;
        public readonly Quaternion Rotation;

        private Ellipse3D(Vector3D center, Vector2D axis, Quaternion rotation, int _) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation;
        }

        public Ellipse3D(Vector3D center, Vector2D axis, Vector3D normal)
            : this(center, axis, Vector3D.Rot((0d, 0d, 1d), normal.Normal), 0) { }

        public Ellipse3D(Vector3D center, Vector2D axis, Quaternion rotation) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation.Normal;
        }

        public Ellipse3D(Ellipse2D ellipse) {
            this.Center = (Vector3D)ellipse.Center;
            this.Axis = ellipse.Axis;
            this.Rotation = Quaternion.One;
        }

        public Vector3D Point(ddouble t) {
            return Center + Rotation * new Vector3D(ddouble.Cos(t) * Axis.X, ddouble.Sin(t) * Axis.Y, 0d);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble MajorAxis => ddouble.Max(ddouble.Abs(Axis.X), ddouble.Abs(Axis.Y));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble MinorAxis => ddouble.Min(ddouble.Abs(Axis.X), ddouble.Abs(Axis.Y));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal => Rotation * new Vector3D(0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => ddouble.Abs(Axis.X * Axis.Y) * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 4d * MajorAxis * ddouble.EllipticE(1d - ddouble.Square(MinorAxis / MajorAxis));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Focus => ddouble.Sqrt(ddouble.Square(MajorAxis) - ddouble.Square(MinorAxis));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Eccentricity => ddouble.Sqrt(1d - ddouble.Square(MinorAxis / MajorAxis));

        public static Ellipse3D operator +(Ellipse3D g) {
            return g;
        }

        public static Ellipse3D operator -(Ellipse3D g) {
            return new(-g.Center, -g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator +(Ellipse3D g, Vector3D v) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator +(Vector3D v, Ellipse3D g) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator -(Ellipse3D g, Vector3D v) {
            return new(g.Center - v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator -(Vector3D v, Ellipse3D g) {
            return new(v - g.Center, -g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator *(Quaternion q, Ellipse3D g) {
            return new(q * g.Center, q.SquareNorm * g.Axis, q.Normal * g.Rotation, 0);
        }

        public static Ellipse3D operator *(Ellipse3D g, ddouble r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipse3D operator *(Ellipse3D g, double r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipse3D operator *(ddouble r, Ellipse3D g) {
            return g * r;
        }

        public static Ellipse3D operator *(double r, Ellipse3D g) {
            return g * r;
        }

        public static Ellipse3D operator /(Ellipse3D g, ddouble r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
        }

        public static Ellipse3D operator /(Ellipse3D g, double r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
        }

        public static bool operator ==(Ellipse3D g1, Ellipse3D g2) {
            return (g1.Center == g2.Center) && (g1.Axis == g2.Axis) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Ellipse3D g1, Ellipse3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Ellipse3D((Vector3D center, Vector2D axis, Quaternion rotation) g) {
            return new(g.center, g.axis, g.rotation);
        }

        public static implicit operator (Vector3D center, Vector2D axis, Quaternion rotation)(Ellipse3D g) {
            return (g.Center, g.Axis, g.Rotation);
        }

        public void Deconstruct(out Vector3D center, out Vector2D axis, out Quaternion rotation)
            => (center, axis, rotation) = (Center, Axis, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse3D Invalid { get; } = new(Vector3D.Invalid, Vector2D.Invalid, Quaternion.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse3D Zero { get; } = new(Vector3D.Zero, Vector2D.Zero, Quaternion.Zero, 0);

        public static bool IsNaN(Ellipse3D g) {
            return Vector3D.IsNaN(g.Center) || Vector2D.IsNaN(g.Axis) || Quaternion.IsNaN(g.Rotation);
        }

        public static bool IsZero(Ellipse3D g) {
            return Vector3D.IsZero(g.Center) && Vector2D.IsZero(g.Axis) && Quaternion.IsZero(g.Rotation);
        }

        public static bool IsFinite(Ellipse3D g) {
            return Vector3D.IsFinite(g.Center) && Vector2D.IsFinite(g.Axis) && Quaternion.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Ellipse3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Ellipse3D g) {
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, axis={Axis}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, axis=({Axis.ToString(format)}, rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Ellipse3D g && g == this);
        }

        public bool Equals(Ellipse3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Axis.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
