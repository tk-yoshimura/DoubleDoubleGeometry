using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
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

                ddouble s = 4d * ddouble.Pi * ddouble.Abs(a * b * c) * ddouble.CarlsonRG(1d / (a * a), 1d / (b * b), 1d / (c * c));

                return s;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Volume => ddouble.Abs(Axis.X * Axis.Y * Axis.Z) * ddouble.Pi / 0.75d;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EllipsoidImplicitParameter implicit_param = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble A => (implicit_param ??= new EllipsoidImplicitParameter(this)).A;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble B => (implicit_param ??= new EllipsoidImplicitParameter(this)).B;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble C => (implicit_param ??= new EllipsoidImplicitParameter(this)).C;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble D => (implicit_param ??= new EllipsoidImplicitParameter(this)).D;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble E => (implicit_param ??= new EllipsoidImplicitParameter(this)).E;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble F => (implicit_param ??= new EllipsoidImplicitParameter(this)).F;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble G => (implicit_param ??= new EllipsoidImplicitParameter(this)).G;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble H => (implicit_param ??= new EllipsoidImplicitParameter(this)).H;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble I => (implicit_param ??= new EllipsoidImplicitParameter(this)).I;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble J => (implicit_param ??= new EllipsoidImplicitParameter(this)).J;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public (ddouble a, ddouble b, ddouble c, ddouble d, ddouble e, ddouble f, ddouble g, ddouble h, ddouble i, ddouble j)
            ImplicitParameter => implicit_param ??= new EllipsoidImplicitParameter(this);

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
            return new(q * g.Center, q.SquareNorm * g.Axis, q.Normal * g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(Ellipsoid3D g, ddouble r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(Ellipsoid3D g, double r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipsoid3D operator *(ddouble r, Ellipsoid3D g) {
            return g * r;
        }

        public static Ellipsoid3D operator *(double r, Ellipsoid3D g) {
            return g * r;
        }

        public static Ellipsoid3D operator /(Ellipsoid3D g, ddouble r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
        }

        public static Ellipsoid3D operator /(Ellipsoid3D g, double r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox3D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox3D BoundingBox {
            get {
                if (bbox is not null) {
                    return bbox;
                }

                EllipsoidImplicitParameter param = new(Axis, Rotation);

                (ddouble a, ddouble b, ddouble c, ddouble d, ddouble e, ddouble f, ddouble j)
                    = (param.A, param.B, param.C, param.D, param.E, param.F, param.J);

                ddouble f2 = f * f, e2 = e * e, d2 = d * d;

                ddouble m = j / (a * f2 + b * e2 + c * d2 - d * e * f - 4d * a * b * c);

                ddouble x = ddouble.Sqrt((4d * b * c - f2) * m);
                ddouble y = ddouble.Sqrt((4d * a * c - e2) * m);
                ddouble z = ddouble.Sqrt((4d * a * b - d2) * m);

                return bbox ??= new BoundingBox3D(Center, (x, y, z));
            }
        }

        public bool Inside(Vector3D v) {
            if (!BoundingBox.Inside(v)) {
                return false;
            }

            bool inside = ((Rotation.Conj * (v - Center)) / (ddouble.Abs(Axis.X), ddouble.Abs(Axis.Y), ddouble.Abs(Axis.Z))).SquareNorm <= 1d;

            return inside;
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector3D> vs) {
            BoundingBox3D bbox = BoundingBox;
            Matrix3D m = Matrix3D.Scale(1d / ddouble.Abs(Axis.X), 1d / ddouble.Abs(Axis.Y), 1d / ddouble.Abs(Axis.Z)) * new Matrix3D(Rotation.Conj);
            Vector3D center = Center;

            foreach (Vector3D v in vs) {
                if (!bbox.Inside(v)) {
                    yield return false;
                }

                bool inside = (m * (v - center)).SquareNorm <= 1d;

                yield return inside;
            }
        }

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

        public static Ellipsoid3D Projection(Plane3D plane, Ellipsoid3D g) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            Ellipsoid3D u = q * g + (0d, 0d, plane.D);

            return u;
        }

        public static IEnumerable<Ellipsoid3D> Projection(Plane3D plane, IEnumerable<Ellipsoid3D> gs) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));
            Vector3D v = (0d, 0d, plane.D);

            foreach (Ellipsoid3D g in gs) {
                Ellipsoid3D u = q * g + v;

                yield return u;
            }
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

        // A x^2 + B y^2 + C z^2 + D x y + E x z + F y z + G x + H y + I z + J = 0
        private class EllipsoidImplicitParameter {
            public readonly ddouble A, B, C, D, E, F, G, H, I, J;

            public EllipsoidImplicitParameter(Ellipsoid3D g)
                : this(g.Axis, g.Rotation) {

                (ddouble x0, ddouble y0, ddouble z0) = g.Center;

                this.G = -(D * y0 + E * z0 + 2d * A * x0);
                this.H = -(D * x0 + F * z0 + 2d * B * y0);
                this.I = -(E * x0 + F * y0 + 2d * C * z0);
                this.J += D * x0 * y0 + E * x0 * z0 + F * y0 * z0 + A * x0 * x0 + B * y0 * y0 + C * z0 * z0;
            }

            public EllipsoidImplicitParameter(Vector3D axis, Quaternion rotation) {
                ddouble a2 = axis.X * axis.X, b2 = axis.Y * axis.Y, c2 = axis.Z * axis.Z;

                Matrix3D s = Matrix3D.Scale(b2 * c2, c2 * a2, a2 * b2);
                Matrix3D r = new(rotation);
                Matrix3D ri = new(rotation.Conj);

                Matrix3D m = r * s * ri;

                this.A = m.E00;
                this.B = m.E11;
                this.C = m.E22;
                this.D = 2d * m.E01;
                this.E = 2d * m.E02;
                this.F = 2d * m.E12;
                this.G = 0d;
                this.H = 0d;
                this.I = 0d;
                this.J = -a2 * b2 * c2;
            }

            public static implicit operator
                (ddouble a, ddouble b, ddouble c,
                ddouble d, ddouble e, ddouble f,
                ddouble g, ddouble h, ddouble i, ddouble j)(EllipsoidImplicitParameter param) {

                return (param.A, param.B, param.C, param.D, param.E, param.F, param.G, param.H, param.I, param.J);
            }
        }
    }
}
