using System.Diagnostics.Tracing;
using SFML.Graphics;

namespace SFMLTest.Entities
{
    public abstract class Entity : Drawable
    {
        protected Entity(int health, float speed)
        {
            Speed = speed;
            Health = new EntityAttribute("Health", health);
            Opacity = 0;
        }

        private float _lastTimeGettingHit;

        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Opacity { get; set; }
        public float Speed { get; private set; }
        public abstract int RenderPriority { get; }
        public bool IsInvincible { get { return _lastTimeGettingHit + InvisibilityTime > Game.ElapsedTime; } }

        protected abstract float InvisibilityTime { get; }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public float Angle { get; set; }
        public EntityAttribute Health { get; private set; }

        public abstract void Update(float dTime);
        public abstract void Draw(RenderTarget target, RenderStates states);

        public delegate void DeathEvent(Entity sender, Entity killer);

        public virtual event DeathEvent Death;
        public virtual void Die(Entity entity)
        {
            if (Death != null)
            {
                Death(this, entity);
            }
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