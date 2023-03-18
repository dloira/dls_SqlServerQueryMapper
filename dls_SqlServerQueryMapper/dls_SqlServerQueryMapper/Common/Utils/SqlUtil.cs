using dls_SqlServerQueryMapper.Common.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Common.Utils
{
    /// <summary>
    /// SQL queries utils
    /// </summary>
    public static class SqlUtil
    {
        /// <summary>
        /// Returns a string with specified value included 
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="type">value type</param>
        /// <returns></returns>
        public static string LiteralSQLValue(object value, Type type = null)
        {
            if (value == null || value.Equals(DBNull.Value))
                return "null";

            try
            {
                var valueType = type ?? value.GetType();
                var dataType = MapUtil.ToDAType(valueType);
                if (dataType == DataType.Unknow)
                    throw new Exception($"Couldn't to know DA parameter type for {valueType.FullName} class type.");

                string result = null;
                switch (dataType)
                {
                    case DataType.Boolean:
                        result = bool.Parse(value.ToString()).ToString();
                        break;

                    case DataType.Integer:
                        result = long.Parse(value.ToString()).ToString(CultureInfo.InvariantCulture);
                        break;

                    case DataType.Decimal:
                        result = decimal.Parse(value.ToString()).ToString(CultureInfo.InvariantCulture);
                        break;

                    case DataType.Timestamp:
                        DateTime date = (DateTime)value;
                        result = "'" + date.ToString(MapUtil.DATE_TIME_FORMAT, CultureInfo.InvariantCulture) + "'";
                        break;

                    case DataType.String:
                        result = "'" + value.ToString().Replace("'", "''") + "'";
                        break;

                    default:
                        throw new Exception($"Can't determine DataAccess Parameter Type for {valueType.FullName}!!!!");
                }

                return result;
            }
            catch (Exception e)
            {
                string description = $"Exception on LiteralSQLValue() method: {e.Message}";
                throw new Exception(description, e);
            }

        }
    }
}
