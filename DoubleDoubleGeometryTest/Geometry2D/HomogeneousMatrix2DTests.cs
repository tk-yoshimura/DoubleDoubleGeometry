using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class HomogeneousMatrix2DTests {
        [TestMethod()]
        public void HomogeneousMatrix2DTest() {
            HomogeneousMatrix2D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(1, matrix1.E00);
            Assert.AreEqual(2, matrix1.E01);
            Assert.AreEqual(3, matrix1.E02);
            Assert.AreEqual(4, matrix1.E10);
            Assert.AreEqual(5, matrix1.E11);
            Assert.AreEqual(6, matrix1.E12);
            Assert.AreEqual(7, matrix1.E20);
            Assert.AreEqual(8, matrix1.E21);
            Assert.AreEqual(9, matrix1.E22);
        }

        [TestMethod()]
        public void TransposeTest() {
            HomogeneousMatrix2D matrix1 = new HomogeneousMatrix2D(1, 2, 3, 4, 5, 6, 7, 8, 9).T;

            Assert.AreEqual(new HomogeneousMatrix2D(1, 4, 7, 2, 5, 8, 3, 6, 9), matrix1);
        }

        [TestMethod()]
        public void InverseTest() {
            HomogeneousMatrix2D matrix1 = new HomogeneousMatrix2D(1, 2, 3, 4, 2, 6, 7, 8, 10);
            HomogeneousMatrix2D matrix2 = matrix1.Inverse.Inverse;

            PrecisionAssert.AreEqual(matrix1.E00, matrix2.E00, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E01, matrix2.E01, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E02, matrix2.E02, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E10, matrix2.E10, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E11, matrix2.E11, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E12, matrix2.E12, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E20, matrix2.E20, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E21, matrix2.E21, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E22, matrix2.E22, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            HomogeneousMatrix2D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
            HomogeneousMatrix2D matrix2 = new(1, 3, 5, 7, 9, 11, 13, 15, 2);
            HomogeneousMatrix2D matrix3 = new(8, 9, 10, 11, 12, 13, 14, 15, 16);
            HomogeneousMatrix2D matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(new HomogeneousMatrix2D(1, 2, 3, 4, 5, 6, 7, 8, 9), +matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(-1, -2, -3, -4, -5, -6, -7, -8, -9), -matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(2, 5, 8, 11, 14, 17, 20, 23, 11), matrix1 + matrix2);
            Assert.AreEqual(new HomogeneousMatrix2D(0, -1, -2, -3, -4, -5, -6, -7, 7), matrix1 - matrix2);
            Assert.AreEqual(new HomogeneousMatrix2D(0, 1, 2, 3, 4, 5, 6, 7, -7), matrix2 - matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(72, 78, 84, 171, 186, 201, 270, 294, 318), matrix1 * matrix3);
            Assert.AreEqual(new HomogeneousMatrix2D(114, 141, 168, 150, 186, 222, 186, 231, 276), matrix3 * matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(2, 4, 6, 8, 10, 12, 14, 16, 18), 2d * matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(2, 4, 6, 8, 10, 12, 14, 16, 18), (ddouble)2d * matrix1);
            Assert.AreEqual(new HomogeneousMatrix2D(2, 4, 6, 8, 10, 12, 14, 16, 18), matrix1 * 2d);
            Assert.AreEqual(new HomogeneousMatrix2D(2, 4, 6, 8, 10, 12, 14, 16, 18), matrix1 * (ddouble)2d);
            Assert.AreEqual(new HomogeneousMatrix2D(0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5), matrix1 / 2d);
            Assert.AreEqual(new HomogeneousMatrix2D(0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5), matrix1 / (ddouble)2d);

            Assert.IsTrue(matrix1 == matrix4);
            Assert.IsFalse(matrix2 == matrix3);
            Assert.IsTrue(matrix1 != matrix2);
        }

        [TestMethod()]
        public void EqualsTest() {
            HomogeneousMatrix2D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
            HomogeneousMatrix2D matrix2 = new(1, 3, 5, 7, 9, 11, 13, 15, 2);
            HomogeneousMatrix2D matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(matrix1.Equals(matrix1));
            Assert.IsFalse(matrix1.Equals(matrix2));
            Assert.IsTrue(matrix1.Equals(matrix3));
            Assert.IsFalse(matrix1.Equals(null));
        }

        [TestMethod()]
        public void StaticMatrixTest() {
            HomogeneousMatrix2D matrix1 = HomogeneousMatrix2D.Zero;
            HomogeneousMatrix2D matrix2 = HomogeneousMatrix2D.Identity;
            HomogeneousMatrix2D matrix3 = HomogeneousMatrix2D.Invalid;

            Assert.AreEqual(0, matrix1.E00);
            Assert.AreEqual(0, matrix1.E01);
            Assert.AreEqual(0, matrix1.E02);
            Assert.AreEqual(0, matrix1.E10);
            Assert.AreEqual(0, matrix1.E11);
            Assert.AreEqual(0, matrix1.E12);
            Assert.AreEqual(0, matrix1.E20);
            Assert.AreEqual(0, matrix1.E21);
            Assert.AreEqual(0, matrix1.E22);

            Assert.AreEqual(1, matrix2.E00);
            Assert.AreEqual(0, matrix2.E01);
            Assert.AreEqual(0, matrix2.E02);
            Assert.AreEqual(0, matrix2.E10);
            Assert.AreEqual(1, matrix2.E11);
            Assert.AreEqual(0, matrix2.E12);
            Assert.AreEqual(0, matrix2.E20);
            Assert.AreEqual(0, matrix2.E21);
            Assert.AreEqual(1, matrix2.E22);

            Assert.IsTrue(ddouble.IsNaN(matrix3.E00));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E01));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E02));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E10));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E11));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E12));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E20));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E21));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E22));
        }

        [TestMethod()]
        public void MoveTest() {
            Vector2D vector = new(1, 2);

            Assert.AreEqual(new Vector2D(3, 5), HomogeneousMatrix2D.Move(2, 3) * vector);
        }

        [TestMethod()]
        public void DetTest() {
            HomogeneousMatrix2D matrix = new HomogeneousMatrix2D(5, 2, 3, 4, 2, 3, 1, 7, 9);

            PrecisionAssert.AreEqual(((Matrix)matrix).Det, matrix.Det, 1e-30);
        }

        [TestMethod()]
        public void ToStringTest() {
            HomogeneousMatrix2D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual("[[1, 2, 3], [4, 5, 6], [7, 8, 9]]", matrix1.ToString());
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0], [4.00e0, 5.00e0, 6.00e0], [7.00e0, 8.00e0, 9.00e0]]", matrix1.ToString("e2"));
            Assert.AreEqual("[[1, 2, 3], [4, 5, 6], [7, 8, 9]]", $"{matrix1}");
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0], [4.00e0, 5.00e0, 6.00e0], [7.00e0, 8.00e0, 9.00e0]]", $"{matrix1:e2}");
        }
    }
}