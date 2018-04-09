using System;

namespace DefaultNamespace
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StylePathAttribute : Attribute
    {
        public string Path { get; set; }

        public StylePathAttribute(string path)
        {
            Path = path;
        }
    }
}