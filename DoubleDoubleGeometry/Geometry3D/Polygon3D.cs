﻿using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Polygon3D : IGeometry<Polygon3D, Vector3D> {
        public readonly Polygon2D Polygon;
        public readonly Vector3D Center;
        public readonly Quaternion Rotation;

        private Polygon3D(Polygon2D polygon, Vector3D center, Quaternion rotation, int _) {
            this.Polygon = polygon;
            this.Center = center;
            this.Rotation = rotation;
        }

        public Polygon3D(Polygon2D polygon, Vector3D center, Vector3D normal)
            : this(polygon, center, Vector3D.Rot((0d, 0d, 1d), normal.Normal), 0) { }

        public Polygon3D(Polygon2D polygon, Vector3D center, Quaternion rotation) {
            this.Center = center;
            this.Polygon = polygon;
            this.Rotation = rotation.Normal;
        }

        public Polygon3D(Polygon2D polygon) {
            this.Center = Vector3D.Zero;
            this.Polygon = polygon;
            this.Rotation = Quaternion.One;
        }

        public int Vertices => Polygon.Vertex.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<Vector3D> vertex = null;
        public ReadOnlyCollection<Vector3D> Vertex
            => vertex ??= Polygon.Vertex.Select(v => Center + Rotation * (Vector3D)v).ToArray().AsReadOnly();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D normal = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Normal => normal ??= Rotation * new Vector3D(0d, 0d, 1d);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Plane3D plane = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Plane3D Plane => plane ??= Plane3D.FromNormal(Center, Normal);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area => Polygon.Area;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter => Polygon.Perimeter;

        public static Polygon3D operator +(Polygon3D g) {
            return g;
        }

        public static Polygon3D operator -(Polygon3D g) {
            return new(-g.Polygon, -g.Center, g.Rotation, 0);
        }

        public static Polygon3D operator +(Polygon3D g, Vector3D v) {
            return new(g.Polygon, g.Center + v, g.Rotation, 0);
        }

        public static Polygon3D operator +(Vector3D v, Polygon3D g) {
            return new(g.Polygon, v + g.Center, g.Rotation, 0);
        }

        public static Polygon3D operator -(Polygon3D g, Vector3D v) {
            return new(g.Polygon, g.Center - v, g.Rotation, 0);
        }

        public static Polygon3D operator -(Vector3D v, Polygon3D g) {
            return new(-g.Polygon, v - g.Center, g.Rotation, 0);
        }

        public static Polygon3D operator *(Quaternion q, Polygon3D g) {
            return new(q.SquareNorm * g.Polygon, q * g.Center, q.Normal * g.Rotation, 0);
        }

        public static Polygon3D operator *(Polygon3D g, ddouble r) {
            return new(g.Polygon * r, g.Center * r, g.Rotation, 0);
        }

        public static Polygon3D operator *(Polygon3D g, double r) {
            return new(g.Polygon * r, g.Center * r, g.Rotation, 0);
        }

        public static Polygon3D operator *(ddouble r, Polygon3D g) {
            return g * r;
        }

        public static Polygon3D operator *(double r, Polygon3D g) {
            return g * r;
        }

        public static Polygon3D operator /(Polygon3D g, ddouble r) {
            return new(g.Polygon / r, g.Center / r, g.Rotation, 0);
        }

        public static Polygon3D operator /(Polygon3D g, double r) {
            return new(g.Polygon / r, g.Center / r, g.Rotation, 0);
        }

        public static bool operator ==(Polygon3D g1, Polygon3D g2) {
            return g1.Polygon == g2.Polygon && g1.Center == g2.Center && g1.Rotation == g2.Rotation;
        }

        public static bool operator !=(Polygon3D g1, Polygon3D g2) {
            return !(g1 == g2);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon3D Invalid { get; } = new(Polygon2D.Invalid, Vector3D.Invalid, Quaternion.NaN, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon3D Zero { get; } = new(Polygon2D.Zero, Vector3D.Zero, Quaternion.Zero, 0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox3D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox3D BoundingBox => bbox ??= new BoundingBox3D(Vertex);

        public static bool IsNaN(Polygon3D g) {
            return Polygon2D.IsNaN(g.Polygon) || Vector3D.IsNaN(g.Center) || Quaternion.IsNaN(g.Rotation);
        }

        public static bool IsZero(Polygon3D g) {
            return Polygon2D.IsZero(g.Polygon) || Vector3D.IsZero(g.Center) || Quaternion.IsZero(g.Rotation);
        }

        public static bool IsFinite(Polygon3D g) {
            return Polygon2D.IsFinite(g.Polygon) && Vector3D.IsFinite(g.Center) && Quaternion.IsFinite(g.Rotation);
        }

        public static bool IsInfinity(Polygon3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Polygon3D g) {
            return Vector3D.IsFinite(g.Center) && Quaternion.IsFinite(g.Rotation) && Polygon2D.IsValid(g.Polygon);
        }

        public static bool IsConvex(Polygon3D g) {
            return Polygon2D.IsConvex(g.Polygon);
        }

        public static bool IsConcave(Polygon3D g) {
            return Polygon2D.IsConcave(g.Polygon);
        }

        public static Polygon3D Projection(Plane3D plane, Polygon3D g) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));

            Polygon3D u = q * g + (0d, 0d, plane.D);

            return u;
        }

        public static IEnumerable<Polygon3D> Projection(Plane3D plane, IEnumerable<Polygon3D> gs) {
            Quaternion q = Vector3D.Rot(plane.Normal, (0d, 0d, 1d));
            Vector3D v = (0d, 0d, plane.D);

            foreach (Polygon3D g in gs) {
                Polygon3D u = q * g + v;

                yield return u;
            }
        }

        public override string ToString() {
            return $"polygon vertices={Vertices}";
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Polygon3D g && g == this);
        }

        public bool Equals(Polygon3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Vertices > 0 ? Vertex[0].GetHashCode() : 0;
        }
    }
}
