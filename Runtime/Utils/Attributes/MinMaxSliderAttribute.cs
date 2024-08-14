using UnityEngine;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxSliderAttribute"/> class with specified min and max values.
        /// </summary>
        /// <param name="min">The minimum value for the slider.</param>
        /// <param name="max">The maximum value for the slider.</param>
        public MinMaxSliderAttribute(float min, float max)
        {
            this.MinSliderValue = min;
            this.MaxSliderValue = max;
        }
    }

    [System.Serializable]
    public class MinMaxSliderValue
    {
        public float MinSliderValue;
        public float MaxSliderValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxSliderValue"/> class with specified min and max values.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public MinMaxSliderValue(float min, float max)
        {
            MinSliderValue = min;
            MaxSliderValue = max;
        }

        /// <summary>
        /// Implicit conversion from float to <see cref="MinMaxSliderValue"/>, setting both MinValue and MaxValue to the same float value.
        /// </summary>
        /// <param name="value">Float value to set for both MinValue and MaxValue.</param>
        public static implicit operator MinMaxSliderValue(float value) => new MinMaxSliderValue(value, value);

        /// <summary>
        /// Implicit conversion from <see cref="MinMaxSliderValue"/> to float, returning the average of MinValue and MaxValue.
        /// </summary>
        /// <param name="sliderValue">MinMaxSliderValue instance to convert.</param>
        public static implicit operator float(MinMaxSliderValue sliderValue) => (sliderValue.MinSliderValue + sliderValue.MaxSliderValue) / 2f;
    }
}
