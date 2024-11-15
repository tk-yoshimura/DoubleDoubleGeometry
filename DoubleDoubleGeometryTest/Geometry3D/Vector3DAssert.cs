using DoubleDouble;
using DoubleDoubleGeometry.Geometry3D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry3D {
    public static class Vector3DAssert {
        public static void AreEqual(Vector3D expected, Vector3D actual, ddouble abserr) {
            PrecisionAssert.AreEqual(expected.X, actual.X, abserr);
            PrecisionAssert.AreEqual(expected.Y, actual.Y, abserr);
            PrecisionAssert.AreEqual(expected.Z, actual.Z, abserr);
        }
    }
}
