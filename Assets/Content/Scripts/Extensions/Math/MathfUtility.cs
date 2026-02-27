namespace Core.Extensions
{
    public static class MathfUtility
    {
        public static float Remap(this float value, float minFrom, float maxFrom, float minTo, float maxTo)
        {
            return minTo + (value - minFrom) * (maxTo - minTo) / (maxFrom - minFrom);
        }
    }
}