using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Common.Enum
{
    /// <summary>
    /// Logic operators to apply with joining conditions (AND/OR)
    /// </summary>
    public enum LogicOperator
    {
        /// <summary>
        /// AND operator
        /// </summary>
        AND = 1,

        /// <summary>
        /// OR operator
        /// </summary>
        OR = 2
    }
}
