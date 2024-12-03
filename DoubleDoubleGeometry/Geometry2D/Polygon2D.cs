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

        public Polygon2D(IEnumerable<Vector2D> vertex) : this(vertex.ToArray()) { }

        public int Vertices => Vertex.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Center => BoundingBox.Center;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Size => BoundingBox.Size;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ddouble? area = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Area {
            get {
                if (area is not null) {
                    return area.Value;
                }

                if (Vertices <= 2) {
                    return area ??= 0d;
                }

                if (!IsValid(this)) {
                    return area ??= ddouble.NaN;
                }

                int n = Vertices;

                ddouble s = Vector2D.Cross(Vertex[n - 1], Vertex[0]);

                for (int i = 1; i < n; i++) {
                    s += Vector2D.Cross(Vertex[i - 1], Vertex[i]);
                }

                area ??= ddouble.Ldexp(ddouble.Abs(s), -1);

                return area.Value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ddouble? perimeter = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ddouble Perimeter {
            get {
                if (perimeter is not null) {
                    return perimeter.Value;
                }

                if (Vertices <= 1) {
                    return perimeter ??= 0d;
                }

                int n = Vertices;

                ddouble s = Vector2D.Distance(Vertex[n - 1], Vertex[0]);

                for (int i = 1; i < n; i++) {
                    s += Vector2D.Distance(Vertex[i - 1], Vertex[i]);
                }

                perimeter ??= s;

                return perimeter.Value;
            }
        }

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

        public static Polygon2D operator *(Matrix2D m, Polygon2D g) {
            return new(g.Vertex.Select(p => m * p));
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
            return g1.Vertex.SequenceEqual(g2.Vertex);
        }

        public static bool operator !=(Polygon2D g1, Polygon2D g2) {
            return !(g1 == g2);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon2D Invalid { get; } = new(Vector2D.Invalid);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Polygon2D Zero { get; } = new(Vector2D.Zero);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BoundingBox2D bbox = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BoundingBox2D BoundingBox => bbox ??= new BoundingBox2D(Vertex);

        public bool Inside(Vector2D v) {
            bool is_convex = IsConvex(this);
            int n = Vertices;

            if (!BoundingBox.Inside(v)) {
                return false;
            }

            ReadOnlyCollection<Line2D> hull_lines = HullLines;

            foreach (Line2D line in hull_lines) {
                ddouble s = line.A * v.X + line.B * v.Y + line.C;

                if (s > 0d) {
                    return false;
                }
            }

            if (is_convex) {
                return true;
            }
            else {
                static bool is_cross_h(Vector2D v0, Vector2D v1) {
                    if ((v0.Y <= 0d) != (v1.Y > 0d)) {
                        return false;
                    }
                    if (v0.X < 0d && v1.X < 0d) {
                        return false;
                    }

                    ddouble x = (v0.Y <= 0d)
                        ? (v0.X * v1.Y - v1.X * v0.Y)
                        : (v1.X * v0.Y - v0.X * v1.Y);

                    return x >= 0d;
                }

                Vector2D[] dv = Vertex.Select(vertex => vertex - v).ToArray();

                bool inside = is_cross_h(dv[n - 1], dv[0]);
                for (int i = 1; i < n; i++) {
                    if (is_cross_h(dv[i - 1], dv[i])) {
                        inside = !inside;
                    }
                }

                return inside;
            }
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector2D> vs) {
            bool is_convex = IsConvex(this);
            int n = Vertices;
            BoundingBox2D bbox = BoundingBox;
            ReadOnlyCollection<Line2D> hull_lines = HullLines;

            foreach (Vector2D v in vs) {
                if (!bbox.Inside(v)) {
                    yield return false;
                }

                bool inside = true;

                foreach (Line2D line in hull_lines) {
                    ddouble s = line.A * v.X + line.B * v.Y + line.C;

                    if (s > 0d) {
                        inside = false;
                        break;
                    }
                }

                if (is_convex) {
                    yield return inside;
                }
                else {
                    static bool is_cross_h(Vector2D v0, Vector2D v1) {
                        if ((v0.Y <= 0d) != (v1.Y > 0d)) {
                            return false;
                        }
                        if (v0.X < 0d && v1.X < 0d) {
                            return false;
                        }

                        ddouble x = (v0.Y <= 0d)
                            ? (v0.X * v1.Y - v1.X * v0.Y)
                            : (v1.X * v0.Y - v0.X * v1.Y);

                        return x >= 0d;
                    }

                    Vector2D[] dv = Vertex.Select(vertex => vertex - v).ToArray();

                    inside = is_cross_h(dv[n - 1], dv[0]);
                    for (int i = 1; i < n; i++) {
                        if (is_cross_h(dv[i - 1], dv[i])) {
                            inside = !inside;
                        }
                    }

                    yield return inside;
                }
            }
        }

        public static bool IsNaN(Polygon2D g) {
            return g.Vertices < 1 || g.Vertex.Any(Vector2D.IsNaN);
        }

        public static bool IsZero(Polygon2D g) {
            return g.Vertices < 1 || g.Vertex.All(Vector2D.IsZero);
        }

        public static bool IsFinite(Polygon2D g) {
            return g.Vertex.All(Vector2D.IsFinite);
        }

        public static bool IsInfinity(Polygon2D g) {
            return !IsFinite(g);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? valid = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool Valid {
            get {
                if (valid is not null) {
                    return valid.Value;
                }

                if (Vertices <= 0 || !IsFinite(this)) {
                    return valid ??= false;
                }

                if (Vertices <= 3) {
                    return valid ??= true;
                }

                static bool is_cross(Vector2D a, Vector2D b, Vector2D c, Vector2D d) {
                    Vector2D ab = b - a, ac = c - a, ad = d - a;

                    ddouble s = Vector2D.Cross(ab, ac), t = Vector2D.Cross(ab, ad);

                    if (s * t > 0d) {
                        return false;
                    }

                    Vector2D cd = d - c, ca = a - c, cb = b - c;

                    ddouble u = Vector2D.Cross(cd, ca), v = Vector2D.Cross(cd, cb);

                    if (u * v > 0d) {
                        return false;
                    }

                    return true;
                }

                int n = Vertices;

                for (int i = 1; i < n; i++) {
                    Vector2D a = Vertex[i - 1], b = Vertex[i];

                    for (int j = i + 1; j < n - 1; j++) {
                        Vector2D c = Vertex[j], d = Vertex[j + 1];

                        if (is_cross(a, b, c, d)) {
                            return valid ??= false;
                        }
                    }

                    if (i + 1 < n && i > 1) {
                        Vector2D c = Vertex[n - 1], d = Vertex[0];

                        if (is_cross(a, b, c, d)) {
                            return valid ??= false;
                        }
                    }
                }

                return valid ??= true;
            }
        }

        public static bool IsValid(Polygon2D g) {
            return g.Valid;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<Line2D> hull_lines = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyCollection<Line2D> HullLines {
            get {
                if (hull_lines is not null) {
                    return hull_lines;
                }

                List<Line2D> hull_line_list = [];

                int n = Vertices;

                for (int i0 = 0, i1 = 1; i0 < n; i0++, i1 = (i0 + 1) % n) {
                    Line2D line = Line2D.FromIntersection(Vertex[i0], Vertex[i1]);

                    (ddouble a, ddouble b, ddouble c) = line;

                    List<ddouble> ss = [];

                    for (int j = 0; j < n; j++) {
                        if (j == i0 || j == i1) {
                            continue;
                        }

                        Vector2D v = Vertex[j];

                        ddouble s = a * v.X + b * v.Y + c;

                        ss.Add(s);
                    }

                    if (ss.Any(s => s < 0d) && ss.Any(s => s > 0d)) {
                        continue;
                    }

                    if (ss.All(s => s > 0d)) {
                        (a, b, c) = (-a, -b, -c);
                    }

                    hull_line_list.Add(Line2D.FromImplicit(a, b, c));
                }

                hull_lines = hull_line_list.AsReadOnly();

                return hull_lines;
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

                if (Vertices <= 3) {
                    return convex ??= true;
                }

                int n = Vertices;
                Vector2D[] delta = Vertex.Select((Vector2D v, int index) => Vertex[(index + 1) % n] - v).ToArray();

                int sgn = ddouble.Sign(Vector2D.Cross(delta[n - 1], delta[0]));

                for (int i = 1; i < n; i++) {
                    int s = ddouble.Sign(Vector2D.Cross(delta[i - 1], delta[i]));

                    if (sgn * s < 0) {
                        return convex ??= false;
                    }

                    if (sgn == 0 && s != 0) {
                        sgn = s;
                    }
                }

                return convex ??= true;
            }
        }

        public static bool IsConvex(Polygon2D g) {
            return g.Convex;
        }

        public static bool IsConcave(Polygon2D g) {
            return !IsConvex(g) && IsValid(g);
        }

        public override string ToString() {
            return $"polygon vertices={Vertices}";
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Polygon2D g && g == this);
        }

        public bool Equals(Polygon2D other) {
            return ReferenceEquals(this, other) || (other is not null && other == this);
        }

        public override int GetHashCode() {
            return Vertices > 0 ? Vertex[0].GetHashCode() : 0;
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
