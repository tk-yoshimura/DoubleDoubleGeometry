﻿using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Line3DTests {
        [TestMethod()]
        public void Line3DTest() {
            Line3D line1 = Line3D.FromDirection((6, 1, 3), (-1, 2, 3));
            Line3D line2 = Matrix3D.Scale(1, 2, 3) * line1 + (2, 4, 6);

            Vector3DAssert.AreEqual((6 * 1 + 2, 1 * 2 + 4, 3 * 3 + 6), line2.Origin, 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(-1 * 1, 2 * 2, 3 * 3).Normal, line2.Direction, 1e-30);
        }

        [TestMethod()]
        public void EqualTest() {
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-1, 2, 3)) == Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-2, 4, 6)) == Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 1, 7), (-1, 3, 3)) != Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
            Assert.IsTrue(Line3D.FromDirection((6, 2, 7), (-1, 2, 3)) != Line3D.FromDirection((6, 1, 7), (-1, 2, 3)));
        }

        [TestMethod()]
        public void OperatorTest() {
            Assert.AreEqual(Line3D.FromDirection((4, 5, 3), (1, 2, 6)), +(Line3D.FromDirection((4, 5, 3), (1, 2, 6))));
            Assert.AreEqual(Line3D.FromDirection((-4, -5, -3), (-1, -2, -6)), -(Line3D.FromDirection((4, 5, 3), (1, 2, 6))));
            Assert.AreEqual(Line3D.FromDirection((5, 9, 10), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) + (1, 4, 7));
            Assert.AreEqual(Line3D.FromDirection((3, 1, -4), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) - (1, 4, 7));
            Assert.AreEqual(Line3D.FromDirection((5, 9, 10), (1, 2, 6)), (1, 4, 7) + Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((-3, -1, 4), (-1, -2, -6)), (1, 4, 7) - Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) * (ddouble)2);
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) * (double)2);
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), (ddouble)2 * Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((8, 10, 6), (1, 2, 6)), (double)2 * Line3D.FromDirection((4, 5, 3), (1, 2, 6)));
            Assert.AreEqual(Line3D.FromDirection((2, 2.5, 1.5), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) / (ddouble)2);
            Assert.AreEqual(Line3D.FromDirection((2, 2.5, 1.5), (1, 2, 6)), Line3D.FromDirection((4, 5, 3), (1, 2, 6)) / (double)2);
        }

        [TestMethod()]
        public void PointTest() {
            Line3D line1 = Line3D.FromDirection(Vector3D.Zero, (1, 1, 1));
            Line3D line2 = Line3D.FromDirection(Vector3D.Zero, (1, 1, 1)) * 2;
            Line3D line3 = Line3D.FromDirection(Vector3D.Zero, (1, 1, 1)) * -2;
            Line3D line4 = Line3D.FromDirection((2, 3, 4), (1, 1, 1));
            Line3D line5 = Line3D.FromDirection(Vector3D.Zero, (3, 4, 5));
            Line3D line6 = Line3D.FromDirection(Vector3D.Zero, (4, 3, 5));

            Quaternion q = new Quaternion(2, 5, 3, 4).Normal;
            Matrix3D m = new(q);

            Line3D line7 = q * line4;
            Line3D line8 = m * line4;

            Vector3DAssert.AreEqual((0, 0, 0), line1.Point(0), 1e-30);
            Vector3DAssert.AreEqual((1, 1, 1), line1.Point(ddouble.Sqrt(3)), 1e-30);
            Vector3DAssert.AreEqual((2, 2, 2), line1.Point(ddouble.Sqrt(3) * 2), 1e-30);

            Vector3DAssert.AreEqual(line1.Point(0) * 2, line2.Point(0), 1e-30);
            Vector3DAssert.AreEqual(line1.Point(ddouble.Pi / 4), line2.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(line1.Point(ddouble.Pi / 2), line2.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(-line1.Point(0), line3.Point(0), 1e-30);
            Vector3DAssert.AreEqual(-line1.Point(ddouble.Pi / 4), line3.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(-line1.Point(ddouble.Pi / 2), line3.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(line1.Point(0) + (2, 3, 4), line4.Point(0), 1e-30);
            Vector3DAssert.AreEqual(line1.Point(ddouble.Pi / 4) + (2, 3, 4), line4.Point(ddouble.Pi / 4), 1e-30);
            Vector3DAssert.AreEqual(line1.Point(ddouble.Pi / 2) + (2, 3, 4), line4.Point(ddouble.Pi / 2), 1e-30);

            Vector3DAssert.AreEqual(new Vector3D(3, 4, 5).Normal * 5, line5.Point(5), 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(3, 4, 5).Normal * 10, line5.Point(10), 1e-30);

            Vector3DAssert.AreEqual(new Vector3D(4, 3, 5).Normal * 5, line6.Point(5), 1e-30);
            Vector3DAssert.AreEqual(new Vector3D(4, 3, 5).Normal * 10, line6.Point(10), 1e-30);

            Vector3DAssert.AreEqual(q * line4.Point(0), line7.Point(0), 1e-30);
            Vector3DAssert.AreEqual(q * line4.Point(1), line7.Point(1), 1e-30);

            Vector3DAssert.AreEqual(m * line4.Point(0), line8.Point(0), 1e-30);
            Vector3DAssert.AreEqual(m * line4.Point(1), line8.Point(1), 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Line3D.IsValid(Line3D.FromDirection((6, 1, 3), (-1, 2, 3))));
            Assert.IsFalse(Line3D.IsValid(Line3D.Invalid));
        }
    }
}