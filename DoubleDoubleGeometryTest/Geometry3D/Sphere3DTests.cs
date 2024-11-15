using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Sphere3DTests {
        [TestMethod()]
        public void Sphere3DTest() {
            Sphere3D sphere1 = new(new Vector3D(1, 2, 3), 4);
            Sphere3D sphere2 = Sphere3D.FromIntersection(new Vector3D(3, 2, -1), new Vector3D(1, 3, -2), new Vector3D(3, -1, -4), new Vector3D(0, 0, -2));

            Vector3DAssert.AreEqual(new Vector3D(1, 2, 3), sphere1.Center, 1e-30);
            PrecisionAssert.AreEqual(4.0, sphere1.Radius, 1e-30);
            PrecisionAssert.AreEqual(4 * 4 * 4 * ddouble.Pi, sphere1.Area, 1e-30);
            PrecisionAssert.AreEqual(4.0 * 4 * 4 * 4 * ddouble.Pi / 3.0, sphere1.Volume, 1e-30);

            Vector3DAssert.AreEqual(new Vector3D(2, 1, -3), sphere2.Center, 1e-30);
            PrecisionAssert.AreEqual(6, sphere2.Radius * sphere2.Radius, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Sphere3D.IsValid(new Sphere3D(new Vector3D(1, 2, 3), 4)));
            Assert.IsFalse(Sphere3D.IsValid(Sphere3D.Invalid));
        }
    }
}