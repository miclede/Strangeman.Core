using System;
using System.Reflection;

namespace Strangeman.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetMemberInfoType(this MemberInfo memberInfo)
        {
            return memberInfo switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                MethodInfo methodInfo => methodInfo.ReturnType,
                EventInfo eventInfo => eventInfo.EventHandlerType,
                _ => throw new ArgumentException("Unsupported MemberInfo type", nameof(memberInfo))
            };
        }
    }
}