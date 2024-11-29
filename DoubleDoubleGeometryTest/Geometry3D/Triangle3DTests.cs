using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Triangle3DTests {
        [TestMethod()]
        public void Triangle3DTest() {
            Triangle3D triangle1 = new((8, 1, 1), (2, 3, 1), (4, 9, 1));
            Triangle3D triangle2 = Matrix3D.Scale(1, 2, 3) * triangle1 + (2, 4, 6);

            PrecisionAssert.AreEqual((2 * 2 + 6 * 6) / 2, (Matrix3D.Rotate(1, 1, 1) * triangle1).Area, 1e-30);
            PrecisionAssert.AreEqual(2 * 2 + 6 * 6, (Matrix3D.Rotate(1, 2, 3) * triangle2).Area, 2e-30);

            Vector3DAssert.AreEqual((8 * 1 + 2, 1 * 2 + 4, 1 * 3 + 6), triangle2.V0, 1e-30);
            Vector3DAssert.AreEqual((2 * 1 + 2, 3 * 2 + 4, 1 * 3 + 6), triangle2.V1, 1e-30);
            Vector3DAssert.AreEqual((4 * 1 + 2, 9 * 2 + 4, 1 * 3 + 6), triangle2.V2, 1e-30);

            Polygon3D p1 = triangle1;
            Polygon3D p2 = triangle2;

            Vector3DAssert.AreEqual(triangle1.V0, p1.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(triangle1.V1, p1.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(triangle1.V2, p1.Vertex[2], 1e-30);

            Vector3DAssert.AreEqual(triangle2.V0, p2.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(triangle2.V1, p2.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(triangle2.V2, p2.Vertex[2], 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 7, 2)) == new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 7, 2)));
            Assert.IsTrue(new Triangle3D((6, 1, 3), (-1, 3, 4), (5, 7, 2)) != new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 7, 2)));
            Assert.IsTrue(new Triangle3D((6, 2, 3), (-1, 2, 4), (5, 7, 2)) != new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 7, 2)));
            Assert.IsTrue(new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 8, 2)) != new Triangle3D((6, 1, 3), (-1, 2, 4), (5, 7, 2)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)), +(new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2))));
            Assert.AreEqual(new Triangle3D((-4, -5, -3), (-1, -2, -7), (-5, -1, -2)), -(new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2))));
            Assert.AreEqual(new Triangle3D((5, 9, 8), (2, 6, 12), (6, 5, 7)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) + (1, 4, 5));
            Assert.AreEqual(new Triangle3D((3, 1, -2), (0, -2, 2), (4, -3, -3)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) - (1, 4, 5));
            Assert.AreEqual(new Triangle3D((5, 9, 8), (2, 6, 12), (6, 5, 7)), (1, 4, 5) + new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)));
            Assert.AreEqual(new Triangle3D((-3, -1, 2), (0, 2, -2), (-4, 3, 3)), (1, 4, 5) - new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)));
            Assert.AreEqual(new Triangle3D((8, 10, 6), (2, 4, 14), (10, 2, 4)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) * (ddouble)2);
            Assert.AreEqual(new Triangle3D((8, 10, 6), (2, 4, 14), (10, 2, 4)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) * (double)2);
            Assert.AreEqual(new Triangle3D((8, 10, 6), (2, 4, 14), (10, 2, 4)), (ddouble)2 * new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)));
            Assert.AreEqual(new Triangle3D((8, 10, 6), (2, 4, 14), (10, 2, 4)), (double)2 * new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)));
            Assert.AreEqual(new Triangle3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) / (ddouble)2);
            Assert.AreEqual(new Triangle3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1)), new Triangle3D((4, 5, 3), (1, 2, 7), (5, 1, 2)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Triangle3D triangle1 = new(Vector3D.Zero, (1, 1, 1), (2, 0, 2));
            Triangle3D triangle2 = new Triangle3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2)) * 2;
            Triangle3D triangle3 = new Triangle3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2)) * -2;
            Triangle3D triangle4 = new Triangle3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2)) + (2, 3, 4);

            Quaternion q = new Quaternion(2, 5, 3, 4).Normal;
            Matrix3D m = new(q);

            Triangle3D triangle5 = q * triangle4;
            Triangle3D triangle6 = m * triangle4;

            Vector3DAssert.AreEqual((0, 0, 0), triangle1.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), triangle1.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual((1, 0.5, 1), triangle1.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), triangle1.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual((2, 0, 2), triangle1.Point(0, 1), 1e-30);

            Vector3DAssert.AreEqual(triangle1.Point(0, 0) * 2, triangle2.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 0) * 2, triangle2.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0.5, 0.5) * 2, triangle2.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 1) * 2, triangle2.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0, 1) * 2, triangle2.Point(0, 1), 1e-30);

            Vector3DAssert.AreEqual(triangle1.Point(0, 0) * -2, triangle3.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 0) * -2, triangle3.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0.5, 0.5) * -2, triangle3.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 1) * -2, triangle3.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0, 1) * -2, triangle3.Point(0, 1), 1e-30);

            Vector3DAssert.AreEqual(triangle1.Point(0, 0) + (2, 3, 4), triangle4.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 0) + (2, 3, 4), triangle4.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0.5, 0.5) + (2, 3, 4), triangle4.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(1, 1) + (2, 3, 4), triangle4.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual(triangle1.Point(0, 1) + (2, 3, 4), triangle4.Point(0, 1), 1e-30);

            Vector3DAssert.AreEqual(q * triangle4.Point(0, 0), triangle5.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(q * triangle4.Point(1, 0), triangle5.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual(q * triangle4.Point(0.5, 0.5), triangle5.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(q * triangle4.Point(1, 1), triangle5.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual(q * triangle4.Point(0, 1), triangle5.Point(0, 1), 1e-30);

            Vector3DAssert.AreEqual(m * triangle4.Point(0, 0), triangle6.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(m * triangle4.Point(1, 0), triangle6.Point(1, 0), 1e-30);
            Vector3DAssert.AreEqual(m * triangle4.Point(0.5, 0.5), triangle6.Point(0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(m * triangle4.Point(1, 1), triangle6.Point(1, 1), 1e-30);
            Vector3DAssert.AreEqual(m * triangle4.Point(0, 1), triangle6.Point(0, 1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Triangle3D.IsValid(new Triangle3D((8, 1, 1), (2, 3, 1), (4, 9, 1))));
            Assert.IsFalse(Triangle3D.IsValid(Triangle3D.Invalid));
        }
    }
}