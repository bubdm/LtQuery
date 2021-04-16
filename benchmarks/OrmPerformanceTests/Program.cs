using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrmPerformanceTests
{
    using Benchmarks;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1:checkAccums, 2:runBenchmarkDotNet, 3:myRunBenchmarks");
                Console.Write(':');
                var str = Console.ReadLine();
                switch (str)
                {
                    case "1":
                        checkAccums();
                        break;
                    case "2":
                        runBenchmarkDotNet();
                        break;
                    case "3":
                        myRunBenchmarks();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void runBenchmarkDotNet()
        {
            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(InitialBenchmark),
                typeof(SelectOneBenchmark),
                //typeof(SelectManyBenchmark),
                typeof(SelectAllBenchmark),
            });
            switcher.Run();
        }
        private static void checkAccums()
        {
            IBenchmark benchmark;
            List<int> accums;

            benchmark = new InitialBenchmark();
            benchmark.Setup();
            accums = new List<int>
            {
                benchmark.LtQuery(),
                benchmark.EFCore(),
                benchmark.Dapper(),
                //benchmark.Mongo()
            };
            benchmark.Cleanup();
            if (accums.Distinct().Count() != 1)
                throw new Exception($"{benchmark.GetType().Name}: Not match accums");

            benchmark = new SelectOneBenchmark();
            benchmark.Setup();
            accums = new List<int>
            {
                benchmark.LtQuery(),
                benchmark.EFCore(),
                benchmark.Dapper(),
                //benchmark.Mongo()
            };
            benchmark.Cleanup();
            if (accums.Distinct().Count() != 1)
                throw new Exception($"{benchmark.GetType().Name}: Not match accums");

            benchmark = new SelectAllBenchmark();
            benchmark.Setup();
            accums = new List<int>
            {
                benchmark.LtQuery(),
                benchmark.EFCore(),
                benchmark.Dapper(),
                //benchmark.Mongo()
            };
            benchmark.Cleanup();
            if (accums.Distinct().Count() != 1)
                throw new Exception($"{benchmark.GetType().Name}: Not match accums");
        }
        private static void myRunBenchmarks()
        {
            new MyBenchmarkRunner(new InitialBenchmark()).Run();
            new MyBenchmarkRunner(new SelectOneBenchmark()).Run();
            new MyBenchmarkRunner(new SelectAllBenchmark()).Run();
        }

        private static void initDatabase()
        {
            var init = new InitDatabase();
            init.Init();
        }
    }
}
