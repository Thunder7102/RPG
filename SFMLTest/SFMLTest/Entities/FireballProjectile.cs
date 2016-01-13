using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFMLTest.Helpers;

namespace SFMLTest.Entities
{
    public class FireballProjectile : Projectile
    {
        private readonly Vector2 _direction;
        private readonly List<RectangleShape> _centerShapes;
        private readonly List<Particle> _particles;
        private float _lastParticleTime;
        private List<Entity> entitiesHit = new List<Entity>(); 
        private const int maxEnemyHit = 3;

        public FireballProjectile(IProjectileOwner owner, Vector2 direction)
            : base(owner, 0)
        {
            _direction = direction;
            Position = owner.Position;

            _centerShapes = new List<RectangleShape>();
            _particles = new List<Particle>();
            for (int i = 0; i < 3; i++)
            {
                _centerShapes.Add(new RectangleShape
                {
                    Size = new Vector2(15, 15),
                    Origin = new Vector2(7.5f, 7.5f),
                    Rotation = RandomHelper.RandomFloat(360),
                    FillColor = Color.Red
                });
            }
        }

        public override int RenderPriority
        {
            get { return 700; }
        }

        protected override float InvisibilityTime
        {
            get { return 0; }
        }

        public override void Update(float dTime)
        {
            Position += _direction*dTime;
            const float rotateSpeed = 360;
            foreach (RectangleShape centerShape in _centerShapes)
            {
                centerShape.Rotation += dTime * rotateSpeed;
                centerShape.Position = Position;
            }
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Update(dTime);
                if (!_particles[index].Exists)
                {
                    _particles.RemoveAt(index);
                }
            }
            if (Game.ElapsedTime - _lastParticleTime > 0.1)
            {
                _lastParticleTime = Game.ElapsedTime;
                for (int i = 0; i < 5; i++)
                {
                    Color color = new Color(
                        255,
                        RandomHelper.RandomByte(),
                        0
                    );
                    _particles.Add(new Particle(
                        Position + Vector2.Random(15, 15) - new Vector2(7.5f, 7.5f),
                        Vector2.Random(15, 15) - new Vector2(7.5f, 7.5f),
                        5,
                        new Vector2(5, 5),
                        color
                        ));
                }
            }

            foreach (Entity entity in Game.Entities.Where(e => e != this
                    && (e.Position - Position).Length < 20
                    && !e.IsInvincible
                    && !entitiesHit.Contains(e)
                    && Owner.ValidateProjectileHit(this, e)).ToArray())
            {
                entity.Hit(this, 1);
                entitiesHit.Add(entity);
            }
            if (entitiesHit.Count > maxEnemyHit)
            {
                // TODO: Explode
                Owner.ProjectileDied(this);
                Game.Entities.Remove(this);
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            foreach (Particle particle in _particles)
            {
                particle.Draw(target, states);
            }
            foreach (RectangleShape shape in _centerShapes)
            {
                shape.Draw(target, states);
            }
        }
    }
}
