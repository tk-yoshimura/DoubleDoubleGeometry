using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace DoubleDoubleGeometry {
    [DebuggerDisplay("{ToString(),nq}")]
    public class Connection : IEqualityOperators<Connection, Connection, bool>, IEquatable<Connection>, IEnumerable<(int i, int j)> {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ReadOnlyCollection<ReadOnlyCollection<int>> map;

        public readonly long Vertices, Edges;

        public Connection(int n, params (int a, int b)[] connection_indexes) {
            List<int>[] map = (new List<int>[n]).Select(_ => new List<int>()).ToArray();

            long count = 0;

            foreach ((int i, int j) in connection_indexes) {
                if (i < 0 || i >= n || j < 0 || j >= n || i == j) {
                    throw new ArgumentOutOfRangeException(nameof(connection_indexes), "contains invalid index");
                }
                if (map[i].Contains(j) || map[j].Contains(i)) {
                    throw new ArgumentException("duplicated connection", nameof(connection_indexes));
                }

                map[i].Add(j);
                map[j].Add(i);
                count++;
            }

            for (int i = 0; i < n; i++) {
                map[i].Sort();
            }

            this.map = map.Select(item => item.AsReadOnly()).ToArray().AsReadOnly();
            this.Vertices = n;
            this.Edges = count;
        }

        public Connection(int n, bool[,] adjacency_matrix) {
            if ((adjacency_matrix.GetLength(0), adjacency_matrix.GetLength(1)) != (n, n)) {
                throw new ArgumentException("mismatch size", nameof(adjacency_matrix));
            }

            List<int>[] map = (new List<int>[n]).Select(_ => new List<int>()).ToArray();

            long count = 0;

            for (int i = 0; i < n; i++) {
                for (int j = i; j < n; j++) {
                    if ((i == j && adjacency_matrix[i, j]) || (i != j && adjacency_matrix[i, j] != adjacency_matrix[j, i])) {
                        throw new ArgumentOutOfRangeException(nameof(adjacency_matrix), "contains invalid index");
                    }

                    map[i].Add(j);
                    map[j].Add(i);
                    count++;
                }
            }

            this.map = map.Select(item => item.AsReadOnly()).ToArray().AsReadOnly();
            this.Vertices = n;
            this.Edges = count;
        }

        public Connection(int n, int[,] adjacency_matrix) {
            if ((adjacency_matrix.GetLength(0), adjacency_matrix.GetLength(1)) != (n, n)) {
                throw new ArgumentException("mismatch size", nameof(adjacency_matrix));
            }

            List<int>[] map = (new List<int>[n]).Select(_ => new List<int>()).ToArray();

            long count = 0;

            for (int i = 0; i < n; i++) {
                for (int j = i; j < n; j++) {
                    if ((i == j && adjacency_matrix[i, j] > 0) || (i != j && adjacency_matrix[i, j] > 0 != adjacency_matrix[j, i] > 0)) {
                        throw new ArgumentOutOfRangeException(nameof(adjacency_matrix), "contains invalid index");
                    }

                    map[i].Add(j);
                    map[j].Add(i);
                    count++;
                }
            }

            this.map = map.Select(item => item.AsReadOnly()).ToArray().AsReadOnly();
            this.Vertices = n;
            this.Edges = count;
        }

        public Connection(int n, params Cycle[] faces) {
            List<Cycle> face_list = [];

            for (int i = 0; i < faces.Length; i++) {
                Cycle face = faces[i];

                if (face.Max() >= n) { 
                    throw new ArgumentOutOfRangeException(nameof(faces), "contains invalid index");
                }

                for (int j = 0; j < face_list.Count; j++) {
                    if (face_list[j].IsOverlap(face)) {
                        face = Cycle.Opposite(face);
                        if (face_list[j].IsOverlap(face)) {
                            throw new ArgumentOutOfRangeException(nameof(faces), "duplicated edge");
                        }
                    }
                }

                face_list.Add(face);
            }

            List<int>[] map = (new List<int>[n]).Select(_ => new List<int>()).ToArray();

            long count = 0;

            foreach (Cycle face in face_list) {
                foreach ((int i, int j) in face.Edge) {
                    map[i].Add(j);
                    map[j].Add(i);
                    count++;
                }
            }

            this.map = map.Select(item => item.AsReadOnly()).ToArray().AsReadOnly();
            this.Vertices = n;
            this.Edges = count / 2;
            this.cycles = face_list.AsReadOnly();
        }

        public Connection(int n, IEnumerable<(int a, int b)> connection_indexes) : this(n, connection_indexes.ToArray()) { }

        public ReadOnlyCollection<int> this[int index] => map[index];

        public bool this[int a, int b] => map[a].Contains(b);

        public static bool operator ==(Connection c1, Connection c2) {
            if (c1.Vertices != c2.Vertices || c1.Edges != c2.Edges) {
                return false;
            }

            for (int i = 0; i < c1.Vertices; i++) {
                if (c1.map[i].Count != c2.map[i].Count) {
                    return false;
                }

                if (!c1.map[i].SequenceEqual(c2.map[i])) {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Connection c1, Connection c2) {
            return !(c1 == c2);
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Connection c && c == this);
        }

        public bool Equals(Connection other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public IEnumerator<(int i, int j)> GetEnumerator() => Edge.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<(int i, int j)> EnumerateEdge() {
            ReadOnlyCollection<(int i, int j)> edge = Edge;

            foreach (var e in edge) {
                yield return e;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ReadOnlyCollection<(int i, int j)> edge;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<(int i, int j)> Edge {
            get {
                if (edge is not null) {
                    return edge;
                }

                List<(int i, int j)> e = [];

                for (int i = 0; i < Vertices; i++) {
                    foreach (int j in map[i]) {
                        if (i >= j) {
                            continue;
                        }

                        e.Add((i, j));
                    }
                }

                edge = e.AsReadOnly();

                return edge;
            }
        }

        public IEnumerable<(int i, int j, int k)> EnumerateTriangle() {
            ReadOnlyCollection<(int i, int j, int k)> triangle = Triangle;

            foreach (var t in triangle) {
                yield return t;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ReadOnlyCollection<(int i, int j, int k)> triangle;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<(int i, int j, int k)> Triangle {
            get {
                if (triangle is not null) {
                    return triangle;
                }

                List<(int i, int j, int k)> t = [];

                for (int i = 0; i < Vertices; i++) {
                    foreach (int j in map[i]) {
                        if (i >= j) {
                            continue;
                        }

                        foreach (int k in map[j]) {
                            if (j >= k || !map[k].Contains(i)) {
                                continue;
                            }

                            t.Add((i, j, k));
                        }
                    }
                }

                triangle = t.AsReadOnly();

                return triangle;
            }
        }

        public IEnumerable<Cycle> EnumerateCycle() {
            ReadOnlyCollection<Cycle> cycles = Cycles;

            foreach (var c in cycles) {
                yield return c;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<Cycle> cycles = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<Cycle> Cycles {
            get {
                if (this.cycles is not null) {
                    return this.cycles;
                }

                List<Cycle> cycles = [];

                List<(int from, int to)> unused_edge = [];
                foreach ((int from, int to) in EnumerateEdge()) {
                    unused_edge.Add((from, to));
                    unused_edge.Add((to, from));
                }

                List<(int from, int to)> start_edge = [unused_edge.First()];

                while (start_edge.Count > 0) {
                    (int start_node, int to_node) = start_edge.First();
                    start_edge.RemoveAt(0);

                    List<(int from, int to)> visited_edge = [];

                    Queue<(int from, int to)> queue = new();
                    queue.Enqueue((start_node, to_node));

                    Dictionary<(int from, int to), List<int>> paths = [];
                    paths[(start_node, to_node)] = [start_node, to_node];

                    bool searched = false;

                    while (queue.Count > 0 && !searched) {
                        (int from_node, to_node) = queue.Dequeue();
                        List<int> path = paths[(from_node, to_node)];

                        visited_edge.Add((from_node, to_node));

                        foreach (int next_node in map[to_node]) {
                            if (next_node == from_node || !unused_edge.Contains((to_node, next_node))) {
                                continue;
                            }

                            if (next_node == start_node) {
#if DEBUG
                                Cycle new_cycle = new(path.AsReadOnly());
#else
                                Cycle new_cycle = new(path.AsReadOnly(), enable_validation: false);
#endif
                                if (cycles.Contains(new_cycle) || cycles.Contains(Cycle.Opposite(new_cycle))) {
                                    continue;
                                }

                                cycles.Add(new_cycle);

                                searched = true;
                                break;
                            }

                            if (path.Contains(next_node)) {
                                continue;
                            }

                            (int, int) edge = (to_node, next_node);
                            queue.Enqueue(edge);
                            paths[edge] = [.. path, next_node];
                        }
                    }

                    if (searched) {
                        Cycle cycle = cycles.Last();

                        foreach ((int i, int j) in cycle.Edge) {
                            Debug.Assert(unused_edge.Contains((i, j)), $"{i}, {j}");

                            unused_edge.Remove((i, j));
                            start_edge.Remove((i, j));

                            if (unused_edge.Contains((j, i)) && !start_edge.Contains((j, i))) {
                                start_edge.Add((j, i));
                            }
                        }
                    }
                }

                if (unused_edge.Count > 0) {
                    throw new ArithmeticException(
                        $"unreachable edges: " +
                        $"{string.Join(", ", unused_edge.Where(e => e.from < e.to))}"
                    );
                }
                
                this.cycles = cycles.Order().ToArray().AsReadOnly();

                return this.cycles;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool[,] AdjacencyMatrix {
            get {
                bool[,] matrix = new bool[Vertices, Vertices];

                foreach ((int i, int j) in EnumerateEdge()) {
                    matrix[i, j] = matrix[j, i] = true;
                }

                return matrix;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? valid = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool Valid {
            get {
                if (valid is not null) {
                    return valid.Value;
                }

                List<int> visited_node = [];
                Stack<int> stack = new([0]);

                while (stack.Count > 0) {
                    Debug.Assert(visited_node.Count + stack.Count <= Vertices);

                    if (visited_node.Count + stack.Count >= Vertices) {
                        return valid ??= true;
                    }

                    int current_node = stack.Pop();
                    visited_node.Add(current_node);

                    foreach (int next_node in map[current_node]) {
                        if (visited_node.Contains(next_node) || stack.Contains(next_node)) {
                            continue;
                        }

                        stack.Push(next_node);
                    }
                }

                return valid ??= false;
            }
        }

        public static bool IsValid(Connection c) {
            return c.Valid;
        }

        public override string ToString() {
            return $"connection vertices={Vertices}, edges={Edges}";
        }

        public override int GetHashCode() {
            return Vertices.GetHashCode() ^ Edges.GetHashCode();
        }
    }
}
