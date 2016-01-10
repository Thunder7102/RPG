using System;

namespace LuaServer
{
    public class ClientState
    {
        public DateTime MoveStart { private get; set; }
        public float DirectionY { get; set; }
        public float DirectionX { get; set; }
        public float Y { get; set; }
        public float X { get; set; }
        public float Speed { get; private set; }

        public ClientState()
        {
            Speed = 100;
        }

        public float ExpectedX { get { return (float) (X + (DirectionX*(DateTime.Now - MoveStart).TotalSeconds*Speed)); } }
        public float ExpectedY { get { return (float) (Y + (DirectionY*(DateTime.Now - MoveStart).TotalSeconds*Speed)); } }
    }
}