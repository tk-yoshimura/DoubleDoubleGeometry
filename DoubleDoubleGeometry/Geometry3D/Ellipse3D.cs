using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using System;
using System.Collections.Generic;
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
        private Vector3D normal = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal => normal ??= Rotation * new Vector3D(0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Plane3D plane = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Plane3D Plane => plane ??= Plane3D.FromNormal(Center, Normal);

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox3D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox3D BoundingBox {
            get {
                if (bbox is not null) {
                    return bbox;
                }

                EllipseImplicitParameter param = new(Axis, Rotation);

                (ddouble X, ddouble Y, ddouble Z) = Plane.Normal;

                (ddouble A, ddouble B, ddouble C, ddouble D, ddouble E, ddouble F, ddouble G)
                    = (param.A, param.B, param.C, param.D, param.E, param.F, param.G);

                int max_index = Vector3D.MaxAbsIndex(Plane.Normal);

                ddouble x2 = X * X, y2 = Y * Y, z2 = Z * Z;

                ddouble x = ddouble.NaN, y = ddouble.NaN, z = ddouble.NaN, emax = 0d;

                {
                    ddouble a = B * x2 + A * y2 - D * Y * X;
                    ddouble b = F * x2 - (D * Z + E * Y) * X + 2d * A * Y * Z;
                    ddouble c = C * x2 + A * z2 - E * Z * X;
                    ddouble f = G * x2;

                    ddouble e = b * b - 4d * a * c;
                    ddouble m = f / e;

                    ddouble yt = 2d * ddouble.Sqrt(c * m);
                    ddouble zt = 2d * ddouble.Sqrt(a * m);

                    if (ddouble.Abs(e) > emax) {
                        emax = ddouble.Abs(e);
                        y = yt;
                        z = zt;
                    }
                }
                {
                    ddouble a = C * y2 + B * z2 - F * Z * Y;
                    ddouble b = E * y2 - (D * Z + F * X) * Y + 2d * B * Z * X;
                    ddouble c = A * y2 + B * x2 - D * X * Y;
                    ddouble f = G * y2;

                    ddouble e = b * b - 4d * a * c;
                    ddouble m = f / e;

                    ddouble zt = 2d * ddouble.Sqrt(c * m);
                    ddouble xt = 2d * ddouble.Sqrt(a * m);

                    if (ddouble.Abs(e) > emax) {
                        emax = ddouble.Abs(e);
                        z = zt;
                        x = xt;
                    }
                    else if (ddouble.IsNaN(z) && ddouble.IsFinite(zt)) {
                        z = zt;
                    }
                }
                {
                    ddouble a = A * z2 + C * x2 - E * X * Z;
                    ddouble b = D * z2 - (F * X + E * Y) * Z + 2d * C * X * Y;
                    ddouble c = B * z2 + C * y2 - F * Y * Z;
                    ddouble f = G * z2;

                    ddouble e = b * b - 4d * a * c;
                    ddouble m = f / e;

                    ddouble xt = 2d * ddouble.Sqrt(c * m);
                    ddouble yt = 2d * ddouble.Sqrt(a * m);

                    if (ddouble.Abs(e) > emax) {
                        emax = ddouble.Abs(e);
                        x = xt;
                        y = yt;
                    }
                    else {
                        if (ddouble.IsNaN(x) && ddouble.IsFinite(xt)) {
                            x = xt;
                        }
                        if (ddouble.IsNaN(y) && ddouble.IsFinite(yt)) {
                            y = yt;
                        }
                    }
                }

                if (emax > 0d) {
                    x = ddouble.IsFinite(x) ? x : 0d;
                    y = ddouble.IsFinite(y) ? y : 0d;
                    z = ddouble.IsFinite(z) ? z : 0d;
                }

                return bbox ??= new BoundingBox3D(Center, (x, y, z));
            }
        }

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

        public static Ellipse3D Projection(Plane3D plane, Ellipse3D g) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            Ellipse3D u = q * g + (0d, 0d, plane.D);

            return u;
        }

        public static IEnumerable<Ellipse3D> Projection(Plane3D plane, IEnumerable<Ellipse3D> gs) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));
            Vector3D v = (0d, 0d, plane.D);

            foreach (Ellipse3D g in gs) {
                Ellipse3D u = q * g + v;

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

        // A x^2 + B y^2 + C z^2 + D x y + E x z + F y z + G = 0
        public class EllipseImplicitParameter {
            public readonly ddouble A, B, C, D, E, F, G;

            public EllipseImplicitParameter(Vector2D axis, Quaternion rotation) {
                ddouble a2 = axis.X * axis.X, b2 = axis.Y * axis.Y;

                Matrix3D s = Matrix3D.Scale(b2, a2, 0d);
                Matrix3D r = new(rotation);
                Matrix3D ri = new(rotation.Conj);

                Matrix3D m = r * s * ri;

                this.A = m.E00;
                this.B = m.E11;
                this.C = m.E22;
                this.D = 2d * m.E01;
                this.E = 2d * m.E02;
                this.F = 2d * m.E12;
                this.G = -a2 * b2;
            }

            public static implicit operator
                (ddouble a, ddouble b, ddouble c,
                ddouble d, ddouble e, ddouble f, ddouble g)(EllipseImplicitParameter param) {

                return (param.A, param.B, param.C, param.D, param.E, param.F, param.G);
            }
        }
    }
}
