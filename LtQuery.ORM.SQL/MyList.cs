namespace LtQuery.ORM.SQL
{
    class MyList
    {
        private readonly int _iterateCapacity;
        public object[][] _items;
        public int _arrayCount;
        public int Count { get; private set; }
        public MyList(int? iterateCapacity = null)
        {
            _iterateCapacity = iterateCapacity ?? 32;
            _items = new object[1][];
            _items[0] = new object[_iterateCapacity];
            Count = 0;
        }

        public object this[int index]
        {
            get
            {
                var index1 = Count / _iterateCapacity;
                var index2 = Count % _iterateCapacity;
                return _items[index1][index2];
            }
        }

        public ref object GetRef(int index)
        {
            var index1 = Count / _iterateCapacity;
            var index2 = Count % _iterateCapacity;
            return ref _items[index1][index2];
        }

        public void Add(object item)
        {
            var index1 = Count / _iterateCapacity;
            var index2 = Count % _iterateCapacity;
            if (_items.Length <= index1)
            {
                var newItems = new object[_items.Length + 1][];
                for (var i = 0; i < _items.Length; i++)
                    newItems[i] = _items[i];
                var array = new object[_iterateCapacity];
                _items[index1] = array;
                array[0] = item;
            }
            else
                _items[index1][index2] = item;
            Count++;
        }
    }
}
