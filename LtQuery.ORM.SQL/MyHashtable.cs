using System;

namespace LtQuery.ORM.SQL
{
    class MyHashtableThirdOrder
    {
        private const int Capacity = 16;
        private readonly Node[][][] _nodes;

        public MyHashtableThirdOrder(int? capacity = null)
        {
            _nodes = new Node[Capacity][][];
        }
        public object this[object key]
        {
            get
            {
                var hashCode = key.GetHashCode();
                var code1 = hashCode % Capacity;
                var array1 = _nodes;
                var array2 = array1[code1];
                if (array2 == null)
                    return null;

                hashCode = hashCode / Capacity;
                var code2 = hashCode % Capacity;
                var array3 = array2[code2];
                if (array3 == null)
                    return null;

                hashCode = hashCode / Capacity;
                var code3 = hashCode % Capacity;
                var node = array3[code3];
                if (node == null)
                    return null;

                while (node != null)
                {
                    var key2 = node.Key;
                    if (Equals(key2.GetHashCode(), hashCode) && Equals(key2, key))
                        return node.Value;
                    node = node.Next;
                }
                return null;
            }
        }
        public void Add(object key, object value)
        {
            var hashCode = key.GetHashCode();
            var code1 = hashCode % Capacity;

            var array1 = _nodes;
            var array2 = array1[code1];
            if (array2 == null)
            {
                array2 = new Node[Capacity][];
                array1[code1] = array2;
            }

            hashCode = hashCode / Capacity;
            var code2 = hashCode % Capacity;
            var array3 = array2[code2];
            if (array3 == null)
            {
                array3 = new Node[Capacity];
                array2[code2] = array3;
            }

            hashCode = hashCode / Capacity;
            var code3 = hashCode % Capacity;
            var firstNode = array3[code3];
            var node = firstNode;
            while (node == null)
            {
                if (Equals(node.Key, key))
                    throw new Exception();
            }
            array3[code3] = new Node(key, value, firstNode);
        }
        public Ref GetRef(object key)
        {
            var hashCode = key.GetHashCode();
            var code1 = hashCode % Capacity;

            var array1 = _nodes;
            var array2 = array1[code1];
            if (array2 == null)
            {
                array2 = new Node[Capacity][];
                array1[code1] = array2;
            }

            hashCode = hashCode / Capacity;
            var code2 = hashCode % Capacity;
            var array3 = array2[code2];
            if (array3 == null)
            {
                array3 = new Node[Capacity];
                array2[code2] = array3;
            }

            hashCode = hashCode / Capacity;
            var code3 = hashCode % Capacity;
            var firstNode = array3[code3];
            var node = firstNode;
            while (node == null)
            {
                var key2 = node.Key;
                if (Equals(key2.GetHashCode(), hashCode) && Equals(key2, key))
                    return new Ref(node.Value, null);
            }

            return new Ref(null, (value) => array3[code3] = new Node(key, value, firstNode));
        }
    }
    class Ref
    {
        private readonly object _value;
        private readonly Action<object> _setter;
        public object Value { get => _value; set => _setter(value); }
        public Ref(object value, Action<object> setter)
        {
            _value = value;
            _setter = setter;
        }
    }
    //class GetRef
    //{
    //    public object Value { get; }
    //    public GetRef(object value)
    //    {
    //        Value = value;
    //    }
    //}
    //class SetRef
    //{
    //    private readonly Action<object> _setter;
    //    public SetRef(Action<object> setter)
    //    {
    //        _setter = setter;
    //    }
    //    public void SetValue(object value) => _setter(value);
    //}


    class Node
    {
        public object Key { get; }
        public object Value { get; }
        public Node Next { get; }
        public Node(object key, object value, Node next)
        {
            Key = key;
            Value = value;
            Next = next;
        }
    }
    readonly struct KeyValuePair
    {
        public object Key { get; }
        public object Value { get; }
        public KeyValuePair(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }

}
