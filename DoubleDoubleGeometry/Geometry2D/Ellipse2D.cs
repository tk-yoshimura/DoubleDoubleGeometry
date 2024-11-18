using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Ellipse2D : IGeometry<Ellipse2D, Vector2D>, IFormattable {
        public readonly Vector2D Center;
        public readonly (ddouble major, ddouble minor) Axis;
        public readonly ddouble Angle;

        public Ellipse2D(Vector2D center, (ddouble major, ddouble minor) axis, ddouble angle) {
            this.Center = center;
            this.Axis = axis;
            this.Angle = angle % ddouble.Pi;
        }

        public static Ellipse2D FromImplicit(ddouble a, ddouble b, ddouble c, ddouble d, ddouble e, ddouble f) {
            if (ddouble.Ldexp(a * c, 2) - b * b <= 0d) {
                return Invalid;
            }

            ddouble angle = (b == 0 && a == c) ? 0 : ddouble.Ldexp(ddouble.Atan2Pi(b, a - c), -1);

            ddouble cs = ddouble.CosPi(angle), sn = ddouble.SinPi(angle);
            ddouble sqcs = cs * cs, sqsn = sn * sn, cssn = cs * sn;

            (a, c, d, e) =
                (a * sqcs + b * cssn + c * sqsn, a * sqsn - b * cssn + c * sqcs,
                 e * sn + d * cs, e * cs - d * sn);
            
            if (a > c) {
                angle += 0.5d;
            }

            ddouble x = -d / ddouble.Ldexp(a, 1), y = -e / ddouble.Ldexp(c, 1);

            ddouble gamma = d * d / ddouble.Ldexp(a, 2) + e * e / ddouble.Ldexp(c, 2) - f;

            ddouble major_axis = ddouble.Sqrt(gamma / c);
            ddouble minor_axis = ddouble.Sqrt(gamma / a);

            ddouble cx = x * cs - y * sn;
            ddouble cy = x * sn + y * cs;

            return new Ellipse2D((cx, cy), (major_axis, minor_axis), angle * ddouble.Pi);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Axis.minor * Axis.major * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 4d * Axis.major * ddouble.EllipticE(ddouble.Sqrt(1d - ddouble.Square(Axis.minor / Axis.major)));

        public static Ellipse2D operator +(Ellipse2D g) {
            return g;
        }

        public static Ellipse2D operator -(Ellipse2D g) {
            return new(-g.Center, g.Axis, g.Angle);
        }

        public static Ellipse2D operator +(Ellipse2D g, Vector2D v) {
            return new(g.Center + v, g.Axis, g.Angle);
        }

        public static Ellipse2D operator +(Vector2D v, Ellipse2D g) {
            return new(g.Center + v, g.Axis, g.Angle);
        }

        public static Ellipse2D operator -(Ellipse2D g, Vector2D v) {
            return new(g.Center - v, g.Axis, g.Angle);
        }

        public static Ellipse2D operator -(Vector2D v, Ellipse2D g) {
            return new(v - g.Center, g.Axis, g.Angle);
        }

        public static Ellipse2D operator *(Complex c, Ellipse2D g) {
            ddouble norm = c.Norm;

            return new(g.Center * norm, (g.Axis.major * norm, g.Axis.minor * norm), g.Angle + c.Phase);
        }

        public static Ellipse2D operator *(Ellipse2D g, ddouble r) {
            return new(g.Center * r, (g.Axis.major * r, g.Axis.minor * r), g.Angle);
        }

        public static Ellipse2D operator *(Ellipse2D g, double r) {
            return new(g.Center * r, (g.Axis.major * r, g.Axis.minor * r), g.Angle);
        }

        public static Ellipse2D operator *(ddouble r, Ellipse2D g) {
            return g * r;
        }

        public static Ellipse2D operator *(double r, Ellipse2D g) {
            return g * r;
        }

        public static Ellipse2D operator /(Ellipse2D g, ddouble r) {
            return new(g.Center / r, (g.Axis.major / r, g.Axis.minor / r), g.Angle);
        }

        public static Ellipse2D operator /(Ellipse2D g, double r) {
            return new(g.Center / r, (g.Axis.major / r, g.Axis.minor / r), g.Angle);
        }

        public static bool operator ==(Ellipse2D g1, Ellipse2D g2) {
            return (g1.Center == g2.Center) && (g1.Axis == g2.Axis) && (g1.Angle == g2.Angle);
        }

        public static bool operator !=(Ellipse2D g1, Ellipse2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Ellipse2D((Vector2D center, (ddouble major, ddouble minor) axis, ddouble angle) g) {
            return new(g.center, g.axis, g.angle);
        }

        public static implicit operator (Vector2D center, (ddouble major, ddouble minor) axis, ddouble angle)(Ellipse2D g) {
            return (g.Center, g.Axis, g.Angle);
        }

        public void Deconstruct(out Vector2D center, out (ddouble major, ddouble minor) axis, out ddouble angle)
            => (center, axis, angle) = (Center, Axis, Angle);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse2D Invalid { get; } = new(Vector2D.Invalid, (ddouble.NaN, ddouble.NaN), ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse2D Zero { get; } = new(Vector2D.Zero, (ddouble.Zero, ddouble.Zero), ddouble.Zero);

        public static bool IsNaN(Ellipse2D g) {
            return Vector2D.IsNaN(g.Center) || ddouble.IsNaN(g.Axis.major) || ddouble.IsNaN(g.Axis.minor) || ddouble.IsNaN(g.Angle);
        }

        public static bool IsZero(Ellipse2D g) {
            return Vector2D.IsZero(g.Center) && ddouble.IsZero(g.Axis.major) && ddouble.IsZero(g.Axis.minor) && ddouble.IsZero(g.Angle);
        }

        public static bool IsFinite(Ellipse2D g) {
            return Vector2D.IsFinite(g.Center) && ddouble.IsFinite(g.Axis.major) && ddouble.IsFinite(g.Axis.minor) && ddouble.IsFinite(g.Angle);
        }

        public static bool IsInfinity(Ellipse2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Ellipse2D g) {
            return IsFinite(g) && g.Axis.major >= 0d && g.Axis.minor >= 0d;
        }

        public override string ToString() {
            return $"center={Center}, axis={Axis}, angle={Angle}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, axis=({Axis.major.ToString(format)}, {Axis.minor.ToString(format)}), angle={Angle.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Ellipse2D geo && geo == this;
        }

        public bool Equals(Ellipse2D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Axis.major.GetHashCode() ^ Axis.minor.GetHashCode() ^ Angle.GetHashCode();
        }
    }
}
