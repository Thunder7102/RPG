using System;
using SFML.Graphics;

namespace SFMLTest.Entities
{
    public class Enemy : EntityWithHead
    {
        private Entity Target { get; set; }
        private bool IsDying { get; set; }
        private float DeathStartTime { get; set; }
        private Vector2 DeathFacing { get; set; }

        private float MoveBackTime { get; set; }
        private Vector2 MoveBackFrom { get; set; }

        public Enemy(Entity target)
            : base(3, 100, Color.Red)
        {
            Target = target;
        }

        public override void Update(float dTime)
        {
            base.Update(dTime);
            if (MoveBackTime > Game.ElapsedTime)
            {
                Console.WriteLine("Enemy {0}", Position);
                Position -= GetDirection() * dTime * Speed * 10;
            }

            if (IsDying)
            {
                if (Game.ElapsedTime - DeathStartTime > 1)
                {
                    Console.WriteLine("Removing enemy");
                    Game.Entities.Remove(this);
                }
                Opacity = (float)(0.5 + (Game.ElapsedTime - DeathStartTime) * 0.5);
                return;
            }
            if(MoveBackTime <= Game.ElapsedTime)
            {
                Position += GetDirection() * dTime * Speed;
            }

            if ((Target.Position - Position).Length < 20)
            {
                if (!Target.IsInvincible)
                {
                    Target.Hit(this, 1);
                }
            }
        }

        protected override void Die(Entity entity)
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
            MoveBackFrom = (entity.Position - Position).Normalize();
        }

        protected override Vector2 GetDirection()
        {
            if (IsDying) return DeathFacing;
            if (MoveBackTime > Game.ElapsedTime) return MoveBackFrom;
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