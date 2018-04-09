using UnityEditor;
using UnityEngine;
using Utf8Json;

namespace UIStyler.Library
{
    public class MaterialJsonFormatter : IJsonFormatter<Material>
    {
        public void Serialize(ref JsonWriter writer, Material value, IJsonFormatterResolver formatterResolver)
        {
            formatterResolver.GetFormatterWithVerify<int>()
                .Serialize(ref writer, value.GetInstanceID(), formatterResolver);
        }

        public Material Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            var id = formatterResolver.GetFormatterWithVerify<int>().Deserialize(ref reader, formatterResolver);
            return EditorUtility.InstanceIDToObject(id) as Material;
        }
    }
}