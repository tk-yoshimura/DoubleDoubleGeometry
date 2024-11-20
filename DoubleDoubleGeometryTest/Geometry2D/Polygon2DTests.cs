using DoubleDouble;
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
    }
}