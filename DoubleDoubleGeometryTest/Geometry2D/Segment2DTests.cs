using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Segment2DTests {
        [TestMethod()]
        public void Segment2DTest() {
            Segment2D segment1 = new((6, 1), (-1, 2));
            Segment2D segment2 = Matrix2D.Scale(1, 2) * segment1 + (2, 4);

            PrecisionAssert.AreEqual(ddouble.Sqrt(7 * 7 + 1 * 1), segment1.Length, 1e-30);

            Vector2DAssert.AreEqual((6 * 1 + 2, 1 * 2 + 4), segment2.V0, 1e-30);
            Vector2DAssert.AreEqual((-1 * 1 + 2, 2 * 2 + 4), segment2.V1, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Segment2D((6, 1), (-1, 2)) == new Segment2D((6, 1), (-1, 2)));
            Assert.IsTrue(new Segment2D((6, 1), (-2, 4)) != new Segment2D((6, 1), (-1, 2)));
            Assert.IsTrue(new Segment2D((6, 1), (-1, 3)) != new Segment2D((6, 1), (-1, 2)));
            Assert.IsTrue(new Segment2D((6, 2), (-1, 2)) != new Segment2D((6, 1), (-1, 2)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Segment2D((4, 5), (1, 2)), +(new Segment2D((4, 5), (1, 2))));
            Assert.AreEqual(new Segment2D((-4, -5), (-1, -2)), -(new Segment2D((4, 5), (1, 2))));
            Assert.AreEqual(new Segment2D((5, 9), (2, 6)), new Segment2D((4, 5), (1, 2)) + (1, 4));
            Assert.AreEqual(new Segment2D((3, 1), (0, -2)), new Segment2D((4, 5), (1, 2)) - (1, 4));
            Assert.AreEqual(new Segment2D((5, 9), (2, 6)), (1, 4) + new Segment2D((4, 5), (1, 2)));
            Assert.AreEqual(new Segment2D((-3, -1), (0, 2)), (1, 4) - new Segment2D((4, 5), (1, 2)));
            Assert.AreEqual(new Segment2D((8, 10), (2, 4)), new Segment2D((4, 5), (1, 2)) * (ddouble)2);
            Assert.AreEqual(new Segment2D((8, 10), (2, 4)), new Segment2D((4, 5), (1, 2)) * (double)2);
            Assert.AreEqual(new Segment2D((8, 10), (2, 4)), (ddouble)2 * new Segment2D((4, 5), (1, 2)));
            Assert.AreEqual(new Segment2D((8, 10), (2, 4)), (double)2 * new Segment2D((4, 5), (1, 2)));
            Assert.AreEqual(new Segment2D((2, 2.5), (0.5, 1)), new Segment2D((4, 5), (1, 2)) / (ddouble)2);
            Assert.AreEqual(new Segment2D((2, 2.5), (0.5, 1)), new Segment2D((4, 5), (1, 2)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Segment2D segment1 = new(Vector2D.Zero, (1, 1));
            Segment2D segment2 = new Segment2D(Vector2D.Zero, (1, 1)) * 2;
            Segment2D segment3 = new Segment2D(Vector2D.Zero, (1, 1)) * -2;
            Segment2D segment4 = new Segment2D(Vector2D.Zero, (1, 1)) + (2, 3);
            Segment2D segment5 = new(Vector2D.Zero, (3, 4));
            Segment2D segment6 = new(Vector2D.Zero, (4, 3));

            Complex c = new Complex(3, 4).Normal;
            Matrix2D m = new(c);

            Segment2D segment7 = c * segment4;
            Segment2D segment8 = m * segment4;

            Vector2DAssert.AreEqual((0, 0), segment1.Point(0), 1e-30);
            Vector2DAssert.AreEqual((1, 1), segment1.Point(1), 1e-30);
            Vector2DAssert.AreEqual((2, 2), segment1.Point(2), 1e-30);

            Vector2DAssert.AreEqual(segment1.Point(0) * 2, segment2.Point(0), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) * 2, segment2.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) * 2, segment2.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(segment1.Point(0) * -2, segment3.Point(0), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) * -2, segment3.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) * -2, segment3.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual(segment1.Point(0) + (2, 3), segment4.Point(0), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) + (2, 3), segment4.Point(ddouble.Pi / 4), 1e-30);
            Vector2DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) + (2, 3), segment4.Point(ddouble.Pi / 2), 1e-30);

            Vector2DAssert.AreEqual((3, 4), segment5.Point(1), 1e-30);
            Vector2DAssert.AreEqual((6, 8), segment5.Point(2), 1e-30);

            Vector2DAssert.AreEqual((4, 3), segment6.Point(1), 1e-30);
            Vector2DAssert.AreEqual((8, 6), segment6.Point(2), 1e-30);

            Vector2DAssert.AreEqual(c * segment4.Point(0), segment7.Point(0), 1e-30);
            Vector2DAssert.AreEqual(c * segment4.Point(1), segment7.Point(1), 1e-30);

            Vector2DAssert.AreEqual(m * segment4.Point(0), segment8.Point(0), 1e-30);
            Vector2DAssert.AreEqual(m * segment4.Point(1), segment8.Point(1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Segment2D.IsValid(new Segment2D((6, 1), (-1, 2))));
            Assert.IsFalse(Segment2D.IsValid(Segment2D.Invalid));
        }
    }
}