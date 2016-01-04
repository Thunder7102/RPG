using System;
using System.Linq;
using System.Net;
using System.Text;

namespace LuaServer
{
    public class Client
    {
        private readonly Server _server;

        public Client(int id, Server server, IPEndPoint endPoint)
        {
            _server = server;
            IPEndPoint = endPoint;
            Buffer = "";
            ID = id;

            State = new ClientState();
        }

        public IPEndPoint IPEndPoint { get; set; }
        private string Buffer { get; set; }
        public int ID { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public ClientState State { get; set; }

        public void HandleData(byte[] data)
        {
            LastUpdateTime = DateTime.Now;
            Buffer += Encoding.ASCII.GetString(data);
            string[] split = Buffer.Split('\n');
            for (int i = 0; i < split.Length - 1; i++)
            {
                string[] parts = split[i].Split(' ');
                if (parts.Length >= 3 && parts[0] == "100")
                {
                    float x, y, dirX, dirY;
                    if (!float.TryParse(parts[1], out x) || !float.TryParse(parts[2], out y) ||
                        !float.TryParse(parts[3], out dirX) || !float.TryParse(parts[4], out dirY))
                    {
                        Console.WriteLine("Could not parse MOVE message: {0}", split[i]);
                        continue;
                    }
                    State.X = x;
                    State.Y = y;
                    State.DirectionX = dirX;
                    State.DirectionY = dirY;
                    State.MoveStart = DateTime.Now;
                    _server.Broadcast(this, MessageType.Move, x, y, dirX, dirY);
                }
            }
            Buffer = split[split.Length - 1];
        }

        public bool Is(IPEndPoint endpoint)
        {
            if (endpoint.Port != IPEndPoint.Port || endpoint.AddressFamily != IPEndPoint.AddressFamily) return false;

            byte[] ownBytes = IPEndPoint.Address.GetAddressBytes();
            byte[] otherBytes = endpoint.Address.GetAddressBytes();
            return ownBytes.SequenceEqual(otherBytes);
        }
    }
}