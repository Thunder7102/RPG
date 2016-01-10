using SFML.Window;

namespace SFMLTest
{
    public class PressListener
    {
        private readonly Keyboard.Key _key;
        private bool _wasDown;

        public PressListener(Keyboard.Key key)
        {
            _key = key;
        }

        public bool IsPressed
        {
            get
            {
                bool isDown = Keyboard.IsKeyPressed(_key);
                bool result = !_wasDown && isDown;
                _wasDown = isDown;
                return result;
            }
        }
    }
}