namespace FlagsTest
{
    public static class MathExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue">minInclusive</param>
        /// <param name="maxValue">maxInclusive</param>
        /// <returns></returns>
        public static int Repeat (this int value, int minValue, int maxValue)
        {
            if (value < minValue || value > maxValue)
            {
                value = (value - minValue) % (maxValue - minValue + 1) + minValue;
            }
            return value;
        }

        public static float Repeat (this float value, float minValue, float maxValue)
        {
            if (value < minValue || value > maxValue)
            {
                value = (value - minValue) % (maxValue - minValue) + minValue;
            }
            return value;
        }
    }
}
