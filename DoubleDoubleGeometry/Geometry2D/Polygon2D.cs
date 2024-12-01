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

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2D? center = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Center => center ??= (Vertex.Max() + Vertex.Min()) / 2d;

#pragma warning disable CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2D? size = null;
#pragma warning restore CS8632
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Vector2D Size => size ??= Vertex.Max() - Vertex.Min();

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
            int n = Vertices;

            Vector2D u = v - Center;

            if (ddouble.Ldexp(u.X, 1) > Size.X || ddouble.Ldexp(u.Y, 1) > Size.Y) {
                return false;
            }

            if (IsConvex(this)) {
                int sgn = ddouble.Sign(Vector2D.Cross(v - Vertex[0], Vertex[n - 1] - Vertex[0]));

                for (int i = 1; i < n; i++) {
                    int s = ddouble.Sign(Vector2D.Cross(v - Vertex[i], Vertex[i - 1] - Vertex[i]));

                    if (sgn * s < 0) {
                        return false;
                    }

                    if (sgn == 0 && s != 0) {
                        sgn = s;
                    }
                }

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

                int crosses = 0;

                if (is_cross_h(dv[n - 1], dv[0])) {
                    crosses++;
                }
                for (int i = 1; i < n; i++) {
                    if (is_cross_h(dv[i - 1], dv[i])) {
                        crosses++;
                    }
                }

                bool inside = (crosses & 1) == 1;

                return inside;
            }
        }

        public IEnumerable<bool> Inside(IEnumerable<Vector2D> vs) {
            int n = Vertices;

            if (IsConvex(this)) {
                Vector2D[] delta = Vertex.Select((Vector2D v, int index) => Vertex[(index + n - 1) % n] - v).ToArray();

                foreach (Vector2D v in vs) {
                    Vector2D u = v - Center;

                    if (ddouble.Ldexp(u.X, 1) > Size.X || ddouble.Ldexp(u.Y, 1) > Size.Y) {
                        yield return false;
                        continue;
                    }

                    int sgn = ddouble.Sign(Vector2D.Cross(v - Vertex[0], delta[0]));

                    bool inside = true;

                    for (int i = 1; i < n; i++) {
                        int s = ddouble.Sign(Vector2D.Cross(v - Vertex[i], delta[i]));

                        if (sgn * s < 0) {
                            inside = false;
                            break;
                        }

                        if (sgn == 0 && s != 0) {
                            sgn = s;
                        }
                    }

                    yield return inside;
                }
            }
            else {
                foreach (Vector2D v in vs) {
                    Vector2D u = v - Center;

                    if (ddouble.Ldexp(u.X, 1) > Size.X || ddouble.Ldexp(u.Y, 1) > Size.Y) {
                        yield return false;
                        continue;
                    }

                    int crosses = 0;

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

                    if (is_cross_h(dv[n - 1], dv[0])) {
                        crosses++;
                    }

                    for (int i = 1; i < n; i++) {
                        if (is_cross_h(dv[i - 1], dv[i])) {
                            crosses++;
                        }
                    }

                    bool inside = (crosses & 1) == 1;

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
