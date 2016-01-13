using System;
using SFML.System;
using SFMLTest.Helpers;

namespace SFMLTest
{
    public struct Vector2
    {
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X;
        public float Y;

        public static Vector2 operator -(Vector2 self, Vector2 other)
        {
            return new Vector2(self.X - other.X, self.Y - other.Y);
        }
        public static Vector2 operator +(Vector2 self, Vector2 other)
        {
            return new Vector2(self.X + other.X, self.Y + other.Y);
        }

        public static Vector2 operator *(Vector2 self, float factor)
        {
            return new Vector2(self.X * factor, self.Y * factor);
        }
        public static Vector2 operator *(Vector2 self, double factor)
        {
            return new Vector2((float) (self.X * factor), (float) (self.Y * factor));
        }
        public static Vector2 operator /(Vector2 self, float factor)
        {
            return new Vector2(self.X / factor, self.Y / factor);
        }
        public static Vector2 operator /(Vector2 self, double factor)
        {
            return new Vector2((float) (self.X / factor), (float) (self.Y / factor));
        }

        public static implicit operator Vector2f(Vector2 origin)
        {
            return new Vector2f(origin.X, origin.Y);
        }

        public static implicit operator Vector2(Vector2f origin)
        {
            return new Vector2(origin.X, origin.Y);
        }

        public Vector2 Normalize()
        {
            float length = Length;
            return new Vector2(X / length, Y / length);
        }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y); } }

        public static Vector2 Random(float maxX, float maxY)
        {
            return new Vector2(RandomHelper.RandomFloat(maxX), RandomHelper.RandomFloat(maxY));
        }

        public float AngleTo(Vector2 target)
        {
            return (float) (Math.Atan2(target.Y - Y, target.X - X) * 180 / Math.PI);
        }

        public static Vector2 FromAngle(float angle, float size)
        {
            return new Vector2(
                (float) (Math.Cos(angle / 180 * Math.PI) * size),
                (float) (Math.Sin(angle / 180 * Math.PI) * size)
            );
        }

        public override string ToString()
        {
            return string.Format("[{0:00},{1:00}]", X, Y);
        }
    }
}