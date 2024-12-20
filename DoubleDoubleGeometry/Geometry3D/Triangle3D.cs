﻿using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Triangle3D : IGeometry<Triangle3D, Vector3D>, IFormattable {
        public readonly Vector3D V0, V1, V2;

        public Triangle3D(Vector3D v0, Vector3D v1, Vector3D v2) {
            this.V0 = v0;
            this.V1 = v1;
            this.V2 = v2;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Polygon3D polygon = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Polygon3D Polygon {
            get {
                if (polygon is not null) {
                    return polygon;
                }

                Vector3D center = Center;

                Vector3D v0 = V0 - center, v1 = V1 - center, v2 = V2 - center;

                Vector2D[] us = Vector3D.Projection(Plane, [v0, v1, v2]).Select(v => (Vector2D)v).ToArray();

                return polygon ??= new Polygon3D(new Polygon2D(us), Center, Normal);
            }
        }

        public Vector3D Point(ddouble u, ddouble v) {
            return V0 + u * (V1 - V0) + (1d - u) * v * (V2 - V0);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area {
            get {
                Vector3D a = V1 - V0, b = V2 - V0;
                ddouble inner_product_ab = Vector3D.Dot(a, b);

                return ddouble.Ldexp(ddouble.Sqrt(a.SquareNorm * b.SquareNorm - inner_product_ab * inner_product_ab), -1);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal {
            get {
                return Vector3D.Cross(V1 - V0, V2 - V0).Normal;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Center {
            get {
                return (V0 + V1 + V2) / 3d;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Plane3D plane = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Plane3D Plane => plane ??= Plane3D.FromNormal(V0, Normal);

        public static Triangle3D operator +(Triangle3D g) {
            return g;
        }

        public static Triangle3D operator -(Triangle3D g) {
            return new(-g.V0, -g.V1, -g.V2);
        }

        public static Triangle3D operator +(Triangle3D g, Vector3D v) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v);
        }

        public static Triangle3D operator +(Vector3D v, Triangle3D g) {
            return new(g.V0 + v, g.V1 + v, g.V2 + v);
        }

        public static Triangle3D operator -(Triangle3D g, Vector3D v) {
            return new(g.V0 - v, g.V1 - v, g.V2 - v);
        }

        public static Triangle3D operator -(Vector3D v, Triangle3D g) {
            return new(v - g.V0, v - g.V1, v - g.V2);
        }

        public static Triangle3D operator *(Triangle3D g, ddouble r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r);
        }

        public static Triangle3D operator *(Triangle3D g, double r) {
            return new(g.V0 * r, g.V1 * r, g.V2 * r);
        }

        public static Triangle3D operator *(ddouble r, Triangle3D g) {
            return g * r;
        }

        public static Triangle3D operator *(double r, Triangle3D g) {
            return g * r;
        }

        public static Triangle3D operator /(Triangle3D g, ddouble r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r);
        }

        public static Triangle3D operator /(Triangle3D g, double r) {
            return new(g.V0 / r, g.V1 / r, g.V2 / r);
        }

        public static Triangle3D operator *(Matrix3D m, Triangle3D g) {
            return new Triangle3D(m * g.V0, m * g.V1, m * g.V2);
        }

        public static Triangle3D operator *(Quaternion q, Triangle3D g) {
            return new Triangle3D(q * g.V0, q * g.V1, q * g.V2);
        }

        public static bool operator ==(Triangle3D g1, Triangle3D g2) {
            return (g1.V0 == g2.V0) && (g1.V1 == g2.V1) && (g1.V2 == g2.V2);
        }

        public static bool operator !=(Triangle3D g1, Triangle3D g2) {
            return !(g1 == g2);
        }

        public static implicit operator Triangle3D((Vector3D v0, Vector3D v1, Vector3D v2) g) {
            return new(g.v0, g.v1, g.v2);
        }

        public static implicit operator (Vector3D v0, Vector3D v1, Vector3D v2)(Triangle3D g) {
            return (g.V0, g.V1, g.V2);
        }

        public static implicit operator Polygon3D(Triangle3D g) {
            return g.Polygon;
        }

        public void Deconstruct(out Vector3D v0, out Vector3D v1, out Vector3D v2)
            => (v0, v1, v2) = (V0, V1, V2);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Triangle3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid, Vector3D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Triangle3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero, Vector3D.Zero);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox3D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox3D BoundingBox => bbox ??= new BoundingBox3D(V0, V1, V2);

        public static bool IsNaN(Triangle3D g) {
            return Vector3D.IsNaN(g.V0) || Vector3D.IsNaN(g.V1) || Vector3D.IsNaN(g.V2);
        }

        public static bool IsZero(Triangle3D g) {
            return Vector3D.IsZero(g.V0) && Vector3D.IsZero(g.V1) && Vector3D.IsZero(g.V2);
        }

        public static bool IsFinite(Triangle3D g) {
            return Vector3D.IsFinite(g.V0) && Vector3D.IsFinite(g.V1) && Vector3D.IsFinite(g.V2);
        }

        public static bool IsInfinity(Triangle3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Triangle3D g) {
            return IsFinite(g) && g.V0 != g.V1 && g.V1 != g.V2 && g.V2 != g.V0;
        }

        public static Triangle3D Projection(Plane3D plane, Triangle3D g) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            Triangle3D u = q * g;

            return new Triangle3D(
                (u.V0.X, u.V0.Y, u.V0.Z + plane.D),
                (u.V1.X, u.V1.Y, u.V1.Z + plane.D),
                (u.V2.X, u.V2.Y, u.V2.Z + plane.D)
            );
        }

        public static IEnumerable<Triangle3D> Projection(Plane3D plane, IEnumerable<Triangle3D> gs) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            foreach (Triangle3D g in gs) {
                Triangle3D u = q * g;

                yield return new Triangle3D(
                    (u.V0.X, u.V0.Y, u.V0.Z + plane.D),
                    (u.V1.X, u.V1.Y, u.V1.Z + plane.D),
                    (u.V2.X, u.V2.Y, u.V2.Z + plane.D)
                );
            }
        }

        public override string ToString() {
            return $"{V0}, {V1}, {V2}";
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            return $"{V0.ToString(format)}, {V1.ToString(format)}, {V2.ToString(format)}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Triangle3D g && g == this);
        }

        public bool Equals(Triangle3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return V0.GetHashCode() ^ V1.GetHashCode() ^ V2.GetHashCode();
        }
    }
}
