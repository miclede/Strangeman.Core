using System;
using System.Reflection;

namespace Strangeman.Utils.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Retrieves the type of a member based on the MemberInfo provided.
        /// </summary>
        /// <param name="memberInfo">The MemberInfo instance representing the member.</param>
        /// <returns>The type of the member (e.g., field type, property type, return type of method, event handler type).</returns>
        /// <exception cref="ArgumentException">Thrown if the MemberInfo type is unsupported.</exception>
        public static Type GetMemberInfoType(this MemberInfo memberInfo)
        {
            return memberInfo switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,                // For FieldInfo, return the type of the field
                PropertyInfo propertyInfo => propertyInfo.PropertyType,    // For PropertyInfo, return the type of the property
                MethodInfo methodInfo => methodInfo.ReturnType,             // For MethodInfo, return the return type of the method
                EventInfo eventInfo => eventInfo.EventHandlerType,         // For EventInfo, return the type of the event handler
                _ => throw new ArgumentException("Unsupported MemberInfo type", nameof(memberInfo)) // Handle unsupported MemberInfo types
            };
        }
    }
}
