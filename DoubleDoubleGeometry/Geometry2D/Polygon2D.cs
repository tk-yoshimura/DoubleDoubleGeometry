using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DoubleDoubleGeometry.Geometry2D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Polygon2D : IGeometry<Polygon2D, Vector2D> {
        public readonly ReadOnlyCollection<Vector2D> Vertex;

        public Polygon2D(params Vector2D[] vertex) {
            this.Vertex = vertex.AsReadOnly();
        }

        public Polygon2D(IEnumerable<Vector2D> vertex) {
            this.Vertex = vertex.ToArray().AsReadOnly();
        }

        public int Count => Vertex.Count;

        public static Polygon2D operator +(Polygon2D g) {
            return g;
        }

        public static Polygon2D operator -(Polygon2D g) {
            return new(g.Vertex.Select(p => -p));
        }

        public static Polygon2D operator +(Polygon2D g, Vector2D v) {
            return new(g.Vertex.Select(p => p + v));
        }

        public static Polygon2D operator +(Vector2D v, Polygon2D g) {
            return new(g.Vertex.Select(p => v + p));
        }

        public static Polygon2D operator -(Polygon2D g, Vector2D v) {
            return new(g.Vertex.Select(p => p - v));
        }

        public static Polygon2D operator -(Vector2D v, Polygon2D g) {
            return new(g.Vertex.Select(p => v - p));
        }

        public static Polygon2D operator *(Complex c, Polygon2D g) {
            return new(g.Vertex.Select(p => c * p));
        }

        public static Polygon2D operator *(Polygon2D g, ddouble r) {
            return new(g.Vertex.Select(p => p * r));
        }

        public static Polygon2D operator *(Polygon2D g, double r) {
            return new(g.Vertex.Select(p => p * r));
        }

        public static Polygon2D operator *(ddouble r, Polygon2D g) {
            return g * r;
        }

        public static Polygon2D operator *(double r, Polygon2D g) {
            return g * r;
        }

        public static Polygon2D operator /(Polygon2D g, ddouble r) {
            return new(g.Vertex.Select(p => p / r));
        }

        public static Polygon2D operator /(Polygon2D g, double r) {
            return new(g.Vertex.Select(p => p / r));
        }

        public static bool operator ==(Polygon2D g1, Polygon2D g2) {
            return g1.Vertex.Zip(g2.Vertex).All(item => item.First == item.Second);
        }

        public static bool operator !=(Polygon2D g1, Polygon2D g2) {
            return !(g1 == g2);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon2D Invalid { get; } = new(Vector2D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon2D Zero { get; } = new(Vector2D.Zero);

        public static bool IsNaN(Polygon2D g) {
            return g.Count < 1 || g.Vertex.Any(Vector2D.IsNaN);
        }

        public static bool IsZero(Polygon2D g) {
            return g.Count < 1 || g.Vertex.All(Vector2D.IsZero);
        }

        public static bool IsFinite(Polygon2D g) {
            return g.Vertex.All(Vector2D.IsFinite);
        }

        public static bool IsInfinity(Polygon2D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Polygon2D g) {
            return g.Count > 0 || IsFinite(g);
        }

        public override string ToString() {
            return $"polygon {Count}";
        }

        public override bool Equals(object obj) {
            return (obj is not null) && obj is Polygon2D geo && geo == this;
        }

        public bool Equals(Polygon2D other) {
            return other == this;
        }

        public override int GetHashCode() {
            return Count > 0 ? Vertex[0].GetHashCode() : 0;
        }

        public static Polygon2D Regular(int n) {
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 3, nameof(n));

            Vector2D[] vs = new Vector2D[n];

            for (int i = 0; i < vs.Length; i++) {
                ddouble theta = 2 * (ddouble)i / n;
                vs[i] = (ddouble.CosPi(theta), ddouble.SinPi(theta));
            }

            return new(vs);
        }
    }
}
