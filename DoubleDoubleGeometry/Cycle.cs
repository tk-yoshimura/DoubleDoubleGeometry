using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace DoubleDoubleGeometry {

    [DebuggerDisplay("{ToString(),nq}")]
    public class Cycle : IEnumerable<int>, IEqualityOperators<Cycle, Cycle, bool>, IComparer<Cycle>, IComparable<Cycle> {
        private readonly ReadOnlyCollection<int> indexes;

        public Cycle(ReadOnlyCollection<int> indexes, bool enable_validation = true) {
            if (enable_validation) {
                if (indexes.Distinct().Count() != indexes.Count) {
                    throw new ArgumentException("duplicated index", nameof(indexes));
                }
                if (indexes.Any(i => i < 0)) { 
                    throw new ArgumentException("negative index", nameof(indexes));
                }
            }

            if (indexes.Count < 1) {
                throw new ArgumentException("empty array", nameof(indexes));
            }

            int n = indexes.Count;
            int min_index = indexes.IndexOf(indexes.Min());

            this.indexes = (new int[indexes.Count]).Select((_, i) => indexes[(i + min_index) % n]).ToArray().AsReadOnly();
        }

        public Cycle(params int[] indexes) :
            this(indexes.AsReadOnly()) { }

        public int this[int i] => indexes[i];

        public static implicit operator ReadOnlyCollection<int>(Cycle cycle) {
            return cycle.indexes;
        }

        public static implicit operator ReadOnlyCollection<(int from, int to)>(Cycle cycle) {
            return cycle.Edge;
        }

        public static bool operator ==(Cycle left, Cycle right) {
            return left.indexes.SequenceEqual(right.indexes);
        }

        public static bool operator !=(Cycle left, Cycle right) {
            return !(left == right);
        }

        public int Count => indexes.Count;

        public bool Contains(int from, int to) {
            int i0 = indexes.IndexOf(from);

            if (i0 < 0) {
                return false;
            }

            int i1 = (i0 + 1) % Count;

            bool is_contains = to == indexes[i1];
            
            return is_contains;
        }

        public bool IsOverlap(Cycle cycle) {
            foreach ((int from, int to) in cycle.Edge) {
                if (Contains(from, to)) {
                    return true;
                }
            }

            return false;
        }

        public static Cycle Opposite(Cycle cycle) {
            ReadOnlyCollection<int> indexes = cycle.indexes;

            if (indexes.Count < 2) {
                return cycle;
            }

            List<int> indexes_reversed = [indexes[0]];

            for (int i = 1; i < indexes.Count; i++) {
                indexes_reversed.Add(indexes[^i]);
            }

            return new(indexes_reversed.AsReadOnly(), enable_validation: false);
        }

        public IEnumerator<int> GetEnumerator() => indexes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ReadOnlyCollection<(int i, int j)> edge;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlyCollection<(int i, int j)> Edge {
            get {
                if (edge is not null) {
                    return edge;
                }

                List<(int i, int j)> e = [];

                for (int i = 1, n = Count; i < n; i++) {
                    e.Add((indexes[i - 1], indexes[i]));
                }

                e.Add((indexes[Count - 1], indexes[0]));

                edge = e.AsReadOnly();

                return edge;
            }
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Cycle cycle && cycle == this);
        }

        public bool Equals(Cycle other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            int hash = indexes[0].GetHashCode();

            for (int i = 1, n = int.Min(4, Count); i < n; i++) {
                hash ^= indexes[i].GetHashCode();
            }

            return hash;
        }

        public override string ToString() {
            return string.Join(" -> ", indexes);
        }

        public int Compare(Cycle x, Cycle y) {
            for (int i = 0, n = int.Min(x.Count, y.Count); i < n; i++) {
                if (x[i] < y[i]) {
                    return -1;
                }

                if (x[i] > y[i]) {
                    return +1;
                }
            }

            if (x.Count < y.Count) {
                return -1;
            }

            if (x.Count > y.Count) {
                return +1;
            }

            return 0;
        }

        public int CompareTo(Cycle other) {
            return Compare(this, other);
        }
    }
}
