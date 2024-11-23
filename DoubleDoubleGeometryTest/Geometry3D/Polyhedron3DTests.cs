using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Polyhedron3DTests {
        [TestMethod()]
        public void Polyhedron3DTest() {
            Polyhedron3D polygon = Polyhedron3D.Cube;

            Assert.AreEqual(8, polygon.Vertices);
            Assert.AreEqual(12, polygon.Edges);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(Polyhedron3D.Cube == Polyhedron3D.Cube);
        }

        [TestMethod()]
        public void OperatorTest() {
            Quaternion q = new Quaternion(2, 3, 5, 3).Normal;
            Matrix3D m = new Matrix3D(q);

            Assert.AreEqual(Polyhedron3D.Cube, +Polyhedron3D.Cube);
            Assert.AreEqual(-(Polyhedron3D.Cube.Vertex[0]), -(Polyhedron3D.Cube).Vertex[0]);
            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] + (1, 4, 3), (Polyhedron3D.Cube + (1, 4, 3)).Vertex[0]);
            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] - (1, 4, 3), (Polyhedron3D.Cube - (1, 4, 3)).Vertex[0]);
            Assert.AreEqual((1, 4, 3) + Polyhedron3D.Cube.Vertex[0], ((1, 4, 3) + Polyhedron3D.Cube).Vertex[0]);
            Assert.AreEqual((1, 4, 3) - Polyhedron3D.Cube.Vertex[0], ((1, 4, 3) - Polyhedron3D.Cube).Vertex[0]);

            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] * (ddouble)2, (Polyhedron3D.Cube * (ddouble)2).Vertex[0]);
            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] * (double)2, (Polyhedron3D.Cube * (double)2).Vertex[0]);
            Assert.AreEqual((ddouble)2 * Polyhedron3D.Cube.Vertex[0], ((ddouble)2 * Polyhedron3D.Cube).Vertex[0]);
            Assert.AreEqual((double)2 * Polyhedron3D.Cube.Vertex[0], ((double)2 * Polyhedron3D.Cube).Vertex[0]);
            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] / (ddouble)2, (Polyhedron3D.Cube / (ddouble)2).Vertex[0]);
            Assert.AreEqual(Polyhedron3D.Cube.Vertex[0] / (double)2, (Polyhedron3D.Cube / (double)2).Vertex[0]);

            Assert.AreEqual(q * Polyhedron3D.Dodecahedron.Vertex[0], (q * Polyhedron3D.Dodecahedron).Vertex[0]);
            Assert.AreEqual(m * Polyhedron3D.Dodecahedron.Vertex[0], (m * Polyhedron3D.Dodecahedron).Vertex[0]);
        }

        [TestMethod()]
        public void TetrahedronTest() {
            Polyhedron3D p = Polyhedron3D.Tetrahedron;

            Assert.AreEqual(4, p.Vertices);
            Assert.AreEqual(6, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            Assert.AreEqual(Vector3D.Zero, p.Center);
            Assert.AreEqual((2, 2, 2), p.Size);

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(2 * ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                PrecisionAssert.AreEqual(ddouble.Sqrt(3), p.Vertex[i].Norm, 1e-30);
            }

            foreach ((int i, int j) in p.Connection) {
                PrecisionAssert.AreEqual(2 * ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = i + 1; j < p.Vertices; j++) {
                    Assert.IsTrue(Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 0.125);
                }
            }

            Assert.AreEqual(4, p.Connection.EnumerateTriangle().Count());
        }

        [TestMethod()]
        public void CubeTest() {
            Polyhedron3D p = Polyhedron3D.Cube;

            Assert.AreEqual(8, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            Assert.AreEqual(Vector3D.Zero, p.Center);
            Assert.AreEqual((2, 2, 2), p.Size);

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                PrecisionAssert.AreEqual(ddouble.Sqrt(3), p.Vertex[i].Norm, 1e-30);
            }

            foreach ((int i, int j) in p.Connection) {
                PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = i + 1; j < p.Vertices; j++) {
                    Assert.IsTrue(Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 0.125);
                }
            }

            Assert.AreEqual(0, p.Connection.EnumerateTriangle().Count());
        }

        [TestMethod()]
        public void OctahedronTest() {
            Polyhedron3D p = Polyhedron3D.Octahedron;

            Assert.AreEqual(6, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(4, p.Connection[i].Count);
            }

            Assert.AreEqual(Vector3D.Zero, p.Center);
            Assert.AreEqual((2, 2, 2), p.Size);

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                PrecisionAssert.AreEqual(1, p.Vertex[i].Norm);
            }

            foreach ((int i, int j) in p.Connection) {
                PrecisionAssert.AreEqual(ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = i + 1; j < p.Vertices; j++) {
                    Assert.IsTrue(Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 0.125);
                }
            }

            Assert.AreEqual(8, p.Connection.EnumerateTriangle().Count());
        }

        [TestMethod()]
        public void DodecahedronTest() {
            Polyhedron3D p = Polyhedron3D.Dodecahedron;

            Assert.AreEqual(20, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            Assert.AreEqual(Vector3D.Zero, p.Center);
            Assert.AreEqual((2, 2, 2), p.Size);

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(4 / (3 + ddouble.Sqrt(5)), Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                PrecisionAssert.AreEqual(ddouble.Sqrt(3) / ddouble.GoldenRatio, p.Vertex[i].Norm, 1e-30);
            }

            foreach ((int i, int j) in p.Connection) {
                PrecisionAssert.AreEqual(4 / (3 + ddouble.Sqrt(5)), Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = i + 1; j < p.Vertices; j++) {
                    Assert.IsTrue(Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 0.125);
                }
            }

            Assert.AreEqual(0, p.Connection.EnumerateTriangle().Count());
        }

        [TestMethod()]
        public void IcosahedronTest() {
            Polyhedron3D p = Polyhedron3D.Icosahedron;

            Assert.AreEqual(12, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(5, p.Connection[i].Count);
            }

            Assert.AreEqual(Vector3D.Zero, p.Center);
            Assert.AreEqual((2, 2, 2), p.Size);

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(4 / (1 + ddouble.Sqrt(5)), Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                PrecisionAssert.AreEqual(ddouble.Sqrt(ddouble.Sqrt(5) / ddouble.GoldenRatio), p.Vertex[i].Norm, 1e-30);
            }

            foreach ((int i, int j) in p.Connection) {
                PrecisionAssert.AreEqual(4 / (1 + ddouble.Sqrt(5)), Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = i + 1; j < p.Vertices; j++) {
                    Assert.IsTrue(Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 0.125);
                }
            }

            Assert.AreEqual(20, p.Connection.EnumerateTriangle().Count());

            foreach ((int i, int j, int k) in p.Connection.EnumerateTriangle()) {
                Console.WriteLine($"{i}, {j}, {k}");
            }
        }
    }
}