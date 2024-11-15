using DoubleDouble;

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

        public static Vector2D[] CircleLine(Circle2D circle, Line2D line) {
            Vector2D ev = circle.Center - line.Origin, dv = line.Direction;
            ddouble dv_sqnorm = dv.SquareNorm, radius = circle.Radius;

            ddouble v = radius * radius * dv_sqnorm - dv.X * dv.X * ev.Y * ev.Y - dv.Y * dv.Y * ev.X * ev.X + 2 * dv.X * dv.Y * ev.X * ev.Y;

            if (!(v >= 0d)) {
                return [];
            }

            ddouble ed_inner_product = Vector2D.Dot(ev, dv);

            if (v == 0d) {
                ddouble t = ed_inner_product / dv_sqnorm;

                Vector2D v1 = line.Origin + t * line.Direction;

                return [v1];
            }
            else {
                ddouble d = ddouble.Sqrt(v);
                ddouble t1 = (ed_inner_product - d) / dv_sqnorm;
                ddouble t2 = (ed_inner_product + d) / dv_sqnorm;

                Vector2D v1 = line.Origin + t1 * line.Direction;
                Vector2D v2 = line.Origin + t2 * line.Direction;

                return [v1, v2];
            }
        }
    }
}
