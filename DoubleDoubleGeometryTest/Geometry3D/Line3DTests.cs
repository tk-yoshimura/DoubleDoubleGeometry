using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Line3DTests {
        [TestMethod()]
        public void Line3DTest() {
            Line3D line1 = Line3D.FromDirection(new Vector3D(6, 1, 3), new Vector3D(-1, 2, 3));
            Line3D line2 = Matrix3D.Scale(1, 2, 3) * line1 + (2, 4, 6);

            Vector3DAssert.AreEqual(new Vector3D(6 * 1 + 2, 1 * 2 + 4, 3 * 3 + 6), line2.Origin, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1 * 1, 2 * 2, 3 * 3).Normal, line2.Direction, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line3D.IsValid(Line3D.FromDirection(new Vector3D(6, 1, 3), new Vector3D(-1, 2, 3))));
            Assert.IsFalse(Line3D.IsValid(Line3D.Invalid));
        }
    }
}