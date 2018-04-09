using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UIStyler.Core
{
    [Serializable]
    public class UITheme : ICloneable
    {
        public string ThemeName;

        public List<string> StyleWrappers = new List<string>();
        
        public UITheme(string themeName)
        {
            ThemeName = themeName;
        }

        public object Clone()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream);
        }
    }
}