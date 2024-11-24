using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Cuboid3DTests {
        [TestMethod()]
        public void Cuboid3DTest() {
            Cuboid3D cuboid = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));

            Vector3DAssert.AreEqual((1, 2, 7), cuboid.Center, 1e-30);
            Vector3DAssert.AreEqual((2, 3, 4), cuboid.Scale, 1e-30);
            Assert.IsTrue((new Quaternion(4, 3, 5, 6).Normal - cuboid.Rotation).Norm < 1e-30);

            PrecisionAssert.AreEqual(24 * 8, cuboid.Volume, 1e-29);
            PrecisionAssert.AreEqual((4 * 6 + 6 * 8 + 4 * 8) * 2, cuboid.Area, 1e-29);
        }

        [TestMethod]
        public void EqualTest() {
            Cuboid3D cuboid1 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Cuboid3D cuboid2 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 6));
            Cuboid3D cuboid3 = new((1, 3, 7), (2, 3, 4), (4, 3, 5, 6));
            Cuboid3D cuboid4 = new((1, 2, 7), (2, 3, 5), (4, 3, 5, 6));
            Cuboid3D cuboid5 = new((1, 2, 7), (2, 3, 4), (4, 3, 5, 7));

            Assert.AreEqual(cuboid1, cuboid2);
            Assert.AreNotEqual(cuboid1, cuboid3);

            Assert.IsTrue(cuboid1 == cuboid2);
            Assert.IsTrue(cuboid1 != cuboid3);
            Assert.IsTrue(cuboid1 != cuboid4);
            Assert.IsTrue(cuboid1 != cuboid5);
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)), +(new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Cuboid3D((-1, -2, -7), (2, 3, 4), (4, 3, 5, 6)), -(new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.AreEqual(new Cuboid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) + (1, 4, 5));
            Assert.AreEqual(new Cuboid3D((0, -2, 2), (2, 3, 4), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) - (1, 4, 5));
            Assert.AreEqual(new Cuboid3D((2, 6, 12), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) + new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Cuboid3D((0, 2, -2), (2, 3, 4), (4, 3, 5, 6)), (1, 4, 5) - new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Cuboid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (ddouble)2);
            Assert.AreEqual(new Cuboid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) * (double)2);
            Assert.AreEqual(new Cuboid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (ddouble)2 * new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Cuboid3D((2, 4, 14), (4, 6, 8), (4, 3, 5, 6)), (double)2 * new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)));
            Assert.AreEqual(new Cuboid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (ddouble)2);
            Assert.AreEqual(new Cuboid3D((0.5, 1, 3.5), (1, 1.5, 2), (4, 3, 5, 6)), new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Cuboid3D cuboid1 = new(Vector3D.Zero, (4, 3, 5), Quaternion.One);
            Cuboid3D cuboid2 = new Cuboid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * 2;
            Cuboid3D cuboid3 = new Cuboid3D(Vector3D.Zero, (4, 3, 5), Quaternion.One) * -2;
            Cuboid3D cuboid4 = new((2, 3, 4), (4, 3, 5), Quaternion.One);

            Quaternion q = (1, 2, 3, 4);

            Cuboid3D cuboid5 = q * cuboid4;

            Vector3DAssert.AreEqual((-4, -3, -5), cuboid1.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((4, -3, -5), cuboid1.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual((4, 3, -5), cuboid1.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual((-4, 3, -5), cuboid1.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual((-4, -3, 5), cuboid1.Vertex[4], 1e-30);
            Vector3DAssert.AreEqual((4, -3, 5), cuboid1.Vertex[5], 1e-30);
            Vector3DAssert.AreEqual((4, 3, 5), cuboid1.Vertex[6], 1e-30);
            Vector3DAssert.AreEqual((-4, 3, 5), cuboid1.Vertex[7], 1e-30);

            Vector3DAssert.AreEqual(cuboid1.Vertex[0] * 2, cuboid2.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[1] * 2, cuboid2.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[2] * 2, cuboid2.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[3] * 2, cuboid2.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[4] * 2, cuboid2.Vertex[4], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[5] * 2, cuboid2.Vertex[5], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[6] * 2, cuboid2.Vertex[6], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[7] * 2, cuboid2.Vertex[7], 1e-30);

            Vector3DAssert.AreEqual(cuboid1.Vertex[0] * -2, cuboid3.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[1] * -2, cuboid3.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[2] * -2, cuboid3.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[3] * -2, cuboid3.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[4] * -2, cuboid3.Vertex[4], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[5] * -2, cuboid3.Vertex[5], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[6] * -2, cuboid3.Vertex[6], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[7] * -2, cuboid3.Vertex[7], 1e-30);

            Vector3DAssert.AreEqual(cuboid1.Vertex[0] + (2, 3, 4), cuboid4.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[1] + (2, 3, 4), cuboid4.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[2] + (2, 3, 4), cuboid4.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[3] + (2, 3, 4), cuboid4.Vertex[3], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[4] + (2, 3, 4), cuboid4.Vertex[4], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[5] + (2, 3, 4), cuboid4.Vertex[5], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[6] + (2, 3, 4), cuboid4.Vertex[6], 1e-30);
            Vector3DAssert.AreEqual(cuboid1.Vertex[7] + (2, 3, 4), cuboid4.Vertex[7], 1e-30);

            Vector3DAssert.AreEqual(q * cuboid4.Vertex[0], cuboid5.Vertex[0], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[1], cuboid5.Vertex[1], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[2], cuboid5.Vertex[2], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[3], cuboid5.Vertex[3], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[4], cuboid5.Vertex[4], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[5], cuboid5.Vertex[5], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[6], cuboid5.Vertex[6], 1e-29);
            Vector3DAssert.AreEqual(q * cuboid4.Vertex[7], cuboid5.Vertex[7], 1e-29);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Cuboid3D.IsValid(new Cuboid3D((1, 2, 7), (2, 3, 4), (4, 3, 5, 6))));
            Assert.IsFalse(Cuboid3D.IsValid(Cuboid3D.Invalid));
        }
    }
}