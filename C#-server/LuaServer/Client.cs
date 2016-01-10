using System;
using System.Collections.Generic;
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

        public IPEndPoint IPEndPoint { get; private set; }
        private string Buffer { get; set; }
        public int ID { get; private set; }
        public DateTime LastUpdateTime { get; private set; }

        public ClientState State { get; private set; }

        /// <exception cref="ArgumentNullException">enumType or value is null. </exception>
        /// <exception cref="ArgumentException">enumType is not an Enum.-or- The type of value is an enumeration, but it is not an enumeration of type enumType.-or- The type of value is not an underlying type of enumType. </exception>
        /// <exception cref="InvalidOperationException">value is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />, or <see cref="T:System.String" />. </exception>
        /// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        public void HandleData(byte[] data)
        {
            LastUpdateTime = DateTime.Now;
            Buffer += Encoding.ASCII.GetString(data);
            string[] split = Buffer.Split('\n');
            for (int i = 0; i < split.Length - 1; i++)
            {
                string[] parts = split[i].Split(' ');
                int typeID;
                if (!int.TryParse(parts[0], out typeID) || !Enum.IsDefined(typeof (MessageType), typeID))
                {
                    continue;
                }

                MessageType type = (MessageType) typeID;
                switch (type)
                {
                    case MessageType.Authenticated:
                        break;
                    case MessageType.Login:
                        break;
                    case MessageType.Logout:
                        break;
                    case MessageType.Ping:
                        _server.Write(this, MessageType.Ping);
                        break;
                    case MessageType.Move:
                        HandleMove(parts);
                        break;
                    case MessageType.Chat:
                        HandleChat(parts);
                        break;
                }
            }
            Buffer = split[split.Length - 1];
        }

        private void HandleChat(IEnumerable<string> parts)
        {
            string message = string.Join(" ", parts.Skip(1));
            _server.Broadcast(this, MessageType.Chat, message);
        }

        private void HandleMove(string[] parts)
        {
            float x, y, dirX, dirY;
            if (!float.TryParse(parts[1], out x) || !float.TryParse(parts[2], out y) || !float.TryParse(parts[3], out dirX) || !float.TryParse(parts[4], out dirY))
            {
                Console.WriteLine("Could not parse MOVE message: {0}", string.Join(" ", parts));
                return;
            }
            State.X = x;
            State.Y = y;
            State.DirectionX = dirX;
            State.DirectionY = dirY;
            State.MoveStart = DateTime.Now;
            _server.Broadcast(this, MessageType.Move, x, y, dirX, dirY);
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