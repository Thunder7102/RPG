using SFML.Graphics;

namespace SFMLTest.Entities
{
    public abstract class Entity : Drawable
    {
        protected Entity(int health, float speed)
        {
            Speed = speed;
            Health = new EntityAttribute(health);
            Opacity = 0;
        }

        private float _lastTimeGettingHit;

        //public int ID { get; set; }
        protected float Opacity { get; set; }
        protected float Speed { get; private set; }
        public abstract int RenderPriority { get; }
        public bool IsInvincible { get { return _lastTimeGettingHit + InvisibilityTime > Game.ElapsedTime; } }

        protected abstract float InvisibilityTime { get; }
        public Vector2 Position { get; set; }

        protected float Angle { get; set; }
        private EntityAttribute Health { get; set; }
        /// <summary>
        /// My boyfriend is a huge dork.
        /// </summary>
        /// <param name="dTime"></param>
        public abstract void Update(float dTime);
        public abstract void Draw(RenderTarget target, RenderStates states);

        protected virtual void Die(Entity entity)
        {
            Game.Entities.Remove(this);
        }

        public virtual void Hit(Entity entity, float damage)
        {
            if (Health.Max == 0) return;

            _lastTimeGettingHit = Game.ElapsedTime;
            Health -= damage;
            if (Health == 0)
            {
                Die(entity);
            }
        }
    }
}