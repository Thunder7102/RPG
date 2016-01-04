using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LuaServer
{
    public class Server
    {
        private UdpClient _udpClient;
        private readonly List<Client> _clients = new List<Client>();
        private int _nextId = 1;
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
                        client = new Client(_nextId++, this, sender);

                        Broadcast(client, MessageType.Login, client.State.X, client.State.Y);
                        WriteTo(client, client, MessageType.Authenticated, client.State.X, client.State.Y);
                        foreach (Client otherClient in _clients)
                        {
                            WriteTo(client, otherClient, MessageType.Login, otherClient.State.ExpectedX, otherClient.State.ExpectedY, otherClient.State.DirectionX, otherClient.State.DirectionY);
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
            Console.WriteLine(message);
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            _udpClient.Send(bytes, bytes.Length, client.IPEndPoint);
        }

        public void Broadcast(Client sender, MessageType type, params object[] arguments)
        {
            foreach (Client c in _clients) WriteTo(c, sender, type, arguments);
        }
    }
}