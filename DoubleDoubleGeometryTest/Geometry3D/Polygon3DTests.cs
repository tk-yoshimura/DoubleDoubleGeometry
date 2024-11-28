using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Polygon3DTests {
        [TestMethod()]
        public void Polygon3DTest() {
            Polygon3D polygon = new(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6));

            Vector3DAssert.AreEqual((1, 3, 5), polygon.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, 4, 6).Normal, polygon.Normal, 1e-30);
            Assert.AreEqual(Polygon2D.Regular(6), polygon.Polygon);

            Assert.AreEqual(6, polygon.Vertices);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) == new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)));
            Assert.IsTrue(new Polygon3D(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6)) != new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)));
            Assert.IsTrue(new Polygon3D(Polygon2D.Regular(6), (1, 3, 6), (2, 4, 6)) != new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)));
            Assert.IsTrue(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 7)) != new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)));

        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)), +new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)));
            Vector3DAssert.AreEqual(-(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0]), -(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6))).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] + (1, 4, 5), (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) + (1, 4, 5)).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] - (1, 4, 5), (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) - (1, 4, 5)).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((1, 4, 5) + new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0], ((1, 4, 5) + new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6))).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((1, 4, 5) - new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0], ((1, 4, 5) - new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6))).Vertex[0], 1e-30);

            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] * (ddouble)2, (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) * (ddouble)2).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] * (double)2, (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) * (double)2).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((ddouble)2 * new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0], ((ddouble)2 * new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6))).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((double)2 * new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0], ((double)2 * new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6))).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] / (ddouble)2, (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) / (ddouble)2).Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)).Vertex[0] / (double)2, (new Polygon3D(Polygon2D.Regular(6), (1, 3, 5), (2, 4, 6)) / (double)2).Vertex[0], 1e-30);
        }

        [TestMethod()]
        public void PointTest() {
            Polygon3D polygon1 = new(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6));
            Polygon3D polygon2 = new Polygon3D(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6)) * 2;
            Polygon3D polygon3 = new Polygon3D(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6)) * -2;
            Polygon3D polygon4 = new Polygon3D(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6)) + (1, 2, 4);

            Quaternion q = new Quaternion(3, 4, 2, 5).Normal;
            Matrix3D m = new(q);

            Polygon3D polygon5 = q * polygon1;

            Vector3DAssert.AreEqual(polygon1.Vertex[2] * 2, polygon2.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[3] * 2, polygon2.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[5] * 2, polygon2.Vertex[5], 1e-30);

            Vector3DAssert.AreEqual(polygon1.Vertex[2] * -2, polygon3.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[3] * -2, polygon3.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[5] * -2, polygon3.Vertex[5], 1e-30);

            Vector3DAssert.AreEqual(polygon1.Vertex[2] + (1, 2, 4), polygon4.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[3] + (1, 2, 4), polygon4.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(polygon1.Vertex[5] + (1, 2, 4), polygon4.Vertex[5], 1e-30);

            Vector3DAssert.AreEqual(q * polygon1.Vertex[2], polygon5.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(q * polygon1.Vertex[3], polygon5.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(q * polygon1.Vertex[5], polygon5.Vertex[5], 1e-30);
        }

        [TestMethod()]
        public void ProjectionTest() {
            Polygon3D polygon = new Polygon3D(Polygon2D.Regular(7), (1, 3, 5), (2, 4, 6)) + (1, 2, 4);

            Plane3D plane = polygon.Plane;

            Vector3D[] vertex = plane.Projection(polygon.Vertex).ToArray();

            Assert.IsTrue(vertex.All(v => ddouble.Abs(v.Z) < 1e-30));

            Assert.IsTrue(ddouble.Abs(plane.Projection(polygon.Vertex[0]).Z) < 1e-30);
        }
    }
}