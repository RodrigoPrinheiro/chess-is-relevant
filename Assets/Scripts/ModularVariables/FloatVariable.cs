using UnityEngine;

namespace ModularVariables
{
    [CreateAssetMenu(menuName="Game Systems/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField]
        private float _value;
        public float Value 
        {
            get => _value;
            set => _value = value;
        }
    }
}