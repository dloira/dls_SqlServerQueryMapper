using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sqlQueryMapper.Attributes
{
    /// <summary>
    /// Interface for custom attributes
    /// </summary>
    public interface IDBAttributeAspect
    {
        /// <summary>
        /// Executes attribute aspect for specific caller object, method and arguments
        /// </summary>
        /// <param name="caller">object that calls method</param>
        /// <param name="method">method info</param>
        /// <param name="args">arguments</param>
        /// <param name="conf">configuration (if needed)</param>
        /// <returns>result of method execution</returns>
        object ExecuteAspect(object caller, MethodInfo method, object[] args, IConfiguration conf = null);
    }
}
