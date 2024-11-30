using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry;
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
                Vector3D[] vertex = [.. Polyhedron3D.Cube.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Cube.Connection, vertex_copy);

                    Assert.IsFalse(Polyhedron3D.IsConvex(g));
                    Assert.IsFalse(Polyhedron3D.IsConvex(-g));
                }
            }

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
                Vector3D[] vertex = [.. Polyhedron3D.Dodecahedron.Vertex];

                for (int i = 0; i < vertex.Length; i++) {
                    Vector3D[] vertex_copy = (Vector3D[])vertex.Clone();

                    vertex_copy[i] *= -0.5;

                    Polyhedron3D g = new(Polyhedron3D.Dodecahedron.Connection, vertex_copy);

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
    }
}