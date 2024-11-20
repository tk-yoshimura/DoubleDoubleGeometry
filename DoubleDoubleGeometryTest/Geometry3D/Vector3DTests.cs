using Algebra;
using DoubleDouble;
using DoubleDoubleComplex;
using DoubleDoubleGeometry.Geometry2D;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Vector3DTests {
        [TestMethod()]
        public void Vector3DTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new Vector2D(1, 2);

            Assert.AreEqual(1.0, vector1.X);
            Assert.AreEqual(2.0, vector1.Y);
            Assert.AreEqual(3.0, vector1.Z);

            Assert.AreEqual(new Vector(1, 2, 3), (Vector)vector1);
            Assert.AreEqual(new Vector(1, 2, 0), (Vector)vector2);

            CollectionAssert.AreEqual(new ddouble[] { 1, 2, 3 }, (ddouble[])vector1);
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
            Assert.AreEqual(new Vector3D(4, 10, 18), vector1 * vector2);
            Assert.AreEqual(new Vector3D(3, 3, 3), vector2 - vector1);
            Assert.AreEqual(new Vector3D(2, 4, 6), vector1 * 2d);
            Assert.AreEqual(new Vector3D(2, 4, 6), vector1 * (ddouble)2d);
            Assert.AreEqual(new Vector3D(2, 4, 6), 2d * vector1);
            Assert.AreEqual(new Vector3D(2, 4, 6), (ddouble)2d * vector1);
            Assert.AreEqual(new Vector3D(0.5, 1, 1.5), vector1 / 2d);
            Assert.AreEqual(new Vector3D(0.5, 1, 1.5), vector1 / (ddouble)2d);

            Assert.IsTrue(vector1 == vector3);
            Assert.IsFalse(vector2 == vector3);
            Assert.IsTrue(vector1 != vector2);

            Quaternion q = (11, 9, 2, 7);

            Quaternion qvq = q * (Quaternion)vector2 * q.Conj;
            Quaternion qvq_conc = (Quaternion)(q * vector2);

            Assert.AreEqual(qvq, qvq_conc);

            Assert.AreEqual(new Quaternion(0, 1, 2, 3), (Quaternion)vector1);
        }

        [TestMethod()]
        public void NormalizeSignTest() {
            ddouble[] tests = [-1, -0.0, 0.0, 1, ddouble.NaN];

            foreach (ddouble x in tests) {
                foreach (ddouble y in tests) {
                    foreach (ddouble z in tests) {
                        Vector3D v = (x, y, z);
                        Vector3D u = Vector3D.NormalizeSign(v);

                        Console.WriteLine(v);
                        Console.WriteLine(u);
                        Console.WriteLine("");

                        if (Vector3D.IsNaN(v)) {
                            Assert.IsTrue(Vector3D.IsNaN(u));
                            continue;
                        }

                        Assert.IsTrue(v == u || v == -u);
                        PrecisionAssert.IsPositive(u.X);

                        if (u.X == 0d) {
                            PrecisionAssert.IsPositive(u.Y);

                            if (u.Y == 0d) {
                                PrecisionAssert.IsPositive(u.Z);
                            }
                        }

                        if (u.X == 0d) {
                            PrecisionAssert.IsPlusZero(u.X);
                        }
                        if (u.Y == 0d) {
                            PrecisionAssert.IsPlusZero(u.Y);
                        }
                        if (u.Z == 0d) {
                            PrecisionAssert.IsPlusZero(u.Z);
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void TupleTest() {
            Vector3D vector = (1, 2, 3);
            (ddouble x, ddouble y, ddouble z) = vector;

            Assert.AreEqual(1.0, vector.X);
            Assert.AreEqual(2.0, vector.Y);
            Assert.AreEqual(3.0, vector.Z);
            Assert.AreEqual(1.0, x);
            Assert.AreEqual(2.0, y);
            Assert.AreEqual(3.0, z);
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
        public void DotTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 6, 9);

            Assert.AreEqual(43, Vector3D.Dot(vector1, vector2));
        }

        [TestMethod()]
        public void ScaleBTest() {
            Vector3D vector1 = new(4, -6, 9);

            Assert.AreEqual(3, vector1.MaxExponent);

            Assert.AreEqual((2, -3, 4.5), Vector3D.ScaleB(vector1, -1));
        }

        [TestMethod()]
        public void CrossTest() {
            Vector3D vector1 = new(1, 2, 3);
            Vector3D vector2 = new(4, 6, 9);

            Assert.AreEqual(-Vector3D.Cross(vector2, vector1), Vector3D.Cross(vector1, vector2));
            Assert.AreEqual(new Vector3D(2 * 9 - 3 * 6, 3 * 4 - 1 * 9, 1 * 6 - 2 * 4), Vector3D.Cross(vector1, vector2));
        }

        [TestMethod()]
        public void MaxAbsIndexTest() {
            Assert.AreEqual(0, Vector3D.MaxAbsIndex((3, 1, 2)));
            Assert.AreEqual(0, Vector3D.MaxAbsIndex((3, 2, 1)));
            Assert.AreEqual(1, Vector3D.MaxAbsIndex((1, 3, 2)));
            Assert.AreEqual(1, Vector3D.MaxAbsIndex((2, 3, 1)));
            Assert.AreEqual(2, Vector3D.MaxAbsIndex((1, 2, 3)));
            Assert.AreEqual(2, Vector3D.MaxAbsIndex((2, 1, 3)));
            Assert.AreEqual(0, Vector3D.MaxAbsIndex((-3, -1, -2)));
            Assert.AreEqual(0, Vector3D.MaxAbsIndex((-3, -2, -1)));
            Assert.AreEqual(1, Vector3D.MaxAbsIndex((-1, -3, -2)));
            Assert.AreEqual(1, Vector3D.MaxAbsIndex((-2, -3, -1)));
            Assert.AreEqual(2, Vector3D.MaxAbsIndex((-1, -2, -3)));
            Assert.AreEqual(2, Vector3D.MaxAbsIndex((-2, -1, -3)));
        }

        [TestMethod()]
        public void IsZeroTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;
            Vector3D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector3D.IsZero(vector1));
            Assert.IsTrue(Vector3D.IsZero(vector2));
            Assert.IsFalse(Vector3D.IsZero(vector3));
            Assert.IsFalse(Vector3D.IsZero(vector4));
        }

        [TestMethod()]
        public void IsValidTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;
            Vector3D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsTrue(Vector3D.IsValid(vector1));
            Assert.IsTrue(Vector3D.IsValid(vector2));
            Assert.IsFalse(Vector3D.IsValid(vector3));
            Assert.IsFalse(Vector3D.IsValid(vector4));
        }

        [TestMethod()]
        public void IsNaNTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;
            Vector3D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector3D.IsNaN(vector1));
            Assert.IsFalse(Vector3D.IsNaN(vector2));
            Assert.IsTrue(Vector3D.IsNaN(vector3));
            Assert.IsFalse(Vector3D.IsNaN(vector4));
        }

        [TestMethod()]
        public void IsFiniteTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;
            Vector3D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsTrue(Vector3D.IsFinite(vector1));
            Assert.IsTrue(Vector3D.IsFinite(vector2));
            Assert.IsFalse(Vector3D.IsFinite(vector3));
            Assert.IsFalse(Vector3D.IsFinite(vector4));
        }

        [TestMethod()]
        public void IsInfinityTest() {
            Vector3D vector1 = new(0, 0, 1);
            Vector3D vector2 = Vector3D.Zero;
            Vector3D vector3 = Vector3D.Invalid;
            Vector3D vector4 = (ddouble.PositiveInfinity, ddouble.PositiveInfinity, ddouble.PositiveInfinity);

            Assert.IsFalse(Vector3D.IsInfinity(vector1));
            Assert.IsFalse(Vector3D.IsInfinity(vector2));
            Assert.IsFalse(Vector3D.IsInfinity(vector3));
            Assert.IsTrue(Vector3D.IsInfinity(vector4));
        }

        [TestMethod()]
        public void ToStringTest() {
            Vector3D vector1 = new(1, 2, 3);

            Assert.AreEqual("[1, 2, 3]", vector1.ToString());
            Assert.AreEqual("[1.00e0, 2.00e0, 3.00e0]", vector1.ToString("e2"));
            Assert.AreEqual("[1, 2, 3]", $"{vector1}");
            Assert.AreEqual("[1.00e0, 2.00e0, 3.00e0]", $"{vector1:e2}");
        }

        [TestMethod]
        public void IOTest() {
            const string filename_bin = "v3_iotest.bin";

            Vector3D v = (ddouble.Pi, ddouble.E, ddouble.RcpE);

            using (BinaryWriter stream = new BinaryWriter(File.Open(filename_bin, FileMode.Create))) {
                stream.Write(v);
            }

            Vector3D u;

            using (BinaryReader stream = new BinaryReader(File.OpenRead(filename_bin))) {
                u = stream.ReadVector3D();
            }

            Assert.AreEqual(v, u);

            File.Delete(filename_bin);
        }
    }
}