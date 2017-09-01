using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CryptoExchange.Common
{
    static class EnumHelper
    {

        public static string EnumToString(this Enum enumVal)
        {
            var fieldInfo = enumVal.GetType().GetTypeInfo().GetDeclaredField(enumVal.ToString());
            return fieldInfo.GetCustomAttribute<EnumMemberAttribute>().Value;
        }

    }
}
