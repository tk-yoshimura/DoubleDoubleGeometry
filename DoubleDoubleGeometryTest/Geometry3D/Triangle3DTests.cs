using DoubleDouble;
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
        public void ValidTest() {
            Assert.IsTrue(Triangle3D.IsValid(new Triangle3D((8, 1, 1), (2, 3, 1), (4, 9, 1))));
            Assert.IsFalse(Triangle3D.IsValid(Triangle3D.Invalid));
        }
    }
}