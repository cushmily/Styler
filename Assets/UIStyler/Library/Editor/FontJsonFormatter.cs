using UnityEditor;
using UnityEngine;
using Utf8Json;

namespace UIStyler.Library
{
    public class FontJsonFormatter : IJsonFormatter<Font>
    {
        public void Serialize(ref JsonWriter writer, Font value, IJsonFormatterResolver formatterResolver)
        {
            formatterResolver.GetFormatterWithVerify<int>()
                .Serialize(ref writer, value.GetInstanceID(), formatterResolver);
        }

        public Font Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            var id = formatterResolver.GetFormatterWithVerify<int>().Deserialize(ref reader, formatterResolver);
            return EditorUtility.InstanceIDToObject(id) as Font;
        }
    }
}