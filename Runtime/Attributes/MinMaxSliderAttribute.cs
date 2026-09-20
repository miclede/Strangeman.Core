using UnityEngine;
using UnityEngine.Serialization;

namespace Strangeman.Utils.Attributes
{
    /// <summary>
    /// Attribute to define a min-max slider range for float fields in Unity's inspector.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float MinSliderValue;
        public float MaxSliderValue;
        
        public MinMaxSliderAttribute(float min, float max)
        {
            this.MinSliderValue = min;
            this.MaxSliderValue = max;
        }
    }
}
