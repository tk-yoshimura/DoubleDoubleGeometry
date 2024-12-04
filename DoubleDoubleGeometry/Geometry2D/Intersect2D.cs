using DoubleDouble;
using System.Linq;

namespace DoubleDoubleGeometry.Geometry2D {

    public static class Intersect2D {

        public static Vector2D LineLine(Line2D line1, Line2D line2) {
            Vector2D v1 = line1.Origin, dv1 = line1.Direction, v2 = line2.Origin, dv2 = line2.Direction;

            ddouble vv1 = dv1.X * v1.Y - dv1.Y * v1.X;
            ddouble vv2 = dv2.X * v2.Y - dv2.Y * v2.X;
            ddouble vv12 = dv1.X * dv2.Y - dv1.Y * dv2.X;

            Vector2D y = new((vv1 * dv2.X - vv2 * dv1.X) / vv12, (vv1 * dv2.Y - vv2 * dv1.Y) / vv12);

            return y;
        }

        public static (Vector2D v, ddouble t)[] CircleLine(Circle2D circle, Line2D line) {
            Vector2D ev = circle.Center - line.Origin, dv = line.Direction;
            ddouble dv_sqnorm = dv.SquareNorm, radius = ddouble.Abs(circle.Radius);

            ddouble u = radius * radius * dv_sqnorm
                - dv.X * dv.X * ev.Y * ev.Y
                - dv.Y * dv.Y * ev.X * ev.X
                + ddouble.Ldexp(dv.X * dv.Y * ev.X * ev.Y, 1);

            if (!(u >= 0d)) {
                return [];
            }

            ddouble ed_inner_product = Vector2D.Dot(ev, dv);

            if (ddouble.IsZero(u)) {
                ddouble t = ed_inner_product / dv_sqnorm;

                Vector2D v = line.Origin + t * line.Direction;

                return [(v, t)];
            }
            else {
                ddouble d = ddouble.Sqrt(u);
                ddouble t1 = (ed_inner_product - d) / dv_sqnorm;
                ddouble t2 = (ed_inner_product + d) / dv_sqnorm;

                Vector2D v1 = line.Origin + t1 * line.Direction;
                Vector2D v2 = line.Origin + t2 * line.Direction;

                return [(v1, t1), (v2, t2)];
            }
        }

        public static Vector2D[] CircleCircle(Circle2D circle1, Circle2D circle2) {
            (ddouble a, ddouble b) = 2d * (circle1.Center - circle2.Center);
            ddouble c = circle1.Radius * circle1.Radius - circle2.Radius * circle2.Radius
                      - circle1.Center.SquareNorm + circle2.Center.SquareNorm;

            Line2D line = Line2D.FromImplicit(a, b, c);

            (Vector2D v, ddouble t)[] cross = CircleLine(circle1, line);

            return cross.Select(c => c.v).ToArray();
        }
    }
}
