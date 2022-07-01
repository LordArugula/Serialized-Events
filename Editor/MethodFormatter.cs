using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Arugula.SerializedEvents.Editor
{
    public static class MethodFormatter
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();

        private static Dictionary<Type, string> typeNames = new Dictionary<Type, string>()
        {
            { typeof(string), "string" },
            { typeof(int), "int" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(long), "long" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" }
        };

        public static string GetFormattedName(MethodInfo method, string methodName = null)
        {
            Type returnType = method.ReturnType;
            if (returnType != typeof(void))
            {
                stringBuilder.Append(GetSimpleTypeName(returnType))
                    .Append(' ');
            }

            stringBuilder.Append(methodName ?? method.Name);

            ParameterInfo[] parameterInfos = method.GetParameters();
            stringBuilder.Append(' ')
                .Append('(');

            if (parameterInfos.Length > 0)
            {
                AppendParameters(parameterInfos);
            }

            stringBuilder.Append(')');

            string formattedMethod = stringBuilder.ToString();
            stringBuilder.Clear();
            return formattedMethod;
        }

        private static void AppendParameters(ParameterInfo[] parameterInfos)
        {
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameter = parameterInfos[i];
                Type parameterType = parameter.ParameterType;

                stringBuilder.Append(GetSimpleTypeName(parameterType));

                if (i + 1 < parameterInfos.Length)
                {
                    stringBuilder.Append(',')
                        .Append(' ');
                }
            }
        }

        private static string GetSimpleTypeName(Type type)
        {
            if (typeNames.TryGetValue(type, out string name))
            {
                return name;
            }
            else
            {
                return type.Name;
            }
        }
    }
}
