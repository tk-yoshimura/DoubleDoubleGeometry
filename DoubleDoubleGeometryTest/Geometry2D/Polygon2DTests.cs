using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Polygon2DTests {
        [TestMethod()]
        public void Polygon2DTest() {
            Polygon2D polygon = Polygon2D.Regular(6);

            Assert.AreEqual(6, polygon.Vertices);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(Polygon2D.Regular(6) == Polygon2D.Regular(6));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(Polygon2D.Regular(6), +Polygon2D.Regular(6));
            Assert.AreEqual(-(Polygon2D.Regular(6).Vertex[0]), -(Polygon2D.Regular(6)).Vertex[0]);
            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] + (1, 4), (Polygon2D.Regular(6) + (1, 4)).Vertex[0]);
            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] - (1, 4), (Polygon2D.Regular(6) - (1, 4)).Vertex[0]);
            Assert.AreEqual((1, 4) + Polygon2D.Regular(6).Vertex[0], ((1, 4) + Polygon2D.Regular(6)).Vertex[0]);
            Assert.AreEqual((1, 4) - Polygon2D.Regular(6).Vertex[0], ((1, 4) - Polygon2D.Regular(6)).Vertex[0]);

            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] * (ddouble)2, (Polygon2D.Regular(6) * (ddouble)2).Vertex[0]);
            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] * (double)2, (Polygon2D.Regular(6) * (double)2).Vertex[0]);
            Assert.AreEqual((ddouble)2 * Polygon2D.Regular(6).Vertex[0], ((ddouble)2 * Polygon2D.Regular(6)).Vertex[0]);
            Assert.AreEqual((double)2 * Polygon2D.Regular(6).Vertex[0], ((double)2 * Polygon2D.Regular(6)).Vertex[0]);
            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] / (ddouble)2, (Polygon2D.Regular(6) / (ddouble)2).Vertex[0]);
            Assert.AreEqual(Polygon2D.Regular(6).Vertex[0] / (double)2, (Polygon2D.Regular(6) / (double)2).Vertex[0]);
        }

        [TestMethod()]
        public void CenterTest() {
            Assert.AreEqual((1, 4), (Polygon2D.Regular(6) + (1, 4)).Center);
        }

        [TestMethod()]
        public void PointTest() {
            Polygon2D polygon1 = Polygon2D.Regular(7);
            Polygon2D polygon2 = Polygon2D.Regular(7) * 2;
            Polygon2D polygon3 = Polygon2D.Regular(7) * -2;
            Polygon2D polygon4 = Polygon2D.Regular(7) + (1, 2);

            Complex c = new Complex(3, 4).Normal;
            Matrix2D m = new(c);

            Polygon2D polygon5 = c * polygon1;
            Polygon2D polygon6 = m * polygon1;

            Vector2DAssert.AreEqual(polygon1.Vertex[2] * 2, polygon2.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[3] * 2, polygon2.Vertex[3], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[5] * 2, polygon2.Vertex[5], 1e-30);

            Vector2DAssert.AreEqual(polygon1.Vertex[2] * -2, polygon3.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[3] * -2, polygon3.Vertex[3], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[5] * -2, polygon3.Vertex[5], 1e-30);

            Vector2DAssert.AreEqual(polygon1.Vertex[2] + (1, 2), polygon4.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[3] + (1, 2), polygon4.Vertex[3], 1e-30);
            Vector2DAssert.AreEqual(polygon1.Vertex[5] + (1, 2), polygon4.Vertex[5], 1e-30);

            Vector2DAssert.AreEqual(c * polygon1.Vertex[2], polygon5.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(c * polygon1.Vertex[3], polygon5.Vertex[3], 1e-30);
            Vector2DAssert.AreEqual(c * polygon1.Vertex[5], polygon5.Vertex[5], 1e-30);

            Vector2DAssert.AreEqual(m * polygon1.Vertex[2], polygon6.Vertex[2], 1e-30);
            Vector2DAssert.AreEqual(m * polygon1.Vertex[3], polygon6.Vertex[3], 1e-30);
            Vector2DAssert.AreEqual(m * polygon1.Vertex[5], polygon6.Vertex[5], 1e-30);
        }

        [TestMethod()]
        public void IsConvexTest() {
            for (int n = 3; n <= 16; n++) {
                Assert.IsTrue(Polygon2D.IsConvex(Polygon2D.Regular(n)));
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[2], vertex[3]) = (vertex[3], vertex[2]);

                Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[i] *= -0.5;

                    Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
                }
            }
        }
    }
}