using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Segment3DTests {
        [TestMethod()]
        public void Segment3DTest() {
            Segment3D segment1 = new((6, 1, 4), (-1, 2, 6));
            Segment3D segment2 = Matrix3D.Scale(1, 2, 5) * segment1 + (2, 4, -1);

            PrecisionAssert.AreEqual(ddouble.Sqrt(7 * 7 + 1 * 1 + 2 * 2), segment1.Length, 1e-30);

            Vector3DAssert.AreEqual((6 * 1 + 2, 1 * 2 + 4, 4 * 5 - 1), segment2.V0, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1 * 1 + 2, 2 * 2 + 4, 6 * 5 - 1), segment2.V1, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Segment3D((6, 1, 3), (-1, 2, 4)) == new Segment3D((6, 1, 3), (-1, 2, 4)));
            Assert.IsTrue(new Segment3D((6, 1, 3), (-1, 3, 4)) != new Segment3D((6, 1, 3), (-1, 2, 4)));
            Assert.IsTrue(new Segment3D((6, 2, 3), (-1, 2, 4)) != new Segment3D((6, 1, 3), (-1, 2, 4)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Segment3D((4, 5, 3), (1, 2, 7)), +(new Segment3D((4, 5, 3), (1, 2, 7))));
            Assert.AreEqual(new Segment3D((-4, -5, -3), (-1, -2, -7)), -(new Segment3D((4, 5, 3), (1, 2, 7))));
            Assert.AreEqual(new Segment3D((5, 9, 8), (2, 6, 12)), new Segment3D((4, 5, 3), (1, 2, 7)) + (1, 4, 5));
            Assert.AreEqual(new Segment3D((3, 1, -2), (0, -2, 2)), new Segment3D((4, 5, 3), (1, 2, 7)) - (1, 4, 5));
            Assert.AreEqual(new Segment3D((5, 9, 8), (2, 6, 12)), (1, 4, 5) + new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((-3, -1, 2), (0, 2, -2)), (1, 4, 5) - new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), new Segment3D((4, 5, 3), (1, 2, 7)) * (ddouble)2);
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), new Segment3D((4, 5, 3), (1, 2, 7)) * (double)2);
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), (ddouble)2 * new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), (double)2 * new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((2, 2.5, 1.5), (0.5, 1, 3.5)), new Segment3D((4, 5, 3), (1, 2, 7)) / (ddouble)2);
            Assert.AreEqual(new Segment3D((2, 2.5, 1.5), (0.5, 1, 3.5)), new Segment3D((4, 5, 3), (1, 2, 7)) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Segment3D.IsValid(new Segment3D((6, 1, 4), (-1, 2, 6))));
            Assert.IsFalse(Segment3D.IsValid(Segment3D.Invalid));
        }
    }
}