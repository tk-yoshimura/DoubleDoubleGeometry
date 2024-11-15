using System;
using System.Numerics;

namespace DoubleDoubleGeometry {
    public interface IGeometry<TSelf, TVector> :
        IEquatable<TSelf>,
        IEqualityOperators<TSelf, TSelf, bool>,
        IAdditionOperators<TSelf, TVector, TSelf>,
        ISubtractionOperators<TSelf, TVector, TSelf>,
        IUnaryPlusOperators<TSelf, TSelf>,
        IUnaryNegationOperators<TSelf, TSelf>
#pragma warning disable CS8632
        where TSelf : IGeometry<TSelf, TVector>?
#pragma warning restore CS8632
        where TVector : IVector<TVector> {

        static abstract bool IsZero(TSelf v);
        static abstract bool IsFinite(TSelf v);
        static abstract bool IsInfinity(TSelf v);
        static abstract bool IsNaN(TSelf v);
        static abstract bool IsValid(TSelf v);

        static abstract TSelf Zero { get; }
        static abstract TSelf Invalid { get; }
    }
}
