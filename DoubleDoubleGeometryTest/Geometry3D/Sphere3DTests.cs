using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Sphere3DTests {
        [TestMethod()]
        public void Sphere3DTest() {
            Sphere3D sphere1 = new((1, 2, 3), 4);
            Sphere3D sphere2 = Sphere3D.FromIntersection((3, 2, -1), (1, 3, -2), (3, -1, -4), (0, 0, -2));
            Sphere3D sphere3 = Sphere3D.FromImplicit(sphere2.A, sphere2.B, sphere2.C, sphere2.D);

            Vector3DAssert.AreEqual((1, 2, 3), sphere1.Center, 1e-30);
            PrecisionAssert.AreEqual(4.0, sphere1.Radius, 1e-30);
            PrecisionAssert.AreEqual(4 * 4 * 4 * ddouble.Pi, sphere1.Area, 1e-30);
            PrecisionAssert.AreEqual(4.0 * 4 * 4 * 4 * ddouble.Pi / 3.0, sphere1.Volume, 1e-30);

            Vector3DAssert.AreEqual((2, 1, -3), sphere2.Center, 1e-30);
            PrecisionAssert.AreEqual(6, sphere2.Radius * sphere2.Radius, 1e-30);

            Vector3DAssert.AreEqual((2, 1, -3), sphere3.Center, 1e-30);
            PrecisionAssert.AreEqual(6, sphere3.Radius * sphere3.Radius, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Sphere3D((4, 5, 7), 3) == new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((4, 6, 7), 3) != new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((4, 5, 7), 4) != new Sphere3D((4, 5, 7), 3));
            Assert.IsTrue(new Sphere3D((-4, -5, 7), 3) != new Sphere3D((4, 5, 7), 3));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Sphere3D((4, 5, 1), 3), +(new Sphere3D((4, 5, 1), 3)));
            Assert.AreEqual(new Sphere3D((-4, -5, -1), 3), -(new Sphere3D((4, 5, 1), 3)));
            Assert.AreEqual(new Sphere3D((5, 9, 8), 3), new Sphere3D((4, 5, 1), 3) + (1, 4, 7));
            Assert.AreEqual(new Sphere3D((3, 1, -6), 3), new Sphere3D((4, 5, 1), 3) - (1, 4, 7));
            Assert.AreEqual(new Sphere3D((5, 9, 8), 3), (1, 4, 7) + new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((-3, -1, 6), 3), (1, 4, 7) - new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), new Sphere3D((4, 5, 1), 3) * (ddouble)2);
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), new Sphere3D((4, 5, 1), 3) * (double)2);
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), (ddouble)2 * new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((8, 10, 2), 6), (double)2 * new Sphere3D((4, 5, 1), 3));
            Assert.AreEqual(new Sphere3D((2, 2.5, 0.5), 1.5), new Sphere3D((4, 5, 1), 3) / (ddouble)2);
            Assert.AreEqual(new Sphere3D((2, 2.5, 0.5), 1.5), new Sphere3D((4, 5, 1), 3) / (double)2);
        }

        [TestMethod()]
        public void InsideTest() {
            Sphere3D t = new((0, 0, 0), 3);

            List<Vector3D> insides = [], outsides = [];

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    insides.Add(t.Point(u, v) * 0.95);
                    outsides.Add(t.Point(u, v) * 1.05);
                }
            }

            foreach (Vector3D v in insides) {
                Assert.IsTrue(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Quaternion q = (5, 6, -3, 6);
            Vector3D s = (4, 6, 2);

            Sphere3D t2 = q * t + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(t2.Inside(q * v + s));
            }

            Assert.IsTrue(t2.Inside(insides.Select(v => q * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(t2.Inside(q * v + s));
            }

            Assert.IsTrue(t2.Inside(outsides.Select(v => q * v + s)).All(b => !b));
        }

        [TestMethod()]
        public void PointTest() {
            Sphere3D sphere1 = new(Vector3D.Zero, 4);
            Sphere3D sphere2 = new Sphere3D(Vector3D.Zero, 4) * 2;
            Sphere3D sphere3 = new Sphere3D(Vector3D.Zero, 4) * -2;
            Sphere3D sphere4 = new((2, 3, 4), 4);

            Quaternion q = (1, 2, 3, 4);

            Sphere3D sphere5 = q * sphere4;
            Sphere3D sphere6 = Sphere3D.FromIntersection(q * sphere4.Point(0, 2), q * sphere4.Point(1, 1), q * sphere4.Point(1, 2), q * sphere4.Point(3, 3));

            Vector3DAssert.AreEqual((4, 0, 0), sphere1.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual((ddouble.Sqrt2 * 2, ddouble.Sqrt2 * 2, 0), sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual((0, 4, 0), sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);
            Vector3DAssert.AreEqual((0, 0, 4), sphere1.Point(0, 0), 1e-30);

            Vector3DAssert.AreEqual(sphere1.Point(0, 0) * 2, sphere2.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, 0) * 2, sphere2.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * 2, sphere2.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * 2, sphere2.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(sphere1.Point(0, 0) * -2, sphere3.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, 0) * -2, sphere3.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * -2, sphere3.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * -2, sphere3.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(sphere1.Point(0, 0) + (2, 3, 4), sphere4.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, 0) + (2, 3, 4), sphere4.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 4) + (2, 3, 4), sphere4.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(sphere1.Point(ddouble.Pi / 2, ddouble.Pi / 2) + (2, 3, 4), sphere4.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(sphere6.Center, sphere5.Center, 2e-29);
            PrecisionAssert.AreEqual(sphere6.Radius, sphere5.Radius, 2e-29);
        }

        [TestMethod()]
        public void ImplicitParamTest() {
            Sphere3D sphere1 = new(Vector3D.Zero, 4);
            Sphere3D sphere2 = new(Vector3D.Zero, 4);
            Sphere3D sphere3 = new((1, 2, 3), 4);

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = sphere1.Point(u, v);

                    ddouble s = x * x + y * y + z * z +
                        sphere1.A * x + sphere1.B * y + sphere1.C * z + sphere1.D;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = sphere2.Point(u, v);

                    ddouble s = x * x + y * y + z * z +
                        sphere2.A * x + sphere2.B * y + sphere2.C * z + sphere2.D;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = sphere3.Point(u, v);

                    ddouble s = x * x + y * y + z * z +
                        sphere3.A * x + sphere3.B * y + sphere3.C * z + sphere3.D;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = sphere3.Point(u, v);
                    (ddouble a, ddouble b, ddouble c, ddouble d) = sphere3.ImplicitParameter;

                    ddouble s = x * x + y * y + z * z + a * x + b * y + c * z + d;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Sphere3D.IsValid(new Sphere3D((1, 2, 3), 4)));
            Assert.IsFalse(Sphere3D.IsValid(Sphere3D.Invalid));
        }
    }
}