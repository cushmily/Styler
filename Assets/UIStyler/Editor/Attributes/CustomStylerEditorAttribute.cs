using System;

namespace UIStyler.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomStylerEditorAttribute : Attribute
    {
        public Type StylerType { get; private set; }

        public CustomStylerEditorAttribute(Type stylerType)
        {
            StylerType = stylerType;
        }
    }
}