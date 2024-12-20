﻿using DoubleDouble;

namespace DoubleDoubleGeometry.Geometry3D {

    public static class Intersect3D {

        public static Vector3D LineLine(Line3D line1, Line3D line2, ddouble distance_threshold) {
            Vector3D v1 = line1.Origin, dv1 = line1.Direction, v2 = line2.Origin, dv2 = line2.Direction;

            ddouble d1dv1 = Vector3D.Dot(v1, dv1);
            ddouble d1dv2 = Vector3D.Dot(v1, dv2);
            ddouble d2dv1 = Vector3D.Dot(v2, dv1);
            ddouble d2dv2 = Vector3D.Dot(v2, dv2);
            ddouble dv1dv2 = Vector3D.Dot(dv1, dv2);

            ddouble inn = 1d / (dv1dv2 * dv1dv2 - 1d);

            ddouble f1 = d2dv2 - d1dv2;
            ddouble f2 = d1dv1 - d2dv1;

            ddouble t1 = (dv1dv2 * f1 + f2) * inn;
            ddouble t2 = (dv1dv2 * f2 + f1) * inn;

            Vector3D rt1 = v1 + dv1 * t1;
            Vector3D rt2 = v2 + dv2 * t2;

            Vector3D y = Vector3D.Distance(rt1, rt2) < distance_threshold
                ? (rt1 + rt2) * 0.5d
                : Vector3D.Invalid;

            return y;
        }

        public static (Vector3D v, ddouble t) LinePlane(Line3D line, Plane3D plane) {
            ddouble inn = Vector3D.Dot(line.Direction, plane.Normal);
            ddouble t = -(Vector3D.Dot(line.Origin, plane.Normal) + plane.D) / inn;

            Vector3D v = line.Origin + line.Direction * t;

            return (v, t);
        }

        public static (Vector3D v, ddouble t) LineTriangle(Line3D line, Triangle3D triangle) {
            Vector3D dir = line.Direction;

            Vector3D eu = triangle.V1 - triangle.V0;
            Vector3D ev = triangle.V2 - triangle.V0;

            Vector3D pvec = Vector3D.Cross(dir, ev), qvec;
            ddouble det = Vector3D.Dot(eu, pvec);

            if (det > 0d) {
                Vector3D tvec = line.Origin - triangle.V0;
                ddouble inv_u = Vector3D.Dot(tvec, pvec);
                if (inv_u < 0d || inv_u > det) {
                    return (Vector3D.Invalid, ddouble.NaN);
                }

                qvec = Vector3D.Cross(tvec, eu);
                ddouble inv_v = Vector3D.Dot(dir, qvec);
                if (inv_v < 0d || inv_u + inv_v > det) {
                    return (Vector3D.Invalid, ddouble.NaN);
                }
            }
            else if (det < 0d) {
                Vector3D tvec = line.Origin - triangle.V0;
                ddouble inv_u = Vector3D.Dot(tvec, pvec);
                if (inv_u > 0d || inv_u < det) {
                    return (Vector3D.Invalid, ddouble.NaN);
                }

                qvec = Vector3D.Cross(tvec, eu);
                ddouble inv_v = Vector3D.Dot(dir, qvec);
                if (inv_v > 0d || inv_u + inv_v < det) {
                    return (Vector3D.Invalid, ddouble.NaN);
                }
            }
            else {
                return (Vector3D.Invalid, ddouble.NaN);
            }

            ddouble t = Vector3D.Dot(ev, qvec) / det;

            Vector3D v = line.Origin + dir * t;

            return (v, t);
        }

        public static (Vector3D v, ddouble t) LineRectangle(Line3D line, Rectangle3D rectangle) {
            Line3D line_rot = rectangle.Rotation.Conj * (line - rectangle.Center);

            ddouble t = -line_rot.Origin.Z / line_rot.Direction.Z;
            ddouble x = line_rot.Origin.X + line_rot.Direction.X * t;
            ddouble y = line_rot.Origin.Y + line_rot.Direction.Y * t;

            bool inside = (ddouble.Abs(x) <= ddouble.Abs(rectangle.Scale.X)) && (ddouble.Abs(y) <= ddouble.Abs(rectangle.Scale.Y));

            if (!inside) {
                return (Vector3D.Invalid, ddouble.NaN);
            }

            Vector3D v = line.Point(t);

            return (v, t);
        }

        public static (Vector3D v, ddouble t) LinePolygon(Line3D line, Polygon3D polygon) {
            Line3D line_rot = polygon.Rotation.Conj * (line - polygon.Center);

            ddouble t = line_rot.Origin.Z / line_rot.Direction.Z;
            ddouble x = line_rot.Origin.X - line_rot.Direction.X * t;
            ddouble y = line_rot.Origin.Y - line_rot.Direction.Y * t;

            bool inside = polygon.Polygon.Inside((x, y));

            if (!inside) {
                return (Vector3D.Invalid, ddouble.NaN);
            }

            t = -t;

            Vector3D v = line.Point(t);

            return (v, t);
        }

        public static (Vector3D v, ddouble t) LineCircle(Line3D line, Circle3D circle) {
            (Vector3D v, ddouble t) = LinePlane(line, Plane3D.FromNormal(circle.Center, circle.Normal));

            if (Vector3D.SquareDistance(v, circle.Center) <= circle.Radius * circle.Radius) {
                return (v, t);
            }
            else {
                return (Vector3D.Invalid, ddouble.NaN);
            }
        }

        public static (Vector3D v, ddouble t)[] LineSphere(Line3D line, Sphere3D sphere) {
            Vector3D otoc = line.Origin - sphere.Center;

            ddouble b = ddouble.Ldexp(Vector3D.Dot(line.Direction, otoc), 1);
            ddouble c = otoc.SquareNorm - sphere.Radius * sphere.Radius;
            ddouble u = b * b - ddouble.Ldexp(c, 2);

            if (!(u >= 0d)) {
                return [];
            }

            if (ddouble.IsZero(u)) {
                ddouble t = -ddouble.Ldexp(b, -1);
                Vector3D v = line.Origin + t * line.Direction;

                return [(v, t)];
            }
            else {
                ddouble d = ddouble.Sqrt(u);
                ddouble t1 = -ddouble.Ldexp(b + d, -1), t2 = -ddouble.Ldexp(b - d, -1);
                Vector3D v1 = line.Origin + t1 * line.Direction;
                Vector3D v2 = line.Origin + t2 * line.Direction;

                return [(v1, t1), (v2, t2)];
            }
        }

        public static Line3D PlanePlane(Plane3D plane1, Plane3D plane2) {
            Vector3D normal1 = plane1.Normal, normal2 = plane1.Normal;

            Vector3D line_dir = Vector3D.NormalizeSign(Vector3D.Cross(plane1.Normal, plane2.Normal).Normal);

            int index = Vector3D.MaxAbsIndex(line_dir);

            if (index == 0) {
                ddouble inv = 1d / line_dir.X;

                Vector3D line_org = new(
                    0d,
                    (plane1.D * normal2.Z - plane2.D * normal1.Z) * inv,
                    (plane2.D * normal1.Y - plane1.D * normal2.Y) * inv
                );

                return Line3D.FromDirection(line_org, line_dir);
            }
            else if (index == 1) {
                ddouble inv = 1d / line_dir.Y;

                Vector3D line_org = new(
                    (plane2.D * normal1.Z - plane1.D * normal2.Z) * inv,
                    0d,
                    (plane1.D * normal2.X - plane2.D * normal1.X) * inv
                );

                return Line3D.FromDirection(line_org, line_dir);
            }
            else {
                ddouble inv = 1d / line_dir.Z;

                Vector3D line_org = new(
                    (plane1.D * normal2.Y - plane2.D * normal1.Y) * inv,
                    (plane2.D * normal1.X - plane1.D * normal2.X) * inv,
                    0d
                );

                return Line3D.FromDirection(line_org, line_dir);
            }
        }

        public static Circle3D PlaneSphere(Plane3D plane, Sphere3D sphere) {
            ddouble t = -(Vector3D.Dot(sphere.Center, plane.Normal) + plane.D);

            if (ddouble.Abs(t) > ddouble.Abs(sphere.Radius)) {
                return Circle3D.Invalid;
            }

            Circle3D y = new(
                sphere.Center + plane.Normal * t,
                ddouble.Sqrt(sphere.Radius * sphere.Radius - t * t),
                plane.Normal
            );

            return y;
        }

        public static Circle3D SphereSphere(Sphere3D sphere1, Sphere3D sphere2) {
            ddouble r0 = ddouble.Abs(sphere1.Radius), r1 = ddouble.Abs(sphere2.Radius);
            Vector3D c0 = sphere1.Center, c1 = sphere2.Center;

            Vector3D c01 = c1 - c0;
            ddouble r01 = c01.SquareNorm;
            ddouble rm01 = r0 - r1;
            ddouble rp01 = r0 + r1;

            if (((rp01 * rp01) <= r01) || ((rm01 * rm01) >= r01)) {
                return Circle3D.Invalid;
            }

            ddouble inv_d = 1d / ddouble.Sqrt(r01);
            ddouble r0_sq = r0 * r0;
            ddouble x = (r01 + r0_sq - r1 * r1) * inv_d * 0.5d;
            ddouble h = ddouble.Sqrt(r0_sq - x * x);

            Vector3D center = c0 + c01 * x * inv_d;

            Circle3D y = new(center, h, c01);

            return y;
        }
    }
}
