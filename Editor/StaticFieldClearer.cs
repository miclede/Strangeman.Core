#if UNITY_EDITOR
using Strangeman.Utils.Attributes;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Automatically clears static fields marked with ClearOnLoadAttribute during subsystem registration.
/// </summary>
public class StaticFieldClearer
{
    /// <summary>
    /// Clears fields marked with ClearOnLoadAttribute on subsystem registration.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ClearFieldsWithAttribute()
    {
        Debug.Log("Clearing fields marked with ClearOnLoadAttribute.");

        // Find all fields with the ClearOnLoadAttribute
        var fieldInfos = TypeCache.GetFieldsWithAttribute<ClearOnLoadAttribute>();
        foreach (var fieldInfo in fieldInfos)
        {
            // Ensure the field is static
            if (!fieldInfo.IsStatic)
            {
                Debug.LogError($"Field '{fieldInfo.Name}' in type '{fieldInfo.DeclaringType.Name}' is marked with ClearOnLoadAttribute but is not static.");
                continue;
            }

            // Skip generic parameters
            if (fieldInfo.FieldType.IsGenericParameter)
            {
                Debug.LogError($"Field '{fieldInfo.Name}' in type '{fieldInfo.DeclaringType.Name}' is marked with ClearOnLoadAttribute but is a Generic Parameter.");
                continue;
            }

            // Clear the field by setting it to its default value
            var defaultValue = GetDefaultValue(fieldInfo.FieldType);
            fieldInfo.SetValue(null, defaultValue);

            Debug.Log($"Cleared field '{fieldInfo.Name}' in type '{fieldInfo.DeclaringType.Name}' to default value. {fieldInfo.GetCustomAttribute<ClearOnLoadAttribute>().Message}");
        }
    }

    /// <summary>
    /// Gets the default value for a given type.
    /// </summary>
    /// <param name="type">The type to get the default value for.</param>
    /// <returns>The default value of the type.</returns>
    private static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
#endif