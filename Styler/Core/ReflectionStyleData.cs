using System.Linq;
using System.Reflection;
using UIStyler.Extensions;
using UnityEngine;

namespace Styler.Core
{
    // TODO Not finished.
    public abstract class ReflectionStyleData<T> : StyleData
    {
        protected virtual BindingFlags Flags
        {
            get { return BindingFlags.Public | BindingFlags.Instance; }
        }

        public StringBoolDictionary Fields = new StringBoolDictionary();

        protected virtual void OnEnable()
        {
            var type = typeof(T);
            var members = type.GetFields(Flags);
            if (Fields != null && Fields.Count == members.Length)
            {
                return;
            }

            Fields = new StringBoolDictionary();
            var fieldNames = members.Select(x => x.Name).ToList();
            foreach (var fieldName in fieldNames)
            {
                Fields.Add(fieldName, false);
            }
        }
    }
}