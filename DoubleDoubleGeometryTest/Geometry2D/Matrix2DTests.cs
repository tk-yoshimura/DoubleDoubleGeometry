using Algebra;
using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Matrix2DTests {
        [TestMethod()]
        public void Matrix2DTest() {
            Matrix2D matrix1 = new(1, 2, 3, 4);

            Assert.AreEqual(1, matrix1.E00);
            Assert.AreEqual(2, matrix1.E01);
            Assert.AreEqual(3, matrix1.E10);
            Assert.AreEqual(4, matrix1.E11);
        }

        [TestMethod()]
        public void TransposeTest() {
            Matrix2D matrix1 = new Matrix2D(1, 2, 3, 4).T;

            Assert.AreEqual(new Matrix2D(1, 3, 2, 4), matrix1);
        }

        [TestMethod()]
        public void InverseTest() {
            Matrix2D matrix1 = new Matrix2D(1, 2, 3, 4);
            Matrix2D matrix2 = matrix1.Inverse.Inverse;

            PrecisionAssert.AreEqual(matrix1.E00, matrix2.E00, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E01, matrix2.E01, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E10, matrix2.E10, 1e-30);
            PrecisionAssert.AreEqual(matrix1.E11, matrix2.E11, 1e-30);
        }

        [TestMethod()]
        public void OperatorTest() {
            Matrix2D matrix1 = new(3, 4, 5, 6);
            Matrix2D matrix2 = new(5, 7, 9, 2);
            Matrix2D matrix3 = new(4, 5, 6, 7);
            Matrix2D matrix4 = new(3, 4, 5, 6);

            Assert.AreEqual(new Matrix2D(3, 4, 5, 6), +matrix1);
            Assert.AreEqual(new Matrix2D(-3, -4, -5, -6), -matrix1);
            Assert.AreEqual(new Matrix2D(8, 11, 14, 8), matrix1 + matrix2);
            Assert.AreEqual(new Matrix2D(-2, -3, -4, 4), matrix1 - matrix2);
            Assert.AreEqual(new Matrix2D(2, 3, 4, -4), matrix2 - matrix1);
            Assert.AreEqual(new Matrix2D(51, 29, 79, 47), matrix1 * matrix2);
            Assert.AreEqual(new Matrix2D(50, 62, 37, 48), matrix2 * matrix1);
            Assert.AreEqual(new Matrix2D(6, 8, 10, 12), 2d * matrix1);
            Assert.AreEqual(new Matrix2D(6, 8, 10, 12), (ddouble)2 * matrix1);
            Assert.AreEqual(new Matrix2D(6, 8, 10, 12), matrix1 * 2d);
            Assert.AreEqual(new Matrix2D(6, 8, 10, 12), matrix1 * (ddouble)2);
            Assert.AreEqual(new Matrix2D(1.5, 2, 2.5, 3), matrix1 / 2d);
            Assert.AreEqual(new Matrix2D(1.5, 2, 2.5, 3), matrix1 / (ddouble)2d);

            Assert.IsTrue(matrix1 == matrix4);
            Assert.IsFalse(matrix2 == matrix3);
            Assert.IsTrue(matrix1 != matrix2);
        }

        [TestMethod()]
        public void EqualsTest() {
            Matrix2D matrix1 = new(1, 2, 3, 4);
            Matrix2D matrix2 = new(1, 3, 5, 7);
            Matrix2D matrix3 = new(1, 2, 3, 4);

            Assert.IsTrue(matrix1.Equals(matrix1));
            Assert.IsFalse(matrix1.Equals(matrix2));
            Assert.IsTrue(matrix1.Equals(matrix3));
            Assert.IsFalse(matrix1.Equals(null));
        }

        [TestMethod()]
        public void StaticMatrixTest() {
            Matrix2D matrix1 = Matrix2D.Zero;
            Matrix2D matrix2 = Matrix2D.Identity;
            Matrix2D matrix3 = Matrix2D.Invalid;

            Assert.AreEqual(0, matrix1.E00);
            Assert.AreEqual(0, matrix1.E01);
            Assert.AreEqual(0, matrix1.E10);
            Assert.AreEqual(0, matrix1.E11);

            Assert.AreEqual(1, matrix2.E00);
            Assert.AreEqual(0, matrix2.E01);
            Assert.AreEqual(0, matrix2.E10);
            Assert.AreEqual(1, matrix2.E11);

            Assert.IsTrue(ddouble.IsNaN(matrix3.E00));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E01));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E10));
            Assert.IsTrue(ddouble.IsNaN(matrix3.E11));
        }

        [TestMethod()]
        public void RotateTest() {
            Vector2D vector = new(1, 2);

            Vector2DAssert.AreEqual(new Vector2D(1, 2), Matrix2D.Rotate(0) * vector, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(-2, 1), Matrix2D.Rotate(ddouble.Pi / 2) * vector, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(-1, -2), Matrix2D.Rotate(ddouble.Pi) * vector, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(2, -1), Matrix2D.Rotate(ddouble.Pi * 3 / 2) * vector, 1e-30);
        }

        [TestMethod()]
        public void ScaleTest() {
            Vector2D vector = new(1, 2);

            Assert.AreEqual(new Vector2D(2, 6), Matrix2D.Scale(2, 3) * vector);
        }

        [TestMethod()]
        public void DetTest() {
            Matrix2D matrix = new Matrix2D(5, 2, 3, 4);

            PrecisionAssert.AreEqual(((Matrix)matrix).Det, matrix.Det, 1e-30);
        }

        [TestMethod()]
        public void ToStringTest() {
            Matrix2D matrix1 = new(1, 2, 3, 4);

            Assert.AreEqual("[[1, 2], [3, 4]]", matrix1.ToString());
            Assert.AreEqual("[[1.00e0, 2.00e0], [3.00e0, 4.00e0]]", matrix1.ToString("e2"));
            Assert.AreEqual("[[1, 2], [3, 4]]", $"{matrix1}");
            Assert.AreEqual("[[1.00e0, 2.00e0], [3.00e0, 4.00e0]]", $"{matrix1:e2}");
        }

        [TestMethod]
        public void IOTest() {
            const string filename_bin = "m2_iotest.bin";

            Matrix2D v = new ddouble[,] { { 1, 2 }, { 4, 5 } };

            using (BinaryWriter stream = new BinaryWriter(File.Open(filename_bin, FileMode.Create))) {
                stream.Write(v);
            }

            Matrix2D u;

            using (BinaryReader stream = new BinaryReader(File.OpenRead(filename_bin))) {
                u = stream.ReadMatrix2D();
            }

            Assert.AreEqual(v, u);

            File.Delete(filename_bin);
        }
    }
}