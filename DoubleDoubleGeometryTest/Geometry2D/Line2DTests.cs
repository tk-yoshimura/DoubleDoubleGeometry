using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Line2DTests {
        [TestMethod()]
        public void Line2DTest() {
            Line2D line1 = Line2D.FromDirection(new Vector2D(6, 1), new Vector2D(-1, 2));
            Line2D line2 = Matrix2D.Scale(1, 2) * line1 + (2, 4);

            Vector2DAssert.AreEqual(new Vector2D(6 * 1 + 2, 1 * 2 + 4), line2.Origin, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(-1 * 1, 2 * 2).Normal, line2.Direction, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(Line2D.FromDirection((6, 1), (-1, 2)) == Line2D.FromDirection((6, 1), (-1, 2)));
            Assert.IsTrue(Line2D.FromDirection((6, 1), (-2, 4)) == Line2D.FromDirection((6, 1), (-1, 2)));
            Assert.IsTrue(Line2D.FromDirection((6, 1), (-1, 3)) != Line2D.FromDirection((6, 1), (-1, 2)));
            Assert.IsTrue(Line2D.FromDirection((6, 2), (-1, 2)) != Line2D.FromDirection((6, 1), (-1, 2)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(Line2D.FromDirection((4, 5), (1, 2)), +(Line2D.FromDirection((4, 5), (1, 2))));
            Assert.AreEqual(Line2D.FromDirection((-4, -5), (-1, -2)), -(Line2D.FromDirection((4, 5), (1, 2))));
            Assert.AreEqual(Line2D.FromDirection((5, 9), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) + (1, 4));
            Assert.AreEqual(Line2D.FromDirection((3, 1), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) - (1, 4));
            Assert.AreEqual(Line2D.FromDirection((5, 9), (1, 2)), (1, 4) + Line2D.FromDirection((4, 5), (1, 2)));
            Assert.AreEqual(Line2D.FromDirection((-3, -1), (-1, -2)), (1, 4) - Line2D.FromDirection((4, 5), (1, 2)));
            Assert.AreEqual(Line2D.FromDirection((8, 10), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) * (ddouble)2);
            Assert.AreEqual(Line2D.FromDirection((8, 10), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) * (double)2);
            Assert.AreEqual(Line2D.FromDirection((8, 10), (1, 2)), (ddouble)2 * Line2D.FromDirection((4, 5), (1, 2)));
            Assert.AreEqual(Line2D.FromDirection((8, 10), (1, 2)), (double)2 * Line2D.FromDirection((4, 5), (1, 2)));
            Assert.AreEqual(Line2D.FromDirection((2, 2.5), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) / (ddouble)2);
            Assert.AreEqual(Line2D.FromDirection((2, 2.5), (1, 2)), Line2D.FromDirection((4, 5), (1, 2)) / (double)2);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line2D.IsValid(Line2D.FromIntersection(new Vector2D(6, 1), new Vector2D(-1, 2))));
            Assert.IsFalse(Line2D.IsValid(Line2D.Invalid));
        }
    }
}