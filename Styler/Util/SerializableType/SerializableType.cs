using System;
using UnityEngine;

[Serializable]
public class SerializableType : ISerializationCallbackReceiver
{
    [SerializeField]
    public Type Type;

    [SerializeField]
    private string _typeFullName;

    // constructors
    public SerializableType()
    {
        Type = null;
    }

    public SerializableType(Type t)
    {
        Type = t;
    }

    // allow SerializableType to implicitly be converted to and from System.Type
    public static implicit operator Type(SerializableType stype)
    {
        return stype.Type;
    }

    public static implicit operator SerializableType(Type t)
    {
        return new SerializableType(t);
    }

    // overload the == and != operators
    public static bool operator ==(SerializableType a, SerializableType b)
    {
        // If both are null, or both are same instance, return true.
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if ((object) a == null || (object) b == null)
        {
            return false;
        }

        // Return true if the fields match:
        return a.Type == b.Type;
    }

    public static bool operator !=(SerializableType a, SerializableType b)
    {
        return !(a == b);
    }

    // we don't need to overload operators between SerializableType and System.Type because we already enabled them to implicitly convert
    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }

    // overload the .Equals method
    public override bool Equals(object obj)
    {
        // If parameter cannot be cast to SerializableType return false.
        var p = obj as SerializableType;
        if ((object) p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return Type == p.Type;
    }

    public bool Equals(SerializableType p)
    {
        // If parameter is null return false:
        if ((object) p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return Type == p.Type;
    }

    #region Unity Serialize Callbacks

    public void OnBeforeSerialize()
    {
        if (Type != null) _typeFullName = Type.FullName;
    }

    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(_typeFullName))
        {
            if (!TypeCache.TryFindType(_typeFullName.Replace('/', '.'), out Type))
                Debug.LogError("Cant resolve " + _typeFullName);
        }
    }

    #endregion
}