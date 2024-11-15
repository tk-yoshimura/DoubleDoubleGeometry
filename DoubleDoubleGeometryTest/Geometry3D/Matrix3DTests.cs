﻿using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Matrix3DTests {
        [TestMethod()]
        public void Matrix3DTest() {
            Matrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

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
            Matrix3D matrix1 = new Matrix3D(1, 2, 3, 4, 5, 6, 7, 8, 9).T;

            Assert.AreEqual(new Matrix3D(1, 4, 7, 2, 5, 8, 3, 6, 9), matrix1);
        }

        [TestMethod()]
        public void OperatorTest() {
            Matrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Matrix3D matrix2 = new(1, 3, 5, 7, 9, 11, 13, 15, 2);
            Matrix3D matrix3 = new(8, 9, 10, 11, 12, 13, 14, 15, 16);
            Matrix3D matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(new Matrix3D(1, 2, 3, 4, 5, 6, 7, 8, 9), +matrix1);
            Assert.AreEqual(new Matrix3D(-1, -2, -3, -4, -5, -6, -7, -8, -9), -matrix1);
            Assert.AreEqual(new Matrix3D(2, 5, 8, 11, 14, 17, 20, 23, 11), matrix1 + matrix2);
            Assert.AreEqual(new Matrix3D(0, -1, -2, -3, -4, -5, -6, -7, 7), matrix1 - matrix2);
            Assert.AreEqual(new Matrix3D(0, 1, 2, 3, 4, 5, 6, 7, -7), matrix2 - matrix1);
            Assert.AreEqual(new Matrix3D(72, 78, 84, 171, 186, 201, 270, 294, 318), matrix1 * matrix3);
            Assert.AreEqual(new Matrix3D(114, 141, 168, 150, 186, 222, 186, 231, 276), matrix3 * matrix1);
            Assert.AreEqual(new Matrix3D(2, 4, 6, 8, 10, 12, 14, 16, 18), 2d * matrix1);
            Assert.AreEqual(new Matrix3D(2, 4, 6, 8, 10, 12, 14, 16, 18), (ddouble)2d * matrix1);
            Assert.AreEqual(new Matrix3D(2, 4, 6, 8, 10, 12, 14, 16, 18), matrix1 * 2d);
            Assert.AreEqual(new Matrix3D(2, 4, 6, 8, 10, 12, 14, 16, 18), matrix1 * (ddouble)2d);
            Assert.AreEqual(new Matrix3D(0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5), matrix1 / 2d);
            Assert.AreEqual(new Matrix3D(0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5), matrix1 / (ddouble)2d);

            Assert.IsTrue(matrix1 == matrix4);
            Assert.IsFalse(matrix2 == matrix3);
            Assert.IsTrue(matrix1 != matrix2);
        }

        [TestMethod()]
        public void EqualsTest() {
            Matrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Matrix3D matrix2 = new(1, 3, 5, 7, 9, 11, 13, 15, 2);
            Matrix3D matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(matrix1.Equals(matrix1));
            Assert.IsFalse(matrix1.Equals(matrix2));
            Assert.IsTrue(matrix1.Equals(matrix3));
            Assert.IsFalse(matrix1.Equals(null));
        }

        [TestMethod()]
        public void StaticMatrixTest() {
            Matrix3D matrix1 = Matrix3D.Zero;
            Matrix3D matrix2 = Matrix3D.Identity;
            Matrix3D matrix3 = Matrix3D.Invalid;

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
        public void RotateXTest() {
            Vector3D vector = new(1, 2, 3);

            Vector3DAssert.AreEqual(new Vector3D(1, 2, 3), Matrix3D.RotateX(0) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(1, -3, 2), Matrix3D.RotateX(ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(1, -2, -3), Matrix3D.RotateX(ddouble.Pi) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(1, 3, -2), Matrix3D.RotateX(ddouble.Pi * 3 / 2) * vector, 1e-30);
        }

        [TestMethod()]
        public void RotateYTest() {
            Vector3D vector = new(1, 2, 3);

            Vector3DAssert.AreEqual(new Vector3D(1, 2, 3), Matrix3D.RotateY(0) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(3, 2, -1), Matrix3D.RotateY(ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1, 2, -3), Matrix3D.RotateY(ddouble.Pi) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-3, 2, 1), Matrix3D.RotateY(ddouble.Pi * 3 / 2) * vector, 1e-30);
        }

        [TestMethod()]
        public void RotateZTest() {
            Vector3D vector = new(1, 2, 3);

            Vector3DAssert.AreEqual(new Vector3D(1, 2, 3), Matrix3D.RotateZ(0) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-2, 1, 3), Matrix3D.RotateZ(ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1, -2, 3), Matrix3D.RotateZ(ddouble.Pi) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, -1, 3), Matrix3D.RotateZ(ddouble.Pi * 3 / 2) * vector, 1e-30);
        }

        [TestMethod()]
        public void RotateAxisTest() {
            Vector3D vector = new(1, 2, 3);

            Vector3DAssert.AreEqual(new Vector3D(1, -3, 2), Matrix3D.RotateAxis(new Vector3D(+1, 0, 0), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(1, 3, -2), Matrix3D.RotateAxis(new Vector3D(-1, 0, 0), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(3, 2, -1), Matrix3D.RotateAxis(new Vector3D(0, +1, 0), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-3, 2, 1), Matrix3D.RotateAxis(new Vector3D(0, -1, 0), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-2, 1, 3), Matrix3D.RotateAxis(new Vector3D(0, 0, +1), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(2, -1, 3), Matrix3D.RotateAxis(new Vector3D(0, 0, -1), ddouble.Pi / 2) * vector, 1e-30);
            Vector3DAssert.AreEqual(vector, Matrix3D.RotateAxis(vector, ddouble.Pi / 2) * vector, 1e-30);
        }

        [TestMethod()]
        public void ScaleTest() {
            Vector3D vector = new(1, 2, 3);

            Assert.AreEqual(new Vector3D(2, 6, 12), Matrix3D.Scale(2, 3, 4) * vector);
        }

        [TestMethod()]
        public void ToStringTest() {
            Matrix3D matrix1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual("[[1, 2, 3], [4, 5, 6], [7, 8, 9]]", matrix1.ToString());
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0], [4.00e0, 5.00e0, 6.00e0], [7.00e0, 8.00e0, 9.00e0]]", matrix1.ToString("e2"));
            Assert.AreEqual("[[1, 2, 3], [4, 5, 6], [7, 8, 9]]", $"{matrix1}");
            Assert.AreEqual("[[1.00e0, 2.00e0, 3.00e0], [4.00e0, 5.00e0, 6.00e0], [7.00e0, 8.00e0, 9.00e0]]", $"{matrix1:e2}");
        }
    }
}