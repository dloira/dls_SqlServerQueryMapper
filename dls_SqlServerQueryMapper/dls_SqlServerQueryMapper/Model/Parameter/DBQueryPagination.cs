using System;

namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Represents a PAGINATION condition that can be used on a SQL query, composed by a set of conditions joined by an operator
    /// </summary>
    public class DBQueryPagination
    {
        /// <summary>
        /// Offset to return rows
        /// </summary>
        public int PageNumber { get; set; } = 0;

        /// <summary>
        /// Number of rows to return
        /// </summary>
        public int PageSize { get; set; } = 0;


        /// <summary>
        /// Returns effective SQL for a paginated query
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetSQL()
        {
            if (PageSize == 0)
                return "";

            try
            {
                string result = "";

                result = " OFFSET " + PageNumber * PageSize + " ROWS FETCH NEXT " + PageSize + " ROWS ONLY ";

                return result;
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(DBQueryPagination).GetType().Name}.GetSQL() method: {e.Message}";
                throw new Exception(description, e);
            }
        }
    }
}
