using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTest.Entities;

namespace SFMLTest
{
    public static class Game
    {
        public static List<Entity> Entities { get; private set; }
        public static Player Player { get; private set; }
        public static float ElapsedTime { get; set; }
        public static RenderWindow Window { private get; set; }

        public static Vector2 MousePosition
        {
            get
            {
                Vector2i position = Mouse.GetPosition(Window);
                return new Vector2(position.X, position.Y);
            }
        }

        public static void Init()
        {
            Player = new Player();
            Entities = new List<Entity>();
            Entities.Add(Player);
        }
    }
}