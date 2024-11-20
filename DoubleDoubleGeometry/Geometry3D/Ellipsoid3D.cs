using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Ellipsoid3D : IGeometry<Ellipsoid3D, Vector3D>, IFormattable {
        public readonly Vector3D Center;
        public readonly Vector3D Axis;
        public readonly Quaternion Rotation;

        public Ellipsoid3D(Vector3D center, Vector3D axis, Quaternion rotation, int _) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation;
        }

        public Ellipsoid3D(Vector3D center, Vector3D axis, Quaternion rotation) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation.Normal;
        }

        public Vector3D Point(ddouble theta, ddouble phi) {
            ddouble ct = ddouble.Cos(theta), st = ddouble.Sin(theta);
            ddouble cp = ddouble.Cos(phi), sp = ddouble.Sin(phi);

            return Center + Rotation * new Vector3D(st * cp * Axis.X, st * sp * Axis.Y, ct * Axis.Z);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area {
            get {
                ddouble a = Axis.X, b = Axis.Y, c = Axis.Z;

                ddouble s = 4d * ddouble.Pi * a * b * c * ddouble.CarlsonRG(1d / (a * a), 1d / (b * b), 1d / (c * c));

                return s;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Volume => Axis.X * Axis.Y * Axis.Z * ddouble.Pi / 0.75d;

        public static Ellipsoid3D operator +(Ellipsoid3D g) {
            return g;
        }

        public static Ellipsoid3D operator -(Ellipsoid3D g) {
            return new(-g.Center, g.Axis, g.Rotation, 0);
        }

        public static Ellipsoid3D operator +(Ellipsoid3D g, Vector3D v) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipsoid3D operator +(Vector3D v, Ellipsoid3D g) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipsoid3D operator -(Ellipsoid3D g, Vector3D v) {
            return new(g.Center - v, g.Axis, g.Rotation, 0);
        }

        public static Ellipsoid3D operator -(Vector3D v, Ellipsoid3D g) {
            return new(v - g.Center, g.Axis, g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(Quaternion q, Ellipsoid3D g) {
            ddouble norm = q.Norm;

            return new(g.Center * norm, g.Axis * norm, (q / norm) * g.Rotation);
        }

        public static Ellipsoid3D operator *(Ellipsoid3D g, ddouble r) {
            return new(g.Center * r, g.Axis * ddouble.Abs(r), g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(Ellipsoid3D g, double r) {
            return new(g.Center * r, g.Axis * double.Abs(r), g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(ddouble r, Ellipsoid3D g) {
            return g * r;
        }

        public static Ellipsoid3D operator *(double r, Ellipsoid3D g) {
            return g * r;
        }

        public static Ellipsoid3D operator /(Ellipsoid3D g, ddouble r) {
            return new(g.Center / r, g.Axis / ddouble.Abs(r), g.Rotation, 0);
        }

        public static Ellipsoid3D operator /(Ellipsoid3D g, double r) {
            return new(g.Center / r, g.Axis / double.Abs(r), g.Rotation, 0);
        }

        public static bool operator ==(Ellipsoid3D g1, Ellipsoid3D g2) {
            return (g1.Center == g2.Center) && (g1.Axis == g2.Axis) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Ellipsoid3D g1, Ellipsoid3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Ellipsoid3D((Vector3D center, Vector3D axis, Quaternion rotation) g) {
            return new(g.center, g.axis, g.rotation);
        }

        public static implicit operator (Vector3D center, Vector3D axis, Quaternion rotation)(Ellipsoid3D g) {
            return (g.Center, g.Axis, g.Rotation);
        }

        public void Deconstruct(out Vector3D center, out Vector3D axis, out Quaternion rotation)
            => (center, axis, rotation) = (Center, Axis, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipsoid3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, Quaternion.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipsoid3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, Quaternion.Zero, 0);

        public static bool IsNaN(Ellipsoid3D g) {
            return Vector3D.IsNaN(g.Center) || Vector3D.IsNaN(g.Axis) || Quaternion.IsNaN(g.Rotation);
        }

        public static bool IsZero(Ellipsoid3D g) {
            return Vector3D.IsZero(g.Center) && Vector3D.IsZero(g.Axis) && Quaternion.IsZero(g.Rotation);
        }

        public static bool IsFinite(Ellipsoid3D g) {
            return Vector3D.IsFinite(g.Center) && Vector3D.IsFinite(g.Axis) && Quaternion.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Ellipsoid3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Ellipsoid3D g) {
            return IsFinite(g) && !Quaternion.IsZero(g.Rotation);
        }

        public override string ToString() {
            return $"center={Center}, axis={Axis}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, axis={Axis.ToString(format)}, rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Ellipsoid3D g && g == this);
        }

        public bool Equals(Ellipsoid3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Axis.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
