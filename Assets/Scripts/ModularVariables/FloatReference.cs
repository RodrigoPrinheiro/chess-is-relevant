using System;
using UnityEngine;
namespace ModularVariables
{
    [Serializable]
    public class FloatReference
    {
        [SerializeField] private bool _useConstant = true;
        [SerializeField] private float _constantValue;
        [SerializeField] private FloatVariable _modularValue;

        public static implicit operator float(FloatReference f) => f.Value;
        public float Value
        {
            get
            {
                return _useConstant ? 
                    _constantValue : _modularValue.Value;
            }
            set
            {
                _modularValue.Value = value;
            }
        }
    }
}