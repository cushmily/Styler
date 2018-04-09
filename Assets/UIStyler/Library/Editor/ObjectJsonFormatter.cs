using UnityEditor;
using UnityEngine;
using Utf8Json;

namespace UIStyler.Library
{
    public class ObjectJsonFormatter : IJsonFormatter<Object>
    {
        public void Serialize(ref JsonWriter writer, Object value, IJsonFormatterResolver formatterResolver)
        {
            formatterResolver.GetFormatterWithVerify<int>()
                .Serialize(ref writer, value.GetInstanceID(), formatterResolver);
        }

        public Object Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            var id = formatterResolver.GetFormatterWithVerify<int>().Deserialize(ref reader, formatterResolver);
            return EditorUtility.InstanceIDToObject(id);
        }
    }
}