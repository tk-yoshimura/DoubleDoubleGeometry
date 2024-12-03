using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Circle2DTests {
        [TestMethod()]
        public void Circle2DTest() {
            Circle2D circle = new((1, 3), 2);

            Vector2DAssert.AreEqual((1, 3), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(2.0, circle.Radius, 1e-30);

            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Area, 1e-30);
            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Perimeter, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Circle2D((4, 5), 3) == new Circle2D((4, 5), 3));
            Assert.IsTrue(new Circle2D((4, 6), 3) != new Circle2D((4, 5), 3));
            Assert.IsTrue(new Circle2D((4, 5), 4) != new Circle2D((4, 5), 3));
            Assert.IsTrue(new Circle2D((-4, -5), 3) != new Circle2D((4, 5), 3));
        }

        [TestMethod()]
        public void CircumTest() {
            Vector2D v0 = (3, 9), v1 = (4, 2), v2 = (12, 6);

            Circle2D circle = Circle2D.FromCircum(new Triangle2D(v0, v1, v2));

            Vector2DAssert.AreEqual((7, 6), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(5, circle.Radius, 1e-30);
        }

        [TestMethod()]
        public void IncircleTest() {
            Vector2D v0 = (2, 1), v1 = (6, 1), v2 = (6, 4);

            Circle2D circle = Circle2D.FromIncircle(new Triangle2D(v0, v1, v2));

            Vector2DAssert.AreEqual((5, 2), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(1, circle.Radius, 1e-30);
        }

        [TestMethod()]
        public void FromImplicitTest() {
            Circle2D circle = Circle2D.FromImplicit(-4, 6, -3);

            Vector2DAssert.AreEqual((2, -3), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(4, circle.Radius, 1e-30);
            PrecisionAssert.AreEqual(-4, circle.A, 1e-30);
            PrecisionAssert.AreEqual(6, circle.B, 1e-30);
            PrecisionAssert.AreEqual(-3, circle.C, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Circle2D((4, 5), 3), +(new Circle2D((4, 5), 3)));
            Assert.AreEqual(new Circle2D((-4, -5), -3), -(new Circle2D((4, 5), 3)));
            Assert.AreEqual(new Circle2D((5, 9), 3), new Circle2D((4, 5), 3) + (1, 4));
            Assert.AreEqual(new Circle2D((3, 1), 3), new Circle2D((4, 5), 3) - (1, 4));
            Assert.AreEqual(new Circle2D((5, 9), 3), (1, 4) + new Circle2D((4, 5), 3));
            Assert.AreEqual(new Circle2D((-3, -1), -3), (1, 4) - new Circle2D((4, 5), 3));
            Assert.AreEqual(new Circle2D((8, 10), 6), new Circle2D((4, 5), 3) * (ddouble)2);
            Assert.AreEqual(new Circle2D((8, 10), 6), new Circle2D((4, 5), 3) * (double)2);
            Assert.AreEqual(new Circle2D((8, 10), 6), (ddouble)2 * new Circle2D((4, 5), 3));
            Assert.AreEqual(new Circle2D((8, 10), 6), (double)2 * new Circle2D((4, 5), 3));
            Assert.AreEqual(new Circle2D((2, 2.5), 1.5), new Circle2D((4, 5), 3) / (ddouble)2);
            Assert.AreEqual(new Circle2D((2, 2.5), 1.5), new Circle2D((4, 5), 3) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Circle2D circle1 = new(Vector2D.Zero, 1);
            Circle2D circle2 = new Circle2D(Vector2D.Zero, 1) * 2;
            Circle2D circle3 = new Circle2D(Vector2D.Zero, 1) * -2;
            Circle2D circle4 = new((2, 3), 1.25);

            Complex c = (3, 4);

            Circle2D circle5 = c * circle4;
            Circle2D circle6 = Circle2D.FromIntersection(c * circle4.Point(0), c * circle4.Point(1), c * circle4.Point(2));

            Vector2DAssert.AreEqual((1, 0), circle1.Point(0), 1e-30);
            Vector2DAssert.AreEqual((ddouble.Sqrt2 / 2, ddouble.Sqrt2 / 2), circle1.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual((0, 1), circle1.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(circle1.Point(0) * 2, circle2.Point(0), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) * 2, circle2.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) * 2, circle2.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(circle1.Point(0) * -2, circle3.Point(0), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) * -2, circle3.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) * -2, circle3.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(circle1.Point(0) * 1.25 + (2, 3), circle4.Point(0), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 4) * 1.25 + (2, 3), circle4.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(circle1.Point(ddouble.Pi / 2) * 1.25 + (2, 3), circle4.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(circle6.Center, circle5.Center, 1e-29);
            PrecisionAssert.AreEqual(circle6.Radius, circle5.Radius, 1e-29);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Circle2D.IsValid(new Circle2D((1, 3), 2)));
            Assert.IsFalse(Circle2D.IsValid(Circle2D.Invalid));
        }
    }
}