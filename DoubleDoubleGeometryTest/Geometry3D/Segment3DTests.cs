using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Segment3DTests {
        [TestMethod()]
        public void Segment3DTest() {
            Segment3D segment1 = new(new Vector3D(6, 1, 4), new Vector3D(-1, 2, 6));
            Segment3D segment2 = Matrix3D.Scale(1, 2, 5) * segment1 + (2, 4, -1);

            PrecisionAssert.AreEqual(ddouble.Sqrt(7 * 7 + 1 * 1 + 2 * 2), segment1.Length, 1e-30);

            Vector3DAssert.AreEqual(new Vector3D(6 * 1 + 2, 1 * 2 + 4, 4 * 5 - 1), segment2.V0, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1 * 1 + 2, 2 * 2 + 4, 6 * 5 - 1), segment2.V1, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Segment3D.IsValid(new Segment3D(new Vector3D(6, 1, 4), new Vector3D(-1, 2, 6))));
            Assert.IsFalse(Segment3D.IsValid(Segment3D.Invalid));
        }
    }
}