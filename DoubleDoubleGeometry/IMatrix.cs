using DoubleDouble;
using System;
using System.Numerics;

namespace DoubleDoubleGeometry {
    public interface IMatrix<TSelf> :
        IEquatable<TSelf>,
        IEqualityOperators<TSelf, TSelf, bool>,
        IAdditionOperators<TSelf, TSelf, TSelf>,
        ISubtractionOperators<TSelf, TSelf, TSelf>,
        IMultiplyOperators<TSelf, TSelf, TSelf>,
        IUnaryPlusOperators<TSelf, TSelf>,
        IUnaryNegationOperators<TSelf, TSelf>
#pragma warning disable CS8632
        where TSelf : IMatrix<TSelf>? {
#pragma warning restore CS8632

        static abstract TSelf Transpose(TSelf m);

        TSelf T { get; }

        static abstract TSelf Invert(TSelf m);

        TSelf Inverse { get; }

        static abstract TSelf ScaleB(TSelf m, int n);

        ddouble Det { get; }

        int MaxExponent { get; }

        static abstract bool IsZero(TSelf m);
        static abstract bool IsFinite(TSelf m);
        static abstract bool IsInfinity(TSelf m);
        static abstract bool IsNaN(TSelf m);
        static abstract bool IsValid(TSelf m);
        static abstract bool IsIdentity(TSelf m);

        static abstract TSelf Zero { get; }
        static abstract TSelf Invalid { get; }
        static abstract TSelf Identity { get; }
    }
}
