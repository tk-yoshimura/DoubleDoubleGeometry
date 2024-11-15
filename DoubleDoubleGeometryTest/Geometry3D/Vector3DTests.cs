using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Vector3DTests {
        [TestMethod()]
        public void Vector3DTest() {
            Vector3D vector = new(1, 2, 3);

            Assert.AreEqual(1.0, vector.X);
            Assert.AreEqual(2.0, vector.Y);
            Assert.AreEqual(3.0, vector.Z);
        }

        [TestMethod()]
        public void OperatorTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 5, 6);
            Vector3D vector3 = new(1, 2, 3);

            Assert.AreEqual(new Vector3D(1, 2, 3), +vector1);
            Assert.AreEqual(new Vector3D(-1, -2, -3), -vector1);
            Assert.AreEqual(new Vector3D(5, 7, 9), vector1 + vector2);
            Assert.AreEqual(new Vector3D(-3, -3, -3), vector1 - vector2);
            Assert.AreEqual(new Vector3D(3, 3, 3), vector2 - vector1);
            Assert.AreEqual(new Vector3D(2, 4, 6), vector1 * 2);
            Assert.AreEqual(new Vector3D(2, 4, 6), 2 * vector1);
            Assert.AreEqual(new Vector3D(0.5, 1, 1.5), vector1 / 2);

            Assert.IsTrue(vector1 == vector3);
            Assert.IsFalse(vector2 == vector3);
            Assert.IsTrue(vector1 != vector2);
        }

        [TestMethod()]
        public void NormTest() {
            Vector3D vector1 = new(1, 2, 3);

            Assert.AreEqual(ddouble.Sqrt(14), vector1.Norm);
            Assert.AreEqual(14, vector1.SquareNorm);
        }

        [TestMethod()]
        public void NormalTest() {
            Vector3D vector1 = new Vector3D(1, 2, 3).Normal;

            Assert.AreEqual(1 / ddouble.Sqrt(14), vector1.X);
            Assert.AreEqual(2 / ddouble.Sqrt(14), vector1.Y);
            Assert.AreEqual(3 / ddouble.Sqrt(14), vector1.Z);
        }

        [TestMethod()]
        public void EqualsTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 5, 6);
            Vector3D vector3 = new(1, 2, 3);

            Assert.IsTrue(vector1.Equals(vector1));
            Assert.IsFalse(vector1.Equals(vector2));
            Assert.IsTrue(vector1.Equals(vector3));
            Assert.IsFalse(vector1.Equals(null));
        }

        [TestMethod()]
        public void DistanceTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 6, 9);

            Assert.AreEqual(ddouble.Sqrt(61), Vector3D.Distance(vector1, vector2));
            Assert.AreEqual(61, Vector3D.SquareDistance(vector1, vector2));
        }

        [TestMethod()]
        public void InnerProductTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 6, 9);

            Assert.AreEqual(43, Vector3D.Dot(vector1, vector2));
        }

        [TestMethod()]
        public void CrossTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 6, 9);

            Assert.AreEqual(-Vector3D.Cross(vector2, vector1), Vector3D.Cross(vector1, vector2));
            Assert.AreEqual(new Vector3D(2 * 9 - 3 * 6, 3 * 4 - 1 * 9, 1 * 6 - 2 * 4), Vector3D.Cross(vector1, vector2));
        }

        [TestMethod()]
        public void IsZeroTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;

            Assert.IsFalse(Vector3D.IsZero(vector1));
            Assert.IsTrue(Vector3D.IsZero(vector2));
        }

        [TestMethod()]
        public void IsValidTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;

            Assert.IsTrue(Vector3D.IsValid(vector1));
            Assert.IsTrue(Vector3D.IsValid(vector2));
            Assert.IsFalse(Vector3D.IsValid(vector3));
        }

        [TestMethod()]
        public void ToStringTest() {
            Vector3D vector1 = new(1, 2, 3);

            Assert.AreEqual("[1, 2, 3]", vector1.ToString());
            Assert.AreEqual("[1.00e0, 2.00e0, 3.00e0]", vector1.ToString("e2"));
            Assert.AreEqual("[1, 2, 3]", $"{vector1}");
            Assert.AreEqual("[1.00e0, 2.00e0, 3.00e0]", $"{vector1:e2}");
        }
    }
}