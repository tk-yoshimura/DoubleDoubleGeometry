using DoubleDouble;
using DoubleDoubleGeometry.Geometry2D;
using PrecisionTestTools;

namespace DoubleDoubleGeometryTest.Geometry2D {
    public static class Vector2DAssert {
        public static void AreEqual(Vector2D expected, Vector2D actual, ddouble abserr) {
            PrecisionAssert.AreEqual(expected.X, actual.X, abserr);
            PrecisionAssert.AreEqual(expected.Y, actual.Y, abserr);
        }
    }
}
