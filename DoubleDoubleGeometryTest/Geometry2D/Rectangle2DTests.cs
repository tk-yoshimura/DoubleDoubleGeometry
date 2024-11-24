using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Rectangle2DTests {
        [TestMethod()]
        public void Rectangle2DTest() {
            Rectangle2D rectangle = new((1, 2), (4, 3), 5);

            Assert.AreEqual((1, 2), rectangle.Center);
            Assert.AreEqual((4d, 3d), rectangle.Scale);
            Assert.AreEqual(8, rectangle.Width);
            Assert.AreEqual(6, rectangle.Height);
            PrecisionAssert.AreEqual(5d - ddouble.Pi * 2, rectangle.Angle, 1e-30);

            PrecisionAssert.AreEqual(4 * 3 * 4, rectangle.Area, 1e-30);
            PrecisionAssert.AreEqual(28, rectangle.Perimeter, 1e-30);
        }

        [TestMethod]
        public void EqualTest() {
            Rectangle2D rectangle1 = new((1, 2), (4, 3), 5);
            Rectangle2D rectangle2 = new((1, 2), (4, 3), 5);
            Rectangle2D rectangle3 = new((1, 2), (4, 4), 5);

            Assert.AreEqual(rectangle1, rectangle2);
            Assert.AreNotEqual(rectangle1, rectangle3);

            Assert.IsTrue(rectangle1 == rectangle2);
            Assert.IsTrue(rectangle1 != rectangle3);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Rectangle2D((4, 5), (1, 2), 3), +(new Rectangle2D((4, 5), (1, 2), 3)));
            Assert.AreEqual(new Rectangle2D((-4, -5), (-1, -2), 3), -(new Rectangle2D((4, 5), (1, 2), 3)));
            Assert.AreEqual(new Rectangle2D((5, 9), (1, 2), 3), new Rectangle2D((4, 5), (1, 2), 3) + (1, 4));
            Assert.AreEqual(new Rectangle2D((3, 1), (1, 2), 3), new Rectangle2D((4, 5), (1, 2), 3) - (1, 4));
            Assert.AreEqual(new Rectangle2D((5, 9), (1, 2), 3), (1, 4) + new Rectangle2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Rectangle2D((-3, -1), (-1, -2), 3), (1, 4) - new Rectangle2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Rectangle2D((8, 10), (2, 4), 3), new Rectangle2D((4, 5), (1, 2), 3) * (ddouble)2);
            Assert.AreEqual(new Rectangle2D((8, 10), (2, 4), 3), new Rectangle2D((4, 5), (1, 2), 3) * (double)2);
            Assert.AreEqual(new Rectangle2D((8, 10), (2, 4), 3), (ddouble)2 * new Rectangle2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Rectangle2D((8, 10), (2, 4), 3), (double)2 * new Rectangle2D((4, 5), (1, 2), 3));
            Assert.AreEqual(new Rectangle2D((2, 2.5), (0.5, 1), 3), new Rectangle2D((4, 5), (1, 2), 3) / (ddouble)2);
            Assert.AreEqual(new Rectangle2D((2, 2.5), (0.5, 1), 3), new Rectangle2D((4, 5), (1, 2), 3) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Rectangle2D rectangle1 = new(Vector2D.Zero, (3, 2), 0);
            Rectangle2D rectangle2 = new Rectangle2D(Vector2D.Zero, (3, 2), 0) * 2;
            Rectangle2D rectangle3 = new Rectangle2D(Vector2D.Zero, (3, 2), 0) * -2;
            Rectangle2D rectangle4 = new((2, 3), (3, 2), 0);
            Rectangle2D rectangle5 = new((2, 3), (3, 2), ddouble.Pi / 2);

            Complex c = (3, 4);

            Rectangle2D rectangle6 = c * rectangle5;

            Vector2DAssert.AreEqual((-3, -2), rectangle1.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual((3, -2), rectangle1.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual((3, 2), rectangle1.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual((-3, 2), rectangle1.Vertex[3], 1e-30);

            Vector2DAssert.AreEqual((4, 0), rectangle5.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual((4, 6), rectangle5.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual((0, 6), rectangle5.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual((0, 0), rectangle5.Vertex[3], 1e-30);

            Vector2DAssert.AreEqual(rectangle1.Vertex[0] * 2, rectangle2.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[1] * 2, rectangle2.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[2] * 2, rectangle2.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[3] * 2, rectangle2.Vertex[3], 1e-30);

            Vector2DAssert.AreEqual(rectangle1.Vertex[0] * -2, rectangle3.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[1] * -2, rectangle3.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[2] * -2, rectangle3.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[3] * -2, rectangle3.Vertex[3], 1e-30);

            Vector2DAssert.AreEqual(rectangle1.Vertex[0] + (2, 3), rectangle4.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[1] + (2, 3), rectangle4.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[2] + (2, 3), rectangle4.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(rectangle1.Vertex[3] + (2, 3), rectangle4.Vertex[3], 1e-30);

            Vector2DAssert.AreEqual(c * rectangle5.Vertex[0], rectangle6.Vertex[0], 1e-30);
            Vector2DAssert.AreEqual(c * rectangle5.Vertex[1], rectangle6.Vertex[1], 1e-30);
            Vector2DAssert.AreEqual(c * rectangle5.Vertex[2], rectangle6.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(c * rectangle5.Vertex[3], rectangle6.Vertex[3], 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Rectangle2D.IsValid(new Rectangle2D((1, 2), (4, 3), 5)));
            Assert.IsFalse(Rectangle2D.IsValid(Rectangle2D.Invalid));
        }
    }
}