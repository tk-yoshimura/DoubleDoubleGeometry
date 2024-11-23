using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Line3DTests {
        [TestMethod()]
        public void Line3DTest() {
            Line3D line1 = Line3D.FromDirection((6, 1, 3), (-1, 2, 3));
            Line3D line2 = Matrix3D.Scale(1, 2, 3) * line1 + (2, 4, 6);

            Vector3DAssert.AreEqual((6 * 1 + 2, 1 * 2 + 4, 3 * 3 + 6), line2.Origin, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1 * 1, 2 * 2, 3 * 3).Normal, line2.Direction, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-1, 2, 3)) == Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-2, 4, 6)) == Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-1, 3, 3)) != Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 2, 7), (-1, 2, 3)) != Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(Line3D.FromDirection((4, 5, 3), (1, 2, 6)), +(Line3D.FromDirection((4, 5, 3), (1, 2, 6))));
            Assert.AreEqual(Line3D.FromDirection((-4, -5, -3), (-1, -2, -6)), -(Line3D.FromDirection((4, 5, 3), (1, 2, 6))));
            Assert.AreEqual(Line3D.FromDirection((5, 9, 10), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) + (1, 4, 7));
            Assert.AreEqual(Line3D.FromDirection((3, 1, -4), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) - (1, 4, 7));
            Assert.AreEqual(Line3D.FromDirection((5, 9, 10), (1, 2, 6)), (1, 4, 7) + Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((-3, -1, 4), (-1, -2, -6)), (1, 4, 7) - Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) * (ddouble)2);
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) * (double)2);
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), (ddouble)2 * Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), (double)2 * Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((2, 2.5, 1.5), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) / (ddouble)2);
            Assert.AreEqual(Line3D.FromDirection((2, 2.5, 1.5), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line3D.IsValid(Line3D.FromDirection((6, 1, 3), (-1, 2, 3))));
            Assert.IsFalse(Line3D.IsValid(Line3D.Invalid));
        }
    }
}