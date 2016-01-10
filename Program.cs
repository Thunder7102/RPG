using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLTest
{
    public static class Program
    {
        public static void Main()
        {
            using (RenderWindow renderWindow = new RenderWindow(VideoMode.DesktopMode, "SFML Test"))
            {
                renderWindow.SetFramerateLimit(60);

                renderWindow.Closed += (sender, e) =>
                {
                    renderWindow.Close();
                };

                renderWindow.KeyPressed += (sender, e) =>
                {
                    // Key press events go here
                };

                renderWindow.KeyReleased += (sender, e) =>
                {
                    // Key release events go here
                };

                renderWindow.MouseMoved += (sender, e) =>
                {
                    // Mouse move events
                };

                renderWindow.MouseButtonPressed += (sender, e) =>
                {
                    // Mouse button pressed
                };

                RectangleShape rectangle = new RectangleShape();
                rectangle.FillColor = Color.Red;
                rectangle.Position = new Vector2f(50, 50);
                rectangle.Size = new Vector2f(50, 50);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (renderWindow.IsOpen)
                {
                    renderWindow.DispatchEvents();

                    double dTime = (double)stopwatch.ElapsedMilliseconds / 1000;
                    stopwatch.Restart();

                    // Update code goes here

                    if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        rectangle.Position += new Vector2f((float) (100 * dTime), 0);
                    }

                    // Rendering code goes here
                    renderWindow.Clear();

                    renderWindow.Draw(rectangle);

                    renderWindow.Display();
                }
            }
        }
    }
}
