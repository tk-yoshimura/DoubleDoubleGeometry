using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Triangle2DTests {
        [TestMethod()]
        public void Triangle2DTest() {
            Triangle2D triangle1 = new((8, 1), (2, 3), (4, 9));
            Triangle2D triangle2 = Matrix2D.Scale(1, 2) * triangle1 + (2, 4);

            PrecisionAssert.AreEqual((2 * 2 + 6 * 6) / 2, triangle1.Area, 1e-30);
            PrecisionAssert.AreEqual(2 * 2 + 6 * 6, triangle2.Area, 1e-30);

            Vector2DAssert.AreEqual((8 * 1 + 2, 1 * 2 + 4), triangle2.V0, 1e-30);
            Vector2DAssert.AreEqual((2 * 1 + 2, 3 * 2 + 4), triangle2.V1, 1e-30);
            Vector2DAssert.AreEqual((4 * 1 + 2, 9 * 2 + 4), triangle2.V2, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Triangle2D((6, 1), (-1, 2), (4, 5)) == new Triangle2D((6, 1), (-1, 2), (4, 5)));
            Assert.IsTrue(new Triangle2D((6, 1), (-1, 3), (4, 5)) != new Triangle2D((6, 1), (-1, 2), (4, 5)));
            Assert.IsTrue(new Triangle2D((6, 2), (-1, 2), (4, 5)) != new Triangle2D((6, 1), (-1, 2), (4, 5)));
            Assert.IsTrue(new Triangle2D((6, 2), (-1, 2), (4, 6)) != new Triangle2D((6, 1), (-1, 2), (4, 5)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Triangle2D((4, 5), (1, 2), (3, 7)), +(new Triangle2D((4, 5), (1, 2), (3, 7))));
            Assert.AreEqual(new Triangle2D((-4, -5), (-1, -2), (-3, -7)), -(new Triangle2D((4, 5), (1, 2), (3, 7))));
            Assert.AreEqual(new Triangle2D((5, 9), (2, 6), (4, 11)), new Triangle2D((4, 5), (1, 2), (3, 7)) + (1, 4));
            Assert.AreEqual(new Triangle2D((3, 1), (0, -2), (2, 3)), new Triangle2D((4, 5), (1, 2), (3, 7)) - (1, 4));
            Assert.AreEqual(new Triangle2D((5, 9), (2, 6), (4, 11)), (1, 4) + new Triangle2D((4, 5), (1, 2), (3, 7)));
            Assert.AreEqual(new Triangle2D((-3, -1), (0, 2), (-2, -3)), (1, 4) - new Triangle2D((4, 5), (1, 2), (3, 7)));
            Assert.AreEqual(new Triangle2D((8, 10), (2, 4), (6, 14)), new Triangle2D((4, 5), (1, 2), (3, 7)) * (ddouble)2);
            Assert.AreEqual(new Triangle2D((8, 10), (2, 4), (6, 14)), new Triangle2D((4, 5), (1, 2), (3, 7)) * (double)2);
            Assert.AreEqual(new Triangle2D((8, 10), (2, 4), (6, 14)), (ddouble)2 * new Triangle2D((4, 5), (1, 2), (3, 7)));
            Assert.AreEqual(new Triangle2D((8, 10), (2, 4), (6, 14)), (double)2 * new Triangle2D((4, 5), (1, 2), (3, 7)));
            Assert.AreEqual(new Triangle2D((2, 2.5), (0.5, 1), (1.5, 3.5)), new Triangle2D((4, 5), (1, 2), (3, 7)) / (ddouble)2);
            Assert.AreEqual(new Triangle2D((2, 2.5), (0.5, 1), (1.5, 3.5)), new Triangle2D((4, 5), (1, 2), (3, 7)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Triangle2D triangle1 = new(Vector2D.Zero, (1, 1), (2, 0));
            Triangle2D triangle2 = new Triangle2D(Vector2D.Zero, (1, 1), (2, 0)) * 2;
            Triangle2D triangle3 = new Triangle2D(Vector2D.Zero, (1, 1), (2, 0)) * -2;
            Triangle2D triangle4 = new Triangle2D(Vector2D.Zero, (1, 1), (2, 0)) + (2, 3);

            Complex c = new Complex(3, 4).Normal;
            Matrix2D m = new(c);

            Triangle2D triangle5 = c * triangle4;
            Triangle2D triangle6 = m * triangle4;

            Vector2DAssert.AreEqual((0, 0), triangle1.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual((1, 1), triangle1.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual((1, 0.5), triangle1.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual((1, 1), triangle1.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual((2, 0), triangle1.Point(0, 1), 1e-30);

            Vector2DAssert.AreEqual(triangle1.Point(0, 0) * 2, triangle2.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 0) * 2, triangle2.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0.5, 0.5) * 2, triangle2.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 1) * 2, triangle2.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0, 1) * 2, triangle2.Point(0, 1), 1e-30);

            Vector2DAssert.AreEqual(triangle1.Point(0, 0) * -2, triangle3.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 0) * -2, triangle3.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0.5, 0.5) * -2, triangle3.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 1) * -2, triangle3.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0, 1) * -2, triangle3.Point(0, 1), 1e-30);

            Vector2DAssert.AreEqual(triangle1.Point(0, 0) + (2, 3), triangle4.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 0) + (2, 3), triangle4.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0.5, 0.5) + (2, 3), triangle4.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(1, 1) + (2, 3), triangle4.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual(triangle1.Point(0, 1) + (2, 3), triangle4.Point(0, 1), 1e-30);

            Vector2DAssert.AreEqual(c * triangle4.Point(0, 0), triangle5.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual(c * triangle4.Point(1, 0), triangle5.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual(c * triangle4.Point(0.5, 0.5), triangle5.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual(c * triangle4.Point(1, 1), triangle5.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual(c * triangle4.Point(0, 1), triangle5.Point(0, 1), 1e-30);

            Vector2DAssert.AreEqual(m * triangle4.Point(0, 0), triangle6.Point(0, 0), 1e-30);
            Vector2DAssert.AreEqual(m * triangle4.Point(1, 0), triangle6.Point(1, 0), 1e-30);
            Vector2DAssert.AreEqual(m * triangle4.Point(0.5, 0.5), triangle6.Point(0.5, 0.5), 1e-30);
            Vector2DAssert.AreEqual(m * triangle4.Point(1, 1), triangle6.Point(1, 1), 1e-30);
            Vector2DAssert.AreEqual(m * triangle4.Point(0, 1), triangle6.Point(0, 1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Triangle2D.IsValid(new Triangle2D(new Vector2D(8, 1), new Vector2D(2, 3), new Vector2D(4, 9))));
            Assert.IsFalse(Triangle2D.IsValid(Triangle2D.Invalid));
        }

        [TestMethod()]
        public void InsideTest() {
            Triangle2D t = new((0, 0), (0, 1), (1, 0));

            Vector2D[] insides = [
                (0.25, 0.25), (0.25, 0.5), (0.5, 0.25)
            ];

            Vector2D[] outsides = [
                (-0.25, -0.25), (-0.25, 0.5), (0.5, -0.25), (-0.25, -0.5), (-0.5, -0.25),
                (0, 1.5), (1.5, 0), (0.5, 0.75), (0.75, 0.5)
            ];

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t.Inside(v));
            }

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Matrix2D m = new double[,] { { 1, 2 }, { 3, 5 } };
            Vector2D s = (4, 6);

            Triangle2D t2 = m * t + s;

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t2.Inside(m * v + s));
            }

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t2.Inside(m * v + s));
            }
        }
    }
}