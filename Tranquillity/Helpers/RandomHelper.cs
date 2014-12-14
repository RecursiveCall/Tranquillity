using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace Tranquillity.Helpers
{
    /// <summary>
    /// Provides helper methods for generating random values
    /// </summary>
    public static class RandomHelper
    {
        static Random random = new Random();
        /// <summary>
        /// Random instance.
        /// </summary>
        public static Random Random
        {
            get { return random; }
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0
        /// </summary>
        public static float Float()
        {
            return (float)Random.NextDouble();
        }

        /// <summary>
        /// Returns a random float within a specified range.
        /// </summary>
        /// <param name="min">Lower bound of the float</param>
        /// <param name="max">Upper bound of the float</param>
        public static float FloatBetween(float min, float max)
        {
            return min + (float)(Random.NextDouble() * (max - min));
        }

        /// <summary>
        /// Returns a random integer within a specified range.
        /// </summary>
        /// <param name="min">Lower bound of the int</param>
        /// <param name="max">Upper bound of the int</param>
        public static int IntBetween(int min, int max)
        {
            return min + (int)(Random.NextDouble() * (max - min));
        }

        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        public static bool Boolean()
        {
            return Random.Next(2) == 0;
        }

        /// <summary>
        /// Returns a random Vector3 within a specified range
        /// </summary>
        public static Vector3 Vector3Between(Vector3 min, Vector3 max)
        {
            return Vector3.Lerp(min, max, Float());
        }

        /// <summary>
        /// Returns a random normalized vector
        /// </summary>
        /// <returns>A normalized vector</returns>
        public static Vector3 NormalizedVector3()
        {
            Vector3 vector = new Vector3(FloatBetween(-1.0f, 1.0f), FloatBetween(-1.0f, 1.0f), FloatBetween(-1.0f, 1.0f));
            vector.Normalize();

            return vector;
        }

        /// <summary>
        /// Returns a random color
        /// </summary>
        /// <returns></returns>
        public static Color Color()
        {
            return new Color(Float(), Float(), Float());
        }

        /// <summary>
        /// Returns a random color with a random alpha
        /// </summary>
        /// <returns></returns>
        public static Color ColorWithAlpha()
        {
            return new Color(Float(), Float(), Float(), Float());
        }

        /// <summary>
        /// Return a random color within a specified range
        /// </summary>
        /// <param name="min">Minimum color value</param>
        /// <param name="max">Maximum color value</param>
        public static Color ColorBetween(Color min, Color max)
        {
            return SharpDX.Color.Lerp(min, max, Float());
        }
    }
}
