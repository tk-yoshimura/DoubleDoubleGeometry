using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Plane3DTests {
        [TestMethod()]
        public void Plane3DTest() {
            Plane3D plane1 = Plane3D.FromIntercept(new Vector3D(1, 2, 3), 4);
            Plane3D plane2 = Plane3D.FromIntersection(new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1));

            Vector3D normal1 = new Vector3D(1, 2, 3).Normal;
            Vector3D normal2 = new Vector3D(1, 1, 1).Normal;

            Assert.AreEqual(normal1, plane1.Normal);
            Assert.AreEqual(normal1.X, plane1.A);
            Assert.AreEqual(normal1.Y, plane1.B);
            Assert.AreEqual(normal1.Z, plane1.C);
            Assert.AreEqual(4, plane1.D);

            Assert.AreEqual(normal2, plane2.Normal);
            Assert.AreEqual(-normal2.X, plane2.D);
        }

        [TestMethod()]
        public void MatrixTest() {
            Matrix3D m = new ddouble[,] { { 3, 5, 1 }, { 2, 7, -1 }, { 6, -2, 3 } };
            Vector3D v1 = (1, 2, 3), v2 = (4, 6, -2), v3 = (7, -3, 1), v4 = (5, -2, 4), v5 = (8, 2, 5);

            Plane3D plane123_a = m * Plane3D.FromIntersection(v1, v2, v3);
            Plane3D plane123_b = Plane3D.FromIntersection(m * v1, m * v2, m * v3);

            Plane3D plane124_a = m * Plane3D.FromIntersection(v1, v2, v4);
            Plane3D plane124_b = Plane3D.FromIntersection(m * v1, m * v2, m * v4);

            Plane3D plane125_a = m * Plane3D.FromIntersection(v1, v2, v5);
            Plane3D plane125_b = Plane3D.FromIntersection(m * v1, m * v2, m * v5);

            Plane3D plane134_a = m * Plane3D.FromIntersection(v1, v3, v4);
            Plane3D plane134_b = Plane3D.FromIntersection(m * v1, m * v3, m * v4);

            Plane3D plane345_a = m * Plane3D.FromIntersection(v3, v4, v5);
            Plane3D plane345_b = Plane3D.FromIntersection(m * v3, m * v4, m * v5);

            Vector3DAssert.AreEqual(plane123_a.Normal, plane123_b.Normal, 2e-30);
            PrecisionAssert.AreEqual(plane123_a.D, plane123_b.D, 2e-30);

            Vector3DAssert.AreEqual(plane124_a.Normal, plane124_b.Normal, 2e-30);
            PrecisionAssert.AreEqual(plane124_a.D, plane124_b.D, 2e-30);

            Vector3DAssert.AreEqual(plane125_a.Normal, plane125_b.Normal, 2e-30);
            PrecisionAssert.AreEqual(plane125_a.D, plane125_b.D, 2e-30);

            Vector3DAssert.AreEqual(plane134_a.Normal, plane134_b.Normal, 2e-30);
            PrecisionAssert.AreEqual(plane134_a.D, plane134_b.D, 2e-30);

            Vector3DAssert.AreEqual(plane345_a.Normal, plane345_b.Normal, 2e-30);
            PrecisionAssert.AreEqual(plane345_a.D, plane345_b.D, 2e-30);
        }

        [TestMethod()]
        public void QuaternionTest() {
            Quaternion q = (2, 1, 3, 4);
            Vector3D v1 = (1, 2, 3), v2 = (4, 6, -2), v3 = (7, -3, 1), v4 = (5, -2, 4), v5 = (8, 2, 5);

            Plane3D plane123_a = q * Plane3D.FromIntersection(v1, v2, v3);
            Plane3D plane123_b = Plane3D.FromIntersection(q * v1, q * v2, q * v3);

            Plane3D plane124_a = q * Plane3D.FromIntersection(v1, v2, v4);
            Plane3D plane124_b = Plane3D.FromIntersection(q * v1, q * v2, q * v4);

            Plane3D plane125_a = q * Plane3D.FromIntersection(v1, v2, v5);
            Plane3D plane125_b = Plane3D.FromIntersection(q * v1, q * v2, q * v5);

            Plane3D plane134_a = q * Plane3D.FromIntersection(v1, v3, v4);
            Plane3D plane134_b = Plane3D.FromIntersection(q * v1, q * v3, q * v4);

            Plane3D plane345_a = q * Plane3D.FromIntersection(v3, v4, v5);
            Plane3D plane345_b = Plane3D.FromIntersection(q * v3, q * v4, q * v5);

            Vector3DAssert.AreEqual(plane123_a.Normal, plane123_b.Normal, 2e-29);
            PrecisionAssert.AreEqual(plane123_a.D, plane123_b.D, 2e-29);

            Vector3DAssert.AreEqual(plane124_a.Normal, plane124_b.Normal, 2e-29);
            PrecisionAssert.AreEqual(plane124_a.D, plane124_b.D, 2e-29);

            Vector3DAssert.AreEqual(plane125_a.Normal, plane125_b.Normal, 2e-29);
            PrecisionAssert.AreEqual(plane125_a.D, plane125_b.D, 2e-29);

            Vector3DAssert.AreEqual(plane134_a.Normal, plane134_b.Normal, 2e-29);
            PrecisionAssert.AreEqual(plane134_a.D, plane134_b.D, 2e-29);

            Vector3DAssert.AreEqual(plane345_a.Normal, plane345_b.Normal, 2e-29);
            PrecisionAssert.AreEqual(plane345_a.D, plane345_b.D, 2e-29);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Plane3D.IsValid(Plane3D.FromIntersection(new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1))));
            Assert.IsFalse(Plane3D.IsValid(Plane3D.Invalid));
        }
    }
}