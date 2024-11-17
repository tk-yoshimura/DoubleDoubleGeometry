using DoubleDouble;

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
                ? (rt1 + rt2) / 2d
                : Vector3D.Invalid;

            return y;
        }

        public static Vector3D LinePlane(Line3D line, Plane3D plane) {
            ddouble inn = Vector3D.Dot(line.Direction, plane.Normal);
            ddouble t = -(Vector3D.Dot(line.Origin, plane.Normal) + plane.D) / inn;

            return line.Origin + line.Direction * t;
        }

        public static Vector3D LineTriangle(Line3D line, Triangle3D triangle) {
            Vector3D dir = line.Direction;

            Vector3D eu = triangle.V1 - triangle.V0;
            Vector3D ev = triangle.V2 - triangle.V0;

            Vector3D pvec = Vector3D.Cross(dir, ev), qvec;
            ddouble det = Vector3D.Dot(eu, pvec);

            if (det > 0d) {
                Vector3D tvec = line.Origin - triangle.V0;
                ddouble inv_u = Vector3D.Dot(tvec, pvec);
                if (inv_u < 0d || inv_u > det) {
                    return Vector3D.Invalid;
                }

                qvec = Vector3D.Cross(tvec, eu);
                ddouble inv_v = Vector3D.Dot(dir, qvec);
                if (inv_v < 0d || inv_u + inv_v > det) {
                    return Vector3D.Invalid;
                }
            }
            else if (det < 0d) {
                Vector3D tvec = line.Origin - triangle.V0;
                ddouble inv_u = Vector3D.Dot(tvec, pvec);
                if (inv_u > 0d || inv_u < det) {
                    return Vector3D.Invalid;
                }

                qvec = Vector3D.Cross(tvec, eu);
                ddouble inv_v = Vector3D.Dot(dir, qvec);
                if (inv_v > 0d || inv_u + inv_v < det) {
                    return Vector3D.Invalid;
                }
            }
            else {
                return Vector3D.Invalid;
            }

            ddouble t = Vector3D.Dot(ev, qvec) / det;

            Vector3D y = line.Origin + dir * t;

            return y;
        }

        public static Vector3D LineCircle(Line3D line, Circle3D circle) {
            Vector3D cross = LinePlane(line, Plane3D.FromNormal(circle.Normal, circle.Center));

            Vector3D y = Vector3D.SquareDistance(cross, circle.Center) < circle.Radius * circle.Radius
                ? cross
                : Vector3D.Invalid;

            return y;
        }

        public static Vector3D[] LineSphere(Line3D line, Sphere3D sphere) {
            Vector3D otoc = line.Origin - sphere.Center;

            ddouble b = 2d * Vector3D.Dot(line.Direction, otoc);
            ddouble c = otoc.SquareNorm - sphere.Radius * sphere.Radius;
            ddouble v = b * b - 4d * c;

            if (!(v >= 0d)) {
                return [];
            }

            if (ddouble.IsZero(v)) {
                ddouble t = -0.5d * b;
                Vector3D v1 = line.Origin + t * line.Direction;

                return [v1];
            }
            else {
                ddouble d = ddouble.Sqrt(v);
                ddouble t1 = -0.5 * (b + d), t2 = -0.5 * (b - d);
                Vector3D v1 = line.Origin + t1 * line.Direction;
                Vector3D v2 = line.Origin + t2 * line.Direction;

                return [v1, v2];
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

            if (ddouble.Abs(t) > sphere.Radius) {
                return Circle3D.Invalid;
            }

            Circle3D y = new(
                sphere.Center + plane.Normal * t,
                plane.Normal,
                ddouble.Sqrt(sphere.Radius * sphere.Radius - t * t)
            );

            return y;
        }

        public static Circle3D SphereSphere(Sphere3D sphere1, Sphere3D sphere2) {
            ddouble r0 = sphere1.Radius, r1 = sphere2.Radius;
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

            Circle3D y = new(center, c01, h);

            return y;
        }
    }
}
