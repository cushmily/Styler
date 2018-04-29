using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ResourcePath("Type Cache")]
public static class TypeCache
{
    [SerializeField]
    private static readonly List<string> FilteredNameSpaces = new List<string>
    {
        "UnityEngine.UI."
    };

    public static Dictionary<string, Type> CachedTypes;

    private static bool StartWithFilteredNameSpaced(Type type)
    {
        var fullName = type.FullName;
        var filtered = FilteredNameSpaces;
        for (var i = 0; i < filtered.Count; i++)
        {
            if (fullName.StartsWith(filtered[i]))
            {
                return true;
            }
        }

        return false;
    }

    private static void BuildCache()
    {
        CachedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(StartWithFilteredNameSpaced)
            .ToDictionary(k => k.FullName, v => v);
    }

    public static bool TryFindType(string typeName, out Type t)
    {
        if (CachedTypes == null) BuildCache();

        if (CachedTypes.TryGetValue(typeName, out t)) return t != null;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            t = a.GetType(typeName);
            if (t != null)
                break;
        }

        CachedTypes[typeName] = t;
        return t != null;
    }
}