using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
public static partial class DataFactory
{
    private static readonly Dictionary<Type, Dictionary<string, Type>> Types 
        = new Dictionary<Type, Dictionary<string, Type>>();
    
    static DataFactory()
    {
        AddPlayerSkills();
    }

    private static void AddDataType<TBase>(string typeName, Type type)
    {
        var baseType = typeof(TBase);
        Debug.Assert(
            type == baseType
            || type.IsSubclassOf(baseType)
            || baseType.IsInterface && baseType.IsAssignableFrom(type),
            $"Type {type} should be same or subclass of {baseType}"
        );
        if (!Types.TryGetValue(baseType, out var dataTypesDict))
        {
            dataTypesDict = new Dictionary<string, Type>();
            Types.Add(baseType, dataTypesDict);
        }

        dataTypesDict.Add(typeName, type);
    }

    public static Type GetType(Type baseType, string typeName)
    {
        if (Types.TryGetValue(baseType, out var resultTypeDict)
            && resultTypeDict.TryGetValue(typeName, out var resultType))
        {
            return resultType;
        }

        throw new ArgumentException(
            $"[{nameof(DataFactory)}] Have no such type as {typeName} for base type {baseType}");
    }

    public static Type GetType<TBase>(string typeName)
    {
        return GetType(typeof(TBase), typeName);
    }
}
}