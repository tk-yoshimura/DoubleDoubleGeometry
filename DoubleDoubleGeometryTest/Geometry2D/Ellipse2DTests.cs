using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Ellipse2DTests {
        [TestMethod()]
        public void Ellipse2DTest() {
            Ellipse2D ellipse = new((1, 2), (4, 3), 5);

            Assert.AreEqual((1, 2), ellipse.Center);
            Assert.AreEqual((4d, 3d), ellipse.Axis);
            PrecisionAssert.AreEqual(5d - ddouble.Pi * 2, ellipse.Angle, 1e-30);

            PrecisionAssert.AreEqual(4 * 3 * ddouble.Pi, ellipse.Area, 1e-30);
            PrecisionAssert.AreEqual("22.1034921607095050452855864638724607782783", ellipse.Perimeter, 1e-30);
            PrecisionAssert.AreEqual("2.64575131106459059050161575363926042571026", ellipse.Focus, 1e-30);
            PrecisionAssert.AreEqual("0.661437827766147647625403938409815106427565", ellipse.Eccentricity, 1e-30);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipse2D ellipse1 = new((1, 2), (4, 3), 5);
            Ellipse2D ellipse2 = new((1, 2), (4, 3), 5);
            Ellipse2D ellipse3 = new((1, 2), (4, 4), 5);

            Assert.AreEqual(ellipse1, ellipse2);
            Assert.AreNotEqual(ellipse1, ellipse3);

            Assert.IsTrue(ellipse1 == ellipse2);
            Assert.IsTrue(ellipse1 != ellipse3);
        }

        [TestMethod()]
        public void FromImplicitTest1() {
            const double a = 5, b = 3, c = 2, d = 7, e = 11, f = -13;

            Ellipse2D ellipse = Ellipse2D.FromImplicit(a, b, c, d, e, f);

            PrecisionAssert.AreEqual(+0.1612903226, ellipse.Center.X, 1e-10);
            PrecisionAssert.AreEqual(-2.8709677419, ellipse.Center.Y, 1e-10);
            PrecisionAssert.AreEqual(4.52471777797, ellipse.Axis.X, 1e-10);
            PrecisionAssert.AreEqual(2.24080472704, ellipse.Axis.Y, 1e-10);
            PrecisionAssert.AreEqual(1.96349540849, ellipse.Angle, 1e-10);
        }

        [TestMethod()]
        public void FromImplicitTest2() {
            const double a = 5, b = 0, c = 5, d = 7, e = 11, f = -13;

            Ellipse2D ellipse = Ellipse2D.FromImplicit(a, b, c, d, e, f);

            PrecisionAssert.AreEqual(-0.7, ellipse.Center.X, 1e-10);
            PrecisionAssert.AreEqual(-1.1, ellipse.Center.Y, 1e-10);
            PrecisionAssert.AreEqual(2.07364413533, ellipse.Axis.X, 1e-10);
            PrecisionAssert.AreEqual(2.07364413533, ellipse.Axis.Y, 1e-10);
            PrecisionAssert.AreEqual(0.0, ellipse.Angle, 1e-10);
        }

        [TestMethod()]
        public void InsideTest() {
            Ellipse2D t = new((0, 0), (3, 3), Complex.One);

            Vector2D[] insides = [
                (0.25, 0.25), (0.25, 0.5), (0.5, 0.25), (1, 2), (0, 2.5), (-1, -2), (2, -1)
            ];

            Vector2D[] outsides = [
                (0, 4), (2, 3), (-2, -3), (-4, 4), (0, -4), (2, -3),
            ];

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Complex c = (1, 2);
            Vector2D s = (4, 6);

            Ellipse2D t2 = c * t + s;

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t2.Inside(c * v + s));
            }

            Assert.IsTrue(t2.Inside(insides.Select(v => c * v + s)).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t2.Inside(c * v + s));
            }

            Assert.IsTrue(t2.Inside(outsides.Select(v => c * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Ellipse2D((4, 5), (1, 2), 3), +(new Ellipse2D((4, 5), (1, 2), 3)));
            Assert.AreEqual(new Ellipse2D((-4, -5), (-1, -2), 3), -(new Ellipse2D((4, 5), (1, 2), 3)));
            Assert.AreEqual(new Ellipse2D((5, 9), (1, 2), 3), new Ellipse2D((4, 5), (1, 2), 3) + (1, 4));
            Assert.AreEqual(new Ellipse2D((3, 1), (1, 2), 3), new Ellipse2D((4, 5), (1, 2), 3) - (1, 4));
            Assert.AreEqual(new Ellipse2D((5, 9), (1, 2), 3), (1, 4) + new Ellipse2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Ellipse2D((-3, -1), (-1, -2), 3), (1, 4) - new Ellipse2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Ellipse2D((8, 10), (2, 4), 3), new Ellipse2D((4, 5), (1, 2), 3) * (ddouble)2);
            Assert.AreEqual(new Ellipse2D((8, 10), (2, 4), 3), new Ellipse2D((4, 5), (1, 2), 3) * (double)2);
            Assert.AreEqual(new Ellipse2D((8, 10), (2, 4), 3), (ddouble)2 * new Ellipse2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Ellipse2D((8, 10), (2, 4), 3), (double)2 * new Ellipse2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Ellipse2D((2, 2.5), (0.5, 1), 3), new Ellipse2D((4, 5), (1, 2), 3) / (ddouble)2);
            Assert.AreEqual(new Ellipse2D((2, 2.5), (0.5, 1), 3), new Ellipse2D((4, 5), (1, 2), 3) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Ellipse2D ellipse1 = new(Vector2D.Zero, (3, 2), 0);
            Ellipse2D ellipse2 = new Ellipse2D(Vector2D.Zero, (3, 2), 0) * 2;
            Ellipse2D ellipse3 = new Ellipse2D(Vector2D.Zero, (3, 2), 0) * -2;
            Ellipse2D ellipse4 = new((2, 3), (3, 2), 0);
            Ellipse2D ellipse5 = new((2, 3), (3, 2), ddouble.Pi / 2);

            Complex c = (3, 4);

            Ellipse2D ellipse6 = c * ellipse5;

            Vector2DAssert.AreEqual((3, 0), ellipse1.Point(0), 1e-30);
            Vector2DAssert.AreEqual((3 * ddouble.Sqrt2 / 2, ddouble.Sqrt2), ellipse1.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual((0, 2), ellipse1.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(ellipse1.Point(0) * 2, ellipse2.Point(0), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) * 2, ellipse2.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) * 2, ellipse2.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(ellipse1.Point(0) * -2, ellipse3.Point(0), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) * -2, ellipse3.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) * -2, ellipse3.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(ellipse1.Point(0) + (2, 3), ellipse4.Point(0), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) + (2, 3), ellipse4.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) + (2, 3), ellipse4.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(c * ellipse5.Point(0), ellipse6.Point(0), 1e-30);
            Vector2DAssert.AreEqual(c * ellipse5.Point(ddouble.Pi / 4), ellipse6.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(c * ellipse5.Point(ddouble.Pi / 2), ellipse6.Point(ddouble.Pi / 2), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Ellipse2D.IsValid(new Ellipse2D((1, 2), (4, 3), 5)));
            Assert.IsFalse(Ellipse2D.IsValid(Ellipse2D.Invalid));
        }
    }
}