using System;
using System.Collections.Generic;
using UIStyler.Core;
using UnityEditor;

namespace UIStyler.Editor
{
    public abstract class UIStylerDrawer
    {
        public virtual void PreDrawStyler(UIStyle style)
        {
            EditorGUI.BeginChangeCheck();
        }

        public virtual bool PostDrawStyler(UIStyle style)
        {
            return EditorGUI.EndChangeCheck();
        }

        public abstract void OnDrawStyler(UIStyle style);

        private static bool IsInit { get; set; }

        public static UIStylerDrawer Default { get; private set; }

        private static readonly Dictionary<Type, UIStylerDrawer> StylerEditors = new Dictionary<Type, UIStylerDrawer>();

        private static void Initialize()
        {
            Default = new DefaultUIStylerDrawer();

            var assembly = typeof(UIStylerDrawer).Assembly;
            var types = assembly.GetTypes();
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var attributes = type.GetCustomAttributes(typeof(CustomStylerEditorAttribute), false);
                foreach (CustomStylerEditorAttribute csea in attributes)
                {
                    if (!StylerEditors.ContainsKey(csea.StylerType))
                    {
                        StylerEditors.Add(csea.StylerType, Activator.CreateInstance(type) as UIStylerDrawer);
                    }
                    else if (StylerEditors[csea.StylerType] == null)
                    {
                        StylerEditors[csea.StylerType] = Activator.CreateInstance(type) as UIStylerDrawer;
                    }
                }
            }

            IsInit = true;
        }

        public static UIStylerDrawer GetEditor(Type targetType)
        {
            if (!IsInit || StylerEditors.Count <= 0)
            {
                Initialize();
            }

            if (StylerEditors.ContainsKey(targetType))
            {
                return StylerEditors[targetType];
            }

            return Default;
        }
    }
}