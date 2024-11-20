using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Ellipsoid3DTests {
        [TestMethod()]
        public void Ellipsoid3DTest() {
            Ellipsoid3D ellipse = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));

            Assert.AreEqual(new Vector3D(1, 2, 7), ellipse.Center);
            Assert.AreEqual(new Vector3D(2, 3, 4), ellipse.Axis);
            Assert.AreEqual(new Quaternion(4, 3, 5, 6).Normal, ellipse.Rotation);
            
            PrecisionAssert.AreEqual("100.53096491487338363080458826494409229", ellipse.Volume, 1e-29);
            PrecisionAssert.AreEqual("111.54576989401032252593683948373857471", ellipse.Area, 1e-29);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipsoid3D ellipse1 = new(new Vector3D(1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipse2 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipse3 = new((1, 3, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipse4 = new((1, 2, 7), (2, 3, 5), (4, 3, 5, 6));
            Ellipsoid3D ellipse5 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 7));

            Assert.AreEqual(ellipse1, ellipse2);
            Assert.AreNotEqual(ellipse1, ellipse3);

            Assert.IsTrue(ellipse1 == ellipse2);
            Assert.IsTrue(ellipse1 != ellipse3);
            Assert.IsTrue(ellipse1 != ellipse4);
            Assert.IsTrue(ellipse1 != ellipse5);
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
        public void ValidTest() {
            Assert.IsTrue(Ellipsoid3D.IsValid(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.IsFalse(Ellipsoid3D.IsValid(Ellipsoid3D.Invalid));
        }
    }
}