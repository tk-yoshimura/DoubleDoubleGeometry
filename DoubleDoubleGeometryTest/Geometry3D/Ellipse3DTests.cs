using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Ellipse3DTests {
        [TestMethod()]
        public void Ellipse3DTest() {
            Ellipse3D ellipse = new((1, 2, 7), (4, 3), Vector3D.Rot((0, 0, 1), (2, 3, 4)));

            Vector3DAssert.AreEqual((1, 2, 7), ellipse.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, 3, 4).Normal, ellipse.Normal, 1e-30);
            Assert.AreEqual(4d, ellipse.MajorAxis);
            Assert.AreEqual(3d, ellipse.MinorAxis);
            Assert.AreEqual(Vector3D.Rot((0, 0, 1), (2, 3, 4)).Normal, ellipse.Rotation);

            PrecisionAssert.AreEqual(4 * 3 * ddouble.Pi, ellipse.Area);
            PrecisionAssert.AreEqual("22.1034921607095050452855864638724607782783", ellipse.Perimeter, 1e-30);
            PrecisionAssert.AreEqual("2.64575131106459059050161575363926042571026", ellipse.Focus, 1e-30);
            PrecisionAssert.AreEqual("0.661437827766147647625403938409815106427565", ellipse.Eccentricity, 1e-30);
        }

        [TestMethod]
        public void EqualTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Ellipse3D ellipse1 = new((1, 2, 7), (4, 3), rot);
            Ellipse3D ellipse2 = new((1, 2, 7), (4, 3), rot);
            Ellipse3D ellipse3 = new((1, 2, 7), (4, 4), rot);
            Ellipse3D ellipse4 = new((1, 2, 7), (4, 4), rot);

            Assert.AreEqual(ellipse1, ellipse2);
            Assert.AreNotEqual(ellipse1, ellipse3);

            Assert.IsTrue(ellipse1 == ellipse2);
            Assert.IsTrue(ellipse1 != ellipse3);
            Assert.IsTrue(ellipse1 != ellipse4);
        }

        [TestMethod()]
        public void OperatorTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Assert.AreEqual(new Ellipse3D((1, 2, 7), (4, 3), rot), +(new Ellipse3D((1, 2, 7), (4, 3), rot)));
            Assert.AreEqual(new Ellipse3D((-1, -2, -7), (-4, -3), rot), -(new Ellipse3D((1, 2, 7), (4, 3), rot)));
            Assert.AreEqual(new Ellipse3D((2, 6, 12), (4, 3), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) + (1, 4, 5));
            Assert.AreEqual(new Ellipse3D((0, -2, 2), (4, 3), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) - (1, 4, 5));
            Assert.AreEqual(new Ellipse3D((2, 6, 12), (4, 3), rot), (1, 4, 5) + new Ellipse3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Ellipse3D((0, 2, -2), (-4, -3), rot), (1, 4, 5) - new Ellipse3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (8, 6), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) * (ddouble)2);
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (8, 6), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) * (double)2);
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (8, 6), rot), (ddouble)2 * new Ellipse3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Ellipse3D((2, 4, 14), (8, 6), rot), (double)2 * new Ellipse3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Ellipse3D((0.5, 1, 3.5), (2, 1.5), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) / (ddouble)2);
            Assert.AreEqual(new Ellipse3D((0.5, 1, 3.5), (2, 1.5), rot), new Ellipse3D((1, 2, 7), (4, 3), rot) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Ellipse3D ellipse1 = new(Vector3D.Zero, (4, 3), (0, 0, 1));
            Ellipse3D ellipse2 = new Ellipse3D(Vector3D.Zero, (4, 3), (0, 0, 1)) * 2;
            Ellipse3D ellipse3 = new Ellipse3D(Vector3D.Zero, (4, 3), (0, 0, 1)) * -2;
            Ellipse3D ellipse4 = new((2, 3, 4), (4, 3), (0, 0, 1));

            Quaternion q = (1, 2, 3, 4);

            Ellipse3D ellipse5 = q * new Ellipse3D((2, 3, 4), (20, 15), (0, 0, 1));

            Vector3DAssert.AreEqual((4, 0, 0), ellipse1.Point(0), 1e-30);
            Vector3DAssert.AreEqual((ddouble.Sqrt2 * 2, ddouble.Sqrt2 * 3 / 2, 0), ellipse1.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual((0, 3, 0), ellipse1.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipse1.Point(0) * 2, ellipse2.Point(0), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) * 2, ellipse2.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) * 2, ellipse2.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipse1.Point(0) * -2, ellipse3.Point(0), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) * -2, ellipse3.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) * -2, ellipse3.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(ellipse1.Point(0) + (2, 3, 4), ellipse4.Point(0), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 4) + (2, 3, 4), ellipse4.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(ellipse1.Point(ddouble.Pi / 2) + (2, 3, 4), ellipse4.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(q * (ellipse1.Point(0) * 5 + (2, 3, 4)), ellipse5.Point(0), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipse1.Point(ddouble.Pi / 4) * 5 + (2, 3, 4)), ellipse5.Point(ddouble.Pi / 4), 2e-29);
            Vector3DAssert.AreEqual(q * (ellipse1.Point(ddouble.Pi / 2) * 5 + (2, 3, 4)), ellipse5.Point(ddouble.Pi / 2), 2e-29);
        }

        [TestMethod()]
        public void BoundingBoxTest() {
            Ellipse3D ellipse1 = new((0, 0, 0), (8, 3), Quaternion.One);
            Ellipse3D ellipse2 = new((0, 0, 0), (8, 3), (0, 1, 0));
            Ellipse3D ellipse3 = new((0, 0, 0), (8, 3), (1, 0, 0));
            Ellipse3D ellipse4 = new((0, 0, 0), (5, 6), (3, -6, 4, 7));
            Ellipse3D ellipse5 = new((0, 0, 0), (8, 3), (1, -2, 3, 4));
            Ellipse3D ellipse6 = new((1, 2, 3), (8, 3), (1, -2, 3, 4));
            Ellipse3D ellipse7 = new((0, 0, 0), (8, 3), (3, 4, 1, -2));

            Vector3DAssert.AreEqual((8, 3, 0), ellipse1.BoundingBox.Scale, 1e-30);
            Vector3DAssert.AreEqual((8, 0, 3), ellipse2.BoundingBox.Scale, 1e-30);
            Vector3DAssert.AreEqual((0, 3, 8), ellipse3.BoundingBox.Scale, 1e-30);

            bool any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(ellipse4.BoundingBox.Inside(ellipse4.Point(t) * 0.9999));

                if (!ellipse4.BoundingBox.Inside(ellipse4.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(ellipse5.BoundingBox.Inside(ellipse5.Point(t) * 0.9999));

                if (!ellipse5.BoundingBox.Inside(ellipse5.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(ellipse6.BoundingBox.Inside(ellipse5.Point(t) * 0.9999 + (1, 2, 3)));

                if (!ellipse6.BoundingBox.Inside(ellipse6.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);

            any_outside = false;
            for (double t = 0; t < 8; t += 0.25) {
                Assert.IsTrue(ellipse7.BoundingBox.Inside(ellipse7.Point(t) * 0.9999));

                if (!ellipse7.BoundingBox.Inside(ellipse7.Point(t) * 1.01)) {
                    any_outside = true;
                }
            }

            Assert.IsTrue(any_outside);
        }

        [TestMethod()]
        public void ValidTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Assert.IsTrue(Ellipse3D.IsValid(new Ellipse3D((1, 2, 7), (4, 3), rot)));
            Assert.IsFalse(Ellipse3D.IsValid(Ellipse3D.Invalid));
        }
    }
}