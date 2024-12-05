using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Ellipsoid3DTests {
        [TestMethod()]
        public void Ellipsoid3DTest() {
            Ellipsoid3D ellipsoid = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));

            Vector3DAssert.AreEqual((1, 2, 7), ellipsoid.Center, 1e-30);
            Vector3DAssert.AreEqual((2, 3, 4), ellipsoid.Axis, 1e-30);
            Assert.IsTrue((new Quaternion(4, 3, 5, 6).Normal - ellipsoid.Rotation).Norm < 1e-30);

            PrecisionAssert.AreEqual("100.53096491487338363080458826494409229", ellipsoid.Volume, 1e-29);
            PrecisionAssert.AreEqual("111.54576989401032252593683948373857471", ellipsoid.Area, 1e-29);
        }

        [TestMethod]
        public void EqualTest() {
            Ellipsoid3D ellipsoid1 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid2 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid3 = new((1, 3, 7), (2, 3, 4), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid4 = new((1, 2, 7), (2, 3, 5), (4, 3, 5, 6));
            Ellipsoid3D ellipsoid5 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 7));

            Assert.AreEqual(ellipsoid1, ellipsoid2);
            Assert.AreNotEqual(ellipsoid1, ellipsoid3);

            Assert.IsTrue(ellipsoid1 == ellipsoid2);
            Assert.IsTrue(ellipsoid1 != ellipsoid3);
            Assert.IsTrue(ellipsoid1 != ellipsoid4);
            Assert.IsTrue(ellipsoid1 != ellipsoid5);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)), +(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Ellipsoid3D((-1, -2, -7), (2, 3, 4), (4, 3, 5, 6)), -(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Ellipsoid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) + (1, 4, 5));
            Assert.AreEqual(new Ellipsoid3D((0, -2, 2), (2, 3, 4), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) - (1, 4, 5));
            Assert.AreEqual(new Ellipsoid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) + new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((0, 2, -2), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) - new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (ddouble)2);
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (double)2);
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (ddouble)2 * new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (double)2 * new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Ellipsoid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (ddouble)2);
            Assert.AreEqual(new Ellipsoid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (double)2);
        }

        [TestMethod()]
        public void InsideTest() {
            Ellipsoid3D t = new((0, 0, 0), (2, 3, 4), Quaternion.One);

            List<Vector3D> insides = [], outsides = [];

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    insides.Add(t.Point(u, v) * 0.95);
                    outsides.Add(t.Point(u, v) * 1.05);
                }
            }

            foreach (Vector3D v in insides) {
                Console.WriteLine(v);

                Assert.IsTrue(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Quaternion q = (5, 6, -3, 6);
            Vector3D s = (4, 6, 2);

            Ellipsoid3D t2 = q * t + s;

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
            Ellipsoid3D ellipsoid1 = new(Vector3D.Zero, (4, 3, 5), Quaternion.One);
            Ellipsoid3D ellipsoid2 = new Ellipsoid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * 2;
            Ellipsoid3D ellipsoid3 = new Ellipsoid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * -2;
            Ellipsoid3D ellipsoid4 = new((2, 3, 4), (4, 3, 5), Quaternion.One);

            Quaternion q = (1, 2, 3, 4);

            Ellipsoid3D ellipsoid5 = q * new Ellipsoid3D((2, 3, 4), (20, 15, 25), Quaternion.One);

            Vector3DAssert.AreEqual((4, 0, 0), ellipsoid1.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual((ddouble.Sqrt2 * 2, ddouble.Sqrt2 * 3 / 2, 0), ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual((0, 3, 0), ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);
            Vector3DAssert.AreEqual((0, 0, 5), ellipsoid1.Point(0, 0), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) * 2, ellipsoid2.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) * 2, ellipsoid2.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * 2, ellipsoid2.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * 2, ellipsoid2.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) * -2, ellipsoid3.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) * -2, ellipsoid3.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * -2, ellipsoid3.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * -2, ellipsoid3.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipsoid1.Point(0, 0) + (2, 3, 4), ellipsoid4.Point(0, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, 0) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, 0), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) + (2, 3, 4), ellipsoid4.Point(ddouble.Pi / 2, ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(0, 0) * 5 + (2, 3, 4)), ellipsoid5.Point(0, 0), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, 0) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, 0), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 4) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, ddouble.Pi / 4), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipsoid1.Point(ddouble.Pi / 2, ddouble.Pi / 2) * 5 + (2, 3, 4)), ellipsoid5.Point(ddouble.Pi / 2, ddouble.Pi / 2), 2e-29);
        }

        [TestMethod()]
        public void ImplicitParamTest() {
            Ellipsoid3D ellipsoid1 = new(Vector3D.Zero, (4, 3, 5), Quaternion.One);
            Ellipsoid3D ellipsoid2 = new(Vector3D.Zero, (4, 3, 5), (1, -2, 3, -4));
            Ellipsoid3D ellipsoid3 = new((1, 2, 3), (4, 3, 5), (1, -2, 3, -4));

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = ellipsoid1.Point(u, v);

                    ddouble s =
                        ellipsoid1.A * x * x + ellipsoid1.B * y * y + ellipsoid1.C * z * z
                        + ellipsoid1.D * x * y + ellipsoid1.E * x * z + ellipsoid1.F * y * z + ellipsoid1.J;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = ellipsoid2.Point(u, v);

                    ddouble s =
                        ellipsoid2.A * x * x + ellipsoid2.B * y * y + ellipsoid2.C * z * z
                        + ellipsoid2.D * x * y + ellipsoid2.E * x * z + ellipsoid2.F * y * z + ellipsoid2.J;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = ellipsoid3.Point(u, v);

                    ddouble s =
                        ellipsoid3.A * x * x + ellipsoid3.B * y * y + ellipsoid3.C * z * z
                        + ellipsoid3.D * x * y + ellipsoid3.E * x * z + ellipsoid3.F * y * z
                        + ellipsoid3.G * x + ellipsoid3.H * y + ellipsoid3.I * z
                        + ellipsoid3.J;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }

            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    (ddouble x, ddouble y, ddouble z) = ellipsoid3.Point(u, v);
                    (ddouble a, ddouble b, ddouble c, ddouble d, ddouble e, ddouble f, ddouble g, ddouble h, ddouble i, ddouble j) = ellipsoid3.ImplicitParameter;

                    ddouble s =
                        a * x * x + b * y * y + c * z * z
                        + d * x * y + e * x * z + f * y * z
                        + g * x + h * y + i * z
                        + j;

                    PrecisionAssert.AreEqual(0, s, 1e-25);
                }
            }
        }

        [TestMethod()]
        public void BoundingBoxTest() {
            Ellipsoid3D ellipsoid1 = new((0, 0, 0), (8, 3, 4), Quaternion.One);
            Ellipsoid3D ellipsoid2 = new((0, 0, 0), (8, 3, 4), (1, -2, 3, 4));
            Ellipsoid3D ellipsoid3 = new((1, 2, 3), (8, 3, 4), (1, -2, 3, 4));

            Vector3DAssert.AreEqual((8, 3, 4), ellipsoid1.BoundingBox.Scale, 1e-30);

            bool any_outside = false;
            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    Assert.IsTrue(ellipsoid2.BoundingBox.Inside(ellipsoid2.Point(u, v) * 0.9999));

                    if (!ellipsoid2.BoundingBox.Inside(ellipsoid2.Point(u, v) * 1.01)) {
                        any_outside = true;
                    }
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double u = 0; u < 8; u += 0.25) {
                for (double v = 0; v < 8; v += 0.25) {
                    Assert.IsTrue(ellipsoid3.BoundingBox.Inside(ellipsoid2.Point(u, v) * 0.9999 + (1, 2, 3)));

                    if (!ellipsoid3.BoundingBox.Inside(ellipsoid3.Point(u, v) * 1.01)) {
                        any_outside = true;
                    }
                }
            }

            Assert.IsTrue(any_outside);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Ellipsoid3D.IsValid(new Ellipsoid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.IsFalse(Ellipsoid3D.IsValid(Ellipsoid3D.Invalid));
        }
    }
}