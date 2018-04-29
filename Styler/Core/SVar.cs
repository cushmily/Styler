using System;
using UnityEngine;

namespace Styler.Core
{
    public class SVar
    {
        [SerializeField]
        private bool _enable;

        public bool Enable
        {
            get { return _enable; }
        }

        public void SetEnable(bool enable)
        {
            _enable = enable;
        }
    }

    public class SVar<T> : SVar
    {
        [SerializeField]
        private T _value;

        public T Value
        {
            get { return _value; }
        }

        public void SetTargetValue(Action<T> Setter)
        {
            if (Enable && Setter != null) Setter.Invoke(Value);
        }
        
        public SVar(T defaultValue)
        {
            _value = defaultValue;
        }
    }
}