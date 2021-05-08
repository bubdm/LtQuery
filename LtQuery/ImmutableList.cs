using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LtQuery
{
    /// <summary>
    /// IReadOnlyList to cache HashCode
    /// </summary>
    /// <typeparam name="T">Immutable</typeparam>
    public sealed class ImmutableList<T> : Immutable<ImmutableList<T>>, IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> _inner;
        public ImmutableList(params T[] values)
        {
            _inner = (IReadOnlyList<T>)values.Clone();
        }
        public ImmutableList(IEnumerable<T> values)
        {
            _inner = values.ToArray();
        }

        public int Count => _inner.Count;
        public T this[int index] => _inner[index];

        public override bool Equals(ImmutableList<T> other)
        {
            if (Count != other.Count)
                return false;
            for (var i = 0; i < Count; i++)
                if (!Equals(this[i], other[i]))
                    return false;
            return true;
        }

        protected override int CreateHashCode()
        {
            var code = 0;
            foreach (var elem in _inner)
                AddHashCode(ref code, elem);
            return code;
        }

        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
