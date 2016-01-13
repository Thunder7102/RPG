using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace SFMLTest.Entities
{
    public sealed class SwordProjectile : Projectile
    {
        private float StartAngle { get; set; }
        private float EndAngle { get; set; }

        private float StartTime { get; set; }
        private float EndTime { get; set; }

        private RectangleShape RectangleShape { get; set; }

        public SwordProjectile(IProjectileOwner owner, float angle, float swingDuration) : base(owner, 0)
        {
            StartAngle = angle - 45;
            EndAngle = angle + 45;
            StartTime = Game.ElapsedTime;
            EndTime = Game.ElapsedTime + swingDuration;

            RectangleShape = new RectangleShape(new RectangleShape(new Vector2(50, 2)));
            RectangleShape.Origin = new Vector2f(-25, 1);
            RectangleShape.FillColor = Color.Blue;
        }

        public override void Update(float dTime)
        {
            Position = Owner.Position;

            float angleDifference = EndAngle - StartAngle;
            float timeDifference = EndTime - StartTime;
            Angle = angleDifference * ((Game.ElapsedTime - StartTime) / timeDifference) + StartAngle;
            

            if (EndTime < Game.ElapsedTime)
            {
                Owner.ProjectileDied(this);
                Game.Entities.Remove(this);
                return;
            }

            foreach (Vector2 hitpoint in HitPoints)
            {
                Vector2 tmpHitpoint = hitpoint;
                IEnumerable<Entity> entities = Game.Entities.Where(e => e != this 
                    && (e.Position - tmpHitpoint).Length < 20 
                    && !e.IsInvincible 
                    && Owner.ValidateProjectileHit(this, e)
                );

                foreach (Entity entity in entities.ToArray())
                {
                    entity.Hit(Owner as Entity, 1);
                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            RectangleShape.Position = Position;
            RectangleShape.Rotation = Angle;
            RectangleShape.Draw(target, states);
        }

        private IEnumerable<Vector2> HitPoints
        {
            get
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return Position + Vector2.FromAngle(Angle, -RectangleShape.Origin.X + RectangleShape.Size.X) * ((i + 1) / 5f);
                }  
            }
        }
    }
}