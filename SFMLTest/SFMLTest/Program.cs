using System.Diagnostics;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using SFMLTest.Entities;

namespace SFMLTest
{
    public static class Program
    {
        public static void Main()
        {
            VideoMode videoMode = VideoMode.DesktopMode;
            videoMode.Width = 800;
            videoMode.Height = 600;
            using (RenderWindow renderWindow = new RenderWindow(videoMode, "SFML Test"))
            {
                renderWindow.SetFramerateLimit(60);

                Game.Window = renderWindow;
                Game.Init();
                
                renderWindow.Closed += (sender, e) =>
                {
                    renderWindow.Close();
                };

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (renderWindow.IsOpen)
                {
                    renderWindow.DispatchEvents();

                    float dTime = (float)stopwatch.ElapsedMilliseconds / 1000;
                    stopwatch.Restart();
                    Game.ElapsedTime += dTime;

                    if (Game.Entities.OfType<Enemy>().Count() < 3)
                    {
                        Game.Entities.Add(new Enemy(Game.Player)
                        {
                            Position = Vector2.Random(renderWindow.Size.X, renderWindow.Size.Y)
                        });
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        renderWindow.Close();
                    }

                    // Update code goes here
                    for (int index = 0; index < Game.Entities.Count; index++)
                    {
                        Game.Entities[index].Update(dTime);
                    }

                    // Rendering code goes here
                    renderWindow.Clear();

                    foreach (Entity entity in Game.Entities.OrderBy(e => e.RenderPriority))
                    {
                        renderWindow.Draw(entity);
                    }
                    renderWindow.Display();
                }
                renderWindow.Close();
            }
        }
    }
}
