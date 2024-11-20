using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
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
        }

        [TestMethod()]
        public void TetrahedronTest() {
            Polyhedron3D p = Polyhedron3D.Tetrahedron;

            Assert.AreEqual(4, p.Vertices);
            Assert.AreEqual(6, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(2 * ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            foreach ((int i, int j) in p.Connection) { 
                PrecisionAssert.AreEqual(2 * ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }
        }

        [TestMethod()]
        public void CubeTest() {
            Polyhedron3D p = Polyhedron3D.Cube;

            Assert.AreEqual(8, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            foreach ((int i, int j) in p.Connection) { 
                PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }
        }

        [TestMethod()]
        public void OctahedronTest() {
            Polyhedron3D p = Polyhedron3D.Octahedron;

            Assert.AreEqual(6, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(4, p.Connection[i].Count);
            }

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    PrecisionAssert.AreEqual(ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            foreach ((int i, int j) in p.Connection) { 
                PrecisionAssert.AreEqual(ddouble.Sqrt2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }
        }

        [TestMethod()]
        public void Dodecahedron() {
            Polyhedron3D p = Polyhedron3D.Dodecahedron;

            Assert.AreEqual(20, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    Console.WriteLine($"{i}, {j}");
                    PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            foreach ((int i, int j) in p.Connection) { 
                PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }
        }

        [TestMethod()]
        public void Icosahedron() {
            Polyhedron3D p = Polyhedron3D.Icosahedron;

            Assert.AreEqual(12, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(5, p.Connection[i].Count);
            }

            for (int i = 0; i < p.Vertices; i++) {
                for (int j = 0; j < p.Vertices; j++) {
                    if (i==j || Vector3D.Distance(p.Vertex[i], p.Vertex[j]) > 2.1) {
                        continue;
                    }

                    Console.WriteLine($"{i}, {j}");
                }
            }

            for (int i = 0; i < p.Vertices; i++) {
                foreach (int j in p.Connection[i]) {
                    Console.WriteLine($"{i}, {j}");
                    PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
                }
            }

            foreach ((int i, int j) in p.Connection) { 
                PrecisionAssert.AreEqual(2, Vector3D.Distance(p.Vertex[i], p.Vertex[j]), 1e-30);
            }
        }
    }
}