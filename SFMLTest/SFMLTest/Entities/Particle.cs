using SFML.Graphics;
using SFMLTest.Helpers;

namespace SFMLTest.Entities
{
    public class Particle : Drawable
    {
        private readonly Vector2 _position;
        private readonly Vector2 _direction;
        private readonly float _startTime;
        private readonly float _decayTime;
        private readonly RectangleShape _shape;

        public Particle(Vector2 position, Vector2 direction, float decayTime, Vector2 size, Color color)
        {
            _position = position;
            _direction = direction;
            _decayTime = decayTime;
            _startTime = Game.ElapsedTime;

            _shape = new RectangleShape
            {
                Size = size,
                Rotation = RandomHelper.RandomFloat(360),
                FillColor = color,
                Origin = size / 2
            };
        }

        public bool Exists { get { return _startTime + _decayTime > Game.ElapsedTime; } }

        public void Update(float dTime)
        {
            float alphaPerSecond = 255/_decayTime;
            float timeElapsed = Game.ElapsedTime - _startTime;
            byte alpha = (byte) (255 - alphaPerSecond*timeElapsed);

            _shape.FillColor = new Color(
                _shape.FillColor.R,
                _shape.FillColor.G,
                _shape.FillColor.B,
                alpha
            );
            _shape.Position = _position + _direction * (Game.ElapsedTime - _startTime);
            _shape.Rotation += dTime*360;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _shape.Draw(target, states);
        }
    }
}