using dls_SqlServerQueryMapper.Factory;
using dls_SqlServerQueryMapper.Model.Connection;
using dls_SqlServerQueryMapper.Model.Connection.Impl;
using Microsoft.Extensions.Configuration;
using sqlQueryMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Attributes.Impl
{
    /// <summary>
    /// Attribute for database source access
    /// </summary>
    public class DBSourceAttribute : Attribute, IDBAttributeAspect
    {
        /// <inheritdoc/>
        public bool IsTransactional { get; set; }

        /// <inheritdoc/>
        public object ExecuteAspect(object caller, MethodInfo method, object[] args, IConfiguration conf = null)
        {
            // Caller has to implement IDBDaoFactory interface
            if (!caller.GetType().GetInterfaces().Contains(typeof(IDBDaoFactory)))
                throw new Exception($"Can't execute {typeof(DBSourceAttribute)} attribute for an object that doesn't implement IDBDaoFactory");

            // Method parameters
            var paramsMethod = method.GetParameters();
            if (paramsMethod.Length < 1)
                throw new Exception($"Can't execute {typeof(DBSourceAttribute)} attribute for a method that doesn't have at least one parameter (IDatabaseSource)");

            if (paramsMethod.Last().ParameterType != typeof(IDBSource))
                throw new Exception($"Can't execute {typeof(DBSourceAttribute)} attribute for a method whose last parameter is not IDatabaseSource");

            // Get the database source and execute main method whne it is null
            IDBSource databaseSource = (IDBSource)args.Last();
            bool databaseSourceWasNull = databaseSource == null;

            try
            {
                // Create database source if needed
                if (databaseSource == null)
                {
                    databaseSource = ((DBDaoFactory)caller).SetDatabaseSource(IsTransactional);

                    // Replace database source on args to invoke
                    args[args.Length - 1] = databaseSource;
                }

                // Execute non aspect logic (original method call).
                var result = method.Invoke(caller, args);

                // Dispose database source
                if (databaseSourceWasNull)
                {
                    if (IsTransactional)
                        ((DBTransaction)databaseSource).Commit();

                    ((DBDaoFactory)caller).ResetDatabaseSource();
                    databaseSource = null;
                }

                return result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (IsTransactional)
                {
                    DBTransaction tx = (DBTransaction)databaseSource;
                    if (tx != null && tx.IsActive)
                        tx.Rollback();
                }

                Exception innerException = targetInvocationException.InnerException;
                if (innerException != null)
                    ExceptionDispatchInfo.Capture(innerException).Throw();

                throw;
            }
            catch (Exception exception)
            {
                if (IsTransactional)
                {
                    DBTransaction tx = (DBTransaction)databaseSource;
                    if (tx != null && tx.IsActive)
                        tx.Rollback();
                }

                string description = $"Exception on {GetType().Name}.ExecuteAspect() method: {exception.Message}";
                throw new Exception(description, exception);
            }
            finally
            {
                if (databaseSourceWasNull && databaseSource != null)
                    ((DBDaoFactory)caller).ResetDatabaseSource();
            }
        }

    }
}
