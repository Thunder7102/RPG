namespace SFMLTest.Entities
{
    public abstract class Projectile : Entity
    {
        protected IProjectileOwner Owner { get; private set; }

        protected Projectile(IProjectileOwner owner, float speed)
            : base(0, speed)
        {
            Owner = owner;
        }

        public override int RenderPriority
        {
            get { return 500; }
        }

        protected override float InvisibilityTime
        {
            get { return 0; }
        }

        protected override void Die(Entity entity)
        {
        }
    }
}