using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LinkedListLoop.src.server
{
    public class MethodFinder
    {
        public static object CallAjaxMethod(string serverSideMethod, object data)
        {
            Type type = Assembly.GetExecutingAssembly().GetType("LinkedListLoop.src.server.AjaxMethods");

            if (type != null)
            {
                MethodInfo methodInfo = type.GetMethod(serverSideMethod);
                if (methodInfo != null)
                {
                    object result = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(type, null);
                    if (parameters.Length == 0)
                    {
                        result = methodInfo.Invoke(classInstance, null);
                    }
                    else
                    {
                        Type dataType = parameters[0].ParameterType;
                        object dataParameter = null;

                        if (dataType == typeof(string))
                        {
                            dataParameter = string.Empty;
                        }
                        else
                        {
                            dataParameter = Activator.CreateInstance(dataType, null);
                        }

                        MethodInfo[] types = typeof(JsonConvert).GetMethods();
                        List<MethodInfo> desList = types.ToList().Where(a => a.IsGenericMethod == true && a.Name == "DeserializeObject" && a.GetParameters().Length == 1).ToList();

                        if (desList != null && desList.Count == 1)
                        {
                            MethodInfo method = desList[0];
                            MethodInfo generic = method.MakeGenericMethod(dataType);
                            object genericResult = generic.Invoke(dataParameter, new object[] { data.ToString() });

                            object[] parametersArray = new object[] { genericResult };

                            result = methodInfo.Invoke(classInstance, parametersArray);
                        }
                        else
                        {
                            throw new Exception(ResourceHelper.GetString("DeserializationMethodNotFound"));
                        }
                    }

                    return result;
                }
                else
                {
                    throw new Exception(ResourceHelper.GetString("MethodNotFoundReflection"));
                }
            }
            else
            {
                throw new Exception(ResourceHelper.GetString("TypeNotFoundReflection"));
            }
        }
    }
}