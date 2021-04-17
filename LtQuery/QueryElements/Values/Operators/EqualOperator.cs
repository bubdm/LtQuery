using System;

namespace LtQuery.QueryElements.Values.Operators
{
    public sealed class EqualOperator : Immutable<EqualOperator>, IBoolValue
    {
        public IValue Left { get; }

        /// <summary>
        /// when null, Default Parameter that Guessed from Left
        /// </summary>
        public IValue Right { get; }

        public EqualOperator(IValue left, IValue right = null)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right;
        }

        protected override int CreateHashCode()
        {
            var code = 0;
            AddHashCode(ref code, Left);
            AddHashCode(ref code, Right);
            return code;
        }

        public override bool Equals(EqualOperator other)
        {
            if (!Equals(Left, other.Left))
                return false;
            return Equals(Right, other.Right);
        }
    }
}
