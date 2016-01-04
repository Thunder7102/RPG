using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LuaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }

        public class Server
        {
            private UdpClient _udpClient;
            private List<Client> _clients = new List<Client>();
            private int nextId = 1;
            public void Run()
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
                _udpClient = new UdpClient(ipep);

                Console.WriteLine("Waiting for a client...");

                while (true)
                {
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                    try
                    {
                        byte[] data = _udpClient.Receive(ref sender);

                        Client client = _clients.FirstOrDefault(c => c.Is(sender));
                        if (client == null)
                        {
                            byte[] bytes = sender.Address.GetAddressBytes();
                            Console.WriteLine("New client connected: {0}.{1}.{2}.{3}:{4}", bytes[0], bytes[1], bytes[2], bytes[3], sender.Port);
                            client = new Client(nextId++, this, sender);
                            Broadcast(client, MessageType.Login);
                            foreach (Client otherClient in _clients)
                            {
                                WriteTo(client, otherClient, MessageType.Login);
                            }
                            _clients.Add(client);
                        }

                        client.HandleData(data);
                        RemoveInactivePlayers();
                    }
                    catch
                    {
                    }
                }
            }

            private void RemoveInactivePlayers()
            {
                List<Client> clientsToRemove = _clients.Where(c => c.LastUpdateTime < DateTime.Now.AddSeconds(-10)).ToList();
                foreach (Client c in clientsToRemove)
                {
                    Console.WriteLine("Client {0} timed out", c.ID);
                    _clients.Remove(c);
                    Broadcast(c, MessageType.Logout);
                }
            }

            private void WriteTo(Client client, Client sender, MessageType type, params object[] arguments)
            {
                string message = string.Format("{0} {1} {2}", sender.ID, (int)type, string.Join(" ", arguments)).Trim() + "\n";
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                _udpClient.Send(bytes, bytes.Length, client.IPEndPoint);
            }

            public void Broadcast(Client sender, MessageType type, params object[] arguments)
            {
                foreach (Client c in _clients) WriteTo(c, sender, type, arguments);
            }
        }

        public enum MessageType
        {
            Login = 1,
            Logout = 2,

            Move = 100
        }

        public class Client
        {
            private readonly Server _server;

            public Client(int id, Server server, IPEndPoint endPoint)
            {
                _server = server;
                IPEndPoint = endPoint;
                Buffer = "";
                ID = id;
            }

            public IPEndPoint IPEndPoint { get; set; }
            private string Buffer { get; set; }
            public int ID { get; set; }
            public DateTime LastUpdateTime { get; set; }

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
                        string x = parts[1];
                        string y = parts[2];
                        _server.Broadcast(this, MessageType.Move, x, y);
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
}
