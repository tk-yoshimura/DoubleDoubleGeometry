using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Intersect2DTests {
        [TestMethod()]
        public void LineLineTest() {
            Line2D line1 = Line2D.FromDirection(new Vector2D(1, 3), new Vector2D(3, 2));
            Line2D line2 = Line2D.FromDirection(new Vector2D(6, 1), new Vector2D(-1, 2));

            Vector2D cross = Intersect2D.LineLine(line1, line2);

            Vector2DAssert.AreEqual(new Vector2D(4, 5), cross, 1e-30);
        }

        [TestMethod()]
        public void CircleLineTest() {
            Line2D line = Line2D.FromDirection(new Vector2D(4, 1), new Vector2D(1, 3));
            Circle2D circle = new(new Vector2D(6, 12), 5);

            Vector2D[] cross = Intersect2D.CircleLine(circle, line);

            Vector2DAssert.AreEqual(new Vector2D(6, 7), cross[0], 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(9, 16), cross[1], 1e-30);
        }
    }
}