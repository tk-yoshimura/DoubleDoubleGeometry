using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Intersect3DTests {
        [TestMethod()]
        public void LineLineTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4) * Matrix3D.Scale(1, 2, 3);

            Line3D line1 = Line3D.FromDirection((1, 3, 1), (3, 2, 0));
            Line3D line2 = Line3D.FromDirection((6, 1, 1), (-1, 2, 0));

            Vector3D cross = Intersect3D.LineLine(matrix * line1, matrix * line2, 1e-28);

            Vector3DAssert.AreEqual(matrix * new Vector3D(4, 5, 1), cross, 1e-30);
        }

        [TestMethod()]
        public void LinePlaneTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4) * Matrix3D.Scale(1, 2, 3);

            Vector3D v1 = new(1, 0, 0), v2 = new(0, 2, 0), v3 = new(0, 0, 3);

            Plane3D plane = Plane3D.FromIntersection(matrix * v1, matrix * v2, matrix * v3);

            Line3D line1 = matrix * Line3D.FromDirection(Vector3D.Zero, v1);
            (Vector3D v, ddouble t) cross1 = Intersect3D.LinePlane(line1, plane);

            Vector3DAssert.AreEqual(matrix * v1, cross1.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v1, line1.Point(cross1.t), 1e-30);

            Line3D line2 = matrix * Line3D.FromDirection(Vector3D.Zero, -v1);
            (Vector3D v, ddouble t) cross2 = Intersect3D.LinePlane(line2, plane);

            Vector3DAssert.AreEqual(matrix * v1, cross2.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v1, line2.Point(cross2.t), 1e-30);
        }

        [TestMethod()]
        public void LineTriangleTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4) * Matrix3D.Scale(1, 2, 3);

            Vector3D v1 = new(1, 0, 0), v2 = new(0, 1, 0), v3 = new(0, 0, 1);

            Vector3D v4 = new Vector3D(1, 1, 1) / 3, v5 = new(1, -0.1, -0.1), v6 = new(-0.1, 1, -0.1), v7 = new(-0.1, -0.1, 1);
            Vector3D v8 = new(1, -0.1, 0.1), v9 = new(1, 0.1, -0.1), v10 = new(-0.1, -1, 0.1), v11 = new(-0.1, 0.1, -1);

            Triangle3D triangle = new(matrix * v1, matrix * v2, matrix * v3);

            Line3D line1 = matrix * Line3D.FromDirection(Vector3D.Zero, v4);
            (Vector3D v, ddouble t) cross1 = Intersect3D.LineTriangle(line1, triangle);

            Vector3DAssert.AreEqual(matrix * v4, cross1.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v4, line1.Point(cross1.t), 1e-30);

            Line3D line2 = matrix * Line3D.FromDirection(Vector3D.Zero, -v4);
            (Vector3D v, ddouble t) cross2 = Intersect3D.LineTriangle(line2, triangle);

            Vector3DAssert.AreEqual(matrix * v4, cross2.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v4, line2.Point(cross2.t), 1e-30);

            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v5), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v6), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v7), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v8), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v9), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v10), triangle).v));
            Assert.IsFalse(Vector3D.IsValid(Intersect3D.LineTriangle(matrix * Line3D.FromDirection(Vector3D.Zero, v11), triangle).v));
        }

        [TestMethod()]
        public void LineCircleTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4) * Matrix3D.Scale(1, 2, 3);

            Vector3D v1 = new(1, 0, 0), v2 = new(0, 1, 0), v3 = new(0, 0, 1);
            Vector3D v4 = new Vector3D(1, 2, 3) / 6, v5 = new(1, -0.1, -0.1);

            Circle3D circle = Circle3D.FromCircum(new Triangle3D(matrix * v1, matrix * v2, matrix * v3));

            Line3D line1 = matrix * Line3D.FromDirection(Vector3D.Zero, v4);
            (Vector3D v, ddouble t) cross1 = Intersect3D.LineCircle(line1, circle);

            Vector3DAssert.AreEqual(matrix * v4, cross1.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v4, line1.Point(cross1.t), 1e-30);

            Line3D line2 = matrix * Line3D.FromDirection(Vector3D.Zero, -v4);
            (Vector3D v, ddouble t) cross2 = Intersect3D.LineCircle(line2, circle);

            Vector3DAssert.AreEqual(matrix * v4, cross2.v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v4, line2.Point(cross2.t), 1e-30);

            Line3D line3 = matrix * Line3D.FromDirection(Vector3D.Zero, v5);
            (Vector3D v, ddouble t) cross3 = Intersect3D.LineCircle(line3, circle);

            Assert.IsFalse(Vector3D.IsValid(cross3.v));
        }

        [TestMethod()]
        public void LineSphereTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4);

            Vector3D v0 = new(3, 4, 0), v1 = new(0, 3, -4);

            Line3D line = matrix * Line3D.FromIntersection(v0, v1);

            Sphere3D sphere = new(matrix * Vector3D.Zero, 5);

            (Vector3D v, ddouble t)[] cross = Intersect3D.LineSphere(line, sphere);

            Vector3DAssert.AreEqual(matrix * v0, cross[0].v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v0, line.Point(cross[0].t), 1e-30);
            Vector3DAssert.AreEqual(matrix * v1, cross[1].v, 1e-30);
            Vector3DAssert.AreEqual(matrix * v1, line.Point(cross[1].t), 1e-30);
        }

        [TestMethod()]
        public void PlanePlaneTest() {
            Vector3D v1 = (-1, 2, 4), v2 = (1, 2, 4), v3 = (1, -2, -1), v4 = (1, 2, 3);

            Plane3D plane_xy = Plane3D.FromNormal(Vector3D.Zero, (0, 0, 1));
            Plane3D plane_yz = Plane3D.FromNormal(Vector3D.Zero, (1, 0, 0));
            Plane3D plane_zx = Plane3D.FromNormal(Vector3D.Zero, (0, 1, 0));

            Plane3D plane1 = Plane3D.FromIntersection(v1, v2, v3);
            Plane3D plane2 = Plane3D.FromIntersection(v1, v2, v4);

            Vector3DAssert.AreEqual((0, 1, 0), Intersect3D.PlanePlane(plane_xy, plane_yz).Direction, 1e-30);
            Vector3DAssert.AreEqual((0, 0, 1), Intersect3D.PlanePlane(plane_yz, plane_zx).Direction, 1e-30);
            Vector3DAssert.AreEqual((1, 0, 0), Intersect3D.PlanePlane(plane_zx, plane_xy).Direction, 1e-30);

            Vector3DAssert.AreEqual((1, 0, 0), Intersect3D.PlanePlane(plane1, plane2).Direction, 1e-30);
        }

        [TestMethod()]
        public void PlaneSphereTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4);

            Vector3D v0 = matrix * Vector3D.Zero, v1 = matrix * new Vector3D(1, 0, 0), v2 = matrix * new Vector3D(0, 1, 0), v3 = matrix * new Vector3D(0, 0, 1);

            Plane3D plane = Plane3D.FromIntersection(v1, v2, v3);
            Circle3D circle = Circle3D.FromCircum(new Triangle3D(v1, v2, v3));
            Sphere3D sphere = new(v0, 1);

            Circle3D cross = Intersect3D.PlaneSphere(plane, sphere);

            Vector3DAssert.AreEqual(circle.Center, cross.Center, 1e-30);
            Vector3DAssert.AreEqual(circle.Normal, cross.Normal, 1e-30);
            PrecisionAssert.AreEqual(circle.Radius, cross.Radius, 1e-30);
        }

        [TestMethod()]
        public void SphereSphereTest() {
            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4);

            Vector3D v0 = matrix * new Vector3D(-2, 0, 0), v1 = matrix * new Vector3D(3, 0, 0), v2 = matrix * new Vector3D("-0.2", 0, 0);

            Sphere3D sphere1 = new(v0, 3);
            Sphere3D sphere2 = new(v1, 4);
            Circle3D circle = new(v2, "2.4", matrix * new Vector3D(1, 0, 0));

            Circle3D cross = Intersect3D.SphereSphere(sphere1, sphere2);

            Vector3DAssert.AreEqual(circle.Center, cross.Center, 1e-30);
            Vector3DAssert.AreEqual(circle.Normal, cross.Normal, 1e-30);
            PrecisionAssert.AreEqual(circle.Radius, cross.Radius, 1e-30);
        }
    }
}