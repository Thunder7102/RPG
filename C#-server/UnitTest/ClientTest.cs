using System;
using System.Diagnostics;
using System.Threading;
using LuaServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void ExpectedPositionTest()
        {
            ClientState clientState = new ClientState
            {
                X = 50,
                Y = 50,
                DirectionX = 1,
                DirectionY = 1,
                MoveStart = DateTime.Now
            };

            Thread.Sleep(1000);
            Console.WriteLine("Difference in X: {0}", Math.Abs(clientState.ExpectedX - clientState.X - clientState.Speed));
            Console.WriteLine("Difference in Y: {0}", Math.Abs(clientState.ExpectedY - clientState.Y - clientState.Speed));

            Debug.Assert(clientState.ExpectedX > clientState.X);
            Debug.Assert(clientState.ExpectedY > clientState.Y);
            Debug.Assert(Math.Abs(clientState.ExpectedX - clientState.X - clientState.Speed) < clientState.Speed / 100);
            Debug.Assert(Math.Abs(clientState.ExpectedY - clientState.Y - clientState.Speed) < clientState.Speed / 100);
        }
    }
}
