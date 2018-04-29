using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class ResourcePathAttribute : Attribute
{
    public ResourcePathAttribute(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            Debug.LogError("Invalid relative path! (its null or empty)");
        }
        else
        {
            if (relativePath[0] == '/')
            {
                relativePath = relativePath.Substring(1);
            }

            filepath = relativePath;
        }
    }

    public string filepath { get; set; }
}