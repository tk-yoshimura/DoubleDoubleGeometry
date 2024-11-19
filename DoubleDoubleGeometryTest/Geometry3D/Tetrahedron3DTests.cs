using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Tetrahedron3DTests {
        [TestMethod()]
        public void Tetrahedron3DTest() {
            Tetrahedron3D tetrahedron1 = new(new Vector3D(0, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 2, 0), new Vector3D(0, 0, 3));
            Tetrahedron3D tetrahedron2 = Matrix3D.Scale(1, 2, 3) * tetrahedron1 + (2, 4, 6);

            PrecisionAssert.AreEqual(9, (Matrix3D.Rotate(1, 1, 1) * tetrahedron1).Area, 1e-30);
            PrecisionAssert.AreEqual(1, (Matrix3D.Rotate(1, 1, 1) * tetrahedron1).Volume, 1e-30);
            PrecisionAssert.AreEqual(6, (Matrix3D.Rotate(1, 2, 3) * tetrahedron2).Volume, 1e-30);

            Vector3DAssert.AreEqual(new Vector3D(0 * 1 + 2, 0 * 2 + 4, 0 * 3 + 6), tetrahedron2.V0, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(1 * 1 + 2, 0 * 2 + 4, 0 * 3 + 6), tetrahedron2.V1, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(0 * 1 + 2, 2 * 2 + 4, 0 * 3 + 6), tetrahedron2.V2, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(0 * 1 + 2, 0 * 2 + 4, 3 * 3 + 6), tetrahedron2.V3, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)), +(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5))));
            Assert.AreEqual(new Tetrahedron3D((-4, -5, -3), (-1, -2, -7), (-5, -1, -2), (-3, -4, -5)), -(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5))));
            Assert.AreEqual(new Tetrahedron3D((5, 9, 8), (2, 6, 12), (6, 5, 7), (4, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) + (1, 4, 5));
            Assert.AreEqual(new Tetrahedron3D((3, 1, -2), (0, -2, 2), (4, -3, -3), (2, 0, 0)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) - (1, 4, 5));
            Assert.AreEqual(new Tetrahedron3D((5, 9, 8), (2, 6, 12), (6, 5, 7), (4, 8, 10)), (1, 4, 5) + new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((-3, -1, 2), (0, 2, -2), (-4, 3, 3), (-2, 0, 0)), (1, 4, 5) - new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) * (ddouble)2);
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) * (double)2);
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), (ddouble)2 * new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), (double)2 * new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1), (1.5, 2, 2.5)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) / (ddouble)2);
            Assert.AreEqual(new Tetrahedron3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1), (1.5, 2, 2.5)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Tetrahedron3D.IsValid(new Tetrahedron3D(new Vector3D(0, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 2, 0), new Vector3D(0, 0, 3))));
            Assert.IsFalse(Tetrahedron3D.IsValid(Tetrahedron3D.Invalid));
        }
    }
}