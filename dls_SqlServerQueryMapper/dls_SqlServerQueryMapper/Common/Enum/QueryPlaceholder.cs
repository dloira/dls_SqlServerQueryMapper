using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Common.Enum
{
    /// <summary>
    /// Placeholders to replace
    /// </summary>
    public static class QueryPlaceholder
    {
        /// <summary>
        /// WHERE conditions
        /// </summary>
        public static readonly string WHERE_LABEL = "${whereConditions}";

        /// <summary>
        /// PAGINATION conditions
        /// </summary>
        public static readonly string PAGINATION_LABEL = "${paginationConditions}";
    }
}
