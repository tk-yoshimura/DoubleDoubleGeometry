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

        public IEnumerator<(int i, int j)> GetEnumerator() => (IEnumerator<(int i, int j)>)EnumerateEdge();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<(int i, int j)> EnumerateEdge() {
            for (int i = 0; i < Vertices; i++) {
                foreach (int j in map[i]) {
                    if (i >= j) {
                        continue;
                    }

                    yield return (i, j);
                }
            }
        }

        public IEnumerable<(int i, int j, int k)> EnumerateTriangle() {
            for (int i = 0; i < Vertices; i++) {
                foreach (int j in map[i]) {
                    if (i >= j) {
                        continue;
                    }

                    foreach (int k in map[j]) {
                        if (j >= k || !map[k].Contains(i)) {
                            continue;
                        }

                        yield return (i, j, k);
                    }
                }
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
