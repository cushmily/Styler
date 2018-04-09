using System;

namespace DefaultNamespace
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StyleTypeAttribute : Attribute
    {
        public string Type { get; set; }

        public StyleTypeAttribute(string type)
        {
            Type = type;
        }
    }
}