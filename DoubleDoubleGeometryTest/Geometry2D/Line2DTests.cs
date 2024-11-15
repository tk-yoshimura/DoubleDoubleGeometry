using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Line2DTests {
        [TestMethod()]
        public void Line2DTest() {
            Line2D line1 = Line2D.FromDirection(new Vector2D(6, 1), new Vector2D(-1, 2));
            Line2D line2 = HomogeneousMatrix2D.Move(2, 4) * Matrix2D.Scale(1, 2) * line1;

            Vector2DAssert.AreEqual(new Vector2D(6 * 1 + 2, 1 * 2 + 4), line2.Origin, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(-1 * 1, 2 * 2).Normal, line2.Direction, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line2D.IsValid(Line2D.FromIntersection(new Vector2D(6, 1), new Vector2D(-1, 2))));
            Assert.IsFalse(Line2D.IsValid(Line2D.Invalid));
        }
    }
}