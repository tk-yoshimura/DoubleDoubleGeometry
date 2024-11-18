using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Ellipse2DTests {
        [TestMethod()]
        public void Ellipse2DTest() {
            Ellipse2D ellipse = new((1, 2), (4, 3), 5);

            Assert.AreEqual(new Vector2D(1, 2), ellipse.Center);
            Assert.AreEqual(4d, ellipse.Axis.major);
            Assert.AreEqual(3d, ellipse.Axis.minor);
            PrecisionAssert.AreEqual(5d - ddouble.Pi, ellipse.Angle);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipse2D ellipse1 = new(new Vector2D(1, 2), (4, 3), 5);
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
            PrecisionAssert.AreEqual(4.52471777797, ellipse.Axis.major, 1e-10);
            PrecisionAssert.AreEqual(2.24080472704, ellipse.Axis.minor, 1e-10);
            PrecisionAssert.AreEqual(1.96349540849, ellipse.Angle, 1e-10);
        }

        [TestMethod()]
        public void FromImplicitTest2() {
            const double a = 5, b = 0, c = 5, d = 7, e = 11, f = -13;

            Ellipse2D ellipse = Ellipse2D.FromImplicit(a, b, c, d, e, f);

            PrecisionAssert.AreEqual(-0.7, ellipse.Center.X, 1e-10);
            PrecisionAssert.AreEqual(-1.1, ellipse.Center.Y, 1e-10);
            PrecisionAssert.AreEqual(2.07364413533, ellipse.Axis.major, 1e-10);
            PrecisionAssert.AreEqual(2.07364413533, ellipse.Axis.minor, 1e-10);
            PrecisionAssert.AreEqual(0.0, ellipse.Angle, 1e-10);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Ellipse2D.IsValid(new Ellipse2D(new Vector2D(1, 2), (4, 3), 5)));
            Assert.IsFalse(Ellipse2D.IsValid(Ellipse2D.Invalid));
        }
    }
}