namespace OrmPerformanceTests
{
    interface IBenchmark
    {
        void Setup();
        void Cleanup();
        int LtQuery();
        int Dapper();
        int EFCore();
    }
}
