using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Segment3DTests {
        [TestMethod()]
        public void Segment3DTest() {
            Segment3D segment1 = new((6, 1, 4), (-1, 2, 6));
            Segment3D segment2 = Matrix3D.Scale(1, 2, 5) * segment1 + (2, 4, -1);

            PrecisionAssert.AreEqual(ddouble.Sqrt(7 * 7 + 1 * 1 + 2 * 2), segment1.Length, 1e-30);

            Vector3DAssert.AreEqual((6 * 1 + 2, 1 * 2 + 4, 4 * 5 - 1), segment2.V0, 1e-30);
            Vector3DAssert.AreEqual((-1 * 1 + 2, 2 * 2 + 4, 6 * 5 - 1), segment2.V1, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Segment3D((6, 1, 3), (-1, 2, 4)) == new Segment3D((6, 1, 3), (-1, 2, 4)));
            Assert.IsTrue(new Segment3D((6, 1, 3), (-1, 3, 4)) != new Segment3D((6, 1, 3), (-1, 2, 4)));
            Assert.IsTrue(new Segment3D((6, 2, 3), (-1, 2, 4)) != new Segment3D((6, 1, 3), (-1, 2, 4)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Segment3D((4, 5, 3), (1, 2, 7)), +(new Segment3D((4, 5, 3), (1, 2, 7))));
            Assert.AreEqual(new Segment3D((-4, -5, -3), (-1, -2, -7)), -(new Segment3D((4, 5, 3), (1, 2, 7))));
            Assert.AreEqual(new Segment3D((5, 9, 8), (2, 6, 12)), new Segment3D((4, 5, 3), (1, 2, 7)) + (1, 4, 5));
            Assert.AreEqual(new Segment3D((3, 1, -2), (0, -2, 2)), new Segment3D((4, 5, 3), (1, 2, 7)) - (1, 4, 5));
            Assert.AreEqual(new Segment3D((5, 9, 8), (2, 6, 12)), (1, 4, 5) + new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((-3, -1, 2), (0, 2, -2)), (1, 4, 5) - new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), new Segment3D((4, 5, 3), (1, 2, 7)) * (ddouble)2);
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), new Segment3D((4, 5, 3), (1, 2, 7)) * (double)2);
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), (ddouble)2 * new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((8, 10, 6), (2, 4, 14)), (double)2 * new Segment3D((4, 5, 3), (1, 2, 7)));
            Assert.AreEqual(new Segment3D((2, 2.5, 1.5), (0.5, 1, 3.5)), new Segment3D((4, 5, 3), (1, 2, 7)) / (ddouble)2);
            Assert.AreEqual(new Segment3D((2, 2.5, 1.5), (0.5, 1, 3.5)), new Segment3D((4, 5, 3), (1, 2, 7)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Segment3D segment1 = new(Vector3D.Zero, (1, 1, 1));
            Segment3D segment2 = new Segment3D(Vector3D.Zero, (1, 1, 1)) * 2;
            Segment3D segment3 = new Segment3D(Vector3D.Zero, (1, 1, 1)) * -2;
            Segment3D segment4 = new Segment3D(Vector3D.Zero, (1, 1, 1)) + (2, 3, 4);
            Segment3D segment5 = new(Vector3D.Zero, (3, 4, 5));
            Segment3D segment6 = new(Vector3D.Zero, (4, 3, 5));

            Quaternion q = new Quaternion(2, 5, 3, 4).Normal;
            Matrix3D m = new(q);

            Segment3D segment7 = q * segment4;
            Segment3D segment8 = m * segment4;

            Vector3DAssert.AreEqual((0, 0, 0), segment1.Point(0), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), segment1.Point(1), 1e-30);
            Vector3DAssert.AreEqual((2, 2, 2), segment1.Point(2), 1e-30);

            Vector3DAssert.AreEqual(segment1.Point(0) * 2, segment2.Point(0), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) * 2, segment2.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) * 2, segment2.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(segment1.Point(0) * -2, segment3.Point(0), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) * -2, segment3.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) * -2, segment3.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(segment1.Point(0) + (2, 3, 4), segment4.Point(0), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 4) + (2, 3, 4), segment4.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(segment1.Point(ddouble.Pi / 2) + (2, 3, 4), segment4.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual((3, 4, 5), segment5.Point(1), 1e-30);
            Vector3DAssert.AreEqual((6, 8, 10), segment5.Point(2), 1e-30);

            Vector3DAssert.AreEqual((4, 3, 5), segment6.Point(1), 1e-30);
            Vector3DAssert.AreEqual((8, 6, 10), segment6.Point(2), 1e-30);

            Vector3DAssert.AreEqual(q * segment4.Point(0), segment7.Point(0), 1e-30);
            Vector3DAssert.AreEqual(q * segment4.Point(1), segment7.Point(1), 1e-30);

            Vector3DAssert.AreEqual(m * segment4.Point(0), segment8.Point(0), 1e-30);
            Vector3DAssert.AreEqual(m * segment4.Point(1), segment8.Point(1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Segment3D.IsValid(new Segment3D((6, 1, 4), (-1, 2, 6))));
            Assert.IsFalse(Segment3D.IsValid(Segment3D.Invalid));
        }
    }
}