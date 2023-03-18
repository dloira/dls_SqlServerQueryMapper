using dls_SqlServerQueryMapper.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Represents a WHERE condition that can be used on a SQL query, composed by a set of conditions joined by an operator
    /// </summary>
    public class DBQueryWhereConditions
    {
        private ICollection<DBQueryCondition> _conditionCollection;

        /// <summary>
        /// Logic operator
        /// </summary>
        public LogicOperator Operator { get; set; } = LogicOperator.AND;

        /// <summary>
        /// Conditions collection
        /// </summary>
        public ICollection<DBQueryCondition> ConditionCollection
        {
            get
            {
                if (_conditionCollection == null)
                {
                    _conditionCollection = new List<DBQueryCondition>();
                }
                return _conditionCollection;
            }
            set
            {
                _conditionCollection = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditions">conditions list to set</param>
        /// <param name="op">logig operator</param>
        public DBQueryWhereConditions(ICollection<DBQueryCondition> conditions = null, LogicOperator op = LogicOperator.AND)
        {
            Operator = op;
            ConditionCollection = conditions;
        }


        /// <summary>
        /// Returns effective SQL for a collection of parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetSQL(ICollection<DBQueryParameter> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return "";
            }

            try
            {
                string result = "";

                if (ConditionCollection == null || ConditionCollection.Count == 0)
                    return result;

                List<string> sqlConditions = new List<string>();
                foreach (var condition in ConditionCollection)
                {
                    if (condition.SelectionParameterName == null)
                    {
                        sqlConditions.Add(condition.Sql);
                    }
                    else
                    {   
                        var selectionParameter = parameters
                            .Where(p => p.Name == condition.SelectionParameterName)
                            .FirstOrDefault();

                        
                        if (selectionParameter?.Value != null && selectionParameter.Value != DBNull.Value)
                            sqlConditions.Add(condition.Sql);
                    }
                }

                if (sqlConditions.Any())
                    result = "(" + string.Join(" " + Operator.ToString() + " ", sqlConditions) + ")";

                return result;
            }
            catch (Exception e)
            {
                string description = $"Exception on {typeof(DBQueryWhereConditions).GetType().Name}.GetSQL() method: {e.Message}";
                throw new Exception(description, e);
            }
        }
    }
}
