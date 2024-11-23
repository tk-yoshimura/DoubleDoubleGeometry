using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Sphere3DTests {
        [TestMethod()]
        public void Sphere3DTest() {
            Sphere3D sphere1 = new((1, 2, 3), 4);
            Sphere3D sphere2 = Sphere3D.FromIntersection((3, 2, -1), (1, 3, -2), (3, -1, -4), (0, 0, -2));

            Vector3DAssert.AreEqual((1, 2, 3), sphere1.Center, 1e-30);
            PrecisionAssert.AreEqual(4.0, sphere1.Radius, 1e-30);
            PrecisionAssert.AreEqual(4 * 4 * 4 * ddouble.Pi, sphere1.Area, 1e-30);
            PrecisionAssert.AreEqual(4.0 * 4 * 4 * 4 * ddouble.Pi / 3.0, sphere1.Volume, 1e-30);

            Vector3DAssert.AreEqual((2, 1, -3), sphere2.Center, 1e-30);
            PrecisionAssert.AreEqual(6, sphere2.Radius * sphere2.Radius, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Sphere3D((4, 5, 7), 3) == new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((4, 6, 7), 3) != new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((4, 5, 7), 4) != new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((-4, -5, 7), 3) != new Sphere3D((4, 5, 7), 3));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Sphere3D((4, 5, 1), 3), +(new Sphere3D((4, 5, 1), 3)));
            Assert.AreEqual(new Sphere3D((-4, -5, -1), 3), -(new Sphere3D((4, 5, 1), 3)));
            Assert.AreEqual(new Sphere3D((5, 9, 8), 3), new Sphere3D((4, 5, 1), 3) + (1, 4, 7));
            Assert.AreEqual(new Sphere3D((3, 1, -6), 3), new Sphere3D((4, 5, 1), 3) - (1, 4, 7));
            Assert.AreEqual(new Sphere3D((5, 9, 8), 3), (1, 4, 7) + new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((-3, -1, 6), 3), (1, 4, 7) - new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), new Sphere3D((4, 5, 1), 3) * (ddouble)2);
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), new Sphere3D((4, 5, 1), 3) * (double)2);
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), (ddouble)2 * new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), (double)2 * new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((2, 2.5, 0.5), 1.5), new Sphere3D((4, 5, 1), 3) / (ddouble)2);
            Assert.AreEqual(new Sphere3D((2, 2.5, 0.5), 1.5), new Sphere3D((4, 5, 1), 3) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Sphere3D.IsValid(new Sphere3D((1, 2, 3), 4)));
            Assert.IsFalse(Sphere3D.IsValid(Sphere3D.Invalid));
        }
    }
}