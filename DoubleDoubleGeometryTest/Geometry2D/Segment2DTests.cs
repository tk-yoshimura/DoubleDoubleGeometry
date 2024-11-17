﻿using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Segment2DTests {
        [TestMethod()]
        public void Segment2DTest() {
            Segment2D segment1 = new(new Vector2D(6, 1), new Vector2D(-1, 2));
            Segment2D segment2 = Matrix2D.Scale(1, 2) * segment1 + (2, 4);

            PrecisionAssert.AreEqual(ddouble.Sqrt(7 * 7 + 1 * 1), segment1.Length, 1e-30);

            Vector2DAssert.AreEqual(new Vector2D(6 * 1 + 2, 1 * 2 + 4), segment2.V0, 1e-30);
            Vector2DAssert.AreEqual(new Vector2D(-1 * 1 + 2, 2 * 2 + 4), segment2.V1, 1e-30);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Segment2D.IsValid(new Segment2D(new Vector2D(6, 1), new Vector2D(-1, 2))));
            Assert.IsFalse(Segment2D.IsValid(Segment2D.Invalid));
        }
    }
}