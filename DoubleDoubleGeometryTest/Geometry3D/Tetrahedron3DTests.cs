using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Tetrahedron3DTests {
        [TestMethod()]
        public void Tetrahedron3DTest() {
            Tetrahedron3D tetrahedron1 = new((0, 0, 0), (1, 0, 0), (0, 2, 0), (0, 0, 3));
            Tetrahedron3D tetrahedron2 = Matrix3D.Scale(1, 2, 3) * tetrahedron1 + (2, 4, 6);

            PrecisionAssert.AreEqual(9, (Matrix3D.Rotate(1, 1, 1) * tetrahedron1).Area, 1e-30);
            PrecisionAssert.AreEqual(1, (Matrix3D.Rotate(1, 1, 1) * tetrahedron1).Volume, 1e-30);
            PrecisionAssert.AreEqual(6, (Matrix3D.Rotate(1, 2, 3) * tetrahedron2).Volume, 1e-30);

            Vector3DAssert.AreEqual((0 * 1 + 2, 0 * 2 + 4, 0 * 3 + 6), tetrahedron2.V0, 1e-30);
            Vector3DAssert.AreEqual((1 * 1 + 2, 0 * 2 + 4, 0 * 3 + 6), tetrahedron2.V1, 1e-30);
            Vector3DAssert.AreEqual((0 * 1 + 2, 2 * 2 + 4, 0 * 3 + 6), tetrahedron2.V2, 1e-30);
            Vector3DAssert.AreEqual((0 * 1 + 2, 0 * 2 + 4, 3 * 3 + 6), tetrahedron2.V3, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)) == new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)));
            Assert.IsTrue(new Tetrahedron3D((6, 1, 3), (-1, 3, 4), (5, 7, 2), (7, 1, 6)) != new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)));
            Assert.IsTrue(new Tetrahedron3D((6, 2, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)) != new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)));
            Assert.IsTrue(new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 8, 2), (7, 1, 6)) != new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)));
            Assert.IsTrue(new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (8, 1, 6)) != new Tetrahedron3D((6, 1, 3), (-1, 2, 4), (5, 7, 2), (7, 1, 6)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)), +(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5))));
            Assert.AreEqual(new Tetrahedron3D((-4, -5, -3), (-1, -2, -7), (-5, -1, -2), (-3, -4, -5)), -(new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5))));
            Assert.AreEqual(new Tetrahedron3D((5, 9, 8), (2, 6, 12), (6, 5, 7), (4, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) + (1, 4, 5));
            Assert.AreEqual(new Tetrahedron3D((3, 1, -2), (0, -2, 2), (4, -3, -3), (2, 0, 0)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) - (1, 4, 5));
            Assert.AreEqual(new Tetrahedron3D((5, 9, 8), (2, 6, 12), (6, 5, 7), (4, 8, 10)), (1, 4, 5) + new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((-3, -1, 2), (0, 2, -2), (-4, 3, 3), (-2, 0, 0)), (1, 4, 5) - new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) * (ddouble)2);
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) * (double)2);
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), (ddouble)2 * new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((8, 10, 6), (2, 4, 14), (10, 2, 4), (6, 8, 10)), (double)2 * new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)));
            Assert.AreEqual(new Tetrahedron3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1), (1.5, 2, 2.5)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) / (ddouble)2);
            Assert.AreEqual(new Tetrahedron3D((2, 2.5, 1.5), (0.5, 1, 3.5), (2.5, 0.5, 1), (1.5, 2, 2.5)), new Tetrahedron3D((4, 5, 3), (1, 2, 7), (5, 1, 2), (3, 4, 5)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Tetrahedron3D tetrahedron1 = new(Vector3D.Zero, (1, 1, 1), (2, 0, 2), (3, 4, 5));
            Tetrahedron3D tetrahedron2 = new Tetrahedron3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2), (3, 4, 5)) * 2;
            Tetrahedron3D tetrahedron3 = new Tetrahedron3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2), (3, 4, 5)) * -2;
            Tetrahedron3D tetrahedron4 = new Tetrahedron3D(Vector3D.Zero, (1, 1, 1), (2, 0, 2), (3, 4, 5)) + (2, 3, 4);

            Quaternion q = new Quaternion(2, 5, 3, 4).Normal;
            Matrix3D m = new(q);

            Tetrahedron3D tetrahedron5 = q * tetrahedron4;
            Tetrahedron3D tetrahedron6 = m * tetrahedron4;

            Vector3DAssert.AreEqual((0, 0, 0), tetrahedron1.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), tetrahedron1.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), tetrahedron1.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual((2, 0, 2), tetrahedron1.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual((3, 4, 5), tetrahedron1.Point(0, 0, 1), 1e-30);

            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 0) * 2, tetrahedron2.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 0, 0) * 2, tetrahedron2.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0.5, 0.5, 0.5) * 2, tetrahedron2.Point(0.5, 0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 1, 0.5) * 2, tetrahedron2.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 1, 0.5) * 2, tetrahedron2.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 1) * 2, tetrahedron2.Point(0, 0, 1), 1e-30);

            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 0) * -2, tetrahedron3.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 0, 0) * -2, tetrahedron3.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0.5, 0.5, 0.5) * -2, tetrahedron3.Point(0.5, 0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 1, 0.5) * -2, tetrahedron3.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 1, 0.5) * -2, tetrahedron3.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 1) * -2, tetrahedron3.Point(0, 0, 1), 1e-30);

            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 0) + (2, 3, 4), tetrahedron4.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 0, 0) + (2, 3, 4), tetrahedron4.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0.5, 0.5, 0.5) + (2, 3, 4), tetrahedron4.Point(0.5, 0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(1, 1, 0.5) + (2, 3, 4), tetrahedron4.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 1, 0.5) + (2, 3, 4), tetrahedron4.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(tetrahedron1.Point(0, 0, 1) + (2, 3, 4), tetrahedron4.Point(0, 0, 1), 1e-30);

            Vector3DAssert.AreEqual(q * tetrahedron4.Point(0, 0, 0), tetrahedron5.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(q * tetrahedron4.Point(1, 0, 0), tetrahedron5.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(q * tetrahedron4.Point(0.5, 0.5, 0.5), tetrahedron5.Point(0.5, 0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(q * tetrahedron4.Point(1, 1, 0.5), tetrahedron5.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(q * tetrahedron4.Point(0, 1, 0.5), tetrahedron5.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(q * tetrahedron4.Point(0, 0, 1), tetrahedron5.Point(0, 0, 1), 1e-30);

            Vector3DAssert.AreEqual(m * tetrahedron4.Point(0, 0, 0), tetrahedron6.Point(0, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(m * tetrahedron4.Point(1, 0, 0), tetrahedron6.Point(1, 0, 0), 1e-30);
            Vector3DAssert.AreEqual(m * tetrahedron4.Point(0.5, 0.5, 0.5), tetrahedron6.Point(0.5, 0.5, 0.5), 1e-30);
            Vector3DAssert.AreEqual(m * tetrahedron4.Point(1, 1, 0.5), tetrahedron6.Point(1, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(m * tetrahedron4.Point(0, 1, 0.5), tetrahedron6.Point(0, 1, 0.5), 1e-30);
            Vector3DAssert.AreEqual(m * tetrahedron4.Point(0, 0, 1), tetrahedron6.Point(0, 0, 1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Tetrahedron3D.IsValid(new Tetrahedron3D((0, 0, 0), (1, 0, 0), (0, 2, 0), (0, 0, 3))));
            Assert.IsFalse(Tetrahedron3D.IsValid(Tetrahedron3D.Invalid));
        }

        [TestMethod()]
        public void InsideTest() {
            Tetrahedron3D t = new((0, 0, 0), (0, 0, 1), (0, 1, 0), (1, 0, 0));

            Vector3D[] insides = [
                (0.25, 0.25, 0.25), (0.25, 0.5, 0.125), (0.5, 0.25, 0.125), (0.125, 0.25, 0.5)
            ];

            Vector3D[] outsides = [
                (-0.25, -0.25, -0.25), (-0.25, 0.5, 0.125), (0.5, -0.25, 0.125), (-0.25, -0.5, 0.125), (0.125, -0.5, -0.25),
                (1.5, 0, 0), (0, 1.5, 0), (0, 0, 1.5), (0.5, 0.75, 0), (0, 0.75, 0.5), (0.5, 0.5, 0.5)
            ];

            foreach (Vector3D v in insides) {
                Assert.IsTrue(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(insides).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(t.Inside(v));
            }

            Assert.IsTrue(t.Inside(outsides).All(b => !b));

            Matrix3D m = new double[,] { { 1, 2, 7 }, { 3, 5, 8 }, { -2, 4, 6 } };
            Vector3D s = (4, 6, 5);

            Tetrahedron3D t2 = m * t + s;

            foreach (Vector3D v in insides) {
                Assert.IsTrue(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(insides.Select(v => m * v + s)).All(b => b));

            foreach (Vector3D v in outsides) {
                Assert.IsFalse(t2.Inside(m * v + s));
            }

            Assert.IsTrue(t2.Inside(outsides.Select(v => m * v + s)).All(b => !b));
        }
    }
}