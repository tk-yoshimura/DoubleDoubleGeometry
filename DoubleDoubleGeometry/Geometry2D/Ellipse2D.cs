﻿using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Ellipse2D : IGeometry<Ellipse2D, Vector2D>, IFormattable {
        public readonly Vector2D Center;
        public readonly Vector2D Axis;
        public readonly Complex Rotation;

        private Ellipse2D(Vector2D center, Vector2D axis, Complex rotation, int _) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation;
        }

        public Ellipse2D(Vector2D center, Vector2D axis, ddouble angle)
            : this(center, axis, Complex.FromPhase(angle), 0) { }

        public Ellipse2D(Vector2D center, Vector2D axis, Complex rotation) {
            this.Center = center;
            this.Axis = axis;
            this.Rotation = rotation.Normal;
        }

        public Vector2D Point(ddouble t) {
            return Center + Rotation * new Vector2D(ddouble.Cos(t) * Axis.X, ddouble.Sin(t) * Axis.Y);
        }

        public static Ellipse2D FromImplicit(ddouble a, ddouble b, ddouble c, ddouble d, ddouble e, ddouble f) {
            if (ddouble.Ldexp(a * c, 2) - b * b <= 0d) {
                return Invalid;
            }

            ddouble rotation = (b == 0 && a == c) ? 0 : ddouble.Ldexp(ddouble.Atan2Pi(b, a - c), -1);

            ddouble cs = ddouble.CosPi(rotation), sn = ddouble.SinPi(rotation);
            ddouble sqcs = cs * cs, sqsn = sn * sn, cssn = cs * sn;

            (a, c, d, e) =
                (a * sqcs + b * cssn + c * sqsn, a * sqsn - b * cssn + c * sqcs,
                 e * sn + d * cs, e * cs - d * sn);

            if (a > c) {
                rotation += 0.5d;
            }

            ddouble x = -d / ddouble.Ldexp(a, 1), y = -e / ddouble.Ldexp(c, 1);

            ddouble gamma = d * d / ddouble.Ldexp(a, 2) + e * e / ddouble.Ldexp(c, 2) - f;

            ddouble major_axis = ddouble.Sqrt(gamma / c);
            ddouble minor_axis = ddouble.Sqrt(gamma / a);

            ddouble cx = x * cs - y * sn;
            ddouble cy = x * sn + y * cs;

            return new Ellipse2D((cx, cy), (major_axis, minor_axis), Complex.FromPhasePi(rotation), 0);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble MajorAxis => ddouble.Max(ddouble.Abs(Axis.X), ddouble.Abs(Axis.Y));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble MinorAxis => ddouble.Min(ddouble.Abs(Axis.X), ddouble.Abs(Axis.Y));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Angle => Rotation.Phase;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => ddouble.Abs(Axis.X) * ddouble.Abs(Axis.Y) * ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => 4d * MajorAxis * ddouble.EllipticE(1d - ddouble.Square(MinorAxis / MajorAxis));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Focus => ddouble.Sqrt(ddouble.Square(MajorAxis) - ddouble.Square(MinorAxis));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Eccentricity => ddouble.Sqrt(1d - ddouble.Square(MinorAxis / MajorAxis));

        public static Ellipse2D operator +(Ellipse2D g) {
            return g;
        }

        public static Ellipse2D operator -(Ellipse2D g) {
            return new(-g.Center, -g.Axis, g.Rotation, 0);
        }

        public static Ellipse2D operator +(Ellipse2D g, Vector2D v) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse2D operator +(Vector2D v, Ellipse2D g) {
            return new(g.Center + v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse2D operator -(Ellipse2D g, Vector2D v) {
            return new(g.Center - v, g.Axis, g.Rotation, 0);
        }

        public static Ellipse2D operator -(Vector2D v, Ellipse2D g) {
            return new(v - g.Center, -g.Axis, g.Rotation, 0);
        }

        public static Ellipse2D operator *(Complex c, Ellipse2D g) {
            ddouble norm = c.Norm;

            return new(c * g.Center, norm * g.Axis, (c / norm) * g.Rotation, 0);
        }

        public static Ellipse2D operator *(Ellipse2D g, ddouble r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipse2D operator *(Ellipse2D g, double r) {
            return new(g.Center * r, g.Axis * r, g.Rotation, 0);
        }

        public static Ellipse2D operator *(ddouble r, Ellipse2D g) {
            return g * r;
        }

        public static Ellipse2D operator *(double r, Ellipse2D g) {
            return g * r;
        }

        public static Ellipse2D operator /(Ellipse2D g, ddouble r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
        }

        public static Ellipse2D operator /(Ellipse2D g, double r) {
            return new(g.Center / r, g.Axis / r, g.Rotation, 0);
        }

        public static bool operator ==(Ellipse2D g1, Ellipse2D g2) {
            return (g1.Center == g2.Center) && (g1.Axis == g2.Axis) && (g1.Rotation == g2.Rotation);
        }

        public static bool operator !=(Ellipse2D g1, Ellipse2D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Ellipse2D((Vector2D center, Vector2D axis, Complex rotation) g) {
            return new(g.center, g.axis, g.rotation);
        }

        public static implicit operator (Vector2D center, Vector2D axis, Complex rotation)(Ellipse2D g) {
            return (g.Center, g.Axis, g.Rotation);
        }

        public void Deconstruct(out Vector2D center, out Vector2D axis, out Complex rotation)
            => (center, axis, rotation) = (Center, Axis, Rotation);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse2D Invalid { get; } = new(Vector2D.Invalid, (ddouble.NaN, ddouble.NaN), ddouble.NaN);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Ellipse2D Zero { get; } = new(Vector2D.Zero, (ddouble.Zero, ddouble.Zero), ddouble.Zero);

        public static bool IsNaN(Ellipse2D g) {
            return Vector2D.IsNaN(g.Center) || Vector2D.IsNaN(g.Axis) || Complex.IsNaN(g.Rotation);
        }

        public static bool IsZero(Ellipse2D g) {
            return Vector2D.IsZero(g.Center) && Vector2D.IsZero(g.Axis) && Complex.IsFinite(g.Rotation);
        }

        public static bool IsFinite(Ellipse2D g) {
            return Vector2D.IsFinite(g.Center) && Vector2D.IsFinite(g.Axis) && Complex.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Ellipse2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Ellipse2D g) {
            return IsFinite(g);
        }

        public override string ToString() {
            return $"center={Center}, axis={Axis}, rotation={Rotation}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"center={Center.ToString(format)}, axis=({Axis.ToString(format)}), rotation={Rotation.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Ellipse2D g && g == this);
        }

        public bool Equals(Ellipse2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Axis.GetHashCode() ^ Rotation.GetHashCode();
        }
    }
}
