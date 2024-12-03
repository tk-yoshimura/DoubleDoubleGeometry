using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry;
using DoubleDoubleGeometry.Geometry2D;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;
using System.Collections.ObjectModel;

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

            Assert.AreEqual(4, p.Connection.EnumerateCycle().Count());

            Assert.IsTrue(p.Connection.EnumerateCycle().All(f => f.Count == 3));

            for (int i = 0; i < p.Connection.EnumerateCycle().Count(); i++) {
                for (int j = i + 1; j < p.Connection.EnumerateCycle().Count(); j++) {
                    Assert.IsFalse(p.Connection.EnumerateCycle().ToArray()[i].Order().SequenceEqual(p.Connection.EnumerateCycle().ToArray()[j].Order()));
                }
            }

            Assert.IsTrue(p.Planes.All(plane => plane.plane.D < 0d));

            Assert.IsTrue(p.FaceFlatness.All(v => v < 1e-30));

            PrecisionAssert.AlmostEqual(p.Area, ddouble.Sqrt(3) * 8, 1e-30);
            PrecisionAssert.AlmostEqual(p.Volume, 8 * ddouble.Rcp(3), 1e-30);
            PrecisionAssert.AlmostEqual((p + (1, 2, 3)).Volume, 8 * ddouble.Rcp(3), 1e-30);
            PrecisionAssert.AlmostEqual((-p).Volume, 8 * ddouble.Rcp(3), 1e-30);
        }

        [TestMethod()]
        public void CubeTest() {
            Polyhedron3D p = Polyhedron3D.Cube;

            Assert.AreEqual(8, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            Assert.IsTrue(Connection.IsValid(p.Connection));

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

            Assert.AreEqual(6, p.Connection.EnumerateCycle().Count());

            Assert.IsTrue(p.Connection.EnumerateCycle().All(f => f.Count == 4));

            for (int i = 0; i < p.Connection.EnumerateCycle().Count(); i++) {
                for (int j = i + 1; j < p.Connection.EnumerateCycle().Count(); j++) {
                    Assert.IsFalse(p.Connection.EnumerateCycle().ToArray()[i].Order().SequenceEqual(p.Connection.EnumerateCycle().ToArray()[j].Order()));
                }
            }

            Assert.IsTrue(p.Planes.All(plane => plane.plane.D < 0d));

            Assert.IsTrue(p.FaceFlatness.All(v => v < 1e-30));

            PrecisionAssert.AlmostEqual(p.Area, 2 * 2 * 6, 1e-30);
            PrecisionAssert.AlmostEqual(p.Volume, 8, 1e-30);
            PrecisionAssert.AlmostEqual((p + (1, 2, 3)).Volume, 8, 1e-30);
            PrecisionAssert.AlmostEqual((-p).Volume, 8, 1e-30);
        }

        [TestMethod()]
        public void OctahedronTest() {
            Polyhedron3D p = Polyhedron3D.Octahedron;

            Assert.AreEqual(6, p.Vertices);
            Assert.AreEqual(12, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(4, p.Connection[i].Count);
            }

            Assert.IsTrue(Connection.IsValid(p.Connection));

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

            Assert.AreEqual(8, p.Connection.EnumerateCycle().Count());

            Assert.IsTrue(p.Connection.EnumerateCycle().All(f => f.Count == 3));

            for (int i = 0; i < p.Connection.EnumerateCycle().Count(); i++) {
                for (int j = i + 1; j < p.Connection.EnumerateCycle().Count(); j++) {
                    Assert.IsFalse(p.Connection.EnumerateCycle().ToArray()[i].Order().SequenceEqual(p.Connection.EnumerateCycle().ToArray()[j].Order()));
                }
            }

            Assert.IsTrue(p.Planes.All(plane => plane.plane.D < 0d));

            Assert.IsTrue(p.FaceFlatness.All(v => v < 1e-30));

            PrecisionAssert.AlmostEqual(p.Area, 4 * ddouble.Sqrt(3), 1e-30);
            PrecisionAssert.AlmostEqual(p.Volume, 4 * ddouble.Rcp(3), 1e-30);
            PrecisionAssert.AlmostEqual((p + (1, 2, 3)).Volume, 4 * ddouble.Rcp(3), 1e-30);
            PrecisionAssert.AlmostEqual((-p).Volume, 4 * ddouble.Rcp(3), 1e-30);
        }

        [TestMethod()]
        public void DodecahedronTest() {
            Polyhedron3D p = Polyhedron3D.Dodecahedron;

            Assert.AreEqual(20, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(3, p.Connection[i].Count);
            }

            Assert.IsTrue(Connection.IsValid(p.Connection));

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

            Assert.AreEqual(12, p.Connection.EnumerateCycle().Count());

            Assert.IsTrue(p.Connection.EnumerateCycle().All(f => f.Count == 5));

            for (int i = 0; i < p.Connection.EnumerateCycle().Count(); i++) {
                for (int j = i + 1; j < p.Connection.EnumerateCycle().Count(); j++) {
                    Assert.IsFalse(p.Connection.EnumerateCycle().ToArray()[i].Order().SequenceEqual(p.Connection.EnumerateCycle().ToArray()[j].Order()));
                }
            }

            Assert.IsTrue(p.Planes.All(plane => plane.plane.D < 0d));

            Assert.IsTrue(p.FaceFlatness.All(v => v < 1e-30));

            PrecisionAssert.AlmostEqual(p.Area, 3 * ddouble.Sqrt(5 * (4 * ddouble.GoldenRatio + 3)) * ddouble.Square(4 / (3 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual(p.Volume, (15 + 7 * ddouble.Sqrt(5)) / 4 * ddouble.Cube(4 / (3 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual((p + (1, 2, 3)).Volume, (15 + 7 * ddouble.Sqrt(5)) / 4 * ddouble.Cube(4 / (3 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual((-p).Volume, (15 + 7 * ddouble.Sqrt(5)) / 4 * ddouble.Cube(4 / (3 + ddouble.Sqrt(5))), 1e-30);

        }

        [TestMethod()]
        public void IcosahedronTest() {
            Polyhedron3D p = Polyhedron3D.Icosahedron;

            Assert.AreEqual(12, p.Vertices);
            Assert.AreEqual(30, p.Edges);

            for (int i = 0; i < p.Vertices; i++) {
                Assert.AreEqual(5, p.Connection[i].Count);
            }

            Console.WriteLine(p.BoundingBox);

            Assert.IsTrue(Connection.IsValid(p.Connection));

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

            Assert.AreEqual(20, p.Connection.EnumerateCycle().Count());

            Assert.IsTrue(p.Connection.EnumerateCycle().All(f => f.Count == 3));

            for (int i = 0; i < p.Connection.EnumerateCycle().Count(); i++) {
                for (int j = i + 1; j < p.Connection.EnumerateCycle().Count(); j++) {
                    Assert.IsFalse(p.Connection.EnumerateCycle().ToArray()[i].Order().SequenceEqual(p.Connection.EnumerateCycle().ToArray()[j].Order()));
                }
            }

            Assert.IsTrue(p.Planes.All(plane => plane.plane.D < 0d));

            Assert.IsTrue(p.FaceFlatness.All(v => v < 1e-30));

            PrecisionAssert.AlmostEqual(p.Area, 5 * ddouble.Sqrt(3) * ddouble.Square(4 / (1 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual(p.Volume, 5 * (3 + ddouble.Sqrt(5)) / 12 * ddouble.Cube(4 / (1 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual((p + (1, 2, 3)).Volume, 5 * (3 + ddouble.Sqrt(5)) / 12 * ddouble.Cube(4 / (1 + ddouble.Sqrt(5))), 1e-30);
            PrecisionAssert.AlmostEqual((-p).Volume, 5 * (3 + ddouble.Sqrt(5)) / 12 * ddouble.Cube(4 / (1 + ddouble.Sqrt(5))), 1e-30);
        }

        [TestMethod()]
        public void IsConvexTest() {
            Assert.IsTrue(Polyhedron3D.IsConvex(Polyhedron3D.Tetrahedron));
            Assert.IsTrue(Polyhedron3D.IsConvex(-Polyhedron3D.Tetrahedron));

            Assert.IsTrue(Polyhedron3D.IsConvex(Polyhedron3D.Cube));
            Assert.IsTrue(Polyhedron3D.IsConvex(-Polyhedron3D.Cube));

            Assert.IsTrue(Polyhedron3D.IsConvex(Polyhedron3D.Octahedron));
            Assert.IsTrue(Polyhedron3D.IsConvex(-Polyhedron3D.Octahedron));

            Assert.IsTrue(Polyhedron3D.IsConvex(Polyhedron3D.Dodecahedron));
            Assert.IsTrue(Polyhedron3D.IsConvex(-Polyhedron3D.Dodecahedron));

            Assert.IsTrue(Polyhedron3D.IsConvex(Polyhedron3D.Icosahedron));
            Assert.IsTrue(Polyhedron3D.IsConvex(-Polyhedron3D.Icosahedron));

            {
                Vector3D[] vertex = [.. Polyhedron3D.Octahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Octahedron.Connection, vertex_copy);

                    Assert.IsFalse(Polyhedron3D.IsConvex(g));
                    Assert.IsFalse(Polyhedron3D.IsConvex(-g));
                }
            }

            {
                Vector3D[] vertex = [.. Polyhedron3D.Icosahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Icosahedron.Connection, vertex_copy);

                    Assert.IsFalse(Polyhedron3D.IsConvex(g));
                    Assert.IsFalse(Polyhedron3D.IsConvex(-g));
                }
            }
        }

        [TestMethod()]
        public void IsConcaveTest() {
            Assert.IsFalse(Polyhedron3D.IsConcave(Polyhedron3D.Tetrahedron));
            Assert.IsFalse(Polyhedron3D.IsConcave(-Polyhedron3D.Tetrahedron));

            Assert.IsFalse(Polyhedron3D.IsConcave(Polyhedron3D.Cube));
            Assert.IsFalse(Polyhedron3D.IsConcave(-Polyhedron3D.Cube));

            Assert.IsFalse(Polyhedron3D.IsConcave(Polyhedron3D.Octahedron));
            Assert.IsFalse(Polyhedron3D.IsConcave(-Polyhedron3D.Octahedron));

            Assert.IsFalse(Polyhedron3D.IsConcave(Polyhedron3D.Dodecahedron));
            Assert.IsFalse(Polyhedron3D.IsConcave(-Polyhedron3D.Dodecahedron));

            Assert.IsFalse(Polyhedron3D.IsConcave(Polyhedron3D.Icosahedron));
            Assert.IsFalse(Polyhedron3D.IsConcave(-Polyhedron3D.Icosahedron));

            {
                Vector3D[] vertex = [.. Polyhedron3D.Octahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Octahedron.Connection, vertex_copy);

                    Assert.IsTrue(Polyhedron3D.IsConcave(g));
                    Assert.IsTrue(Polyhedron3D.IsConcave(-g));
                }
            }

            {
                Vector3D[] vertex = [.. Polyhedron3D.Icosahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Icosahedron.Connection, vertex_copy);

                    Assert.IsTrue(Polyhedron3D.IsConcave(g));
                    Assert.IsTrue(Polyhedron3D.IsConcave(-g));
                }
            }

            {
                Vector3D[] vertex = [.. Polyhedron3D.Octahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -1.5;

                    Polyhedron3D g = new(Polyhedron3D.Octahedron.Connection, vertex_copy);

                    Assert.IsTrue(Polyhedron3D.IsConcave(g));
                    Assert.IsTrue(Polyhedron3D.IsConcave(-g));
                }
            }

            {
                Vector3D[] vertex = [.. Polyhedron3D.Icosahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -1.5;

                    Polyhedron3D g = new(Polyhedron3D.Icosahedron.Connection, vertex_copy);

                    Assert.IsFalse(Polyhedron3D.IsConcave(g));
                    Assert.IsFalse(Polyhedron3D.IsConcave(-g));
                }
            }
        }

        [TestMethod()]
        public void PolygonsTest() {
            Polyhedron3D p = Polyhedron3D.Dodecahedron;

            Polygon3D[] polygons = [.. p.Polygons];

            foreach ((Polygon3D polygon, ReadOnlyCollection<int> face) in polygons.Zip(p.Faces)) {
                for (int i = 0; i < face.Count; i++) {
                    Vector3D actual = polygon.Vertex[i];
                    Vector3D expected = p.Vertex[face[i]];

                    Vector3DAssert.AreEqual(expected, actual, 1e-30);
                }
            }
        }

        [TestMethod()]
        public void InsideTest() {
            Polyhedron3D p = Polyhedron3D.Dodecahedron;

            List<Vector3D> insides = [], outsides = [];

            foreach (Polygon3D polygon in p.Polygons) {
                insides.Add(polygon.Center * 0.95);
                insides.Add(polygon.Center * 0.75);

                outsides.Add(polygon.Center * 1.05);
                outsides.Add(polygon.Center * 1.25);

                foreach (Vector3D v in polygon.Vertex) {
                    insides.Add((polygon.Center + v) / 2 * 0.95);
                    insides.Add((polygon.Center + v) / 2 * 0.75);

                    outsides.Add((polygon.Center + v) / 2 * 1.05);
                    outsides.Add((polygon.Center + v) / 2 * 1.25);

                    insides.Add((polygon.Center * 3 + v) / 4 * 0.95);
                    insides.Add((polygon.Center * 3 + v) / 4 * 0.75);

                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.05);
                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.25);

                    insides.Add((polygon.Center + v * 3) / 4 * 0.95);
                    insides.Add((polygon.Center + v * 3) / 4 * 0.75);

                    outsides.Add((polygon.Center + v * 3) / 4 * 1.05);
                    outsides.Add((polygon.Center + v * 3) / 4 * 1.25);
                }
            }

            foreach (Vector3D v in p.Vertex) {
                insides.Add(v * 0.95);
                insides.Add(v * 0.75);

                outsides.Add(v * 1.05);
                outsides.Add(v * 1.25);
            }

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Polyhedron3D p2 = m * p + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest2() {
            Polyhedron3D p = Polyhedron3D.Icosahedron;

            List<Vector3D> insides = [], outsides = [];

            foreach (Polygon3D polygon in p.Polygons) {
                insides.Add(polygon.Center * 0.95);
                insides.Add(polygon.Center * 0.75);

                outsides.Add(polygon.Center * 1.05);
                outsides.Add(polygon.Center * 1.25);

                foreach (Vector3D v in polygon.Vertex) {
                    insides.Add((polygon.Center + v) / 2 * 0.95);
                    insides.Add((polygon.Center + v) / 2 * 0.75);

                    outsides.Add((polygon.Center + v) / 2 * 1.05);
                    outsides.Add((polygon.Center + v) / 2 * 1.25);

                    insides.Add((polygon.Center * 3 + v) / 4 * 0.95);
                    insides.Add((polygon.Center * 3 + v) / 4 * 0.75);

                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.05);
                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.25);

                    insides.Add((polygon.Center + v * 3) / 4 * 0.95);
                    insides.Add((polygon.Center + v * 3) / 4 * 0.75);

                    outsides.Add((polygon.Center + v * 3) / 4 * 1.05);
                    outsides.Add((polygon.Center + v * 3) / 4 * 1.25);
                }
            }

            foreach (Vector3D v in p.Vertex) {
                insides.Add(v * 0.95);
                insides.Add(v * 0.75);

                outsides.Add(v * 1.05);
                outsides.Add(v * 1.25);
            }

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Polyhedron3D p2 = m * p + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest3() {
            Vector3D[] vertex = [.. Polyhedron3D.Icosahedron.Vertex];

            vertex[0] *= 0.25;

            Polyhedron3D p = new(Polyhedron3D.Icosahedron.Connection, vertex);

            Assert.IsFalse(Polyhedron3D.IsConvex(p));

            List<Vector3D> insides = [], outsides = [];

            foreach (Polygon3D polygon in p.Polygons) {
                insides.Add(polygon.Center * 0.95);
                insides.Add(polygon.Center * 0.75);

                outsides.Add(polygon.Center * 1.05);
                outsides.Add(polygon.Center * 1.25);

                foreach (Vector3D v in polygon.Vertex) {
                    insides.Add((polygon.Center + v) / 2 * 0.95);
                    insides.Add((polygon.Center + v) / 2 * 0.75);

                    outsides.Add((polygon.Center + v) / 2 * 1.05);
                    outsides.Add((polygon.Center + v) / 2 * 1.25);

                    insides.Add((polygon.Center * 3 + v) / 4 * 0.95);
                    insides.Add((polygon.Center * 3 + v) / 4 * 0.75);

                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.05);
                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.25);

                    insides.Add((polygon.Center + v * 3) / 4 * 0.95);
                    insides.Add((polygon.Center + v * 3) / 4 * 0.75);

                    outsides.Add((polygon.Center + v * 3) / 4 * 1.05);
                    outsides.Add((polygon.Center + v * 3) / 4 * 1.25);
                }
            }

            foreach (Vector3D v in p.Vertex.Skip(1)) {
                insides.Add(v * 0.95);
                insides.Add(v * 0.75);

                outsides.Add(v * 1.05);
                outsides.Add(v * 1.25);
            }

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Polyhedron3D p2 = m * p + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest4() {
            Polyhedron3D p = new(
                new Connection(12,
                    (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 0),
                    (6, 7), (7, 8), (8, 9), (9, 10), (10, 11), (11, 6),
                    (0, 6), (1, 7), (2, 8), (3, 9), (4, 10), (5, 11)
                ),
                (-1, 1, -1), (0, 1, -1), (0.25, 0.25, -1), (1, 0, -1), (1, -1, -1), (-1, -1, -1),
                (-1, 1, +1), (0, 1, +1), (0.25, 0.25, +1), (1, 0, +1), (1, -1, +1), (-1, -1, +1)
            );

            Assert.IsTrue(Polyhedron3D.IsConcave(p));

            PrecisionAssert.AreEqual(6.5, p.Volume);

            Assert.AreEqual(8, p.Faces.Count);

            List<Vector3D> insides = [], outsides = [];

            foreach (Polygon3D polygon in p.Polygons) {
                insides.Add(polygon.Center * 0.95);
                insides.Add(polygon.Center * 0.75);

                outsides.Add(polygon.Center * 1.05);
                outsides.Add(polygon.Center * 1.25);

                foreach (Vector3D v in polygon.Vertex) {
                    insides.Add((polygon.Center + v) / 2 * 0.95);
                    insides.Add((polygon.Center + v) / 2 * 0.75);

                    outsides.Add((polygon.Center + v) / 2 * 1.05);
                    outsides.Add((polygon.Center + v) / 2 * 1.25);

                    insides.Add((polygon.Center * 3 + v) / 4 * 0.95);
                    insides.Add((polygon.Center * 3 + v) / 4 * 0.75);

                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.05);
                    outsides.Add((polygon.Center * 3 + v) / 4 * 1.25);

                    insides.Add((polygon.Center + v * 3) / 4 * 0.95);
                    insides.Add((polygon.Center + v * 3) / 4 * 0.75);

                    outsides.Add((polygon.Center + v * 3) / 4 * 1.05);
                    outsides.Add((polygon.Center + v * 3) / 4 * 1.25);
                }
            }

            foreach (Vector3D v in p.Vertex.Skip(1)) {
                insides.Add(v * 0.95);
                insides.Add(v * 0.75);

                outsides.Add(v * 1.05);
                outsides.Add(v * 1.25);
            }

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Polyhedron3D p2 = m * p + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest5() {
            Polyhedron3D p = new(
                new Connection(16,
                    new Cycle(0, 1, 5, 4),
                    new Cycle(4, 5, 9, 8),
                    new Cycle(8, 9, 13, 12),
                    new Cycle(12, 13, 1, 0),
                    new Cycle(1, 2, 6, 5),
                    new Cycle(5, 6, 10, 9),
                    new Cycle(9, 10, 14, 13),
                    new Cycle(13, 14, 2, 1),
                    //new Cycle(2, 3, 7, 6),
                    new Cycle(2, 6, 7, 3),
                    new Cycle(6, 7, 11, 10),
                    new Cycle(10, 11, 15, 14),
                    new Cycle(14, 15, 3, 2),
                    new Cycle(3, 0, 4, 7),
                    new Cycle(7, 4, 8, 11),
                    new Cycle(11, 8, 12, 15),
                    new Cycle(15, 12, 0, 3)
                ),
                (0, 1, 0), (0, 2, -1), (0, 3, 0), (0, 2, 1),
                (1, 0, 0), (2, 0, -1), (3, 0, 0), (2, 0, 1),
                (0, -1, 0), (0, -2, -1), (0, -3, 0), (0, -2, 1),
                (-1, 0, 0), (-2, 0, -1), (-3, 0, 0), (-2, 0, 1)
            );

            int n = p.Vertices;
            bool[,] matrix = p.Connection.AdjacencyMatrix;

            Assert.IsTrue(Polyhedron3D.IsConcave(p));

            PrecisionAssert.AreEqual(32, p.Edges);
            PrecisionAssert.AreEqual(16, p.Volume);
            Assert.AreEqual(16, p.Faces.Count);

            List<Vector3D> insides = [], outsides = [];

            insides.Add((0, 1.125, 0));
            insides.Add((0, 2, -0.875));
            insides.Add((0, 2.875, 0));
            insides.Add((0, 2, 0.875));

            insides.Add((1.125, 0, 0));
            insides.Add((2, 0, -0.875));
            insides.Add((2.875, 0, 0));
            insides.Add((2, 0, 0.875));

            insides.Add((0, -1.125, 0));
            insides.Add((0, -2, -0.875));
            insides.Add((0, -2.875, 0));
            insides.Add((0, -2, 0.875));

            insides.Add((-1.125, 0, 0));
            insides.Add((-2, 0, -0.875));
            insides.Add((-2.875, 0, 0));
            insides.Add((-2, 0, 0.875));

            outsides.Add((0, 0.875, 0));
            outsides.Add((0, 2, -1.125));
            outsides.Add((0, 3.125, 0));
            outsides.Add((0, 2, 1.125));

            outsides.Add((0.875, 0, 0));
            outsides.Add((2, 0, -1.125));
            outsides.Add((3.125, 0, 0));
            outsides.Add((2, 0, 1.125));

            outsides.Add((0, -0.875, 0));
            outsides.Add((0, -2, -1.125));
            outsides.Add((0, -3.125, 0));
            outsides.Add((0, -2, 1.125));

            outsides.Add((-0.875, 0, 0));
            outsides.Add((-2, 0, -1.125));
            outsides.Add((-3.125, 0, 0));
            outsides.Add((-2, 0, 1.125));

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p.Inside(v));
            }

            Assert.IsTrue(p.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Polyhedron3D p2 = m * p + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(p2.Inside(m * v + s));
            }

            Assert.IsTrue(p2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void ConcaveVolumeTest() {
            Polyhedron3D p = Polyhedron3D.Icosahedron;

            Console.WriteLine(p.Volume);

            Vector3D[] vs = p.Connection[0].Select(index => p.Vertex[index]).ToArray();

            Vector3D[] vertex = [.. p.Vertex];
            int[] nodes = [.. p.Connection[0]];

            for (int theta = 0; theta < 32; theta++) {
                Vector3D v = (0, 0.75 * ddouble.CosPi(theta / 16d), 0.75 * ddouble.SinPi(theta / 16d));

                vertex[0] = v;

                Polyhedron3D p2 = new(new Connection(7,
                    (0, 1), (0, 2), (0, 3), (0, 4), (0, 5),
                    (1, 2), (2, 3), (3, 4), (4, 5), (5, 1),
                    (6, 1), (6, 2), (6, 3), (6, 4), (6, 5)), [p.Vertex[0], .. vs, v]
                );

                Polyhedron3D p3 = new(p.Connection, vertex);

                PrecisionAssert.AreEqual(p.Volume, p2.Volume + p3.Volume, 1e-30);
                PrecisionAssert.AreEqual(p.Volume, (p2 + (1, 2, 3)).Volume + p3.Volume, 1e-30);
                PrecisionAssert.AreEqual(p.Volume, (-p2).Volume + p3.Volume, 1e-30);
            }
        }

        [TestMethod()]
        public void VertexSortTest() {
            Polyhedron3D p = Polyhedron3D.Octahedron;

            Quaternion rot = Vector3D.Rot(p.Vertex[0], (0, 0, 1));

            Polyhedron3D q = rot * p;

            List<int> selected = [];
            List<int> index_order = [];

            while (selected.Count < q.Vertices) {
                ddouble z = q.Vertex.Select((v, i) => (v, i)).Where(item => !selected.Contains(item.i)).Select(item => item.v.Z).Max();

                int[] indexes = q.Vertex.Select((v, i) => (v, i)).Where(item => ddouble.Abs(item.v.Z - z) < 0.05).Select(item => item.i).ToArray();
                Vector3D[] vertex = indexes.Select(i => q.Vertex[i]).ToArray();

                int[] indexes_sorted = vertex.Select((v, i) => (i, phase: ddouble.Atan2Pi(v.Y, v.X))).OrderBy(item => item.phase).Select(item => item.i).ToArray();

                selected.AddRange(indexes);

                index_order.AddRange(indexes_sorted.Select(i => indexes[i]));
            }

            int[] index_perm = new int[p.Vertices];

            for (int i = 0; i < index_order.Count; i++) {
                index_perm[index_order[i]] = i;
            }

            foreach ((int i, int j) in p.Connection) {
                Console.WriteLine($"({int.Min(index_perm[i], index_perm[j])}, {int.Max(index_perm[i], index_perm[j])}), ");
            }

            foreach (int i in index_order) {
                Console.WriteLine(p.Vertex[i]);
            }
        }

        [TestMethod()]
        public void AdjacencyMatrixTest() {
            Polyhedron3D p = Polyhedron3D.Tetrahedron;

            int n = p.Vertices;
            bool[,] matrix = p.Connection.AdjacencyMatrix;

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    Console.Write($"{(matrix[i, j] ? "■" : "□")}");
                }

                Console.Write("\n");
            }
        }
    }
}