using SFML.Graphics;

namespace SFMLTest.Entities
{
    public class Enemy : EntityWithHead
    {
        public Entity Target { get; set; }
        public bool IsDying { get; set; }
        public float DeathStartTime { get; set; }
        public Vector2 DeathFacing { get; set; }

        public float MoveBackTime { get; set; }

        public Enemy(Entity target)
            : base(3, 100, Color.Red)
        {
            Target = target;
        }

        public override void Update(float dTime)
        {
            base.Update(dTime);
            if (IsDying)
            {
                if (Game.ElapsedTime - DeathStartTime > 1)
                {
                    Game.Entities.Remove(this);
                }
                Opacity = (float) (0.5 + (Game.ElapsedTime - DeathStartTime)*0.5);
                return;
            }
            if (MoveBackTime > Game.ElapsedTime)
            {
                Position -= GetDirection() * dTime * Speed * 10;
            }
            else
            {
                //Position += GetDirection() * dTime * Speed;
            }

            if ((Target.Position - Position).Length < 20)
            {
                if (!Target.IsInvincible)
                {
                    Target.Hit(this, 1);
                }
            }
        }

        public override void Die(Entity entity)
        {
            DeathFacing = GetDirection();
            DeathStartTime = Game.ElapsedTime;
            IsDying = true;
        }

        public override void Hit(Entity entity, float damage)
        {
            if (IsDying) return;
            base.Hit(entity, damage);
            MoveBackTime = Game.ElapsedTime + 0.05f;
        }

        protected override Vector2 GetDirection()
        {
            if (IsDying) return DeathFacing;
            return (Target.Position - Position).Normalize();
        }

        public override int RenderPriority
        {
            get { return 100; }
        }

        protected override float InvisibilityTime
        {
            get { return 0.3f; }
        }
    }
}