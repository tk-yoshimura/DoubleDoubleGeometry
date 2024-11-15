﻿using DoubleDouble;
using System.Diagnostics;

namespace DoubleDoubleGeometry.Geometry3D {

    public class Line3D : IGeometry<Line3D, Vector3D> {
        public readonly Vector3D Origin, Direction;

        private Line3D(Vector3D origin, Vector3D direction) {
            this.Origin = origin;
            this.Direction = direction;
        }

        public static Line3D FromDirection(Vector3D origin, Vector3D direction) {
            return new Line3D(origin, direction.Normal);
        }

        public static Line3D FromIntersection(Vector3D v0, Vector3D v1) {
            return new Line3D(v0, (v1 - v0).Normal);
        }

        public static Line3D operator +(Line3D g) {
            return g;
        }

        public static Line3D operator -(Line3D g) {
            return new(-g.Origin, -g.Direction);
        }

        public static Line3D operator +(Line3D g, Vector3D v) {
            return new(g.Origin + v, g.Direction);
        }

        public static Line3D operator +(Vector3D v, Line3D g) {
            return new(g.Origin + v, g.Direction);
        }

        public static Line3D operator -(Line3D g, Vector3D v) {
            return new(g.Origin - v, g.Direction);
        }

        public static Line3D operator -(Vector3D v, Line3D g) {
            return new(v - g.Origin, -g.Direction);
        }

        public static Line3D operator *(Line3D g, ddouble r) {
            return new(g.Origin * r, g.Direction * ddouble.Sign(r));
        }

        public static Line3D operator *(Line3D g, double r) {
            return new(g.Origin * r, g.Direction * double.Sign(r));
        }

        public static Line3D operator *(ddouble r, Line3D g) {
            return g * r;
        }

        public static Line3D operator *(double r, Line3D g) {
            return g * r;
        }

        public static Line3D operator /(Line3D g, ddouble r) {
            return new(g.Origin / r, g.Direction * ddouble.Sign(r));
        }

        public static Line3D operator /(Line3D g, double r) {
            return new(g.Origin / r, g.Direction * double.Sign(r));
        }

        public static Line3D operator *(Matrix3D matrix, Line3D line) {
            Vector3D v0 = matrix * line.Origin, v1 = matrix * (line.Origin + line.Direction);

            return FromIntersection(v0, v1);
        }

        public static Line3D operator *(HomogeneousMatrix3D matrix, Line3D line) {
            Vector3D v0 = matrix * line.Origin, v1 = matrix * (line.Origin + line.Direction);

            return FromIntersection(v0, v1);
        }

        public static bool operator ==(Line3D t1, Line3D t2) {
            return (t1.Origin == t2.Origin) && (t1.Direction == t2.Direction);
        }

        public static bool operator !=(Line3D t1, Line3D t2) {
            return !(t1 == t2);
        }

        public static implicit operator Line3D((Vector3D v0, Vector3D v1) g) {
            return FromIntersection(g.v0, g.v1);
        }

        public static implicit operator (Vector3D origin, Vector3D direction)(Line3D g) {
            return (g.Origin, g.Direction);
        }

        public void Deconstruct(out Vector3D origin, out Vector3D direction)
            => (origin, direction) = (Origin, Direction);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Line3D Invalid { get; } = new(Vector3D.Invalid, Vector3D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Line3D Zero { get; } = new(Vector3D.Zero, Vector3D.Zero);

        public static bool IsNaN(Line3D g) {
            return Vector3D.IsNaN(g.Origin) || Vector3D.IsNaN(g.Direction);
        }

        public static bool IsZero(Line3D g) {
            return Vector3D.IsZero(g.Origin) && Vector3D.IsZero(g.Direction);
        }

        public static bool IsFinite(Line3D g) {
            return Vector3D.IsFinite(g.Origin) && Vector3D.IsFinite(g.Direction);
        }

        public static bool IsInfinity(Line3D g) {
            return Vector3D.IsInfinity(g.Origin) || Vector3D.IsInfinity(g.Direction);
        }

        public static bool IsValid(Line3D g) {
            return IsFinite(g) && Vector3D.IsFinite(g.Direction) && !Vector3D.IsZero(g.Direction);
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Line3D geo && geo == this;
        }

        public bool Equals(Line3D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Origin.GetHashCode() ^ Direction.GetHashCode();
        }
    }
}
