using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Rectangle3DTests {
        [TestMethod()]
        public void Rectangle3DTest() {
            Rectangle3D rectangle = new((1, 2, 7), (4, 3), Vector3D.Rot((0, 0, 1), (2, 3, 4)));

            Vector3DAssert.AreEqual((1, 2, 7), rectangle.Center, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, 3, 4).Normal, rectangle.Normal, 1e-30);
            Assert.AreEqual(4d, rectangle.Scale.X);
            Assert.AreEqual(3d, rectangle.Scale.Y);
            Assert.AreEqual(Vector3D.Rot((0, 0, 1), (2, 3, 4)).Normal, rectangle.Rotation);

            PrecisionAssert.AreEqual(8 * 6, rectangle.Area);
            PrecisionAssert.AreEqual((8 + 6) * 2, rectangle.Perimeter, 1e-30);
        }

        [TestMethod]
        public void EqualTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Rectangle3D rectangle1 = new((1, 2, 7), (4, 3), rot);
            Rectangle3D rectangle2 = new((1, 2, 7), (4, 3), rot);
            Rectangle3D rectangle3 = new((1, 2, 7), (4, 4), rot);
            Rectangle3D rectangle4 = new((1, 2, 7), (4, 4), rot);

            Assert.AreEqual(rectangle1, rectangle2);
            Assert.AreNotEqual(rectangle1, rectangle3);

            Assert.IsTrue(rectangle1 == rectangle2);
            Assert.IsTrue(rectangle1 != rectangle3);
            Assert.IsTrue(rectangle1 != rectangle4);
        }

        [TestMethod()]
        public void OperatorTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Assert.AreEqual(new Rectangle3D((1, 2, 7), (4, 3), rot), +(new Rectangle3D((1, 2, 7), (4, 3), rot)));
            Assert.AreEqual(new Rectangle3D((-1, -2, -7), (-4, -3), rot), -(new Rectangle3D((1, 2, 7), (4, 3), rot)));
            Assert.AreEqual(new Rectangle3D((2, 6, 12), (4, 3), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) + (1, 4, 5));
            Assert.AreEqual(new Rectangle3D((0, -2, 2), (4, 3), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) - (1, 4, 5));
            Assert.AreEqual(new Rectangle3D((2, 6, 12), (4, 3), rot), (1, 4, 5) + new Rectangle3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Rectangle3D((0, 2, -2), (-4, -3), rot), (1, 4, 5) - new Rectangle3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Rectangle3D((2, 4, 14), (8, 6), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) * (ddouble)2);
            Assert.AreEqual(new Rectangle3D((2, 4, 14), (8, 6), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) * (double)2);
            Assert.AreEqual(new Rectangle3D((2, 4, 14), (8, 6), rot), (ddouble)2 * new Rectangle3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Rectangle3D((2, 4, 14), (8, 6), rot), (double)2 * new Rectangle3D((1, 2, 7), (4, 3), rot));
            Assert.AreEqual(new Rectangle3D((0.5, 1, 3.5), (2, 1.5), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) / (ddouble)2);
            Assert.AreEqual(new Rectangle3D((0.5, 1, 3.5), (2, 1.5), rot), new Rectangle3D((1, 2, 7), (4, 3), rot) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Rectangle3D rectangle1 = new(Vector3D.Zero, (4, 3), (0, 0, 1));
            Rectangle3D rectangle2 = new Rectangle3D(Vector3D.Zero, (4, 3), (0, 0, 1)) * 2;
            Rectangle3D rectangle3 = new Rectangle3D(Vector3D.Zero, (4, 3), (0, 0, 1)) * -2;
            Rectangle3D rectangle4 = new((2, 3, 4), (4, 3), (0, 0, 1));

            Quaternion q = (1, 2, 3, 4);

            Rectangle3D rectangle5 = q * new Rectangle3D((2, 3, 4), (20, 15), (0, 0, 1));

            Vector3DAssert.AreEqual((-4, -3, 0), rectangle1.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual((4, -3, 0), rectangle1.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual((4, 3, 0), rectangle1.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual((-4, 3, 0), rectangle1.Vertex[3], 1e-30);

            Vector3DAssert.AreEqual(rectangle1.Vertex[0] * 2, rectangle2.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[1] * 2, rectangle2.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[2] * 2, rectangle2.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[3] * 2, rectangle2.Vertex[3], 1e-30);

            Vector3DAssert.AreEqual(rectangle1.Vertex[0] * -2, rectangle3.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[1] * -2, rectangle3.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[2] * -2, rectangle3.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[3] * -2, rectangle3.Vertex[3], 1e-30);

            Vector3DAssert.AreEqual(rectangle1.Vertex[0] + (2, 3, 4), rectangle4.Vertex[0], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[1] + (2, 3, 4), rectangle4.Vertex[1], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[2] + (2, 3, 4), rectangle4.Vertex[2], 1e-30);
            Vector3DAssert.AreEqual(rectangle1.Vertex[3] + (2, 3, 4), rectangle4.Vertex[3], 1e-30);

            Vector3DAssert.AreEqual(q * (rectangle1.Vertex[0] * 5 + (2, 3, 4)), rectangle5.Vertex[0], 2e-29);
            Vector3DAssert.AreEqual(q * (rectangle1.Vertex[1] * 5 + (2, 3, 4)), rectangle5.Vertex[1], 2e-29);
            Vector3DAssert.AreEqual(q * (rectangle1.Vertex[2] * 5 + (2, 3, 4)), rectangle5.Vertex[2], 2e-29);
            Vector3DAssert.AreEqual(q * (rectangle1.Vertex[3] * 5 + (2, 3, 4)), rectangle5.Vertex[3], 2e-29);
        }

        [TestMethod()]
        public void ValidTest() {
            Quaternion rot = Quaternion.FromAxisAngle(new Vector3D(2, 3, 4).Normal, 5);

            Assert.IsTrue(Rectangle3D.IsValid(new Rectangle3D((1, 2, 7), (4, 3), rot)));
            Assert.IsFalse(Rectangle3D.IsValid(Rectangle3D.Invalid));
        }
    }
}