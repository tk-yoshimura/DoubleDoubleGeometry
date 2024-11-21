using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Ellipse3D : IGeometry<Ellipse3D, Vector3D>, IFormattable {
        public readonly Vector3D Center, Normal;
        public readonly (ddouble major, ddouble minor) Axis;
        public readonly ddouble Rotation;

        private Ellipse3D(Vector3D center, Vector3D normal, (ddouble major, ddouble minor) axis, ddouble rotation, int _) {
            this.Center = center;
            this.Normal = normal;
            this.Axis = axis;
            this.Rotation = rotation % ddouble.Pi;
        }

        public Ellipse3D(Vector3D center, Vector3D normal, (ddouble major, ddouble minor) axis, ddouble angle) {
            this.Center = center;
            this.Normal = normal.Normal;
            this.Axis = axis;
            this.Rotation = angle % ddouble.Pi;
        }

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Quaternion? rot = null;
#pragma warning restore CS8632
        public Vector3D Point(ddouble t) {
            rot ??= Vector3D.Rot((0d, 0d, 1d), Normal);

            ddouble cs = ddouble.Cos(Rotation), sn = ddouble.Sin(Rotation);
            ddouble a = ddouble.Cos(t) * Axis.major, b = ddouble.Sin(t) * Axis.minor;

            return Center + rot * new Vector3D(cs * a - sn * b, sn * a + cs * b, 0d);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Axis.minor * Axis.major * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 4d * Axis.major * ddouble.EllipticE(1d - ddouble.Square(Axis.minor / Axis.major));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Focus => ddouble.Sqrt(ddouble.Square(Axis.major) - ddouble.Square(Axis.minor));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Eccentricity => ddouble.Sqrt(1d - ddouble.Square(Axis.minor / Axis.major));

        public static Ellipse3D operator +(Ellipse3D g) {
            return g;
        }

        public static Ellipse3D operator -(Ellipse3D g) {
            return new(-g.Center, -g.Normal, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator +(Ellipse3D g, Vector3D v) {
            return new(g.Center + v, g.Normal, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator +(Vector3D v, Ellipse3D g) {
            return new(g.Center + v, g.Normal, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator -(Ellipse3D g, Vector3D v) {
            return new(g.Center - v, g.Normal, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator -(Vector3D v, Ellipse3D g) {
            return new(v - g.Center, -g.Normal, g.Axis, g.Rotation, 0);
        }

        public static Ellipse3D operator *(Quaternion q, Ellipse3D g) {
            ddouble norm = q.Norm;

            return new(g.Center * norm, (q / norm) * g.Normal, (g.Axis.major * norm, g.Axis.minor * norm), g.Rotation);
        }

        public static Ellipse3D operator *(Ellipse3D g, ddouble r) {
            return new(g.Center * r, g.Normal * ddouble.Sign(r), (g.Axis.major * ddouble.Abs(r), g.Axis.minor * ddouble.Abs(r)), g.Rotation, 0);
        }

        public static Ellipse3D operator *(Ellipse3D g, double r) {
            return new(g.Center * r, g.Normal * double.Sign(r), (g.Axis.major * double.Abs(r), g.Axis.minor * double.Abs(r)), g.Rotation, 0);
        }

        public static Ellipse3D operator *(ddouble r, Ellipse3D g) {
            return g * r;
        }

        public static Ellipse3D operator *(double r, Ellipse3D g) {
            return g * r;
        }

        public static Ellipse3D operator /(Ellipse3D g, ddouble r) {
            return new(g.Center / r, g.Normal * ddouble.Sign(r), (g.Axis.major / ddouble.Abs(r), g.Axis.minor / ddouble.Abs(r)), g.Rotation, 0);
        }

        public static Ellipse3D operator /(Ellipse3D g, double r) {
            return new(g.Center / r, g.Normal * double.Sign(r), (g.Axis.major / double.Abs(r), g.Axis.minor / double.Abs(r)), g.Rotation, 0);
        }

        public static bool operator ==(Ellipse3D g1, Ellipse3D g2) {
            return (g1.Center == g2.Center) && (g1.Axis == g2.Axis) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Ellipse3D g1, Ellipse3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Ellipse3D((Vector3D center, Vector3D normal, (ddouble major, ddouble minor) axis, ddouble angle) g) {
            return new(g.center, g.normal, g.axis, g.angle);
        }

        public static implicit operator (Vector3D center, Vector3D normal, (ddouble major, ddouble minor) axis, ddouble angle)(Ellipse3D g) {
            return (g.Center, g.Normal, g.Axis, g.Rotation);
        }

        public void Deconstruct(out Vector3D center, out Vector3D normal, out (ddouble major, ddouble minor) axis, out ddouble angle)
            => (center, normal, axis, angle) = (Center, Normal, Axis, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, (ddouble.NaN, ddouble.NaN), ddouble.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, (ddouble.Zero, ddouble.Zero), ddouble.Zero, 0);

        public static bool IsNaN(Ellipse3D g) {
            return Vector3D.IsNaN(g.Center) || Vector3D.IsNaN(g.Normal) || ddouble.IsNaN(g.Axis.major) || ddouble.IsNaN(g.Axis.minor) || ddouble.IsNaN(g.Rotation);
        }

        public static bool IsZero(Ellipse3D g) {
            return Vector3D.IsZero(g.Center) && Vector3D.IsZero(g.Normal) && ddouble.IsZero(g.Axis.major) && ddouble.IsZero(g.Axis.minor) && ddouble.IsZero(g.Rotation);
        }

        public static bool IsFinite(Ellipse3D g) {
            return Vector3D.IsFinite(g.Center) && Vector3D.IsFinite(g.Normal) && ddouble.IsFinite(g.Axis.major) && ddouble.IsFinite(g.Axis.minor) && ddouble.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Ellipse3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Ellipse3D g) {
            return IsFinite(g) && !Vector3D.IsZero(g.Normal) && g.Axis.major >= 0d && g.Axis.minor >= 0d && g.Axis.major >= g.Axis.minor;
        }

        public override string ToString() {
            return $"center={Center}, normal={Normal}, axis={Axis}, angle={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, normal={Normal.ToString(format)}, axis=({Axis.major.ToString(format)}, {Axis.minor.ToString(format)}), angle={Rotation.ToString(format)}";
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
            return Center.GetHashCode() ^ Normal.GetHashCode() ^ Axis.major.GetHashCode() ^ Axis.minor.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
