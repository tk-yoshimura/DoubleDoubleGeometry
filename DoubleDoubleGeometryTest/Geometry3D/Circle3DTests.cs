using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Circle3DTests {
        [TestMethod()]
        public void Circle3DTest() {
            Circle3D circle = new(new Vector3D(1, 3, 5), new Vector3D(2, 4, 6), 2);

            Vector3DAssert.AreEqual(new Vector3D(1, 3, 5), circle.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, 4, 6).Normal, circle.Normal, 1e-30);
            PrecisionAssert.AreEqual(2.0, circle.Radius, 1e-30);

            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Area, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Circle3D((4, 5, 7), (3, 2, 1), 3) == new Circle3D((4, 5, 7), (3, 2, 1), 3));
            Assert.IsTrue(new Circle3D((4, 6, 7), (3, 2, 1), 3) != new Circle3D((4, 5, 7), (3, 2, 1), 3));
            Assert.IsTrue(new Circle3D((4, 5, 7), (3, 2, 1), 4) != new Circle3D((4, 5, 7), (3, 2, 1), 3));
            Assert.IsTrue(new Circle3D((4, 5, 7), (3, 2, 1), 7) != new Circle3D((4, 5, 7), (3, 2, 1), 3));
            Assert.IsTrue(new Circle3D((-4, -5, 7), (3, 2, 1), 3) != new Circle3D((4, 5, 7), (3, 2, 1), 3));
        }

        [TestMethod()]
        public void CircumTest() {
            Vector3D v0 = new(3, 9, 1), v1 = new(4, 2, 1), v2 = new(12, 6, 1);

            Circle3D circle1 = Circle3D.FromCircum(new Triangle3D(v0, v1, v2));

            Vector3DAssert.AreEqual(new Vector3D(7, 6, 1), circle1.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(0, 0, 1), circle1.Normal, 1e-30);
            PrecisionAssert.AreEqual(5, circle1.Radius, 1e-30);

            Matrix3D matrix = Matrix3D.RotateAxis(new Vector3D(1, 2, 3), 0.5);

            Circle3D circle2 = Circle3D.FromCircum(new Triangle3D(matrix * v0, matrix * v1, matrix * v2));

            Vector3DAssert.AreEqual(circle2.Center, matrix * new Vector3D(7, 6, 1), 1e-30);
            Vector3DAssert.AreEqual(circle2.Normal, matrix * new Vector3D(0, 0, 1), 1e-30);
            PrecisionAssert.AreEqual(5, circle2.Radius, 1e-30);
        }

        [TestMethod()]
        public void IncircleTest() {
            Vector3D v0 = new(2, 1, 1), v1 = new(6, 1, 1), v2 = new(6, 4, 1);

            Circle3D circle1 = Circle3D.FromIncircle(new Triangle3D(v0, v1, v2));

            Vector3DAssert.AreEqual(new Vector3D(5, 2, 1), circle1.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(0, 0, 1), circle1.Normal, 1e-30);
            PrecisionAssert.AreEqual(1, circle1.Radius, 1e-30);

            Matrix3D matrix = Matrix3D.RotateAxis(new Vector3D(1, 2, 3), 0.5);

            Circle3D circle2 = Circle3D.FromIncircle(new Triangle3D(matrix * v0, matrix * v1, matrix * v2));

            Vector3DAssert.AreEqual(circle2.Center, matrix * new Vector3D(5, 2, 1), 1e-30);
            Vector3DAssert.AreEqual(circle2.Normal, matrix * new Vector3D(0, 0, 1), 1e-30);
            PrecisionAssert.AreEqual(1, circle2.Radius, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Circle3D((4, 5, 2), (1, 2, 4), 3), +(new Circle3D((4, 5, 2), (1, 2, 4), 3)));
            Assert.AreEqual(new Circle3D((-4, -5, -2), (-1, -2, -4), 3), -(new Circle3D((4, 5, 2), (1, 2, 4), 3)));
            Assert.AreEqual(new Circle3D((5, 9, 8), (1, 2, 4), 3), new Circle3D((4, 5, 2), (1, 2, 4), 3) + (1, 4, 6));
            Assert.AreEqual(new Circle3D((3, 1, -4), (1, 2, 4), 3), new Circle3D((4, 5, 2), (1, 2, 4), 3) - (1, 4, 6));
            Assert.AreEqual(new Circle3D((5, 9, 8), (1, 2, 4), 3), (1, 4, 6) + new Circle3D((4, 5, 2), (1, 2, 4), 3));
            Assert.AreEqual(new Circle3D((-3, -1, 4), (-1, -2, -4), 3), (1, 4, 6) - new Circle3D((4, 5, 2), (1, 2, 4), 3));
            Assert.AreEqual(new Circle3D((8, 10, 4), (1, 2, 4), 6), new Circle3D((4, 5, 2), (1, 2, 4), 3) * (ddouble)2);
            Assert.AreEqual(new Circle3D((8, 10, 4), (1, 2, 4), 6), new Circle3D((4, 5, 2), (1, 2, 4), 3) * (double)2);
            Assert.AreEqual(new Circle3D((8, 10, 4), (1, 2, 4), 6), (ddouble)2 * new Circle3D((4, 5, 2), (1, 2, 4), 3));
            Assert.AreEqual(new Circle3D((8, 10, 4), (1, 2, 4), 6), (double)2 * new Circle3D((4, 5, 2), (1, 2, 4), 3));
            Assert.AreEqual(new Circle3D((2, 2.5, 1), (1, 2, 4), 1.5), new Circle3D((4, 5, 2), (1, 2, 4), 3) / (ddouble)2);
            Assert.AreEqual(new Circle3D((2, 2.5, 1), (1, 2, 4), 1.5), new Circle3D((4, 5, 2), (1, 2, 4), 3) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Circle3D.IsValid(new Circle3D(new Vector3D(1, 3, 5), new Vector3D(2, 4, 6), 2)));
            Assert.IsFalse(Circle3D.IsValid(Circle3D.Invalid));
        }
    }
}