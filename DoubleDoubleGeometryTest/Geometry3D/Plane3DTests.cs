using DoubleDoubleGeometry.Geometry3D;

namespace DoubleDoubleGeometryTest.Geometry3D {
    [TestClass()]
    public class Plane3DTests {
        [TestMethod()]
        public void Plane3DTest() {
            Plane3D plane1 = new(new Vector3D(1, 2, 3), 4);
            Plane3D plane2 = Plane3D.FromIntersection(new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1));

            Vector3D normal1 = new Vector3D(1, 2, 3).Normal;
            Vector3D normal2 = new Vector3D(1, 1, 1).Normal;

            Assert.AreEqual(normal1, plane1.Normal);
            Assert.AreEqual(normal1.X, plane1.A);
            Assert.AreEqual(normal1.Y, plane1.B);
            Assert.AreEqual(normal1.Z, plane1.C);
            Assert.AreEqual(4, plane1.D);

            Assert.AreEqual(normal2, plane2.Normal);
            Assert.AreEqual(-normal2.X, plane2.D);
        }

        [TestMethod()]
        public void ValidTest() {
            Assert.IsTrue(Plane3D.IsValid(Plane3D.FromIntersection(new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1))));
            Assert.IsFalse(Plane3D.IsValid(Plane3D.Invalid));
        }
    }
}