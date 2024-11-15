using DoubleDouble;
using System.Numerics;

namespace DoubleDoubleGeometry {
    public interface IVector<TSelf> :
        IGeometry<TSelf, TSelf>,

        IMultiplyOperators<TSelf, TSelf, TSelf>,
        IDivisionOperators<TSelf, TSelf, TSelf>

#pragma warning disable CS8632
        where TSelf : IVector<TSelf>? {
#pragma warning restore CS8632

        ddouble Norm { get; }
        ddouble SquareNorm { get; }

        TSelf Normal { get; }

        static abstract ddouble Distance(TSelf v1, TSelf v2);
        static abstract ddouble SquareDistance(TSelf v1, TSelf v2);

        static abstract ddouble Dot(TSelf v1, TSelf v2);
    }
}
