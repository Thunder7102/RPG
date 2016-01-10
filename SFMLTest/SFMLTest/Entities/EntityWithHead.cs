using SFML.Graphics;
using SFML.System;

namespace SFMLTest.Entities
{
    public abstract class EntityWithHead : Entity
    {
        private readonly RectangleShape _bodyShape;
        private readonly RectangleShape _headShape;

        private Color _color;
        private Vector2 _currentHeadPosition, _targetHeadPosition;

        protected EntityWithHead(int health, float speed, Color color) : base(health, speed)
        {
            _color = color;
            _bodyShape = new RectangleShape(new Vector2f(20, 20))
            {
                Origin = new Vector2f(10, 10)
            };
            _headShape = new RectangleShape(new Vector2f(10, 10))
            {
                Origin = new Vector2f(5f, 10f)
            };
        }

        protected abstract Vector2 GetDirection();

        public override void Update(float dTime)
        {
            const int headMoveSpeed = 50;
            _targetHeadPosition = GetDirection()*5;

            if (_targetHeadPosition.X > _currentHeadPosition.X)
            {
                _currentHeadPosition.X += dTime*headMoveSpeed;
            }
            else if (_targetHeadPosition.X < _currentHeadPosition.X)
            {
                _currentHeadPosition.X -= dTime*headMoveSpeed;
            }
            if (_targetHeadPosition.Y > _currentHeadPosition.Y)
            {
                _currentHeadPosition.Y += dTime*headMoveSpeed;
            }
            else if (_targetHeadPosition.Y < _currentHeadPosition.Y)
            {
                _currentHeadPosition.Y -= dTime*headMoveSpeed;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            byte alphaColor = (byte) (255 - 255*Opacity);

            _bodyShape.Position = Position;
            _bodyShape.FillColor = new Color(_color.R, _color.G, _color.B, alphaColor);

            _headShape.Position = Position + new Vector2(0, -15) + _currentHeadPosition;
            _headShape.FillColor = new Color(_color.R, _color.G, _color.B, alphaColor);

            _bodyShape.Draw(target, states);
            _headShape.Draw(target, states);
        }
    }
}