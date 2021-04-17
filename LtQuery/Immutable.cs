using System;

namespace LtQuery
{
    /// <summary>
    /// immutable with caches HashCode
    /// </summary>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class Immutable<TDerived> : IEquatable<TDerived> where TDerived : Immutable<TDerived>
    {
        public int? _hashCode;
        public override int GetHashCode()
        {
            if (_hashCode == null)
                _hashCode = CreateHashCode();
            return _hashCode.Value;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is TDerived))
                return false;

            var other = (TDerived)obj;
            return Equals(other);
        }
        protected abstract int CreateHashCode();
        public abstract bool Equals(TDerived other);

        protected void AddHashCode(ref int code, object value) => code = unchecked((code * 5) ^ value?.GetHashCode() ?? 0);
    }
}
