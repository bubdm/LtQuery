﻿using System;

namespace LtQuery.ORM.SQL.Tests
{
    class RandomEx : Random
    {
        public RandomEx(int seed) : base(seed) { }

        public char NextChar() => (char)(Next() % (0x5a - 0x41) + 0x41);

        public string NextString()
        {
            var str = string.Empty;
            for (var i = 0; i < 10; i++)
                str += NextChar();
            return str;
        }

        public DateTime NextDateTime() => new DateTime(Next() % 20 + 2000, Next() % 12 + 1, Next() % 20 + 1);
    }
}
