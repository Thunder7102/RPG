using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace SFMLTest.Entities
{
    public class SwordProjectile : Entity
    {
        public Entity Owner { get; set; }

        public float StartAngle { get; set; }
        public float EndAngle { get; set; }

        public float StartTime { get; set; }
        public float EndTime { get; set; }

        public RectangleShape RectangleShape { get; set; }

        public SwordProjectile(Entity owner, float angle, float swingDuration) : base(0, 0)
        {
            Owner = owner;
            Console.WriteLine("Angle: {0}", angle);
            StartAngle = angle - 45;
            EndAngle = angle + 45;
            StartTime = Game.ElapsedTime;
            EndTime = Game.ElapsedTime + swingDuration;

            RectangleShape = new RectangleShape(new RectangleShape(new Vector2(50, 2)));
            RectangleShape.Origin = new Vector2f(-25, 1);
            RectangleShape.FillColor = Color.Blue;
        }

        public override int RenderPriority
        {
            get { return 500; }
        }

        protected override float InvisibilityTime
        {
            get { return 0; }
        }

        public event Func<Entity, bool> ValidateHit;

        public override void Update(float dTime)
        {
            X = Owner.X;
            Y = Owner.Y;

            float angleDifference = EndAngle - StartAngle;
            float timeDifference = EndTime - StartTime;
            Angle = angleDifference * ((Game.ElapsedTime - StartTime) / timeDifference) + StartAngle;
            

            if (EndTime < Game.ElapsedTime)
            {
                Die(this);
            }

            foreach (Vector2 hitpoint in HitPoints)
            {
                IEnumerable<Entity> entities = Game.Entities.Where(e => e != this && (e.Position - hitpoint).Length < 20 && !e.IsInvincible);
                if (ValidateHit != null)
                {
                    entities = entities.Where(ValidateHit);
                }
                foreach (Entity entity in entities.ToArray())
                {
                    entity.Hit(Owner, 1);
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

        public override event DeathEvent Death;
        public override void Die(Entity entity)
        {
            if (Death != null)
            {
                Death(this, entity);
            }
            Game.Entities.Remove(this);
        }
    }
}