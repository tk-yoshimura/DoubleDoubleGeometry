using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Ellipsoid3DTests {
        [TestMethod()]
        public void Ellipsoid3DTest() {
            Ellipsoid3D ellipsoid = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));

            Vector3DAssert.AreEqual((1, 2, 7), ellipsoid.Center, 1e-30);
            Vector3DAssert.AreEqual((2, 3, 4), ellipsoid.Axis, 1e-30);
            Assert.IsTrue((new Quaternion(4, 3, 5, 6).Normal - ellipsoid.Rotation).Norm < 1e-30);

            PrecisionAssert.AreEqual("100.53096491487338363080458826494409229", ellipsoid.Volume, 1e-29);
            PrecisionAssert.AreEqual("111.54576989401032252593683948373857471", ellipsoid.Area, 1e-29);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipsoid3D ellipsoid1 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid2 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid3 = new((1, 3, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid4 = new((1, 2, 7), (2, 3, 5), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid5 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 7));

            Assert.AreEqual(ellipsoid1, ellipsoid2);
            Assert.AreNotEqual(ellipsoid1, ellipsoid3);

            Assert.IsTrue(ellipsoid1 == ellipsoid2);
            Assert.IsTrue(ellipsoid1 != ellipsoid3);
            Assert.IsTrue(ellipsoid1 != ellipsoid4);
            Assert.IsTrue(ellipsoid1 != ellipsoid5);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)), +(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Ellipsoid3D((-1, -2, -7), (2, 3, 4), (4, 3, 5, 6)), -(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Ellipsoid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) + (1, 4, 5));
            Assert.AreEqual(new Ellipsoid3D((0, -2, 2), (2, 3, 4), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) - (1, 4, 5));
            Assert.AreEqual(new Ellipsoid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) + new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((0, 2, -2), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) - new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (ddouble)2);
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (double)2);
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (ddouble)2 * new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (double)2 * new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (ddouble)2);
            Assert.AreEqual(new Ellipsoid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Ellipsoid3D ellipsoid1 = new(Vector3D.Zero, (4, 3, 5), Quaternion.One);
            Ellipsoid3D ellipsoid2 = new Ellipsoid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * 2;
            Ellipsoid3D ellipsoid3 = new Ellipsoid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * -2;
            Ellipsoid3D ellipsoid4 = new((2, 3, 4), (4, 3, 5), Quaternion.One);

            Quaternion q = (1, 2, 3, 4);

            Ellipsoid3D ellipsoid5 = q * new Ellipsoid3D((2, 3, 4), (20, 15, 25), Quaternion.One);

            Vector3DAssert.AreEqual((4, 0, 0), ellipsoid1.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual((ddouble.Sqrt2 * 2, ddouble.Sqrt2 * 3 / 2, 0), ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual((0, 3, 0), ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);
            Vector3DAssert.AreEqual((0, 0, 5), ellipsoid1.Point(0, 0), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) * 2, ellipsoid2.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) * 2, ellipsoid2.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * 2, ellipsoid2.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * 2, ellipsoid2.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) * -2, ellipsoid3.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) * -2, ellipsoid3.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * -2, ellipsoid3.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * -2, ellipsoid3.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) + (2, 3, 4), ellipsoid4.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(0, 0) * 5 + (2, 3, 4)), ellipsoid5.Point(0, 0), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, 0) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, 0), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, ddouble.Pi / 4), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, ddouble.Pi / 2), 2e-29);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Ellipsoid3D.IsValid(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.IsFalse(Ellipsoid3D.IsValid(Ellipsoid3D.Invalid));
        }
    }
}