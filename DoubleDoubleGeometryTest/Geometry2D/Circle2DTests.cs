using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Circle2DTests {
        [TestMethod()]
        public void Circle2DTest() {
            Circle2D circle = new(new Vector2D(1, 3), 2);

            Vector2DAssert.AreEqual(new Vector2D(1, 3), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(2.0, circle.Radius, 1e-30);

            PrecisionAssert.AreEqual(4 * ddouble.Pi, circle.Area, 1e-30);
        }

        [TestMethod()]
        public void CircumTest() {
            Vector2D v0 = new(3, 9), v1 = new(4, 2), v2 = new(12, 6);

            Circle2D circle = Circle2D.Circum(new Triangle2D(v0, v1, v2));

            Vector2DAssert.AreEqual(new Vector2D(7, 6), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(5, circle.Radius, 1e-30);
        }

        [TestMethod()]
        public void IncircleTest() {
            Vector2D v0 = new(2, 1), v1 = new(6, 1), v2 = new(6, 4);

            Circle2D circle = Circle2D.Incircle(new Triangle2D(v0, v1, v2));

            Vector2DAssert.AreEqual(new Vector2D(5, 2), circle.Center, 1e-30);
            PrecisionAssert.AreEqual(1, circle.Radius, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Circle2D.IsValid(new Circle2D(new Vector2D(1, 3), 2)));
            Assert.IsFalse(Circle2D.IsValid(Circle2D.Invalid));
        }
    }
}