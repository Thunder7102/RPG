namespace SFMLTest
{
    public class EntityAttribute
    {
        private float Current { get; set; }
        public int Max { get; private set; }

        public EntityAttribute(int max)
        {
            Current = max;
            Max = max;
        }

        public static EntityAttribute operator -(EntityAttribute attr, float value)
        {
            attr.Current -= value;
            if (attr.Current < 0) attr.Current = 0;
            return attr;
        }

        public static implicit operator float(EntityAttribute attr)
        {
            return attr.Current;
        }
    }
}