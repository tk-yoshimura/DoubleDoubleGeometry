using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Ellipse3DTests {
        [TestMethod()]
        public void Ellipse3DTest() {
            Ellipse3D ellipse = new((1, 2, 7), (2, 3, 4), (4, 3), 5);

            Assert.AreEqual(new Vector3D(1, 2, 7), ellipse.Center);
            Assert.AreEqual(new Vector3D(2, 3, 4).Normal, ellipse.Normal);
            Assert.AreEqual(4d, ellipse.Axis.major);
            Assert.AreEqual(3d, ellipse.Axis.minor);
            PrecisionAssert.AreEqual(5d - ddouble.Pi, ellipse.Rotation);

            PrecisionAssert.AreEqual(4 * 3 * ddouble.Pi, ellipse.Area);
            PrecisionAssert.AreEqual("22.1034921607095050452855864638724607782783", ellipse.Perimeter, 1e-30);
            PrecisionAssert.AreEqual("2.64575131106459059050161575363926042571026", ellipse.Focus, 1e-30);
            PrecisionAssert.AreEqual("0.661437827766147647625403938409815106427565", ellipse.Eccentricity, 1e-30);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipse3D ellipse1 = new(new Vector3D(1, 2, 7), (2, 3, 4), (4, 3), 5);
            Ellipse3D ellipse2 = new((1, 2, 7), (2, 3, 4), (4, 3), 5);
            Ellipse3D ellipse3 = new((1, 2, 7), (2, 3, 4), (4, 4), 5);
            Ellipse3D ellipse4 = new((1, 2, 7), (2, 3, 5), (4, 4), 5);

            Assert.AreEqual(ellipse1, ellipse2);
            Assert.AreNotEqual(ellipse1, ellipse3);

            Assert.IsTrue(ellipse1 == ellipse2);
            Assert.IsTrue(ellipse1 != ellipse3);
            Assert.IsTrue(ellipse1 != ellipse4);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5), +(new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5)));
            Assert.AreEqual(new Ellipse3D((-1, -2, -7), (2, 3, 4), (4, 3), 5), -(new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5)));
            Assert.AreEqual(new Ellipse3D((2, 6, 12), (2, 3, 4), (4, 3), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) + (1, 4, 5));
            Assert.AreEqual(new Ellipse3D((0, -2, 2), (2, 3, 4), (4, 3), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) - (1, 4, 5));
            Assert.AreEqual(new Ellipse3D((2, 6, 12), (2, 3, 4), (4, 3), 5), (1, 4, 5) + new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5));
            Assert.AreEqual(new Ellipse3D((0, 2, -2), (2, 3, 4), (4, 3), 5), (1, 4, 5) - new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5));
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (2, 3, 4), (8, 6), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) * (ddouble)2);
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (2, 3, 4), (8, 6), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) * (double)2);
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (2, 3, 4), (8, 6), 5), (ddouble)2 * new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5));
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (2, 3, 4), (8, 6), 5), (double)2 * new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5));
            Assert.AreEqual(new Ellipse3D((0.5, 1, 3.5), (2, 3, 4), (2, 1.5), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) / (ddouble)2);
            Assert.AreEqual(new Ellipse3D((0.5, 1, 3.5), (2, 3, 4), (2, 1.5), 5), new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Ellipse3D.IsValid(new Ellipse3D((1, 2, 7), (2, 3, 4), (4, 3), 5)));
            Assert.IsFalse(Ellipse3D.IsValid(Ellipse3D.Invalid));
        }
    }
}