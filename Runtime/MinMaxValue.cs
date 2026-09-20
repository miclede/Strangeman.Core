namespace Strangeman.Utils
{
    [System.Serializable]
    public class MinMaxValue
    {
        public float minValue;
        public float maxValue;
        
        public MinMaxValue(float min, float max)
        {
            minValue = min;
            maxValue = max;
        }
        
        public static implicit operator MinMaxValue(float value) => new MinMaxValue(value, value);
        
        public static implicit operator float(MinMaxValue sliderValue) => (sliderValue.minValue + sliderValue.maxValue) / 2f;
    }
}