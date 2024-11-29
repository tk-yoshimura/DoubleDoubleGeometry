using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

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
                Assert.IsTrue(Polygon2D.IsConvex(-Polygon2D.Regular(n)));

                Assert.IsFalse(Polygon2D.IsConcave(Polygon2D.Regular(n)));
                Assert.IsFalse(Polygon2D.IsConcave(-Polygon2D.Regular(n)));
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[2], vertex[3]) = (vertex[3], vertex[2]);

                Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
                Assert.IsFalse(Polygon2D.IsConcave(new Polygon2D(vertex)));
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
                        Assert.IsFalse(Polygon2D.IsConcave(new Polygon2D(vertex)));
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[i] *= -0.5;

                    Assert.IsFalse(Polygon2D.IsConvex(new Polygon2D(vertex)));
                    Assert.IsTrue(Polygon2D.IsConcave(new Polygon2D(vertex)));
                }
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        Assert.IsFalse(Polygon2D.IsConvex(-new Polygon2D(vertex)));
                        Assert.IsFalse(Polygon2D.IsConcave(-new Polygon2D(vertex)));
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[i] *= -0.5;

                    Assert.IsFalse(Polygon2D.IsConvex(-new Polygon2D(vertex)));
                    Assert.IsTrue(Polygon2D.IsConcave(-new Polygon2D(vertex)));
                }
            }
        }

        [TestMethod()]
        public void IsValidTest() {
            for (int n = 3; n <= 16; n++) {
                Assert.IsTrue(Polygon2D.IsValid(Polygon2D.Regular(n)), $"{n}");
                Assert.IsTrue(Polygon2D.IsValid(-Polygon2D.Regular(n)), $"{n}");
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[2], vertex[3]) = (vertex[3], vertex[2]);

                Assert.IsFalse(Polygon2D.IsValid(new Polygon2D(vertex)));
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[0], vertex[3]) = (vertex[3], vertex[0]);

                Assert.IsFalse(Polygon2D.IsValid(new Polygon2D(vertex)));
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        Assert.IsFalse(Polygon2D.IsValid(new Polygon2D(vertex)));
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[i] *= -0.5;

                    Assert.IsTrue(Polygon2D.IsValid(new Polygon2D(vertex)));
                }
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        Assert.IsFalse(Polygon2D.IsValid(-new Polygon2D(vertex)));
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[i] *= -0.5;

                    Assert.IsTrue(Polygon2D.IsValid(-new Polygon2D(vertex)));
                }
            }
        }

        [TestMethod()]
        public void AreaTest() {
            for (int n = 3; n <= 16; n++) {
                ddouble s = n * ddouble.SinPi(2 * ddouble.Rcp(n)) / 2;

                PrecisionAssert.AreEqual(s, Polygon2D.Regular(n).Area, 1e-30, $"{n}");
                PrecisionAssert.AreEqual(s, (-Polygon2D.Regular(n)).Area, 1e-30, $"{n}");

                PrecisionAssert.AreEqual(s, (Polygon2D.Regular(n) + (1, 2)).Area, 1e-30, $"{n}");
                PrecisionAssert.AreEqual(s, (-Polygon2D.Regular(n) + (1, 2)).Area, 1e-30, $"{n}");
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[2], vertex[3]) = (vertex[3], vertex[2]);

                PrecisionAssert.IsNaN(new Polygon2D(vertex).Area);
            }

            {
                Vector2D[] vertex = [.. Polygon2D.Regular(4).Vertex];

                (vertex[0], vertex[3]) = (vertex[3], vertex[0]);

                PrecisionAssert.IsNaN(new Polygon2D(vertex).Area);
            }

            for (int n = 5; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    for (int j = i + 1; j < n; j++) {
                        Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                        (vertex[i], vertex[j]) = (vertex[j], vertex[i]);

                        PrecisionAssert.IsNaN(new Polygon2D(vertex).Area);
                    }
                }
            }

            for (int n = 4; n <= 16; n++) {
                for (int i = 0; i < n; i++) {
                    Polygon2D p = Polygon2D.Regular(n);

                    Vector2D[] vertex = [.. p.Vertex];

                    ddouble s = p.Area - new Polygon2D(vertex[i], vertex[(i + 1) % n], vertex[i] * -0.5, vertex[(i + n - 1) % n]).Area;

                    vertex[i] *= -0.5;

                    PrecisionAssert.AreEqual(s, new Polygon2D(vertex).Area, 1e-30, $"{n}");
                    PrecisionAssert.AreEqual(s, (-new Polygon2D(vertex)).Area, 1e-30, $"{n}");

                    PrecisionAssert.AreEqual(s, (new Polygon2D(vertex) + (1, 2)).Area, 1e-30, $"{n}");
                    PrecisionAssert.AreEqual(s, (-new Polygon2D(vertex) + (1, 2)).Area, 1e-30, $"{n}");
                }
            }
        }


        [TestMethod()]
        public void PerimeterTest() {
            for (int n = 3; n <= 16; n++) {
                ddouble s = 2 * n * ddouble.SinPi(ddouble.Rcp(n));

                PrecisionAssert.AreEqual(s, Polygon2D.Regular(n).Perimeter, 1e-30, $"{n}");
                PrecisionAssert.AreEqual(s, (-Polygon2D.Regular(n)).Perimeter, 1e-30, $"{n}");

                PrecisionAssert.AreEqual(s, (Polygon2D.Regular(n) + (1, 2)).Perimeter, 1e-30, $"{n}");
                PrecisionAssert.AreEqual(s, (-Polygon2D.Regular(n) + (1, 2)).Perimeter, 1e-30, $"{n}");
            }
        }

        [TestMethod()]
        public void InsideTest() {
            Polygon2D t = new((0, 0), (0, 1), (1, 0));

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

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Matrix2D m = new double[,] { { 1, 2 }, { 3, 5 } };
            Vector2D s = (4, 6);

            Polygon2D t2 = m * t + s;

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest2() {
            Polygon2D t = Polygon2D.Regular(6);

            List<Vector2D> insides = [], outsides = [];

            for (int i = 0; i < 12; i++) {
                Complex c = Complex.FromPolarPi(ddouble.Sqrt(3) / 2 - 0.05, 2 * i * ddouble.Rcp(12));

                insides.Add((c.R, c.I));
            }

            for (int i = 0; i < 12; i++) {
                Complex c = Complex.FromPolarPi(ddouble.Sqrt(3) / 2 + 0.05, 2 * i * ddouble.Rcp(12));

                if ((i % 2) == 0) {
                    insides.Add((c.R, c.I));
                }
                else {
                    outsides.Add((c.R, c.I));
                }
            }

            for (int i = 0; i < 12; i++) {
                Complex c = Complex.FromPolarPi(0.995, 2 * i * ddouble.Rcp(12));

                if ((i % 2) == 0) {
                    insides.Add((c.R, c.I));
                }
                else {
                    outsides.Add((c.R, c.I));
                }
            }

            for (int i = 0; i < 12; i++) {
                Complex c = Complex.FromPolarPi(1.05, 2 * i * ddouble.Rcp(12));

                outsides.Add((c.R, c.I));
            }

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Matrix2D m = new double[,] { { 1, 2 }, { 3, 5 } };
            Vector2D s = (4, 6);

            Polygon2D t2 = m * t + s;

            foreach (Vector2D v in insides) {
                Assert.IsTrue(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector2D v in outsides) {
                Assert.IsFalse(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void InsideTest3() {
            for (int n = 5; n <= 10; n++) {
                for (int j = 0; j < n; j++) {
                    Vector2D[] vertex = [.. Polygon2D.Regular(n).Vertex];

                    vertex[j] *= 0.125;

                    Polygon2D t = new(vertex);

                    Assert.IsTrue(Polygon2D.IsConcave(t));

                    List<Vector2D> insides = [], outsides = [];

                    for (int i = 0; i < n; i++) {
                        insides.Add(vertex[i] * 0.95);
                        insides.Add(vertex[i] * 0.75);
                        outsides.Add(vertex[i] * 1.05);
                        outsides.Add(vertex[i] * 1.25);
                    }

                    foreach (Vector2D v in insides) {
                        Assert.IsTrue(t.Inside(v));
                    }

                    Assert.IsTrue(t.Inside(insides).All(b => b));

                    foreach (Vector2D v in outsides) {
                        Assert.IsFalse(t.Inside(v));
                    }

                    Assert.IsTrue(t.Inside(outsides).All(b => !b));

                    Matrix2D m = new double[,] { { 1, 2 }, { 3, 5 } };
                    Vector2D s = (4, 6);

                    Polygon2D t2 = m * t + s;

                    foreach (Vector2D v in insides) {
                        Assert.IsTrue(t2.Inside(m * v + s));
                    }

                    Assert.IsTrue(t2.Inside(insides.Select(v => m * v + s)).All(b => b));

                    foreach (Vector2D v in outsides) {
                        Assert.IsFalse(t2.Inside(m * v + s));
                    }

                    Assert.IsTrue(t2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
                }
            }
        }
    }
}