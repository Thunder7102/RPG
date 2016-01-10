using System;

namespace SFMLTest
{
    public class EntityAttribute
    {
        public string Name { get; set; }
        public float Current { get; set; }
        public int Max { get; set; }

        public EntityAttribute(string name, int max)
        {
            Name = name;
            Current = max;
            Max = max;
        }

        public static EntityAttribute operator -(EntityAttribute attr, float value)
        {
            attr.Current -= value;
            if (attr.Current < 0) attr.Current = 0;
            Console.WriteLine("{0} is at {1}/{2}", attr.Name, attr.Current, attr.Max);
            return attr;
        }

        public static implicit operator float(EntityAttribute attr)
        {
            return attr.Current;
        }
    }
}