using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;

namespace DoubleDoubleGeometryTest.Geometry2D {
    [TestClass()]
    public class Vector2DTests {
        [TestMethod()]
        public void Vector2DTest() {
            Vector2D vector = new(1, 2);

            Assert.AreEqual(1.0, vector.X);
            Assert.AreEqual(2.0, vector.Y);

            Assert.AreEqual(new Vector(1, 2), (Vector)vector);

            CollectionAssert.AreEqual(new ddouble[] { 1, 2 }, (ddouble[])vector);
        }

        [TestMethod()]
        public void OperatorTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = new(3, 4);
            Vector2D vector3 = new(1, 2);

            Assert.AreEqual(new Vector2D(1, 2), +vector1);
            Assert.AreEqual(new Vector2D(-1, -2), -vector1);
            Assert.AreEqual(new Vector2D(4, 6), vector1 + vector2);
            Assert.AreEqual(new Vector2D(-2, -2), vector1 - vector2);
            Assert.AreEqual(new Vector2D(2, 2), vector2 - vector1);
            Assert.AreEqual(new Vector2D(2, 4), vector1 * 2d);
            Assert.AreEqual(new Vector2D(2, 4), vector1 * (ddouble)2d);
            Assert.AreEqual(new Vector2D(2, 4), 2d * vector1);
            Assert.AreEqual(new Vector2D(2, 4), (ddouble)2d * vector1);
            Assert.AreEqual(new Vector2D(0.5, 1), vector1 / 2d);
            Assert.AreEqual(new Vector2D(0.5, 1), vector1 / (ddouble)2d);

            Assert.AreEqual(new Vector2D(3, 8), vector1 * vector2);
            Assert.AreEqual(new Vector2D(9, 16), vector2 * vector2);
            Assert.AreEqual(new Vector2D(3, 2), vector2 / vector1);

            Assert.AreEqual(new Vector2D(-4, 3), Complex.ImaginaryOne * vector2);

            Assert.IsTrue(vector1 == vector3);
            Assert.IsFalse(vector2 == vector3);
            Assert.IsTrue(vector1 != vector2);
        }

        [TestMethod()]
        public void TupleTest() {
            Vector2D vector = (1, 2);
            (ddouble x, ddouble y) = vector;

            Assert.AreEqual(1.0, vector.X);
            Assert.AreEqual(2.0, vector.Y);
            Assert.AreEqual(1.0, x);
            Assert.AreEqual(2.0, y);
        }

        [TestMethod()]
        public void NormTest() {
            Vector2D vector1 = new(1, 2);

            Assert.AreEqual(ddouble.Sqrt(5), vector1.Norm);
            Assert.AreEqual(5, vector1.SquareNorm);
        }

        [TestMethod()]
        public void NormalTest() {
            Vector2D vector1 = new Vector2D(1, 2).Normal;

            Assert.AreEqual(1 / ddouble.Sqrt(5), vector1.X);
            Assert.AreEqual(2 / ddouble.Sqrt(5), vector1.Y);
        }

        [TestMethod()]
        public void EqualsTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = new(3, 4);
            Vector2D vector3 = new(1, 2);

            Assert.IsTrue(vector1.Equals(vector1));
            Assert.IsFalse(vector1.Equals(vector2));
            Assert.IsTrue(vector1.Equals(vector3));
            Assert.IsFalse(vector1.Equals(null));
        }

        [TestMethod()]
        public void DistanceTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = new(4, 6);

            Assert.AreEqual(ddouble.Sqrt(25), Vector2D.Distance(vector1, vector2));
            Assert.AreEqual(25, Vector2D.SquareDistance(vector1, vector2));
        }

        [TestMethod()]
        public void DotTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = new(4, -6);

            Assert.AreEqual(-8.0, Vector2D.Dot(vector1, vector2));
        }

        [TestMethod()]
        public void MaxAbsIndexTest() {
            Assert.AreEqual(0, Vector2D.MaxAbsIndex((2, 1)));
            Assert.AreEqual(1, Vector2D.MaxAbsIndex((1, 2)));
            Assert.AreEqual(0, Vector2D.MaxAbsIndex((-2, -1)));
            Assert.AreEqual(1, Vector2D.MaxAbsIndex((-1, -2)));
            Assert.AreEqual(0, Vector2D.MaxAbsIndex((-2, 1)));
            Assert.AreEqual(1, Vector2D.MaxAbsIndex((1, -2)));
        }

        [TestMethod()]
        public void IsZeroTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = Vector2D.Zero;
            Vector2D vector3 = Vector2D.Invalid;
            Vector2D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector2D.IsZero(vector1));
            Assert.IsTrue(Vector2D.IsZero(vector2));
            Assert.IsFalse(Vector2D.IsZero(vector3));
            Assert.IsFalse(Vector2D.IsZero(vector4));
        }

        [TestMethod()]
        public void IsValidTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = Vector2D.Zero;
            Vector2D vector3 = Vector2D.Invalid;
            Vector2D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsTrue(Vector2D.IsValid(vector1));
            Assert.IsTrue(Vector2D.IsValid(vector2));
            Assert.IsFalse(Vector2D.IsValid(vector3));
            Assert.IsFalse(Vector2D.IsValid(vector4));
        }

        [TestMethod()]
        public void IsNaNTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = Vector2D.Zero;
            Vector2D vector3 = Vector2D.Invalid;
            Vector2D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector2D.IsNaN(vector1));
            Assert.IsFalse(Vector2D.IsNaN(vector2));
            Assert.IsTrue(Vector2D.IsNaN(vector3));
            Assert.IsFalse(Vector2D.IsNaN(vector4));
        }

        [TestMethod()]
        public void IsFiniteTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = Vector2D.Zero;
            Vector2D vector3 = Vector2D.Invalid;
            Vector2D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsTrue(Vector2D.IsFinite(vector1));
            Assert.IsTrue(Vector2D.IsFinite(vector2));
            Assert.IsFalse(Vector2D.IsFinite(vector3));
            Assert.IsFalse(Vector2D.IsFinite(vector4));
        }

        [TestMethod()]
        public void IsInfinityTest() {
            Vector2D vector1 = new(1, 2);
            Vector2D vector2 = Vector2D.Zero;
            Vector2D vector3 = Vector2D.Invalid;
            Vector2D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector2D.IsInfinity(vector1));
            Assert.IsFalse(Vector2D.IsInfinity(vector2));
            Assert.IsFalse(Vector2D.IsInfinity(vector3));
            Assert.IsTrue(Vector2D.IsInfinity(vector4));
        }

        [TestMethod()]
        public void ToStringTest() {
            Vector2D vector1 = new(1, 2);

            Assert.AreEqual("[1, 2]", vector1.ToString());
            Assert.AreEqual("[1.00e0, 2.00e0]", vector1.ToString("e2"));
            Assert.AreEqual("[1, 2]", $"{vector1}");
            Assert.AreEqual("[1.00e0, 2.00e0]", $"{vector1:e2}");
        }
    }
}