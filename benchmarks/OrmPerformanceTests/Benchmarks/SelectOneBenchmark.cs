using BenchmarkDotNet.Attributes;

namespace OrmPerformanceTests.Benchmarks
{
    using Dapper;
    using EFCore;
    using LtQuery;

    [Config(typeof(BenchmarkConfig))]
    public class SelectOneBenchmark : IBenchmark
    {
        private LtQueryBenchmark _fastORMBenchmark;
        private DapperBenchmark _dapperBenchmark;
        private EFCoreBenchmark _eFCoreBenchmark;

        [GlobalSetup]
        public void Setup()
        {
            _fastORMBenchmark = new LtQueryBenchmark();
            _dapperBenchmark = new DapperBenchmark();
            _eFCoreBenchmark = new EFCoreBenchmark();
            _fastORMBenchmark.Setup();
            _dapperBenchmark.Setup();
            _eFCoreBenchmark.Setup();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _fastORMBenchmark?.Cleanup();
            _dapperBenchmark.Cleanup();
            _eFCoreBenchmark?.Cleanup();
        }

        [Benchmark]
        public int LtQuery()
        {
            return _fastORMBenchmark.SelectOne();
        }

        [Benchmark]
        public int Dapper()
        {
            return _dapperBenchmark.SelectOne();
        }

        [Benchmark]
        public int EFCore()
        {
            return _eFCoreBenchmark.SelectOne();
        }
    }
}
