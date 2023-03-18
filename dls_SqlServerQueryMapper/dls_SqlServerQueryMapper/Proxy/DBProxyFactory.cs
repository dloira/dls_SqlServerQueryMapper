using dls_SqlServerQueryMapper.Attributes.Impl;
using Microsoft.Extensions.Configuration;
using sqlQueryMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sqlQueryMapper.Proxy
{
    /// <summary>
    ///  PROXY PATTERN to intermediate access to the decorated object
    ///  URL: https://refactoring.guru/es/design-patterns/proxy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DBProxyFactory<T> : DispatchProxy
    {
        /// <summary>
        /// Configuration to use
        /// </summary>
        public static IConfiguration Configuration { get; set; }


        /// <summary>
        /// Object decorated with the aspect you want to mediate
        /// </summary>
        private T _decorated;


        /// <summary>
        /// Base method of the PROXY pattern (it is executed every time you want to access the decorated object)
        /// </summary>
        /// <param name="targetMethod">target method to execute</param>
        /// <param name="args">arguments</param>
        /// <returns></returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object result = null;
            IDBAttributeAspect attributeAspect = this.GetAttributeAspect(targetMethod);
            if (attributeAspect == null)
            {   // if there is no attribute defined for method, execute as is
                result = targetMethod.Invoke(_decorated, args);
            }
            else
            {
                var methods = _decorated.GetType().GetMethods()
                    .Where(x => x.Name == targetMethod.Name)
                    .ToList();

                // execute related aspect for attribute
                var inheritedMethod = _decorated.GetType().GetMethods()
                    .FirstOrDefault(x => MatchMethods(x, targetMethod));

                result = attributeAspect.ExecuteAspect(_decorated, inheritedMethod, args, Configuration);
            }
            return result;
        }

        /// <summary>
        /// this method is called by runtime to create instance of proxy
        /// </summary>
        /// <param name="instance">instance of class to decorate</param>
        /// <param name="configuration">configuration to use</param>
        /// <returns></returns>
        public static T Create(T instance, IConfiguration configuration)
        {
            object proxy = Create<T, DBProxyFactory<T>>();
            ((DBProxyFactory<T>)proxy).SetParameters(instance, configuration);

            return (T)proxy;
        }

        private void SetParameters(T decorated, IConfiguration configuration)
        {
            _decorated = decorated;
            Configuration = configuration;
        }

        /// <summary>
        /// Returns an object with attribute aspect related to specified attribute, if it's defined for method
        /// </summary>
        /// <param name="methodInfo">method</param>
        /// <returns></returns>
        private IDBAttributeAspect GetAttributeAspect(MethodInfo methodInfo)
        {
            var attributes = methodInfo.GetCustomAttributes(true);
            var attributeAspects = new List<IDBAttributeAspect>();

            // first, try to get DASource attribute
            Attribute dbAttribute = attributes
                .FirstOrDefault(x => typeof(DBSourceAttribute).IsAssignableFrom(x.GetType()))
                as DBSourceAttribute;
            if (dbAttribute != null)
            {
                attributeAspects.Add((IDBAttributeAspect)dbAttribute);
            }

            // seconf, try to add DAQuery attribute
            dbAttribute = attributes
                .FirstOrDefault(x => typeof(DBQueryAttribute).IsAssignableFrom(x.GetType()))
                as DBQueryAttribute;
            if (dbAttribute != null)
            {
                attributeAspects.Add((IDBAttributeAspect)dbAttribute);
            }

            if (attributeAspects.Count > 1)
            {
                throw new Exception("Only one DAAttribute can be used for a method!!!");
            }


            return attributeAspects.FirstOrDefault();
        }


        /// <summary>
        /// Returns a boolean that informs if two method has exactly the same names and parameters
        /// </summary>
        /// <param name="method1">first method</param>
        /// <param name="method2">second method</param>
        /// <returns></returns>
        private bool MatchMethods(MethodInfo method1, MethodInfo method2)
        {
            bool result = false;

            if (method1 != null && method2 != null)
            {
                if (method1.Name == method2.Name)
                {
                    var argumentTypes1 = method1.GetParameters().Select(t => t.ParameterType.Name).ToList();
                    var argumentTypes2 = method2.GetParameters().Select(t => t.ParameterType.Name).ToList();
                    if (argumentTypes1.Count == argumentTypes2.Count)
                    {
                        result = true;
                        for (int i = 0; i < argumentTypes1.Count; i++)
                        {
                            if (argumentTypes1[i] != argumentTypes2[i])
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
