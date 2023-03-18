using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Common.Enum
{
    /// <summary>
    /// Enum for types of data to consider on data access
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Unknow
        /// </summary>
        Unknow = 0,

        /// <summary>
        /// String
        /// </summary>
        String = 10,

        /// <summary>
        /// Number (positive or negative) without decimals
        /// </summary>
        Integer = 20,

        /// <summary>
        /// Number (positive or negative) with decimal part
        /// </summary>
        Decimal = 30,

        /// <summary>
        /// A moment of time (date and time)
        /// </summary>
        Timestamp = 40,

        /// <summary>
        /// A logic binary value (true or false)
        /// </summary>
        Boolean = 50
    }
}
