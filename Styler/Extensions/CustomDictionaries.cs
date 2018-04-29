using System;
using Styler.Core;

namespace UIStyler.Extensions
{
    [Serializable]
    public class StyleDataDictionary : SerializableDictionary<StyleType, StyleData> { }

    [Serializable]
    public class ThemeStyleDictionary : SerializableDictionary<string, StyleDataDictionary> { }
    
    [Serializable]
    public class StringBoolDictionary : SerializableDictionary<string, bool>{}
}