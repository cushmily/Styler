using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Utf8Json;

namespace UIStyler.Core
{
    [Serializable]
    public struct StyleWrapper
    {
        public Type Type;

        [TextArea]
        public string StyleData;

        public StyleWrapper(UIStyle style)
        {
            Type = style.GetType();
            StyleData = JsonSerializer.ToJsonString(style);
        }

        public string ToWarperData()
        {
            return JsonSerializer.ToJsonString(this);
        }
    }
}