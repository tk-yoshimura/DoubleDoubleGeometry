using DoubleDouble;
using DoubleDoubleComplex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DoubleDoubleGeometry.Geometry3D {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Polyhedron3D : IGeometry<Polyhedron3D, Vector3D> {
        public readonly Connection Connection;
        public readonly ReadOnlyCollection<Vector3D> Vertex;

        public Polyhedron3D(Connection connection, params Vector3D[] vertex) {
            if (connection.Vertices != vertex.Length) {
                throw new ArgumentException("mismatch vertices", nameof(connection));
            }

            this.Connection = connection;
            this.Vertex = vertex.AsReadOnly();
        }

        public Polyhedron3D(Connection connection, IEnumerable<Vector3D> vertex) : this(connection, vertex.ToArray()) { }

        public int Vertices => Vertex.Count;
        public long Edges => Connection.Edges;

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D? center = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Center => center ??= (Vertex.Max() + Vertex.Min()) / 2d;

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D? size = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector3D Size => size ??= Vertex.Max() - Vertex.Min();

        public static Polyhedron3D operator +(Polyhedron3D g) {
            return g;
        }

        public static Polyhedron3D operator -(Polyhedron3D g) {
            return new(g.Connection, g.Vertex.Select(p => -p));
        }

        public static Polyhedron3D operator +(Polyhedron3D g, Vector3D v) {
            return new(g.Connection, g.Vertex.Select(p => p + v));
        }

        public static Polyhedron3D operator +(Vector3D v, Polyhedron3D g) {
            return new(g.Connection, g.Vertex.Select(p => v + p));
        }

        public static Polyhedron3D operator -(Polyhedron3D g, Vector3D v) {
            return new(g.Connection, g.Vertex.Select(p => p - v));
        }

        public static Polyhedron3D operator -(Vector3D v, Polyhedron3D g) {
            return new(g.Connection, g.Vertex.Select(p => v - p));
        }

        public static Polyhedron3D operator *(Matrix3D m, Polyhedron3D g) {
            return new(g.Connection, g.Vertex.Select(p => m * p));
        }

        public static Polyhedron3D operator *(Quaternion q, Polyhedron3D g) {
            return new(g.Connection, g.Vertex.Select(p => q * p));
        }

        public static Polyhedron3D operator *(Polyhedron3D g, ddouble r) {
            return new(g.Connection, g.Vertex.Select(p => p * r));
        }

        public static Polyhedron3D operator *(Polyhedron3D g, double r) {
            return new(g.Connection, g.Vertex.Select(p => p * r));
        }

        public static Polyhedron3D operator *(ddouble r, Polyhedron3D g) {
            return g * r;
        }

        public static Polyhedron3D operator *(double r, Polyhedron3D g) {
            return g * r;
        }

        public static Polyhedron3D operator /(Polyhedron3D g, ddouble r) {
            return new(g.Connection, g.Vertex.Select(p => p / r));
        }

        public static Polyhedron3D operator /(Polyhedron3D g, double r) {
            return new(g.Connection, g.Vertex.Select(p => p / r));
        }

        public static bool operator ==(Polyhedron3D g1, Polyhedron3D g2) {
            return g1.Vertex.SequenceEqual(g2.Vertex) && g1.Connection == g2.Connection;
        }

        public static bool operator !=(Polyhedron3D g1, Polyhedron3D g2) {
            return !(g1 == g2);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Invalid { get; } = new(new Connection(1), Vector3D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Zero { get; } = new(new Connection(1), Vector3D.Zero);

        public static bool IsNaN(Polyhedron3D g) {
            return g.Vertices < 1 || g.Vertex.Any(Vector3D.IsNaN);
        }

        public static bool IsZero(Polyhedron3D g) {
            return g.Vertices < 1 || g.Vertex.All(Vector3D.IsZero);
        }

        public static bool IsFinite(Polyhedron3D g) {
            return g.Vertex.All(Vector3D.IsFinite);
        }

        public static bool IsInfinity(Polyhedron3D g) {
            return !IsFinite(g);
        }

        public static bool IsValid(Polyhedron3D g) {
            return g.Vertices > 0 || IsFinite(g);
        }

        public override string ToString() {
            return $"polyhedron vertices={Vertices}, edges={Edges}";
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Polyhedron3D g && g == this);
        }

        public bool Equals(Polyhedron3D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Vertices > 0 ? Vertex[0].GetHashCode() : 0;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Tetrahedron => new(
            new Connection(4,
                (0, 1), (0, 2), (0, 3), (1, 2), (1, 3), (2, 3)
            ),
            (-1, -1, -1), (-1, 1, 1), (1, -1, 1), (1, 1, -1)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Cube => new(
            new Connection(8,
                (0, 1), (0, 2), (0, 4), (1, 3), (1, 5), (2, 3), (2, 6), (3, 7),
                (4, 5), (4, 6), (5, 7), (6, 7)
            ),
            (-1, -1, -1), (1, -1, -1), (-1, 1, -1), (1, 1, -1),
            (-1, -1, 1), (1, -1, 1), (-1, 1, 1), (1, 1, 1)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Octahedron => new(
            new Connection(6,
                (0, 1), (0, 2), (0, 3), (0, 4), (1, 2), (1, 4), (1, 5), (2, 3),
                (2, 5), (3, 4), (3, 5), (4, 5)
            ),
            (0, 0, -1), (1, 0, 0), (0, 1, 0), (-1, 0, 0),
            (0, -1, 0), (0, 0, 1)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Dodecahedron {
            get {
                ddouble p1 = (ddouble.Sqrt(5) - 1d) * 0.5d;
                ddouble p2 = (3d - ddouble.Sqrt(5)) * 0.5d;

                return new(
                    new Connection(20,
                        (0, 1), (0, 2), (0, 3), (1, 4), (1, 6), (2, 8), (2, 9), (3, 5),
                        (3, 7), (4, 8), (4, 10), (5, 9), (5, 11), (6, 7), (6, 12), (7, 13),
                        (8, 15), (9, 16), (10, 12), (10, 17), (11, 13), (11, 18), (12, 14), (13, 14),
                        (14, 19), (15, 16), (15, 17), (16, 18), (17, 19), (18, 19)
                    ),
                    (0, -p2, -1), (0, p2, -1), (-p1, -p1, -p1), (p1, -p1, -p1),
                    (-p1, p1, -p1), (p2, -1, 0), (p1, p1, -p1), (1, 0, -p2),
                    (-1, 0, -p2), (-p2, -1, 0), (-p2, 1, 0), (p1, -p1, p1),
                    (p2, 1, 0), (1, 0, p2), (p1, p1, p1), (-1, 0, p2),
                    (-p1, -p1, p1), (-p1, p1, p1), (0, -p2, 1), (0, p2, 1)
                );
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Icosahedron {
            get {
                ddouble p1 = (ddouble.Sqrt(5) - 1d) * 0.5d;

                return new(
                    new Connection(12,
                        (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (1, 2), (1, 3), (1, 6),
                        (1, 7), (2, 5), (2, 7), (2, 8), (3, 4), (3, 6), (3, 9), (4, 5),
                        (4, 9), (4, 10), (5, 8), (5, 10), (6, 7), (6, 9), (6, 11), (7, 8),
                        (7, 11), (8, 10), (8, 11), (9, 10), (9, 11), (10, 11)
                    ),
                    (0, -p1, -1), (0, p1, -1), (-1, 0, -p1), (1, 0, -p1),
                    (p1, -1, 0), (-p1, -1, 0), (p1, 1, 0), (-p1, 1, 0),
                    (-1, 0, p1), (1, 0, p1), (0, -p1, 1), (0, p1, 1)
                );
            }
        }
    }
}
