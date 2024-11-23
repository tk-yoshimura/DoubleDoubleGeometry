using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Segment3D : IGeometry<Segment3D, Vector3D>, IFormattable {
        public readonly Vector3D V0, V1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Vector3D dv;

        public Segment3D(Vector3D v0, Vector3D v1) {
            this.V0 = v0;
            this.V1 = v1;
            this.dv = v1 - v0;
        }

        public Vector3D Point(ddouble t) {
            return V0 + t * dv;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Length => Vector3D.Distance(V0, V1);

        public static Segment3D operator +(Segment3D g) {
            return g;
        }

        public static Segment3D operator -(Segment3D g) {
            return new(-g.V0, -g.V1);
        }

        public static Segment3D operator +(Segment3D g, Vector3D v) {
            return new(g.V0 + v, g.V1 + v);
        }

        public static Segment3D operator +(Vector3D v, Segment3D g) {
            return new(g.V0 + v, g.V1 + v);
        }

        public static Segment3D operator -(Segment3D g, Vector3D v) {
            return new(g.V0 - v, g.V1 - v);
        }

        public static Segment3D operator -(Vector3D v, Segment3D g) {
            return new(v - g.V0, v - g.V1);
        }

        public static Segment3D operator *(Segment3D g, ddouble r) {
            return new(g.V0 * r, g.V1 * r);
        }

        public static Segment3D operator *(Segment3D g, double r) {
            return new(g.V0 * r, g.V1 * r);
        }

        public static Segment3D operator *(ddouble r, Segment3D g) {
            return g * r;
        }

        public static Segment3D operator *(double r, Segment3D g) {
            return g * r;
        }

        public static Segment3D operator /(Segment3D g, ddouble r) {
            return new(g.V0 / r, g.V1 / r);
        }

        public static Segment3D operator /(Segment3D g, double r) {
            return new(g.V0 / r, g.V1 / r);
        }

        public static Segment3D operator *(Matrix3D m, Segment3D g) {
            return new Segment3D(m * g.V0, m * g.V1);
        }

        public static Segment3D operator *(Quaternion q, Segment3D g) {
            return new Segment3D(q * g.V0, q * g.V1);
        }

        public static bool operator ==(Segment3D g1, Segment3D g2) {
            return (g1.V0 == g2.V0) && (g1.V1 == g2.V1);
        }

        public static bool operator !=(Segment3D g1, Segment3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Segment3D((Vector3D v0, Vector3D v1) g) {
            return new(g.v0, g.v1);
        }

        public static implicit operator (Vector3D v0, Vector3D v1)(Segment3D g) {
            return (g.V0, g.V1);
        }

        public void Deconstruct(out Vector3D v0, out Vector3D v1)
            => (v0, v1) = (V0, V1);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Segment3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Segment3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero);

        public static bool IsNaN(Segment3D g) {
            return Vector3D.IsNaN(g.V0) || Vector3D.IsNaN(g.V1);
        }

        public static bool IsZero(Segment3D g) {
            return Vector3D.IsZero(g.V0) && Vector3D.IsZero(g.V1);
        }

        public static bool IsFinite(Segment3D g) {
            return Vector3D.IsFinite(g.V0) && Vector3D.IsFinite(g.V1);
        }

        public static bool IsInfinity(Segment3D g) {
            return Vector3D.IsInfinity(g.V0) || Vector3D.IsInfinity(g.V1);
        }

        public static bool IsValid(Segment3D g) {
            return IsFinite(g) && g.V0 != g.V1;
        }

        public override string ToString() {
            return $"{V0}, {V1}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"{V0.ToString(format)}, {V1.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Segment3D g && g == this);
        }

        public bool Equals(Segment3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return V0.GetHashCode() ^ V1.GetHashCode();
        }
    }
}
