using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Intersect2DTests {
        [TestMethod()]
        public void LineLineTest() {
            Line2D line1 = Line2D.FromDirection((1, 3), (3, 2));
            Line2D line2 = Line2D.FromDirection((6, 1), (-1, 2));

            Vector2D cross = Intersect2D.LineLine(line1, line2);

            Vector2DAssert.AreEqual((4, 5), cross, 1e-30);
        }

        [TestMethod()]
        public void CircleLineTest() {
            Line2D line = Line2D.FromDirection((4, 1), (1, 3));
            Circle2D circle = new((6, 12), 5);

            Vector2D[] cross = Intersect2D.CircleLine(circle, line);

            Vector2DAssert.AreEqual((6, 7), cross[0], 1e-30);
            Vector2DAssert.AreEqual((9, 16), cross[1], 1e-30);
        }

        [TestMethod()]
        public void CircleCircleTest() {
            Vector2D[] cross1 = Intersect2D.CircleCircle(((0, 1), 5), ((4, 3), ddouble.Sqrt(5)));
            Vector2D[] cross2 = Intersect2D.CircleCircle(((6, 12), 5), ((7, 12), 3));
            Vector2D[] cross3 = Intersect2D.CircleCircle(((0, 0), 5), ((0, 2), 3));

            Assert.AreEqual(2, cross1.Length);
            Vector2DAssert.AreEqual((5, 1), cross1[0], 1e-30);
            Vector2DAssert.AreEqual((3, 5), cross1[1], 1e-30);

            Assert.AreEqual(0, cross2.Length);

            Assert.AreEqual(1, cross3.Length);

            Vector2DAssert.AreEqual((0, 5), cross3[0], 1e-30);
        }
    }
}