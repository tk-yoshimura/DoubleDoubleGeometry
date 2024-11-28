using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
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
            return g.Vertices > 0 && IsFinite(g) && Connection.IsValid(g.Connection);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<(Plane3D plane, bool is_convex)> planes = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<(Plane3D plane, bool is_convex)> Planes {
            get {
                if (planes is not null) {
                    return planes;
                }

                List<(Plane3D, bool)> plane_list = [];

                foreach (ReadOnlyCollection<int> face in Connection.Face) {
                    int n = face.Count;
                    Vector3D[] vertex_polygon = face.Select(idx => Vertex[idx]).ToArray();

                    Vector3D[] delta = vertex_polygon.Select((Vector3D v, int index) => vertex_polygon[(index + 1) % n] - v).ToArray();

                    Vector3D cross = Vector3D.Cross(delta[n - 1], delta[0]);
                    Vector3D normal = cross;

                    bool is_convex = true;

                    for (int i = 1; i < n; i++) {
                        Vector3D c = Vector3D.Cross(delta[i - 1], delta[i]);

                        if (Vector3D.Dot(cross, c) < 0d) {
                            c = -c;
                            is_convex = false;
                        }

                        normal += c;
                    }

                    normal = normal.Normal;

                    ddouble d = -vertex_polygon.Select(v => Vector3D.Dot(v, normal)).Average();

                    Plane3D plane = Plane3D.FromIntercept(normal, d);

                    plane_list.Add((plane, is_convex));
                }

                planes = plane_list.AsReadOnly();

                return planes;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? convex = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool Convex {
            get {
                if (convex is not null) {
                    return convex.Value;
                }

                if (Vertices <= 4) {
                    return convex ??= true;
                }

                ReadOnlyCollection<(Plane3D plane, bool is_convex)> planes = Planes;
                ReadOnlyCollection<ReadOnlyCollection<int>> faces = Connection.Face;

                foreach (((Plane3D plane, bool is_convex), ReadOnlyCollection<int> face) in planes.Zip(faces)) {
                    if (!is_convex) {
                        return convex ??= false;
                    }

                    List<ddouble> ss = [];

                    for (int vertex_index = 0; vertex_index < Vertices; vertex_index++) {
                        if (face.Contains(vertex_index)) {
                            continue;
                        }

                        Vector3D p = Vertex[vertex_index];

                        ddouble s = Vector3D.Dot(plane.Normal, p) + plane.D;

                        ss.Add(s);
                    }

                    if (ss.Any(s => s < 0d) && ss.Any(s => s > 0d)) {
                        return convex ??= false;
                    }
                }

                return convex ??= true;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<Polygon3D> polygons = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<Polygon3D> Polygons {
            get {
                if (polygons is not null) {
                    return polygons;
                }

                List<Polygon3D> polygon_list = [];

                ReadOnlyCollection<(Plane3D plane, bool is_convex)> planes = Planes;
                ReadOnlyCollection<ReadOnlyCollection<int>> faces = Connection.Face;

                foreach (((Plane3D plane, bool is_convex), ReadOnlyCollection<int> face) in planes.Zip(faces)) {
                    IEnumerable<Vector3D> vs = face.Select(i => Vertex[i]);
                    IEnumerable<Vector3D> us = plane.Projection(vs);

                    Vector3D center = (us.Max() + us.Min()) / 2d;
                    Polygon2D polygon = new(us.Select(u => (Vector2D)(u - center)));

                    Quaternion rot = Vector3D.Rot((0d, 0d, 1d), plane.Normal);

                    Polygon3D p = new(polygon, rot * new Vector3D(center.X, center.Y, -plane.D), rot);

                    polygon_list.Add(p);
                }

                polygons = polygon_list.AsReadOnly();

                return polygons;
            }
        }

        public IEnumerable<int> ValidateFaceFlatness(double abserr = 1e-28, double relerr = 1e-28, bool enable_throw_expection = true) {
            ReadOnlyCollection<(Plane3D plane, bool is_convex)> planes = Planes;
            ReadOnlyCollection<ReadOnlyCollection<int>> faces = Connection.Face;

            int face_index = 0;

            foreach (((Plane3D plane, bool is_convex), ReadOnlyCollection<int> face) in planes.Zip(faces)) {
                IEnumerable<Vector3D> vs = face.Select(i => Vertex[i]);
                IEnumerable<Vector3D> us = plane.Projection(vs);

                ddouble delta = ddouble.Abs(plane.D) * relerr + abserr;

                if (us.Any(u => ddouble.Abs(u.Z) >= delta)) {
                    if (enable_throw_expection) {
                        throw new ArithmeticException($"invalid face: index={face_index}");
                    }
                    else {
                        yield return face_index;
                    }
                }

                face_index++;
            }
        }

        public static bool IsConvex(Polyhedron3D g) {
            return g.Convex;
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
                (0, 1), (0, 3), (0, 4), (1, 2), (1, 5), (2, 3),
                (2, 6), (3, 7), (4, 5), (4, 7), (5, 6), (6, 7)
            ),
            (-1, -1, -1), (1, -1, -1), (1, 1, -1), (-1, 1, -1),
            (-1, -1, 1), (1, -1, 1), (1, 1, 1), (-1, 1, 1)
        );

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polyhedron3D Octahedron => new(
            new Connection(6,
                (0, 1), (0, 2), (0, 3), (0, 4), (1, 2), (1, 4),
                (1, 5), (2, 3), (2, 5), (3, 4), (3, 5), (4, 5)
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
