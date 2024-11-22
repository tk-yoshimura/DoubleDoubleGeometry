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
        public void PointTest() {
            Line2D line1 = Line2D.FromDirection(Vector2D.Zero, (1, 1));
            Line2D line2 = Line2D.FromDirection(Vector2D.Zero, (1, 1)) * 2;
            Line2D line3 = Line2D.FromDirection(Vector2D.Zero, (1, 1)) * -2;
            Line2D line4 = Line2D.FromDirection((2, 3), (1, 1));
            Line2D line5 = Line2D.FromDirection(Vector2D.Zero, (3, 4));
            Line2D line6 = Line2D.FromDirection(Vector2D.Zero, (4, 3));

            Vector2DAssert.AreEqual((0, 0), line1.Point(0), 1e-30);
            Vector2DAssert.AreEqual((1, 1), line1.Point(ddouble.Sqrt2), 1e-30);
            Vector2DAssert.AreEqual((2, 2), line1.Point(ddouble.Sqrt2 * 2), 1e-30);

            Vector2DAssert.AreEqual(line1.Point(0) * 2, line2.Point(0), 1e-30);
            Vector2DAssert.AreEqual(line1.Point(ddouble.Pi / 4), line2.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(line1.Point(ddouble.Pi / 2), line2.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(-line1.Point(0), line3.Point(0), 1e-30);
            Vector2DAssert.AreEqual(-line1.Point(ddouble.Pi / 4), line3.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(-line1.Point(ddouble.Pi / 2), line3.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(line1.Point(0) + (2, 3), line4.Point(0), 1e-30);
            Vector2DAssert.AreEqual(line1.Point(ddouble.Pi / 4) + (2, 3), line4.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(line1.Point(ddouble.Pi / 2) + (2, 3), line4.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual((3, 4), line5.Point(5), 1e-30);
            Vector2DAssert.AreEqual((6, 8), line5.Point(10), 1e-30);

            Vector2DAssert.AreEqual((4, 3), line6.Point(5), 1e-30);
            Vector2DAssert.AreEqual((8, 6), line6.Point(10), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line2D.IsValid(Line2D.FromIntersection(new Vector2D(6, 1), new Vector2D(-1, 2))));
            Assert.IsFalse(Line2D.IsValid(Line2D.Invalid));
        }
    }
}