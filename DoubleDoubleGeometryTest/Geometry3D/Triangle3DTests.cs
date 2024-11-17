using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Triangle3DTests {
        [TestMethod()]
        public void Triangle3DTest() {
            Triangle3D triangle1 = new(new Vector3D(8, 1, 1), new Vector3D(2, 3, 1), new Vector3D(4, 9, 1));
            Triangle3D triangle2 = Matrix3D.Scale(1, 2, 3) * triangle1 + (2, 4, 6);

            PrecisionAssert.AreEqual((2 * 2 + 6 * 6) / 2, (Matrix3D.Rotate(1, 1, 1) * triangle1).Area, 1e-30);
            PrecisionAssert.AreEqual(2 * 2 + 6 * 6, (Matrix3D.Rotate(1, 2, 3) * triangle2).Area, 2e-30);

            Vector3DAssert.AreEqual(new Vector3D(8 * 1 + 2, 1 * 2 + 4, 1 * 3 + 6), triangle2.V0, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2 * 1 + 2, 3 * 2 + 4, 1 * 3 + 6), triangle2.V1, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(4 * 1 + 2, 9 * 2 + 4, 1 * 3 + 6), triangle2.V2, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Triangle3D.IsValid(new Triangle3D(new Vector3D(8, 1, 1), new Vector3D(2, 3, 1), new Vector3D(4, 9, 1))));
            Assert.IsFalse(Triangle3D.IsValid(Triangle3D.Invalid));
        }
    }
}