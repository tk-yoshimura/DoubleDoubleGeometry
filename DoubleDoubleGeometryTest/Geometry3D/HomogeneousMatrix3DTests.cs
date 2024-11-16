using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class HomogeneousMatrix3DTests {
        [TestMethod()]
        public void HomogeneousMatrix3DTest() {
            HomogeneousMatrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(1, matrix1.E00);
            Assert.AreEqual(2, matrix1.E01);
            Assert.AreEqual(3, matrix1.E02);
            Assert.AreEqual(4, matrix1.E03);
            Assert.AreEqual(5, matrix1.E10);
            Assert.AreEqual(6, matrix1.E11);
            Assert.AreEqual(7, matrix1.E12);
            Assert.AreEqual(8, matrix1.E13);
            Assert.AreEqual(9, matrix1.E20);
            Assert.AreEqual(10, matrix1.E21);
            Assert.AreEqual(11, matrix1.E22);
            Assert.AreEqual(12, matrix1.E23);
            Assert.AreEqual(13, matrix1.E30);
            Assert.AreEqual(14, matrix1.E31);
            Assert.AreEqual(15, matrix1.E32);
            Assert.AreEqual(16, matrix1.E33);
        }

        [TestMethod()]
        public void TransposeTest() {
            HomogeneousMatrix3D matrix1 = new HomogeneousMatrix3D(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).T;

            Assert.AreEqual(new HomogeneousMatrix3D(1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16), matrix1);
        }

        [TestMethod()]
        public void InverseTest() {
            HomogeneousMatrix3D matrix1 = new HomogeneousMatrix3D(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16);
            HomogeneousMatrix3D matrix2 = matrix1.Inverse.Inverse;

            PrecisionAssert.AreEqual(matrix1.E00, matrix2.E00, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E01, matrix2.E01, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E02, matrix2.E02, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E03, matrix2.E03, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E10, matrix2.E10, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E11, matrix2.E11, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E12, matrix2.E12, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E13, matrix2.E13, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E20, matrix2.E20, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E21, matrix2.E21, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E22, matrix2.E22, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E23, matrix2.E23, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E30, matrix2.E30, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E31, matrix2.E31, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E32, matrix2.E32, 2e-30);
            PrecisionAssert.AreEqual(matrix1.E33, matrix2.E33, 2e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            HomogeneousMatrix3D matrix1 = new(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16);
            HomogeneousMatrix3D matrix2 = new(13, 14, 9, 10, 11, 7, 16, 8, 15, 3, 1, 2, 4, 12, 5, 6);
            HomogeneousMatrix3D matrix3 = new(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16);

            Assert.AreEqual(new HomogeneousMatrix3D(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16), +matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(-9, -10, -11, -13, -14, -3, -1, -2, -7, -8, -4, -12, -5, -6, -15, -16), -matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(22, 24, 20, 23, 25, 10, 17, 10, 22, 11, 5, 14, 9, 18, 20, 22), matrix1 + matrix2);
            Assert.AreEqual(new HomogeneousMatrix3D(-4, -4, 2, 3, 3, -4, -15, -6, -8, 5, 3, 10, 1, -6, 10, 10), matrix1 - matrix2);
            Assert.AreEqual(new HomogeneousMatrix3D(4, 4, -2, -3, -3, 4, 15, 6, 8, -5, -3, -10, -1, 6, -10, -10), matrix2 - matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(444, 385, 317, 270, 238, 244, 185, 178, 287, 310, 255, 214, 420, 349, 236, 224), matrix1 * matrix2);
            Assert.AreEqual(new HomogeneousMatrix3D(426, 304, 343, 465, 349, 307, 312, 477, 194, 179, 202, 245, 269, 152, 166, 232), matrix2 * matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(18, 20, 22, 26, 28, 6, 2, 4, 14, 16, 8, 24, 10, 12, 30, 32), 2d * matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(18, 20, 22, 26, 28, 6, 2, 4, 14, 16, 8, 24, 10, 12, 30, 32), (ddouble)2d * matrix1);
            Assert.AreEqual(new HomogeneousMatrix3D(18, 20, 22, 26, 28, 6, 2, 4, 14, 16, 8, 24, 10, 12, 30, 32), matrix1 * 2d);
            Assert.AreEqual(new HomogeneousMatrix3D(18, 20, 22, 26, 28, 6, 2, 4, 14, 16, 8, 24, 10, 12, 30, 32), matrix1 * (ddouble)2d);
            Assert.AreEqual(new HomogeneousMatrix3D(4.5, 5.0, 5.5, 6.5, 7.0, 1.5, 0.5, 1.0, 3.5, 4.0, 2.0, 6.0, 2.5, 3.0, 7.5, 8.0), matrix1 / 2d);
            Assert.AreEqual(new HomogeneousMatrix3D(4.5, 5.0, 5.5, 6.5, 7.0, 1.5, 0.5, 1.0, 3.5, 4.0, 2.0, 6.0, 2.5, 3.0, 7.5, 8.0), matrix1 / (ddouble)2d);

            Assert.IsTrue(matrix1 == matrix3);
            Assert.IsFalse(matrix2 == matrix3);
            Assert.IsTrue(matrix1 != matrix2);
        }

        [TestMethod()]
        public void EqualsTest() {
            HomogeneousMatrix3D matrix1 = new(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16);
            HomogeneousMatrix3D matrix2 = new(13, 14, 9, 10, 11, 7, 16, 8, 15, 3, 1, 2, 4, 12, 5, 6);
            HomogeneousMatrix3D matrix3 = new(9, 10, 11, 13, 14, 3, 1, 2, 7, 8, 4, 12, 5, 6, 15, 16);

            Assert.IsTrue(matrix1.Equals(matrix1));
            Assert.IsFalse(matrix1.Equals(matrix2));
            Assert.IsTrue(matrix1.Equals(matrix3));
            Assert.IsFalse(matrix1.Equals(null));
        }

        [TestMethod()]
        public void StaticMatrixTest() {
            HomogeneousMatrix3D matrix1 = HomogeneousMatrix3D.Zero;
            HomogeneousMatrix3D matrix2 = HomogeneousMatrix3D.Identity;
            HomogeneousMatrix3D matrix3 = HomogeneousMatrix3D.Invalid;

            Assert.AreEqual(0, matrix1.E00);
            Assert.AreEqual(0, matrix1.E01);
            Assert.AreEqual(0, matrix1.E02);
            Assert.AreEqual(0, matrix1.E03);
            Assert.AreEqual(0, matrix1.E10);
            Assert.AreEqual(0, matrix1.E11);
            Assert.AreEqual(0, matrix1.E12);
            Assert.AreEqual(0, matrix1.E13);
            Assert.AreEqual(0, matrix1.E20);
            Assert.AreEqual(0, matrix1.E21);
            Assert.AreEqual(0, matrix1.E22);
            Assert.AreEqual(0, matrix1.E23);
            Assert.AreEqual(0, matrix1.E30);
            Assert.AreEqual(0, matrix1.E31);
            Assert.AreEqual(0, matrix1.E32);
            Assert.AreEqual(0, matrix1.E33);

            Assert.AreEqual(1, matrix2.E00);
            Assert.AreEqual(0, matrix2.E01);
            Assert.AreEqual(0, matrix2.E02);
            Assert.AreEqual(0, matrix2.E03);
            Assert.AreEqual(0, matrix2.E10);
            Assert.AreEqual(1, matrix2.E11);
            Assert.AreEqual(0, matrix2.E12);
            Assert.AreEqual(0, matrix2.E13);
            Assert.AreEqual(0, matrix2.E20);
            Assert.AreEqual(0, matrix2.E21);
            Assert.AreEqual(1, matrix2.E22);
            Assert.AreEqual(0, matrix2.E23);
            Assert.AreEqual(0, matrix2.E30);
            Assert.AreEqual(0, matrix2.E31);
            Assert.AreEqual(0, matrix2.E32);
            Assert.AreEqual(1, matrix2.E33);

            Assert.IsTrue(ddouble.IsNaN(matrix3.E00));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E01));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E02));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E03));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E10));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E11));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E12));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E13));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E20));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E21));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E22));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E23));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E30));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E31));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E32));
        }

        [TestMethod()]
        public void MoveTest() {
            Vector3D vector = new(1, 2, 4);

            Assert.AreEqual(new Vector3D(3, 5, 9), HomogeneousMatrix3D.Move(2, 3, 5) * vector);
        }

        [TestMethod()]
        public void DetTest() {
            HomogeneousMatrix3D matrix = new HomogeneousMatrix3D(13, 14, 9, 10, 11, 7, 16, 8, 15, 3, 1, 2, 4, 12, 5, 6);

            PrecisionAssert.AreEqual(((Matrix)matrix).Det, matrix.Det, 1e-30);
        }

        [TestMethod()]
        public void ToStringTest() {
            HomogeneousMatrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual("[[1, 2, 3, 4], [5, 6, 7, 8], [9, 10, 11, 12], [13, 14, 15, 16]]", matrix1.ToString());
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0, 4.00e0], [5.00e0, 6.00e0, 7.00e0, 8.00e0], [9.00e0, 1.00e1, 1.10e1, 1.20e1], [1.30e1, 1.40e1, 1.50e1, 1.60e1]]", matrix1.ToString("e2"));
            Assert.AreEqual("[[1, 2, 3, 4], [5, 6, 7, 8], [9, 10, 11, 12], [13, 14, 15, 16]]", $"{matrix1}");
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0, 4.00e0], [5.00e0, 6.00e0, 7.00e0, 8.00e0], [9.00e0, 1.00e1, 1.10e1, 1.20e1], [1.30e1, 1.40e1, 1.50e1, 1.60e1]]", $"{matrix1:e2}");
        }
    }
}