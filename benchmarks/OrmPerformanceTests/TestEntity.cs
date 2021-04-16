using System;

namespace OrmPerformanceTests
{
    public class TestEntity
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int? Code2 { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
