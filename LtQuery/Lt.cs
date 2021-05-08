namespace LtQuery
{
    using Mutables;

    /// <summary>
    /// Lt-Objects Starting Class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class Lt
    {
        public static QueryFluent<TEntity> Query<TEntity>() => new QueryFluent<TEntity>();

        public static TProperty Arg<TProperty>() => default(TProperty);
        public static TProperty Arg<TProperty>(string name) => default(TProperty);
    }
}
