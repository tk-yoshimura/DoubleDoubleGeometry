using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Circle3DTests {
        [TestMethod()]
        public void Circle3DTest() {
            Circle3D circle = new((1, 3, 5), 2, (2, 4, 6));

            Vector3DAssert.AreEqual((1, 3, 5), circle.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, 4, 6).Normal, circle.Normal, 1e-30);
            PrecisionAssert.AreEqual(2.0, circle.Radius, 1e-30);

            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Area, 1e-30);
            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Perimeter, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Circle3D((4, 5, 7), 3, (3, 2, 1)) == new Circle3D((4, 5, 7), 3, (3, 2, 1)));
            Assert.IsTrue(new Circle3D((4, 6, 7), 3, (3, 2, 1)) != new Circle3D((4, 5, 7), 3, (3, 2, 1)));
            Assert.IsTrue(new Circle3D((4, 5, 7), 4, (3, 2, 1)) != new Circle3D((4, 5, 7), 3, (3, 2, 1)));
            Assert.IsTrue(new Circle3D((4, 5, 7), 7, (3, 2, 1)) != new Circle3D((4, 5, 7), 3, (3, 2, 1)));
            Assert.IsTrue(new Circle3D((-4, -5, 7), 3, (3, 2, 1)) != new Circle3D((4, 5, 7), 3, (3, 2, 1)));
        }

        [TestMethod()]
        public void CircumTest() {
            Vector3D v0 = (3, 9, 1), v1 = (4, 2, 1), v2 = (12, 6, 1);

            Circle3D circle1 = Circle3D.FromCircum(new Triangle3D(v0, v1, v2));

            Vector3DAssert.AreEqual((7, 6, 1), circle1.Center, 1e-30);
            Vector3DAssert.AreEqual((0, 0, 1), circle1.Normal, 1e-30);
            PrecisionAssert.AreEqual(5, circle1.Radius, 1e-30);

            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 0.5);

            Circle3D circle2 = Circle3D.FromCircum(new Triangle3D(matrix * v0, matrix * v1, matrix * v2));

            Vector3DAssert.AreEqual(circle2.Center, matrix * new Vector3D(7, 6, 1), 1e-30);
            Vector3DAssert.AreEqual(circle2.Normal, matrix * new Vector3D(0, 0, 1), 1e-30);
            PrecisionAssert.AreEqual(5, circle2.Radius, 1e-30);
        }

        [TestMethod()]
        public void IncircleTest() {
            Vector3D v0 = (2, 1, 1), v1 = (6, 1, 1), v2 = (6, 4, 1);

            Circle3D circle1 = Circle3D.FromIncircle(new Triangle3D(v0, v1, v2));

            Vector3DAssert.AreEqual((5, 2, 1), circle1.Center, 1e-30);
            Vector3DAssert.AreEqual((0, 0, 1), circle1.Normal, 1e-30);
            PrecisionAssert.AreEqual(1, circle1.Radius, 1e-30);

            Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 0.5);

            Circle3D circle2 = Circle3D.FromIncircle(new Triangle3D(matrix * v0, matrix * v1, matrix * v2));

            Vector3DAssert.AreEqual(circle2.Center, matrix * new Vector3D(5, 2, 1), 1e-30);
            Vector3DAssert.AreEqual(circle2.Normal, matrix * new Vector3D(0, 0, 1), 1e-30);
            PrecisionAssert.AreEqual(1, circle2.Radius, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Circle3D((4, 5, 2), 3, (1, 2, 4)), +(new Circle3D((4, 5, 2), 3, (1, 2, 4))));
            Assert.AreEqual(new Circle3D((-4, -5, -2), -3, (1, 2, 4)), -(new Circle3D((4, 5, 2), 3, (1, 2, 4))));
            Assert.AreEqual(new Circle3D((5, 9, 8), 3, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) + (1, 4, 6));
            Assert.AreEqual(new Circle3D((3, 1, -4), 3, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) - (1, 4, 6));
            Assert.AreEqual(new Circle3D((5, 9, 8), 3, (1, 2, 4)), (1, 4, 6) + new Circle3D((4, 5, 2), 3, (1, 2, 4)));
            Assert.AreEqual(new Circle3D((-3, -1, 4), -3, (1, 2, 4)), (1, 4, 6) - new Circle3D((4, 5, 2), 3, (1, 2, 4)));
            Assert.AreEqual(new Circle3D((8, 10, 4), 6, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) * (ddouble)2);
            Assert.AreEqual(new Circle3D((8, 10, 4), 6, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) * (double)2);
            Assert.AreEqual(new Circle3D((8, 10, 4), 6, (1, 2, 4)), (ddouble)2 * new Circle3D((4, 5, 2), 3, (1, 2, 4)));
            Assert.AreEqual(new Circle3D((8, 10, 4), 6, (1, 2, 4)), (double)2 * new Circle3D((4, 5, 2), 3, (1, 2, 4)));
            Assert.AreEqual(new Circle3D((2, 2.5, 1), 1.5, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) / (ddouble)2);
            Assert.AreEqual(new Circle3D((2, 2.5, 1), 1.5, (1, 2, 4)), new Circle3D((4, 5, 2), 3, (1, 2, 4)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Circle3D circle1 = new(Vector3D.Zero, 1, (0, 0, 1));
            Circle3D circle2 = new Circle3D(Vector3D.Zero, 1, (0, 0, 1)) * 2;
            Circle3D circle3 = new Circle3D(Vector3D.Zero, 1, (0, 0, 1)) * -2;
            Circle3D circle4 = new((2, 3, 4), 1, (0, 0, 1));

            Quaternion q = (1, 2, 3, 4);

            Circle3D circle5 = q * new Circle3D((2, 3, 4), 5, (0, 0, 1));

            Vector3DAssert.AreEqual((1, 0, 0), circle1.Point(0), 1e-30);
            Vector3DAssert.AreEqual((ddouble.Sqrt2 / 2, ddouble.Sqrt2 / 2, 0), circle1.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual((0, 1, 0), circle1.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(circle1.Point(0) * 2, circle2.Point(0), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) * 2, circle2.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) * 2, circle2.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(circle1.Point(0) * -2, circle3.Point(0), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) * -2, circle3.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) * -2, circle3.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(circle1.Point(0) + (2, 3, 4), circle4.Point(0), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) + (2, 3, 4), circle4.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) + (2, 3, 4), circle4.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(q * (circle1.Point(0) * 5 + (2, 3, 4)), circle5.Point(0), 1e-29);
            Vector3DAssert.AreEqual(q * (circle1.Point(ddouble.Pi / 4) * 5 + (2, 3, 4)), circle5.Point(ddouble.Pi / 4), 1e-29);
            Vector3DAssert.AreEqual(q * (circle1.Point(ddouble.Pi / 2) * 5 + (2, 3, 4)), circle5.Point(ddouble.Pi / 2), 1e-29);
        }

        [TestMethod()]
        public void BoundingBoxTest() {
            Circle3D circle1 = new((0, 0, 0), 8, Quaternion.One);
            Circle3D circle2 = new((0, 0, 0), 8, (0, 1, 0));
            Circle3D circle3 = new((0, 0, 0), 8, (1, 0, 0));
            Circle3D circle4 = new((0, 0, 0), 8, (3, -6, 4, 7));
            Circle3D circle5 = new((0, 0, 0), 8, (1, -2, 3, 4));
            Circle3D circle6 = new((1, 2, 3), 8, (1, -2, 3, 4));
            Circle3D circle7 = new((0, 0, 0), 8, (3, 4, 1, -2));

            Vector3DAssert.AreEqual((8, 8, 0), circle1.BoundingBox.Scale, 1e-30);
            Vector3DAssert.AreEqual((8, 0, 8), circle2.BoundingBox.Scale, 1e-30);
            Vector3DAssert.AreEqual((0, 8, 8), circle3.BoundingBox.Scale, 1e-30);

            bool any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(circle4.BoundingBox.Inside(circle4.Point(t) * 0.9999));

                if (!circle4.BoundingBox.Inside(circle4.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(circle5.BoundingBox.Inside(circle5.Point(t) * 0.9999));

                if (!circle5.BoundingBox.Inside(circle5.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(circle6.BoundingBox.Inside(circle5.Point(t) * 0.9999 + (1, 2, 3)));

                if (!circle6.BoundingBox.Inside(circle6.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(circle7.BoundingBox.Inside(circle7.Point(t) * 0.9999));

                if (!circle7.BoundingBox.Inside(circle7.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Circle3D.IsValid(new Circle3D((1, 3, 5), 2, (2, 4, 6))));
            Assert.IsFalse(Circle3D.IsValid(Circle3D.Invalid));
        }
    }
}