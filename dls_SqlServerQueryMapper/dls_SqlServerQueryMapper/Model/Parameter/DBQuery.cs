namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Data Access Query used on annotated methods with [DBQuery] attribute
    /// </summary>
    public class DBQuery
    {
        /// <summary>
        /// Query name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Query SQL
        /// </summary>
        public string Sql { get; }

        /// <summary>
        /// Where conditions
        /// </summary>
        public DBQueryWhereConditions WhereConditions { get; }

        /// <summary>
        /// Pagination conditions
        /// </summary>
        public DBQueryPagination PaginationConditions { get; }
    }
}
