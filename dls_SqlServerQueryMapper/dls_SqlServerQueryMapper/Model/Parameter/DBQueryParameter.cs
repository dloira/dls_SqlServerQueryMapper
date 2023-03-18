using dls_SqlServerQueryMapper.Common.Enum;
using dls_SqlServerQueryMapper.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Atomic query parameter
    /// </summary>
    public class DBQueryParameter
    {
        /// <summary>
        /// Constructor for single parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="type">Parameter type</param>
        public DBQueryParameter(string name, object value, Type type = null)
        {
            if (value == null)
                value = DBNull.Value;

            type ??= value?.GetType();
            if (type == null)
                throw new Exception($"Can't create a {typeof(DBQueryParameter).FullName} without value or type");

            CheckName(name);

            Name = name;
            Value = CreateCastedValue(value, type);
            Values = null;
            HasMultipleValues = false;

            ValueType = type;
            ParameterType = MapUtil.ToDAType(type);
        }

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value type
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Parameter type
        /// </summary>
        public DataType ParameterType { get; }

        /// <summary>
        /// Parameter value
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Parameter values list
        /// </summary>
        public IList<object> Values { get; }

        /// <summary>
        /// Flag for parameter values list
        /// </summary>
        public bool HasMultipleValues { get; }

        /// <summary>
        /// Checks parameter name
        /// </summary>
        /// <param name="name">Parameter name</param>
        private void CheckName(string name)
        {
            if (name == null)
                throw new Exception("Parameter name can't be null!!!");

            var regex = new Regex("^[a-zA-Z][0-9a-zA-Z_]*$");

            if (!regex.IsMatch(name))
                throw new Exception($"Parameter name of '{name}' is not valid; use a name only with letters, numbers or '_', and begining with a letter");
        }

        /// <summary>
        /// Returns the specified value object casted to class type used by DataAccess acoording to specified type.
        /// </summary>
        /// <param name="value">value to cast</param>
        /// <param name="type">original type</param>
        /// <returns>corresponding data value</returns>
        private object CreateCastedValue(object value, Type type)
        {
            if (value == null)
                return null;

            if (value.Equals(DBNull.Value) || value.Equals("DBNull.Value"))
                return DBNull.Value;

            try
            {
                if (type == null)
                    throw new Exception($"Can't create a value for [{value}] without a type!!!");

                var dataType = MapUtil.ToDAType(type);
                if (dataType == DataType.Unknow)
                    throw new Exception($"Invalid specified class type '{type}' for value: {value}.");

                object castedValue = null;
                switch (dataType)
                {
                    case DataType.Boolean:
                        castedValue = MapUtil.ToBool(value);
                        break;

                    case DataType.Integer:
                        castedValue = MapUtil.ToLong(value);
                        break;

                    case DataType.Decimal:
                        castedValue = MapUtil.ToDecimal(value);
                        break;

                    case DataType.Timestamp:
                        castedValue = MapUtil.ToDateTime(value);
                        break;

                    case DataType.String:
                        castedValue = value.ToString();
                        break;

                    default:
                        throw new Exception($"Data access data type of [{dataType}] unexpected!!!");
                }

                return castedValue;
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.CreateCastedValue() method with arguments value = [{value}] and type = [{type?.FullName}]: {e.Message}";
                throw new Exception(description, e);
            }
        }
    }
}
