using System;

namespace SFMLTest.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random Random;
        static RandomHelper()
        {
            Random = new Random();
        }
        public static float RandomFloat(float max)
        {
            return (float) (Random.NextDouble()*max);
        }
        public static byte RandomByte()
        {
            return (byte) Random.Next(255);
        }
    }
}